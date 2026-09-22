CREATE PROCEDURE [FOXT].[SP_GuardarCabeceraForma30]
	  @concreteformid AS uniqueidentifier
    , @maincampid AS DECIMAL
    , @company AS VARCHAR(50)
    , @contract AS VARCHAR(50)
    , @battery AS VARCHAR(50)
    , @tank AS VARCHAR(50)
    , @month AS DECIMAL
    , @year AS DECIMAL
    , @explotationmodality AS VARCHAR(50)
    , @annotations AS VARCHAR(50)
    , @version AS DECIMAL
    , @currentstate AS uniqueidentifier
    , @generationflag AS DECIMAL
    , @campid AS DECIMAL
    , @pdenid AS VARCHAR(50)
    , @formid AS uniqueidentifier
    , @generationjobid AS DECIMAL
    , @iqistatus AS DECIMAL
    , @usersigning AS VARCHAR(10)
    , @minrepsigning AS VARCHAR(50)
    , @formname AS VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	set @concreteformid = NEWID()
    
	INSERT INTO [FOXT].[CONCRETEFORM]
           ([concreteformid]
           ,[maincampid]
           ,[company]
           ,[contract]
           ,[battery]
           ,[tank]
           ,[month]
           ,[year]
           ,[explotationmodality]
           ,[annotations]
           ,[version]
           ,[currentstate]
           ,[generationflag]
           ,[campid]
           ,[pdenid]
           ,[formid]
           ,[generationjobid]
           ,[iqistatus]
           ,[usersigning]
           ,[minrepsigning]
           ,[formname])
     VALUES
           (  @concreteformid
           , @maincampid
           , @company
           , @contract
           , @battery
           , @tank
           , @month
           , @year
           , @explotationmodality
           , @annotations
           , @version
           , NEWID()
           , @generationflag
           , @campid
           , @pdenid
           , NEWID()
           , @generationjobid
           , @iqistatus
           , @usersigning
           , @minrepsigning
           , @formname)


	SELECT *
	FROM [FOXT].[CONCRETEFORM]
	WHERE concreteformid = @concreteformid

END