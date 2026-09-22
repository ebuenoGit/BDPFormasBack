using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Formas.Entities.DAO
{
    public class ConsultaFormaOperadorModel
    {

        public string volume_date { set; get; }
        public string field_name { set; get; }
        public string field { set; get; }
        public string field_pden_id { set; get; }
        public string basicgasproduction { set; get; }
        public string incrementalgasproduction { set; get; }
        public string totalgasproduction { set; get; }
        public string formationprocessedgas { set; get; }
        public string ugcoconsumed { set; get; }
        public string ugcurbangaspipeline { set; get; }
        public string ugcelectricgeneration { set; get; }
        public string ugcothersales { set; get; }
        public string ugcburned { set; get; }
        public string ugcmechanicpumping { set; get; }
        public string ugcinyected { set; get; }
        public string wpcpropane { set; get; }
        public string wpcbutane { set; get; }
        public string wpcgasoline { set; get; }
    }
}
