using System;

namespace Backend.Formas.Entities.DAO
{
    public class Formc4totalvolumedetail
    {
        public Guid Formc4totalvolumedetailid { get; set; }
        public Guid? Formid { get; set; }
        public string Formation { get; set; }
        public string Activity { get; set; }
        public decimal? Totalvolume { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}