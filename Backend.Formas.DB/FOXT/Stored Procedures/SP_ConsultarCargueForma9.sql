CREATE PROCEDURE FOXT.SP_ConsultarCargueForma9 
	-- Add the parameters for the stored procedure here
	@fecha AS DATE
	, @forma AS VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    
	SELECT formname, month as mes, year as anio
	FROM foxt.CONCRETEFORM as C
	WHERE c.formname = @forma
		and c.year = DATEPART(YEAR,@fecha)
		and c.month = DATEPART(MONTH,@fecha)

END
GO