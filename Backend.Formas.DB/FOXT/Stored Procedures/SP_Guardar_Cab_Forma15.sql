CREATE PROCEDURE FOXT.SP_Guardar_Cab_Forma15
	-- Add the parameters for the stored procedure here
	  @form_id AS UniqueIdentifier
	, @compania_id AS VARCHAR(50)
	, @compania AS VARCHAR(50)
	, @concesion AS VARCHAR(50)
	, @contrato_id AS VARCHAR(50)
	, @contrato AS VARCHAR(50)
	, @campo_id AS VARCHAR(50)
	, @campo AS VARCHAR(50)
	, @mes AS VARCHAR(50)
	, @anio AS VARCHAR(50)
	, @usuario AS VARCHAR(50)
AS
BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [FOXT].[Forma15CR]
           ([form_id]
           ,[id_detalle]
           ,[compania_id]
           ,[compania]
           ,[concesion]
           ,[contrato_id]
           ,[contrato]
           ,[campo_id]
           ,[campo]
           ,[mes]
           ,[anio]
           ,[row_created_by]
		   ,[row_created_date]
		   ,[row_changed_by]
		   ,[row_changed_date])
     VALUES
           ( @form_id
           , NEWID()
           , @compania_id
                , @compania
                , @concesion
                , @contrato_id
                , @contrato
                , @campo_id
                , @campo
                , @mes
                , @anio
				, @usuario
				, GETDATE()
                , @usuario
				, GETDATE())

	SELECT TOP 1 *
	FROM FOXT.Forma15CR
	WHERE form_id = @form_id
	ORDER BY row_created_date DESC

                
END
GO