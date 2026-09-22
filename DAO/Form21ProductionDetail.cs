using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form21ProductionDetail
    {
        public Guid Form21proddetailid { get; set; }
        public Guid Formid { get; set; }
        public string Oilwell { get; set; }
        public string Zone { get; set; }
        public decimal? Danecode { get; set; }
        public string Productionmethod { get; set; }
        public decimal? Monthdays { get; set; }
        public decimal? Accumulatedays { get; set; }
        public decimal? Dailyoilproduction { get; set; }
        public decimal? Monthlyoilproduction { get; set; }
        public decimal? Accumulateoilproduction { get; set; }
        public decimal? Dailygasproduction { get; set; }
        public decimal? Montlhygasproduction { get; set; }
        public decimal? Accumulategasproduction { get; set; }
        public decimal? Dailywaterproduction { get; set; }
        public decimal? Monthlywaterproduction { get; set; }
        public decimal? Accumulatewaterproduction { get; set; }
        public string Oilwellfinalstate { get; set; }
        public decimal? Pressure { get; set; }
        public string Pden_id { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
