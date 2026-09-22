using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO
{
    public class Usuario
    {
        public string id { set; get; }
        public string nombre { set; get; }
        public string email { set; get; }
        public string idUsuarioSeguridad { set; get; }
        public List<dynamic> permisosCargueIdp { set; get; }
    }

    public class UsuarioCorreo
    {
        public int id { get; set; }
        public string nombreUsuario { get; set; }
        public string correo { get; set; }
        public int idMenu { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string codigoUsuario { get; set; }

    }
}
