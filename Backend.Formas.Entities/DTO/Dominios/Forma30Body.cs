namespace Backend.Formas.Entities.DTO.Dominios
{
    public class Forma30Body
    {
        public string Campo { set; get; }
        public string GasDeFormacion { set; get; }
        public string ContenidoDePropano { set; get; }
        public string ContenidoDeButano { set; get; }
        public string ContenidoDeGasolinaNatural { set; get; }
        public string GasDeFormacionProcesado { set; get; }
        public string SinProcesarConsumoEnCampo { set; get; }
        public string SinProcesarGasoductosUrbanos { set; get; }
        public string SinProcesarGeneracionElectrica { set; get; }
        public string SinProcesarOtros { set; get; }
        public string SinProcesarQuemaAlAire { set; get; }
        public string SinProcesarBombeoAutomatico { set; get; }
        public string SinProcesarInyacionYacimiento { set; get; }
    }
}