using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class RequestForma21
    {
        public HeaderForma21 info { set; get; }
        public List<BodyTableForma21> data { set; get; }
    }
}
