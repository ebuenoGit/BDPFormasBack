using Backend.Formas.Entities.DAO;
using System.Collections.Generic;
using Backend.Formas.Entities.DTO.Validaciones;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class BodyContenteDTOMasiva
    {
        public string Workbook { set; get; }
        public List<ProductionDetailForma9MasivaDTO> ProductionDetail { set; get; }
        public Form9Masivatotalvolumedetail Total { set; get; }
        public HeaderForma9MasivaDTO Header { set; get; }
        public List<DetalleJsonF9Masiva> RespBDP { set; get; }
    }
}