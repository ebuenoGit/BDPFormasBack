using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{


    public class FormasCabecera
    {
        public string FORMA_CODIGO { get; set; }
        public string FORMA_NOMBRE { get; set; }
        public string MES { get; set; }
        public string ANIO { get; set; }
        public string OPERADOR_ID { get; set; }
        public string OPERADOR { get; set; }
        public string CONTRATO_ID { get; set; }
        public string CONTRATO { get; set; }
        public string CAMPO_ID { get; set; }
        public string CAMPO { get; set; }
        public string BATERIA_ID { get; set; }
        public string BATERIA { get; set; }
        public string TANQUE_ID { get; set; }
        public string TANQUE { get; set; }
        public string ESTRUCTURA_ID { get; set; }
        public string ESTRUCTURA { get; set; }
        public string BLOQUE_ID { get; set; }
        public string BLOQUE { get; set; }
        public string FORMACION_ID { get; set; }
        public string FORMACION_SET_ID { get; set; }
        public string FORMACION { get; set; }
        public string MIEMBRO_ID { get; set; }
        public string MIEMBRO { get; set; }
        public string YACIMIENTO_ID { get; set; }
        public string YACIMIENTO { get; set; }
        public string MODALIDADEXPLOTACION_ID { get; set; }
        public string MODALIDADEXPLOTACION { get; set; }
        public string REPRESENTANTE_OPERADOR_NM { get; set; }
        public string REPRESENTANTE_OPERADOR_TP { get; set; }
        public string REPRESENTANTE_ANH_NAME { get; set; }
        public string REPRESENTANTE_ANH_TP { get; set; }
        public string GENERADO_DESDE { get; set; }
        public string ROW_CHANGED_BY { get; set; }
        public string ROW_CHANGED_DATE { get; set; }
        public string ROW_CREATED_BY { get; set; }
        public string ROW_CREATED_DATE { get; set; }
        public string OBSERVACIONES { get; set; }
        public List<DetalleForma20SH> DETALLE { get; set; }
    }

    public class DetalleForma20SH
    {
        public string PDEN_ID { get; set; }
        public string PDEN_TYPE { get; set; }
        public string PDEN_SOURCE { get; set; }
        public string VOLUME_METHOD { get; set; }
        public string ACTIVITY_TYPE { get; set; }
        public string PERIOD_TYPE { get; set; }
        public string VOLUME_DATE { get; set; }
        public string AMENDMENT_SEQ_NO { get; set; }
        public string ACTIVE_IND { get; set; }
        public string EFFECTIVE_DATE { get; set; }
        public string EXPIRY_DATE { get; set; }
        public string PERIOD_ON_INJECTION { get; set; }
        public string PERIOD_ON_INJECTION_OUOM { get; set; }
        public string INJECTION_PRESSURE { get; set; }
        public string PRIMARY_PRODUCT { get; set; }
        public string EC_INJECTION_VOLUME { get; set; }
        public string EC_INJECTION_VOLUME_OUOM { get; set; }
        public string EC_INJECTION_CUM_VOLUME { get; set; }
        public string ROW_CHANGED_BY { get; set; }
        public string ROW_CHANGED_DATE { get; set; }
        public string ROW_CREATED_BY { get; set; }
        public string ROW_CREATED_DATE { get; set; }
        public string ECP_CUM_PERIOD_ON_PRODUCTION { get; set; }
    }

}
