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
    public class Forma15CR : IFormaAprobacion<Aprobacion<Detalle15Aprobacion>>
    {

        private readonly IBaseRepository<Concreteform> ConcreRepo;
        private readonly IBaseRepository<Forma15CRTable> Repository;
        private readonly IBaseRepository<Forma15CRTableDetalle> RepositoryDetail;

        public Forma15CR(IBaseRepository<Concreteform> concreRepo,
                         IBaseRepository<Forma15CRTable> repository,
                         IBaseRepository<Forma15CRTableDetalle> detail)
        {
            ConcreRepo = concreRepo;
            Repository = repository;
            RepositoryDetail = detail;
        }

        public async Task<Aprobacion<Detalle15Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            Guid id = data.IdForma.Value;
            var concret = await ConcreRepo.GetAsync(predicate: x => x.Formid == id);
            var f15 = await Repository.GetAsync(predicate: x => x.form_id == id);
            var f15detaild = await RepositoryDetail.GetAllAsync(predicate: x => x.form_id == id);
            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

            //detalle lista de datos forma
            List<Backend.Formas.Entities.DTOI.Detalle15Aprobacion> list = new List<Backend.Formas.Entities.DTOI.Detalle15Aprobacion>();
            foreach (var item in f15detaild)
            {
                Backend.Formas.Entities.DTOI.Detalle15Aprobacion detail = new Backend.Formas.Entities.DTOI.Detalle15Aprobacion()
                {
                    PDEN_ID = item.pden_id,
                    PDEN_TYPE = "PDEN_PR_STR_FORM",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "207",
                    PERIOD_TYPE = "004",
                    VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                    AMENDMENT_SEQ_NO = "0",
                    ACTIVE_IND = "Y",
                    EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20501231",
                    OIL_VOLUME = item.produccionPetroleoBlsNetosMensual.ToString(),
                    OIL_VOLUME_OUOM = "BLS",
                    OIL_CUM_VOLUME = item.produccionPetroleoBlsNetosAcumulado.ToString(),
                    WATER_VOLUME = item.produccionAguaBlsMensual.ToString(),
                    WATER_VOLUME_OUOM = "BLS",
                    WATER_CUM_VOLUME = item.produccionAguaBlsAcumulado.ToString(),
                    PERIOD_ON_INJECTION = decimal.Round(item.valInyecDiasMes).ToString(),
                    PERIOD_ON_INJECTION_OUOM = "DIAS",
                    INJECTION_CYCLE = decimal.Round(item.valInyecCiclo).ToString(),
                    PRIMARY_PRODUCT = "038",
                    INJECTION_PRESSURE = item.valInyecPresionInyeccion.ToString(),
                    EC_INJECTION_VOLUME_OUOM = "MMBTU",
                    EC_INJECTION_VOLUME = item.valInyecBTUMes.ToString(),
                    EC_INJECTION_CUM_VOLUME =  item.valInyecBTUAcumulados.ToString(),
                    ECP_PROD_METHOD = item.valInyecMetodoProduccion.ToString(),
                    INJECTION_PRODUCT = "038",
                    ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                    ROW_CHANGED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                    ROW_CREATED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                    FORMACION =  item.valInyecFormacionProductora,
                    ECP_CUM_PERIOD_ON_INJECTION = item.valInyecDiasAcumulados.ToString(),
                    GAS_VOLUME = item.valInyecLibrasMes.ToString(),
                    GAS_VOLUME_OUOM = "MLBS",
                    GAS_QUALITY = item.valInyecCalidadVapor.ToString(),
                    GAS_CUM_VOLUME = item.valInyecLibrasAcumulados.ToString(),
                    
                };
                list.Add(detail);
            }

            //header aprobación 
            Aprobacion<Backend.Formas.Entities.DTOI.Detalle15Aprobacion> json = new Aprobacion<Backend.Formas.Entities.DTOI.Detalle15Aprobacion>()
            {
                FORMA_CODIGO = 15,
                FORMA_NOMBRE = "Forma 15",
                OPERADOR_ID = f15.compania_id,
                OPERADOR = f15.compania,
                CONTRATO = f15.contrato,
                CONTRATO_ID = f15.contrato_id,
                CAMPO_ID = f15.campo_id,
                CAMPO = f15.campo,
                MES = concret.Month.ToString(),
                ANIO = concret.Year.ToString(),
                MODALIDADEXPLOTACION = "Comercial",
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
