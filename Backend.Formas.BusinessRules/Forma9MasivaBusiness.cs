
using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.BusinessRules.ViewSendMail;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.ModelsAdm;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Entities.Services;
using Backend.Formas.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Backend.Formas.BusinessRules
{
    public class Forma9MasivaBusiness : IFormas9MasivaBusiness
    {
        private const int StartIndex = 16;
        private readonly IForma9MasivaRepository _repository;
        private readonly IAdministracionFormas _repositoryAdministracion;
        private readonly List<string> ErrorsList = new List<string>();
        private readonly IImportarExcel Excel;
        private readonly ExcelError ExcelError = new ExcelError();
        private readonly Forma9MasivaStructureDTO Forma9Respose = new Forma9MasivaStructureDTO();
        private readonly HeaderForma9MasivaDTO Header = new HeaderForma9MasivaDTO();
        private readonly List<BodyContenteDTOMasiva> content = new List<BodyContenteDTOMasiva>();
        private BodyContenteDTOMasiva bodyForma = new BodyContenteDTOMasiva();
        private readonly IFormaValidate RepositoryValidate;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;


        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly JsonF9Masiva JsonF9Masiva = new JsonF9Masiva();
        private readonly DetalleJsonF9Masiva detJsonF9Masiva = new DetalleJsonF9Masiva();

        private readonly ProductionDetailForma9MasivaDTO jsondetalleDorma9 = new ProductionDetailForma9MasivaDTO();

        private string _fileName = "";


        private readonly List<DetalleJsonF9Masiva> _RespBDPJson = new List<DetalleJsonF9Masiva>();

        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");

        readonly List<CeldasExcel> Celdas_Excel = new List<CeldasExcel>();

        public Forma9MasivaBusiness(
            IForma9MasivaRepository _repo,
            IAdministracionFormas admin,
            IFormaValidate _formaValidate,
            Utilities.Telemetry.ITelemetryException telemetryException,
            IImportarExcel excel,
            Administracion.AdmGrpc.AdmGrpcClient administracionService,
            Ppdm.PpdmGrpc.PpdmGrpcClient formasService,
            Commons.CommonGrpc.CommonGrpcClient commonService,
            Backend.Formas.Utilities.SendMail.ISendMailService sendMailService
            )
        {
            Excel = excel;
            _repository = _repo;
            _repositoryAdministracion = admin;
            RepositoryValidate = _formaValidate;

            ExcelError.state = false;
            ExcelError.cargaExtemporal = false;

            TelemetryException = telemetryException;
            AdministracionService = administracionService;
            FormasService = formasService;
            CommonService = commonService;
            SendMailService = sendMailService;
        }

        public async Task<ResponseBase<Forma9MasivaStructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId)
        {
            var response = new ResponseBase<Forma9MasivaStructureDTO>();
            Excel.setFile(files);

            //CultureInfo cultureInfo = new CultureInfo("es-co");
            ///Excel.
            Excel.reader("FORMA 9");
            if (Excel.isLoad())
            {
                _fileName = fileName;
                // var worksheets = await Excel.getWorksheets();

                var rows = Excel.Rows();
                if (rows <= 1)
                {
                    ExcelError.state = true;
                    ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido en la Hoja [FORMA 9]";
                    Forma9Respose.Errors = ExcelError;
                    Forma9Respose.Data = content; // DetalleJsonC4Masiva;
                }
                else if (rows > 0)
                {
                    Console.Clear();
                    //validamos datos de cabecera
                    Console.WriteLine("Inicia Operacion : {0}", DateTime.Now);
                    await HeaderForma9Masiva(date, getUserId);
                    Console.WriteLine("Fin Encabezado Operacion : {0}", DateTime.Now);
                    var detalle = await BodyContentForma9Masiva();
                    Console.WriteLine("Fin Body Operacion : {0}", DateTime.Now);
                    bodyForma = detalle;
                    ValidaContexto(fileName, files, getUserId);
                    Console.WriteLine("Fin Validacion Excel : {0}", DateTime.Now);
                    detalle.Header = Header;
                    detalle.RespBDP = _RespBDPJson;
                    detalle.Workbook = "Forma 9";
                    content.Add(detalle);

                    if (ExcelError.state == false)
                    {
                        Forma9Respose.Data = content;
                        Forma9Respose.Errors = ExcelError;
                    }

                    if (ExcelError.cargaExtemporal == true)
                    {
                        ExcelError.message = "La forma que intenta cargar se encuentra en un mes operativo anterior o esta forma ya ha sido aprobada";
                        ExcelError.header = Header;
                        Forma9Respose.Data = content;
                        Forma9Respose.Errors = ExcelError;
                        ExcelError.state = true;
                        //ErrorsList.Add(error);
                    }

                    if (ExcelError.state == true && ExcelError.cargaExtemporal == false)
                    {
                        ExcelError.message = "El archivo cargado tiene validaciones pendientes, verifique e intente de nuevo";
                        Forma9Respose.Data = content;
                        Forma9Respose.Errors = ExcelError;
                        //ExcelError.state = false;
                    }
                }
                else
                {
                    ExcelError.state = true;
                    ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido de la Hoja [FORMA 9] e intente de nuevo";
                    Forma9Respose.Errors = ExcelError;
                    Forma9Respose.Data = content;
                }
            }
            else
            {
                ExcelError.state = true;
                ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido de la Hoja [FORMA 9]";
                Forma9Respose.Errors = ExcelError;
                Forma9Respose.Data = content;
            }
            ExcelError.listError = ErrorsList;

            response.Data = Forma9Respose;

            return response;
        }

        public async Task<ResponseBase<dynamic>> Create(RequestCreateForma9Masiva data, DateTime date, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();

            try
            {
                var header = data.Header9Masiva;
                var body = data.Detail9Masiva;
                var total = data.Totalacumulados;
                CultureInfo cultureInfo = new CultureInfo("es-co");
                var form = new Form9Masiva
                {
                    //***************  MCG 2/15/2022  Se uita encabezado para estas cargacontinua
                    // Block = !string.IsNullOrEmpty(header.Bloque) ? header.Bloque : "",
                    // Oilfield = !string.IsNullOrEmpty(header.Yacimiento) ? header.Yacimiento : "",
                    // Structure = !string.IsNullOrEmpty(header.CierreEstructural) ? header.CierreEstructural : "",
                    // Member = !string.IsNullOrEmpty(header.Miembro) ? header.Miembro : "",

                    Form9id = Guid.NewGuid(),

                    // operadorId = !string.IsNullOrEmpty(header.OperadorId) ? header.OperadorId : "",
                    // campo = header.Campo ?? "",
                    // campoId = !string.IsNullOrEmpty(header.CampoId) ? header.CampoId : "",
                    // bloque = header.Bloque ?? "",
                    // bloqueId = !string.IsNullOrEmpty(header.BloqueId) ? header.BloqueId : "",
                    // formacion = header.Formacion ?? "",
                    // formacionId = !string.IsNullOrEmpty(header.FormacionId) ? header.FormacionId : "",
                    // formacionSetId = !string.IsNullOrEmpty(header.FormacionSetId) ? header.FormacionSetId : "",
                    // yacimiento = header.Yacimiento ?? "",
                    // yacimientoId = !string.IsNullOrEmpty(header.YacimientoId) ? header.YacimientoId : "",
                    // contratoId = !string.IsNullOrEmpty(header.ContratoId) ? header.ContratoId : "",
                    // **************** MCG 2/15/2022  Se uita encabezado para estas cargacontinua
                    row_created_by = GetUserId,
                    row_created_date = DateTime.Now,
                    row_changed_by = GetUserId,
                    row_changed_date = DateTime.Now
                };

                try
                {
                    await _repository.CreateForm9(form);
                }
                catch (Exception e)
                {
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                    TelemetryException.RegisterException(e);
                    return response;
                }

                var forStateid = await _repositoryAdministracion.GetState("En Proceso de Aprobación");
                var concrete = new Concreteform();
                decimal campo = 0;
                try
                {
                    string dateFormaString = $"{header.Mes}/{header.Ano}";
                    DateTime dateForm = DateTime.Parse(dateFormaString, cultureInfo);
                    // **************** MCG 2/15/2022  Se uita encabezado para estas cargacontinua
                    //campo = decimal.Parse(header.CampoId);

                    concrete = new Concreteform
                    {
                        Concreteformid = Guid.NewGuid(),
                        Maincampid = 0,
                        // **************** MCG 2/15/2022  Se uita encabezado para estas cargacontinua
                        //Company = !string.IsNullOrEmpty(header.Operador) ? header.Operador : "",
                        //Contract = !string.IsNullOrEmpty(header.Contrato) ? header.Contrato : "",
                        Battery = "",
                        Tank = "",
                        Month = dateForm.Month,
                        Year = dateForm.Year,
                        // **************** MCG 2/15/2022  Se uita encabezado para estas cargacontinua
                        //Explotationmodality = !string.IsNullOrEmpty(header.ModalidadExplotacion)? header.ModalidadExplotacion : "",
                        Annotations = "",
                        Version = 1,
                        Currentstate = forStateid,
                        Generationflag = 0,
                        Campid = campo,
                        // **************** MCG 2/15/2022  Se uita encabezado para estas cargacontinua
                        // Pdenid = header.OperadorId,
                        // **************** MCG 2/15/2022  Se uita encabezado para estas cargacontinua

                        Formid = form.Form9id,
                        Generationjobid = 0,
                        Iqistatus = 0,
                        Usersigning = header.UsuarioAprobador,
                        Minrepsigning = header.UsuarioMin,
                        Formname = "Forma 9"
                    };
                }
                catch (Exception e)
                {
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                    TelemetryException.RegisterException(e);
                    return response;
                }


                var form9 = await _repository.CreateConcreteform(concrete);
                List<Form9Masivadetail> form9DetailList = new List<Form9Masivadetail>();
                List<Acumulados> Acumulados = new List<Acumulados>();

                for (var row = 0; row <= (body.Count() - 1); row++)
                {
                    try
                    {
                        var item = body[row];
                        var form9Detail = new Form9Masivadetail
                        {
                            Oilwell = item.Pozo,
                            // **************** MCG 2/15/2022  Se quita encabezado para esta carga continua
                            // Formation = header.Formacion,
                            Formation = null,
                            Danecode = item.Municipio,
                            Productionmethod = item.MetProducion,
                            Monthdays = decimal.Parse(item.Mes_dia),
                            Accumulatedays = decimal.Parse(item.Acumulado_dia),
                            Dailyoilproduction = decimal.Parse(item.Diario_crudo),
                            Monthlyoilproduction = decimal.Parse(item.Mensual_crudo),
                            Accumulateoilproduction = decimal.Parse(item.Acumuado_crudo),
                            Correctionfactor = decimal.Parse(item.FactorCorrecion),
                            Dailywaterproduction = decimal.Parse(item.Diario_agua),
                            Monthlywaterproduction = decimal.Parse(item.Mensual_agua),
                            Accumulatewaterproduction = decimal.Parse(item.Acumuado_agua),
                            Dailygasproduction = decimal.Parse(item.Diario_gas),
                            Montlhygasproduction = decimal.Parse(item.Mensual_gas),
                            Accumulategasproduction = decimal.Parse(item.Acumuado_gas),
                            Bsw = decimal.Parse(item.Bsw),
                            Apigrades = decimal.Parse(item.Api),
                            Rgp = decimal.Parse(item.Rgp),
                            Oilwellfinalstate = item.Estado ?? "",
                            Formid = form.Form9id,
                            Poolname = "",
                            PdenId = item.PdenId,
                            Row_created_by = GetUserId,
                            Row_created_date = DateTime.Now,
                            Row_changed_by = GetUserId,
                            Row_changed_date = DateTime.Now,
                            Form9detailid = Guid.NewGuid(),
                            Eliminar = item.Eliminar,
                        };

                        Acumulados itemAcumulado = new Acumulados()
                        {
                            Id_Forma = form.Form9id,
                            AguaAcumulado = !string.IsNullOrEmpty(item.AGUA) ? float.Parse(item.AGUA) : 0,
                            CrudoAcumulado = !string.IsNullOrEmpty(item.CRUDO) ? float.Parse(item.CRUDO) : 0,
                            GasAcumulado = !string.IsNullOrEmpty(item.GAS) ? float.Parse(item.GAS) : 0
                        };

                        Acumulados.Add(itemAcumulado);
                        form9DetailList.Add(form9Detail);
                    }
                    catch (Exception e)
                    {
                        response.Code = (int)HttpStatusCode.BadRequest;
                        response.Message = Messages.ErrorCreation;
                        TelemetryException.RegisterException(e);
                        return response;
                    }
                }

                var totalRegister = new Form9Masivatotalvolumedetail()
                {
                    Form9tvdetailid = Guid.NewGuid(),
                    Volumetype = total.Volumetype,
                    Accumulategasproduction = total.Accumulategasproduction,
                    Montlhygasproduction = total.Montlhygasproduction,
                    Dailygasproduction = total.Dailygasproduction,
                    Accumulatewaterproduction = total.Accumulatewaterproduction,
                    Dailywaterproduction = total.Dailywaterproduction,
                    Monthlywaterproduction = total.Monthlywaterproduction,
                    Accumulateoilproduction = total.Accumulateoilproduction,
                    Dailyoilproduction = total.Dailyoilproduction,
                    Monthlyoilproduction = total.Monthlyoilproduction,
                    Bsw = total.Bsw,
                    Apigrades = total.Apigrades,
                    Rgp = total.Rgp,
                    Formid = form.Form9id,
                    // **************** MCG 2/15/2022  Se uita encabezado para estas cargacontinua
                    // Formation = header.Formacion,
                    Formation = null,
                    Row_created_by = GetUserId,
                    Row_created_date = DateTime.Now,
                    Row_changed_by = GetUserId,
                    Row_changed_date = DateTime.Now
                };

                try
                {
                    await _repository.CreateForm9Detail(form9DetailList);

                    await _repository.CreateForm9TotalVolumenDetail(totalRegister);
                    await _repository.CreateAcumulados(Acumulados);
                }
                catch (Exception e)
                {
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                    TelemetryException.RegisterException(e);
                    return response;
                }


                /*
                 *  Ejecucion de micro servicio de BDP con el Json Formado para validacion de datos 
                 *  Mauricio
                 * 
                */

                try
                {
                    string fecha = $"{header.Ano}/{header.Mes}";
                    DateTime dateForma = DateTime.Parse(fecha, cultureInfo);
                    var datauser = await AdministracionService.UsuarioAprobadorAsync(new Administracion.RequestAprobaciones { NombreForma = "Forma 9" });
                    var dataService = datauser.Data.Deserialize<UsuariosRecursosAprobaciones>();
                    var codeUser = dataService.CodigoUsuario;
                    // var dateTime = DateTime.Parse(date, cultureInfo);
                    var _aprobacion = new Aprobacioncarga
                    {
                        IdForma = form9.Formid,
                        FechaForma = dateForma,
                        Usuario = GetUserId,
                        FechaCarga = DateTime.Now,
                        Estado = forStateid,
                        FechaActualizacion = DateTime.Now,
                        UsuarioAprobador = dataService.CodigoUsuario,
                        ComparativoAgua = 0,
                        ComparativoCrudo = 0,
                        ComparativoGas = 0,
                        UsuarioNombre = userName,
                        UsuarioNombreAprobador = dataService.NombreUsuario,
                        FormaName = "Forma 9",
                        UrlForma = $"Forma9/{header.Ano}/{header.Mes}/{header.FileName}",
                        // **************** MCG 2/15/2022  Se ajusta encabezado para esta carga continua
                        // Campo = header.Campo,
                        // Contrato = header.Contrato,
                        // Operadora = header.Operador
                        Campo = null,
                        Contrato = null,
                        Operadora = null

                    };

                    await _repositoryAdministracion.CreteAprobacion(_aprobacion);

                    try
                    {
                        var moveFile = await CommonService.MoveItemAsync(new Commons.FileSystemMoveItemOptions
                        {
                            Container = "formas",
                            DestinationDirectory = new Commons.FileSystemItemInfo
                            {
                                Path = $"Forma9/{header.Ano}/{header.Mes}/{header.FileName}"
                            },
                            Item = new Commons.FileSystemItemInfo
                            {
                                Path = $"tmp/{header.FileName}"
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        TelemetryException.RegisterException(e);
                    }

                    try
                    {
                        string asunto = "Notificación de carga Forma 9";
                        MailForma9 mailForma = new MailForma9();
                        // **************** MCG 2/15/2022  Se quita encabezado para esta carga continua
                        // string view = mailForma.GetView(asunto, dataService.NombreUsuario, header.Operador, header.Contrato, header.Campo, fecha,
                        string view = mailForma.GetView(asunto, dataService.NombreUsuario, null, null, null, fecha,
                            "Se ha detectado la carga de una forma  relacionada a la Forma 9SH en estado de <b>En Proceso de Aprobación</b> ");
                        EmailInfo Email = new EmailInfo()
                        {
                            To = new List<string>() { dataService.Correo },
                            Subject = asunto,
                            Body = view,
                            Styles = mailForma.GetHeaderStyle()
                        };

                        await SendMailService.SendEmailAsync(Email);
                    }
                    catch (Exception e)
                    {
                        TelemetryException.RegisterException(e);
                    }

                }
                catch (Exception e)
                {
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                    TelemetryException.RegisterException(e);
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = !string.IsNullOrEmpty(ex.InnerException.Message)
                    ? ex.InnerException.Message
                    : Messages.ServerError;
            }

            return response;
        }

        private string ModalidaDeExplotacion()
        {
            var modalidadDeExplotacion = Excel.getCell(10, 3);
            if (!string.IsNullOrEmpty(modalidadDeExplotacion) && modalidadDeExplotacion.ToUpper() == "X")
            {
                modalidadDeExplotacion = "Pruebas Iniciales";
            }

            if (string.IsNullOrEmpty(modalidadDeExplotacion))
            {
                modalidadDeExplotacion = Excel.getCell(10, 8);
            }

            if (!string.IsNullOrEmpty(modalidadDeExplotacion) && modalidadDeExplotacion.ToUpper() == "X")
            {
                modalidadDeExplotacion = "Pruebas extensas";
            }

            if (string.IsNullOrEmpty(modalidadDeExplotacion))
            {
                modalidadDeExplotacion = Excel.getCell(10, 11);
            }

            if (!string.IsNullOrEmpty(modalidadDeExplotacion) && modalidadDeExplotacion.ToUpper() == "X")
            {
                modalidadDeExplotacion = "Sólo Riesgo";
            }

            if (string.IsNullOrEmpty(modalidadDeExplotacion))
            {
                modalidadDeExplotacion = Excel.getCell(10, 15);
            }

            if (!string.IsNullOrEmpty(modalidadDeExplotacion) && modalidadDeExplotacion.ToUpper() == "X")
            {
                modalidadDeExplotacion = "Comercial ";
            }
            return modalidadDeExplotacion ?? "";
        }

        //private async Task HeaderForma9Masiva(DateTime dateForma, string getUserId)
        private Task HeaderForma9Masiva(DateTime dateForma, string getUserId)
        {


            try
            {
                DateTime FechaVolumen = dateForma;
                //new DateTime(dateForma.Year, dateForma.Month, 1).AddMonths(1).AddDays(-1);

                // new DateTime(dateForma.Year, dateForma.Month, 1);

                // Periodo de carga 
                var iAnio = FechaVolumen.Year.ToString();

                var iMes = FechaVolumen.Month.ToString();
                //Console.WriteLine(iAnio.ToString() + " -> mes " +iMes.ToString());
                int iFila = Excel.Rows();
                int iCol = 50;
                var hashPden = new HashSet<string>();
                string cMens = "";
                string cValor = string.Empty;
                //Boolean lTitulos = true;
                /*
                 * Validacion inicial del archivo excel 
                 */
                string[] TitulosHoja = { "", "GERENCIA", "ANO", "MES", "UWI", "PDEN_ID",
                                     "CAMPO_ID", "CAMPO", "AREA", "POZO",
                                     "ZONA", "MUNICIPIO", "METODOPROD", "DIASMES",
                                     "DIASACUM", "PETROLEODIARIO", "PETROLEOMENSUAL",
                                     "PETROLEOACUM", "FACTORCORRECCION", "AGUADIARIO",
                                     "AGUAMENSUAL", "AGUAACUM", "GASDIARIO", "GASMENSUAL",
                                     "GASACUM", "BSW", "API", "RGA", "ESTADOPOZO", "OPERADOR",
                                     "NOMBRE_ZONA", "ELIMINAR" };

                for (var nCol = 1; nCol < iCol; nCol++)
                {
                    cValor = Excel.getCell(1, nCol);
                    if (!string.IsNullOrEmpty(cValor))
                    {
                        CeldasExcel itemCeldasExcel = new CeldasExcel()
                        {
                            Titulos = Excel.getCell(1, nCol)
                        };
                        Celdas_Excel.Add(itemCeldasExcel);
                        if (cValor != TitulosHoja[nCol])
                        {
                            string cTitulErr = $"En la columna " + nCol.ToString() + " => " + cValor + " No corresponde el nombre de la columna, " + TitulosHoja[nCol];
                            ErrorsList.Add(cTitulErr);
                            //lTitulos = true;
                        }
                    }
                }
                //cValor.toLocaleString
                for (var iCon = 2; iCon <= iFila; iCon++)
                {
                    dynamic colAnio = Excel.getCell(iCon, 2);  // Carga informacion de la columna de Anio
                    dynamic colMes = Excel.getCell(iCon, 3);  // Carga informacion de la columna de Mes
                    string colPden = Excel.getCell(iCon, 5);  // Carga informacion de la columna de Pden_id 

                    /* Validacion del año vs periodo de carga*/
                    if (string.IsNullOrEmpty(colAnio))
                    {
                        cMens += "En la fila " + iCon.ToString() + "  no se indicó el año,";
                        ErrorsList.Add("En la fila " + iCon.ToString() + "  no se indicó el año, ");
                        ExcelError.state = true;
                    }
                    else
                    {
                        if (colAnio != iAnio)
                        {
                            cMens += "En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado en el archivo excel, ";
                            ErrorsList.Add("En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado  en el archivo excel, ");
                        }
                    }
                    /* Validacion del mes vs periodo de carga*/
                    if (string.IsNullOrEmpty(colMes))
                    {
                        cMens += "En la fila " + iCon.ToString() + " El mes esta en blanco, ";
                        ErrorsList.Add("En la fila " + iCon.ToString() + " El mes esta en blanco, ");
                    }
                    else
                    {
                        if (colMes != iMes)
                        {
                            cMens += "En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ";
                            ErrorsList.Add("En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ");
                        }

                    }

                    /* Validacion del PDEN_ID no esta en blanco y nulo*/
                    if (string.IsNullOrEmpty(colPden))
                    {
                        cMens += "En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ";
                        ErrorsList.Add("En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ");
                    }
                    else
                    {
                        colPden = colPden.Trim();
                        if (!hashPden.Add(colPden))
                        {
                            cMens += "En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, ";
                            ErrorsList.Add("En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, ");
                        }
                    }

                }
                JsonF9Masiva.ANIO = iAnio.ToString();
                JsonF9Masiva.MES = iMes.ToString();
                JsonF9Masiva.FORMA_CODIGO = "9M";
                JsonF9Masiva.MIEMBRO_ID = getUserId;

                //  var list = await RepositoryValidate.ValidaContrato(bloque);
                //      string operador_id = ConvertTypes.ConverDataDynamic(list, "OPERADOR_ID");
                if (FechaVolumen < dateForma)
                {
                    ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
                    ExcelError.state = true;
                }
                if (FechaVolumen > dateForma)
                {
                    ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
                    ExcelError.state = true;
                }
                DateTime mesActual = DateTime.Now;
                DateTime mesOpertivo = new DateTime(mesActual.Year, mesActual.Month, 1).AddDays(-1);
                if (FechaVolumen > mesOpertivo)
                {
                    ErrorsList.Add("La fecha de la forma es superior a la fecha operativa.");
                    ExcelError.cargaExtemporal = false;
                    ExcelError.state = true;
                }
            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
                ErrorsList.Add("La fecha de la forma no es valida corrija e intente de nuevo");
            }

            if (ErrorsList.Count == 0 || ExcelError.cargaExtemporal == true)
            {
                Header.Mes = JsonF9Masiva.MES;
                Header.Ano = JsonF9Masiva.ANIO;
            }
            else
            {
                if (ErrorsList.Count > 0)
                {
                    ExcelError.state = true;
                    ExcelError.message =
                        "El archivo cargado tiene mensajes de validación de los datos, verifique he intente de nuevo";
                    // ExcelError.listError = ErrorsList;
                }
            }

            return Task.CompletedTask;
        }

        private async Task<BodyContenteDTOMasiva> BodyContentForma9Masiva()
        {
            List<ProductionDetailForma9MasivaDTO> list = new List<ProductionDetailForma9MasivaDTO>();
            List<DetalleJsonF9Masiva> DetalleJsonF9Masiva = new List<DetalleJsonF9Masiva>();
            BodyContenteDTOMasiva _content = new BodyContenteDTOMasiva();
            int rows = Excel.Rows() + 1;
            var _total = new Form9Masivatotalvolumedetail
            {
                Form9tvdetailid = Guid.NewGuid(),
                Volumetype = 0,
                Formid = Guid.NewGuid()
            };
            _content.Total = _total;
            Header.UsuarioAprobador = null;
            Header.UsuarioMin = null;
            for (var row = 2; row < rows; row++)
            {
                ValidacionDiaMes($"{Excel.getCell(row, 2)}/{Excel.getCell(row, 3)}",
                    Excel.getCell(row, 13),
                    row,
                    Excel.getCell(row, 16),
                    Excel.getCell(row, 23),
                    Excel.getCell(row, 20)
                    );
                DetalleJsonF9Masiva itemDetalleJsonF9Masiva = new DetalleJsonF9Masiva()
                {
                    GERENCIA = ValidateCell("snull", Excel.getCell(row, 1), "Gerencia", row),
                    ANIO = ValidateCell("s", Excel.getCell(row, 2), "Año", row),
                    MES = ValidateCell("s", Excel.getCell(row, 3), "Mes", row),
                    UWI = ValidateCell("s", Excel.getCell(row, 4), "UWI", row),
                    PDEN_ID = ValidateCell("s", Excel.getCell(row, 5), "Pden Id", row),
                    ID_CAMPO = ValidateCell("snull", Excel.getCell(row, 6), "Id Campo", row),
                    CAMPO = ValidateCell("snull", Excel.getCell(row, 7), "Campo", row),
                    AREA = ValidateCell("snull", Excel.getCell(row, 8), "Area", row),
                    POZO = ValidateCell("snull", Excel.getCell(row, 9), "Pozo", row),
                    ZONA = ValidateCell("snull", Excel.getCell(row, 10), "Zona", row),
                    MUNICIPIO = ValidateCell("s", Excel.getCell(row, 11), "Municipio", row),
                    METODOPROD = ValidateCell("snull", Excel.getCell(row, 12), "Método Produción", row),
                    DIASMES = ValidateCell("nnnull", Excel.getCell(row, 13), "Mes Día", row),
                    DIASACUM = ValidateCell("nnnull", Excel.getCell(row, 14), "Acumulado Día", row),
                    PETROLEODIARIO = ValidateCell("nnnull", Excel.getCell(row, 15), "Diario Crudo", row),
                    PETROLEOMENSUAL = ValidateCell("nnnull", Excel.getCell(row, 16), "Mensual Crudo", row),
                    PETROLEOACUM = ValidateCell("nnnull", Excel.getCell(row, 17), "Acumulado Crudo", row),
                    FACTORCORRECCION = ValidateCell("nnnull", Excel.getCell(row, 18), "Factor Correción", row),
                    AGUADIARIO = ValidateCell("nnnull", Excel.getCell(row, 19), "Diario Agua", row),
                    AGUAMENSUAL = ValidateCell("nnnull", Excel.getCell(row, 20), "Mensual Agua", row),
                    AGUAACUM = ValidateCell("nnnull", Excel.getCell(row, 21), "Acumulado Agua", row),
                    GASDIARIO = ValidateCell("nnnull", Excel.getCell(row, 22), "Diario Gas", row),
                    GASMENSUAL = ValidateCell("nnnull", Excel.getCell(row, 23), "Mensual Gas", row),
                    GASACUM = ValidateCell("nnnull", Excel.getCell(row, 24), "Acumulado Gas", row),
                    BSW = ValidateCell("nnnull", Excel.getCell(row, 25), "BSW", row, true),
                    API = ValidateCell("nnnull", Excel.getCell(row, 26), "API", row, true),
                    RGA = ValidateCell("nnnull", Excel.getCell(row, 27), "RGA", row),
                    ESTADOPOZO = ValidateCell("snull", Excel.getCell(row, 28), "Estado", row),
                    OPERADOR = ValidateCell("snull", Excel.getCell(row, 29), "Operador", row),
                    NOMBRE_ZONA = ValidateCell("snull", Excel.getCell(row, 30), "Nombre_zona", row),
                    ELIMINAR = (!string.IsNullOrEmpty(Excel.getCell(row, 31)) && (Excel.getCell(row, 31).ToUpper() == "X")),
                    APROBADO = true

                };
                DetalleJsonF9Masiva.Add(itemDetalleJsonF9Masiva);

            }
            _content.ProductionDetail = list;
            JsonF9Masiva.REGISTRO = DetalleJsonF9Masiva;
            return await Task.Run(() => _content);
        }

        private void ValidacionDiaMes(string mesOperativo, string dia, int i, string crudo, string gas, string agua)
        {
            try
            {
                int seachPoint = 0;
                double dianum = 0;
                if (string.IsNullOrEmpty(dia))
                {
                    seachPoint = 0;
                    dianum = 0;
                }
                else
                {
                    seachPoint = dia.IndexOf(".");
                    dianum = Convert.ToDouble(dia);
                }
                var splitMesOperativo = mesOperativo.Split("/");
                string dateString = $"{splitMesOperativo[0]}-{splitMesOperativo[1]}-1";
                DateTime periodo = Convert.ToDateTime(dateString, CultureInfo.CreateSpecificCulture("es-ES"));
                DateTime date = new DateTime(periodo.Year, periodo.Month, 1).AddMonths(1).AddDays(-1);

                if ((dianum > date.Day) && (dianum > 0))
                {
                    var error = $"En la fila {i}, Verificar que la información diligenciada para el campo Dia en el mes no contenga valores vacíos o inválidos, días del Periodo cargado {date.Day} -> Dias Cargados en la forma {dianum} ";
                    ExcelError.message = "Verifique y corrija  la siguiente lista de errores ";
                    ExcelError.state = true;
                    ErrorsList.Add(error);
                }
                if (dianum <= 0 && ((Convert.ToDouble(crudo) + Convert.ToDouble(gas) + Convert.ToDouble(agua)) > 0))
                {
                    var error = $"En la fila {i}, Verificar que la información diligenciada para el campo Dia, que el mes sea mayor a 0 ya que la producción mensual de crudo, gas o agua contiene valores diferentes a 0";
                    ExcelError.message = "Verifique y corrija  la siguiente lista de errores ";
                    ExcelError.state = true;
                    ErrorsList.Add(error);
                }
            }
            catch
            {
                var error = $"Verificar que la información diligenciada para el campo Dia en el mes no contenga valores vacíos o inválidos en la línea {i}";
                ExcelError.message = "Verifique y corrija  la siguiente lista de errores ";
                ExcelError.state = true;
                ErrorsList.Add(error);

            }
        }

        private string ValidateCell(string type, dynamic Cell, string variable, int i, bool condicion = false)
        {
            var res = "";
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            CultureInfo ora = new CultureInfo("en-US");
            string cSeparadorOrigen = ci.NumberFormat.CurrencyDecimalSeparator;
            string cSeparadorDestino = ora.NumberFormat.CurrencyDecimalSeparator;
            try
            {
                if (type == "s") // Validar datos tipo caracter sin nulos
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vacíos o inválidos en la línea {i}";
                        ExcelError.message = "Verifique y corrija  la siguiente lista de errores ";
                        ExcelError.state = true;
                        ErrorsList.Add(error);
                    }
                    else
                    {
                        res = Cell;
                    }
                }
                if (type == "snull")   // deja pasar datos tipo caracter con nulos 
                {
                    res = Cell;
                }

                if (type == "nnnull")   // valida pero con valores numericos con un rango si llega nulo lo deja pasar
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        //Cell = "0"; Pasa el nulo
                        res = Cell;
                    }
                }
                if ((type == "n"))  // valida pero con valores numericos con un rango 
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Cell))
                        {
                            try
                            {

                                float value = float.Parse(Cell);
                                if (condicion)
                                {
                                    if (value < 0)
                                    {
                                        var error =
                                            $"El valor del campo {variable} no debe ser menor a 0 en la linea {i}";
                                        ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                        ExcelError.state = true;
                                        ErrorsList.Add(error);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Cell))
                                {
                                    res = Cell;
                                }
                            }
                            catch (Exception e)
                            {
                                res = Cell;
                                var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores inválidos en la línea {i}";
                                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                ExcelError.state = true;
                                ErrorsList.Add(error);
                                TelemetryException.RegisterException(e);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        res = Cell;
                        var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vaciós o inválidos en la línea {i}";
                        ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                        ExcelError.state = true;
                        ErrorsList.Add(error);

                        TelemetryException.RegisterException(e);
                    }
                }

                if ((type == "nn") || (type == "nnnull"))   // valida pero con valores numericos con un rango si llega nulo lo deja pasar
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        //Cell = "0";
                        Cell = string.Empty;
                    }
                    else
                    {
                        try
                        {
                            double valor = 0;
                            string cvalor = Cell.ToString();
                            valor = double.Parse(cvalor.Replace(cSeparadorOrigen, cSeparadorDestino));
                            if (valor.ToString() != Cell.ToString() && cSeparadorOrigen == cSeparadorDestino)
                            {
                                var error = $"El valor del campo {variable} debe ser un valor numerico, validar en la linea {i} la propiedad de decimales";
                                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                ExcelError.state = true;
                                ErrorsList.Add(error);
                            }
                            if (valor < 0)
                            {
                                var error =
                                    $"El valor del campo {variable} no debe ser menor a 0 en la linea {i}";
                                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                ExcelError.state = true;
                                ErrorsList.Add(error);
                            }
                            else
                            {
                                res = valor.ToString();
                            }
                        }
                        catch (Exception e)
                        {
                            res = "0";
                            var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vaciós o inválidos en la línea {i}";
                            ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                            ExcelError.state = true;
                            ErrorsList.Add(error);
                            TelemetryException.RegisterException(e);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                res = Cell;
                var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vaciós o inválidos en la línea {i}";
                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                ExcelError.state = true;
                ErrorsList.Add(error);
                TelemetryException.RegisterException(e);
            }

            return res;
        }

        private void ValidaContexto(string fileName, Stream file, string getUserId)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            var resForma9 = JsonF9Masiva.REGISTRO.ToList();
            int registros = JsonF9Masiva.REGISTRO.ToList().Count();
            int nBloque = 100;
            int ncortes = 1;
            int nInicial = 0;
            int nFinal = nBloque;
            int nReciduo = 0;
            if (registros >= nBloque)
            {
                ncortes = (int)((double)registros / nBloque);
                nReciduo = registros - (ncortes * nBloque);
            }
            if (registros < nBloque)
            {
                nFinal = registros;
            }
            int nveces = 1;

            List<JsonF9Masiva> JSFiltros = new List<JsonF9Masiva>();
            //JsonControlForma9 JSFiltros = new JsonControlForma9();
            if (nReciduo > 0)
            {
                ncortes = ncortes + 1;
            }

            while (nveces <= ncortes)
            {
                JsonF9Masiva reg = new JsonF9Masiva();
                reg.ANIO = JsonF9Masiva.ANIO;
                reg.MES = JsonF9Masiva.MES;
                reg.MIEMBRO_ID = JsonF9Masiva.MIEMBRO_ID;
                reg.FORMA_CODIGO = JsonF9Masiva.FORMA_CODIGO;
                reg.CAMPO_ID = nveces.ToString();
                var nTake = 0;
                if ((nReciduo > 0) && (nveces >= ncortes))
                {
                    nTake = nReciduo;
                }
                else
                {
                    nTake = nBloque;
                }
                var subjason = JsonF9Masiva.REGISTRO.Skip(nInicial).Take(nTake);
                reg.REGISTRO = subjason.ToList();
                var prueba = reg;
                JSFiltros.Add(reg);

                nveces++;
                nInicial += nBloque;
                nFinal += nBloque;
            }

            var json = JsonF9Masiva.Serialize(settings);

            try
            {
                Console.WriteLine("Inicia ValidarFormasMinAsync Excel : {0}", DateTime.Now);
                //var res = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });
                //var res = await _repository.SqlValidarFormas(json);
                var nControl = 1;
                /*new ParallelOptions { MaxDegreeOfParallelism = 8 }, */
                Parallel.ForEach(JSFiltros,
                            async msg =>
                            {
                                try
                                {
                                    Console.WriteLine("id " + msg.CAMPO_ID + " Excel :  " + DateTime.Now.ToString());
                                    var json2 = msg.Serialize(settings).ToString();
                                    //JSFiltros.Where(i => i.CAMPO_ID == nControl.ToString()).Serialize(settings);
                                    //var json2 = JSFiltros.Serialize(settings);

                                    var res = _repository.SqlValidarFormas(json2).GetAwaiter().GetResult();
                                    int codes = res.Code;

                                    //var codes = 200;
                                    if (codes == 200)
                                    {
                                        // var dataService = res.Data.Deserialize<ResponseForm9>(settings);
                                        try
                                        {
                                            RespuestaForma RespFormas = JsonSerializer.Deserialize<RespuestaForma>(res.Data);
                                            //RespuestaForma RespFormas = JsonSerializer.Deserialize<RespuestaForma>(Data);
                                            string cRegla = string.Empty;
                                            string cMensa = string.Empty;
                                            string cHomol = string.Empty;
                                            string cColumna = string.Empty;

                                            foreach (var itemPden in RespFormas.REGISTROS)
                                            {
                                                List<REG_MENSAJE> listError = new List<REG_MENSAJE>();
                                                if (itemPden.LLAVE != "")
                                                {
                                                    foreach (var itemValidacion in itemPden.VALIDACIONES)
                                                    {
                                                        cMensa = string.Empty;
                                                        cRegla = string.Empty;
                                                        cColumna = string.Empty;
                                                        for (int itemCol = 1; itemCol < Celdas_Excel.Count(); itemCol++)
                                                        {
                                                            if (itemValidacion.Columna.ToString().ToUpper() == Celdas_Excel[itemCol].Titulos.ToUpper())
                                                            {
                                                                if (itemValidacion.Codigo == 1)  // Estado del registro si es 1 Es Error 0 no es error
                                                                {
                                                                    if (!string.IsNullOrEmpty(itemValidacion.Mensaje.ToString()))
                                                                    {
                                                                        cMensa = itemValidacion.Mensaje.ToString();

                                                                        //ErrorsList.Add(itemValidacion.Mensaje); //se elimina por validaciones en la pantalla de detalles

                                                                        if (!string.IsNullOrEmpty(itemValidacion.Tipo.ToString()))
                                                                        {
                                                                            if (itemValidacion.Tipo.ToString() == "1")
                                                                            {
                                                                                resForma9.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { u.APROBADO = false; u.UserId = getUserId; return u; }).ToList();
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    cMensa = itemValidacion.Mensaje.ToString();
                                                                    //ErrorsList.Add(itemValidacion.Mensaje); para no confundir con los errores de excel
                                                                }
                                                                try
                                                                {
                                                                    cHomol = itemValidacion.DatoBDP;
                                                                    if (String.IsNullOrEmpty(cHomol))
                                                                    {
                                                                        cHomol = null;
                                                                    }
                                                                }
                                                                catch (Exception)
                                                                {
                                                                    cHomol = string.Empty;
                                                                }

                                                                if (!String.IsNullOrEmpty(cHomol)) //itemValidacion.DatoBDP.ToString()))
                                                                {
                                                                    //cHomol = itemValidacion.DatoBDP;
                                                                    resForma9.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u =>
                                                                    {
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "PDEN_ID") { u.PDEN_ID = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "UWI") { u.UWI = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "ID_CAMPO") { u.ID_CAMPO = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "CAMPO") { u.CAMPO = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "POZO") { u.POZO = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "ZONA") { u.ZONA = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "MUNICIPIO") { u.MUNICIPIO = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "METODOPROD") { u.METODOPROD = cHomol; }
                                                                        if (itemValidacion.Columna.ToString().ToUpper() == "ESTADOPOZO") { u.ESTADOPOZO = cHomol; }
                                                                        return u;
                                                                    }).ToList();
                                                                }
                                                                if (!String.IsNullOrEmpty(itemValidacion.Regla.ToString()))
                                                                {
                                                                    cRegla = itemValidacion.Regla;
                                                                    cColumna = itemValidacion.Columna.ToString().ToUpper();
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(cMensa) && !string.IsNullOrEmpty(cRegla))
                                                        {
                                                            REG_MENSAJE itemlistError = new REG_MENSAJE()
                                                            {
                                                                DETALLEMENSAJE = cMensa,
                                                                REGLA = cRegla,
                                                                COLUMNA = cColumna
                                                            };
                                                            listError.Add(itemlistError);
                                                        }
                                                    }
                                                    if (listError.Count > 0)
                                                    {
                                                        resForma9.Where(a => a.PDEN_ID == itemPden.LLAVE).Select(a =>
                                                        {
                                                            a.REG_MENSAJES = listError;
                                                            return a;
                                                        }).ToList();
                                                    }
                                                }
                                                else
                                                {
                                                    string cPozo = string.Empty;
                                                    resForma9.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { cPozo = u.UWI + "- " + u.POZO; return u; }).ToList();
                                                    ErrorsList.Add($"No se encontrarón registros en la Base de Datos tipo PDEN_ID {itemPden.LLAVE} relacionada al Pozo {cPozo} y la Formación {JsonF9Masiva.FORMACION}");
                                                    ExcelError.state = true;

                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            TelemetryException.RegisterException(e);

                                        }

                                    }

                                    if (codes != 200)
                                    {
                                        ErrorsList.Add($"No se pudo procesar la validación en BDP de las configuraciones asociadas a la forma 9 que se esta cargando, verifique e intente nuevamente");
                                        ExcelError.state = true;
                                    }
                                    nControl++;

                                }
                                catch (Exception e)
                                {
                                    ErrorsList.Add($"No se pudo procesar en BDP, verifique e intente nuevamente " + e.Message.ToString());
                                    ExcelError.state = true;
                                }
                            });
                Console.WriteLine("Fin ValidarFormasMinAsync Excel : {0}", DateTime.Now);
/*
                if (ErrorsList.Count == 0)
                {

                    var upload = await CommonService.UploadFileAsync(new Commons.FileSystemUploadFileOptions
                    {
                        FileName = fileName,
                        FileData = ConvertTypes.ConvertToBase64(file),
                        Container = "formas",
                        DestinationDirectory = new Commons.FileSystemItemInfo
                        {
                            Path = $"tmp"
                        }
                    });

                    if (upload.Code == 200)
                    {
                        Header.FileName = fileName;
                        Header.Url = "tmp";
                    }

                }
*/

            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
                ErrorsList.Add($"No se pudo procesar la validación en la base de datos BDP, de la forma que se esta cargando, verifique e intente nuevamente [Validar si existe conexion a BDP]");
                ExcelError.state = true;
            }
            /* Carga de Mensajes por cada Detlla de la forma*/
            Console.WriteLine("Inicio Json Para guardado: {0}", DateTime.Now);
            foreach (var item in resForma9)
            {
                var RegDatosSQL = new DetalleJsonF9Masiva
                {
                    GERENCIA = item.GERENCIA,
                    ANIO = item.ANIO,
                    MES = item.MES,
                    UWI = item.UWI,
                    PDEN_ID = item.PDEN_ID,
                    ID_CAMPO = item.ID_CAMPO,
                    CAMPO = item.CAMPO,
                    AREA = item.AREA,
                    POZO = item.POZO,
                    ZONA = item.ZONA,
                    MUNICIPIO = item.MUNICIPIO,
                    METODOPROD = item.METODOPROD,
                    DIASMES = item.DIASMES,
                    DIASACUM = item.DIASACUM,
                    PETROLEODIARIO = item.PETROLEODIARIO,
                    PETROLEOMENSUAL = item.PETROLEOMENSUAL,
                    PETROLEOACUM = item.PETROLEOACUM,
                    FACTORCORRECCION = item.FACTORCORRECCION,
                    AGUADIARIO = item.AGUADIARIO,
                    AGUAMENSUAL = item.AGUAMENSUAL,
                    AGUAACUM = item.AGUAACUM,
                    GASDIARIO = item.GASDIARIO,
                    GASMENSUAL = item.GASMENSUAL,
                    GASACUM = item.GASACUM,
                    BSW = item.BSW,
                    API = item.API,
                    RGA = item.RGA,
                    ESTADOPOZO = item.ESTADOPOZO,
                    APROBADO = item.APROBADO,
                    ELIMINAR = item.ELIMINAR,
                    OPERADOR = item.OPERADOR,
                    NOMBRE_ZONA = item.NOMBRE_ZONA,
                    REG_MENSAJES = item.REG_MENSAJES,
                    UserId = getUserId
                };
                _RespBDPJson.Add(RegDatosSQL);
            }
            Console.WriteLine("Fin Json Para guardado: {0}", DateTime.Now);
        }



        private async Task<bool> ValidaFechasPermisos(DateTime fechaForma, DateTime mesOperativo, string operador, string campo, string contrato, string getUserId)
        {
            DateTime mesActual = DateTime.Now;
            bool state = false;

            if (fechaForma < mesOperativo)
            {
                var permisos = await _repositoryAdministracion.GetPrecarga("Forma 9", fechaForma, operador, campo, contrato);
                if (permisos.Count() > 0)
                {
                    foreach (Aprobacionprecarga precarga in permisos)
                    {
                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                        {

                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                            {
                                Aprobacioncarga data = await _repositoryAdministracion.GetaFormAprobacion("Forma 9", precarga.FechaForma.Value, precarga.Operadora, precarga.Campo,
                                precarga.Contrato);
                                if (data != null)
                                {
                                    if (data.Formstate.Name != "Aprobada")
                                    {
                                        await _repository.Delete(data.IdForma.Value);
                                        await _repositoryAdministracion.Delete(data.IdForma.Value);
                                        state = true;
                                    }
                                    else
                                    {
                                        state = true;
                                    }
                                }
                                if (data == null)
                                {
                                    state = true;
                                }

                            }
                        }
                    }
                }
            }

            if (mesOperativo == fechaForma)
            {
                Aprobacioncarga data = await _repositoryAdministracion.GetaFormAprobacion("Forma 9", fechaForma, operador, campo, contrato, getUserId);
                List<Aprobacionprecarga> permisos = await _repositoryAdministracion.GetPrecarga("Forma 9", fechaForma, operador, campo, contrato, getUserId);

                if (data?.Formstate.Name != "Aprobada" && data?.Formstate.Name != null)
                {
                    await _repository.Delete(data.IdForma.Value);
                    await _repositoryAdministracion.Delete(data.IdForma.Value);
                    state = true;
                }
                else if (permisos?.Count > 0)
                {
                    foreach (Aprobacionprecarga item in permisos)
                    {
                        if (item.FechaForma == fechaForma)
                        {
                            state = true;
                            break;
                        }
                    }
                }
                else if (data?.Formstate.Name == "Aprobada" && permisos?.Count == 0)
                {
                    state = false;
                }
                else
                {
                    state = true;
                }

            }
            return state;
        }

        public Task<string> GuardarFormaPPDM(List<DetalleJsonF9Masiva> Datos)
        {
            //registerForm 
            //Detail9Multiple
            string resultado = string.Empty;
            var resForma9 = Datos.ToList();
            Console.WriteLine("Inicio Json Para guardado: {0}", DateTime.Now);



            List<JsonF9M_SQL> SQL_F9Masiva = new List<JsonF9M_SQL>();
            List<JsonF9M_BDP> BDP_F9Masiva = new List<JsonF9M_BDP>();
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };
            foreach (var item in resForma9)
            {
                string tempMensaje = string.Empty;
                int nVol = 0;

                try
                {
                    nVol = item.REG_MENSAJES.AsEnumerable().Count(); //.ToList().Count();
                }
                catch
                {
                    nVol = 0;
                }
                if (nVol == 0)
                {
                    tempMensaje = "";
                }
                else
                {
                    var json = item.REG_MENSAJES.Serialize(settings);
                    tempMensaje = json.ToString();
                };

                var RegDatosSQL = new JsonF9M_SQL
                {
                    Gerencia = item.GERENCIA,
                    Anio = item.ANIO,
                    Mes = item.MES,
                    Uwi = item.UWI,
                    PdenId = item.PDEN_ID,
                    CampoId = item.ID_CAMPO,
                    Campo = item.CAMPO,
                    Area = item.AREA,
                    Pozo = item.POZO,
                    Zona = item.ZONA,
                    Municipio = item.MUNICIPIO,
                    MetProduccion = item.METODOPROD,
                    Mes_dia = item.DIASMES,
                    Acumulado_dia = item.DIASACUM,
                    Diario_crudo = item.PETROLEODIARIO,
                    Mensual_crudo = item.PETROLEOMENSUAL,
                    Acumulado_crudo = item.PETROLEOACUM,
                    FactorCorreccion = item.FACTORCORRECCION,
                    Diario_agua = item.AGUADIARIO,
                    Mensual_agua = item.AGUAMENSUAL,
                    Acumulado_agua = item.AGUAACUM,
                    Diario_gas = item.GASDIARIO,
                    Mensual_gas = item.GASMENSUAL,
                    Acumulado_gas = item.GASACUM,
                    Bsw = item.BSW,
                    Api = item.API,
                    Rgp = item.RGA,
                    Estado = item.ESTADOPOZO,
                    Aprobado = item.APROBADO.ToString(),
                    CargadoaBDP = "true",
                    row_created_by = item.UserId,
                    row_created_date = DateTime.Now.ToString(),
                    row_changed_by = item.UserId,
                    row_changed_date = DateTime.Now.ToString(),
                    Json_MensajesErr = tempMensaje,
                    Operador = item.OPERADOR,
                    Nombrezona = item.NOMBRE_ZONA,
                    Eliminar = item.ELIMINAR,
                };
                SQL_F9Masiva.Add(RegDatosSQL);
                if (item.APROBADO)
                {
                    var RegDatosBDP = new JsonF9M_BDP
                    {
                        GERENCIA = item.GERENCIA,
                        ANIO = item.ANIO,
                        MES = item.MES,
                        UWI = item.UWI,
                        PDEN_ID = item.PDEN_ID,
                        ID_CAMPO = item.ID_CAMPO,
                        CAMPO = item.CAMPO,
                        AREA = item.AREA,
                        POZO = item.POZO,
                        ZONA = item.ZONA,
                        MUNICIPIO = item.MUNICIPIO,
                        METODOPROD = item.METODOPROD,
                        DIASMES = item.DIASMES,
                        DIASACUM = item.DIASACUM,
                        PETROLEODIARIO = item.PETROLEODIARIO,
                        PETROLEOMENSUAL = item.PETROLEOMENSUAL,
                        PETROLEOACUM = item.PETROLEOACUM,
                        FACTORCORRECCION = item.FACTORCORRECCION,
                        AGUADIARIO = item.AGUADIARIO,
                        AGUAMENSUAL = item.AGUAMENSUAL,
                        AGUAACUM = item.AGUAACUM,
                        GASDIARIO = item.GASDIARIO,
                        GASMENSUAL = item.GASMENSUAL,
                        GASACUM = item.GASACUM,
                        BSW = item.BSW,
                        API = item.API,
                        RGA = item.RGA,
                        ESTADOPOZO = item.ESTADOPOZO,
                        USERID = item.UserId,
                        ELIMINAR = item.ELIMINAR ? "1" : "0",
                        OPERADOR = item.OPERADOR,
                        NOMBRE_ZONA = item.NOMBRE_ZONA
                    };
                    BDP_F9Masiva.Add(RegDatosBDP);
                }
            }

            //var resForma9 = JsonF9Masiva.REGISTRO.ToList();
            int registros = BDP_F9Masiva.Count();
            int nBloque = 100;
            int ncortes = 1;
            int nInicial = 0;
            int nFinal = nBloque;
            int nReciduo = 0;
            if (registros >= nBloque)
            {
                ncortes = (int)((double)registros / nBloque);
                nReciduo = registros - (ncortes * nBloque);
            }
            if (registros < nBloque)
            {
                nFinal = registros;
            }
            int nveces = 1;

            List<Ctr_JsonF9M_BDP> JSFiltros = new List<Ctr_JsonF9M_BDP>();
            if (nReciduo > 0)
                ncortes = ncortes + 1;

            while (nveces <= ncortes)
            {
                Ctr_JsonF9M_BDP reg = new Ctr_JsonF9M_BDP();
                reg.Contador = nveces.ToString();
                var nTake = 0;
                if ((nReciduo > 0) && (nveces >= ncortes))
                {
                    nTake = nReciduo;
                }
                else
                {
                    nTake = nBloque;
                }
                var subjason = BDP_F9Masiva.Skip(nInicial).Take(nTake);
                reg.JContenido = subjason.ToList();
                var prueba = reg;
                JSFiltros.Add(reg);

                nveces++;
                nInicial += nBloque;
                nFinal += nBloque;
            }
            string Bdp_Json = BDP_F9Masiva.Serialize(settings).ToString();
            resultado = "";
            bool lGuardarBdp = false;
            if (Bdp_Json.Count() > 0)
            {
                Console.Clear();
                Parallel.ForEach(JSFiltros, async msg =>
                {
                    try
                    {
                        Console.WriteLine("id " + msg.Contador + " Excel :  " + DateTime.Now.ToString());
                        var Bdp_Json = msg.JContenido.Serialize(settings).ToString();
                        string EncJson = "{";
                        EncJson += "\"FORMA_CODIGO\": \"9M\",";
                        EncJson += "\"REGISTRO\": ";
                        Bdp_Json = EncJson + Bdp_Json + "}";
                        var respBdp = _repository.GuardaBdpforma9(Bdp_Json).GetAwaiter().GetResult();
                        //await _repository.GuardaBdpforma9(Bdp_Json);
                        string crSql = respBdp.Substring(respBdp.IndexOf("BDP_Respuesta:", 0) + 14, 1);
                        lGuardarBdp = (crSql == "0");
                        if (crSql == "1")  //Respuesta con mensajes de error 
                        {
                            respBdp = respBdp.Substring(StartIndex, respBdp.Length - 2 - 16);  // quitar los caracteres especiales 
                            dynamic myORespBdp = JValue.Parse(respBdp);
                            string cTabla = string.Empty;
                            string cMensa = string.Empty;
                            string cColumna = string.Empty;
                            foreach (var itemPden in myORespBdp?.REGISTROS)
                            {
                                List<REG_MENSAJE> listValError = new List<REG_MENSAJE>();
                                if (itemPden.LLAVE != "")
                                {
                                    foreach (var itemErrValidacion in itemPden.VALIDACIONES)
                                    {
                                        cMensa = string.Empty;
                                        cTabla = string.Empty;
                                        cColumna = string.Empty;
                                        if (!string.IsNullOrEmpty(itemErrValidacion.Mensaje.ToString()))
                                        {
                                            cMensa = itemErrValidacion.Mensaje.ToString();
                                            REG_MENSAJE itemlistValError = new REG_MENSAJE()   // compatibilida con tabla de mensajes 
                                            {
                                                DETALLEMENSAJE = cMensa,
                                                REGLA = itemErrValidacion.Tabla.ToString(),
                                                COLUMNA = itemErrValidacion.Columna.ToString().ToUpper()
                                        };
                                            listValError.Add(itemlistValError);
                                        }
                                    }
                                    /* Buscar En el JSON Anterior*/
                                    if (listValError.Count > 0)
                                    {
                                        var jsonc = listValError.Serialize(settings);
                                        string cMensaNuevo = jsonc.ToString().Replace("}]", "}").Replace("[{", "{");
                                        foreach (var UpdSQL in SQL_F9Masiva)
                                        {
                                            if (UpdSQL.PdenId.ToString() == itemPden.LLAVE.ToString())
                                            {
                                                string cOldValue = UpdSQL.Json_MensajesErr.ToString().Replace("}]", "}").Replace("[{", "{");
                                                string cNewMensaje = "[" + cMensaNuevo + (string.IsNullOrEmpty(cOldValue) ? "" : ", ") + cOldValue + "]";
                                                SQL_F9Masiva.Where(u => u.PdenId == itemPden.LLAVE.ToString()).Select(u => { u.Json_MensajesErr = cNewMensaje; return u; });
                                            }
                                        }
                                    }
                                }
                            }


                        }
                        if (crSql == "2")  //Error Oracle
                        {
                            respBdp = respBdp.Substring(StartIndex, respBdp.Length - 2 - 16);  // quitar los caracteres especiales 
                        }
                        resultado = "BDP_Respuesta:" + crSql + ";" + respBdp + ".;";
                    }
                    catch (Exception e)
                    {
                        ErrorsList.Add($"No se pudo procesar en BDP, verifique e intente nuevamente " + e.Message.ToString());
                        ExcelError.state = true;
                    }
                });
            }
            string Sql_Json = SQL_F9Masiva.Serialize(settings).ToString();
            if ((Sql_Json.Count() > 0)) //&& (!lGuardarBdp))  //Pasa si pasa BDP 
            {
                //string Sql_Json = SQL_F9Masiva.Serialize(settings).ToString();
                Console.WriteLine("Inicio Guardado SQL: {0}", DateTime.Now);
                var respSql = _repository.Guardasqlforma9(Sql_Json);
                Console.WriteLine("Fin Guardado SQL: {0}", DateTime.Now);
                resultado = respSql + resultado;
                //if (codes == 200)
            }
            return Task.Run(() => resultado);
            //ResponseBase<RequestCreateForma9Masiva>(message: "Registros Guardados", data: null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAnio"></param>
        /// <param name="PMes"></param>
        /// <param name="PTodo"></param>
        /// <returns></returns>
        public Task<string> consultaforma9(int pAnio, int PMes, int PTodo)
        {
            string respuesta = _repository.Observacionforma9(pAnio, PMes, PTodo);

            return Task.Run(() => respuesta);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAnio"></param>
        /// <param name="pMes"></param>
        /// <param name="GetUserId"></param>
        /// <returns></returns>
        public async Task<string> consultaValidaAcumlaforma9(int pAnio, int pMes, string GetUserId)
        {
            string respuesta = await _repository.rep_consultaValidaAcumlaforma9(pAnio, pMes, GetUserId);
            return respuesta;
            // return Task.FromResult(Task.Run(() => respuesta));
        }
        public string StringBetween(string cSource, string Start, string End)
        {
            return _repository.StringBetween(cSource, Start, End);
        }
    }
}

