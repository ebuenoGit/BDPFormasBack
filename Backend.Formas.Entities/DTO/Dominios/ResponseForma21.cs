using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ResponseForma21
    {
        public HeaderForma21 Info { set; get; }
        public List<BodyTableForma21> Data { set; get; }
    }
}
