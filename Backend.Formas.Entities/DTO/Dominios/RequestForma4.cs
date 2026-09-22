using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class RequestForma4
    {
        public Forma4Header info { set; get; }
        public List<DetalleForma4> data { set; get; }
        public TotalForma4 total { set; get; }
    }
}
