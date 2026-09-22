CREATE PROCEDURE FOXT.SP_Guardar_Det_Forma17
	-- Add the parameters for the stored procedure here
	      @id_cabecera UniqueIdentifier
		, @form_id UniqueIdentifier
		, @pozo varchar(100)
		, @diasEnElMes decimal(20,2)
		, @diasAcumulados decimal(20,2)
		, @produccionGasMCPDiaria decimal(20,2)
		, @produccionGasMCPMensual decimal(20,2)
		, @produccionGasMCPAcumulada decimal(20,2)
		, @produccionAguaMensual decimal(20,2)
		, @produccionAguaAcumulada decimal(20,2)
		, @estadoPozosFinalMes VARCHAR(100)
		, @usuario varchar(100) 
		, @pden_id varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO FOXT.Forma17CRDetalle
	(  
			  id_detalle
			, form_id 
			, pozo 
			, diasEnElMes 
			, diasAcumulados 
			, produccionGasMCPDiaria 
			, produccionGasMCPMensual 
			, produccionGasMCPAcumulada 
			, produccionAguaMensual 
			, produccionAguaAcumulada 
			, estadoPozosFinalMes 
			, fecha_creacion 
			, usuario_creacion 
			, pdenId
			, row_created_by
			, row_created_date
			, row_changed_by
			, row_changed_date
	)
	values
	(
		      @id_cabecera
			, @form_id 
			, @pozo 
			, @diasEnElMes 
			, @diasAcumulados 
			, @produccionGasMCPDiaria 
			, @produccionGasMCPMensual 
			, @produccionGasMCPAcumulada 
			, @produccionAguaMensual 
			, @produccionAguaAcumulada 
			, @estadoPozosFinalMes 
			, GETDATE()
			, @usuario
			, @pden_id
			, @usuario
			, GETDATE()
			,@usuario
			,GETDATE()

	)

	select *
	from FOXT.Forma17CRDetalle
	WHERE id_detalle = @id_cabecera
			AND form_id = @form_id



END
GO