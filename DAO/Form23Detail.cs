using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form23Detail
    {
        public Guid Form23detailid { get; set; }
        public Guid? Formid { get; set; }
        public decimal? Widays { get; set; }
        public decimal? Wiaccumulateddays { get; set; }
        public decimal? Pressure { get; set; }
        public decimal? Widailywater { get; set; }
        public decimal? Wimonthlywater { get; set; }
        public decimal? Wiaccumulatedwater { get; set; }
        public string Poolname { get; set; }
        public decimal? Injectionoilwellfinalstate { get; set; }
        public decimal? Monthdays { get; set; }
        public decimal? Accumulatedays { get; set; }
        public decimal? Dailyoilproduction { get; set; }
        public decimal? Monthlyoilproduction { get; set; }
        public decimal? Accumulateoilproduction { get; set; }
        public decimal? Dailywaterproduction { get; set; }
        public decimal? Monthlywaterproduction { get; set; }
        public decimal? Accumulatewaterproduction { get; set; }
        public decimal? Dailygasproduction { get; set; }
        public decimal? Monthlygasproduction { get; set; }
        public decimal? Accumulategasproduction { get; set; }
        public string Productionoilwellfinalstate { get; set; }
        public string PdenId { set; get; }
        public string PressureProd { set; get; }
        public string productionmethodproduct { set; get; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
