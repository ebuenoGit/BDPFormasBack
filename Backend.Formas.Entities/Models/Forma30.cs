using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{

    public class Forma30Data
    {
        public Forma30 Data { get; set; }
        public List<Errores> errors { get; set; }
        public bool permisoDeCargue { get; set; }
    }

    public class Forma30
    {
        /* public info info { get; set; }
         public List<data> data { get; set; }*/
        public List<fileJSON> Archivo { get; set; }
    }

    public class fileJSON
    {
        public info info { get; set; }
        public List<data> data { get; set; }
        public List<RegisValidadorForma30> FORMAS { get; set; }
    }

    public class info
    {
        public string operador_id { get; set; }
        public string operador { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string UsuarioAprobador { get; set; }
        public string UsuarioNombreAprobador { get; set; }
        public string pden_id { get; set; }
        public string mes { set; get; }
        public string anio { set; get; }
    }
    public class data
    {
        public string pden_id { get; set; }
        public string volume_date { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public decimal gasFormacionKPC { get; set; }
        public decimal contenidoPropano { get; set; }
        public decimal contenidoButano { get; set; }
        public decimal contenidoGasolinaNatural { get; set; }
        public decimal gasFormacionProcesado { get; set; }
        public decimal consumoEnCampo { get; set; }
        public decimal generacionElectrica { get; set; }
        public decimal otros { get; set; }
        public decimal quemadoAire { get; set; }
        public decimal usadoBombeoNeumatico { get; set; }
        public decimal inyectadoYacimiento { get; set; }
        public decimal productosObtenidosGasTotalGasProcesadoPlanta { get; set; }
        public decimal productosObtenidosGasPropano { get; set; }
        public decimal productosObtenidosGasButano { get; set; }
        public decimal productosObtenidosGasGasolina { get; set; }
        public decimal productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos { get; set; }
        public decimal productosGasProcesadoConsumoEnCampo { get; set; }
        public decimal productosGasProcesadoGasoductosUrbanos { get; set; }
        public decimal productosGasProcesadoGeneracionElectrica { get; set; }
        public decimal productosGasProcesadoOtros { get; set; }
        public decimal productosGasProcesadoQuemadoAire { get; set; }
        public decimal productosGasProcesadoUsadoBombeoNeumatico { get; set; }
        public decimal productosGasProcesadoInyectadoYacimiento { get; set; }
        public string productosGasProcesadoObservaciones { get; set; }
        public string REAL_TP { get; set; }
        public string REAL { get; set; }
        public string REAL_BAS { get; set; }
        public string REAL_INC { get; set; }
        public string GAS_PROCESADO_TP { get; set; }
        public string GAS_QUEMADO_TP { get; set; }
        public string GLP_TP { get; set; }
        public string GLP { get; set; }
        public string PROPANO_TP { get; set; }
        public string BUTANO_TP { get; set; }
        public string GASOLINA_TP { get; set; }
        public string APIASOL_TP { get; set; }
        public string APIASOL { get; set; }
        public string CONDENSADO_TP { get; set; }
        public string CONDENSADO { get; set; }
        public string CONSUMOS_TP { get; set; }
        public string GASODUCTOS_URBANOS_TP { get; set; }
        public string GASODUCTOS_URBANOS { get; set; }
        public string GENERACION_ELECTRICA_TP { get; set; }
        public string OTRAS_VENTAS_TP { get; set; }
        public string GAS_TRANFERENCIA_TP { get; set; }
        public string GAS_TRANFERENCIA { get; set; }
        public string GAS_NEUMATICO_TP { get; set; }
        public string GAS_INYECTADO_TP { get; set; }
        public string TOTAL_PROC_PLANTA_TP { get; set; }
        public string GP_TRATOTAL_PROC_PLANTA_TP { get; set; }
        public string GP_TRATOTAL_PROC_PLANTA { get; set; }
        public string GP_CONSUMOS_TP { get; set; }
        public string GP_GASODUCTOS_URBANOS_TP { get; set; }
        public string GP_GENERACION_ELECTRICA_TP { get; set; }
        public string GP_OTRAS_VENTAS_TP { get; set; }
        public string GP_NEUMATICO_TP { get; set; }
        public string GP_QUEMADO_TP { get; set; }
        public string GP_INYECTADO_TP { get; set; }
        public string REPRESENTA_OPER { get; set; }
        public string REPRESENTA_ANH { get; set; }
        public string IDFORMA { get; set; }
    }

    public class regVal30
    {
        public FormasValidador30 FORMAS { get; set; }
    }

    public class FormasValidador30
    {
        public RegistroValidadorForma30 FORMA { get; set; }
    }

    public class RegistroValidadorForma30
    {
        public RegisValidadorForma30 REGISTRO { get; set; }
    }

    public class RegisValidadorForma30
    {
        public string CAMPO { get; set; }
        public string PDEN_ID { get; set; }
        public string VALIDACION_F9F30 { get; set; }
        public string GAS_F9 { get; set; }
        public string GAS_F30 { get; set; }
        public string MENSAJE { get; set; }
    }

    public class values
    {
        public DateTime date { get; set; }
        public List<Files> file { get; set; }
    }
    public class Files
    {
        public string uid { get; set; }
        public decimal lastModified { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public string name { get; set; }
        public decimal size { get; set; }
        public string type { get; set; }
        public decimal percent { get; set; }
        public originFileObj originFileObj { get; set; }
    }

    public class originFileObj
    {
        public string uid { get; set; }
    }

    public class Errores
    {
        public int? sheet { get; set; }
        public string column { get; set; }
        public int row { get; set; }
        public string message { get; set; }
        public string value { get; set; }
    }

    public class BDForma30
    {
        public Guid form30id { get; set; }
        public string operador_id { get; set; }
        public string operador { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public DateTime date { get; set; }
        public string uid { get; set; }
        public decimal lastModified { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public string name { get; set; }
        public decimal size { get; set; }
        public string type { get; set; }
        public decimal percent { get; set; }
        public string originFileObj_uid { get; set; }
        public string usuario { get; set; }
    }

    public class BDForma30Detail
    {
        public Guid form30detail { get; set; }
        public Guid formid { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public decimal gasFormacionKPC { get; set; }
        public decimal contenidoPropano { get; set; }
        public decimal contenidoButano { get; set; }
        public decimal contenidoGasolinaNatural { get; set; }
        public decimal gasFormacionProcesado { get; set; }
        public decimal consumoEnCampo { get; set; }
        public decimal generacionElectrica { get; set; }
        public decimal otros { get; set; }
        public decimal quemadoAire { get; set; }
        public decimal usadoBombeoNeumatico { get; set; }
        public decimal inyectadoYacimiento { get; set; }
        public decimal productosObtenidosGasTotalGasProcesadoPlanta { get; set; }
        public decimal productosObtenidosGasPropano { get; set; }
        public decimal productosObtenidosGasButano { get; set; }
        public decimal productosObtenidosGasGasolina { get; set; }
        public decimal productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos { get; set; }
        public decimal productosGasProcesadoConsumoEnCampo { get; set; }
        public decimal productosGasProcesadoGasoductosUrbanos { get; set; }
        public decimal productosGasProcesadoGeneracionElectrica { get; set; }
        public decimal productosGasProcesadoOtros { get; set; }
        public decimal productosGasProcesadoQuemadoAire { get; set; }
        public decimal productosGasProcesadoUsadoBombeoNeumatico { get; set; }
        public decimal productosGasProcesadoInyectadoYacimiento { get; set; }
        public string productosGasProcesadoObservaciones { get; set; }
        public string REAL_TP { get; set; }
        public string REAL { get; set; }
        public string REAL_BAS { get; set; }
        public string REAL_INC { get; set; }
        public string GAS_PROCESADO_TP { get; set; }
        public string GAS_QUEMADO_TP { get; set; }
        public string GLP_TP { get; set; }
        public string GLP { get; set; }
        public string PROPANO_TP { get; set; }
        public string BUTANO_TP { get; set; }
        public string GASOLINA_TP { get; set; }
        public string APIASOL_TP { get; set; }
        public string APIASOL { get; set; }
        public string CONDENSADO_TP { get; set; }
        public string CONDENSADO { get; set; }
        public string CONSUMOS_TP { get; set; }
        public string GASODUCTOS_URBANOS_TP { get; set; }
        public string GASODUCTOS_URBANOS { get; set; }
        public string GENERACION_ELECTRICA_TP { get; set; }
        public string OTRAS_VENTAS_TP { get; set; }
        public string GAS_TRANFERENCIA_TP { get; set; }
        public string GAS_TRANFERENCIA { get; set; }
        public string GAS_NEUMATICO_TP { get; set; }
        public string GAS_INYECTADO_TP { get; set; }
        public string TOTAL_PROC_PLANTA_TP { get; set; }
        public string GP_TRATOTAL_PROC_PLANTA_TP { get; set; }
        public string GP_TRATOTAL_PROC_PLANTA { get; set; }
        public string GP_CONSUMOS_TP { get; set; }
        public string GP_GASODUCTOS_URBANOS_TP { get; set; }
        public string GP_GENERACION_ELECTRICA_TP { get; set; }
        public string GP_OTRAS_VENTAS_TP { get; set; }
        public string GP_NEUMATICO_TP { get; set; }
        public string GP_QUEMADO_TP { get; set; }
        public string GP_INYECTADO_TP { get; set; }
        public string REPRESENTA_OPER { get; set; }
        public string REPRESENTA_ANH { get; set; }
        public string IDFORMA { get; set; }
        public string pden_id { get; set; }
        public string volume_date { get; set; }
        public string usuario { get; set; }

    }

    public class BDConcreteForm
    {
        public Guid concreteformid { get; set; }
        public decimal maincampid { get; set; }
        public string company { get; set; }
        public string contract { get; set; }
        public string battery { get; set; }
        public string tank { get; set; }
        public decimal month { get; set; }
        public decimal year { get; set; }
        public string explotationmodality { get; set; }
        public string annotations { get; set; }
        public decimal version { get; set; }
        public Guid currentstate { get; set; }
        public decimal generationflag { get; set; }
        public decimal campid { get; set; }
        public string pdenid { get; set; }
        public Guid formid { get; set; }
        public decimal generationjobid { get; set; }
        public decimal iqistatus { get; set; }
        public string usersigning { get; set; }
        public string minrepsigning { get; set; }
        public string formname { get; set; }
    }

    public class ParametrosCreaForma30
    {
        public int anno { get; set; }
        public int mes { get; set; }

        public List<ParametrosForma30> registros { get; set; }
    }

    public class ParametrosForma30
    {
        public int pden_id { get; set; }
        public string real_tp { get; set; }
        public double real { get; set; }
        public string gas_procesado_tp { get; set; }
        public double gas_procesado { get; set; }
        public string gas_quemado_tp { get; set; }
        public double gas_quemado { get; set; }
        public string glp_tp { get; set; }
        public double glp { get; set; }
        public string propano_tp { get; set; }
        public double propano { get; set; }
        public string butano_tp { get; set; }
        public double butano { get; set; }
        public string gasolina_tp { get; set; }
        public double gasolina { get; set; }
        public string apiasol_tp { get; set; }
        public double apiasol { get; set; }
        public string condensado_tp { get; set; }
        public double condensado { get; set; }
        public string consumos_tp { get; set; }
        public double consumos { get; set; }
        public string gasoductos_urbanos_tp { get; set; }
        public double gasoductos_urbanos { get; set; }
        public string generacion_elect_tp { get; set; }
        public double generacion_elect { get; set; }
        public string otras_ventas_tp { get; set; }
        public double otras_ventas { get; set; }
        public string gas_transfer_tp { get; set; }
        public double gas_transfer { get; set; }
        public string gas_neumatico_tp { get; set; }
        public double gas_neumatico { get; set; }
        public string gas_inyectado_tp { get; set; }
        public double gas_inyectado { get; set; }
        public string total_procesado_planta_tp { get; set; }
        public double total_procesado_planta { get; set; }
        public string gp_tratotal_proc_planta_tp { get; set; }
        public double gp_tratotal_proc_planta { get; set; }
        public string gp_transformado_tp { get; set; }
        public double gp_transformado { get; set; }
        public string gp_consumos_tp { get; set; }
        public double gp_consumos { get; set; }
        public string gp_gasoductos_urbanos_tp { get; set; }
        public double gp_gasoductos_urbanos { get; set; }
        public string gp_generacion_elect_tp { get; set; }
        public double gp_generacion_elect { get; set; }
        public string gp_otras_ventas_tp { get; set; }
        public double gp_otras_ventas { get; set; }
        public string gp_neumatico_tp { get; set; }
        public double gp_neumatico { get; set; }
        public string gp_quemado_tp { get; set; }
        public double gp_quemado { get; set; }
        public string gp_inyectado_tp { get; set; }
        public double gp_inyectado { get; set; }
    }

    public class ParametrosPvsCrea
    {
        public string pden_id { get; set; }
        public string vol_meth { get; set; }
        public string act_type { get; set; }
        public string per_type { get; set; }
        public DateTime vol_date { get; set; }
        public double volumen { get; set; }
    }

    public class ParametrosPvsoCrea
    {
        public string pden_id { get; set; }
        public string vol_meth { get; set; }
        public string act_type { get; set; }
        public string pro_type { get; set; }
        public string per_type { get; set; }
        public DateTime vol_date { get; set; }
        public double volumen { get; set; }
    }

    /// <summary>
    /// Clase para enviar al validador de formularios.
    /// </summary>
    public class formaValidador30
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
        public List<REGISTROVVALFORM30> REGISTRO { get; set; }
    }

    public class REGISTROVVALFORM30
    {
        public string CAMPO { get; set; }
        public string GAS_TOTAL_FORMACION { get; set; }
    }
}




