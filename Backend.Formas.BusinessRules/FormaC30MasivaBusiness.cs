
using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.DTO.Validaciones;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Responses;
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
using OfficeOpenXml;

namespace Backend.Formas.BusinessRules
{
    public class FormaC30MasivaBusiness : IFormasC30MasivaBusiness
    {

        private readonly List<BodyContenteDTOC30Masiva> content = new List<BodyContenteDTOC30Masiva>();

        private readonly IImportarExcel Excel;
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        private string _fileName = "";

        private readonly ExcelError ExcelError = new ExcelError();
        private readonly List<string> ErrorsList = new List<string>();
        readonly List<CeldasExcel> Celdas_Excel = new List<CeldasExcel>();
        private readonly BodyContenteDTOC30Masiva bodyForma = new BodyContenteDTOC30Masiva();


        private readonly FormaC30MasivaStructureDTO FormaC30Respose = new FormaC30MasivaStructureDTO();
        private readonly HeaderFormaC30MasivaDTO Header = new HeaderFormaC30MasivaDTO();

        private readonly JsonC30Masivo JsonC30ValBDP = new JsonC30Masivo();

        private readonly List<DetalleJsonC30Masiva> DetalleJsonC30Masiva = new List<DetalleJsonC30Masiva>();

        private readonly List<DetalleJsonC30Masiva> _RespBDPJson = new List<DetalleJsonC30Masiva>();

        private readonly IFormaC30MasivaRepository _repository;
        

        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly Commons.CommonGrpc.CommonGrpcClient CommonService;
        private readonly Backend.Formas.Utilities.SendMail.ISendMailService SendMailService;
        private readonly Administracion.AdmGrpc.AdmGrpcClient AdministracionService;
        private readonly IAdministracionFormas _repositoryAdministracion;


        public FormaC30MasivaBusiness(
            IImportarExcel excel,
             Utilities.Telemetry.ITelemetryException telemetryException,
            IFormaC30MasivaRepository _repo,
            Ppdm.PpdmGrpc.PpdmGrpcClient formasService,
            Commons.CommonGrpc.CommonGrpcClient commonService,
            Backend.Formas.Utilities.SendMail.ISendMailService sendMailService,
            Administracion.AdmGrpc.AdmGrpcClient administracionService,
            IAdministracionFormas admin
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
        }



        public async Task<ResponseBase<FormaC30MasivaStructureDTO>> UploadFile(DateTime date, Stream files, string fileName, string getUserId)
        {

            var response = new ResponseBase<FormaC30MasivaStructureDTO>();
            var lHoja = false;
            Excel.setFile(files);
            Excel.reader("FORMA 30");

            using (var package = new ExcelPackage(files)) // new FileInfo(fileName)))
            {
                lHoja =  package.Workbook.Worksheets.Any(sheet => sheet.Name.ToUpper() == "FORMA 30");
            }

            if (!lHoja)
            {
                ExcelError.state = true;
                ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido en la Hoja [FORMA 30]";
                FormaC30Respose.Errors = ExcelError;
                FormaC30Respose.Data = content; // DetalleJsonC4Masiva;
            }

            if (Excel.isLoad() && lHoja)
            {
                _fileName = fileName;
                var rows = Excel.Rows();
                if (rows <= 1)
                {
                    ExcelError.state = true;
                    ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido en la Hoja [FORMA 30]";
                    FormaC30Respose.Errors = ExcelError;
                    FormaC30Respose.Data = content; // DetalleJsonC4Masiva;
                }
                else if (rows > 0)
                {
                    //validamos datos de cabecera
                    bool lValiaEncabezado = ValidaEncabezadoExcel();
                    if (lValiaEncabezado)
                        {
                        await HeaderFormaC30Masiva(date, getUserId);

                        var detalle = await BodyContentFormaC30Masiva(getUserId);

                        await ValidaContexto(fileName, files, getUserId);

                        bodyForma.Header = Header;
                        bodyForma.RespBDP = DetalleJsonC30Masiva;
                        bodyForma.Workbook = "FORMA 30";

                        detalle.Header = Header;
                        detalle.RespBDP = _RespBDPJson;
                        detalle.Workbook = "FORMA 30";

                        content.Add(detalle);

                        if (ExcelError.state == false)
                        {
                            FormaC30Respose.Data = content; //DetalleJsonC30Masiva;
                            FormaC30Respose.Errors = ExcelError;
                        }

                        if (ExcelError.cargaExtemporal == true)
                        {
                            ExcelError.message = "La forma que intenta cargar se encuentra en un mes operativo anterior o esta forma ya ha sido aprobada";
                            ExcelError.header = Header;
                            FormaC30Respose.Data = content; // DetalleJsonC30Masiva;
                            FormaC30Respose.Errors = ExcelError;
                            ExcelError.state = true;
                        }

                        if (ExcelError.state == true) //&& ExcelError.cargaExtemporal == false)
                        {
                            ExcelError.message = "El archivo cargado tiene validaciones pendientes, verifique e intente de nuevo";
                            FormaC30Respose.Data = content; //DetalleJsonC30Masiva;
                            FormaC30Respose.Errors = ExcelError;
                        }
                    }
                }
                else
                {
                    ExcelError.state = true;
                    ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido de la Hoja [FORMA 30] e intente de nuevo";
                    FormaC30Respose.Errors = ExcelError;
                    FormaC30Respose.Data = content; // DetalleJsonC30Masiva;
                }
            }
            else
            {
                ExcelError.state = true;
                ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido de la Hoja [FORMA 30]";
                FormaC30Respose.Errors = ExcelError;
                FormaC30Respose.Data = content; // DetalleJsonC30Masiva;
            }
 
            ExcelError.listError = ErrorsList;

            response.Data = FormaC30Respose;

            return response;
        }

        private bool ValidaEncabezadoExcel()
        {
            bool lTitulo = false;
            int iCol = 50;
            string cValor = string.Empty;
            string[] TitulosHoja = { "", "ANIO", "MES", "OPERADOR", "CONTRATO","CAMPO", "PDEN_ID", "GASFORMACION", "CONTENIDOPROPANO",
                    "CONTENIDOBUTANO","CONTENIDOGASOLINANATURAL","GASFORMACIONPROCESADO", "CONSUMOCAMPO","GASODUCTOURBANO", "GENERACIONELECTRICA",
                    "OTROSCONSUMOS", "GASQUEMACAMPO","VENTEADOALAIRE", "GASBOMBEONEUMATICO", "GASINYECTADO", "TOTALGASPROCESADO",
                    "PRODUCCIONPROPANO", "PRODUCCIONBUTANO", "PRODUCCIONGASOLINA", "GASTRANSFORMADO", "CONSUMOENPLANTA",
                    "ENTREGAGASODUCTO", "ENTREGAGENERACION", "OTRASENTREGAS", "GASQUEMAPLANTA", "GASBOMBEOPLANTA",
                    "GASINYECTADOPLANTA", "OBSERVACIONES", "ELIMINAR" };
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
                        string cTitulErr = $"Hoja [FORMA 30], En la columna " + nCol.ToString() + " => " + cValor + " No corresponde el nombre de la columna, " + TitulosHoja[nCol];
                        ErrorsList.Add(cTitulErr);
                    }
                    else
                    {
                        lTitulo = true;
                    }
                }
            }
            if (lTitulo == false)
            {
                ExcelError.state = true;
                ExcelError.message = "El cargue solicitado no contiene información o no es el formato preestablecido en la Hoja [FORMA 30]";
                FormaC30Respose.Errors = ExcelError;
                FormaC30Respose.Data = content; // DetalleJsonC4Masiva;
            }
            return lTitulo;
        }

        private Task HeaderFormaC30Masiva(DateTime dateForma, string getUserId)
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
                string[] TitulosHoja = { "", "ANIO", "MES", "OPERADOR", "CONTRATO","CAMPO", "PDEN_ID", "GASFORMACION", "CONTENIDOPROPANO",
                    "CONTENIDOBUTANO","CONTENIDOGASOLINANATURAL","GASFORMACIONPROCESADO", "CONSUMOCAMPO", "GASODUCTOURBANO", "GENERACIONELECTRICA",
                    "OTROSCONSUMOS", "GASQUEMACAMPO","VENTEADOALAIRE", "GASBOMBEONEUMATICO", "GASINYECTADO", "TOTALGASPROCESADO",
                    "PRODUCCIONPROPANO", "PRODUCCIONBUTANO", "PRODUCCIONGASOLINA", "GASTRANSFORMADO", "CONSUMOENPLANTA",
                    "ENTREGAGASODUCTO", "ENTREGAGENERACION", "OTRASENTREGAS", "GASQUEMAPLANTA", "GASBOMBEOPLANTA",
                    "GASINYECTADOPLANTA", "OBSERVACIONES", "ELIMINAR" };
                

                /*
                 * Validacion inicial del archivo excel 
                 */
                Header.Mes = iMes;
                Header.Ano = iAnio;
                Header.FORMA_CODIGO = "30M";
                Header.MIEMBRO_ID = getUserId.ToString();
                Header.FileName = getUserId.ToString();
                Header.Url = getUserId.ToString();

                JsonC30ValBDP.MES = iMes;
                JsonC30ValBDP.ANIO = iAnio;
                JsonC30ValBDP.FORMA_CODIGO = "30M";

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
                            string cTitulErr = $"Hoja [FORMA 30], En la columna " + nCol.ToString() + " => " + cValor + " No corresponde el nombre de la columna, " + TitulosHoja[nCol];
                            ErrorsList.Add(cTitulErr);
                        }
                    }
                }
                for (var iCon = 2; iCon <= iFila; iCon++)
                {
                    dynamic colAnio = Excel.getCell(iCon, 1); // Carga informacion de la columna de Anio
                    dynamic colMes = Excel.getCell(iCon, 2);  // Carga informacion de la columna de Mes
                    string colPden = Excel.getCell(iCon, 6);  // Carga informacion de la columna de Pden_id 

                    /* Validacion del año vs periodo de carga*/
                    if (string.IsNullOrEmpty(colAnio))
                    {
                        cMens += "Hoja [FORMA 30], En la fila " + iCon.ToString() + "  no se indicó el año, ";
                        ErrorsList.Add("Hoja [FORMA 30], En la fila " + iCon.ToString() + "  no se indicó el año, ");
                    }
                    else
                    {
                        if (colAnio != iAnio)
                        {
                            cMens += "Hoja [FORMA 30], En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado en el archivo excel, ";
                            ErrorsList.Add("Hoja [FORMA 30], En la fila " + iCon.ToString() + " El año (" + iAnio.ToString() + ") es diferente al período cargado  en el archivo excel, ");
                        }
                    }
                    /* Validacion del mes vs periodo de carga*/
                    if (string.IsNullOrEmpty(colMes))
                    {
                        cMens += "Hoja [FORMA 30], En la fila " + iCon.ToString() + " El mes esta en blanco, ";
                        ErrorsList.Add("Hoja [FORMA 30], En la fila " + iCon.ToString() + " El mes esta en blanco, ");
                    }
                    else
                    {
                        if (colMes != iMes)
                        {
                            cMens += "Hoja [FORMA 30], En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ";
                            ErrorsList.Add("Hoja [FORMA 30], En la fila " + iCon.ToString() + " El mes (" + iMes.ToString() + ") es diferente al período cargado, ");
                        }
                    }
                    /* Validacion del PDEN_ID no esta en blanco y nulo*/
                    if (string.IsNullOrEmpty(colPden))
                    {
                        cMens += "Hoja [FORMA 30], En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ";
                        ErrorsList.Add("Hoja [FORMA 30], En la fila " + iCon.ToString() + " El Pden_id esta en blanco, ");
                    }
                    else
                    {
                        colPden = colPden.Trim();
                        if (!hashPden.Add(colPden))
                        {
                            cMens += "Hoja [FORMA 30], En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, ";
                            ErrorsList.Add("Hoja [FORMA 30], En la fila " + iCon.ToString() + " El Pden_id [" + colPden + "] esta repetido, ");
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

            if (ErrorsList.Count > 0 )//|| !ExcelError.cargaExtemporal)
            {
                ExcelError.state = true;
                ExcelError.message =
                    "Hoja [FORMA 30], El archivo cargado tiene mensajes de validación de los datos, verifique he intente de nuevo";
            }
            return Task.CompletedTask;
        }


        private async Task<BodyContenteDTOC30Masiva> BodyContentFormaC30Masiva(string getUserId)
        {
            BodyContenteDTOC30Masiva _content = new BodyContenteDTOC30Masiva();
            int rows = Excel.Rows();
            for (var row = 2; row <= rows; row++)
            {
                DetalleJsonC30Masiva itemDetalleJsonC30Masiva = new DetalleJsonC30Masiva()
                {

                    ANIO = ValidateCell("s", Excel.getCell(row, 1), "ANIO", row),
                    MES = ValidateCell("s", Excel.getCell(row, 2), "MES", row),
                    OPERADOR = ValidateCell("snull", Excel.getCell(row, 3), "OPERADOR", row),
                    CONTRATO = ValidateCell("snull", Excel.getCell(row, 4), "CONTRATO", row),
                    CAMPO = ValidateCell("snull", Excel.getCell(row, 5), "CAMPO", row),
                    PDEN_ID = ValidateCell("s", Excel.getCell(row, 6), "PDEN_ID", row),

                    GASFORMACION = ValidateCell("nnnull", Excel.getCell(row, 7), "GASFORMACION", row),
                    CONTENIDOPROPANO = ValidateCell("nnnull", Excel.getCell(row, 8), "CONTENIDOPROPANO", row),
                    CONTENIDOBUTANO = ValidateCell("nnnull", Excel.getCell(row, 9), "CONTENIDOBUTANO", row),
                    CONTENIDOGASOLINANATURAL = ValidateCell("nnnull", Excel.getCell(row, 10), "CONTENIDOGASOLINANATURAL", row),

                    GASFORMACIONPROCESADO = ValidateCell("nnnull", Excel.getCell(row, 11), "GASFORMACIONPROCESADO", row),
                    CONSUMOCAMPO = ValidateCell("nnnull", Excel.getCell(row, 12), "CONSUMOCAMPO", row),
                    GASODUCTOURBANO = ValidateCell("nnnull", Excel.getCell(row, 13), "GASODUCTOURBANO", row),
                    GENERACIONELECTRICA = ValidateCell("nnnull", Excel.getCell(row, 14), "GENERACIONELECTRICA", row),
                    OTROSCONSUMOS = ValidateCell("nnnull", Excel.getCell(row, 15), "OTROSCONSUMOS", row),
                    GASQUEMACAMPO = ValidateCell("nnnull", Excel.getCell(row, 16), "GASQUEMACAMPO", row),
                    VENTEADOALAIRE = ValidateCell("nnnull", Excel.getCell(row, 17), "VENTEADOALAIRE", row), //INDRA - 15/NOV/2023 - DMRICO - RF475973 Ajuste en el cargue de Forma30 de la asociada Incluir VENTEO
                    GASBOMBEONEUMATICO = ValidateCell("nnnull", Excel.getCell(row, 18), "GASBOMBEONEUMATICO", row),

                    GASINYECTADO = ValidateCell("nnnull", Excel.getCell(row, 19), "GASINYECTADO", row),
                    TOTALGASPROCESADO = ValidateCell("nnnull", Excel.getCell(row, 20), "TOTALGASPROCESADO", row),
                    PRODUCCIONPROPANO = ValidateCell("nnnull", Excel.getCell(row, 21), "PRODUCCIONPROPANO", row),

                    PRODUCCIONBUTANO = ValidateCell("nnnull", Excel.getCell(row, 22), "PRODUCCIONBUTANO", row),
                    PRODUCCIONGASOLINA = ValidateCell("nnnull", Excel.getCell(row, 23), "PRODUCCIONGASOLINA", row),
                    GASTRANSFORMADO = ValidateCell("nnnull", Excel.getCell(row, 24), "GASTRANSFORMADO", row),
                    CONSUMOENPLANTA = ValidateCell("nnnull", Excel.getCell(row, 25), "CONSUMOENPLANTA", row),
                    ENTREGAGASODUCTO = ValidateCell("nnnull", Excel.getCell(row, 26), "ENTREGAGASODUCTO", row),
                    ENTREGAGENERACION = ValidateCell("nnnull", Excel.getCell(row, 27), "ENTREGAGENERACION", row),
                    OTRASENTREGAS = ValidateCell("nnnull", Excel.getCell(row, 28), "OTRASENTREGAS", row),
                    GASQUEMAPLANTA = ValidateCell("nnnull", Excel.getCell(row, 29), "GASQUEMAPLANTA", row, true),
                    GASBOMBEOPLANTA = ValidateCell("nnnull", Excel.getCell(row, 30), "GASBOMBEOPLANTA", row, true),
                    GASINYECTADOPLANTA = ValidateCell("nnnull", Excel.getCell(row, 31), "GASINYECTADOPLANTA", row, true),
                    OBSERVACIONES = ValidateCell("snull", Excel.getCell(row, 32), "OBSERVACIONES", row),
                    ELIMINAR = (!string.IsNullOrEmpty(Excel.getCell(row, 33)) && (Excel.getCell(row, 32).ToUpper() == "X")),
                    APROBADO = true,
                    USERID = getUserId
                };
                DetalleJsonC30Masiva.Add(itemDetalleJsonC30Masiva);
            }
            _content.RespBDP = DetalleJsonC30Masiva;
            return await Task.Run(() => _content);
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

                if (type == "nnnull")   // valida pero con valores numericos con un rango sillega nulo lo deja pasar
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
                            cvalor = cvalor.Replace(cSeparadorOrigen, cSeparadorDestino);
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
                                res = cvalor.ToString();  //value.ToString();
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

        public async Task<ResponseBase<dynamic>> Create(RequestCreateFormaC30Masiva data, DateTime date, string GetUserId, string userName)
        {
            var response = new ResponseBase<dynamic>();

            try
            {
                // var header = data.Header9Masiva;
                // var body = data.Detail9Masiva;
                // var total = data.Totalacumulados;
                CultureInfo cultureInfo = new CultureInfo("es-co");

                var formC30 = new BodyContenteDTOC30Masiva
                {
                    Workbook = "FORMA C30"
                };
                try
                {
                    await _repository.CreateFormC30Masiva(formC30);
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
            JsonC30ValBDP.REGISTRO = DetalleJsonC30Masiva;
            var resFormaC30 = DetalleJsonC30Masiva.ToList();
            var json = JsonC30ValBDP.Serialize(settings);
            try
            {
                var res = await _repository.SqlValidarFormas30(json);
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
                                                                resFormaC30.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { u.APROBADO = false; u.USERID = getUserId; return u; }).ToList();
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
                                                    resFormaC30.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u =>
                                                    {
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "PDEN_ID") { u.PDEN_ID = cHomol; }
                                                        if (itemValidacion.Columna.ToString().ToUpper() == "CAMPO") { u.CAMPO = cHomol; }
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
                                    resFormaC30.Where(a => a.PDEN_ID == itemPden.LLAVE).Select(a =>
                                    {
                                        a.REG_MENSAJES = listError;
                                        return a;
                                    }).ToList();
                                }
                            }
                            else
                            {
                                string cCampo = string.Empty;
                                string cMensVal = "No se tiene una validación por parte de la Base de Datos. ";
                                resFormaC30.Where(u => u.PDEN_ID == itemPden.LLAVE).Select(u => { cCampo = u.CAMPO; return u; }).ToList();
                                foreach (var itemValidacion in itemPden.VALIDACIONES) {
                                    if (itemValidacion.Mensaje != null) {
                                        cMensVal += "\n"+itemValidacion.Mensaje;
                                    }
                                }
                                ErrorsList.Add(cMensVal);
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
                if (codes != 200)
                {
                    ErrorsList.Add($"No se pudo procesar la validación en BDP de las configuraciones asociadas a la forma C30 que se esta cargando, verifique e intente nuevamente, "+ cmessaje);
                    ExcelError.state = true;
                }
            }
            catch (Exception e)
            {
                TelemetryException.RegisterException(e);
                ErrorsList.Add($"No se pudo procesar la validación en la base de datos BDP, de la forma C30 [FORMA 30] que se esta cargando, verifique e intente nuevamente [Validar si existe conexion a BDP] => " + e.Message);
                ExcelError.state = true;
            }
            //-- Carga de Mensajes por cada Detlla de la forma
            foreach (var item in resFormaC30)
            {
                var RegDatosSQL = new DetalleJsonC30Masiva
                {
                    ANIO = item.ANIO,
                    MES = item.MES,
                    PDEN_ID = item.PDEN_ID,
                    CAMPO = item.CAMPO,
                    CONTRATO = item.CONTRATO,
                    OPERADOR = item.OPERADOR,
                    GASFORMACION = item.GASFORMACION,
                    CONTENIDOPROPANO = item.CONTENIDOPROPANO,
                    CONTENIDOBUTANO = item.CONTENIDOBUTANO,
                    CONTENIDOGASOLINANATURAL = item.CONTENIDOGASOLINANATURAL,
                    GASFORMACIONPROCESADO = item.GASFORMACIONPROCESADO,
                    CONSUMOCAMPO = item.CONSUMOCAMPO,
                    GASODUCTOURBANO = item.GASODUCTOURBANO,
                    GENERACIONELECTRICA = item.GENERACIONELECTRICA,
                    OTROSCONSUMOS = item.OTROSCONSUMOS,
                    GASQUEMACAMPO = item.GASQUEMACAMPO,
                    VENTEADOALAIRE = item.VENTEADOALAIRE,//INDRA - 15/NOV/2023 - DMRICO - RF475973 Ajuste en el cargue de Forma30 de la asociada Incluir VENTEO
                    GASBOMBEONEUMATICO = item.GASBOMBEONEUMATICO,
                    GASINYECTADO = item.GASINYECTADO,
                    TOTALGASPROCESADO = item.TOTALGASPROCESADO,
                    PRODUCCIONPROPANO = item.PRODUCCIONPROPANO,
                    PRODUCCIONBUTANO = item.PRODUCCIONBUTANO,
                    PRODUCCIONGASOLINA = item.PRODUCCIONGASOLINA,
                    GASTRANSFORMADO = item.GASTRANSFORMADO,
                    CONSUMOENPLANTA = item.CONSUMOENPLANTA,
                    ENTREGAGASODUCTO = item.ENTREGAGASODUCTO,
                    ENTREGAGENERACION = item.ENTREGAGENERACION,
                    OTRASENTREGAS = item.OTRASENTREGAS,
                    GASQUEMAPLANTA = item.GASQUEMAPLANTA,
                    GASBOMBEOPLANTA = item.GASBOMBEOPLANTA,
                    GASINYECTADOPLANTA = item.GASINYECTADOPLANTA,
                    OBSERVACIONES = item.OBSERVACIONES,
                    APROBADO = item.APROBADO,
                    USERID = item.USERID,
                    ELIMINAR = item.ELIMINAR,
                    REG_MENSAJES = item.REG_MENSAJES
                };
                _RespBDPJson.Add(RegDatosSQL);
            }
        }


        public Task<string> GuardarFormaC30PPDM (List<DetalleJsonC30Masiva> Datos)
        {
            //registerForm 
            //Detail9Multiple
            string resultado = string.Empty;
            var resFormaC30 = Datos.ToList();

            List<JsonC30MSQL> SQL_F30Masiva = new List<JsonC30MSQL>();
            List<JsonC30MBDP> BDP_F30Masiva = new List<JsonC30MBDP>();
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };
            foreach (var item in resFormaC30)
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

                var RegDatosSQL = new JsonC30MSQL
                {
                    Anio = item.ANIO,
                    Mes = item.MES,
                    Pden_Id = item.PDEN_ID,
                    Campo = item.CAMPO,
                    Contrato = item.CONTRATO,
                    Operdor = item.OPERADOR,
                    GasFormacion = item.GASFORMACION,
                    ContenidoPropano = item.CONTENIDOPROPANO,
                    ContenidoButano = item.CONTENIDOBUTANO,
                    ContenidoGasolinaNatural = item.CONTENIDOGASOLINANATURAL,
                    GasFormacionProcesado = item.GASFORMACIONPROCESADO,
                    ConsumoCampo = item.CONSUMOCAMPO,
                    GasoductoUrbano = item.GASODUCTOURBANO,
                    GeneracionElectrica = item.GENERACIONELECTRICA,
                    OtrosConsumos = item.OTROSCONSUMOS,
                    GasQuemaCampo = item.GASQUEMACAMPO,
                    VenteadoAlAire = item.VENTEADOALAIRE,//INDRA - 15/NOV/2023 - DMRICO - RF475973 Ajuste en el cargue de Forma30 de la asociada Incluir VENTEO
                    GasBombeoNeumatico = item.GASBOMBEONEUMATICO,
                    GasInyectado = item.GASINYECTADO,
                    TotalGasProcesado = item.TOTALGASPROCESADO,
                    ProduccionPropano = item.PRODUCCIONPROPANO,
                    ProduccionButano = item.PRODUCCIONBUTANO,
                    ProduccionGasolina = item.PRODUCCIONGASOLINA,
                    GasTransformado = item.GASTRANSFORMADO,
                    ConsumoenPlanta = item.CONSUMOENPLANTA,
                    EntregaGasoducto = item.ENTREGAGASODUCTO,
                    EntregaGeneracion = item.ENTREGAGENERACION,
                    OtrasEntregas = item.OTRASENTREGAS,
                    GasQuemaPlanta = item.GASQUEMAPLANTA,
                    GasBombeoPlanta = item.GASBOMBEOPLANTA,
                    GasInyectadoPlanta = item.GASINYECTADOPLANTA,
                    Observaciones = item.OBSERVACIONES,
                    Aprobado = item.APROBADO.ToString(),
                    CargadoaBDP = "true",
                    row_created_by = item.USERID,
                    row_created_date = DateTime.Now.ToString(),
                    row_changed_by = item.USERID,
                    row_changed_date = DateTime.Now.ToString(),
                    Json_MensajesErr = tempMensaje,
                    
                    Eliminar = item.ELIMINAR.ToString()
                };
                SQL_F30Masiva.Add(RegDatosSQL);
                if (item.APROBADO == true)
                {

                    var RegDatosBDP = new JsonC30MBDP
                    {
                        ANIO = item.ANIO,
                        MES = item.MES,
                        PDEN_ID = item.PDEN_ID,
                        CAMPO = item.CAMPO,
                        CONTRATO = item.CONTRATO,
                        OPERADOR = item.OPERADOR,
                        GASFORMACION = item.GASFORMACION,
                        CONTENIDOPROPANO = item.CONTENIDOPROPANO,
                        CONTENIDOBUTANO = item.CONTENIDOBUTANO,
                        CONTENIDOGASOLINANATURAL = item.CONTENIDOGASOLINANATURAL,
                        GASFORMACIONPROCESADO = item.GASFORMACIONPROCESADO,
                        CONSUMOCAMPO = item.CONSUMOCAMPO,
                        GASODUCTOURBANO = item.GASODUCTOURBANO,
                        GENERACIONELECTRICA = item.GENERACIONELECTRICA,
                        OTROSCONSUMOS = item.OTROSCONSUMOS,
                        GASQUEMACAMPO = item.GASQUEMACAMPO,
                        VENTEADOALAIRE = item.VENTEADOALAIRE, //INDRA - 15/NOV/2023 - DMRICO - RF475973 Ajuste en el cargue de Forma30 de la asociada Incluir VENTEO
                        GASBOMBEONEUMATICO = item.GASBOMBEONEUMATICO,
                        GASINYECTADO = item.GASINYECTADO,
                        TOTALGASPROCESADO = item.TOTALGASPROCESADO,
                        PRODUCCIONPROPANO = item.PRODUCCIONPROPANO,
                        PRODUCCIONBUTANO = item.PRODUCCIONBUTANO,
                        PRODUCCIONGASOLINA = item.PRODUCCIONGASOLINA,
                        GASTRANSFORMADO = item.GASTRANSFORMADO,
                        CONSUMOENPLANTA = item.CONSUMOENPLANTA,
                        ENTREGAGASODUCTO = item.ENTREGAGASODUCTO,
                        ENTREGAGENERACION = item.ENTREGAGENERACION,
                        OTRASENTREGAS = item.OTRASENTREGAS,
                        GASQUEMAPLANTA = item.GASQUEMAPLANTA,
                        GASBOMBEOPLANTA = item.GASBOMBEOPLANTA,
                        GASINYECTADOPLANTA = item.GASINYECTADOPLANTA,
                        OBSERVACIONES = item.OBSERVACIONES,
                        APROBADO = item.APROBADO.ToString(),
                        CARGADOABDP = "true",
                        USERID = item.USERID,
                        ROW_CREATED_BY = item.USERID,
                        ROW_CREATED_DATE = DateTime.Now.ToString(),
                        ROW_CHANGED_BY = item.USERID,
                        ROW_CHANGED_DATE = DateTime.Now.ToString(),
                        JSON_MENSAJESERR = tempMensaje,
                        ELIMINAR = item.ELIMINAR ? "1" : "0"
                    };
                    BDP_F30Masiva.Add(RegDatosBDP);
                }
            }


            string Bdp_Json = BDP_F30Masiva.Serialize(settings).ToString();
            resultado = "";
            bool lGuardarBdp = false;
            if (Bdp_Json.Count() > 0)
            {
                string EncJson = "{";
                EncJson += "\"FORMA_CODIGO\": \"30M\",";
                EncJson += "\"REGISTRO\": ";
                Bdp_Json = EncJson + Bdp_Json + "}";
                var respBdp = _repository.GuardaBdpforma30(Bdp_Json);
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
                                foreach (var UpdSQL in SQL_F30Masiva)
                                {
                                    if (UpdSQL.Pden_Id.ToString() == itemPden.LLAVE.ToString())
                                    {
                                        string cOldValue = UpdSQL.Json_MensajesErr.ToString().Replace("}]", "}").Replace("[{", "{");
                                        string cNewMensaje = "[" + cMensaNuevo + (string.IsNullOrEmpty(cOldValue) ? "" : ", ") + cOldValue + "]";
                                        SQL_F30Masiva.Where(u => u.Pden_Id == itemPden.LLAVE.ToString()).Select(u => { u.Json_MensajesErr = cNewMensaje; return u; }).ToList();
                                    }
                                }
                            }
                        }
                    }

                }
                resultado = "BDP_Respuesta:" + (lGuardarBdp ? "0;" : "1;") + respBdp + ".;";
            }
            string Sql_Json = SQL_F30Masiva.Serialize(settings).ToString();
            if ((Sql_Json.Count() > 0) )  //Pasa si pasa BDP 
            {
                var respSql = _repository.Guardasqlforma30(Sql_Json);
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
        public Task<string> consultaformaC30(int pAnio, int PMes, int PTodo)
        {
            string respuesta = _repository.ConsultarformaC30(pAnio, PMes, PTodo);

            return Task.Run(() => respuesta);
        }

    }
}
