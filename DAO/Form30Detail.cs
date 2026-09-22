using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form30detail
    {
        public Guid Form30detail1 { get; set; }
        public Guid? Formid { get; set; }
        public string Formation { get; set; }
        public decimal? Oilfieldid { get; set; }
        public decimal? Danecode { get; set; }
        public decimal? Basicgasproduction { get; set; }
        public decimal? Incrementalgasproduction { get; set; }
        public decimal? Totalgasproduction { get; set; }
        public decimal? Wpcpropane { get; set; }
        public decimal? Wpcbutane { get; set; }
        public decimal? Wpcgasoline { get; set; }
        public decimal? Formationprocessedgas { get; set; }
        public decimal? Ugcoconsumed { get; set; }
        public decimal? Ugcurbangaspipeline { get; set; }
        public decimal? Ugcelectricgeneration { get; set; }
        public decimal? Ugcothersales { get; set; }
        public decimal? Ugcburned { get; set; }
        public decimal? Ugcmechanicpumping { get; set; }
        public decimal? Ugcinyected { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}