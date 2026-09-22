using System.Collections.Generic;

namespace Backend.Formas.Entities.Models
{

    public class Forma21CRAprobar
    {
        public List<FormasCabecera> forma21CR { get; set; }
        public List<Forma21CR> DETALLE { get; set; }
    }
    public class Forma21CR
    {
        public string PDEN_ID { get; set; }
        public string PDEN_TYPE { get; set; }
        public string PDEN_SOURCE { get; set; }
        public string VOLUME_METHOD { get; set; }
        public string ACTIVITY_TYPE { get; set; }
        public string PERIOD_TYPE { get; set; }
        public string VOLUME_DATE { get; set; }
        public string AMENDMENT_SEQ_NO { get; set; }
        public string ACTIVE_IND { get; set; }
        public string EFFECTIVE_DATE { get; set; }
        public string EXPIRY_DATE { get; set; }
        public string PERIOD_ON_INJECTION { get; set; }
        public string PERIOD_ON_INJECTION_OUOM { get; set; }
        public string INJECTION_PRESSURE { get; set; }
        public string PRIMARY_PRODUCT { get; set; }
        public string EC_INJECTION_VOLUME { get; set; }
        public string EC_INJECTION_VOLUME_OUOM { get; set; }
        public string EC_INJECTION_CUM_VOLUME { get; set; }
        public string ECP_CUM_PERIOD_ON_PRODUCTION { get; set; }
        public string ROW_CHANGED_BY { get; set; }
        public string ROW_CHANGED_DATE { get; set; }
        public string ROW_CREATED_BY { get; set; }
        public string ROW_CREATED_DATE { get; set; }
    }
}
