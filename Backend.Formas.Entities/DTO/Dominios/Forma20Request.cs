using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class Forma20Request
    {
        public Forma20Header Info { set; get; }
        public List<BodyForma20> data { set; get; }
    }
}
