using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form9MasivaValidaAcumulado
    {   /// <summary>
        /// MCG  --- > Mejoras BDP ++ 
        /// 2022 - Feb - 15
        /// Carga de acumulados de la pden_vol_summary por pozo formacion PDEN_PR_STR_FORM
        /// </summary>
        public Guid Form9tvdetailid { get; set; }
        public string pden_id { get; set; }
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
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}