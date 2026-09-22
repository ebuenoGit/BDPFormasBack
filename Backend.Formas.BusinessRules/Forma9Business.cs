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

namespace Backend.Formas.BusinessRules
{
    public class Forma9Business : IFormas9Business
    {
        private readonly IForma9Repository _repository;
        private readonly IAdministracionFormas _repositoryAdministracion;
        private readonly List<string> ErrorsList = new List<string>();
        private readonly IImportarExcel Excel;
        private readonly ExcelError ExcelError = new ExcelError();
        private readonly Forma9StructureDTO Forma9Respose = new Forma9StructureDTO();
        private readonly HeaderForma9DTO Header = new HeaderForma9DTO();
        private List<BodyContenteDTO> content = new List<BodyContenteDTO>();
        private BodyContenteDTO bodyForma = new BodyContenteDTO();
        private readonly IFormaValidate RepositoryValidate;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;


        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        private readonly JsonF9 JsonF9 = new JsonF9();
        private string _fileName = "";
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");

        public Forma9Business(
            IForma9Repository _repo,
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

        public async Task<ResponseBase<Forma9StructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId)
        {
            var response = new ResponseBase<Forma9StructureDTO>();

            Excel.setFile(files);
            Excel.reader("FORMA 9");
            if (Excel.isLoad())
            {
                _fileName = fileName;
                // var worksheets = await Excel.getWorksheets();
                var rows = Excel.Rows();
                if (rows > 0)
                {
                    //validamos datos de cabecera
                    await HeaderFroma9(date, getUserId);

                    var detalle = await BodyContentForma9();

                    bodyForma = detalle;
                    await ValidaContexto(fileName, files, getUserId);

                    if (ExcelError.state == false)
                    {
                        detalle.Header = Header;
                        detalle.workbook = "Forma 9";
                        content.Add(detalle);
                        Forma9Respose.data = content;
                    }

                    if (ExcelError.cargaExtemporal == true)
                    {
                        ExcelError.message = "La forma que intenta cargar se encuentra en un mes operativo anterior o esta forma ya ha sido aprobada";
                        ExcelError.header = Header;
                        Forma9Respose.data = content;
                        Forma9Respose.errors = ExcelError;
                    }

                    if (ExcelError.state == true && ExcelError.cargaExtemporal == false)
                    {
                        content = new List<BodyContenteDTO>();
                        ExcelError.message = "El archivo cargado no tiene el formato correcto verifique e intente de nuevo";
                        Forma9Respose.data = content;
                        Forma9Respose.errors = ExcelError;
                    }
                }
                else
                {
                    ExcelError.state = true;
                    ExcelError.message = "Error el archivo no cuenta con un formato valido verifique e intente de nuevo";
                    Forma9Respose.errors = ExcelError;
                }
            }
            else
            {
                ExcelError.state = true;
                ExcelError.message = "La Forma no contiene información o no fue posible leer la información del documento";
                Forma9Respose.errors = ExcelError;
            }

            response.Data = Forma9Respose;

            return response;
        }

        public async Task<ResponseBase<dynamic>> Create(RequestCreateForma9 data, DateTime date, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();

            try
            {
                var header = data.header;
                var body = data.detail;
                var total = data.total;
                CultureInfo cultureInfo = new CultureInfo("es-co");

                var form = new Form9
                {
                    Block = !string.IsNullOrEmpty(header.Bloque) ? header.Bloque : "",
                    Oilfield = !string.IsNullOrEmpty(header.Yacimiento) ? header.Yacimiento : "",
                    Structure = !string.IsNullOrEmpty(header.CierreEstructural) ? header.CierreEstructural : "",
                    Member = !string.IsNullOrEmpty(header.Miembro) ? header.Miembro : "",
                    Form9id = Guid.NewGuid(),
                    operadorId = !string.IsNullOrEmpty(header.OperadorId) ? header.OperadorId : "",
                    campo = header.Campo ?? "",
                    campoId = !string.IsNullOrEmpty(header.CampoId) ? header.CampoId : "",
                    bloque = header.Bloque ?? "",
                    bloqueId = !string.IsNullOrEmpty(header.BloqueId) ? header.BloqueId : "",
                    formacion = header.Formacion ?? "",
                    formacionId = !string.IsNullOrEmpty(header.FormacionId) ? header.FormacionId : "",
                    formacionSetId = !string.IsNullOrEmpty(header.FormacionSetId) ? header.FormacionSetId : "",
                    yacimiento = header.Yacimiento ?? "",
                    yacimientoId = !string.IsNullOrEmpty(header.YacimientoId) ? header.YacimientoId : "",
                    contratoId = !string.IsNullOrEmpty(header.ContratoId) ? header.ContratoId : "",
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
                    campo = decimal.Parse(header.CampoId);

                    concrete = new Concreteform
                    {
                        Concreteformid = Guid.NewGuid(),
                        Maincampid = 0,
                        Company = !string.IsNullOrEmpty(header.Operador) ? header.Operador : "",
                        Contract = !string.IsNullOrEmpty(header.Contrato) ? header.Contrato : "",
                        Battery = "",
                        Tank = "",
                        Month = dateForm.Month,
                        Year = dateForm.Year,
                        Explotationmodality = !string.IsNullOrEmpty(header.ModalidadExplotacion)
                        ? header.ModalidadExplotacion
                        : "",
                        Annotations = "",
                        Version = 1,
                        Currentstate = forStateid,
                        Generationflag = 0,
                        Campid = campo,
                        Pdenid = header.OperadorId,
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
                List<Form9detail> form9DetailList = new List<Form9detail>();
                List<Acumulados> Acumulados = new List<Acumulados>();

                for (var row = 0; row <= body.Count - 1; row++)
                {
                    try
                    {
                        var item = body[row];
                        var form9Detail = new Form9detail
                        {
                            Oilwell = item.pozo,
                            Formation = header.Formacion,
                            Danecode = item.municipio,
                            Productionmethod = item.metProducion,
                            Monthdays = decimal.Parse(item.mes_dia),
                            Accumulatedays = decimal.Parse(item.acumulado_dia),
                            Dailyoilproduction = decimal.Parse(item.diario_crudo),
                            Monthlyoilproduction = decimal.Parse(item.mensual_crudo),
                            Accumulateoilproduction = decimal.Parse(item.acumuado_crudo),
                            Correctionfactor = decimal.Parse(item.factorCorrecion),
                            Dailywaterproduction = decimal.Parse(item.diario_agua),
                            Monthlywaterproduction = decimal.Parse(item.mensual_agua),
                            Accumulatewaterproduction = decimal.Parse(item.acumuado_agua),
                            Dailygasproduction = decimal.Parse(item.diario_gas),
                            Montlhygasproduction = decimal.Parse(item.mensual_gas),
                            Accumulategasproduction = decimal.Parse(item.acumuado_gas),
                            Bsw = decimal.Parse(item.bsw),
                            Apigrades = decimal.Parse(item.api),
                            Rgp = decimal.Parse(item.rgp),
                            Oilwellfinalstate = item.estado ?? "",
                            Formid = form.Form9id,
                            Poolname = "",
                            PdenId = item.pdenId,
                            row_created_by = GetUserId,
                            row_created_date = DateTime.Now,
                            row_changed_by = GetUserId,
                            row_changed_date = DateTime.Now,
                            Form9detailid = Guid.NewGuid(),
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

                var totalRegister = new Form9totalvolumedetail()
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
                    Formation = header.Formacion,
                    row_created_by = GetUserId,
                    row_created_date = DateTime.Now,
                    row_changed_by = GetUserId,
                    row_changed_date = DateTime.Now
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
                        Campo = header.Campo,
                        Contrato = header.Contrato,
                        Operadora = header.Operador
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

                        string view = mailForma.GetView(asunto, dataService.NombreUsuario, header.Operador, header.Contrato, header.Campo, fecha,
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

        private async Task HeaderFroma9(DateTime dateForma, string getUserId)
        {

            try
            {
                //compañia o operador
                if (string.IsNullOrEmpty(Excel.getCell(7, 2)))
                {
                    ErrorsList.Add("El formato no tiene un Operador");
                }

                if (!string.IsNullOrEmpty(Excel.getCell(7, 2)))
                {
                    var operador = Excel.getCell(7, 2);
                    var list = await RepositoryValidate.ValidaOperador(operador);
                    string BUSINESS_ASSOCIATE = ConvertTypes.ConverDataDynamic(list, "BUSINESS_ASSOCIATE");
                    if (list?.Count > 0)
                    {
                        JsonF9.OPERADOR = operador;
                        JsonF9.OPERADOR_ID = BUSINESS_ASSOCIATE;
                        Header.OperadorId = BUSINESS_ASSOCIATE;
                    }
                    if (list?.Count == 0 || list == null || string.IsNullOrEmpty(BUSINESS_ASSOCIATE))
                    {
                        ErrorsList.Add("El Operador o Compañía no existe en la Base de Datos");
                    }
                }

                // contrato
                if (string.IsNullOrEmpty(Excel.getCell(7, 8)))
                {
                    ErrorsList.Add("El formato no tiene un Contrato ");
                }

                if (!string.IsNullOrEmpty(Excel.getCell(7, 8)))
                {
                    var contrato = Excel.getCell(7, 8);
                    var list = await RepositoryValidate.ValidaContrato(contrato);
                    string LAND_RIGHT_ID = ConvertTypes.ConverDataDynamic(list, "LAND_RIGHT_ID");
                    if (list?.Count > 0)
                    {
                        JsonF9.CONTRATO_ID = LAND_RIGHT_ID;
                        Header.ContratoId = LAND_RIGHT_ID;
                        JsonF9.CONTRATO = contrato;
                    }
                    if (list?.Count == 0 || list == null || string.IsNullOrEmpty(LAND_RIGHT_ID))
                    {
                        ErrorsList.Add("El Contrato no existe en la Base de Datos");
                    }
                }

                //campo
                if (string.IsNullOrEmpty(Excel.getCell(7, 11)))
                {
                    ErrorsList.Add("El formato no cuenta con un Campo");
                }
                if (!string.IsNullOrEmpty(Excel.getCell(7, 11)))
                {
                    var campo = Excel.getCell(7, 11);
                    var list = await RepositoryValidate.ValidaCampo(campo);
                    string FIELD_ID = ConvertTypes.ConverDataDynamic(list, "FIELD_ID");


                    if (list?.Count > 0)
                    {
                        JsonF9.CAMPO_ID = FIELD_ID;
                        JsonF9.CAMPO = campo;
                        Header.CampoId = FIELD_ID;
                    }

                    if (list?.Count == 0 || list == null || string.IsNullOrEmpty(FIELD_ID))
                    {
                        ErrorsList.Add("El Campo no existe en la Base de Datos");
                    }
                }

                if (!string.IsNullOrEmpty(Excel.getCell(7, 2)) && !string.IsNullOrEmpty(Excel.getCell(7, 8)) && !string.IsNullOrEmpty(Excel.getCell(7, 11)))
                {
                    string operador = Excel.getCell(7, 2);
                    string contrato = Excel.getCell(7, 8);
                    string campo = Excel.getCell(7, 11);

                    var list = await RepositoryValidate.ValidaCampoContratoOperador(operador, campo, contrato);
                    if (list?.Count > 0)
                    {
                        string operador_id = ConvertTypes.ConverDataDynamic(list, "OPERADOR_ID");
                        string contrato_id = ConvertTypes.ConverDataDynamic(list, "CONTRATO_ID");
                        string campo_id = ConvertTypes.ConverDataDynamic(list, "CAMPO_ID");

                        Header.Operador = operador;
                        Header.OperadorId = operador_id;
                        Header.Campo = campo;
                        Header.CampoId = campo_id;
                        Header.Contrato = contrato;
                        JsonF9.CONTRATO_ID = contrato_id;
                        JsonF9.CAMPO_ID = campo_id;
                        JsonF9.OPERADOR_ID = operador_id;
                        JsonF9.CAMPO = campo;
                    }
                    else
                    {
                        ErrorsList.Add($"Validar nombre del contrato { contrato }, o nombre del campo { campo }, u operador, ya que no se identifica la forma a cargar");
                    }
                }


                //cierre estructural
                if (string.IsNullOrEmpty(Excel.getCell(7, 15)))
                {
                    ErrorsList.Add("No se encontró el valor de Cierre Estructural");
                }
                //JsonF9.ESTRUCTURA_ID 

                //bloque
                //bloque usa validacion de campo
                if (string.IsNullOrEmpty(Excel.getCell(7, 19)))
                {
                    ErrorsList.Add("No se encontró el valor de Bloque");
                }
                if (!string.IsNullOrEmpty(Excel.getCell(7, 19)))
                {
                    var bloque = Excel.getCell(7, 19);
                    var list = await RepositoryValidate.ValidaContrato(bloque);
                    var LAND_RIGHT_ID = ConvertTypes.ConverDataDynamic(list, "LAND_RIGHT_ID");

                    if (list?.Count > 0)
                    {
                        JsonF9.BLOQUE_ID = LAND_RIGHT_ID;
                        Header.BloqueId = LAND_RIGHT_ID;
                    }

                    if (list?.Count == 0 || list == null || string.IsNullOrEmpty(LAND_RIGHT_ID))
                    {
                        ErrorsList.Add("El Bloque no existe en la Base de Datos");
                    }
                }

                //formacion
                if (string.IsNullOrEmpty(Excel.getCell(8, 2)))
                {
                    ErrorsList.Add("No se en contró una Formación");
                }

                if (!string.IsNullOrEmpty(Excel.getCell(8, 2)))
                {
                    var formacion = Excel.getCell(8, 2);
                    var list = await RepositoryValidate.ValidaFormacion(formacion);
                    var STRAT_UNIT_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_UNIT_ID");
                    var STRAT_NAME_SET_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_NAME_SET_ID");

                    if (list?.Count > 0)
                    {
                        JsonF9.FORMACION_ID = STRAT_UNIT_ID;
                        JsonF9.FORMACION_SET_ID = STRAT_NAME_SET_ID;
                        JsonF9.FORMACION = formacion;
                        Header.FormacionId = STRAT_UNIT_ID;
                        Header.FormacionSetId = STRAT_NAME_SET_ID;
                    }
                    if (list?.Count == 0 || list == null || string.IsNullOrEmpty(STRAT_UNIT_ID))
                    {
                        ErrorsList.Add("La Formación no existe en la Base de Datos");
                    }
                }

                //miembro
                if (string.IsNullOrEmpty(Excel.getCell(8, 8)))
                {
                    ErrorsList.Add("No se encontró un Miembro");
                }

                //JsonF9.MIEMBRO_ID { set; get; }

                //modalidad de explotación
                string modalidad = ModalidaDeExplotacion();
                if (string.IsNullOrEmpty(modalidad))
                {
                    ErrorsList.Add("No se encontró un Modalidad de Explotación");
                }

                //yacimiento
                if (string.IsNullOrEmpty(Excel.getCell(8, 11)))
                {
                    ErrorsList.Add("No se en contró el valor del Yacimiento");
                }

                if (!string.IsNullOrEmpty(Excel.getCell(8, 11)))
                {
                    var yacimiento = Excel.getCell(8, 11);
                    var list = await RepositoryValidate.ValidaYacimiento(yacimiento);
                    string STRAT_UNIT_ID = ConvertTypes.ConverDataDynamic(list, "STRAT_UNIT_ID");

                    if (list.Count > 0)
                    {
                        JsonF9.YACIMIENTO_ID = STRAT_UNIT_ID;
                        Header.YacimientoId = STRAT_UNIT_ID;
                    }

                    if (list?.Count == 0 || list == null || string.IsNullOrEmpty(STRAT_UNIT_ID))
                    {
                        ErrorsList.Add("El Yacimiento no existe en la Base de Datos");
                    }
                }

                //fecha mes y año
                if (string.IsNullOrEmpty(Excel.getCell(8, 15)) || string.IsNullOrEmpty(Excel.getCell(8, 19)))
                {
                    ErrorsList.Add("Verifique que el Mes y el Año no estén vacíos y sean validos");
                }

                if (!string.IsNullOrEmpty(Excel.getCell(8, 15)) || !string.IsNullOrEmpty(Excel.getCell(8, 19)))
                {
                    string dateString = $"{Excel.getCell(8, 15)}/{Excel.getCell(8, 19)}";
                    var dateTime = DateTime.Parse(dateString, cultureInfo);

                    try
                    {
                        bool stateDate = true;
                        if (dateTime < dateForma)
                        {
                            ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
                            stateDate = false;
                        }
                        if (dateTime > dateForma)
                        {
                            ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
                            stateDate = false;
                        }

                        DateTime mesActual = DateTime.Now;
                        DateTime mesOpertivo;
                        if (mesActual.Month == 1)
                            mesOpertivo = new DateTime(mesActual.Year - 1, 12, 1);
                        else
                            mesOpertivo = new DateTime(mesActual.Year, mesActual.Month - 1, 1);



                        if (dateTime < mesOpertivo && stateDate)
                        {
                            bool validaPermiso = await ValidaFechasPermisos(fechaForma: dateForma, mesOpertivo,
                            operador: Excel.getCell(7, 2), campo: Excel.getCell(7, 11), contrato: Excel.getCell(7, 8), getUserId);

                            if (validaPermiso == false)
                            {
                                ExcelError.cargaExtemporal = true;
                                ExcelError.state = true;

                            }

                        }

                        if (dateTime > mesOpertivo)
                        {
                            //ExcelError.cargaExtemporal = true;
                            ErrorsList.Add("La fecha de la forma es superior a la fecha operativa");
                        }


                        JsonF9.ANIO = dateTime.Year.ToString();
                        JsonF9.MES = dateTime.Month.ToString();

                        if (dateTime == mesOpertivo)
                        {
                            bool validaPermiso = await ValidaFechasPermisos(fechaForma: dateForma, mesOpertivo,
                            operador: Excel.getCell(7, 2), campo: Excel.getCell(7, 11), contrato: Excel.getCell(7, 8), getUserId);

                            if (validaPermiso == false)
                            {
                                ExcelError.cargaExtemporal = true;
                                ExcelError.state = true;
                            }
                        }


                    }
                    catch (Exception e)
                    {
                        TelemetryException.RegisterException(e);
                        ErrorsList.Add("La fecha de la forma no es valida corrija e intente de nuevo");
                    }

                }



                int day = dateForma.AddMonths(1).AddDays(-1).Day;
                DateTime volumDate = new DateTime(dateForma.Year, dateForma.Month, day);

                var res = await RepositoryValidate.ValidaCompaniaCampoEstado(Excel.getCell(7, 2), Excel.getCell(7, 11), volumDate.ToString("yyyyMMdd"));

                if (res?.Count == 0 || res == null)
                {
                    ErrorsList.Add("Es posible que el campo contrato no este activo  o  la fecha de cargue no esta dentro del rango de la fecha efectiva o fecha expedicion ");
                }

                JsonF9.FORMA_CODIGO = "9";
            }
            catch (Exception e)
            {
                ExcelError.message = "Se presentó un error al validar la forma";
                TelemetryException.RegisterException(e);
            }


            if (ErrorsList.Count == 0 || ExcelError.cargaExtemporal == true)
            {
                Header.Operador = Excel.getCell(7, 2);
                Header.Contrato = Excel.getCell(7, 8);
                Header.Campo = Excel.getCell(7, 11);
                Header.CierreEstructural = Excel.getCell(7, 15);
                Header.Formacion = Excel.getCell(8, 2);
                Header.Miembro = Excel.getCell(8, 8);
                Header.ModalidadExplotacion = ModalidaDeExplotacion();
                Header.Mes = Excel.getCell(8, 15);
                Header.Ano = Excel.getCell(8, 19);
                Header.Bloque = Excel.getCell(7, 19);
                Header.Yacimiento = Excel.getCell(8, 11);

            }
            else
            {
                if (ErrorsList.Count > 0)
                {
                    ExcelError.state = true;
                    ExcelError.message =
                        "El archivo cargado no tiene el formato correcto verifique he intente de nuevo";
                    ExcelError.listError = ErrorsList;
                }
            }
        }

        private async Task<BodyContenteDTO> BodyContentForma9()
        {
            List<ProductionDetailForma9DTO> list = new List<ProductionDetailForma9DTO>();
            List<DetalleJsonF9> DetalleJsonF9 = new List<DetalleJsonF9>();
            BodyContenteDTO _content = new BodyContenteDTO();
            int rows = Excel.Rows();
            for (var row = 17; row <= rows - 1; row++)
            {
                dynamic total = Excel.getCell(row, 1);
                bool isTotal = false;
                if (total != null)
                {
                    isTotal = total.StartsWith("Total", StringComparison.CurrentCultureIgnoreCase);
                    if (isTotal)
                    {
                        try
                        {
                            var _total = new Form9totalvolumedetail
                            {
                                Form9tvdetailid = Guid.NewGuid(),
                                Volumetype = 0,
                                Accumulategasproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 15), "Acumuado Gas", row)),
                                Montlhygasproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 14), "Mensual Gas", row)),
                                Dailygasproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 13), "Diario Gas", row)),
                                Accumulatewaterproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 12), "Acumulado Agua", row)),
                                Monthlywaterproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 11), "Mensual Agua", row)),
                                Dailywaterproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 10), "Diario Gas", row)),
                                Accumulateoilproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 8), "Acumulado Crudo", row)),
                                Dailyoilproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 6), "Diario Crudo", row)),
                                Monthlyoilproduction =
                           decimal.Parse(ValidateCell("nn", Excel.getCell(row, 7), "Mensual Crudo", row)),
                                Bsw = decimal.Parse(ValidateCell("n", Excel.getCell(row, 16), "BSW", row, true)),
                                Apigrades = decimal.Parse(ValidateCell("n", Excel.getCell(row, 17), "API", row, true)),
                                Rgp = decimal.Parse(ValidateCell("nn", Excel.getCell(row, 18), "RGA", row)),
                                Formid = Guid.NewGuid(),
                                Formation = Header.Formacion
                            };
                            _content.total = _total;
                            var aprobador = Excel.getCell(row + 8, 1);
                            var ministerio = Excel.getCell(row + 8, 14);

                            Header.UsuarioAprobador = aprobador;
                            Header.UsuarioMin = ministerio;

                        }
                        catch (Exception e)
                        {
                            var error = $"Se presento un error en la linea {row} verifique e intente de nuevamente";
                            ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                            ExcelError.state = true;
                            ErrorsList.Add(error);
                            TelemetryException.RegisterException(e);
                        }

                        break;
                    }
                }

                isTotal = total?.StartsWith("Total", StringComparison.CurrentCultureIgnoreCase) ?? false;
                if (isTotal == false)
                {
                    var totales = new ProductionDetailForma9DTO
                    {
                        pozo = ValidateCell("s", Excel.getCell(row, 1), "Pozo", row),
                        municipio = ValidateCell("s", Excel.getCell(row, 2), "Municipio", row),
                        metProducion = ValidateCell("s", Excel.getCell(row, 3), "Método Produción", row),
                        mes_dia = ValidateCell("nn", Excel.getCell(row, 4), "Mes Día", row),
                        acumulado_dia = ValidateCell("nn", Excel.getCell(row, 5), "Acumulado Día", row),
                        diario_crudo = ValidateCell("nn", Excel.getCell(row, 6), "Diario Crudo", row),
                        mensual_crudo = ValidateCell("nn", Excel.getCell(row, 7), "Mensual Crudo", row),
                        acumuado_crudo = ValidateCell("nn", Excel.getCell(row, 8), "Acumulado Crudo", row),
                        factorCorrecion = ValidateCell("nn", Excel.getCell(row, 9), "Factor Correción", row),
                        diario_agua = ValidateCell("nn", Excel.getCell(row, 10), "Diario Agua", row),
                        mensual_agua = ValidateCell("nn", Excel.getCell(row, 11), "Mensual Agua", row),
                        acumuado_agua = ValidateCell("nn", Excel.getCell(row, 12), "Acumulado Agua", row),
                        diario_gas = ValidateCell("nn", Excel.getCell(row, 13), "Diario Gas", row),
                        mensual_gas = ValidateCell("nn", Excel.getCell(row, 14), "Mensual Gas", row),
                        acumuado_gas = ValidateCell("nn", Excel.getCell(row, 15), "Acumulado Gas", row),
                        bsw = ValidateCell("n", Excel.getCell(row, 16), "BSW", row, true),
                        api = ValidateCell("n", Excel.getCell(row, 17), "API", row, true),
                        rgp = ValidateCell("nn", Excel.getCell(row, 18), "RGA", row),
                        estado = ValidateCell("s", Excel.getCell(row, 19), "Estado", row)
                    };
                    ValidacionDiaMes($"{JsonF9.ANIO}/{JsonF9.MES}", totales.mes_dia, row, totales.diario_crudo, totales.diario_gas, totales.diario_agua);
                    list.Add(totales);
                    DetalleJsonF9 itemDetalleJsonF9 = new DetalleJsonF9()
                    {
                        POZO = ValidateCell("s", Excel.getCell(row, 1), "Pozo", row),
                        MUNICIPIO = ValidateCell("s", Excel.getCell(row, 2), "Municipio", row),
                        OIL_VOLUME = Excel.getCell(row, 7),
                        OIL_CUM_VOLUME = Excel.getCell(row, 8),
                        GAS_VOLUME = Excel.getCell(row, 14),
                        GAS_CUM_VOLUME = Excel.getCell(row, 15),
                        WATER_VOLUME = Excel.getCell(row, 11),
                        WATER_CUM_VOLUME = Excel.getCell(row, 12),
                    };

                    DetalleJsonF9.Add(itemDetalleJsonF9);

                }


            }

            _content.productionDetail = list;
            JsonF9.REGISTRO = DetalleJsonF9;

            ExcelError.listError = ErrorsList;
            return await Task.Run(() => _content);
        }

        private void ValidacionDiaMes(string mesOperativo, string dia, int i, string crudo, string gas, string agua)
        {
            try
            {
                int seachPoint = 0;
                int dianum = 0;
                seachPoint = dia.IndexOf(".");
                dianum = (seachPoint != -1) ? Convert.ToInt32(dia.Substring(0, seachPoint)) : Convert.ToInt32(dia);
                var splitMesOperativo = mesOperativo.Split("/");
                string dateString = $"{splitMesOperativo[0]}-{splitMesOperativo[1]}-1";
                DateTime dateTime = Convert.ToDateTime(dateString, CultureInfo.CreateSpecificCulture("es-ES"));                
                int year = 0;
                int month = 0;

                if (dateTime.Month == 12)
                {
                    year = dateTime.Year + 1;
                    month = 1;
                }
                else
                {
                    year = dateTime.Year;
                    month = dateTime.Month + 1;
                }

                DateTime date = new DateTime(year, month, 1).AddDays(-1);

                if (dianum > date.Day)
                {
                    var error = $"Verificar que la información diligenciada para el campo Dia en el mes no contenga valores vacíos o inválidos en la línea {i}";
                    ExcelError.message = "Verifique y corrija  la siguiente lista de errores ";
                    ExcelError.state = true;
                    ErrorsList.Add(error);
                }
                else if (dianum < 1 && (Convert.ToDouble(crudo) != 0 || Convert.ToDouble(gas) != 0 || Convert.ToDouble(agua) != 0))
                {
                    var error = $"Verificar que la información diligenciada para el campo Dia, que el mes sea mayor a 0 ya que la producción diaria de crudo, gas o agua contiene valores diferentes a 0 en la línea {i}";
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
            try
            {
                if (type == "s")
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

                if (type == "n")
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vacíos o inválidos en la línea {i}";
                        ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                        ExcelError.state = true;
                        ErrorsList.Add(error);
                        res = "0";
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(Cell))
                        {
                            try
                            {
                                float value = float.Parse(Cell);
                                if (condicion)
                                {
                                    if (value < 0 || value > 100)
                                    {
                                        var error =
                                            $"El valor del campo {variable} no debe ser menor a 0 ó mayor a 100  en la linea {i}";
                                        ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                        ExcelError.state = true;
                                        ErrorsList.Add(error);
                                    }
                                }

                                if (!string.IsNullOrEmpty(Cell))
                                {

                                    if (value == 0)
                                    {
                                        res = "0";
                                    }
                                    else
                                    {
                                        res = Cell;
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                res = "0";
                                var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vacíos o inválidos en la línea {i}";
                                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                ExcelError.state = true;
                                ErrorsList.Add(error);
                                TelemetryException.RegisterException(e);
                            }
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

                if (type == "nn")
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vaciós o inválidos en la línea {i}";
                        ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                        ExcelError.state = true;
                        ErrorsList.Add(error);
                        res = "0";
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(Cell))
                        {
                            try
                            {
                                float value = float.Parse(Cell);
                                if (value < 0)
                                {
                                    var error =
                                        $"El valor del campo {variable} no debe ser menor a 0 en la linea {i}";
                                    ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                    ExcelError.state = true;
                                    ErrorsList.Add(error);
                                }
                                else
                                {
                                    res = value.ToString();
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
            catch (Exception e)
            {
                res = "0";
                var error = $"Verificar que la información diligenciada para el campo  {variable} no contenga valores vaciós o inválidos en la línea {i}";
                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                ExcelError.state = true;
                ErrorsList.Add(error);
                TelemetryException.RegisterException(e);
            }

            return res;
        }

        private async Task ValidaContexto(string fileName, Stream file, string getUserId)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            try
            {
                List<JsonF9> listJson = new List<JsonF9>
                {
                    JsonF9
                };
                var json = listJson.Serialize(settings);
                var res = await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });

                if (res.Code == 200)
                {
                    //validar codigo
                    try
                    {
                        var dataService = res.Data.Deserialize<ResponseForm9>(settings);
                        List<Registro> registros = dataService.FORMAS?.FORMA?.REGISTRO;

                        if (registros?.Count > 0)
                        {
                            foreach (var item in registros)
                            {
                                if (item.PDEN_ID == "FALSE")
                                {
                                    ErrorsList.Add($"No se encontrarón registros en la Base de Datos relacionada al Pozo {item.POZO} y la Formación {JsonF9.FORMACION}");
                                    ExcelError.state = true;
                                }

                                if (!string.IsNullOrEmpty(item.PDEN_ID) && item.PDEN_ID != "FALSE")
                                {
                                    bodyForma.productionDetail.FirstOrDefault(x => x.pozo == item.POZO).pdenId = item.PDEN_ID;
                                    bodyForma.productionDetail.FirstOrDefault(x => x.pozo == item.POZO).VALIDACION_PRODUCCION = item.VALIDACION_PRODUCCION;
                                    bodyForma.productionDetail.FirstOrDefault(x => x.pozo == item.POZO).AGUA = item.AGUA;
                                    bodyForma.productionDetail.FirstOrDefault(x => x.pozo == item.POZO).CRUDO = item.CRUDO;
                                    bodyForma.productionDetail.FirstOrDefault(x => x.pozo == item.POZO).GAS = item.GAS;
                                }

                                if (string.IsNullOrEmpty(item.PDEN_ID))
                                {
                                    ErrorsList.Add($"No se encontrarón registros en la Base de Datos relacionada al Pozo {item.POZO} y la Formación {JsonF9.FORMACION}");
                                    ExcelError.state = true;
                                }
                            }
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
                        }
                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;

                            if (element.MENSAJE == "Esta Forma ya fue cargada")
                            {
                                CultureInfo cultureInfo = new CultureInfo("es-co");
                                var dateTime = DateTime.Parse($"{JsonF9.ANIO}/{JsonF9.MES}", cultureInfo);
                                var mes = DateTime.Now;
                                DateTime mesActual = new DateTime(mes.Year, mes.Month - 1, 1);
                                var permisos = await _repositoryAdministracion.GetPrecarga("Forma 9", dateTime, JsonF9.OPERADOR, JsonF9.CAMPO, JsonF9.CONTRATO);
                                if (permisos.Count() > 0)
                                {
                                    foreach (Aprobacionprecarga precarga in permisos)
                                    {
                                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                                        {

                                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                                            {
                                                ExcelError.cargaExtemporal = false;
                                                ExcelError.state = true;
                                            }
                                        }
                                    }
                                }
                                else { ExcelError.cargaExtemporal = true; ExcelError.state = true; }

                            }
                            else
                            {

                                ErrorsList.Add(element.MENSAJE);
                                ExcelError.state = true;
                            }
                        }

                    }
                    catch
                    {

                        var dataService = res.Data.Deserialize<ResponseForm9Json>(settings);
                        Registro registro = dataService.FORMAS?.FORMA?.REGISTRO;


                        if (registro != null)
                        {
                            if (registro.PDEN_ID == "FALSE")
                            {
                                ErrorsList.Add($"No se encontrarón registros en la Base de Datos relacionada al Pozo {registro.POZO} y la Formación {JsonF9.FORMACION}");
                                ExcelError.state = true;
                            }

                            if (!string.IsNullOrEmpty(registro.PDEN_ID) && registro.PDEN_ID != "FALSE")
                            {
                                bodyForma.productionDetail.FirstOrDefault(x => x.pozo == registro.POZO).pdenId = registro.PDEN_ID;
                            }

                            if (string.IsNullOrEmpty(registro.PDEN_ID))
                            {
                                ErrorsList.Add($"No se encontrarón registros en la Base de Datos relacionada al Pozo {registro.POZO} y la Formación {JsonF9.FORMACION}");
                                ExcelError.state = true;
                            }


                            bodyForma.productionDetail.FirstOrDefault(x => x.pozo == registro.POZO).VALIDACION_PRODUCCION = registro.VALIDACION_PRODUCCION;
                            bodyForma.productionDetail.FirstOrDefault(x => x.pozo == registro.POZO).AGUA = registro.AGUA;
                            bodyForma.productionDetail.FirstOrDefault(x => x.pozo == registro.POZO).CRUDO = registro.CRUDO;
                            bodyForma.productionDetail.FirstOrDefault(x => x.pozo == registro.POZO).GAS = registro.GAS;


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
                        }
                        if (dataService.root != null)
                        {
                            Element element = dataService.root.element;
                            if (element.MENSAJE == "Esta Forma ya fue cargada")
                            {
                                CultureInfo cultureInfo = new CultureInfo("es-co");
                                var dateTime = DateTime.Parse($"{JsonF9.ANIO}/{JsonF9.MES}", cultureInfo);
                                var mes = DateTime.Now;
                                DateTime mesActual = new DateTime(mes.Year, mes.Month - 1, 1);
                                var permisos = await _repositoryAdministracion.GetPrecarga("Forma 9", dateTime, JsonF9.OPERADOR, JsonF9.CAMPO, JsonF9.CONTRATO);
                                if (permisos.Count() > 0)
                                {
                                    foreach (Aprobacionprecarga precarga in permisos)
                                    {
                                        if (precarga.Activo == 1 && precarga.Usuario == getUserId)
                                        {

                                            if (precarga.FechaApertura <= mesActual && mesActual <= precarga.FechaCierre)
                                            {
                                                ExcelError.cargaExtemporal = false;
                                            }
                                        }
                                    }
                                }
                                else { ExcelError.cargaExtemporal = true; }

                            }
                            else
                            {

                                ErrorsList.Add(element.MENSAJE);
                                ExcelError.state = true;
                            }

                        }
                    }
                }

                if (res.Code != 200)
                {
                    ErrorsList.Add($"No se pudo procesar la validación de la forma que se esta cargando, verifique e intente nuevamente");
                    ExcelError.state = true;

                }
            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
                ErrorsList.Add($"No se pudo procesar la validación de la forma que se esta cargando, verifique e intente nuevamente");

                ExcelError.state = true;
            }


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

    }
}