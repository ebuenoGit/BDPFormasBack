namespace Backend.Formas.Entities.Model
{
    public class Form9Detail
    {
        public int form9detailid { set; get; }
        public int oilwell { set; get; }
        public string formation { set; get; }
        public string danecode { set; get; }
        public string productionmethod { set; get; }
        public float monthdays { set; get; }
        public float accumulatedays { set; get; }
        public float dailyoilproduction { set; get; }
        public float monthlyoilproduction { set; get; }
        public float accumulateoilproduction { set; get; }
        public float correctionfactor { set; get; }
        public float dailywaterproduction { set; get; }
        public float monthlywaterproduction { set; get; }
        public float accumulatewaterproduction { set; get; }
        public float dailygasproduction { set; get; }
        public float montlhygasproduction { set; get; }
        public float accumulategasproduction { set; get; }
        public float bsw { set; get; }
        public float apigrades { set; get; }
        public float rgp { set; get; }
        public string oilwellfinalstate { set; get; }
        public int formid { set; get; }
        public string poolname { set; get; }
    }
}