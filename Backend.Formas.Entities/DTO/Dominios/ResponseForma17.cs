
using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ResponseForma17
    {
        public HeaderForma17 Header { set; get; }
        public List<BodyForma17> Body { set; get; }
        public TotalesForma17 Totales { set; get; }
        public ExcelError Errors { set; get; }
    }
}
