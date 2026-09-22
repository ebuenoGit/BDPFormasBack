using Backend.Formas.Entities.DTO;
using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{
    public class Forma17CR
    {
        public InfoForma17CR info { get; set; }
        public List<dataForma17CR> data { get; set; }
        public List<ErrorFormasStructura> errors { get; set; }
        public bool permisoDeCargue { get; set; }
    }

    public class InfoForma17CR
    {
        public string concesion { get; set; }
        public string operador_id { get; set; }
        public string operador { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public string estructura { get; set; }
        public string formacion { get; set; }
        public string bloque { get; set; }
        public string yacimiento { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
    }

    public class dataForma17CR
    {
        public string pozo { get; set; }
        public decimal diasEnElMes { get; set; }
        public decimal diasAcumulados { get; set; }
        public decimal produccionGasMCPDiaria { get; set; }
        public decimal produccionGasMCPMensual { get; set; }
        public decimal produccionGasMCPAcumulada { get; set; }
        public decimal produccionAguaMensual { get; set; }
        public decimal produccionAguaAcumulada { get; set; }
        public string estadoPozosFinalMes { get; set; }
        public string pden_id { get; set; }
    }


    public class CabValForma17CR
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
        public List<DetValForma17CR> REGISTRO { get; set; }
    }

    public class DetValForma17CR
    {
        public string POZO { get; set; }
        public string FORMACION { get; set; }
    }


    public class CabForma17Val
    {
        public formaForma17 FORMAS { get; set; }
    }

    public class formaForma17
    {
        public regiForma17reg FORMA { get; set; }
    }

    public class regiForma17reg
    {
        public registroForma17 REGISTRO { get; set; }
    }

    public class registroForma17
    {
        public string POZO { get; set; }
        public string FORMACION { get; set; }
        public string PDEN_ID { get; set; }
        public string FORMACION_PRODUCTORA { get; set; }
    }

    public class rootVal17
    {
        public rootElementVal17 root { get; set; }
    }

    public class rootElementVal17
    {
        public elementVal7 element { get; set; }
    }

    public class elementVal7
    {
        public string POZO { get; set; }
        public string MENSAJE { get; set; }
    }




    public class CabForma17Valvarios
    {
        public formaForma17varios FORMAS { get; set; }
    }

    public class formaForma17varios
    {
        public regiForma17regvarios FORMA { get; set; }
    }

    public class regiForma17regvarios
    {
        public List<registroForma17varios> REGISTRO { get; set; }
    }

    public class registroForma17varios
    {
        public string POZO { get; set; }
        public string FORMACION { get; set; }
        public string PDEN_ID { get; set; }
        public string FORMACION_PRODUCTORA { get; set; }
    }

}
