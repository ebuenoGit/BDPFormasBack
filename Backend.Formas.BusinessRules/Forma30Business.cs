using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class Forma30Business : IForma30Business
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IForma30Repository _repository;

        private readonly IFormaValidate RepositoryValidate;
        /// <summary>
        /// The telemetry exception
        /// </summary>
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;

        private List<Forma30Body> _body = new List<Forma30Body>();
        private Forma30Header _header = new Forma30Header();
        private readonly List<string> ErrorsList = new List<string>();
        private readonly IImportarExcel Excel;
        private readonly ExcelError ExcelError = new ExcelError();
        private Forma30GasProcesado GasProcesado = new Forma30GasProcesado();
        private readonly Forma30BodyTotal totalData = new Forma30BodyTotal();

        public Forma30Business(IFormaValidate _validate, IImportarExcel excel, Utilities.Telemetry.ITelemetryException telemetryException, IForma30Repository repository)
        {
            RepositoryValidate = _validate;
            Excel = excel;
            TelemetryException = telemetryException;
            _repository = repository;
        }

        public async Task<ResponseBase<ResponseForma30>> LoadFile(DateTime _date, Stream _file, string name)
        {
            var response = new ResponseBase<ResponseForma30>();

            try
            {
                Excel.setFile(_file);
                Excel.reader("FORMA 30");
                if (Excel.isLoad())
                {
                    HeaderForma();
                    BodyForma();
                    var responseForma30 = new ResponseForma30
                    {
                        Header = _header,
                        Body = _body,
                        BodyTotal = totalData,
                        GasProcesado = GasProcesado
                    };
                    response.Data = responseForma30;
                    response.Count = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                }
                else
                {
                    ExcelError.message = "Error de Lectura el Archivo no Cuenta Con la Hoja de Forma 30 ";
                    ExcelError.state = true;
                    response.Count = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                    var resposeForma = new ResponseForma30
                    {
                        Errors = ExcelError
                    };
                    response.Data = resposeForma;
                }

                if (ExcelError.state == false)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = Messages.Created;
                }
                else
                {
                    response.Count = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = !string.IsNullOrEmpty(ex.InnerException.Message)
                    ? ex.InnerException.Message
                    : Messages.ServerError;
            }

            return await Task.Run(() => response);
        }


        public async Task<ResponseBase<dynamic>> Create(dynamic data, DateTime date)
        {
            var response = new ResponseBase<dynamic>
            {
                Code = (int)HttpStatusCode.OK,
                Message = Messages.Created
            };
            return await Task.Run(() => response);
        }

        private void BodyForma()
        {
            if (ErrorsList.Count == 0)
            {
                var Total = "Total";
                var list = new List<Forma30Body>();

                var rows = Excel.Rows();
                var row2 = 0;
                for (var row = 14; row <= rows; row++)
                {
                    var Body = new Forma30Body();

                    var total = Excel.getCell(row, 1);
                    var isTotal = Total.StartsWith(total, StringComparison.CurrentCultureIgnoreCase);
                    if (isTotal)
                    {
                        totalData.GasDeFormacionTotal = Excel.getCell(row, 2) ?? "0";
                        totalData.ContenidoDePropanoTotal = Excel.getCell(row, 3) ?? "0";
                        totalData.ContenidoDeButanoTotal = Excel.getCell(row, 4) ?? "0";
                        totalData.ContenidoDeGasolinaNaturalTotal = Excel.getCell(row, 5) ?? "0";
                        totalData.GasDeFormacionProcesadoTotal = Excel.getCell(row, 6) ?? "0";
                        totalData.SinProcesarConsumoEnCampoTotal = Excel.getCell(row, 7) ?? "0";
                        totalData.SinProcesarGasoductosUrbanosTotal = Excel.getCell(row, 8) ?? "0";
                        totalData.SinProcesarGeneracionElectricaTotal = Excel.getCell(row, 9) ?? "0";
                        totalData.SinProcesarOtrosTotal = Excel.getCell(row, 10) ?? "0";
                        totalData.SinProcesarQuemaAlAireTotal = Excel.getCell(row, 11) ?? "0";
                        totalData.SinProcesarBombeoAutomaticoTotal = Excel.getCell(row, 12) ?? "0";
                        totalData.SinProcesarInyacionYacimientoTotal = Excel.getCell(row, 13) ?? "0";
                        row2 = row + 7;
                        break;
                    }

                    Body.Campo = Excel.getCell(row, 1) ?? "0";
                    Body.GasDeFormacion = Excel.getCell(row, 2) ?? "0";
                    Body.ContenidoDePropano = Excel.getCell(row, 3) ?? "0";
                    Body.ContenidoDeButano = Excel.getCell(row, 4) ?? "0";
                    Body.ContenidoDeGasolinaNatural = Excel.getCell(row, 5) ?? "0";
                    Body.GasDeFormacionProcesado = Excel.getCell(row, 6) ?? "0";
                    Body.SinProcesarConsumoEnCampo = Excel.getCell(row, 7) ?? "0";
                    Body.SinProcesarGasoductosUrbanos = Excel.getCell(row, 8) ?? "0";
                    Body.SinProcesarGeneracionElectrica = Excel.getCell(row, 9) ?? "0";
                    Body.SinProcesarOtros = Excel.getCell(row, 10) ?? "0";
                    Body.SinProcesarQuemaAlAire = Excel.getCell(row, 11) ?? "0";
                    Body.SinProcesarBombeoAutomatico = Excel.getCell(row, 12) ?? "0";
                    Body.SinProcesarInyacionYacimiento = Excel.getCell(row, 13) ?? "0";

                    list.Add(Body);
                }

                _body = list;

                for (var row = row2; row <= rows; row++)
                {
                    if (!string.IsNullOrEmpty(Excel.getCell(row, 1)))
                    {
                        GasProcesado = new Forma30GasProcesado
                        {
                            gasTotal = float.Parse(Excel.getCell(row, 1) ?? "0"),
                            propano = float.Parse(Excel.getCell(row, 2) ?? "0"),
                            butano = float.Parse(Excel.getCell(row, 3) ?? "0"),
                            gasolina = float.Parse(Excel.getCell(row, 4) ?? "0"),
                            gasTrasformado = float.Parse(Excel.getCell(row, 5) ?? "0"),
                            consumido = float.Parse(Excel.getCell(row, 6) ?? "0"),
                            entregado = float.Parse(Excel.getCell(row, 7) ?? "0"),
                            gasoducto = float.Parse(Excel.getCell(row, 8) ?? "0"),
                            generacionElectrica = float.Parse(Excel.getCell(row, 9) ?? "0"),
                            otros = float.Parse(Excel.getCell(row, 10) ?? "0"),
                            quemadoAlAire = float.Parse(Excel.getCell(row, 11) ?? "0"),
                            usadoEnBombeo = float.Parse(Excel.getCell(row, 12) ?? "0")
                        };
                        break;
                    }
                }
            }
        }

        private void HeaderForma()
        {
            var operador = Excel.getCell(8, 2);
            var operador1 = Excel.getCell(8, 3);
            var operador2 = Excel.getCell(8, 1);
            var contrato = Excel.getCell(8, 7);
            var mes = Excel.getCell(8, 10);
            var ano = Excel.getCell(8, 13);

            _header = new Forma30Header
            {
                Operator = operador,
                Contract = contrato,
                Month = mes,
                Year = ano
            };
        }

        public async Task<ResponseBase<bool>> CreaForma30(ParametrosCreaForma30 id)
        {
            ResponseBase<bool> response = new ResponseBase<bool>();
            try
            {
                response.Data = await _repository.CreaForma30(id);
                if (response.Data)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Solicitud ok";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Ningun item creado";
                }
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseBase<bool>> PvsCrea(ParametrosPvsCrea id, string p_code)
        {
            ResponseBase<bool> response = new ResponseBase<bool>();
            try
            {
                response.Data = await _repository.PvsCrea(id, p_code);
                if (response.Data)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Solicitud ok";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Ningun item encontrado";
                }
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseBase<bool>> PvsoCrea(ParametrosPvsoCrea id, string p_code)
        {
            ResponseBase<bool> response = new ResponseBase<bool>();
            try
            {
                response.Data = await _repository.PvsoCrea(id, p_code);
                if (response.Data)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Solicitud ok";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = "Ningun item encontrado";
                }
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}