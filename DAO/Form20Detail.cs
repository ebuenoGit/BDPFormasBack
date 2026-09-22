using System;

namespace Backend.Formas.Entities.DAO
{
    public partial class Form20detail
    {
        public Guid Form20detailid { get; set; }
        public Guid? Formid { get; set; }
        public string Oilwell { get; set; }
        public string PdenId { get; set; }
        public string Danecode { get; set; }
        public string Zone { get; set; }
        public decimal? Widays { get; set; }
        public decimal? Wiaccumulateddays { get; set; }
        public decimal? Pressure { get; set; }
        public decimal? Widailywater { get; set; }
        public decimal? Wimonthlywater { get; set; }
        public decimal? Wiaccumulatedwater { get; set; }
        public string Poolname { get; set; }
        public string Oilwellfinalstate { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
