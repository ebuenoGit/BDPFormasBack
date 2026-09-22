CREATE PROCEDURE FOXT.SP_Guardar_Cab_Forma17
	-- Add the parameters for the stored procedure here
	  @form_id AS UniqueIdentifier
	, @concesion AS VARCHAR(100)
	, @operador_id AS VARCHAR(100)
	, @operador AS VARCHAR(100)
	, @contrato_id AS VARCHAR(100)
	, @contrato AS VARCHAR(100)
	, @campo_id AS VARCHAR(100)
	, @campo AS VARCHAR(100)
	, @estructura AS VARCHAR(100)
	, @formacion AS VARCHAR(100)
	, @bloque AS VARCHAR(100)
	, @yacimiento AS VARCHAR(100)
	, @mes AS VARCHAR(100)
	, @anio AS VARCHAR(100)
	, @usuario AS VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO FOXT.Forma17CR
		(
			  form_id
			, id_detalle 
			, concesion 
			, operador_id
			, operador 
			, contrato_id 
			, contrato 
			, campo_id 
			, campo 
			, estructura 
			, formacion 
			, bloque 
			, yacimiento 
			, mes 
			, anio
			, fecha_creacion 
			, usuario_crea
			, row_created_by
			, row_created_date
			, row_changed_by
			, row_changed_date
		)
		VALUES
		(
			  @form_id
			, NEWID()
			, @concesion
			, @operador_id
			, @operador
			, @contrato_id
			, @contrato
			, @campo_id
			, @campo
			, @estructura
			, @formacion
			, @bloque
			, @yacimiento
			, @mes
			, @anio
			, GETDATE()
			, @usuario
			, @usuario
			, GETDATE()
			, @usuario
			, GETDATE()
		)

		SELECT TOP 1 *
		FROM FOXT.Forma17CR
		WHERE form_id = @form_id
		ORDER BY fecha_creacion DESC


END
GO