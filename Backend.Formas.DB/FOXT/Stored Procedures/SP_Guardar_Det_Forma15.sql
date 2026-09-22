CREATE PROCEDURE FOXT.SP_Guardar_Det_Forma15
	-- Add the parameters for the stored procedure here
				  @id_cabecera AS UniqueIdentifier
				, @form_id AS UniqueIdentifier
                , @valInyecPozo AS VARCHAR(50)
                , @valInyecFormacionProductora AS VARCHAR(260)
                , @valInyecMetodoProduccion AS VARCHAR(50)
                , @valInyecPresionInyeccion  AS DECIMAL(20,2)
                , @valInyecCiclo AS DECIMAL(20,2)
                , @valInyecDiasMes AS DECIMAL(20,2)
                , @valInyecDiasAcumulados AS DECIMAL(20,2)
                , @valInyecLibrasMes AS DECIMAL(20,2)
                , @valInyecLibrasAcumulados AS DECIMAL(20,2)
                , @valInyecBTUMes AS DECIMAL(20,2)
                , @valInyecBTUAcumulados AS DECIMAL(20,2)
                , @valInyecCalidadVapor AS VARCHAR(50)
                , @produccionPetroleoBlsNetosMensual AS DECIMAL(20,2)
                , @produccionPetroleoBlsNetosAcumulado AS DECIMAL(20,2)
                , @produccionAguaBlsMensual AS DECIMAL(20,2)
                , @produccionAguaBlsAcumulado AS DECIMAL(20,2)
                , @usuario AS VARCHAR(50)
                , @pden_id AS VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [FOXT].[Forma15CRDetalle]
           ([id_detalle]
           ,[form_id]
           ,[valInyecPozo]
           ,[valInyecFormacionProductora]
           ,[valInyecMetodoProduccion]
           ,[valInyecPresionInyeccion]
           ,[valInyecCiclo]
           ,[valInyecDiasMes]
           ,[valInyecDiasAcumulados]
           ,[valInyecLibrasMes]
           ,[valInyecLibrasAcumulados]
           ,[valInyecBTUMes]
           ,[valInyecBTUAcumulados]
           ,[valInyecCalidadVapor]
           ,[produccionPetroleoBlsNetosMensual]
           ,[produccionPetroleoBlsNetosAcumulado]
           ,[produccionAguaBlsMensual]
           ,[produccionAguaBlsAcumulado]
           ,[pden_id]
		   ,[row_created_by]
		   ,[row_created_date]
		   ,[row_changed_by]
		   ,[row_changed_date])
     VALUES
           (      @id_cabecera
				, @form_id
                , @valInyecPozo
                , @valInyecFormacionProductora
                , @valInyecMetodoProduccion
                , @valInyecPresionInyeccion
                , @valInyecCiclo
                , @valInyecDiasMes
                , @valInyecDiasAcumulados
                , @valInyecLibrasMes
                , @valInyecLibrasAcumulados
                , @valInyecBTUMes
                , @valInyecBTUAcumulados
                , @valInyecCalidadVapor
                , @produccionPetroleoBlsNetosMensual
                , @produccionPetroleoBlsNetosAcumulado
                , @produccionAguaBlsMensual
                , @produccionAguaBlsAcumulado
				, @pden_id
				, @usuario
				, GETDATE()
                , @usuario
				, GETDATE()
                )


	SELECT *
	FROM [FOXT].[Forma15CRDetalle]
	WHERE form_id = @form_id

		          
END
GO