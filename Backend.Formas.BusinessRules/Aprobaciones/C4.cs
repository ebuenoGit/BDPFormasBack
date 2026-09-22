using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using Backend.Formas.Entities.Interface.Repository;
using Backend.Formas.Utilities.Telemetry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{

    public class C4 : IC4
    {
        private readonly IForma4Repository RepositoryC4;
        private readonly IBaseRepository<Formc4netvolumedetail> DetalleC4Repository;
        private readonly IBaseRepository<Formc4totalvolumedetail> TotalC4Repository;
        private readonly ITelemetryException TelemetryExcepcion;

        public C4(
            IForma4Repository _forma,
            IBaseRepository<Formc4netvolumedetail> detalleC4Repository,
            IBaseRepository<Formc4totalvolumedetail> totalC4Repository,
            ITelemetryException telemetryExcepcion
            )
        {
            RepositoryC4 = _forma;
            DetalleC4Repository = detalleC4Repository;
            TotalC4Repository = totalC4Repository;
            TelemetryExcepcion = telemetryExcepcion;
        }

        public async Task<Aprobacion<DetalleForma4>> JsonFormat(Aprobacioncarga data, string getUserid)
        {
            var header = await GenerarHeader(data.IdForma.Value, getUserid);
            return header;
        }

        private async Task<Aprobacion<DetalleForma4>> GenerarHeader(Guid id, string getUserid)
        {
            Aprobacion<DetalleForma4> jsonForm = new Aprobacion<DetalleForma4>();

            try
            {
                var concret = await RepositoryC4.GetConcreForm(id);
                var formc4 = await RepositoryC4.GetFormC4(id);
                var formacion = await DetalleC4Repository.GetAsync(predicate: x => x.Formid == id);
                var detalles = await DetalleC4Repository.GetAllAsync(predicate: x => x.Formid == id);
                var totales = await TotalC4Repository.GetAsync(predicate: x => x.Formid == id);

                CultureInfo cultureInfo = new CultureInfo("es-co");
                string dateForma = $"{concret.Month}/{concret.Year}";
                DateTime dateTime = DateTime.Parse(dateForma, cultureInfo);
                int day = dateTime.AddMonths(1).AddDays(-1).Day;
                DateTime volumDate = new DateTime(dateTime.Year, dateTime.Month, day);

                List<DetalleForma4> listC4 = new List<DetalleForma4>();
                foreach (var item in detalles)
                {
                    if (item.Activity == "007")
                    {
                        listC4.Add(Producion(item, volumDate, formc4, totales));
                    }

                    if (item.Activity == "018")
                    {
                        listC4.Add(Consumos(item, volumDate, formc4));
                    }
                    if (item.Activity == "032")
                    {
                        listC4.Add(Perdidas(item, volumDate, formc4));
                    }
                    if (item.Activity == "030")
                    {
                        listC4.Add(Gravable(item, volumDate, formc4));
                    }
                }


                jsonForm = new Aprobacion<DetalleForma4>()
                {
                    FORMA_CODIGO = 4,
                    FORMA_NOMBRE = "Forma 4",
                    OPERADOR_ID = concret.Pdenid,
                    OPERADOR = concret.Company,
                    CONTRATO = formc4.Contrato,
                    CONTRATO_ID = formc4.ContratoId,
                    CAMPO_ID = formc4.CampoId,
                    CAMPO = formc4.Campo,
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
                    ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd"),
                    ROW_CREATED_BY = $"FRONTALBDP/{getUserid}",
                    ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd"),
                    DETALLE = listC4,
                    OBSERVACIONES = "",
                };

            }
            catch (Exception e)
            {
                TelemetryExcepcion.RegisterException(e);
            }

            return jsonForm;
        }

        private DetalleForma4 Producion(Formc4netvolumedetail item, DateTime volumDate, Formc4 formc4, dynamic totales)
        {
            decimal total = (decimal.Parse(item.Basica) + decimal.Parse(item.Incremental));
            DetalleForma4 detail = new DetalleForma4()
            {
                PDEN_ID = item.PdenId,
                PDEN_TYPE = "PDEN_LAND_RIGHT",
                PDEN_SOURCE = "OFFICIAL",
                VOLUME_METHOD = "1005",
                ACTIVITY_TYPE = "007",
                PERIOD_TYPE = "004",
                VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                AMENDMENT_SEQ_NO = "0",
                ACTIVE_IND = "Y",
                EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                EXPIRY_DATE = "20501231",
                EXISTENCIA_INI = formc4.Initialstock?.ToString(),
                EXISTENCIA_FIN = formc4.Finalexistence?.ToString(),
                API = formc4.Apigrades?.ToString(),
                PROD_BAS = item.Basica?.ToString(),
                PROD_INC = item.Incremental?.ToString(),
                PROD_TOTAL = total.ToString(),
                CONSUMO_BAS = "",
                CONSUMO_INC = "",
                CONSUMO_TOTAL = "",
                PERDIDAS_BAS = "",
                PERDIDAS_INC = "",
                PERDIDAS_TOTAL = "",
                PERDIDAS_NG_BAS = "",
                PERDIDAS_NG_INC = "",
                PERDIDAS_NG_TOTAL = "",
                ENTREGAS = formc4.Deliverysite?.ToString(),
                CRU_MUERTO_VASIJAS = "",
                CRU_MUERTO_LINEAS = "",
                MUNICIPIO = item.Municipality,
                CODIGO_DANE = item.Danecode,
                PROD_MUN_BAS = item.Basica?.ToString(),
                PROD_MUN_INC = item.Incremental?.ToString(),
                PROD_MUN_TOTAL = totales.Totalvolume?.ToString(),
                CONTENIDO_S = formc4.Sulfurcontent?.ToString(),
                ECP_BSW = formc4.Bsw?.ToString(),
                ROW_CHANGED_BY = $"FRONTALBDP/{formc4.row_changed_by}",
                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{formc4.row_created_by}",
                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
            };
            return detail;
        }

        private DetalleForma4 Consumos(Formc4netvolumedetail item, DateTime volumDate, Formc4 formc4)
        {
            decimal total = (decimal.Parse(item.Basica) + decimal.Parse(item.Incremental));
            DetalleForma4 detail = new DetalleForma4()
            {
                PDEN_ID = item.PdenId,
                PDEN_TYPE = "PDEN_LAND_RIGHT",
                PDEN_SOURCE = "OFFICIAL",
                VOLUME_METHOD = "1005",
                ACTIVITY_TYPE = "018",
                PERIOD_TYPE = "004",
                VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                AMENDMENT_SEQ_NO = "0",
                ACTIVE_IND = "Y",
                EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                EXPIRY_DATE = "20501231",
                EXISTENCIA_INI = "",
                EXISTENCIA_FIN = "",
                API = "",
                PROD_BAS = "",
                PROD_INC = "",
                PROD_TOTAL = "",
                CONSUMO_BAS = item.Basica?.ToString(),
                CONSUMO_INC = item.Incremental?.ToString(),
                CONSUMO_TOTAL = total.ToString(),
                PERDIDAS_BAS = "",
                PERDIDAS_INC = "",
                PERDIDAS_TOTAL = "",
                PERDIDAS_NG_BAS = "",
                PERDIDAS_NG_INC = "",
                PERDIDAS_NG_TOTAL = "",
                ENTREGAS = "",
                CRU_MUERTO_VASIJAS = "",
                CRU_MUERTO_LINEAS = "",
                MUNICIPIO = item.Municipality,
                CODIGO_DANE = item.Danecode,
                PROD_MUN_BAS = "",
                PROD_MUN_INC = "",
                PROD_MUN_TOTAL = "",
                CONTENIDO_S = "",
                ECP_BSW = "",
                ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
            };

            return detail;
        }

        private DetalleForma4 Perdidas(Formc4netvolumedetail item, DateTime volumDate, Formc4 formc4)
        {
            decimal total = (decimal.Parse(item.Basica) + decimal.Parse(item.Incremental));
            DetalleForma4 detail = new DetalleForma4()
            {
                PDEN_ID = item.PdenId,
                PDEN_TYPE = "PDEN_LAND_RIGHT",
                PDEN_SOURCE = "OFFICIAL",
                VOLUME_METHOD = "1005",
                ACTIVITY_TYPE = "032",
                PERIOD_TYPE = "004",
                VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                AMENDMENT_SEQ_NO = "0",
                ACTIVE_IND = "Y",
                EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                EXPIRY_DATE = "20501231",
                EXISTENCIA_INI = "",
                EXISTENCIA_FIN = "",
                API = "",
                PROD_BAS = "",
                PROD_INC = "",
                PROD_TOTAL = "",
                CONSUMO_BAS = "",
                CONSUMO_INC = "",
                CONSUMO_TOTAL = "",
                PERDIDAS_BAS = item.Basica.ToString(),
                PERDIDAS_INC = item.Incremental.ToString(),
                PERDIDAS_TOTAL = total.ToString(),
                PERDIDAS_NG_BAS = "",
                PERDIDAS_NG_INC = "",
                PERDIDAS_NG_TOTAL = "",
                ENTREGAS = "",
                CRU_MUERTO_VASIJAS = "",
                CRU_MUERTO_LINEAS = "",
                MUNICIPIO = item.Municipality,
                CODIGO_DANE = item.Danecode,
                PROD_MUN_BAS = "",
                PROD_MUN_INC = "",
                PROD_MUN_TOTAL = "",
                CONTENIDO_S = "",
                ECP_BSW = "",
                ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
            };

            return detail;
        }

        private DetalleForma4 Gravable(Formc4netvolumedetail item, DateTime volumDate, Formc4 formc4)
        {
            decimal total = (decimal.Parse(item.Basica) + decimal.Parse(item.Incremental));
            DetalleForma4 detail = new DetalleForma4()
            {
                PDEN_ID = item.PdenId,
                PDEN_TYPE = "PDEN_LAND_RIGHT",
                PDEN_SOURCE = "OFFICIAL",
                VOLUME_METHOD = "1005",
                ACTIVITY_TYPE = "030",
                PERIOD_TYPE = "004",
                VOLUME_DATE = volumDate.ToString("yyyyMMdd"),
                AMENDMENT_SEQ_NO = "0",
                ACTIVE_IND = "Y",
                EFFECTIVE_DATE = volumDate.ToString("yyyyMMdd"),
                EXPIRY_DATE = "20501231",
                EXISTENCIA_INI = "",
                EXISTENCIA_FIN = "",
                API = "",
                PROD_BAS = "",
                PROD_INC = "",
                PROD_TOTAL = "",
                CONSUMO_BAS = "",
                CONSUMO_INC = "",
                CONSUMO_TOTAL = "",
                PERDIDAS_BAS = "",
                PERDIDAS_INC = "",
                PERDIDAS_TOTAL = "",
                PERDIDAS_NG_BAS = "",
                PERDIDAS_NG_INC = "",
                PERDIDAS_NG_TOTAL = "",
                ENTREGAS = "",
                CRU_MUERTO_VASIJAS = "",
                CRU_MUERTO_LINEAS = "",
                MUNICIPIO = item.Municipality,
                CODIGO_DANE = item.Danecode,
                PROD_MUN_BAS = item.Basica.ToString(),
                PROD_MUN_INC = item.Incremental.ToString(),
                PROD_MUN_TOTAL = total.ToString(),
                CONTENIDO_S = "",
                ECP_BSW = "",
                ROW_CHANGED_BY = $"FRONTALBDP/{item.row_changed_by}",
                ROW_CHANGED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                ROW_CREATED_BY = $"FRONTALBDP/{item.row_created_by}",
                ROW_CREATED_DATE = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyyMMdd HH:mm:ss"),
                TO_PDEN_ID = "",
                TO_PDEN_TYPE = "",
                TO_PDEN_SOURCE = "",
            };

            return detail;
        }
    }
}
