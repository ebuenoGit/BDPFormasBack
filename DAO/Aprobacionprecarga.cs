using System;

namespace Backend.Formas.Entities.DAO
{
    public class Aprobacionprecarga
    {
        public decimal Id { get; set; }
        public Guid? IdForma { get; set; }
        public string Operadora { set; get; }
        public string Campo { set; get; }
        public string Contrato { set; get; }
        public DateTime? FechaForma { get; set; }
        public string Usuario { get; set; }
        public string UsuarioNombre { get; set; }
        public DateTime? FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string UsuarioAprobador { get; set; }
        public string UsuarioNombreAprobador { get; set; }
        public string FormaName { get; set; }
        public string UrlForma { set; get; }
        public decimal Activo { set; get; }
        public string Motivo { set; get; }
    }
}