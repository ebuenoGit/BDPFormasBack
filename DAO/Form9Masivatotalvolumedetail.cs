using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form9Masivatotalvolumedetail
    {
        public Guid Form9tvdetailid { get; set; }
        public decimal? Volumetype { get; set; }
        public decimal? Accumulategasproduction { get; set; }
        public decimal? Montlhygasproduction { get; set; }
        public decimal? Dailygasproduction { get; set; }
        public decimal? Accumulatewaterproduction { get; set; }
        public decimal? Monthlywaterproduction { get; set; }
        public decimal? Dailywaterproduction { get; set; }
        public decimal? Accumulateoilproduction { get; set; }
        public decimal? Dailyoilproduction { get; set; }
        public decimal? Monthlyoilproduction { get; set; }
        public decimal? Bsw { get; set; }
        public decimal? Apigrades { get; set; }
        public decimal? Rgp { get; set; }
        public Guid? Formid { get; set; }
        public string Formation { get; set; }
        public string Row_created_by { set; get; }
        public DateTime Row_created_date { set; get; }
        public string Row_changed_by { set; get; }
        public DateTime Row_changed_date { set; get; }
        public string Eliminar { set; get; }
    }
}