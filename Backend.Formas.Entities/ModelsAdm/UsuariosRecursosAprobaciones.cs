namespace Backend.Formas.Entities.ModelsAdm
{
    using System;

    public partial class UsuariosRecursosAprobaciones
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Descripcion { get; set; }
        public int? IdMenu { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string CodigoUsuario { get; set; }
    }
}