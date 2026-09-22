using System;

namespace Backend.Formas.Entities.Models
{
    public class Forma17CRTable
    {
        public Guid form_id { get; set; }
        public Guid id_detalle { get; set; }
        public string concesion { get; set; }
        public string operador_id { get; set; }
        public string operador { get; set; }
        public string contrato_id { get; set; }
        public string contrato { get; set; }
        public string campo_id { get; set; }
        public string campo { get; set; }
        public string estructura { get; set; }
        public string formacion { get; set; }
        public string bloque { get; set; }
        public string yacimiento { get; set; }
        public string mes { get; set; }
        public string anio { get; set; }
        public DateTime fecha_creacion { get; set; }
        public string usuario_crea { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }

    public class Forma17CRTableDetalle
    {
        public Guid id_detalle { get; set; }
        public Guid form_id { get; set; }
        public string pozo { get; set; }
        public decimal diasEnElMes { get; set; }
        public decimal diasAcumulados { get; set; }
        public decimal produccionGasMCPDiaria { get; set; }
        public decimal produccionGasMCPMensual { get; set; }
        public decimal produccionGasMCPAcumulada { get; set; }
        public decimal produccionAguaMensual { get; set; }
        public decimal produccionAguaAcumulada { get; set; }
        public string estadoPozosFinalMes { get; set; }
        public DateTime fecha_creacion { get; set; }
        public string usuario_creacion { get; set; }
        public string pden_id { set; get; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
