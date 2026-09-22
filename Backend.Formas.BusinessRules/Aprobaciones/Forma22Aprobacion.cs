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
    public class Forma22Aprobacion : IFormaAprobacion<Aprobacion<Detalle22Aprobacion>>
    {

        private readonly IBaseRepository<Forma22cr> Repository;
        private readonly IBaseRepository<Forma22crdetalle> RepositoryDetail;
        private readonly IBaseRepository<Concreteform> ConcretRepository;
        public Forma22Aprobacion(
            IBaseRepository<Concreteform> _concret,
            IBaseRepository<Forma22cr> _repository,
            IBaseRepository<Forma22crdetalle> _detailRepository
            )
        {
            ConcretRepository = _concret;
            Repository = _repository;
            RepositoryDetail = _detailRepository;
        }
        public async Task<Aprobacion<Entities.DTOI.Detalle22Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            Guid id = data.IdForma.Value;
            var concret = await ConcretRepository.GetAsync(predicate: x => x.Formid == id);
            var details = await RepositoryDetail.GetAllAsync(predicate: x => x.FormId == id);
            var f22 = await Repository.GetAsync(predicate: x => x.FormId == id);

            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

            List<Entities.DTOI.Detalle22Aprobacion> list = new List<Entities.DTOI.Detalle22Aprobacion>();
            foreach (var item in details)
            {
                string dateForma2 = $"{item.Mes}/{concret.Year}";
                DateTime dateTime2 = DateTime.Parse(dateForma2, cultureInfo);
                int day2 = dateTime2.AddMonths(1).AddDays(-1).Day;
                DateTime VOLUME_DATE = new DateTime(dateTime2.Year, dateTime2.Month, day2);

                Entities.DTOI.Detalle22Aprobacion detalle = new Entities.DTOI.Detalle22Aprobacion()
                {
                    PDEN_ID = item.PdenId,
                    PDEN_TYPE = "PDEN_XREF",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "007",
                    PERIOD_TYPE = "004",
                    VOLUME_DATE = VOLUME_DATE.ToString("yyyyMMdd"),
                    AMENDMENT_SEQ_NO = "0",
                    PRIMARY_PRODUCT = "009",
                    OIL_VOLUME = item.PetroleoProducidoMensual.ToString(),
                    OIL_CUM_VOLUME = item.PetroleoProducidoAcumulado.ToString(),
                    WATER_VOLUME_INY = item.AguaInyectadoMensual.ToString(),
                    WATER_CUM_VOLUME_INY = item.AguaInyectadoAcumulado.ToString(),
                    WATER_VOLUME_PRO = item.AguaProducidoMensual.ToString(),
                    WATER_CUM_VOLUME_PRO = item.AguaProducidoAcumulado.ToString(),
                    GAS_VOLUME_INY = item.GasInyectadoMensual.ToString(),
                    GAS_CUM_VOLUME_INY = item.GasInyectadoAcumulado.ToString(),
                    GAS_VOLUME_PRO = item.GasProducidoMensual.ToString(),
                    GAS_CUM_VOLUME_PRO = item.GasProducidoAcumulado.ToString(),
                    INJECTION_PRESSURE = item.PresionFondo.ToString(),
                    EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20451130",
                    ACTIVE_IND = "Y",
                    ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                    ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                    ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                };
                list.Add(detalle);
            }

            Aprobacion<Entities.DTOI.Detalle22Aprobacion> json = new Aprobacion<Entities.DTOI.Detalle22Aprobacion>()
            {
                FORMA_CODIGO = 22,
                FORMA_NOMBRE = "Forma 22",
                OPERADOR_ID = f22.CompaniaId,
                OPERADOR = f22.Compania,
                CONTRATO = "0",
                CONTRATO_ID="0",
                CAMPO_ID = f22.CampoId,
                CAMPO = f22.Campo,
                MES = concret.Month.ToString(),
                ANIO = concret.Year.ToString(),
                MODALIDADEXPLOTACION_ID =null,
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
                YACIMIENTO = f22.Yacimiento,
                YACIMIENTO_ID = f22.YacimientoId
            };

            return json;
        }
    }
}
