using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form21TotalVolumenDetail
    {
        public Guid Idform21tvdetailid { get; set; }
        public Guid? Formid { get; set; }
        public decimal? Volumetype { get; set; }
        public decimal? Accumulategasinjection { get; set; }
        public decimal? Monthlygasinjection { get; set; }
        public decimal? Dailygasinjection { get; set; }
        public string Formation { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
