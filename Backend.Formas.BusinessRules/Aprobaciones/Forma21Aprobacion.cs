using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public class Forma21Aprobacion : IFormaAprobacion<Aprobacion<Detalle21Aprobacion>>
    {
        private readonly IBaseRepository<Concreteform> ConcretRepository;
        private readonly IBaseRepository<Form21> Repository;
        private readonly IBaseRepository<Form21InyectionDetail> RepositoryInyecion;
        private readonly IBaseRepository<Form21ProductionDetail> RepositoryProduccion;
        private readonly IBaseRepository<Form21TotalVolumenDetail> RepositoryTotal;
        public Forma21Aprobacion(
            IBaseRepository<Concreteform> concretRepository,
            IBaseRepository<Form21> repository,
            IBaseRepository<Form21InyectionDetail> repositoryInyecion,
            IBaseRepository<Form21ProductionDetail> repositoryProduccion,
            IBaseRepository<Form21TotalVolumenDetail> repositoryTotal
            )
        {
            ConcretRepository = concretRepository;
            Repository = repository;
            RepositoryInyecion = repositoryInyecion;
            RepositoryProduccion = repositoryProduccion;
            RepositoryTotal = repositoryTotal;
        }


        public async Task<Aprobacion<Detalle21Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserId)
        {

            Guid id = data.IdForma.Value;
            var concret = await ConcretRepository.GetAsync(predicate: x => x.Formid == id);
            var f21 = await Repository.GetAsync(predicate: x => x.Form21id == id);
            var f21Inyecion = await RepositoryInyecion.GetAllAsync(predicate: x => x.Formid == id);
            var f21Producion = await RepositoryProduccion.GetAllAsync(predicate: x => x.Formid == id);

            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

            //detalle lista de datos forma
            List<Backend.Formas.Entities.DTOI.Detalle21Aprobacion> list = new List<Backend.Formas.Entities.DTOI.Detalle21Aprobacion>();
            for (var i = 0;i <f21Inyecion.Count ; i++)
            {
                var inyect = f21Inyecion[i];
                var production = f21Producion[i];
                
                var dayAcu = Convert.ToInt32(production?.Accumulatedays);
                Backend.Formas.Entities.DTOI.Detalle21Aprobacion detail = new Backend.Formas.Entities.DTOI.Detalle21Aprobacion()
                {
                    PDEN_ID = inyect.Pden_id,
                    PDEN_TYPE = "PDEN_PR_STR_FORM",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "210",
                    PERIOD_TYPE = "004",
                    VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                    AMENDMENT_SEQ_NO = "0",
                    ACTIVE_IND = "Y",
                    EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20451130",

                    PERIOD_ON_INJECTION = Convert.ToInt32(inyect?.Gidays).ToString(),// dias en el mes cambiar
                    PERIOD_ON_INJECTION_OUOM = "DIAS",
                    INJECTION_PRESSURE = inyect.Pressure?.ToString(),
                    PRIMARY_PRODUCT = "016",
                    EC_INJECTION_VOLUME = inyect?.Gimonthlygas.ToString(),
                    EC_INJECTION_VOLUME_OUOM = "KPC",
                    EC_INJECTION_CUM_VOLUME = inyect.Giaccumulatedgas.ToString(),
                    ECP_CUM_PERIOD_ON_INJECTION = Convert.ToInt32(inyect.Giaccumulateddays).ToString(),
                    ROW_CHANGED_BY = $"FRONTALBDP/{getUserId}",
                    ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{getUserId}",
                    ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                    ECP_FINAL_STATUS = inyect.Oilwellfinalstate.ToUpper(),
                    ECP_INJECTION_PRESSURE_PROD = production.Pressure?.ToString(),
                    ECP_CUM_PERIOD_ON_PRODUCTION = dayAcu.ToString(),
                    PERIOD_ON_PRODUCTION = Convert.ToInt32(production?.Monthdays).ToString(),
                    PERIOD_ON_PRODUCTION_OUOM = "DIAS",
                    OIL_VOLUME = production?.Monthlyoilproduction.ToString(),
                    OIL_VOLUME_OUOM = "BLS",
                    OIL_CUM_VOLUME =production?.Accumulateoilproduction.ToString(),
                    GAS_VOLUME = production?.Montlhygasproduction.ToString(),
                    GAS_CUM_VOLUME = production?.Accumulategasproduction.ToString(),
                    GAS_VOLUME_OUOM = "KPC",
                    WATER_VOLUME = production?.Monthlywaterproduction.ToString(),
                    WATER_CUM_VOLUME = production?.Accumulatewaterproduction.ToString(),
                    WATER_VOLUME_OUOM = "BLS",
                    ECP_FINAL_STATUS_PROD = production.Oilwellfinalstate.ToUpper() ,
                    ECP_PROD_METHOD = production?.Productionmethod
                };
                list.Add(detail);
            }

            foreach (var production in f21Producion)
            {
                var dayAcu = Convert.ToInt32(production?.Accumulatedays);
                Backend.Formas.Entities.DTOI.Detalle21Aprobacion detail = new Backend.Formas.Entities.DTOI.Detalle21Aprobacion()
                {
                    PDEN_ID = production.Pden_id,
                    PDEN_TYPE = "PDEN_PR_STR_FORM",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "501",
                    PERIOD_TYPE = "004",
                    VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                    AMENDMENT_SEQ_NO = "0",
                    ACTIVE_IND = "Y",
                    EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20451130",

                    INJECTION_PRESSURE = production.Pressure?.ToString(),
                    ECP_CUM_PERIOD_ON_PRODUCTION = dayAcu.ToString(),
                    ROW_CHANGED_BY = $"FRONTALBDP/{getUserId}",
                    ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{getUserId}",
                    ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),

                    PERIOD_ON_PRODUCTION = Convert.ToInt32(production?.Monthdays).ToString(),
                    PERIOD_ON_PRODUCTION_OUOM = "DIAS",
                    OIL_VOLUME = production?.Monthlyoilproduction.ToString(),
                    OIL_VOLUME_OUOM = "BLS",
                    OIL_CUM_VOLUME =production?.Accumulateoilproduction.ToString(),
                    GAS_VOLUME = production?.Montlhygasproduction.ToString(),
                    GAS_CUM_VOLUME = production?.Accumulategasproduction.ToString(),
                    GAS_VOLUME_OUOM = "KPC",
                    WATER_VOLUME = production?.Monthlywaterproduction.ToString(),
                    WATER_CUM_VOLUME = production?.Accumulatewaterproduction.ToString(),
                    WATER_VOLUME_OUOM = "BLS",
                    ECP_FINAL_STATUS = production.Oilwellfinalstate.ToUpper() ,
                    ECP_PROD_METHOD = production?.Productionmethod
                };
            }

            //header aprobación 
            Aprobacion<Backend.Formas.Entities.DTOI.Detalle21Aprobacion> json = new Aprobacion<Backend.Formas.Entities.DTOI.Detalle21Aprobacion>()
            {
                FORMA_CODIGO = 21,
                FORMA_NOMBRE = "Forma 21",
                OPERADOR_ID = concret.Pdenid,
                OPERADOR = concret.Company,
                CONTRATO = concret.Contract,
                CAMPO_ID = f21.Campoid,
                CONTRATO_ID = f21.Contractid,
                CAMPO = f21.Oilfield,
                MES = concret.Month.ToString(),
                ANIO = concret.Year.ToString(),
                MODALIDADEXPLOTACION_ID = concret.Explotationmodality,
                REPRESENTANTE_OPERADOR_NM = concret.Usersigning,
                REPRESENTANTE_OPERADOR_TP = concret.Usersigning,
                REPRESENTANTE_ANH_NAME = concret.Minrepsigning,
                REPRESENTANTE_ANH_TP = concret.Minrepsigning,
                GENERADO_DESDE = "FRONTALBDP",
                ROW_CHANGED_BY = $"FRONTALBDP/{getUserId}",
                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{getUserId}",
                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                DETALLE = list,
                OBSERVACIONES = "",
            };
            return json;

        }
    }
}
