CREATE PROCEDURE  [FOXT].[SP_Guardar_Det_Cuadro1]
	-- Add the parameters for the stored procedure here
	  @id_cabecera AS uniqueidentifier
	, @form_id AS uniqueidentifier
	, @dias AS VARCHAR(20)
	, @medidaMM AS DECIMAL(20,2)
	, @aforoBLS AS DECIMAL(20,2)
	, @tempF AS DECIMAL(20,2)
	, @factorTemp AS DECIMAL(20,2)
	, @BLS60F AS DECIMAL(20,2)
	, @BSW AS DECIMAL(20,2)
	, @factorBSW AS DECIMAL(20,2)
	, @CTSH AS DECIMAL(20,2)
	, @tempAmb AS DECIMAL(20,2)
	, @BLSNetos AS DECIMAL(20,2)
	, @transfBLS AS DECIMAL(20,2)
	, @recibidoBLS AS DECIMAL(20,2)
	, @entregaBLS AS DECIMAL(20,2)
	, @API60F AS DECIMAL(20,2)
	, @GE AS DECIMAL(20,2)
	, @netosGE AS DECIMAL(20,2)
	, @salBTB AS DECIMAL(20,2)
	, @usuario AS VARCHAR(100)
    , @pden_tanque AS VARCHAR(20)
    , @pden_bateria AS VARCHAR(100)
    ,@trasRecibido as DECIMAL(20,2)
                ,@TrasEnvio  as DECIMAL(20,2)
                ,@movIntraRecibido  as DECIMAL(20,2)
                ,@movIntraEnvio  as DECIMAL(20,2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @aniomes as varchar(20)

	select top 1 @aniomes = anio + mes
	from foxt.CUADRO1CABECERA
	where id_FormaCuadro1 = @id_cabecera
		and Forma_id = @form_id

   INSERT INTO [FOXT].[CUADRO1DETALLE]
           ([id_cabecera]
           ,[Forma_id]
           ,[dias]
           ,[medidaMM]
           ,[aforoBLS]
           ,[tempF]
           ,[factorTemp]
           ,[BLS60F]
           ,[BSW]
           ,[factorBSW]
           ,[CTSH]
           ,[tempAmb]
           ,[BLSNetos]
           ,[transfBLS]
           ,[recibidoBLS]
           ,[entregaBLS]
           ,[API60F]
           ,[GE]
           ,[netosGE]
           ,[salBTB]
           ,[fecha_creacion]
           ,[usuario_crea]
           ,[pdenId]
           ,[pden_tanque]
           ,[pden_bateria]
           ,[TrasRecibido]
           ,[TrasEnvio]
           ,[MovIntraRecibido]
           ,[MovIntraEnvio]
		   , row_created_by
		   ,row_created_date
		   ,row_changed_by
		   ,row_changed_date)
     VALUES
           (@id_cabecera
                , @form_id
                , @aniomes + convert(varchar(2),convert(int,@dias))
                , @medidaMM
                , @aforoBLS
                , @tempF
                , @factorTemp
                , @BLS60F
                , @BSW
                , @factorBSW
                , @CTSH
                , @tempAmb
                , @BLSNetos
                , @transfBLS
                , @recibidoBLS
                , @entregaBLS
                , @API60F
                , @GE
                , @netosGE
                , @salBTB
				, GETDATE()
                , @usuario
                , @pden_tanque
                , @pden_tanque
                , @pden_bateria
                ,@trasRecibido
                ,@TrasEnvio
                ,@movIntraRecibido
                ,@movIntraEnvio
                
				, @usuario
				, GETDATE()
				, @usuario
				, GETDATE())

	select top 1 *
	from foxt.CUADRO1DETALLE
	where Forma_id = @form_id
		and id_cabecera = @id_cabecera
END
GO