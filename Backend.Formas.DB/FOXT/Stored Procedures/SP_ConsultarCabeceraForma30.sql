CREATE PROCEDURE FOXT.SP_ConsultarCabeceraForma30
	-- Add the parameters for the stored procedure here
	@id AS uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

    
	SELECT [concreteformid]
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
      ,[formname]
	FROM FOXT.CONCRETEFORM
	WHERE formid = @id

END
GO
