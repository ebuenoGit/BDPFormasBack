
using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;


namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public class C1 : IC1
    {
        private readonly IBaseRepository<Concreteform> ConcreFormRepository;
        private readonly IBaseRepository<Cuadro1cabecera> Cuadro1Repository;
        private readonly IBaseRepository<Cuadro1detalle> Cuadro1DetalleRepository;
        private readonly IBaseRepository<Cuadro1total> Cuadro1TotalRepository;

        public C1(
        IBaseRepository<Concreteform> _concreFormRepository,
        IBaseRepository<Cuadro1cabecera> _cuadro1Repository,
        IBaseRepository<Cuadro1detalle> _cuadro1DetalleRepository,
        IBaseRepository<Cuadro1total> _cuadro1TotalRepository
            )
        {
            ConcreFormRepository = _concreFormRepository;
            Cuadro1DetalleRepository = _cuadro1DetalleRepository;
            Cuadro1Repository = _cuadro1Repository;
            Cuadro1TotalRepository = _cuadro1TotalRepository;

        }
        public async Task<Aprobacion<Cuadro1>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            Guid id = data.IdForma.Value;
            var concret = await ConcreFormRepository.GetAsync(x => x.Formid == id);
            var cuadro1 = await Cuadro1Repository.GetAsync(x => x.FormaId == id);

            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

            var list = await listDetalle(id, volumDate);
             
            Aprobacion<Cuadro1> json = new Aprobacion<Cuadro1>()
            {
                FORMA_CODIGO = 1,
                FORMA_NOMBRE = "Cuadro 1",
                OPERADOR_ID = cuadro1.CompaniaId.ToString(),
                OPERADOR = concret.Company,
                CONTRATO = concret.Contract,
                BATERIA_ID = cuadro1.id_bateria,
                BATERIA = cuadro1.Bateria,
                TANQUE_ID = cuadro1.id_tanque,
                TANQUE = cuadro1.Tanque,
                CAMPO = cuadro1.Campo,
                CAMPO_ID = cuadro1.CampoId?.ToString(),
                CONTRATO_ID = cuadro1.ContratoId?.ToString(),
                MES = concret.Month.ToString(),
                ANIO = concret.Year.ToString(),
                MODALIDADEXPLOTACION_ID = concret.Explotationmodality,
                REPRESENTANTE_OPERADOR_NM = concret.Usersigning,
                REPRESENTANTE_OPERADOR_TP = concret.Usersigning,
                REPRESENTANTE_ANH_NAME = concret.Minrepsigning,
                REPRESENTANTE_ANH_TP = concret.Minrepsigning,
                GENERADO_DESDE = "FRONTALBDP",
                ROW_CHANGED_BY = $"FRONTALBDP/{cuadro1.row_changed_by}",
                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{cuadro1.row_created_by}",
                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                OBSERVACIONES = "",
                DETALLE = list
            };

            return json;
        }

        private async Task<List<Cuadro1>> listDetalle(Guid id, DateTime volumDate)
        {
            var listDetalle = await Cuadro1DetalleRepository.GetAllAsync(predicate: x => x.FormaId == id);
            var total = await Cuadro1TotalRepository.GetAsync(predicate: x => x.FormaId == id);

            DateTime now = DateTime.Now;
            string dateVolum = $"{volumDate.ToString("yyyyMMdd")}2359";

            List<Cuadro1> list = new List<Cuadro1>();
            foreach (Cuadro1detalle item in listDetalle)
            {
                Cuadro1 c1 = new Cuadro1()
                {
                    PDEN_ID = item.PdenId,
                    PDEN_TYPE = "PDEN_FACILITY",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "011",
                    PERIOD_TYPE = "002",
                    VOLUME_DATE = dateVolum,
                    AMENDMENT_SEQ_NO = "0",
                    ACTIVE_IND = "Y",
                    EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20501231",
                    ECP_LIQUID_LEVEL = item.MedidaMm?.ToString(),
                    ECP_TOTAL_VOLUME = item.AforoBls?.ToString(),
                    ECP_WATER_LEVEL = item.Ctsh.ToString(),
                    ECP_FREE_WATER_VOLUME = item.Bls60f.ToString(),
                    ECP_STANDARD_VOLUME = item.TempAmb.ToString(),
                    ECP_FLUID_TEMPERATURE = item.TempF?.ToString(),
                    ECP_CORRECTION_FACTOR = item.FactorTemp?.ToString(),
                    ECP_BSW = item.Bsw?.ToString(),
                    ECP_BSW_FACTOR = item.FactorBsw?.ToString(),
                    OIL_VOLUME = item.Blsnetos?.ToString(),
                    OIL_VOLUME_RECEIVED = "0",
                    OIL_VOLUME_RECEIVED_OUOM = "BLS",
                    OIL_VOLUME_DELIVERED_OUOM = "BLS",
                    OIL_QUALITY = item.Api60f?.ToString(),
                    ECP_SPECIFIC_GRAVITY = item.Ge?.ToString(),
                    ECP_SALINITY = item.SalBtb?.ToString(),
                    ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                    ROW_CHANGED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                    ROW_CREATED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                    ECP_BALANCE_RECEIVED = item.TrasRecibido?.ToString(),
                    ECP_BALANCE_SENT= item.TrasEnvio?.ToString(),
                    ECP_INTRADIARY_MOV_RECEIVED= item.MovIntraRecibido?.ToString(),
                    ECP_INTRADIARY_MOV_SENT= item.MovIntraEnvio?.ToString(),
                    OIL_VOLUME_OUOM = "BLS",
                    DISPOSITION_OBS_NO= "235959"
                };
                list.Add(c1);
            }
            return list;
        }
    }
}
