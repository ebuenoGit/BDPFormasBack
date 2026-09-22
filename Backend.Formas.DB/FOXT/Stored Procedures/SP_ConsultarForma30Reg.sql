CREATE PROCEDURE FOXT.SP_ConsultarForma30Reg
	@id AS uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [form30id]
	  ,[operador_id] 
      ,[operador]
	  ,[contrato_id]
      ,[contrato]
      ,[date]
      ,[uid]
      ,[lastModified]
      ,[lastModifiedDate]
      ,[name]
      ,[size]
      ,[type]
      ,[percent]
      ,[originFileObj_uid]
  FROM [FOXT].[FORM30]
  where form30id = @id

END
GO