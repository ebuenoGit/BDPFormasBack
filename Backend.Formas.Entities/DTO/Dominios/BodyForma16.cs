namespace Backend.Formas.Entities.DTO.Dominios
{
    public class BodyForma16
    {
        public string Pozo { get; set; }
        public string MetodoProduccion { set; get; }
        public string FechaEnsayo { set; get; }
        public string EstadoPozo { set; get; }
        public string TamanioReduccion { set; get; }
        public string PresionTuberiaProduccion { get; set; }
        public string PresionTuberiaRevestimiento { get; set; }
        public string PozoBombeoLongitudEmbolada { get; set; }
        public string PozoBombeoEmboladasMinuto { get; set; }
        public string DuracionEnsayo { get; set; }
        public string ProduccionEnsayoPetroleoBLS { set; get; }
        public string ProduccionEnsayoGravedadPetroleo { set; get; }
        public string produccionEnsayoSedimientoAguaBLS { set; get; }
        public string ProduccionEnsayoGasMPC { set; get; }
        public string Rga { set; get; }
        public string Pden_id { get; set; }
    }
}