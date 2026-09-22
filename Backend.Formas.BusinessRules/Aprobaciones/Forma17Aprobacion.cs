using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Entities.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public class Forma17Aprobacion : IFormaAprobacion<Aprobacion<Detalle17Aprobacion>>
    {
        private readonly IBaseRepository<Concreteform> ConcreRepo;
        private readonly IBaseRepository<Forma17CRTable> Repository;
        private readonly IBaseRepository<Forma17CRTableDetalle> RepositoryDetail;


        public Forma17Aprobacion(
           IBaseRepository<Concreteform> concreRepo,
            IBaseRepository<Forma17CRTable> repository,
            IBaseRepository<Forma17CRTableDetalle> detail
            )
        {
            ConcreRepo = concreRepo;
            Repository = repository;
            RepositoryDetail = detail;
        }


        public async Task<Aprobacion<Detalle17Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            Guid id = data.IdForma.Value;
            var concret = await ConcreRepo.GetAsync(predicate: x => x.Formid == id);
            var f17 = await Repository.GetAsync(predicate: x => x.form_id == id);
            var f17detaild = await RepositoryDetail.GetAllAsync(predicate: x => x.form_id == id);

            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

            //detalle lista de datos forma
            List<Backend.Formas.Entities.DTOI.Detalle17Aprobacion> list = new List<Backend.Formas.Entities.DTOI.Detalle17Aprobacion>();
            foreach (var item in f17detaild)
            {
                Backend.Formas.Entities.DTOI.Detalle17Aprobacion detail = new Backend.Formas.Entities.DTOI.Detalle17Aprobacion()
                {
                    PDEN_ID = item.pden_id,
                    PDEN_TYPE = "PDEN_PR_STR_FORM",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "007",
                    PERIOD_TYPE = "004",
                    VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                    AMENDMENT_SEQ_NO = "0",
                    PERIOD_ON_PRODUCTION = Convert.ToInt32(item.diasEnElMes).ToString(),
                    PERIOD_ON_PRODUCTION_OUOM = "DIAS",
                    WATER_VOLUME = item.produccionAguaMensual.ToString(),
                    WATER_CUM_VOLUME = item.produccionAguaAcumulada.ToString(),
                    WATER_VOLUME_OUOM = "BLS",
                    GAS_VOLUME = item.produccionGasMCPDiaria.ToString(),
                    GAS_CUM_VOLUME = item.produccionGasMCPAcumulada.ToString(),
                    GAS_VOLUME_OUOM = "KPC",
                    EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20451130",
                    ACTIVE_IND = "Y",
                    ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                    ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                    ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                };
                list.Add(detail);
            }


            //header aprobación 
            Aprobacion<Backend.Formas.Entities.DTOI.Detalle17Aprobacion> json = new Aprobacion<Backend.Formas.Entities.DTOI.Detalle17Aprobacion>()
            {
                FORMA_CODIGO = 17,
                FORMA_NOMBRE = "Forma 17",
                OPERADOR_ID = f17.operador_id,
                OPERADOR = f17.operador,
                CONTRATO_ID = f17.contrato_id,
                CONTRATO = f17.contrato,
                CAMPO_ID = f17.contrato_id,
                CAMPO = f17.campo,
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
                OBSERVACIONES = "",
            };
            return json;
        }
    }
}
