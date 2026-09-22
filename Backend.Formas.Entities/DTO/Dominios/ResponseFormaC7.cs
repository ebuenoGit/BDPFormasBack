using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ResponseFormaC7
    {
        public HeaderFormaC7 Info { get; set; }
        public List<BodyFormaC7> Data { get; set; }
        public TotalesFormaC7 Total { get; set; }
    }
}
