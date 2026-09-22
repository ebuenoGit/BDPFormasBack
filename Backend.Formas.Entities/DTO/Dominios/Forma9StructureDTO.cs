using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class Forma9StructureDTO
    {
        public List<BodyContenteDTO> data { set; get; }
        public ExcelError errors { get; set; }
    }
}