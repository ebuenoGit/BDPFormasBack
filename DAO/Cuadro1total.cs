using System;

namespace Backend.Formas.Entities.DAO
{
    public partial class Cuadro1total
    {
        public Guid IdCabecera { get; set; }
        public Guid FormaId { get; set; }
        public decimal? Bls60f { get; set; }
        public decimal? Blsnetos { get; set; }
        public decimal? RecibidoBls { get; set; }
        public decimal? EntregaBls { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string UsuarioCrea { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
