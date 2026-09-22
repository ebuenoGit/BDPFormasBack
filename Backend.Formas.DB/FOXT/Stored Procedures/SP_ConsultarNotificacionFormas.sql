CREATE PROCEDURE FOXT.SP_ConsultarNotificacionFormas
	-- Add the parameters for the stored procedure here
	@id as decimal
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @forma as varchar(20)

	set @forma = (SELECT FormaName FROM FOXT.APROBACIONCARGA WHERE ID = @ID)

	
	IF(@forma = 'Forma 30SEE')
	begin

		DECLARE @campo as varchar(200)
	, @campoRegistros as varchar(max) = ''

		DECLARE MY_CURSOR CURSOR 
		  LOCAL STATIC READ_ONLY FORWARD_ONLY
		FOR 
		select f.campo
		from foxt.APROBACIONCARGA as ap
		inner join foxt.FORM30DETAIL as f on 
			ap.ID_Forma = f.formid
			WHERE ap.ID = @ID

		OPEN MY_CURSOR
		FETCH NEXT FROM MY_CURSOR INTO @campo
		WHILE @@FETCH_STATUS = 0
		BEGIN 
			--Do something with Id here
			set @campoRegistros = @campoRegistros + @campo+ ', ' 

			FETCH NEXT FROM MY_CURSOR INTO @campo
		END
		CLOSE MY_CURSOR
		DEALLOCATE MY_CURSOR

		select ap.operador, ap.contrato, @campoRegistros as campo, ap.UsuarioNombre, ap.UsuarioAprobador
		from foxt.APROBACIONCARGA as ap
		inner join foxt.FORM30DETAIL as f on 
			ap.ID_Forma = f.formid
		WHERE AP.ID = @id
	end
	else
	begin
		SELECT operador
				, contrato
				, campo
				, UsuarioNombre
				, '' as UsuarioAprobador
		FROM FOXT.APROBACIONCARGA
		WHERE ID = @ID
	end
	

END
GO
