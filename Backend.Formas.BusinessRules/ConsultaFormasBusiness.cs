using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Models;
using Backend.Formas.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules
{
    public class ConsultaFormasBusiness : Entities.Interface.Business.IConsultaFormasBusiness
    {
        /// <summary>
        /// The telemetry exception
        /// </summary>
        private readonly Utilities.Telemetry.ITelemetryException TelemetryException;
        /// <summary>
        /// The repository
        /// </summary>
        /// 
        private readonly Ppdm.PpdmGrpc.PpdmGrpcClient FormasService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsultaFormasBusiness"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="telemetryException">The telemetry exception.</param>
        public ConsultaFormasBusiness(
                                      Utilities.Telemetry.ITelemetryException telemetryException,
                                      Ppdm.PpdmGrpc.PpdmGrpcClient formasService)
        {

            TelemetryException = telemetryException;
            FormasService = formasService;


        }
        /// <summary>
        /// Gets the consulta formas.
        /// </summary>
        /// <param name="parametros">The parametros.</param>
        /// <returns></returns>

        public async Task<Entities.Responses.ResponseBase<List<ConsultaFormasModel>>> GetConsultaFormas(DetalleForma parametros)
        {
            var response = new Entities.Responses.ResponseBase<List<ConsultaFormasModel>>();
            try
            {

                Backend.Ppdm.ResponseBase res = await ConsultaFormasServices<ConsultaFormasModel>(parametros);
                ConsultaFormasServices<ConsultaFormasModel> resData = new ConsultaFormasServices<ConsultaFormasModel>();
                try
                {
                    try
                    {
                        resData = res.Data.Deserialize<ConsultaFormasServices<ConsultaFormasModel>>(getSetting()) ?? null;
                    }
                    catch
                    {
                        List<ConsultaFormasModel> obj = new List<ConsultaFormasModel>();
                        var objRequest = res.Data.Deserialize<ObjectServiceForma>(getSetting());
                        obj.Add(objRequest.root.registro);

                        //ConsultaFormasServices<ConsultaFormasModel> req = new ConsultaFormasServices<ConsultaFormasModel>();
                        Root<ConsultaFormasModel> root = new Root<ConsultaFormasModel>()
                        {
                            registro = obj
                        };
                        resData.root = root;


                    }


                }
                catch (Exception ex)
                {
                    TelemetryException.RegisterException(ex);

                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Error de conexión intente de nuevo";
                }

                if (resData != null && resData.root != null && resData.root.registro != null && resData.root.registro?.Count > 0)
                {
                    response.Data = resData.root.registro;
                    response.Message = "Solicitud ok";
                    response.Count = response.Data.Count();
                }
                else
                {
                    response.Code = 406;
                    response.Message = "No se encontraron registros";
                }
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);

                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error de conexión intente de nuevo";
            }
            return response;
        }

        public async Task<Entities.Responses.ResponseBase<List<dynamic>>> GetConsultaFormasOperador(DetalleForma parametros)
        {
            var response = new Entities.Responses.ResponseBase<List<dynamic>>();
            ConsultaFormasServices<dynamic> res = new ConsultaFormasServices<dynamic>();
            try
            {
                Backend.Ppdm.ResponseBase data = await ConsultaFormasServices<dynamic>(parametros);
                try
                {
                    try
                    {
                        res = data.Data.Deserialize<ConsultaFormasServices<dynamic>>(getSetting()) ?? null;
                    }
                    catch (Exception)
                    {
                        List<dynamic> obj = new List<dynamic>();
                        var objRequest = data.Data.Deserialize<dynamic>(getSetting());
                        obj.Add(objRequest.root.registro);
                        Root<dynamic> root = new Root<dynamic>()
                        {
                            registro = obj
                        };
                        res.root = root;
                    }


                }
                catch (Exception ex)
                {
                    TelemetryException.RegisterException(ex);

                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Error de conexión intente de nuevo";
                }

                if (res != null && res.root != null)
                {
                    if (res.root.registro != null && res.root.registro?.Count > 0)
                    {
                        response.Data = res.root.registro;
                        response.Message = "Solicitud ok";
                        response.Count = response.Data.Count();
                    }

                    if (!string.IsNullOrEmpty(res.root.error))
                    {
                        response.Code = 406;
                        response.Message = res.root.error;
                    }
                }
                else
                {
                    response.Code = 200;
                    response.Data = new List<dynamic>();
                    response.Message = "Solicitud ok";
                }
            }
            catch (Exception ex)
            {
                TelemetryException.RegisterException(ex);

                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error de conexión intente de nuevo";
            }
            return response;
        }

        private async Task<dynamic> ConsultaFormasServices<T>(DetalleForma parametros)
        {



            List<DetalleForma> listParameter = new List<DetalleForma>();
            listParameter.Add(parametros);

            ParametrosConsultaFormas data = new ParametrosConsultaFormas()
            {
                DETALLE = listParameter,
                FORMA_CODIGO = "REPORTEFORMAS"
            };


            List<ParametrosConsultaFormas> listJson = new List<ParametrosConsultaFormas>();
            listJson.Add(data);
            var json = listJson.Serialize(getSetting());

            return await FormasService.IntegradorFormasMinAsync(new Ppdm.RequestBase { SJson = json });

        }

        private Newtonsoft.Json.JsonSerializerSettings getSetting()
        {
            return new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Include,
            };
        }


    }
}
