using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{

    public class Forma23Aprobacion : IForma23Aprobacion
    {
        private readonly IBaseRepository<Form23> Repository;
        private readonly IBaseRepository<Form23Detail> RepositoryDetail;
        private readonly IBaseRepository<Concreteform> ConcretRepository;
        public Forma23Aprobacion(
            IBaseRepository<Concreteform> _concret,
            IBaseRepository<Form23> _repository,
            IBaseRepository<Form23Detail> _detailRepository
            )
        {
            ConcretRepository = _concret;
            Repository = _repository;
            RepositoryDetail = _detailRepository;
        }
        public async Task<Aprobacion<Entities.DTOI.Forma23Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            Guid id = data.IdForma.Value;
            var concret = await ConcretRepository.GetAsync(predicate: x => x.Formid == id);
            var details = await RepositoryDetail.GetAllAsync(predicate: x => x.Formid == id);
            var f23 = await Repository.GetAsync(predicate: x => x.Form23id == id);

            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);
            int daysMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            DateTime newVolumDate = Convert.ToDateTime($"{ dateTime.Year }-{ dateTime.Month }-{daysMonth}");

            List<Entities.DTOI.Forma23Aprobacion> list = new List<Entities.DTOI.Forma23Aprobacion>();
            foreach (var item in details)
            {
                int widays = Convert.ToInt32(item.Widays);
                int monthDays = Convert.ToInt32(item.Monthdays);
                Entities.DTOI.Forma23Aprobacion detalle = new Entities.DTOI.Forma23Aprobacion()
                {
                    PDEN_ID = item.PdenId,
                    PDEN_TYPE = "PDEN_PR_STR_FORM",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "006",
                    PERIOD_TYPE = "004",
                    VOLUME_DATE = newVolumDate.ToString("yyyyMMdd"),
                    AMENDMENT_SEQ_NO = "0",
                    ACTIVE_IND = "Y",
                    EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20501231",
                    PERIOD_ON_INJECTION = monthDays.ToString(),
                    PERIOD_ON_INJECTION_OUOM = "DIAS",
                    INJECTION_PRESSURE = item.Pressure?.ToString(),
                    EC_INJECTION_VOLUME = item.Monthlywaterproduction?.ToString(),
                    EC_INJECTION_VOLUME_OUOM = "KPC",
                    EC_INJECTION_CUM_VOLUME = item.Accumulatewaterproduction.ToString(),
                    PRIMARY_PRODUCT = "016",
                    ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                    ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                    ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),

                    ECP_CUM_PERIOD_ON_INJECTION = decimal.ToInt32( item.Wiaccumulateddays ?? 0).ToString(),
                    VOLUME = item.Wiaccumulatedwater.ToString(),
                    CUM_VOLUME =  item.Wimonthlywater.ToString(),
                    PERIOD_ON_PRODUCTION = Convert.ToInt32(item.Widays).ToString(),
                    ECP_CUM_PERIOD_ON_PRODUCTION = item.Dailyoilproduction.ToString(),
                    OIL_VOLUME = item.Monthlyoilproduction.ToString(),
                    OIL_CUM_VOLUME = item.Accumulateoilproduction.ToString(),
                    GAS_VOLUME = item.Monthlygasproduction.ToString(),
                    GAS_CUM_VOLUME = item.Accumulategasproduction.ToString(),
                    ECP_SULPHUR =item.Widailywater.ToString(),
                    ECP_PROD_METHOD =item.productionmethodproduct,
                    ECP_FINAL_STATUS =item.Productionoilwellfinalstate.ToString(),
                    ECP_GROSS_THICKNESS=item.Dailygasproduction.ToString(),
                    INJECTION_PRESSURE_PROD = item.PressureProd
                };
                list.Add(detalle);
            }

            Aprobacion<Entities.DTOI.Forma23Aprobacion> json = new Aprobacion<Entities.DTOI.Forma23Aprobacion>()
            {
                FORMA_CODIGO = 23,
                FORMA_NOMBRE = "Forma 23",
                OPERADOR_ID = concret.Pdenid,
                OPERADOR = concret.Company,
                CONTRATO = f23.Contract,
                CAMPO_ID = f23.Campoid,
                CONTRATO_ID = f23.Contractid,
                CAMPO = f23.Campo,
                ESTRUCTURA_ID = null,
                BLOQUE_ID = null,
                FORMACION_ID = null,
                FORMACION = null,
                FORMACION_SET_ID = null,
                MIEMBRO_ID = null,
                YACIMIENTO_ID = null,
                MES = concret.Month.ToString(),
                ANIO = concret.Year.ToString(),
                MODALIDADEXPLOTACION_ID = concret.Explotationmodality,
                REPRESENTANTE_OPERADOR_NM = concret.Usersigning,
                REPRESENTANTE_OPERADOR_TP = concret.Usersigning,
                REPRESENTANTE_ANH_NAME = concret.Minrepsigning,
                REPRESENTANTE_ANH_TP = concret.Minrepsigning,
                GENERADO_DESDE = "FRONTALBDP",
                ROW_CHANGED_BY = $"FRONTALBDP/{getUserid}",
                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{getUserid}",
                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                DETALLE = list,
                OBSERVACIONES = ""
            };

            return json;
        }
    }
}
