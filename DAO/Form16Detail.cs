using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form16Detail
    {
        public Guid Form16detailid { get; set; }
        public Guid Formid { get; set; }
        public string Oilwell { get; set; }
        public string Productionmethod { get; set; }
        public string Zone { get; set; }
        public DateTime? Testingdate { get; set; }
        public string Oilwellstate { get; set; }
        public decimal? Thppressure { get; set; }
        public decimal? Chppressure { get; set; }
        public decimal? Hours { get; set; }
        public decimal? Dailyoilproduction { get; set; }
        public decimal? ReductionSize { get; set; }
        public decimal? WellPumpingLength {get; set;}
        public decimal? WellPumpDumpsMinute { get; set; }
        public decimal? ProductionTestingOilBLS { get; set; }
        public decimal? ProductionTestGravityOil { get; set; }
        public decimal? Api { get; set; }
        public decimal? Water { get; set; }
        public decimal? Gas { get; set; }
        public decimal? Rga { get; set; }
        public string Pden_id { get; set; }

        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}
