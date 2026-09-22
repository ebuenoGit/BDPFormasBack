using Backend.Formas.Entities.DTO;
using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{
    public class Forma15CR
    {
        public infoForma15CR info { get; set; }
        public List<dataForma15CR> data { get; set; }
        public List<ErrorFormasStructura> errors { get; set; }
        public bool permisoDeCargue { get; set; }
    }

    public class infoForma15CR
    {
        public string compania_id { get; set; }
        public string compania { get; set; }
        public string concesion { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
        public string row_created_by { get; set; }
        public DateTime row_created_date { get; set; }
        public string row_changed_by { get; set; }
        public DateTime row_changed_date { get; set; }
        public string PDEN_ID { get; set; }
    }

    public class dataForma15CR
    {
        public string PDEN_ID { get; set; }
        public string valInyecPozo { get; set; }
        public string valInyecFormacionProductora { get; set; }
        public string valInyecMetodoProduccion { get; set; }
        public decimal valInyecPresionInyeccion { get; set; }
        public decimal valInyecCiclo { get; set; }
        public decimal valInyecDiasMes { get; set; }
        public decimal valInyecDiasAcumulados { get; set; }
        public decimal valInyecLibrasMes { get; set; }
        public decimal valInyecLibrasAcumulados { get; set; }
        public decimal valInyecBTUMes { get; set; }
        public decimal valInyecBTUAcumulados { get; set; }
        public string valInyecCalidadVapor { get; set; }
        public decimal produccionPetroleoBlsNetosMensual { get; set; }
        public decimal produccionPetroleoBlsNetosAcumulado { get; set; }
        public decimal produccionAguaBlsMensual { get; set; }
        public decimal produccionAguaBlsAcumulado { get; set; }
        public string row_created_by { get; set; }
        public DateTime row_created_date { get; set; }
        public string row_changed_by { get; set; }
        public DateTime row_changed_date { get; set; }
    }

    public class ValCabForma15CR
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
        public List<DetValForma15CR> REGISTRO { get; set; }
    }

    public class DetValForma15CR
    {
        public string POZO { get; set; }
        public string FORMACION { get; set; }
        public string METODO_PRODUCCION { get; set; }
    }

    public class valFormasForm15CRCab
    {
        public formaForm15cr FORMAS { get; set; }
    }

    public class formaForm15cr
    {
        public registroforma15cr FORMA { get; set; }
    }

    public class registroforma15cr
    {
        public valFormasForma15CR REGISTRO { get; set; }
    }
    public class valFormasForma15CR
    {


        public string POZO { get; set; }
        public string FORMACION { get; set; }
        public string PDEN_ID { get; set; }
        public string FORMACION_PRODUCTORA { get; set; }
    }

    public class rootVal15
    {
        public rootElementVal15 root { get; set; }
    }

    public class rootElementVal15
    {
        public List<elementVal5> element { get; set; }
    }

    public class elementVal5
    {
        public string POZO { get; set; }
        public string MENSAJE { get; set; }
    }
}
