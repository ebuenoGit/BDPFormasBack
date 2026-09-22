using Backend.Formas.BusinessRules.Middle;
using Backend.Formas.Entities;
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Utilities;
using Backend.Formas.Utilities.Telemetry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class ReporteBusiness: IReporteBusiness
    {
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;
        private readonly ITelemetryException TelemetriExcepcion;
        public ReporteBusiness(
            Ppdm.PpdmGrpc.PpdmGrpcClient _formasService,
            ITelemetryException telemetriExcepcion
        ) {
            FormasService = _formasService;
            TelemetriExcepcion = telemetriExcepcion;
        }

        public string GetUserId { get; private set; }

        public async  Task<ResponseBase<List<DetalleReporte>>> GenerarReporteFormas(string forma, DateTime date) {
            var response = new ResponseBase<List<DetalleReporte>>();

            jsonDetalle detalle = new jsonDetalle() {
                I_FORMA = forma,
                I_ANNO = date.ToString("yyyy"),
                I_MES = date.ToString("MM"),
                I_ID_USUARIO = GetUserId,

            };
            List<jsonDetalle> listJson = new  List<jsonDetalle>();
            listJson.Add(detalle);

            PeticionJsonReporte json = new PeticionJsonReporte()
            {
                FORMA_CODIGO = "REPORTEFORMAS",
                DETALLE = listJson
            };

            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            var jsonConvert = json.Serialize(settings);

            var res = await FormasService.IntegradorFormasMinAsync(new Ppdm.RequestBase { SJson = jsonConvert });
            try
            {
                var deserializa = res.Data.Deserialize< ReporteForma<DetalleReporte>> ();

                if (deserializa?.root?.Registro != null || deserializa?.root?.Registro.Count>0)
                {
                    response.Count = (int)HttpStatusCode.Created;
                    response.Data = deserializa.root.Registro;
                }
                else
                {
                    response.Code = 204;
                    response.Message = "";
                    response.Data = new List<DetalleReporte>();
                }
            }catch(Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = !string.IsNullOrEmpty(ex.InnerException.Message)
                    ? ex.InnerException.Message
                    : Messages.ServerError;
                TelemetriExcepcion.RegisterException(ex);
            }
            return response; 

        }

        public async Task<ResponseBase<List<DetalleReporteForma>>> GenerarReporteFormasDetalle(string forma, DateTime date,string idOperador, string idCampo, string idContrato)
        {
            var response = new ResponseBase<List<DetalleReporteForma>>();

            jsonDetalle detalle = new jsonDetalle()
            {
                I_FORMA = forma,
                I_ANNO = date.ToString("yyyy"),
                I_MES = date.ToString("MM"),
                I_ID_OPERADOR = idOperador,
                I_ID_CAMPO = idCampo,
                I_ID_CONTRATO = idContrato               // MCG 1-19-2022  Mejora para identificar el Contrato de la forma 
            };

            List<jsonDetalle> listJson = new List<jsonDetalle>();
            listJson.Add(detalle);

            PeticionJsonReporte json = new PeticionJsonReporte()
            {
                FORMA_CODIGO = "REPORTEFORMAS",
                DETALLE = listJson
            };

            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };

            var jsonConvert = json.Serialize(settings);

            var res = await FormasService.IntegradorFormasMinAsync(new Ppdm.RequestBase { SJson = jsonConvert });
            try
            {
                var deserializa = res.Data.Deserialize<ReporteForma<DetalleReporteForma>>();

                if (deserializa?.root?.Registro != null || deserializa?.root?.Registro.Count > 0)
                {
                    response.Count = (int)HttpStatusCode.Created;
                    response.Data = deserializa.root.Registro;
                }
                else
                {
                    response.Code = 204;
                    response.Message = "";
                    response.Data = new List<DetalleReporteForma>();
                }
            }
            catch 
            {
                response = new ResponseBase<List<DetalleReporteForma>>();
                List<DetalleReporteForma> list = new List<DetalleReporteForma>();
                var deserializa = res.Data.Deserialize<ReporteFormaJson<DetalleReporteForma>>();
                if (deserializa?.root?.Registro != null && !string.IsNullOrEmpty(deserializa?.root?.Registro?.pden_land_right))
                {
                    list.Add(deserializa.root.Registro);
                    response.Count = (int)HttpStatusCode.Created;
                    response.Data = list;
                }
                else
                {
                    response.Code = 204;
                    response.Message = "";
                    response.Data = new List<DetalleReporteForma>();
                }
            }

            return response;

        }

    }
}
