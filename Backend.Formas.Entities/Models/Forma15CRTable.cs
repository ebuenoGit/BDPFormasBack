using System;

namespace Backend.Formas.Entities.Models
{
    public class Forma15CRTable
    {
        public Guid form_id { get; set; }
        public Guid id_detalle { get; set; }
        public string compania_id { get; set; }
        public string compania { get; set; }
        public string concesion { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
        public string row_created_by { get; set; }
        public DateTime row_created_date { get; set; }
        public string row_changed_by { get; set; }
        public DateTime row_changed_date { get; set; }
    }

    public class Forma15CRTableDetalle
    {
        public Guid id_detalle { get; set; }
        public Guid form_id { get; set; }
        public string valInyecPozo { get; set; }
        public string valInyecFormacionProductora { get; set; }
        public string valInyecMetodoProduccion { get; set; }
        public decimal valInyecPresionInyeccion { get; set; }
        public decimal valInyecCiclo { get; set; }
        public decimal valInyecDiasMes { get; set; }
        public decimal valInyecDiasAcumulados { get; set; }
        public decimal valInyecLibrasMes { get; set; }
        public decimal valInyecLibrasAcumulados { get; set; }
        public decimal valInyecBTUMes { get; set; }
        public decimal valInyecBTUAcumulados { get; set; }
        public string valInyecCalidadVapor { get; set; }
        public decimal produccionPetroleoBlsNetosMensual { get; set; }
        public decimal produccionPetroleoBlsNetosAcumulado { get; set; }
        public decimal produccionAguaBlsMensual { get; set; }
        public decimal produccionAguaBlsAcumulado { get; set; }
        public string row_created_by { get; set; }
        public DateTime row_created_date { get; set; }
        public string row_changed_by { get; set; }
        public DateTime row_changed_date { get; set; }
        public string pden_id { get; set; }
    }
}
