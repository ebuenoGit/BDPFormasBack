using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace Backend.Formas.BusinessRules
{
    public class FormaC4MasivaBusiness : IFormasC4MasivaBusiness
    {

        private readonly List<BodyContenteDTOC4Masiva> content = new List<BodyContenteDTOC4Masiva>();

        private readonly IImportarExcel Excel;
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        private string _fileName = "";

        private readonly ExcelError ExcelError = new ExcelError();
        private readonly List<string> ErrorsList = new List<string>();
        readonly List<CeldasExcel> Celdas_Excel = new List<CeldasExcel>();
        readonly List<CeldasExcel> Celdas_Excel_Muni = new List<CeldasExcel>();
        private readonly BodyContenteDTOC4Masiva bodyForma = new BodyContenteDTOC4Masiva();


        private readonly FormaC4MasivaStructureDTO FormaC4Respose = new FormaC4MasivaStructureDTO();
        private readonly HeaderFormaC4MasivaDTO Header = new HeaderFormaC4MasivaDTO();
        private readonly HeaderFormaC4MasivaDTO HeaderMuni = new HeaderFormaC4MasivaDTO();

        private readonly JsonC4Masivo JsonC4ValBDP = new JsonC4Masivo();
        private readonly JsonC4Municipio JsonC4ValMuniBDP = new JsonC4Municipio();

        private readonly List<DetalleJsonC4Masiva> DetalleJsonC4Masiva = new List<DetalleJsonC4Masiva>();
        private readonly List<DetalleJsonC4Municipio> DetalleJsonC4Muni = new List<DetalleJsonC4Municipio>();
        private readonly List<DetalleJsonC4Masiva> _RespBDPJson = new List<DetalleJsonC4Masiva>();
        private readonly List<DetalleJsonC4Municipio> _RespBDPMuniJson = new List<DetalleJsonC4Municipio>();
        private readonly IFormaC4MasivaRepository _repository;


        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly IAdministracionFormas _repositoryAdministracion;

        /*
        
        private readonly IFormaValidate RepositoryValidate;
        
        
        
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;
        
        private readonly JsonF9Masiva JsonF9Masiva = new JsonF9Masiva();
        private readonly DetalleJsonF9Masiva detJsonF9Masiva = new DetalleJsonF9Masiva();

        private readonly ProductionDetailForma9MasivaDTO jsondetalleDorma9 = new ProductionDetailForma9MasivaDTO();

        private List<DetalleJsonF9Masiva> _RespBDPJson = new List<DetalleJsonF9Masiva>();
        private readonly CultureInfo cultureInfo = new CultureInfo("es-co");

        
        */
        public FormaC4MasivaBusiness(
            IImportarExcel excel,
             Utilities.Telemetry.ITelemetryException telemetryException,
            IFormaC4MasivaRepository _repo,
            Ppdm.PpdmGrpc.PpdmGrpcClient formasService,
            Commons.CommonGrpc.CommonGrpcClient commonService,
            Backend.Formas.Utilities.SendMail.ISendMailService sendMailService,
            Administracion.AdmGrpc.AdmGrpcClient administracionService,
            IAdministracionFormas admin
            /*
            
            IFormaValidate _formaValidate,
            
            
            
            Backend.Formas.Utilities.SendMail.ISendMailService sendMailService
            */
            )
        {
            Excel = excel;
            TelemetryException = telemetryException;
            _repository = _repo;
            FormasService = formasService;
            CommonService = commonService;
            SendMailService = sendMailService;
            AdministracionService = administracionService;
            _repositoryAdministracion = admin;
            ExcelError.cargaExtemporal = false;
            ExcelError.state = false;
            /*
            _repositoryAdministracion = admin;
            RepositoryValidate = _formaValidate;
            AdministracionService = administracionService;
            FormasService = formasService;
            CommonService = commonService;
            SendMailService = sendMailService;
            */
        }



        public async Task<ResponseBase<FormaC4MasivaStructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId)
        {

            var response = new ResponseBase<FormaC4MasivaStructureDTO>();
            Excel.setFile(files);
            Excel.reader("CUADRO 4");
            if (Excel.isLoad())
            {
                _fileName = fileName;
                var rows = Excel.Rows();
                if (rows <= 1)
                {
                    ExcelError.state = true;
                    ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido en la Hoja [CUADRO 4]";
                    FormaC4Respose.Errors = ExcelError;
                    FormaC4Respose.Data = content; // DetalleJsonC4Masiva;
                }
                else if (rows > 0)
                {
                    //validamos datos de cabecera
                    await HeaderFormaC4Masiva(date, getUserId);

                    var detalle = await BodyContentFormaC4Masiva(getUserId);

                    await ValidaContexto(fileName, files, getUserId);

                    bodyForma.Header = Header;
                    bodyForma.RespBDP = DetalleJsonC4Masiva;
                    bodyForma.Workbook = "CUADRO 4";

                    detalle.Header = Header;
                    detalle.RespBDP = _RespBDPJson;
                    detalle.Workbook = "CUADRO 4";

                    /* Carga de la Hoja municipio */
                    Excel.reader("MUNICIPIO");
                    if (Excel.isLoad())
                    {
                        var rowsMuni = Excel.Rows();
                        if (rows > 0)
                        {
                            //validamos datos de cabecera
                            await HeaderFormaC4MuniMasiva(date, getUserId);

                            var detalleM = await BodyContentFormaC4MuniMasiva(getUserId);

                            await ValidaContextoMunicipio(fileName, files, getUserId);

                            bodyForma.RespBDPMuni = _RespBDPMuniJson;

                            detalle.RespBDPMuni = detalleM.RespBDP;
                        }

                    }
                    content.Add(detalle);

                    if (ExcelError.state == false)
                    {
                        FormaC4Respose.Data = content; //DetalleJsonC4Masiva;
                        FormaC4Respose.Errors = ExcelError;
                    }

                    if (ExcelError.cargaExtemporal == true)
                    {
                        ExcelError.message = "La forma que intenta cargar se encuentra en un mes operativo anterior o esta forma ya ha sido aprobada";
                        ExcelError.header = Header;
                        FormaC4Respose.Data = content; // DetalleJsonC4Masiva;
                        FormaC4Respose.Errors = ExcelError;
                        ExcelError.state = true;
                    }

                    if (ExcelError.state == true) //&& ExcelError.cargaExtemporal == false)
                    {
                        ExcelError.message = "El archivo cargado tiene validaciones pendientes, verifique e intente de nuevo";
                        FormaC4Respose.Data = content; //DetalleJsonC4Masiva;
                        FormaC4Respose.Errors = ExcelError;
                    }
                }
                else
                {
                    ExcelError.state = true;
                    ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido de Hoja [CUADRO 4] e intente de nuevo";
                    FormaC4Respose.Errors = ExcelError;
                    FormaC4Respose.Data = content; // DetalleJsonC4Masiva;
                }
            }
            else
            {
                ExcelError.state = true;
                ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido de [CUADRO 4]";
                FormaC4Respose.Errors = ExcelError;
                FormaC4Respose.Data = content; // DetalleJsonC4Masiva;
            }

            ExcelError.listError = ErrorsList;

            response.Data = FormaC4Respose;

            return response;
        }

        private Task HeaderFormaC4Masiva(DateTime dateForma, string getUserId)
        {
            try
            {
                DateTime FechaVolumen = dateForma;
                // Periodo de carga 
                string iAnio = FechaVolumen.Year.ToString();

                string iMes = FechaVolumen.Month.ToString();
                int iFila = Excel.Rows();
                int iCol = 50;
                var hashPden = new HashSet<string>();
                string cMens = "";
                string cValor = string.Empty;
                string[] TitulosHoja = { "", "ANIO", "MES", "PDEN_ID","PDEN_NAME", "ID_CAMPO", "CAMPO", "CONTRATO", "MODALIDAD", "EXISTENCIAINICIAL",
                    "PRODUCCIONBASICA", "PRODUCCIONINCREMENTAL", "PRODUCCIONTOTAL", "CONSUMOSBASICA", "CONSUMOSINCREMENTAL", "CONSUMOSTOTAL",
                    "PERDIDASBASICA", "PERDIDASINCREMENTAL", "PERDIDASTOTAL", "GRAVABLEBASICA", "GRAVABLEINCREMENTAL", "GRAVABLETOTAL", "OTRASENTRADAS",
                    "ENTREGAS", "EXISTENCIAFINAL", "LLENADOLINEAS", "LLENADOVASIJAS", "OTRASPERDIDAS", "OTROSCONSUMOS", "GRAVEDADAPI",
                    "CONTENIDOAZUFRE", "BSW", "GRAVEDADESPECIFICA", "CONTENIDOSAL", "OPERADOR", "ELIMINAR" };
                /*
                 * Validacion inicial del archivo excel 
                 */
                Header.Mes = iMes;
                Header.Ano = iAnio;
                Header.FORMA_CODIGO = "4C";
                Header.MIEMBRO_ID = getUserId.ToString();
                Header.FileName = getUserId.ToString();
                Header.Url = getUserId.ToString();

                JsonC4ValBDP.MES = iMes;
                JsonC4ValBDP.ANIO = iAnio;
                JsonC4ValBDP.FORMA_CODIGO = "4C";

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
                        if (cValor.ToUpper() != TitulosHoja[nCol])
                        {
                            string cTitulErr = $"Hoja [CUADRO 4], En la columna " + nCol.ToString() + " => " + cValor + " No corresponde el nombre de la columna, " + TitulosHoja[nCol];
                            ErrorsList.Add(cTitulErr);
                        }
                    }
                }

                for (var iCon = 2; iCon <= iFila; iCon++)
                {
                    dynamic colAnio = Excel.getCell(iCon, 1); // Carga informacion de la columna de Anio
                    dynamic colMes = Excel.getCell(iCon, 2);  // Carga informacion de la columna de Mes
                    string colPden = Excel.getCell(iCon, 3);  // Carga informacion de la columna de Pden_id 

                    /* Validacion del año vs periodo de carga*/
                    if (string.IsNullOrEmpty(colAnio))
                    {
                        cMens += "Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El año esta en blanco, ";
                        ErrorsList.Add("Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El año esta en blanco, ");
                    }
                    else
                    {
                        if (colAnio != iAnio)
                        {
                            cMens += "Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado en el archivo excel, ";
                            ErrorsList.Add("Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado  en el archivo excel, ");
                        }
                    }
                    /* Validacion del mes vs periodo de carga*/
                    if (string.IsNullOrEmpty(colMes))
                    {
                        cMens += "Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El mes esta en blanco, ";
                        ErrorsList.Add("Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El mes esta en blanco, ");
                    }
                    else
                    {
                        if (colMes != iMes)
                        {
                            cMens += "Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ";
                            ErrorsList.Add("Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ");
                        }
                    }
                    /* Validacion del PDEN_ID no esta en blanco y nulo*/
                    if (string.IsNullOrEmpty(colPden))
                    {
                        cMens += "Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ";
                        ErrorsList.Add("Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ");
                    }
                    else
                    {
                        colPden = colPden.Trim();
                        if (!hashPden.Add(colPden))
                        {
                            cMens += "Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, ";
                            ErrorsList.Add("Hoja [CUADRO 4], En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, ");
                        }
                    }

                }

                if (FechaVolumen < dateForma)
                {
                    ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
                }
                if (FechaVolumen > dateForma)
                {
                    ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
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

            if (ErrorsList.Count > 0)//|| !ExcelError.cargaExtemporal)
            {
                ExcelError.state = true;
                ExcelError.message =
                    "Hoja [CUADRO 4], El archivo cargado tiene mensajes de validación de los datos, verifique he intente de nuevo";
            }
            return Task.CompletedTask;
        }

        private Task HeaderFormaC4MuniMasiva(DateTime dateForma, string getUserId)
        {
            try
            {
                DateTime FechaVolumen = dateForma;
                // Periodo de carga 
                string iAnio = FechaVolumen.Year.ToString();

                string iMes = FechaVolumen.Month.ToString();
                int iFila = Excel.Rows();
                int iCol = 50;
                var hashPden = new HashSet<string>();
                string cMens = "";
                string cValor = string.Empty;
                string[] TitulosHoja = { "", "ANIO", "MES", "PDEN_ID","PDEN_NAME", "ID_CAMPO", "CAMPO", "CONTRATO", "MUNICIPIO",
                    "PRODUCCIONBASICA", "PRODUCCIONINCREMENTAL", "PRODUCCIONTOTAL", "CONSUMOSBASICA", "CONSUMOSINCREMENTAL", "CONSUMOSTOTAL",
                    "PERDIDASBASICA", "PERDIDASINCREMENTAL", "PERDIDASTOTAL", "GRAVABLEBASICA", "GRAVABLEINCREMENTAL", "GRAVABLETOTAL",
                    "ELIMINAR", "PDEN_XREF" };

                /*
                 * Validacion inicial del archivo excel 
                 */

                JsonC4ValMuniBDP.MES = iMes;
                JsonC4ValMuniBDP.ANIO = iAnio;
                JsonC4ValMuniBDP.FORMA_CODIGO = "4M";
                for (var nCol = 1; nCol < iCol; nCol++)
                {
                    cValor = Excel.getCell(1, nCol);
                    if (!string.IsNullOrEmpty(cValor))
                    {
                        CeldasExcel itemCeldasExcel = new CeldasExcel()
                        {
                            Titulos = Excel.getCell(1, nCol)
                        };
                        Celdas_Excel_Muni.Add(itemCeldasExcel);
                        if (cValor.ToUpper() != TitulosHoja[nCol])
                        {
                            string cTitulErr = $"Hoja [MUNICIPIO], En la columna " + nCol.ToString() + " => " + cValor + " No corresponde el nombre de la columna, " + TitulosHoja[nCol];
                            ErrorsList.Add(cTitulErr);
                        }
                    }
                }

                for (var iCon = 2; iCon <= iFila; iCon++)
                {
                    dynamic colAnio = Excel.getCell(iCon, 1); // Carga informacion de la columna de Anio
                    dynamic colMes = Excel.getCell(iCon, 2);  // Carga informacion de la columna de Mes
                    string colPden = Excel.getCell(iCon, 3);  // Carga informacion de la columna de Pden_id 
                    string colMuni = Excel.getCell(iCon, 8);  // Carga informacion de la columna de Municipio 

                    /* Validacion del año vs periodo de carga*/
                    if (string.IsNullOrEmpty(colAnio))
                    {
                        cMens += "Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " no se indicó el año, ";
                        ErrorsList.Add("Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " no se indicó el año, ");
                    }
                    else
                    {
                        if (colAnio != iAnio)
                        {
                            cMens += "Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado en el archivo excel, ";
                            ErrorsList.Add("Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado  en el archivo excel, ");
                        }
                    }
                    /* Validacion del mes vs periodo de carga*/
                    if (string.IsNullOrEmpty(colMes))
                    {
                        cMens += "Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El mes esta en blanco, ";
                        ErrorsList.Add("Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El mes esta en blanco, ");
                    }
                    else
                    {
                        if (colMes != iMes)
                        {
                            cMens += "Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ";
                            ErrorsList.Add("Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ");
                        }
                    }
                    if (string.IsNullOrEmpty(colMuni))
                    {
                        cMens += "Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El Municipio esta en blanco, ";
                        ErrorsList.Add("Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El Municipio  esta en blanco, ");
                    }
                    /* Validacion del PDEN_ID no esta en blanco y nulo */

                    if (string.IsNullOrEmpty(colPden))
                    {
                        cMens += "Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ";
                        ErrorsList.Add("Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ");
                    }
                    else
                    {
                        colPden = colPden.Trim() + colMuni.Trim();
                        if (!hashPden.Add(colPden))
                        {
                            cMens += "Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, con el mismo municipio " + colMuni;
                            ErrorsList.Add("Hoja [MUNICIPIO], En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, con el mismo municipio " + colMuni);
                        }
                    }

                }

                if (FechaVolumen < dateForma)
                {
                    ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
                }
                if (FechaVolumen > dateForma)
                {
                    ErrorsList.Add("La fecha de la forma no concuerda con la seleccionada");
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

            if (ErrorsList.Count > 0) //|| !ExcelError.cargaExtemporal)
            {
                ExcelError.state = true;
                ExcelError.message =
                    "Hoja [MUNICIPIO], El archivo cargado tiene mensajes de validación de los datos, verifique he intente de nuevo";
            }
            return Task.CompletedTask;
        }

        private async Task<BodyContenteDTOC4Masiva> BodyContentFormaC4Masiva(string getUserId)
        {
            BodyContenteDTOC4Masiva _content = new BodyContenteDTOC4Masiva();
            int rows = Excel.Rows();
            for (var row = 2; row <= rows; row++)
            {
                DetalleJsonC4Masiva itemDetalleJsonC4Masiva = new DetalleJsonC4Masiva()
                {
                    ANIO = ValidateCell("s", Excel.getCell(row, 1), "ANIO", row),
                    MES = ValidateCell("s", Excel.getCell(row, 2), "MES", row),
                    PDEN_ID = ValidateCell("s", Excel.getCell(row, 3), "PDEN_ID", row),
                    PDEN_NAME = ValidateCell("snull", Excel.getCell(row, 4), "PDEN_NAME", row),
                    ID_CAMPO = ValidateCell("snull", Excel.getCell(row, 5), "ID_CAMPO", row),
                    CAMPO = ValidateCell("snull", Excel.getCell(row, 6), "CAMPO", row),
                    CONTRATO = ValidateCell("snull", Excel.getCell(row, 7), "CONTRATO", row),
                    MODALIDAD = ValidateCell("snull", Excel.getCell(row, 8), "MODALIDAD", row),
                    EXISTENCIAINICIAL = ValidateCell("nnnull", Excel.getCell(row, 9), "EXISTENCIAINICIAL", row),
                    //MUNICIPIO = ValidateCell("snull", Excel.getCell(row, 9), "MUNICIPIO", row),
                    PRODUCCIONBASICA = ValidateCell("nnnull", Excel.getCell(row, 10), "PRODUCCIONBASICA", row),
                    PRODUCCIONINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 11), "PRODUCCIONINCREMENTAL", row),
                    PRODUCCIONTOTAL = ValidateCell("nnnull", Excel.getCell(row, 12), "PRODUCCIONTOTAL", row),
                    CONSUMOSBASICA = ValidateCell("nnnull", Excel.getCell(row, 13), "CONSUMOSBASICA", row),
                    CONSUMOSINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 14), "CONSUMOSINCREMENTAL", row),
                    CONSUMOSTOTAL = ValidateCell("nnnull", Excel.getCell(row, 15), "CONSUMOSTOTAL", row),
                    PERDIDASBASICA = ValidateCell("nnnull", Excel.getCell(row, 16), "PERDIDASBASICA", row),
                    PERDIDASINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 17), "PERDIDASINCREMENTAL", row),
                    PERDIDASTOTAL = ValidateCell("nnnull", Excel.getCell(row, 18), "PERDIDASTOTAL", row),
                    GRAVABLEBASICA = ValidateCell("nnnull", Excel.getCell(row, 19), "GRAVABLEBASICA", row),
                    GRAVABLEINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 20), "GRAVABLEINCREMENTAL", row),
                    GRAVABLETOTAL = ValidateCell("nnnull", Excel.getCell(row, 21), "GRAVABLETOTAL", row),
                    OTRASENTRADAS = ValidateCell("nnnull", Excel.getCell(row, 22), "OTRASENTRADAS", row),
                    ENTREGAS = ValidateCell("nnnull", Excel.getCell(row, 23), "ENTREGAS", row),
                    EXISTENCIAFINAL = ValidateCell("nnnull", Excel.getCell(row, 24), "EXISTENCIAFINAL", row),
                    LLENADOLINEAS = ValidateCell("nnnull", Excel.getCell(row, 25), "LLENADOLINEAS", row),
                    LLENADOVASIJAS = ValidateCell("nnnull", Excel.getCell(row, 26), "LLENADOVASIJAS", row),
                    OTRASPERDIDAS = ValidateCell("nnnull", Excel.getCell(row, 27), "OTRASPERDIDAS", row),
                    OTROSCONSUMOS = ValidateCell("nnnull", Excel.getCell(row, 28), "OTROSCONSUMOS", row),
                    GRAVEDADAPI = ValidateCell("nnnull", Excel.getCell(row, 29), "GRAVEDADAPI", row),
                    CONTENIDOAZUFRE = ValidateCell("nnnull", Excel.getCell(row, 30), "CONTENIDOAZUFRE", row),
                    BSW = ValidateCell("nnnull", Excel.getCell(row, 31), "BSW", row, true),
                    GRAVEDADESPECIFICA = ValidateCell("nnnull", Excel.getCell(row, 32), "GRAVEDADESPECIFICA", row, true),
                    CONTENIDOSAL = ValidateCell("nnnull", Excel.getCell(row, 33), "CONTENIDOSAL", row),
                    OPERADOR = ValidateCell("snull", Excel.getCell(row, 34), "OPERADOR", row),
                    ELIMINAR = (!string.IsNullOrEmpty(Excel.getCell(row, 35)) && (Excel.getCell(row, 34).ToUpper() == "X")),
                    APROBADO = true,
                    USERID = getUserId
                };
                /*
                ValidarValor("Producción", "Cuadro 4", row, Excel.getCell(row, 9), Excel.getCell(row, 10), Excel.getCell(row, 11));
                ValidarValor("Consumos", "Cuadro 4", row, Excel.getCell(row, 12), Excel.getCell(row, 13), Excel.getCell(row, 14));
                ValidarValor("Pérdidas", "Cuadro 4", row, Excel.getCell(row, 15), Excel.getCell(row, 16), Excel.getCell(row, 17));
                ValidarValor("Gravable", "Cuadro 4", row, Excel.getCell(row, 18), Excel.getCell(row, 19), Excel.getCell(row, 20));
                */
                DetalleJsonC4Masiva.Add(itemDetalleJsonC4Masiva);
            }
            _content.RespBDP = DetalleJsonC4Masiva;
            //return await Task.Run(function: () => DetalleJsonC4Masiva);
            return await Task.Run(() => _content);
        }

        private async Task<BodyContenteDTOC4Municipio> BodyContentFormaC4MuniMasiva(string getUserId)
        {
            BodyContenteDTOC4Municipio _contentM = new BodyContenteDTOC4Municipio();
            int rows = Excel.Rows();
            for (var row = 2; row <= rows; row++)
            {
                DetalleJsonC4Municipio itemDetalleJsonC4Municipio = new DetalleJsonC4Municipio()
                {
                    ANIO = ValidateCell("s", Excel.getCell(row, 1), "ANIO", row),
                    MES = ValidateCell("s", Excel.getCell(row, 2), "MES", row),
                    PDEN_ID = ValidateCell("s", Excel.getCell(row, 3), "PDEN_ID", row),
                    PDEN_NAME = ValidateCell("snull", Excel.getCell(row, 4), "PDEN_NAME", row),
                    ID_CAMPO = ValidateCell("snull", Excel.getCell(row, 5), "ID_CAMPO", row),
                    CAMPO = ValidateCell("snull", Excel.getCell(row, 6), "CAMPO", row),
                    CONTRATO = ValidateCell("snull", Excel.getCell(row, 7), "CONTRATO", row),
                    MUNICIPIO = ValidateCell("nnnull", Excel.getCell(row, 8), "MUNICIPIO", row),
                    PRODUCCIONBASICA = ValidateCell("nnnull", Excel.getCell(row, 9), "PRODUCCIONBASICA", row),
                    PRODUCCIONINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 10), "PRODUCCIONINCREMENTAL", row),
                    PRODUCCIONTOTAL = ValidateCell("nnnull", Excel.getCell(row, 11), "PRODUCCIONTOTAL", row),
                    CONSUMOSBASICA = ValidateCell("nnnull", Excel.getCell(row, 12), "CONSUMOSBASICA", row),
                    CONSUMOSINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 13), "CONSUMOSINCREMENTAL", row),
                    CONSUMOSTOTAL = ValidateCell("nnnull", Excel.getCell(row, 14), "CONSUMOSTOTAL", row),
                    PERDIDASBASICA = ValidateCell("nnnull", Excel.getCell(row, 15), "PERDIDASBASICA", row),
                    PERDIDASINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 16), "PERDIDASINCREMENTAL", row),
                    PERDIDASTOTAL = ValidateCell("nnnull", Excel.getCell(row, 17), "PERDIDASTOTAL", row),
                    GRAVABLEBASICA = ValidateCell("nnnull", Excel.getCell(row, 18), "GRAVABLEBASICA", row),
                    GRAVABLEINCREMENTAL = ValidateCell("nnnull", Excel.getCell(row, 19), "GRAVABLEINCREMENTAL", row),
                    GRAVABLETOTAL = ValidateCell("nnnull", Excel.getCell(row, 20), "GRAVABLETOTAL", row),
                    ELIMINAR = (!string.IsNullOrEmpty(Excel.getCell(row, 21)) && (Excel.getCell(row, 21).ToUpper() == "X")),
                    APROBADO = true,
                    USERID = getUserId,
                    PDEN_XREF = ""
                };
                /*
                ValidarValor("Producción", "Municipio", row, Excel.getCell(row, 9), Excel.getCell(row, 10), Excel.getCell(row, 11));
                ValidarValor("Consumos", "Municipio", row, Excel.getCell(row, 12), Excel.getCell(row, 13), Excel.getCell(row, 14));
                ValidarValor("Pérdidas", "Municipio", row, Excel.getCell(row, 15), Excel.getCell(row, 16), Excel.getCell(row, 17));
                ValidarValor("Gravable", "Municipio", row, Excel.getCell(row, 18), Excel.getCell(row, 19), Excel.getCell(row, 20));
                */
                DetalleJsonC4Muni.Add(itemDetalleJsonC4Municipio);
            }
            _contentM.RespBDP = DetalleJsonC4Muni;
            return await Task.Run(() => _contentM);
        }

        private double ValidarValor(string mensvar, string hoja, int i, string valorBase, string valorIncremental, string ValorTotal)
        {
            double resultado = 0;
            dynamic rBase = 0, rIncre = 0, rTotal = 0;

            if (!string.IsNullOrEmpty(valorBase))
            {
                try
                {
                    rBase = Convert.ToDouble(valorBase);
                }
                catch
                {
                    rBase = 0;
                }
            }
            if (!string.IsNullOrEmpty(valorIncremental))
            {
                try
                {
                    rIncre = Convert.ToDouble(valorIncremental);
                }
                catch
                {
                    rIncre = 0;
                }

            }
            if (!string.IsNullOrEmpty(ValorTotal))
            {
                try
                {
                    rTotal = Convert.ToDouble(ValorTotal);
                }
                catch
                {
                    rTotal = 0;
                }
            }

            if ((rBase + rIncre) !=  rTotal)
            {  
                //dynamic nSuma = rBase + rIncre;
                var error = $"Verificar que la información diligenciada en la hoja [{hoja}] para las columnas de  {mensvar} que contienen volumetria en Básica [{rBase}] e Incremental [{rIncre}] = [{rBase + rIncre}] no concuerdan con la columna Total [{rTotal}], validar la fila {i}";
                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                ExcelError.state = true;
                ErrorsList.Add(error);
            }
            return resultado;
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

                if (type == "nnull")   // valida pero con valores numericos con un rango si llega nulo lo deja pasar
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

                if (type == "nnnull")   // valida pero con valores numericos con un rango si llega nulo lo deja pasar
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        Cell = "0";
                    }
                    else
                    {
                        try
                        {
                            float value = float.Parse(Cell);
                            double valor = 0;
                            string cvalor = Cell.ToString();
                            valor = double.Parse(cvalor.Replace(cSeparadorOrigen, cSeparadorDestino));
                            cvalor= cvalor.Replace(cSeparadorOrigen, cSeparadorDestino);

                            if (valor.ToString() != Cell.ToString() && cSeparadorOrigen == cSeparadorDestino)
                            {
                                var error = $"El valor del campo {variable} debe ser un valor numerico, validar en la linea {i} la propiedad de decimales";
                                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                ExcelError.state = true;
                                ErrorsList.Add(error);
                            }
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
                                res = cvalor.ToString();  // se ajusta al valor oficial del sistema 
                                    //value.ToString();
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

                if (type == "sinnull")   // valida pero con valores numericos con un rango si llega nulo lo deja pasar
                {
                    if (string.IsNullOrEmpty(Cell))
                    {
                        var error = $"El valor del campo {Cell} debe ser un valor numerico y no puede ser nulo , validar en la linea {i} la propiedad de decimales";
                        ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                        ExcelError.state = true;
                        ErrorsList.Add(error);
                    }
                    else
                    {
                        try
                        {
                            float value = float.Parse(Cell);
                            double valor = 0;
                            string cvalor = Cell.ToString();
                            valor = double.Parse(cvalor.Replace(cSeparadorOrigen, cSeparadorDestino));
                            if (valor.ToString() != Cell.ToString() && cSeparadorOrigen == cSeparadorDestino)
                            {
                                var error = $"El valor del campo {Cell} debe ser un valor numerico, validar en la linea {i} la propiedad de decimales";
                                ExcelError.message = "Verifique y corrija  la siguiente lista de errores";
                                ExcelError.state = true;
                                ErrorsList.Add(error);
                            }
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

        public async Task<ResponseBase<dynamic>> Create(RequestCreateFormaC4Masiva data, DateTime date, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();

            try
            {
                // var header = data.Header9Masiva;
                // var body = data.Detail9Masiva;
                // var total = data.Totalacumulados;
                CultureInfo cultureInfo = new CultureInfo("es-co");

                var formC4 = new BodyContenteDTOC4Masiva
                {
                    Workbook = "FORMA C4"
                };
                try
                {
                    await _repository.CreateFormC4Masiva(formC4);
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

        private async Task ValidaContexto(string fileName, Stream file, string getUserId)
        {
            string cmessaje = "";
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };
            JsonC4ValBDP.REGISTRO = DetalleJsonC4Masiva;
            var resFormaC4 = DetalleJsonC4Masiva.ToList();
            var json = JsonC4ValBDP.Serialize(settings);
            DetalleJsonC4Muni.Serialize(settings);
            try
            {
                var res = await _repository.SqlValidarFormas(json);
                //await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });
                int codes = res.Code;
                cmessaje = res.Message;
                //int codes = 200;
                //cmessaje = "";
                if (codes == 200)
                {
                    var dataService = res.Data.Deserialize<ResponseForm9>(settings);

                    //var Data = "{\"FORMA\":\"FORMA_09\",\"REGISTROS\":[{\"LLAVE\":\"39674\",\"VALIDACIONES\":[{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":0,\"Mensaje\":\"OK\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":\"FN\",\"Tipo\":1},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 458785.97  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 1504786.1  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 571.41  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]},{\"LLAVE\":\"39675\",\"VALIDACIONES\":[{\"Columna\":\"BSW\",\"Regla\":\"006001\",\"Codigo\":1,\"Mensaje\":\"BSW = 120 no cumple con los rangos definidos\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":0,\"Mensaje\":\"OK\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":\"FN\",\"Tipo\":1},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 12.427  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]},{\"LLAVE\":\"143320\",\"VALIDACIONES\":[{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":0,\"Mensaje\":\"OK\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":\"FN\",\"Tipo\":1},{\"Columna\":\"UWI\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato UWI = FLOREÑA no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"UWI\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"POZO\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato POZO = FLOREÑA A-1XST 1Z no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"WELL_NAME\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 200  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]},{\"LLAVE\":\"102834\",\"VALIDACIONES\":[{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":1,\"Mensaje\":\"Tipo METODOPROD = XX no se encuentra homologado.  Ver PPDM_MAP_DETAIL\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"UWI\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato UWI = FLOREÑA no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"UWI\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"POZO\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato POZO = FLOREÑA A-1XST no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"WELL_NAME\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"PDEN_ID\",\"Regla\":\"006004\",\"Codigo\":1,\"Mensaje\":\"Error validando dato CAMPO según consulta para PDEN_ID. ORA-00904:invalid identifier. Dato PDEN_ID = 102834  no relacionado con CAMPO = FLOREÑA, Ver PDEN_COMPONENT con TYPE=002.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 200  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]}]}";
                    //var dataService = Data.Deserialize<ResponseForm9>(settings);
                    try
                    {
                        //Se utiliza la misma respuesta de Forma9Msiva para C4 
                        RespuestaForma RespFormas = JsonSerializer.Deserialize<RespuestaForma>(res.Data);
                        //RespuestaForma RespFormas = JsonSerializer.Deserialize<RespuestaForma>(Data);
                        string cRegla = string.Empty;
                        string cMensa = string.Empty;
                        string cHomol = string.Empty;
                        string cColumna = string.Empty;
                        foreach (var itemPden in RespFormas.REGISTROS)
                        {
                            List<REG_MENSAJE> listError = new List<REG_MENSAJE>();
                            if (itemPden.LLAVE != "" && itemPden.LLAVE != null)
                            {
                                foreach (var itemValidacion in itemPden.VALIDACIONES)
                                {
                                    cMensa = string.Empty;
                                    cRegla = string.Empty;
                                    cColumna = string.Empty;
                                    for (int itemCol = 1; itemCol < Celdas_Excel.Count(); itemCol++)
                                    {
                                        if (itemValidacion.Columna != null)
                                        {
                                            if (itemValidacion.Columna.ToString().ToUpper() == Celdas_Excel[itemCol].Titulos.ToUpper())
                                            {
                                                if (itemValidacion.Codigo == 1)  // Estado del registro si es 1 Es Error 0 no es error
                                                {
                                                    if (!string.IsNullOrEmpty(itemValidacion.Mensaje.ToString()))
                                                    {
                                                        cMensa = itemValidacion.Mensaje.ToString();

                                                        if (!string.IsNullOrEmpty(itemValidacion.Tipo.ToString()))
                                                        {
                                                            if (itemValidacion.Tipo.ToString() == "1")
                                                            {
                                                                resFormaC4.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { u.APROBADO = false; u.USERID = getUserId; return u; }).ToList();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    cMensa = itemValidacion.Mensaje.ToString();
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
                                                    resFormaC4.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u =>
                                                    {
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "PDEN_ID") { u.PDEN_ID = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "MODALIDAD") { u.MODALIDAD = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "ID_CAMPO") { u.ID_CAMPO = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "CAMPO") { u.CAMPO = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "PDEN_NAME") { u.PDEN_NAME = cHomol; }
                                                        return u;
                                                    }).ToList();
                                                }
                                                if (!String.IsNullOrEmpty(itemValidacion.Regla.ToString()))
                                                {
                                                    cRegla = itemValidacion.Regla;
                                                    cColumna = itemValidacion.Columna;
                                                }
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
                                    resFormaC4.Where(a => a.PDEN_ID == itemPden.LLAVE).Select(a =>
                                    {
                                        a.REG_MENSAJES = listError;
                                        return a;
                                    }).ToList();
                                }
                            }
                            else
                            {
                                string cCampo = string.Empty;
                                resFormaC4.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { cCampo = u.CAMPO; return u; }).ToList();
                                ErrorsList.Add($"No se encontrarón registros en la Base de Datos tipo PDEN_ID {itemPden.LLAVE} relacionada al Campo {cCampo}");
                                ExcelError.state = true;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        TelemetryException.RegisterException(e);
                    }
                    if (ErrorsList.Count == 0)
                    {
                        try
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
                        catch (Exception e)
                        {
                            TelemetryException.RegisterException(e);
                            ErrorsList.Add($"No se pudo procesar la carga de Archivo,  de la forma C4 Hoja [CUADRO 4] en el repositrio de datos, verifique e intente nuevamente [Validar si existe conexion GRPC Commons] => " + e.Message);
                            ExcelError.state = true;
                        }
                    }
                }
                if (codes != 200)
                {
                    ErrorsList.Add($"No se pudo procesar la validación en BDP de las configuraciones asociadas a la forma C4 que se esta cargando, verifique e intente nuevamente, " + cmessaje);
                    ExcelError.state = true;
                }
            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
                ErrorsList.Add($"No se pudo procesar la validación en la base de datos BDP, de la forma C4 hoja [CUADRO 4] que se esta cargando, verifique e intente nuevamente [Validar si existe conexion a BDP] => " + e.Message);
                ExcelError.state = true;
            }
            //-- Carga de Mensajes por cada Detlla de la forma
            foreach (var item in resFormaC4)
            {
                var RegDatosSQL = new DetalleJsonC4Masiva
                {
                    ANIO = item.ANIO,
                    MES = item.MES,
                    PDEN_ID = item.PDEN_ID,
                    PDEN_NAME = item.PDEN_NAME,
                    ID_CAMPO = item.ID_CAMPO,
                    CAMPO = item.CAMPO,
                    CONTRATO = item.CONTRATO,
                    MODALIDAD = item.MODALIDAD,
                    EXISTENCIAINICIAL = item.EXISTENCIAINICIAL,
                    PRODUCCIONBASICA = item.PRODUCCIONBASICA,
                    PRODUCCIONINCREMENTAL = item.PRODUCCIONINCREMENTAL,
                    PRODUCCIONTOTAL = item.PRODUCCIONTOTAL,
                    CONSUMOSBASICA = item.CONSUMOSBASICA,
                    CONSUMOSINCREMENTAL = item.CONSUMOSINCREMENTAL,
                    CONSUMOSTOTAL = item.CONSUMOSTOTAL,
                    PERDIDASBASICA = item.PERDIDASBASICA,
                    PERDIDASINCREMENTAL = item.PERDIDASINCREMENTAL,
                    PERDIDASTOTAL = item.PERDIDASTOTAL,
                    GRAVABLEBASICA = item.GRAVABLEBASICA,
                    GRAVABLEINCREMENTAL = item.GRAVABLEINCREMENTAL,
                    GRAVABLETOTAL = item.GRAVABLETOTAL,
                    OTRASENTRADAS = item.OTRASENTRADAS,
                    ENTREGAS = item.ENTREGAS,
                    EXISTENCIAFINAL = item.EXISTENCIAFINAL,
                    LLENADOLINEAS = item.LLENADOLINEAS,
                    LLENADOVASIJAS = item.LLENADOVASIJAS,
                    OTRASPERDIDAS = item.OTRASPERDIDAS,
                    OTROSCONSUMOS = item.OTROSCONSUMOS,
                    GRAVEDADAPI = item.GRAVEDADAPI,
                    CONTENIDOAZUFRE = item.CONTENIDOAZUFRE,
                    BSW = item.BSW,
                    GRAVEDADESPECIFICA = item.GRAVEDADESPECIFICA,
                    CONTENIDOSAL = item.CONTENIDOSAL,
                    APROBADO = item.APROBADO,
                    USERID = item.USERID,
                    OPERADOR = item.OPERADOR,
                    ELIMINAR = item.ELIMINAR,
                    REG_MENSAJES = item.REG_MENSAJES
                };
                _RespBDPJson.Add(RegDatosSQL);
            }
        }

        private async Task ValidaContextoMunicipio(string fileName, Stream file, string getUserId)
        {
            string cmessaje = "";
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            JsonC4ValMuniBDP.REGISTRO = DetalleJsonC4Muni;
            var resFormaC4Muni = DetalleJsonC4Muni.ToList();
            var json = JsonC4ValMuniBDP.Serialize(settings);
            DetalleJsonC4Muni.Serialize(settings);
            try
            {
                var res = await _repository.SqlValidarFormas(json);
                //await FormasService.ValidarFormasMinAsync(new Ppdm.RequestBase { SJson = json });
                int codes = res.Code;
                cmessaje = res.Message;
                //int codes = 200;
                //cmessaje = "";
                if (codes == 200)
                {
                    var dataService = res.Data.Deserialize<ResponseForm9>(settings);

                    //var Data = "{\"FORMA\":\"FORMA_09\",\"REGISTROS\":[{\"LLAVE\":\"39674\",\"VALIDACIONES\":[{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":0,\"Mensaje\":\"OK\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":\"FN\",\"Tipo\":1},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 458785.97  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 1504786.1  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 571.41  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]},{\"LLAVE\":\"39675\",\"VALIDACIONES\":[{\"Columna\":\"BSW\",\"Regla\":\"006001\",\"Codigo\":1,\"Mensaje\":\"BSW = 120 no cumple con los rangos definidos\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":0,\"Mensaje\":\"OK\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":\"FN\",\"Tipo\":1},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 12.427  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]},{\"LLAVE\":\"143320\",\"VALIDACIONES\":[{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":0,\"Mensaje\":\"OK\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":\"FN\",\"Tipo\":1},{\"Columna\":\"UWI\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato UWI = FLOREÑA no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"UWI\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"POZO\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato POZO = FLOREÑA A-1XST 1Z no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"WELL_NAME\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 200  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]},{\"LLAVE\":\"102834\",\"VALIDACIONES\":[{\"Columna\":\"METODOPROD\",\"Regla\":\"006002\",\"Codigo\":1,\"Mensaje\":\"Tipo METODOPROD = XX no se encuentra homologado.  Ver PPDM_MAP_DETAIL\",\"ColumnaBDP\":\"PRODUCTION_METHOD\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"UWI\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato UWI = FLOREÑA no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"UWI\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"POZO\",\"Regla\":\"006003\",\"Codigo\":1,\"Mensaje\":\"Dato POZO = FLOREÑA A-1XST no se encuentra homologado.  Ver WELL_ALIAS\",\"ColumnaBDP\":\"WELL_NAME\",\"DatoBDP\":null,\"Tipo\":1},{\"Columna\":\"PDEN_ID\",\"Regla\":\"006004\",\"Codigo\":1,\"Mensaje\":\"Error validando dato CAMPO según consulta para PDEN_ID. ORA-00904:invalid identifier. Dato PDEN_ID = 102834  no relacionado con CAMPO = FLOREÑA, Ver PDEN_COMPONENT con TYPE=002.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"PETROLEOACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para PETROLEOACUM.. Dato PETROLEOACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"GASACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para GASACUM.. Dato GASACUM = 0  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0},{\"Columna\":\"AGUAACUM\",\"Regla\":\"006006\",\"Codigo\":1,\"Mensaje\":\"No se encontraron datos para MES según consulta para AGUAACUM.. Dato AGUAACUM = 200  no corresponde con período anterior + 0, Ver ADMINBDP.PDEN_VOL_SUMMARY.\",\"ColumnaBDP\":null,\"DatoBDP\":null,\"Tipo\":0}]}]}";
                    //var dataService = Data.Deserialize<ResponseForm9>(settings);
                    try
                    {
                        //Se utiliza la misma respuesta de Forma9Msiva para C4 
                        RespuestaForma RespFormas = JsonSerializer.Deserialize<RespuestaForma>(res.Data);
                        //RespuestaForma RespFormas = JsonSerializer.Deserialize<RespuestaForma>(Data);
                        string cRegla = string.Empty;
                        string cMensa = string.Empty;
                        string cHomol = string.Empty;
                        foreach (var itemPden in RespFormas.REGISTROS)
                        {
                            List<REG_MENSAJE> listError = new List<REG_MENSAJE>();
                            if (itemPden.LLAVE != "" && itemPden.LLAVE != null)
                            {
                                string sLLaveCompuesta = itemPden.LLAVE;

                                string[] aLLave = sLLaveCompuesta.Split("::");
                                string cPden = aLLave[0];
                                string cMuni = aLLave[1];
                                string cColumna = string.Empty;
                                foreach (var itemValidacion in itemPden.VALIDACIONES)
                                {
                                    cMensa = string.Empty;
                                    cRegla = string.Empty;
                                    cColumna = string.Empty;
                                    for (int itemCol = 1; itemCol < Celdas_Excel_Muni.Count(); itemCol++)
                                    {
                                        if (itemValidacion.Columna != null)
                                        {
                                            if (itemValidacion.Columna.ToString().ToUpper() == Celdas_Excel_Muni[itemCol].Titulos.ToUpper())
                                            {
                                                if (itemValidacion.Codigo == 1)  // Estado del registro si es 1 Es Error 0 no es error
                                                {
                                                    if (!string.IsNullOrEmpty(itemValidacion.Mensaje.ToString()))
                                                    {
                                                        cMensa = itemValidacion.Mensaje.ToString();

                                                        if (!string.IsNullOrEmpty(itemValidacion.Tipo.ToString()))
                                                        {
                                                            if (itemValidacion.Tipo.ToString() == "1")
                                                            {
                                                                // resFormaC4Muni.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { u.APROBADO = false; u.USERID = getUserId; return u; }).ToList();
                                                                resFormaC4Muni.Where(u => u.PDEN_ID == cPden && u.MUNICIPIO == cMuni ).Select(u => { u.APROBADO = false; u.USERID = getUserId; return u; }).ToList();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    cMensa = itemValidacion.Mensaje.ToString();
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
                                                {   //u.PDEN_ID == itemPden.LLAVE 
                                                    resFormaC4Muni.Where(u =>  u.PDEN_ID == cPden && u.MUNICIPIO == cMuni ).Select(u =>
                                                    {
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "PDEN_ID") { u.PDEN_ID = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "ID_CAMPO") { u.ID_CAMPO = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "CAMPO") { u.CAMPO = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "PDEN_NAME") { u.PDEN_NAME = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "PDEN_XREF") { u.PDEN_XREF = cHomol; }
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
                                    resFormaC4Muni.Where(a => a.PDEN_ID == cPden && a.MUNICIPIO == cMuni).Select(a =>
                                    {
                                        a.REG_MENSAJES = listError;
                                        return a;
                                    }).ToList();
                                }
                            }
                            else
                            {
                                string cCampo = string.Empty;
                                resFormaC4Muni.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { cCampo = u.CAMPO; return u; }).ToList();
                                ErrorsList.Add($"No se encontrarón registros en la Base de Datos tipo PDEN_ID {itemPden.LLAVE} relacionada al Campo {cCampo} de la hoja [MUNICIPIO]");
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
                    ErrorsList.Add($"No se pudo procesar la validación en BDP de las configuraciones asociadas a la forma C4 que se esta cargando, verifique e intente nuevamente, " + cmessaje);
                    ExcelError.state = true;
                }
            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
                ErrorsList.Add($"No se pudo procesar la validación en la base de datos BDP, de la forma C4 [MUNICIPIO] que se esta cargando, verifique e intente nuevamente [Validar si existe conexion a BDP] => " + cmessaje);
                ExcelError.state = true;
            }
            //-- Carga de Mensajes por cada Detlla de la forma
            foreach (var item in resFormaC4Muni)
            {
                var RegDatosSQLMuni = new DetalleJsonC4Municipio
                {
                    ANIO = item.ANIO,
                    MES = item.MES,
                    PDEN_ID = item.PDEN_ID,
                    PDEN_NAME = item.PDEN_NAME,
                    ID_CAMPO = item.ID_CAMPO,
                    CAMPO = item.CAMPO,
                    CONTRATO = item.CONTRATO,
                    MUNICIPIO = item.MUNICIPIO,
                    PRODUCCIONBASICA = item.PRODUCCIONBASICA,
                    PRODUCCIONINCREMENTAL = item.PRODUCCIONINCREMENTAL,
                    PRODUCCIONTOTAL = item.PRODUCCIONTOTAL,
                    CONSUMOSBASICA = item.CONSUMOSBASICA,
                    CONSUMOSINCREMENTAL = item.CONSUMOSINCREMENTAL,
                    CONSUMOSTOTAL = item.CONSUMOSTOTAL,
                    PERDIDASBASICA = item.PERDIDASBASICA,
                    PERDIDASINCREMENTAL = item.PERDIDASINCREMENTAL,
                    PERDIDASTOTAL = item.PERDIDASTOTAL,
                    GRAVABLEBASICA = item.GRAVABLEBASICA,
                    GRAVABLEINCREMENTAL = item.GRAVABLEINCREMENTAL,
                    GRAVABLETOTAL = item.GRAVABLETOTAL,
                    APROBADO = item.APROBADO,
                    USERID = item.USERID,
                    ELIMINAR = item.ELIMINAR,
                    REG_MENSAJES = item.REG_MENSAJES
                };
                _RespBDPMuniJson.Add(RegDatosSQLMuni);
            }
        }
        public Task<string> GuardarFormaC4PPDM(List<DetalleJsonC4Masiva> Datos)
        {
            //registerForm 
            //Detail9Multiple
            string resultado = string.Empty;
            var resFormaC4 = Datos.ToList();

            List<JsonC4MSQL> SQL_F4Masiva = new List<JsonC4MSQL>();
            List<JsonC4MBDP> BDP_F4Masiva = new List<JsonC4MBDP>();
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };
            foreach (var item in resFormaC4)
            {
                string tempMensaje = string.Empty;
                int nVol = 0;

                try
                {
                    nVol = item.REG_MENSAJES.ToList().Count();
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

                var RegDatosSQL = new JsonC4MSQL
                {
                    Anio = item.ANIO,
                    Mes = item.MES,
                    Pden_Id = item.PDEN_ID,
                    Pden_Name = item.PDEN_NAME,
                    Id_Campo = item.ID_CAMPO,
                    Campo = item.CAMPO,
                    Contrato = item.CONTRATO,
                    Modalidad = item.MODALIDAD,
                    ExistenciaInicial = item.EXISTENCIAINICIAL,
                    ProduccionBasica = item.PRODUCCIONBASICA,
                    ProduccionIncremental = item.PRODUCCIONINCREMENTAL,
                    ProduccionTotal = item.PRODUCCIONTOTAL,
                    ConsumosBasica = item.CONSUMOSBASICA,
                    ConsumosIncremental = item.CONSUMOSINCREMENTAL,
                    ConsumosTotal = item.CONSUMOSTOTAL,
                    PerdidasBasica = item.PERDIDASBASICA,
                    PerdidasIncremental = item.PERDIDASINCREMENTAL,
                    PerdidasTotal = item.PERDIDASTOTAL,
                    GravableBasica = item.GRAVABLEBASICA,
                    GravableIncremental = item.GRAVABLEINCREMENTAL,
                    GravableTotal = item.GRAVABLETOTAL,
                    OtrasEntradas = item.OTRASENTRADAS,
                    Entregas = item.ENTREGAS,
                    ExistenciaFinal = item.EXISTENCIAFINAL,
                    LlenadoLineas = item.LLENADOLINEAS,
                    LlenadoVasijas = item.LLENADOVASIJAS,
                    OtrasPerdidas = item.OTRASPERDIDAS,
                    OtrosConsumos = item.OTROSCONSUMOS,
                    //2024-07-04 MFCE RF615276 Para estos campos se envian solo los primeros 10 caracteres, porque el campo en la tabla FORMAS.FOXT.FORM4_Masiva es NVARCHAR(10)
                    GravedadApi = item.GRAVEDADAPI.Length > 10 ? item.GRAVEDADAPI[..10] : item.GRAVEDADAPI,
                    ContenidoAzufre = item.CONTENIDOAZUFRE.Length > 10 ? item.CONTENIDOAZUFRE[..10] : item.CONTENIDOAZUFRE,
                    BSW = item.BSW.Length > 10 ? item.BSW[..10] : item.BSW,
                    GravedadEspecifica = item.GRAVEDADESPECIFICA.Length > 10 ? item.GRAVEDADESPECIFICA[..10] : item.GRAVEDADESPECIFICA,
                    ContenidoSal = item.CONTENIDOSAL.Length > 10 ? item.CONTENIDOSAL[..10] : item.CONTENIDOSAL,
                    Aprobado = item.APROBADO.ToString(),
                    CargadoaBDP = "true",
                    row_created_by = item.USERID,
                    row_created_date = DateTime.Now.ToString(),
                    row_changed_by = item.USERID,
                    row_changed_date = DateTime.Now.ToString(),
                    Json_MensajesErr = tempMensaje,
                    Operador = item.OPERADOR,
                    Eliminar = item.ELIMINAR.ToString()
                };
                SQL_F4Masiva.Add(RegDatosSQL);
                if (item.APROBADO == true)
                {

                    var RegDatosBDP = new JsonC4MBDP
                    {
                        ANIO = item.ANIO,
                        MES = item.MES,
                        PDEN_ID = item.PDEN_ID,
                        PDEN_NAME = item.PDEN_NAME,
                        ID_CAMPO = item.ID_CAMPO,
                        CAMPO = item.CAMPO,
                        CONTRATO = item.CONTRATO,
                        MODALIDAD = item.MODALIDAD,
                        PRODUCCIONBASICA = item.PRODUCCIONBASICA,
                        PRODUCCIONINCREMENTAL = item.PRODUCCIONINCREMENTAL,
                        PRODUCCIONTOTAL = item.PRODUCCIONTOTAL,
                        CONSUMOSBASICA = item.CONSUMOSBASICA,
                        CONSUMOSINCREMENTAL = item.CONSUMOSINCREMENTAL,
                        CONSUMOSTOTAL = item.CONSUMOSTOTAL,
                        PERDIDASBASICA = item.PERDIDASBASICA,
                        PERDIDASINCREMENTAL = item.PERDIDASINCREMENTAL,
                        PERDIDASTOTAL = item.PERDIDASTOTAL,
                        GRAVABLEBASICA = item.GRAVABLEBASICA,
                        GRAVABLEINCREMENTAL = item.GRAVABLEINCREMENTAL,
                        GRAVABLETOTAL = item.GRAVABLETOTAL,
                        OTRASENTRADAS = item.OTRASENTRADAS,
                        ENTREGAS = item.ENTREGAS,
                        EXISTENCIAFINAL = item.EXISTENCIAFINAL,
                        LLENADOLINEAS = item.LLENADOLINEAS,
                        LLENADOVASIJAS = item.LLENADOVASIJAS,
                        OTRASPERDIDAS = item.OTRASPERDIDAS,
                        OTROSCONSUMOS = item.OTROSCONSUMOS,
                        GRAVEDADAPI = item.GRAVEDADAPI,
                        CONTENIDOAZUFRE = item.CONTENIDOAZUFRE,
                        BSW = item.BSW,
                        GRAVEDADESPECIFICA = item.GRAVEDADESPECIFICA,
                        CONTENIDOSAL = item.CONTENIDOSAL,
                        APROBADO = item.APROBADO.ToString(),
                        CARGADOABDP = "true",
                        ROW_CREATED_BY = item.USERID,
                        ROW_CREATED_DATE = DateTime.Now.ToString(),
                        ROW_CHANGED_BY = item.USERID,
                        ROW_CHANGED_DATE = DateTime.Now.ToString(),
                        JSON_MENSAJESERR = tempMensaje,
                        ELIMINAR = item.ELIMINAR ? "1" : "0",
                        USERID = item.USERID
                    };
                    BDP_F4Masiva.Add(RegDatosBDP);
                }
            }


            string Bdp_Json = BDP_F4Masiva.Serialize(settings).ToString();
            resultado = "";
            bool lGuardarBdp = false;
            if (Bdp_Json.Count() > 0)
            {
                string EncJson = "{";
                EncJson += "\"FORMA_CODIGO\": \"4C\",";
                EncJson += "\"REGISTRO\": ";
                Bdp_Json = EncJson + Bdp_Json + "}";
                var respBdp = _repository.GuardaBdpforma4(Bdp_Json);
                string crSql = respBdp.Substring(respBdp.IndexOf("BDP_Respuesta:", 0) + 14, 1);
                lGuardarBdp = (crSql == "0");
                if (crSql == "1")
                {
                    int nDigito = 16;
                    int nDigitoFinal = 18;
                    respBdp = respBdp.Substring(nDigito, (respBdp.Length) - nDigitoFinal);  // quitar los caracteres especiales 
                    dynamic myORespBdp = JValue.Parse(respBdp);
                    var cForm = myORespBdp?.REGISTROS;
                    string cTabla = string.Empty;
                    string cMensa = string.Empty;
                    string cCodigo = string.Empty;
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
                                foreach (var UpdSQL in SQL_F4Masiva)
                                {
                                    if (UpdSQL.Pden_Id.ToString() == itemPden.LLAVE.ToString())
                                    {
                                        string cOldValue = UpdSQL.Json_MensajesErr.ToString().Replace("}]", "}").Replace("[{", "{");
                                        string cNewMensaje = "[" + cMensaNuevo + (string.IsNullOrEmpty(cOldValue) ? "" : ", ") + cOldValue + "]";
                                        SQL_F4Masiva.Where(u => u.Pden_Id == itemPden.LLAVE.ToString()).Select(u => { u.Json_MensajesErr = cNewMensaje; return u; }).ToList();
                                    }
                                }
                            }
                        }
                    }

                }
                resultado = "BDP_Respuesta:" + (lGuardarBdp ? "0;" : "1;") + respBdp + ".;";
            }
            string Sql_Json = SQL_F4Masiva.Serialize(settings).ToString();
            if ((Sql_Json.Count() > 0))  //Pasa si pasa BDP 
            {
                var respSql = _repository.Guardasqlforma4(Sql_Json);
                resultado = respSql + resultado;
            }
            return Task.Run(() => resultado);
        }

        public Task<string> GuardarFormaC4PPDMuni(List<DetalleJsonC4Municipio> Datos)
        {
            //registerForm 
            //Detail9Multiple
            string resultado = string.Empty;
            var resFormaC4 = Datos.ToList();

            List<JsonC4MSQLMuni> SQL_F4MasivaMuni = new List<JsonC4MSQLMuni>();
            List<JsonC4MBDPMuni> BDP_F4MasivaMuni = new List<JsonC4MBDPMuni>();
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };
            foreach (var item in resFormaC4)
            {
                string tempMensaje = string.Empty;
                int nVol = 0;

                try
                {
                    nVol = item.REG_MENSAJES.ToList().Count();
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

                var RegDatosSQL = new JsonC4MSQLMuni
                {
                    Anio = item.ANIO,
                    Mes = item.MES,
                    Pden_Id = item.PDEN_ID,
                    Pden_Name = item.PDEN_NAME,
                    Id_Campo = item.ID_CAMPO,
                    Campo = item.CAMPO,
                    Contrato = item.CONTRATO,
                    Monicipio = item.MUNICIPIO,
                    ProduccionBasica = item.PRODUCCIONBASICA,
                    ProduccionIncremental = item.PRODUCCIONINCREMENTAL,
                    ProduccionTotal = item.PRODUCCIONTOTAL,
                    ConsumosBasica = item.CONSUMOSBASICA,
                    ConsumosIncremental = item.CONSUMOSINCREMENTAL,
                    ConsumosTotal = item.CONSUMOSTOTAL,
                    PerdidasBasica = item.PERDIDASBASICA,
                    PerdidasIncremental = item.PERDIDASINCREMENTAL,
                    PerdidasTotal = item.PERDIDASTOTAL,
                    GravableBasica = item.GRAVABLEBASICA,
                    GravableIncremental = item.GRAVABLEINCREMENTAL,
                    GravableTotal = item.GRAVABLETOTAL,
                    Aprobado = item.APROBADO.ToString(),
                    CargadoaBDP = "true",
                    row_created_by = item.USERID,
                    row_created_date = DateTime.Now.ToString(),
                    row_changed_by = item.USERID,
                    row_changed_date = DateTime.Now.ToString(),
                    Json_MensajesErr = tempMensaje,
                    Eliminar = item.ELIMINAR.ToString()
                };
                SQL_F4MasivaMuni.Add(RegDatosSQL);
                if (item.APROBADO == true)
                {

                    var RegDatosBDP = new JsonC4MBDPMuni
                    {
                        ANIO = item.ANIO,
                        MES = item.MES,
                        PDEN_ID = item.PDEN_ID,
                        PDEN_NAME = item.PDEN_NAME,
                        ID_CAMPO = item.ID_CAMPO,
                        CAMPO = item.CAMPO,
                        CONTRATO = item.CONTRATO,
                        MUNICIPIO = item.MUNICIPIO,
                        PRODUCCIONBASICA = item.PRODUCCIONBASICA,
                        PRODUCCIONINCREMENTAL = item.PRODUCCIONINCREMENTAL,
                        PRODUCCIONTOTAL = item.PRODUCCIONTOTAL,
                        CONSUMOSBASICA = item.CONSUMOSBASICA,
                        CONSUMOSINCREMENTAL = item.CONSUMOSINCREMENTAL,
                        CONSUMOSTOTAL = item.CONSUMOSTOTAL,
                        PERDIDASBASICA = item.PERDIDASBASICA,
                        PERDIDASINCREMENTAL = item.PERDIDASINCREMENTAL,
                        PERDIDASTOTAL = item.PERDIDASTOTAL,
                        GRAVABLEBASICA = item.GRAVABLEBASICA,
                        GRAVABLEINCREMENTAL = item.GRAVABLEINCREMENTAL,
                        GRAVABLETOTAL = item.GRAVABLETOTAL,
                        APROBADO = item.APROBADO.ToString(),
                        PDEN_XREF = item.PDEN_XREF,
                        JSON_MENSAJESERR = tempMensaje,
                        ELIMINAR = item.ELIMINAR ? "1" : "0",
                        USERID = item.USERID
                    };
                    BDP_F4MasivaMuni.Add(RegDatosBDP);
                }
            }


            string Bdp_Json = BDP_F4MasivaMuni.Serialize(settings).ToString();
            resultado = "";
            bool lGuardarBdp = false;
            if (Bdp_Json.Count() > 0)
            {
                string EncJson = "{";
                EncJson += "\"FORMA_CODIGO\": \"4M\",";
                EncJson += "\"REGISTRO\": ";
                Bdp_Json = EncJson + Bdp_Json + "}";
                var respBdp = _repository.GuardaBdpforma4(Bdp_Json);
                string crSql = respBdp.Substring(respBdp.IndexOf("BDP_Respuesta:", 0) + 14, 1);
                lGuardarBdp = (crSql == "0");
                if (crSql == "1")
                {
                    int nDigito = 16;
                    int nDigitoFinal = 18;
                    respBdp = respBdp.Substring(nDigito, (respBdp.Length) - nDigitoFinal);  // quitar los caracteres especiales 
                    dynamic myORespBdp = JValue.Parse(respBdp);
                    var cForm = myORespBdp?.REGISTROS;
                    string cTabla = string.Empty;
                    string cMensa = string.Empty;
                    string cCodigo = string.Empty;
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
                                    cColumna = itemErrValidacion.Columna.ToString().ToUpper();
                                    REG_MENSAJE itemlistValError = new REG_MENSAJE()   // compatibilida con tabla de mensajes 
                                    {
                                        DETALLEMENSAJE = cMensa,
                                        REGLA = itemErrValidacion.Tabla.ToString(),
                                        COLUMNA = cColumna
                                    };
                                    listValError.Add(itemlistValError);
                                }
                            }
                            /* Buscar En el JSON Anterior*/
                            if (listValError.Count > 0)
                            {
                                var jsonc = listValError.Serialize(settings);
                                string cMensaNuevo = jsonc.ToString().Replace("}]", "}").Replace("[{", "{");
                                foreach (var UpdSQL in SQL_F4MasivaMuni)
                                {
                                    if (UpdSQL.Pden_Id.ToString() == itemPden.LLAVE.ToString())
                                    {
                                        string cOldValue = UpdSQL.Json_MensajesErr.ToString().Replace("}]", "}").Replace("[{", "{");
                                        string cNewMensaje = "[" + cMensaNuevo + (string.IsNullOrEmpty(cOldValue) ? "" : ", ") + cOldValue + "]";
                                        SQL_F4MasivaMuni.Where(u => u.Pden_Id == itemPden.LLAVE.ToString()).Select(u => { u.Json_MensajesErr = cNewMensaje; return u; }).ToList();
                                    }
                                }
                            }
                        }
                    }

                }
                resultado = "BDP_Respuesta:" + (lGuardarBdp ? "0;" : "1;") + respBdp + ".;";
            }
            string Sql_Json = SQL_F4MasivaMuni.Serialize(settings).ToString();
            if ((Sql_Json.Count() > 0)) //&& (!lGuardarBdp))  //Pasa si pasa BDP 
            {
                var respSql = _repository.Guardasqlforma4Muni(Sql_Json);
                resultado = respSql + resultado;
            }
            return Task.Run(() => resultado);
        }
        public string StringBetween(string Source, string Start, string End)
        {
            return _repository.StringBetween(Source, Start, End);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAnio"></param>
        /// <param name="PMes"></param>
        /// <param name="PTodo"></param>
        /// <returns></returns>
        public Task<string> consultaformaC4(int pAnio, int PMes, int PTodo)
        {
            string respuesta = _repository.ConsultarformaC4(pAnio, PMes, PTodo);

            return Task.Run(() => respuesta);
        }
        public Task<string> consultaformaC4Muni(int pAnio, int PMes, int PTodo)
        {
            string respuesta = _repository.ConsultarformaC4Muni(pAnio, PMes, PTodo);

            return Task.Run(() => respuesta);
        }

    }
}
