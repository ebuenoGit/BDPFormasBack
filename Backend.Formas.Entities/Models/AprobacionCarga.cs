using System;

namespace Backend.Formas.Entities.Models
{
    public class AprobacionCarga
    {
        public int id { get; set; }
        public Guid ID_Forma { get; set; }
        public DateTime FechaForma { get; set; }
        public string Usuario { get; set; }
        public string UsuarioNombre { get; set; }
        public DateTime FechaCarga { get; set; }
        public Guid Estado { get; set; }
        public decimal? ComparativoAgua { get; set; }
        public decimal? ComparativoGas { get; set; }
        public decimal? ComparativoCrudo { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string UsuarioAprobador { get; set; }
        public string UsuarioNombreAprobador { get; set; }
        public Guid FormTypeID { get; set; }
        public string FormEntidad { get; set; }
        public string FormaName { get; set; }
        public string urlForma { get; set; }
        public string operador { get; set; }
        public string contrato { get; set; }
        public string campo { get; set; }
    }
}
