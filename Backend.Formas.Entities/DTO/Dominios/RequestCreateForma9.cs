using Backend.Formas.Entities.DAO;
using System;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class RequestCreateForma9
    {
        public string formName { set; get; }
        public DateTime dateForm { set; get; }
        public int idUser { set; get; }
        public string recarga { set; get; }
        public DateTime apertura { set; get; }
        public HeaderForma9DTO header { set; get; }
        public List<ProductionDetailForma9DTO> detail { set; get; }
        public Form9totalvolumedetail total { set; get; }
    }
}