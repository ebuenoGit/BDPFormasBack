namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ProductionDetailForma9DTO
    {
        public string pozo { set; get; }
        public string municipio { set; get; }
        public string metProducion { set; get; }
        public string mes_dia { set; get; }
        public string acumulado_dia { set; get; }
        public string diario_crudo { set; get; }
        public string mensual_crudo { set; get; }
        public string acumuado_crudo { set; get; }
        public string factorCorrecion { set; get; }
        public string diario_agua { set; get; }
        public string mensual_agua { set; get; }
        public string acumuado_agua { set; get; }
        public string diario_gas { set; get; }
        public string mensual_gas { set; get; }
        public string acumuado_gas { set; get; }
        public string bsw { set; get; }
        public string api { set; get; }
        public string rgp { set; get; }
        public string estado { set; get; }
        public string pdenId { set; get; }

        public string VALIDACION_PRODUCCION { set; get; }
        public string CRUDO { set; get; }
        public string AGUA { set; get; }
        public string GAS { set; get; }

    }
}