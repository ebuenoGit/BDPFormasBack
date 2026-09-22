CREATE PROCEDURE FOXT.SP_GuardarAprobacion30
	-- Add the parameters for the stored procedure here
	   @ID_Forma AS uniqueidentifier
	, @FechaForma AS DATETIME
	, @Usuario AS VARCHAR(50) = ''
	, @UsuarioNombre AS VARCHAR(50) = ''
	, @FechaCarga AS DATETIME
	, @Estado AS uniqueidentifier
	, @ComparativoAgua AS DECIMAL = 0
	, @ComparativoGas AS DECIMAL = 0
	, @ComparativoCrudo AS DECIMAL = 0
	, @FechaActualizacion AS DATETIME
	, @UsuarioAprobador AS VARCHAR(50) = ''
	, @UsuarioNombreAprobador AS VARCHAR(150) = ''
	, @FormTypeID AS uniqueidentifier
	, @FormEntidad AS VARCHAR(50) = ''
	, @FormaName AS VARCHAR(50) = ''
	, @urlForma AS VARCHAR(2000) = ''
	, @operador AS VARCHAR(50) = ''
	, @contrato AS VARCHAR(50) = ''
	, @campo AS VARCHAR(50) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

--declare temp as varchar(10) = 'tmp'

	INSERT INTO [FOXT].[APROBACIONCARGA]
           ([ID_Forma]
           ,[FechaForma]
           ,[Usuario]
           ,[UsuarioNombre]
           ,[FechaCarga]
           ,[Estado]
           ,[ComparativoAgua]
           ,[ComparativoGas]
           ,[ComparativoCrudo]
           ,[FechaActualizacion]
           ,[UsuarioAprobador]
           ,[UsuarioNombreAprobador]
           ,[FormTypeID]
           ,[FormEntidad]
           ,[FormaName]
           ,[urlForma]
           ,[operador]
           ,[contrato]
           ,[campo])
     VALUES
           (@ID_Forma 
           ,@FechaForma 
           ,@Usuario
           ,@UsuarioNombre 
           ,@FechaCarga 
           ,@Estado
           ,@ComparativoAgua 
           ,@ComparativoGas
           ,@ComparativoCrudo
           ,@FechaActualizacion 
           ,@UsuarioAprobador
           ,@UsuarioNombreAprobador 
           ,@FormTypeID
           ,@FormEntidad 
           ,@FormaName 
           ,@urlForma
           ,@operador 
           ,@contrato
           ,@campo)


		   SELECT *
		   FROM [FOXT].[APROBACIONCARGA]
		   WHERE ID_Forma = @ID_Forma

END
GO