CREATE PROCEDURE [FOXT].[SP_ConsultarDetalleForma21]
    @id AS decimal
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT c.pdenid AS  PDEN_ID,
                'PDEN_PR_STR_FORM' AS  PDEN_TYPE,
                'OFFICIAL' AS  PDEN_SOURCE,
                '1005' AS  VOLUME_METHOD,
                '021' AS  ACTIVITY_TYPE,
                '004' AS  PERIOD_TYPE,
                GETDATE() AS  VOLUME_DATE,
                '0' AS  AMENDMENT_SEQ_NO,
                'Y' AS  ACTIVE_IND,
                GETDATE() AS  EFFECTIVE_DATE,
                GETDATE() AS  EXPIRY_DATE,
                '001' AS  PERIOD_ON_INJECTION,
                '' AS  PERIOD_ON_INJECTION_OUOM,
                FD.pressure AS  INJECTION_PRESSURE,
                '' AS  PRIMARY_PRODUCT,
                FT.dailygasinjection AS  EC_INJECTION_VOLUME,
                FT.monthlygasinjection AS  EC_INJECTION_VOLUME_OUOM,
                FT.accumulategasinjection AS  EC_INJECTION_CUM_VOLUME,
                '' AS  ECP_CUM_PERIOD_ON_PRODUCTION,
                AP.UsuarioNombre AS  ROW_CHANGED_BY ,
                AP.FechaActualizacion AS  ROW_CHANGED_DATE,
                AP.UsuarioNombre AS  ROW_CREATED_BY,
                AP.FechaCarga AS  ROW_CREATED_DATE
		FROM FOXT.CONCRETEFORM AS C (nolock)
		inner join foxt.APROBACIONCARGA AS AP (nolock) ON
		c.formid = ap.ID_Forma
		INNER JOIN FOXT.FORM21 AS F (NOLOCK) ON 
			C.formid = F.form21id
		INNER JOIN FOXT.FORM21INYECTIONDETAIL AS FY (NOLOCK) ON
			C.formid = FY.formid
		INNER JOIN FOXT.FORM21PRODUCTIONDETAIL AS FD (NOLOCK) ON 
			C.formid = FD.formid
		INNER JOIN FOXT.FORM21TOTALVOLUMEDETAIL AS FT (NOLOCK) ON 
			C.formid = FT.formid
		WHERE AP.Id = @id
END
GO
