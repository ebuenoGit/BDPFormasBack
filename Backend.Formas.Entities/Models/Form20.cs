using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{
    public class Form20Load
    {
        public string forma { get; set; }
        public Form20dataFile payload { get; set; }
    }

    public class Form20dataFile
    {
        public List<Forma20Json> dataFile { get; set; }
        public Form20Values dataForm { get; set; }

        public string fileJSON { get; set; }
    }

    public class Forma20Json
    {
        public string sheet { get; set; }
        public Forma20DatosContenido content { get; set; }
    }

    public class Forma20DatosContenido
    {
        public Form20Info info { get; set; }
        public List<Form20Data> data { get; set; }
    }

    public class Form20Info
    {
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

    public class Form20Data
    {
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

    public class Form20Values
    {
        public DateTime fecha { get; set; }
        public string campo { get; set; }
        public List<Forma20File> file { get; set; }

    }

    public class Forma20File
    {
        public string uid { get; set; }
        public string lastModified { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public decimal percent { get; set; }
        public Forma20OriginFile originFileObj { get; set; }
    }

    public class Forma20OriginFile
    {
        public string uid { get; set; }
    }

}
