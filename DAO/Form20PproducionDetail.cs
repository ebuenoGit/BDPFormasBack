using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form20PproducionDetail
    {

        public Guid form20productiondetailid { set; get; }
        public string oilwell { set; get; }
        public decimal danecode { set; get; }
        public string productionmethod { set; get; }
        public decimal monthdays { set; get; }
        public decimal accumulatedays { set; get; }
        public decimal dailyoilproduction { set; get; }
        public decimal monthlyoilproduction { set; get; }
        public decimal accumulateoilproduction { set; get; }
        public decimal correctionfactor { set; get; }
        public decimal dailywater { set; get; }
        public decimal monthlywater { set; get; }
        public decimal accumulatedwater { set; get; }
        public decimal pressure { set; get; }
        public decimal oilwellfinalstate { set; get; }
        public Guid formid { set; get; }
    }
}

