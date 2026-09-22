using System.Collections.Generic;

namespace Backend.Formas.Entities.DTOI
{
    public class Aprobacion<T>
    {
        public int FORMA_CODIGO { set; get; }
        public string FORMA_NOMBRE { set; get; }
        public string OPERADOR_ID { set; get; }
        public string OPERADOR { set; get; }
        public string CONTRATO { set; get; }
        public string CONTRATO_ID { set; get; }
        public string CAMPO_ID { set; get; }
        public string CAMPO { set; get; }
        public string ESTRUCTURA_ID { set; get; }
        public string BLOQUE_ID { set; get; }
        public string BATERIA_ID { set; get; }
        public string BATERIA { set; get; }
        public string FORMACION_ID { set; get; }
        public string FORMACION { set; get; }
        public string FORMACION_SET_ID { set; get; }
        public string MIEMBRO_ID { set; get; }
        public string YACIMIENTO_ID { set; get; }
        public string MES { set; get; }
        public string ANIO { set; get; }
        public string MODALIDADEXPLOTACION_ID { set; get; }
        public string MODALIDADEXPLOTACION { set; get; }
        public string REPRESENTANTE_OPERADOR_NM { set; get; }
        public string REPRESENTANTE_OPERADOR_TP { set; get; }
        public string REPRESENTANTE_ANH_NAME { set; get; }
        public string REPRESENTANTE_ANH_TP { set; get; }
        public string GENERADO_DESDE { set; get; }
        public string ROW_CHANGED_BY { set; get; }
        public string ROW_CHANGED_DATE { set; get; }
        public string ROW_CREATED_BY { set; get; }
        public string ROW_CREATED_DATE { set; get; }
        public string OBSERVACIONES { set; get; }
        public string TANQUE_ID { set; get; }
        public string TANQUE { set; get; }
        public string ESTRUCTURA { set; get; }
        public string BLOQUE { set; get; }
        public string MIEMBRO { set; get; }
        public string YACIMIENTO { set; get; }
        public List<T> DETALLE { set; get; }
    }
}
