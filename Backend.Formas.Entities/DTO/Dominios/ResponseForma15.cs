using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ResponseForma15
    {
        public HeaderForma15 Header { set; get; }
        public List<BodyTableForma15> Body { set; get; }
        public ExcelError Errors { set; get; }
    }
}