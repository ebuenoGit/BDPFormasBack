using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO.Validaciones;
using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class RequestCreateForma9Masiva
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
        public HeaderForma9MasivaDTO Header9Masiva { set; get; }
        public List<ProductionDetailForma9MasivaDTO> Detail9Masiva { set; get; }
        public List<DetalleJsonF9Masiva> Detail9Multiple{ set; get; }
        public Form9MasivaValidaAcumulado Totalacumulados { set; get; }
        public List<DetalleJsonF9Masiva> DetaSaveBDP { set; get; }
    }
}


