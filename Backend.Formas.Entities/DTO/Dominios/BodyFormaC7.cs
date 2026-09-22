namespace Backend.Formas.Entities.DTO.Dominios
{
    public class BodyFormaC7
    {
        public string EstructuraCampo { get; set; }
        public string ProduccionBbls { get; set; }
        public string PozosProductoresActivosLevantamientoArtificial { get; set; }
        public string PozosProductoresActivosFlujoNatural { get; set; }
        public string PozosProductoresActivosTotal { get; set; }
        public string PozosProductoresInactivosCerradoAltaRelacionAguaPetroleo { get; set; }
        public string PozosProductoresInactivosCerradoTemporalmente { get; set; }
        public string PozosProductoresInactivosMiscelaneos { get; set; }
     //   public string PozosTaponadosInyectores { get; set; }
        public string PozosTaponadosSecos { get; set; }
        public string PozosTaponadosAbandonados { get; set; }
        public string TotalPozosTerminadosOficialmente { get; set; }
        public string PozosSuspendidosTemporalmente { get; set; }
        public string PozosSuspendidosSecosSinTerminar { get; set; }
        public string Pden_id { get; set; }
        public string PozosSinTerminar { set; get; }
        public string pozosTaponadosInyectores { set; get; }
        public string pozosTaponadosInyectoresGas { set; get; }
        public string pozosTaponadosInyectoresAire { set; get; }
    }
}
