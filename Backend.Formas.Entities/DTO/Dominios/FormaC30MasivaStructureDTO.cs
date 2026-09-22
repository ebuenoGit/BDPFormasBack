using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class FormaC30MasivaStructureDTO
    {
        public List<BodyContenteDTOC30Masiva> Data { set; get; }
        public ExcelError Errors { get; set; }
    }
}