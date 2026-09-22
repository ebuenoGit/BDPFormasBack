using Backend.Formas.Entities.DTO;
using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{
    public class FormaCuadro1A
    {
        public infoFormaCuadro1A info { get; set; }
        public List<dataFormaCuadro1A> data { get; set; }
        public totalFormaCuadro1A total { get; set; }
        public List<ErrorFormasStructura> errors { get; set; }
        public bool permisoDeCargue { get; set; }

    }

    public class infoFormaCuadro1A
    {
        public string mes { get; set; }
        public string anio { get; set; }
        public string compania_id { get; set; }
        public string compania { get; set; }
        public string lugar { get; set; }
        public string tanque { get; set; }
        public string bateria { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string PDEN_ID_TANQUE { get; set; }
        public string PDEN_ID_BATERIA { get; set; }
    }

    public class dataFormaCuadro1A
    {
        public string dias { get; set; }
        public decimal medidaMM { get; set; }
        public decimal aforoBLS { get; set; }
        public decimal tempF { get; set; }
        public decimal factorTemp { get; set; }
        public decimal BLS60F { get; set; }
        public decimal BSW { get; set; }
        public decimal factorBSW { get; set; }
        public decimal CTSH { get; set; }
        public decimal tempAmb { get; set; }
        public decimal BLSNetos { get; set; }
        public decimal transfBLS { get; set; }
        public decimal recibidoBLS { get; set; }
        public decimal entregaBLS { get; set; }
        public decimal API60F { get; set; }
        public decimal GE { get; set; }
        public decimal netosGE { get; set; }
        public decimal salBTB { get; set; }
        public string PDEN_ID_TANQUE { get; set; }
        public string PDEN_ID_BATERIA { get; set; }
        public decimal TrasRecibido { set; get; }
        public decimal TrasEnvio { set; get; }
        public decimal MovIntraRecibido { set; get; }
        public decimal MovIntraEnvio { set; get; }
    }

    public class totalFormaCuadro1A
    {
        public decimal BLS60F { get; set; }
        public decimal BLSNetos { get; set; }
        public decimal recibidoBLS { get; set; }
        public decimal entregaBLS { get; set; }
    }

    public class FormaCuadro1Validador
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
        public List<FormaCuadro1ValidadorDetalle> REGISTRO { get; set; }
    }

    public class FormaCuadro1ValidadorDetalle
    {
        public string TANQUE { get; set; }
        public string BATERIA { get; set; }
    }

    public class cabeCuadro1
    {
        public Guid form_id { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
        public int compania_id { get; set; }
        public string compania { get; set; }
        public int contrato_id { get; set; }
        public string contrato { get; set; }
        public int campo_id { get; set; }
        public string campo { get; set; }
        public string lugar { get; set; }
        public string tanque { get; set; }
        public string bateria { get; set; }
        public string usuario { get; set; }
        public string PDEN_ID_TANQUE { get; set; }
        public string PDEN_ID_BATERIA { get; set; }
    }

    public class detaCuadro1
    {
        public Guid id_cabecera { get; set; }
        public Guid form_id { get; set; }
        public string dias { get; set; }
        public decimal medidaMM { get; set; }
        public decimal aforoBLS { get; set; }
        public decimal tempF { get; set; }
        public decimal factorTemp { get; set; }
        public decimal BLS60F { get; set; }
        public decimal BSW { get; set; }
        public decimal factorBSW { get; set; }
        public decimal CTSH { get; set; }
        public decimal tempAmb { get; set; }
        public decimal BLSNetos { get; set; }
        public decimal transfBLS { get; set; }
        public decimal recibidoBLS { get; set; }
        public decimal entregaBLS { get; set; }
        public decimal API60F { get; set; }
        public decimal GE { get; set; }
        public decimal netosGE { get; set; }
        public decimal salBTB { get; set; }
        public string usuario { get; set; }
        public string PDEN_ID_TANQUE { get; set; }
        public string PDEN_ID_BATERIA { get; set; }
        public decimal ECP_BALANCE_RECEIVED { set; get; }
        public decimal ECP_BALANCE_SENT { set; get; }
        public decimal ECP_INTRADIARY_MOV_RECEIVED { set; get; }
        public decimal ECP_INTRADIARY_MOV_SENT { set; get; }
    }


    public class Cuadro1EstrucValCab
    {
        public cuadro1FormaVal FORMAS { get; set; }
    }

    public class cuadro1FormaVal
    {
        public Cuadro1registroVal FORMA { get; set; }
    }

    public class Cuadro1registroVal
    {
        public Cuadro1Detallepden REGISTRO { get; set; }
    }

    public class Cuadro1Detallepden
    {
        public string TANQUE { get; set; }
        public string PDEN_ID_TANQUE { get; set; }
        public string BATERIA { get; set; }
        public string PDEN_ID_BATERIA { get; set; }
    }

    public class rootValCuadro1
    {
        public rootElementValCuadro1 root { get; set; }
    }

    public class rootElementValCuadro1
    {
        public elementVacuadro1 element { get; set; }
    }

    public class elementVacuadro1
    {
        public string FORMA_CODIGO { get; set; }
        public string MENSAJE { get; set; }
    }

}


