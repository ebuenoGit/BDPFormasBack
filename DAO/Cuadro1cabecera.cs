using System;

namespace Backend.Formas.Entities.DAO
{
    public partial class Cuadro1cabecera
    {
        public Guid IdFormaCuadro1 { get; set; }
        public Guid FormaId { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
        public int? CompaniaId { get; set; }
        public string Compania { get; set; }
        public int? ContratoId { get; set; }
        public string Contrato { get; set; }
        public int? CampoId { get; set; }
        public string Campo { get; set; }
        public string Lugar { get; set; }
        public string Tanque { get; set; }
        public string Bateria { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCrea { get; set; }
        public string id_tanque { get; set; }
        public string id_bateria { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
