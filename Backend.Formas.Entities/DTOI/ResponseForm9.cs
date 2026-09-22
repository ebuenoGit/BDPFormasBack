using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTOI
{
    public class ResponseForm9
    {
        public Formas FORMAS { set; get; }
        public Root root { set; get; }
    }
    public class ResponseForm9Json
    {
        public FormasJson FORMAS { set; get; }
        public Root root { set; get; }
    }

    public class Formas
    {
        public Forma FORMA { set; get; }
    }

    public class FormasJson
    {
        public FormaJson FORMA { set; get; }
    }
    public class Root
    {
        public Element element { set; get; }
    }
    public class Forma
    {
        public List<Registro> REGISTRO { set; get; }
    }

    public class FormaJson
    {
        public Registro REGISTRO { set; get; }

    }
    public class Registro
    {
        public string POZO { set; get; }
        public string POZO_YACIMIENTO { set; get; }
        public string MUNICIPIO { set; get; }
        public string PDEN_ID { set; get; }
        public string COD_MUNICIPIO { set; get; }
        public string VALIDACION_PRODUCCION { set; get; }
        public string CRUDO { set; get; }
        public string AGUA { set; get; }
        public string GAS { set; get; }
        public string CAMPO { get; set; }

    }


    public class Element
    {
        public string FORMA_CODIGO { set; get; }
        public string MENSAJE { set; get; }
        public string POZO { set; get; }
    }


    public class VALIDACIONES
    {
        public string Columna { get; set; }
        public string Regla { get; set; }
        public int Codigo { get; set; }
        public int Tipo { get; set; }
        public string Mensaje { get; set; }
        public string ColumnaBDP { get; set; }
        public string DatoBDP { get; set; }

    }
    public class REGISTROS_F9
    {
        public string LLAVE { get; set; }
        public IList<VALIDACIONES> VALIDACIONES { get; set; }

    }
    public class RespuestaForma
    {
        public string FORMA { get; set; }
        public IList<REGISTROS_F9> REGISTROS { get; set; }

    }

}
