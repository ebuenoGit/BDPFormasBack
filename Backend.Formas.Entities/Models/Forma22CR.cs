using Backend.Formas.Entities.DTO;
using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{
    public class Forma22CR
    {
        public infoForm22CR info { get; set; }
        public List<dataForm22CR> data { get; set; }
        public List<ErrorFormasStructura> errors { get; set; }
        public bool permisoDeCargue { get; set; }
    }

    public class Forma22CrReques
    {
        public infoForm22CR info { get; set; }
        public List<dataForm22CR> data { get; set; }
    }

    public class infoForm22CR
    {
        public string operador {set; get ;}
        public string operador_id {set;get;}
        public string compania_id { get; set; }
        public string compania { get; set; }
        public string formacion { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string bloque { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public string yacimiento { get; set; }
        public string yacimiento_id { get; set; }
        public string estructura { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
        public string pden_id { get; set; }
    }

    public class dataForm22CR
    {
        public string Pozo { get; set; }
        public string mes { get; set; }
        public decimal petroleoProducidoMensual { get; set; }
        public decimal petroleoProducidoAcumulado { get; set; }
        public decimal aguaInyectadoMensual { get; set; }
        public decimal aguaInyectadoAcumulado { get; set; }
        public decimal aguaProducidoMensual { get; set; }
        public decimal aguaProducidoAcumulado { get; set; }
        public decimal gasInyectadoMensual { get; set; }
        public decimal gasInyectadoAcumulado { get; set; }
        public decimal gasProducidoMensual { get; set; }
        public decimal gasProducidoAcumulado { get; set; }
        public decimal presionFondo { get; set; }
        public string pden_id { get; set; }
    }

    public class valCabForm22CR
    {
        public string FORMA_CODIGO { get; set; }
        public string OPERADOR { get; set; }
        public string OPERADOR_ID { get; set; }
        public string CONTRATO_ID { get; set; }
        public string CONTRATO { get; set; }
        public string CAMPO_ID { get; set; }
        public string CAMPO { get; set; }
        public string ESTRUCTURA_ID { get; set; }
        public string BLOQUE_ID { get; set; }
        public string FORMACION_ID { get; set; }
        public string FORMACION_SET_ID { get; set; }
        public string FORMACION { get; set; }
        public string MIEMBRO_ID { get; set; }
        public string YACIMIENTO_ID { get; set; }
        public string ANIO { get; set; }
        public string MES { get; set; }
        public string RECARGA { get; set; }
        public string MODALIDADEXPLOTACION_ID { get; set; }
        public List<valDetForm22CR> REGISTRO { get; set; }
    }

    public class valDetForm22CR
    {
        public string MES { get; set; }
        public string POZO_YACIMIENTO_UWI { get; set; }
    }


    public class validadorForma22CR
    {
        public valFormasForm22CR FORMAS { get; set; }
    }

    public class valFormasForm22CR
    {
        public valFormaForm22CR FORMA { get; set; }
    }

    public class valFormaForm22CR
    {
        public List<valregistroForm22CR> REGISTRO { get; set; }
    }
    public class valregistroForm22CR
    {
        public string POZO_YACIMIENTO_UWI { get; set; }
        public string PDEN_ID { get; set; }
    }

    public class validadorForma22CRUni
    {
        public valFormasForm22CRUni FORMAS { get; set; }
    }

    public class valFormasForm22CRUni
    {
        public valFormaForm22CRUni FORMA { get; set; }
    }

    public class valFormaForm22CRUni
    {
        public valregistroForm22CRUni REGISTRO { get; set; }
    }
    public class valregistroForm22CRUni
    {
        public string POZO_YACIMIENTO_UWI { get; set; }
        public string PDEN_ID { get; set; }
    }
}
