using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form9Masivadetail
    {
        public Guid Form9detailid { get; set; }
        public string Oilwell { get; set; }
        public string Formation { get; set; }
        public string Danecode { get; set; }
        public string Productionmethod { get; set; }
        public decimal? Monthdays { get; set; }
        public decimal? Accumulatedays { get; set; }
        public decimal? Dailyoilproduction { get; set; }
        public decimal? Monthlyoilproduction { get; set; }
        public decimal? Accumulateoilproduction { get; set; }
        public decimal? Correctionfactor { get; set; }
        public decimal? Dailywaterproduction { get; set; }
        public decimal? Monthlywaterproduction { get; set; }
        public decimal? Accumulatewaterproduction { get; set; }
        public decimal? Dailygasproduction { get; set; }
        public decimal? Montlhygasproduction { get; set; }
        public decimal? Accumulategasproduction { get; set; }
        public decimal? Bsw { get; set; }
        public decimal? Apigrades { get; set; }
        public decimal? Rgp { get; set; }
        public string Oilwellfinalstate { get; set; }
        public Guid? Formid { get; set; }
        public string Poolname { get; set; }
        public string PdenId { set; get; }
        public string Row_created_by { set; get; }
        public DateTime Row_created_date { set; get; }
        public string Row_changed_by { set; get; }
        public DateTime Row_changed_date { set; get; }
        public bool Eliminar { get; set; }
        public virtual Form9Masiva Form { get; set; }
        
    }
}