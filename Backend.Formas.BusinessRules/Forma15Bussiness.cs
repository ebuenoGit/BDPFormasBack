using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Utilities.Telemetry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class Forma15Bussiness : IForma15Business
    {
        private List<BodyTableForma15> _body = new List<BodyTableForma15>();
        private HeaderForma15 _header = new HeaderForma15();
        private readonly List<string> ErrorsList = new List<string>();

        private readonly IImportarExcel Excel;
        private readonly ExcelError ExcelError = new ExcelError();
        private readonly ITelemetryException TelemetryException;

        public Forma15Bussiness(
            IImportarExcel excel,
            ITelemetryException telemetryException
                )
        {
            Excel = excel;
            ExcelError.message = "";
            ExcelError.listError = new List<string>();
            ExcelError.state = false;
            TelemetryException = telemetryException;
        }

        public Task<ResponseBase<dynamic>> Create(ResponseForma15 data, DateTime date)
        {
            var response = new ResponseBase<dynamic>();
            try
            {
                response.Data = "";
                response.Code = 200;
            }
            catch (Exception e)
            {
                response.Message = "Error  al registrar la forma 15";
                response.Code = 500;
                TelemetryException.RegisterException(e);
            }

            return Task.Run(() => response);
        }

        public async Task<ResponseBase<ResponseForma15>> LoadFile(DateTime date, Stream _file)
        {
            var response = new ResponseBase<ResponseForma15>();

            try
            {
                _header = new HeaderForma15();
                Excel.setFile(_file);
                Excel.reader("Forma 15A");
                if (Excel.isLoad())
                {
                    headerForma();
                    bodyForma();
                    var resposeForma = new ResponseForma15
                    {
                        Header = _header,
                        Body = _body,
                        Errors = ExcelError
                    };
                    response.Data = resposeForma;
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = Messages.Created;
                }
                else
                {
                    ExcelError.message = "Error de Lectura el Archivo no Cuenta Con la Hoja de Forma 15A ";
                    ExcelError.state = true;
                    response.Count = (int)HttpStatusCode.BadRequest;
                    response.Message = Messages.ErrorCreation;
                    var resposeForma = new ResponseForma15
                    {
                        Errors = ExcelError
                    };
                    response.Data = resposeForma;
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

        private void bodyForma()
        {
            if (ErrorsList.Count == 0)
            {
                var list = new List<BodyTableForma15>();
                var rows = Excel.Rows();
                for (var row = 19; row <= rows; row++)
                {
                    var Body = new BodyTableForma15
                    {
                        Well = Excel.getCell(row, 1),
                        Traiding = Excel.getCell(row, 2),
                        Method = Excel.getCell(row, 3),
                        Pressure = Excel.getCell(row, 4),
                        Cycle = Excel.getCell(row, 5),
                        DayMonths = Excel.getCell(row, 6),
                        DayAccumulated = Excel.getCell(row, 7),
                        QuidMonths = Excel.getCell(row, 8),
                        QuidAccumulated = Excel.getCell(row, 9),
                        BtuMoths = Excel.getCell(row, 10),
                        BtuAccumulated = Excel.getCell(row, 11),
                        Quality = Excel.getCell(row, 12),
                        OilMoths = Excel.getCell(row, 13),
                        OilAccumulated = Excel.getCell(row, 14),
                        WaterMoths = Excel.getCell(row, 15),
                        WaterAccumulated = Excel.getCell(row, 16)
                    };
                    list.Add(Body);
                }

                _body = list;
            }
        }

        private void headerForma()
        {
            var Compania = Excel.getCell(9, 2);
            var Months = Excel.getCell(9, 6);
            var Year = Excel.getCell(9, 9);
            var Concession = Excel.getCell(11, 2);
            var Field = Excel.getCell(11, 6);

            if (string.IsNullOrEmpty(Compania))
            {
                ErrorsList.Add("No se Encontro el Valor de Compañia");
            }

            if (string.IsNullOrEmpty(Months))
            {
                ErrorsList.Add("No se Encontro el Valor de Mes");
            }

            if (string.IsNullOrEmpty(Year))
            {
                ErrorsList.Add("No se Encontro el Valor de Año");
            }

            if (string.IsNullOrEmpty(Concession))
            {
                ErrorsList.Add("No se Encontro el Valor de Concesion");
            }

            if (string.IsNullOrEmpty(Field))
            {
                ErrorsList.Add("No se Encontro el Valor de Campo");
            }

            if (ErrorsList.Count == 0)
            {
                _header.Compani = Compania;
                _header.Concession = Concession;
                _header.Monhts = Months;
                _header.Year = Year;
                _header.Field = Field;
            }
            else
            {
                if (ErrorsList.Count > 0)
                {
                    ExcelError.state = true;
                    ExcelError.message =
                        "El Archivo Cargado no Tiene el Formato Correcto Verifique he Intente de nuevo";
                    ExcelError.listError = ErrorsList;
                }
            }
        }
    }
}