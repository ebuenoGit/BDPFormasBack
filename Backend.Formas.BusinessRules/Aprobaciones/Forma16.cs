using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Utilities.Telemetry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public class Forma16 : IForma16
    {
        private readonly IBaseRepository<Concreteform> ConcretRepository;
        private readonly IBaseRepository<Form16> RepositoryF16;
        private readonly IBaseRepository<Form16Detail> RepositoryF16Detalle;
        private readonly ITelemetryException Telemetryc;
        public Forma16(
            IBaseRepository<Concreteform> _concretRepository,
            IBaseRepository<Form16> _repositoryF16,
            IBaseRepository<Form16Detail> _repositoryF16Detalle,
            ITelemetryException telemetryc
            )
        {
            ConcretRepository = _concretRepository;
            RepositoryF16 = _repositoryF16;
            RepositoryF16Detalle = _repositoryF16Detalle;
            Telemetryc = telemetryc;
        }

        public async Task<Aprobacion<Backend.Formas.Entities.DTOI.Forma16>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            Guid id = data.IdForma.Value;
            var concret = await ConcretRepository.GetAsync(predicate: x => x.Formid == id);
            var f16 = await RepositoryF16.GetAsync(predicate: x => x.Form16id == id);
            var f16detaild = await RepositoryF16Detalle.GetAllAsync(predicate: x => x.Formid == id);

            CultureInfo cultureInfo = new CultureInfo("es-CO");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

            //detalle lista de datos forma
            List<Backend.Formas.Entities.DTOI.Forma16> list = new List<Backend.Formas.Entities.DTOI.Forma16>();
            foreach (var item in f16detaild)
            {
                string dateFormatesting = $"{item.Testingdate?.Month}/{item.Testingdate?.Year}";
                DateTime date = DateTime.Parse(dateForma, cultureInfo);
                

                try
                {
                   
                    var testingDate = item.Testingdate?.Date;
                    string dateformat = $"{testingDate?.Year}{date.Month}{testingDate?.Day}";
                    var getDay = Int32.Parse(testingDate?.Day.ToString());
                    DateTime testing = new DateTime(date.Year, date.Month, getDay);                   

                    Backend.Formas.Entities.DTOI.Forma16 detail = new Backend.Formas.Entities.DTOI.Forma16()
                    {
                        UWI = item.Pden_id,
                        SOURCE = "OFFICIAL",
                        TEST_TYPE = "017",
                        TEST_DATE = testing.ToString("yyyyMMdd"),
                        EC_TEST_METHOD = "1005",
                        ACTIVE_IND = "Y",
                        EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                        EXPIRY_DATE = "20501231",
                        PRODUCTION_METHOD = item.Productionmethod,
                        FLOW_PRESSURE = item.Thppressure?.ToString(),
                        CASING_PRESSURE = item.Chppressure?.ToString(),
                        TEST_DURATION = item.Hours?.ToString(),
                        TEST_DURATION_OUOM = "HRS",
                        OIL_FLOW_AMOUNT = item.Rga?.ToString(),
                        OIL_FLOW_AMOUNT_OUOM = "BOPD",
                        OIL_GRAVITY = "0",
                        WATER_FLOW_AMOUNT = item.Water?.ToString(),
                        WATER_FLOW_AMOUNT_OUOM = "BOPD",
                        GAS_FLOW_AMOUNT = item.Gas?.ToString(),
                        GAS_FLOW_AMOUNT_OUOM = "MSCFD",
                        GOR = "",
                        ECP_FINAL_STATUS = "",
                        FORMACION = f16.Formation,
                        ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                        ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                        ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                        ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                    };
                    list.Add(detail);
                }
                catch(Exception e)
                {
                    Telemetryc.RegisterException(e);
                        
                }

            }

            //header aprobación 
            Aprobacion<Backend.Formas.Entities.DTOI.Forma16> json = new Aprobacion<Backend.Formas.Entities.DTOI.Forma16>()
            {
                FORMA_CODIGO = 16,
                FORMA_NOMBRE = "Forma 16",
                OPERADOR_ID = concret.Pdenid,
                OPERADOR = f16.Operador,
                CONTRATO = f16.Contract,
                CONTRATO_ID = f16.Contractid,
                CAMPO_ID = f16.Campoid,
                CAMPO = f16.Campo,
                MES = concret.Month.ToString(),
                ANIO = concret.Year.ToString(),
                MODALIDADEXPLOTACION_ID = concret.Explotationmodality,
                REPRESENTANTE_OPERADOR_NM = concret.Usersigning,
                REPRESENTANTE_OPERADOR_TP = concret.Usersigning,
                REPRESENTANTE_ANH_NAME = concret.Minrepsigning,
                REPRESENTANTE_ANH_TP = concret.Minrepsigning,
                GENERADO_DESDE = "FRONTALBDP",
                ROW_CHANGED_BY = $"FRONTALBDP/{getUserid}",
                ROW_CHANGED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss", cultureInfo),
                ROW_CREATED_BY = $"FRONTALBDP/{getUserid}",
                ROW_CREATED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss", cultureInfo),
                DETALLE = list,
                OBSERVACIONES = "",
            };
            return json;
        }
    }

}
