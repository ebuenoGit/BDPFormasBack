using System;

namespace Backend.Formas.Entities.DAO
{
    public class Forma20Archivo
    {
        public int id { get; set; }
        public string forma { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string uid { get; set; }
        public string User_Created { get; set; }
        public DateTime Date_Created { get; set; }
    }

    public class Forma20Datafile
    {
        public int id { get; set; }
        public string sheet { get; set; }
        public string compania { get; set; }
        public string operador { get; set; }
        public string campo { get; set; }
        public string estructura { get; set; }
        public string formacion { get; set; }
        public string bloque { get; set; }
        public string yacimiento { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
    }

    public class Forma20Data
    {
        public int id { get; set; }
        public string inyeccionPozo { get; set; }
        public int inyeccionMes { get; set; }
        public int inyeccionAcumulados { get; set; }
        public int inyeccionEspesorEfectivoZonaAbiertaPies { get; set; }
        public int inyeccionPresionMediaInyeccion { get; set; }
        public int inyeccionVolumenAguaInyMesBls { get; set; }
        public int inyeccionVolumenAguaInyAcumuladoBls { get; set; }
        public string inyeccionEstadoPozoFinalMes { get; set; }
        public int produccionPozo { get; set; }
        public int produccionDiasMes { get; set; }
        public int produccionDiasAcumulados { get; set; }
        public int produccionPetroleoMensualBls { get; set; }
        public int produccionPetroleoAcumuladoBbl { get; set; }
        public int produccionAguaMensualBls { get; set; }
        public int produccionAguaAcumuladaBls { get; set; }
        public int presionFondo { get; set; }
        public string estadoFinalMes { get; set; }
    }
}
