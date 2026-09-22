-- =============================================
-- Author:		JJPRIETO
-- Create date: 25/08/2020
-- Description:	Inserta la cabecera de forma Cuadro 1
-- =============================================
CREATE PROCEDURE FOXT.SP_Guardar_Cab_Cuadro1
	-- Add the parameters for the stored procedure here
	  @form_id AS UniqueIdentifier
	, @mes AS VARCHAR(10)
	, @anio AS VARCHAR(10)
	, @compania_id AS INT
	, @compania AS VARCHAR(100)
	, @contrato_id AS INT
	, @contrato AS VARCHAR(100)
	, @campo_id AS INT
	, @campo AS VARCHAR(100)
	, @lugar AS VARCHAR(100)
	, @tanque AS VARCHAR(100)
	, @bateria AS VARCHAR(100)
	, @usuario AS VARCHAR(100)
    , @pden_id_tanque AS VARCHAR(50)
    , @pden_id_bateria AS VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [FOXT].[CUADRO1CABECERA]
           ([id_FormaCuadro1]
           ,[Forma_id]
           ,[mes]
           ,[anio]
           ,[compania_id]
           ,[compania]
           ,[contrato_id]
           ,[contrato]
           ,[campo_id]
           ,[campo]
           ,[lugar]
           ,[tanque]
           ,[bateria]
           ,[fecha_creacion]
           ,[usuario_crea]
           , [id_tanque]
           , [id_bateria]
		   ,row_created_by
		   ,row_created_date
		   ,row_changed_by
		   ,row_changed_date)
     VALUES
           (NEWID()
           , @Form_id
           , @mes
           , @anio
           , @compania_id
           , @compania
           , @contrato_id
           , @contrato
           , @campo_id
           , @campo
           , @lugar
           , @tanque
           , @bateria
           , getdate()
           , @usuario
           , @pden_id_tanque
           , @pden_id_bateria
		   ,@usuario
		   ,GETDATE()
		   ,@usuario
		   ,GETDATE())

		   select *
		   from FOXT.CUADRO1CABECERA
		   where forma_id = @form_id
END
GO
