using System;

namespace Backend.Formas.Entities.DAO
{
    public class Formc4
    {
        public Guid Formc4id { get; set; }
        public string Deliverysite { get; set; }
        public string Contrato { get; set; }
        public string ContratoId { get; set; }
        public string CampoId { get; set; }
        public string Campo { get; set; }
        public decimal? Initialstock { get; set; }
        public decimal? Deliveries { get; set; }
        public decimal? Vesselsdeadvolume { get; set; }
        public decimal? Linesdeadvolume { get; set; }
        public decimal? Finalexistence { get; set; }
        public decimal? Totalbalance { get; set; }
        public decimal? Apigrades { get; set; }
        public decimal? Bsw { get; set; }
        public decimal? Sulfurcontent { get; set; }
        public string Bloque { get; set; }
        public string BloqueId { get; set; }
        public string Formacion { get; set; }
        public string FormacionId { get; set; }
        public string FormacionSetId { get; set; }
        public string EstructuraId { get; set; }
        public string Estructura { get; set; }
        public string Yacimiento { get; set; }
        public string YacimientoId { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}