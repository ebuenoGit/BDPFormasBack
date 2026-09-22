using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class Forma9MasivaStructureDTO
    {
        public List<BodyContenteDTOMasiva> Data { set; get; }
        public ExcelError Errors { get; set; }
    }
}