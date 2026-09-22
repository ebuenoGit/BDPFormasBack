namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ProductionDetailForma9MasivaDTO
    {
        /// <summary>
        /// MCG  --- > Mejoras BDP ++ 
        /// 2022 - Feb - 15
        /// </summary>
        /// 
        public string TipoForma { set; get; }
        public string Gerencia { set; get; }
        public string Anio { set; get; }
        public string Mes { set; get; }
        public string Uwi { set; get; }
        public string PdenId { set; get; }
        public string Formacion  { set; get; }
        public string Campoid { set; get; }
        public string Campo { set; get; }
        public string Area { set; get; }
        public string Pozo { set; get; }
        public string Zona { set; get; }
        public string Municipio { set; get; }    // zona municipio
        public string MetProducion { set; get; }
        /// <summary>
        /// dias de produccion simpre en horas de 24 se presentan en dias 
        /// </summary>
        public string Mes_dia { set; get; }      // Priodo de carga
        public string Acumulado_dia { set; get; }

        /// <summary>
        /// Producccion Crudo 
        /// </summary>
        public string Diario_crudo { set; get; }
        public string Mensual_crudo { set; get; }
        public string Acumuado_crudo { set; get; }
        public string FactorCorrecion { set; get; }
        /// <summary>
        /// produccion agua
        /// </summary>
        public string Diario_agua { set; get; }
        public string Mensual_agua { set; get; }
        public string Acumuado_agua { set; get; }

        /// <summary>
        /// produccion de gas
        /// </summary>
        public string Diario_gas { set; get; }
        public string Mensual_gas { set; get; }
        public string Acumuado_gas { set; get; }
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
        public string Operador { set; get; }
        /// <summary>
        /// /// Validaciones de los registros 
        /// /// </summary>
        public string VALIDACION_PRODUCCION { set; get; }
        public string CRUDO { set; get; }
        public string AGUA { set; get; }
        public string GAS { set; get; }

        public bool Aprobado { set; get; }
        public bool Eliminar { set; get; }

    }
}