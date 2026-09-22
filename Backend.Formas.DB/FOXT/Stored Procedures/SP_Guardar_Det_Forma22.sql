CREATE PROCEDURE FOXT.SP_Guardar_Det_Forma22
	-- Add the parameters for the stored procedure here
	  @id_cabecera UniqueIdentifier
	, @form_id UniqueIdentifier 
    , @Pozo VARCHAR(50)
	, @mes VARCHAR(10)
	, @petroleoProducidoMensual DECIMAL(20,2)
	, @petroleoProducidoAcumulado DECIMAL(20,2)
	, @aguaInyectadoMensual DECIMAL(20,2)
	, @aguaInyectadoAcumulado DECIMAL(20,2)
	, @aguaProducidoMensual DECIMAL(20,2)
	, @aguaProducidoAcumulado DECIMAL(20,2)
	, @gasInyectadoMensual DECIMAL(20,2)
	, @gasInyectadoAcumulado DECIMAL(20,2)
	, @gasProducidoMensual DECIMAL(20,2)
	, @gasProducidoAcumulado DECIMAL(20,2)
	, @presionFondo DECIMAL(20,2)
	, @usuario VARCHAR(50)
    , @pden_id VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	INSERT INTO [FOXT].[FORMA22CRDETALLE]
           ([id_cabecera]
           ,[form_id]
           ,[mes]
           ,[petroleoProducidoMensual]
           ,[petroleoProducidoAcumulado]
           ,[aguaInyectadoMensual]
           ,[aguaInyectadoAcumulado]
           ,[aguaProducidoMensual]
           ,[aguaProducidoAcumulado]
           ,[gasInyectadoMensual]
           ,[gasInyectadoAcumulado]
           ,[gasProducidoMensual]
           ,[gasProducidoAcumulado]
           ,[presionFondo]
           ,[fecha_creacion]
           ,[usuario_creacion]
           , [pdenId]
           , [Pozo])
     VALUES
           (@id_cabecera
                , @form_id
                , @mes
                , @petroleoProducidoMensual
                , @petroleoProducidoAcumulado
                , @aguaInyectadoMensual
                , @aguaInyectadoAcumulado
                , @aguaProducidoMensual
                , @aguaProducidoAcumulado
                , @gasInyectadoMensual
                , @gasInyectadoAcumulado
                , @gasProducidoMensual
                , @gasProducidoAcumulado
                , @presionFondo
				, GETDATE()
                , @usuario
                , @pden_id
                , @Pozo)

	SELECT *
	FROM [FOXT].[FORMA22CRDETALLE]
	WHERE FORM_ID = @form_id
		AND id_cabecera = @id_cabecera

END
GO