using Backend.Formas.Entities.DAO;
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class BodyContenteDTO
    {
        public string workbook { set; get; }
        public List<ProductionDetailForma9DTO> productionDetail { set; get; }
        public Form9totalvolumedetail total { set; get; }
        public HeaderForma9DTO Header { set; get; }
    }
}