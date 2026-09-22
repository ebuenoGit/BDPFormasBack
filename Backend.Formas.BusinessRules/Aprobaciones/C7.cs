using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public class C7 : IC7
    {
        private readonly IBaseRepository<FormC7> _repositoryC7;
        private readonly IBaseRepository<FormC7Detail> _repositoryC7Detail;
        private readonly IBaseRepository<Concreteform> _repositoryConcretForm;
        public C7(
            IBaseRepository<FormC7> repositoryCuadro7,
            IBaseRepository<FormC7Detail> repositoryC7Detail,
            IBaseRepository<Concreteform> repositoryConcretForm
            )
        {
            _repositoryC7 = repositoryCuadro7;
            _repositoryC7Detail = repositoryC7Detail;
            _repositoryConcretForm = repositoryConcretForm;
        }
        public async Task<Aprobacion<DetalleC7Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            Guid id = data.IdForma.Value;
            var concret = await _repositoryConcretForm.GetAsync(predicate: x => x.Formid == id);
            var f7 = await _repositoryC7.GetAsync(predicate: x => x.Formc7id == id);
            var details = await _repositoryC7Detail.GetAllAsync(predicate: x => x.Formid == id);

            CultureInfo cultureInfo = new CultureInfo("es-co");
            string dateForma = $"{concret.Month}/{concret.Year}";
            DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
            int day = dateTime.AddMonths(1).AddDays(-1).Day;
            DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

            List<DetalleC7Aprobacion> list = new List<DetalleC7Aprobacion>();
            foreach (var item in details)
            {
                DetalleC7Aprobacion detalleC7Aprobacion = new DetalleC7Aprobacion()
                {
                    PDEN_ID = item.PdenId,
                    PDEN_TYPE = "PDEN_FIELD",
                    PDEN_SOURCE = "OFFICIAL",
                    VOLUME_METHOD = "1005",
                    ACTIVITY_TYPE = "007",
                    PERIOD_TYPE = "004",
                    VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                    AMENDMENT_SEQ_NO = "0",
                    ACTIVE_IND = "Y",
                    EFFECTIVE_DATE = DateTime.Now.ToString("yyyyMMdd"),
                    EXPIRY_DATE = "20451130",
                    OIL_VOLUME = item.Oilproduction.ToString(),
                    OIL_VOLUME_OUOM = "BLS",
                    GAS_VOLUME = item.Gasproduction.ToString(),
                    GAS_VOLUME_OUOM = "KPC",
                    NO_OF_GAS_WELLS = item.Gasproductionwells.ToString(),
                    NO_OF_INJECTION_WELLS = item.Injectorwells.ToString(),
                    NO_OF_OIL_WELLS = item.Oilproduction.ToString(),
                    ECP_NO_OF_UNF_WELLS = item.Unfinishedwells.ToString(),
                    ECP_NO_OF_ACT_WELLS = item.Activewells.ToString(),
                    ECP_NO_OF_INA_WELLS = item.Inactivewells.ToString(),
                    ECP_NO_OF_FIN_WELLS = item.Endwells.ToString(),
                    ECP_NO_OF_ABA_WELLS = item.Abandonedwells.ToString(),
                    ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                    ROW_CHANGED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                    ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                    ROW_CREATED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                    ECP_NO_OF_ACT_ART_WELL = item.pozosProductoresActivosLevantamientoArtificial,
                    ECP_NO_OF_ACT_NAT_WELL = item.pozosProductoresActivosFlujoNatural,
                    ECP_NO_OF_CLO_WAT_WELL = item.pozosProductoresInactivosCerradoAltaRelacionAguaPetroleo,
                    ECP_NO_OF_CLO_TEM_WELL = item.pozosProductoresInactivosCerradoTemporalmente,
                    ECP_NO_OF_CLO_SEC_WELL = item.pozosTaponadosSecos,
                    ECP_NO_OF_CLO_SUS_TEM_WELL = item.pozosSuspendidosTemporalmente,
                    ECP_NO_OF_UNFINISHED_WELL = item.PozosSinTerminar,
                    NO_OF_INJECTION_GAS_WELLS = item.pozosTaponadosInyectoresGas.ToString(),
                    NO_OF_INJECTION_AIR_WELLS = item.pozosTaponadosInyectoresAire.ToString()
                };

                list.Add(detalleC7Aprobacion);
            }

            Aprobacion<DetalleC7Aprobacion> json = new Aprobacion<DetalleC7Aprobacion>()
            {
                FORMA_CODIGO = 7,
                FORMA_NOMBRE = "Forma 7",
                OPERADOR_ID = concret.Pdenid,
                OPERADOR = concret.Company,
                CONTRATO = concret.Contract,
                CONTRATO_ID = f7.Contractid,
                CAMPO_ID = f7.Campoid.ToString(),
                CAMPO = f7.Campo,
                ESTRUCTURA_ID = "",
                BLOQUE_ID = "",
                FORMACION_ID = "",
                FORMACION = "",
                FORMACION_SET_ID = "",
                MIEMBRO_ID = "",
                YACIMIENTO_ID = "",
                MES = concret.Month.ToString(),
                ANIO = concret.Year.ToString(),
                MODALIDADEXPLOTACION_ID = concret.Explotationmodality,
                REPRESENTANTE_OPERADOR_NM = concret.Usersigning,
                REPRESENTANTE_OPERADOR_TP = concret.Usersigning,
                REPRESENTANTE_ANH_NAME = concret.Minrepsigning,
                REPRESENTANTE_ANH_TP = concret.Minrepsigning,
                GENERADO_DESDE = "FRONTALBDP",
                ROW_CHANGED_BY = $"FRONTALBDP/{getUserid}",
                ROW_CHANGED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{getUserid}",
                ROW_CREATED_DATE = DateTime.Now.ToString("yyyyMMdd HH:mm:ss"),
                DETALLE = list,
                OBSERVACIONES = ""
            };
            return json;
        }
    }
}
