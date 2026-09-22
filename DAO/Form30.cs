using System;

namespace Backend.Formas.Entities.DAO
{
    public class Form30
    {
        public Guid Form30id { get; set; }
        public decimal? Totalbasicgasproduction { get; set; }
        public decimal? Totalincrementalgasproduction { get; set; }
        public decimal? Totalgasproduction { get; set; }
        public decimal? Totalwpcpropane { get; set; }
        public decimal? Totalwpcbutane { get; set; }
        public decimal? Totalwpcgasoline { get; set; }
        public decimal? Totalformationprocessedgas { get; set; }
        public decimal? Totalugcurbangaspipeline { get; set; }
        public decimal? Totalugcelectricgeneration { get; set; }
        public decimal? Totalugcothersales { get; set; }
        public decimal? Totalugcburned { get; set; }
        public decimal? Totalugcmechanicpumping { get; set; }
        public decimal? Totalugcinyected { get; set; }
        public decimal? Totalugcconsumed { get; set; }
        public decimal? Gptotalprocessedgas { get; set; }
        public decimal? Gppropane { get; set; }
        public decimal? Gpbutane { get; set; }
        public decimal? Gpgasoline { get; set; }
        public decimal? Gptransformedgas { get; set; }
        public decimal? Gppgcconsumed { get; set; }
        public decimal? Gppgcurbangaspipeline { get; set; }
        public decimal? Gppgcelectricgeneration { get; set; }
        public decimal? Gppgcothersales { get; set; }
        public decimal? Gppgcburned { get; set; }
        public decimal? Gppgcinyected { get; set; }
        public decimal? Gpugcmechanicpumping { get; set; }
        public string row_created_by { set; get; }
        public DateTime row_created_date { set; get; }
        public string row_changed_by { set; get; }
        public DateTime row_changed_date { set; get; }
    }
}