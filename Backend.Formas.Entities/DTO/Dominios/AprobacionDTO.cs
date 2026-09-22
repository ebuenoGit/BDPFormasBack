using System;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class AprobacionDTO
    {
        public decimal Id { get; set; }
        public Guid? IdForma { get; set; }
        public DateTime? FechaForma { get; set; }
        public string Usuario { get; set; }
        public string UsuarioCodigo { get; set; }
        public DateTime? FechaCarga { get; set; }
        public Guid? Estado { get; set; }
        public string EstadoNombre { set; get; }
        public decimal? ComparativoAgua { get; set; }
        public decimal? ComparativoGas { get; set; }
        public decimal? ComparativoCrudo { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string UsuarioAprobador { get; set; }
        public string FormaName { set; get; }
        public string Url { set; get; }
        public System.Collections.Generic.List<Backend.Formas.Entities.DAO.Acumulados> Acumulados { set; get; }
    }
}
