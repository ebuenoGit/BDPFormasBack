using System;
using System.Collections.Generic;

//using Backend.Recursos.Entities.Models;

namespace Backend.Formas.Utilities
{
    public static class SessionHelper
    {
        public static List<T> AddUserAndDate<T>(ref T entidadAGuardar)
        {
            var salida = new List<T>();
            entidadAGuardar.GetType().GetProperty("RowCreatedBy").SetValue(entidadAGuardar, "usuarioActual");
            entidadAGuardar.GetType().GetProperty("RowChangedDate").SetValue(entidadAGuardar, DateTime.Now);
            entidadAGuardar.GetType().GetProperty("RowChangedBy").SetValue(entidadAGuardar, "usuarioActual");
            entidadAGuardar.GetType().GetProperty("RowCreatedDate").SetValue(entidadAGuardar, DateTime.Now);
            salida.Add(entidadAGuardar);
            return salida;
        }
    }
}