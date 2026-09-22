using System.Collections.Generic;

namespace Backend.Formas.Entities.DTO.Dominios
{
    public class ExcelError
    {
        public string message { set; get; }
        public List<string> listError { set; get; }
        public bool state { set; get; }
        public bool cargaExtemporal { set; get; }
        public dynamic header { set; get; }
    }
}