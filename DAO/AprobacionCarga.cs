using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Formas.Entities.DAO
{
    public class Aprobacioncarga
    {
        public decimal Id { get; set; }
        public Guid? IdForma { get; set; }
        public DateTime FechaForma { get; set; }
        public string Usuario { get; set; }
        public string UsuarioNombre { set; get; }
        public DateTime? FechaCarga { get; set; }
        public Guid? Estado { get; set; }
        public decimal? ComparativoAgua { get; set; }
        public decimal? ComparativoGas { get; set; }
        public decimal? ComparativoCrudo { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string UsuarioAprobador { get; set; }
        public string UsuarioNombreAprobador { set; get; }
        public string UrlForma { set; get; }
        public string FormaName { set; get; }
        public string Operadora { set; get; }
        public string Campo { set; get; }
        public string Contrato { set; get; }

        [ForeignKey("Estado")]
        public virtual Formstate Formstate { set; get; }

        //public string FromtType { get; set; }
    }
}