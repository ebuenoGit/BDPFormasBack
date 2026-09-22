using System;

namespace Backend.Formas.Entities.DAO
{
    public partial class Form20productiondetail
    {
        public Guid Form20productiondetailid { get; set; }
        public string Oilwell { get; set; }
        public string Danecode { get; set; }
        public string Productionmethod { get; set; }
        public decimal? Monthdays { get; set; }
        public decimal? Accumulatedays { get; set; }
        public decimal? Dailyoilproduction { get; set; }
        public decimal? Monthlyoilproduction { get; set; }
        public decimal? Accumulateoilproduction { get; set; }
        public decimal? Correctionfactor { get; set; }
        public decimal? Dailywater { get; set; }
        public decimal? Monthlywater { get; set; }
        public decimal? Accumulatedwater { get; set; }
        public decimal? Pressure { get; set; }
        public string Oilwellfinalstate { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
        public Guid Formid { get; set; }
    }
}
