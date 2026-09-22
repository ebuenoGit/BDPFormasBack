using System;

namespace Backend.Formas.Entities.DAO
{
    public class FormC7
    {
        public Guid Formc7id { get; set; }
        public string Formation { get; set; }
        public string Block { get; set; }
        public string Oilfield { get; set; }
        public string Structure { get; set; }
        public string Member { get; set; }
        public string Operadorid { get; set; }
        public string Operador { get; set; }
        public string Contractid { get; set; }
        public string Contract { get; set; }
        public string Campoid { get; set; }
        public string Campo { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
