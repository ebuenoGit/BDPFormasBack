-- =============================================
-- Author:		JJPRIETO
-- Create date: 25/08/2020
-- Description:	Inserta los totales de la forma cuadro 1
-- =============================================
CREATE PROCEDURE FOXT.SP_Guardar_Tot_Cuadro1
	-- Add the parameters for the stored procedure here
	  @id_cabecera AS uniqueidentifier
	, @form_id AS uniqueidentifier
	, @BLS60F AS DECIMAL(20,2)
	, @BLSNetos AS DECIMAL(20,2)
	, @recibidoBLS AS DECIMAL(20,2)
	, @entregaBLS AS DECIMAL(20,2)
	, @usuario AS VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [FOXT].[CUADRO1TOTAL]
           ([id_cabecera]
           ,[Forma_id]
           ,[BLS60F]
           ,[BLSNetos]
           ,[recibidoBLS]
           ,[entregaBLS]
           ,[fecha_creacion]
           ,[usuario_crea]
		   ,row_created_by
		   ,row_created_date
		   ,row_changed_by
		   ,row_changed_date)
     VALUES
           (@id_cabecera
                , @form_id
                , @BLS60F
				, @BLSNetos
                , @recibidoBLS
                , @entregaBLS
				, getdate()
                , @usuario
				, @usuario
				, GETDATE()
				, @usuario
				, GETDATE())


		select top 1 *
		from foxt.CUADRO1TOTAL 
		where Forma_id = @form_id
			and id_cabecera = @id_cabecera

END
GO