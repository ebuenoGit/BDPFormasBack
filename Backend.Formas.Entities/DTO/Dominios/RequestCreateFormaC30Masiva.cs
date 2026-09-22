using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO.Validaciones;
using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class RequestCreateFormaC30Masiva
    {   /// <summary>
        /// MCG  --- > Mejoras BDP ++ 
        /// 2022 - Feb - 15
        /// Modelo de datos para el cargue de la forma9 por pden_id Pozo formacion 
        /// </summary>
        public string FormName { set; get; }
        public DateTime DateForm { set; get; }
        public int IdUser { set; get; }
        public string Recarga { set; get; }
        public DateTime Apertura { set; get; }
        public HeaderFormaC30MasivaDTO HeaderC4Masiva { set; get; }
        public List<DetalleJsonC30Masiva> DetailC4Multiple{ set; get; }
        public List<DetalleJsonC30Masiva> DetaSaveBDP { set; get; }

    }
}


