using System.Collections.Generic;

namespace Backend.Formas.Entities.DTOI
{
    public class jsonForma30Aprobador
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
        public List<DetalleAprobador30> DETALLE { get; set; }
    }

    public class DetalleAprobador30
    {
        public string PDEN_ID { get; set; }
        public string PDEN_TYPE { get; set; }
        public string PDEN_SOURCE { get; set; }
        public string VOLUME_METHOD { get; set; }
        public string VOLUME_DATE { get; set; }
        public string PERIOD_TYPE { get; set; }
        public string AMENDMENT_SEQ_NO { get; set; }
        public string ACTIVE_IND { get; set; }
        public string ACTIVE_TYPE { get; set; }
        public string REAL_TP { get; set; }
        public string REAL_OIL { get; set; }
        public string REAL_BAS { get; set; }
        public string REAL_INC { get; set; }
        public string GAS_PROCESADO_TP { get; set; }
        public string GAS_VOLUME { get; set; }
        public string GP_BUTANO { get; set; }
        public string GP_GASOLINA { get; set; }
        public string GP_PROPANO { get; set; }
        public string GAS_PROCESADO { get; set; }
        public string GAS_QUEMADO_TP { get; set; }
        public string GAS_QUEMADO { get; set; }
        public string GLP_TP { get; set; }
        public string GLP { get; set; }
        public string PROPANO_TP { get; set; }
        public string PROPANO { get; set; }
        public string PRODUCT_TYPE { get; set; }
        public string BUTANO_TP { get; set; }
        public string BUTANO { get; set; }
        public string GASOLINA_TP { get; set; }
        public string GASOLINA { get; set; }
        public string APIASOL_TP { get; set; }
        public string APIASOL { get; set; }
        public string CONDENSADO_TP { get; set; }
        public string CONDENSADO { get; set; }
        public string CONSUMOS_TP { get; set; }
        public string CONSUMOS { get; set; }
        public string GASODUCTOS_URBANOS_TP { get; set; }
        public string GASODUCTOS_URBANOS { get; set; }
        public string GENERACION_ELECTRICA_TP { get; set; }
        public string GENERACION_ELECTRICA { get; set; }
        public string OTRAS_VENTAS_TP { get; set; }
        public string OTRAS_VENTAS { get; set; }
        public string GAS_TRANFERENCIA_TP { get; set; }
        public string GAS_TRANFERENCIA { get; set; }
        public string GAS_NEUMATICO_TP { get; set; }
        public string GAS_NEUMATICO { get; set; }
        public string GAS_INYECTADO_TP { get; set; }
        public string GAS_INYECTADO { get; set; }
        public string TOTAL_PROC_PLANTA_TP { get; set; }
        public string TOTAL_PROC_PLANTA { get; set; }
        public string GP_TRATOTAL_PROC_PLANTA_TP { get; set; }
        public string GP_TRATOTAL_PROC_PLANTA { get; set; }
        public string GP_TRANSFORMADO_TP { get; set; }
        public string GP_TRANSFORMADO { get; set; }
        public string GP_CONSUMOS_TP { get; set; }
        public string GP_CONSUMOS { get; set; }
        public string GP_GASODUCTOS_URBANOS_TP { get; set; }
        public string GP_GASODUCTOS_URBANOS { get; set; }
        public string GP_GENERACION_ELECTRICA_TP { get; set; }
        public string GP_GENERACION_ELECTRICA { get; set; }
        public string GP_OTRAS_VENTAS_TP { get; set; }
        public string GP_OTRAS_VENTAS { get; set; }
        public string GP_NEUMATICO_TP { get; set; }
        public string GP_NEUMATICO { get; set; }
        public string GP_QUEMADO_TP { get; set; }
        public string GP_QUEMADO { get; set; }
        public string GP_INYECTADO_TP { get; set; }
        public string GP_INYECTADO { get; set; }
        public string OBSERVACIONES { get; set; }
        public string REPRESENTA_OPER { get; set; }
        public string REPRESENTA_ANH { get; set; }
        public string IDFORMA { get; set; }
        public string ESTADOFORMA { get; set; }
        public string ROW_CHANGED_BY { get; set; }
        public string ROW_CHANGED_DATE { get; set; }
        public string ROW_CREATED_BY { get; set; }
        public string ROW_CREATED_DATE { get; set; }
        public string GP_TRANSFORMADO_GASOLINA {set;get;}
    }
}
