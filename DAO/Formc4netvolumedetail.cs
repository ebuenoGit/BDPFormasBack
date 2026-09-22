using System;

namespace Backend.Formas.Entities.DAO
{
    public class Formc4netvolumedetail
    {
        public Guid Formc4netvolumedetailid { get; set; }
        public Guid? Formid { get; set; }
        public string Formation { get; set; }
        public string Municipality { get; set; }
        public string Danecode { get; set; }
        public string Activity { get; set; }
        public string Basica { set; get; }
        public string Incremental { set; get; }
        public decimal? Volume { get; set; }
        public string Productiontype { get; set; }
        public string PdenId { set; get; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}