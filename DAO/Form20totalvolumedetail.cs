using System;

namespace Backend.Formas.Entities.DAO
{
    public partial class Form20totalvolumedetail
    {
        public Guid Form20tvdetailid { get; set; }
        public decimal? Volumetype { get; set; }
        public decimal? Accumulatewaterinjection { get; set; }
        public decimal? Monthlywaterinjection { get; set; }
        public decimal? Dailywaterinjection { get; set; }
        public Guid? Formid { get; set; }
        public string Formation { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
