namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public class DetalleC7Aprobacion
    {
        public string PDEN_ID { set; get; }
        public string PDEN_TYPE { set; get; }
        public string PDEN_SOURCE { set; get; }
        public string VOLUME_METHOD { set; get; }
        public string ACTIVITY_TYPE { set; get; }
        public string PERIOD_TYPE { set; get; }
        public string VOLUME_DATE { set; get; }
        public string AMENDMENT_SEQ_NO { set; get; }
        public string ACTIVE_IND { set; get; }
        public string EFFECTIVE_DATE { set; get; }
        public string EXPIRY_DATE { set; get; }
        public string OIL_VOLUME { get; set; }
        public string OIL_VOLUME_OUOM { get; set; }
        public string GAS_VOLUME { get; set; }
        public string GAS_VOLUME_OUOM { get; set; }
        public string NO_OF_GAS_WELLS { get; set; }
        //public string NO_OF_INJECTION_WELLS { get; set; }
        public string NO_OF_OIL_WELLS { get; set; }
        public string ECP_NO_OF_UNF_WELLS { get; set; }
        public string ECP_NO_OF_ACT_WELLS { get; set; }
        public string ECP_NO_OF_INA_WELLS { get; set; }
        public string ECP_NO_OF_FIN_WELLS { get; set; }
        public string ECP_NO_OF_ABA_WELLS { get; set; }
        public string ROW_CHANGED_BY { set; get; }
        public string ROW_CHANGED_DATE { set; get; }
        public string ROW_CREATED_BY { set; get; }
        public string ROW_CREATED_DATE { set; get; }
        public string ECP_NO_OF_ACT_ART_WELL { set; get; }
        public string ECP_NO_OF_ACT_NAT_WELL { set; get; }
        public string ECP_NO_OF_CLO_WAT_WELL { set; get; }
        public string ECP_NO_OF_CLO_TEM_WELL { set; get; }
        public string ECP_NO_OF_CLO_SEC_WELL { set; get; }
        public string ECP_NO_OF_CLO_SUS_TEM_WELL { set; get; }
        public string ECP_NO_OF_UNFINISHED_WELL { set; get; }

        public string NO_OF_INJECTION_WELLS { get; set; }
        public string NO_OF_INJECTION_GAS_WELLS { get; set; }
        public string NO_OF_INJECTION_AIR_WELLS { get; set; }

    }
}
