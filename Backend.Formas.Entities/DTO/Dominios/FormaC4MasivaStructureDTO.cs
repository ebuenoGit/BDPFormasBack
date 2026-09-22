using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class FormaC4MasivaStructureDTO
    {
        public List<BodyContenteDTOC4Masiva> Data { set; get; }
        public ExcelError Errors { get; set; }
    }
}