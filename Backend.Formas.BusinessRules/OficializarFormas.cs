using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.Responses;
using Backend.Formas.Utilities;
using Backend.Formas.Utilities.Telemetry;
using Newtonsoft.Json;
using System;
using System.Xml;
using System.Xml.Linq;

namespace Backend.Formas.BusinessRules
{
    public class OficializarFormas : IOficializarFormas
    {
        private readonly ITelemetryException Telemetry;
        private readonly IFormaValidate Repository;
        public OficializarFormas(ITelemetryException telemetry, IFormaValidate repository)
        {
            Telemetry = telemetry;
            Repository = repository;
        }
        public async System.Threading.Tasks.Task<ResponseBase<dynamic>> GetOficializarFormas(OficializarModel data, string getUser)
        {


            try{
                

                XElement xml = new XElement("FORMAS",
                    new XElement("FORMA",
                        new XElement("DETALLE",
                            new XElement("I_FORMA", data.Forma),
                            new XElement("I_ANNO", data.Anno),
                            new XElement("I_MES", data.Mes),
                            new XElement("I_USER", getUser),
                            new XElement("I_CAMPO_CONTRATO", data.Campo)
                         )
                    )
                );

                var xmlres  = await Repository.Oficialializar(xml.ToString());
                if (string.IsNullOrEmpty(xmlres))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlres);

                    string json = JsonConvert.SerializeXmlNode(doc);
                    var res = json.Deserialize<RootOficializar>();
                    return new ResponseBase<dynamic>()
                    {
                        Code = 200,
                        Message = res?.root ?? null
                    };
                }
                return new ResponseBase<dynamic>()
                {
                    Code = 200,
                    Message = "Solicitud ok"

                };
            }
            catch(Exception e){
                Telemetry.RegisterException(e);
                return new ResponseBase<dynamic>()
                {
                    Code = 500,
                    Message = "Error de servidor"
                };
            }
        }


        public async System.Threading.Tasks.Task<ResponseBase<dynamic>> ConsultarOficializacion(DateTime FechaOperativa, string Forma)
        {
            try
            {
                var res = await Repository.ConsultarOficilizacion(FechaOperativa.ToString("yyyyMMdd"), Forma);

                if (res?.Count > 0)
                {
                    return new ResponseBase<dynamic>()
                    {
                        Code = 200,
                        Data = res,
                        Message = "Solicitud ok"
                    };
                }

                return new ResponseBase<dynamic>()
                {
                    Code = 206,
                    Data = res,
                    Message = "No se encontraron registros para el mes operativo"
                };


            }
            catch (Exception e)
            {
                Telemetry.RegisterException(e);
                return new ResponseBase<dynamic>()
                {
                    Code = 500,
                    Message = "Error de servidor"
                };
            }
        }
    }
}
