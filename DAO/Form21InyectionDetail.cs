using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form21InyectionDetail
    {
        public Guid Form21detailid { get; set; }
        public Guid Formid { get; set; }
        public string Oilwell { get; set; }
        public string Zone { get; set; }
        public decimal? Gidays { get; set; }
        public decimal? Giaccumulateddays { get; set; }
        public decimal? Pressure { get; set; }
        public decimal? Gidailygas { get; set; }
        public decimal? Gimonthlygas { get; set; }
        public decimal? Giaccumulatedgas { get; set; }
        public string Oilwellfinalstate { get; set; }
        public string Poolname { get; set; }
        public string Danecode { get; set; }
        public string Pden_id { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
