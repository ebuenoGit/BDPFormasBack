using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Validaciones
{

    public class REG_MENSAJE
    {
        public string DETALLEMENSAJE { get; set; }
        public string REGLA { get; set; }
        public string COLUMNA { get; set; }
    }
    public class REG_VALIDACIONES
    {
        public string Tabla { get; set; }
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
    }

    public class REG_REGISTRO
    {
        public string LLAVE { get; set; }
        public IList<REG_VALIDACIONES> VALIDACIONES { get; set; }
    }

    public class DetalleJsonF9Masiva
    {

        //GERENCIA ANIO MES UWI PDEN_ID ID_CAMPO CAMPO AREA
        //POZO ZONA MUNICIPIO
        //METODOPROD DIASMES DIASACUM
        //PETROLEODIARIO PETROLEOMENSUAL PETROLEOACUM
        //FACTORCORRECCION
        //AGUADIARIO AGUAMENSUAL AGUAACUM
        //GASDIARIO   GASMENSUAL GASACUM
        //BSW API RGA ESTADOPOZO  OPERADOR NOMBRE_ZONA ELIMINAR

        public string GERENCIA { set; get; }
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string UWI { set; get; }
        public string PDEN_ID { set; get; }
        public string ID_CAMPO { set; get; }
        public string CAMPO { set; get; }
        public string AREA { set; get; }
        public string POZO { set; get; }
        public string ZONA { set; get; }
        public string MUNICIPIO { set; get; }    // zona municipio
        public string METODOPROD { set; get; }
        /// <summary>
        /// dias de produccion simpre en horas de 24 se presentan en dias 
        /// </summary>
        public string DIASMES { set; get; }      // Priodo de carga
        public string DIASACUM { set; get; }

        /// <summary>
        /// Producccion Crudo 
        /// </summary>
        public string PETROLEODIARIO { set; get; }
        public string PETROLEOMENSUAL { set; get; }
        public string PETROLEOACUM { set; get; }
        public string FACTORCORRECCION { set; get; }
        /// <summary>
        /// produccion agua
        /// </summary>
        public string AGUADIARIO { set; get; }
        public string AGUAMENSUAL { set; get; }
        public string AGUAACUM { set; get; }

        /// <summary>
        /// produccion de gas
        /// </summary>
        public string GASDIARIO { set; get; }
        public string GASMENSUAL { set; get; }
        public string GASACUM { set; get; }

        /// <summary>
        /// Medicionaes de la forma
        /// </summary>
        public string BSW { set; get; }
        public string API { set; get; }
        public string RGA { set; get; }
        /// <summary>
        /// Estado del poso Homologacion especial
        /// </summary>
        public string ESTADOPOZO { set; get; }
        public string OPERADOR { set; get; }
        public string NOMBRE_ZONA { set; get; }
        /// <summary>
        /// /// Validaciones de los registros 
        /// /// </summary>
        public string VALIDACION_PRODUCCION { set; get; }
        public bool APROBADO { set; get; }
        public string UserId { set; get; }
        public bool ELIMINAR { set; get; }

        public IList<REG_MENSAJE> REG_MENSAJES { get; set; }

    }

    public class JsonF9M_SQL
    {
        public string Gerencia { set; get; }
        public string Anio { set; get; }
        public string Mes { set; get; }
        public string Uwi { set; get; }
        public string PdenId { set; get; }
        public string CampoId { set; get; }
        public string Campo { set; get; }
        public string Area { set; get; }
        public string Pozo { set; get; }
        public string Zona { set; get; }
        public string Municipio { set; get; }
        public string MetProduccion { set; get; }    // zona municipio

        public string Mes_dia { set; get; }      // Priodo de carga
        public string Acumulado_dia { set; get; }

        /// <summary>
        /// Producccion Crudo 
        /// </summary>
        public string Diario_crudo { set; get; }
        public string Mensual_crudo { set; get; }
        public string Acumulado_crudo { set; get; }
        public string FactorCorreccion { set; get; }
        /// <summary>
        /// produccion agua
        /// </summary>
        public string Diario_agua { set; get; }
        public string Mensual_agua { set; get; }
        public string Acumulado_agua { set; get; }

        /// <summary>
        /// produccion de gas
        /// </summary>
        public string Diario_gas { set; get; }
        public string Mensual_gas { set; get; }
        public string Acumulado_gas { set; get; }

        /// <summary>
        /// Medicionaes de la forma
        /// </summary>
        public string Bsw { set; get; }
        public string Api { set; get; }
        public string Rgp { set; get; }
        /// <summary>
        /// Estado del poso Homologacion especial
        /// </summary>
        public string Estado { set; get; }
        public string Aprobado { set; get; }
        /// <summary>
        /// /// Validaciones de los registros 
        /// /// </summary>
        public string CargadoaBDP { set; get; }
        public string row_created_by { set; get; }
        public string row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public string row_changed_date { set; get; }
        public string Json_MensajesErr { set; get; }
        public string Operador { set; get; }
        public string Nombrezona { set; get; }
        public bool Eliminar { set; get; }

    }

    public class JsonF9M_BDP
    {
        public string GERENCIA { set; get; }
        public string ANIO { set; get; }
        public string MES { set; get; }
        public string UWI { set; get; }
        public string PDEN_ID { set; get; }
        public string ID_CAMPO { set; get; }
        public string CAMPO { set; get; }
        public string AREA { set; get; }
        public string POZO { set; get; }
        public string ZONA { set; get; }
        public string MUNICIPIO { set; get; }    // zona municipio
        
        public string METODOPROD { set; get; }
        public string DIASMES { set; get; }      // Priodo de carga
        public string DIASACUM { set; get; }
        public string PETROLEODIARIO { set; get; }
        public string PETROLEOMENSUAL { set; get; }
        public string PETROLEOACUM { set; get; }
        public string FACTORCORRECCION { set; get; }
        public string AGUADIARIO { set; get; }
        public string AGUAMENSUAL { set; get; }
        public string AGUAACUM { set; get; }
        public string GASDIARIO { set; get; }
        public string GASMENSUAL { set; get; }
        public string GASACUM { set; get; }
        public string BSW { set; get; }
        public string API { set; get; }
        public string RGA { set; get; }
        public string ESTADOPOZO { set; get; }
        public string OPERADOR { set; get; }
        public string NOMBRE_ZONA { set; get; }
        public string USERID { set; get; }
        public string ELIMINAR { set; get; }
    }

    public class Ctr_JsonF9M_BDP
    {
        public string Contador { get; set; }
        public List<JsonF9M_BDP> JContenido { get; set; }
    }
    public class JsonF9Error_BDP
    {
        public string FORMA { set; get; }
        public IList<REG_REGISTRO> REGISTROS { get; set; }
    }
}


