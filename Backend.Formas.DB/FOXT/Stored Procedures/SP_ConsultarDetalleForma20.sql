CREATE PROCEDURE FOXT.SP_ConsultarDetalleForma20
	-- Add the parameters for the stored procedure here
	@id AS decimal
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		

		SELECT distinct D.pdenId AS  PDEN_ID , 
			'PDEN_PR_STR_FORM' AS  PDEN_TYPE , 
			'OFFICIAL' AS  PDEN_SOURCE , 
			'' AS  VOLUME_METHOD , 
			'' AS  ACTIVITY_TYPE , 
			'' AS  PERIOD_TYPE , 
			CONVERT(DATE, convert(varchar(10),EOMONTH( ap.FechaForma ))) AS  VOLUME_DATE ,  
			'0' AS  AMENDMENT_SEQ_NO , 
			'Y' AS  ACTIVE_IND , 
			GETDATE() AS  EFFECTIVE_DATE , 
			convert(date,'20501231') AS  EXPIRY_DATE , 
			convert(int,D.widays) AS  PERIOD_ON_INJECTION , 
			'DIAS' AS  PERIOD_ON_INJECTION_OUOM , 
			D.pressure AS  INJECTION_PRESSURE , 
			'' AS  PRIMARY_PRODUCT , 
			D.wimonthlywater AS  EC_INJECTION_VOLUME , 
			'' AS  EC_INJECTION_VOLUME_OUOM , 
			D.wiaccumulatedwater AS  EC_INJECTION_CUM_VOLUME , 
			D.oilwellfinalstate AS ESTADO,
			ap.UsuarioNombre AS  ROW_CHANGED_BY , 
			ap.FechaActualizacion AS  ROW_CHANGED_DATE , 
			ap.UsuarioNombre AS  ROW_CREATED_BY , 
			ap.FechaCarga AS  ROW_CREATED_DATE ,
			max(fd.accumulatedays) as ECP_CUM_PERIOD_ON_PRODUCTION
		FROM FOXT.CONCRETEFORM AS C (nolock)
		inner join foxt.APROBACIONCARGA AS AP (nolock) ON
		c.formid = ap.ID_Forma
		INNER JOIN FOXT.FORM20 as F (nolock) ON 
		c.formid = f.form20id
		inner join foxt.FORM20DETAIL AS D (nolock) ON 
		F.form20id = D.formid
		inner join FOXT.FORM20PRODUCTIONDETAIL as fd on 
			f.form20id = fd.formid
			and D.oilwellfinalstate = FD.oilwellfinalstate
		WHERE AP.Id = @id
		group by D.pdenId
			, D.pressure 
			, D.wimonthlywater 
			, D.wiaccumulatedwater 
			, D.oilwellfinalstate 
			, ap.UsuarioNombre 
			, ap.FechaActualizacion 
			, ap.UsuarioNombre 
			, ap.FechaCarga 
			, ap.FechaForma
			, D.widays

END
GO