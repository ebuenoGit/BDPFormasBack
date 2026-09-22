using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Validaciones
{
    public class CabeceraFormas<T>
    {
        public string FORMA_CODIGO { set; get; }
        public string OPERADOR { set; get; }
        public string OPERADOR_ID { set; get; }
        public string CONTRATO_ID { set; get; }
        public string CONTRATO { set; get; }
        public string CAMPO_ID { set; get; }
        public string CAMPO { set; get; }
        public string ESTRUCTURA_ID { set; get; }
        public string BLOQUE_ID { set; get; }
        public string FORMACION_ID { set; get; }
        public string FORMACION_SET_ID { set; get; }
        public string FORMACION { set; get; }
        public string MIEMBRO_ID { set; get; }
        public string YACIMIENTO_ID { set; get; }
        public string YACIMIENTO { set; get; }
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string MODALIDADEXPLOTACION_ID { set; get; }
        public string RECARGAR { set; get; }
        public List<T> REGISTRO { set; get; }
    }
}
