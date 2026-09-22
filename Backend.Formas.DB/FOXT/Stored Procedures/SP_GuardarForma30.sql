CREATE PROCEDURE [FOXT].[SP_GuardarForma30]
	 @form30id AS UniqueIdentifier
	, @operador AS VarChar(200)
	, @contrato AS VarChar(200)
	, @date AS Date
	, @uid AS VarChar
	, @lastModified AS Decimal(20,0)
	, @lastModifiedDate AS DateTime
	, @name AS VarChar
	, @size AS Decimal(20,0)
	, @type AS VarChar
	, @percent AS Decimal(20,0)
	, @originFileObj_uid AS VarChar
	, @operador_id AS VARCHAR(10)
	, @contrato_id AS VARCHAR(10)
	, @usuario AS VARCHAR(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   
		   INSERT INTO [FOXT].[FORM30]
           ([form30id]
           ,[operador]
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
		   ,[operador_id]
		   ,[contrato_id]
		   ,[row_created_by]
		   ,[row_created_date]
		   ,[row_changed_by]
		   ,[row_changed_date]
		   )
     VALUES
           (
			  @form30id
			, @operador
			, @contrato
			, @date
			, @uid
			, @lastModified
			, @lastModifiedDate
			, @name
			, @size
			, @type
			, @percent
			, @originFileObj_uid
			, @operador_id 
			, @contrato_id
			, @usuario
			, GETDATE()
			, @usuario
			, GETDATE()
		   )

		   SELECT *
		   FROM [FOXT].[FORM30]
		   WHERE form30id = @form30id
		   ORDER BY form30id DESC 
END
