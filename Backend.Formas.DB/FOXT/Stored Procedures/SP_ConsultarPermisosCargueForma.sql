CREATE PROCEDURE [FOXT].[SP_ConsultarPermisosCargueForma]
	-- Add the parameters for the stored procedure here
	  @forma AS VARCHAR(50) --= 'Forma Cuadro 1A'
	, @fechaForma AS DATETIME --= '20200801'
	, @operador AS VARCHAR(20) --= 'PETROSANTANDER (COLOMBIA) INC.'
	, @contrato AS VARCHAR(20) = NULL
	, @usuario AS VARCHAR(20) --= 'JJPRIETO'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	declare @forma_id as Uniqueidentifier 
			, @FormaName as varchar(100)
	/*si el archivo no esta aprobado se elimina para volver a carguar el registro.*/

	SELECT @forma_id = max(c.ID_Forma)
	FROM FOXT.APROBACIONCARGA AS C 
	INNER JOIN FOXT.FORMSTATE AS F ON 
	C.Estado = F.formstateid
	WHERE c.FormaName = @forma
	AND F.name = 'En Proceso de Aprobación'
	AND C.FechaForma = @fechaForma

	delete t
	FROM FOXT.CONCRETEFORM AS T
	where formid = @forma_id

	delete t
	from FOXT.APROBACIONCARGA as t 
	where t.ID_Forma = @forma_id


	set @FormaName = (select FormaName
	from FOXT.APROBACIONPRECARGA
	WHERE FormaName = @forma
	and GETDATE() between FechaApertura and FechaCierre
	AND FechaForma = @fechaForma
	and Activo = 1)	

	if((@FormaName = '' or @FormaName is null) AND @fechaForma < GETDATE())
	begin 
	/*Si devuelve registros no tiene permisos para realiar la cargua*/

		if(SELECT count(0)
		FROM FOXT.APROBACIONCARGA AS C 
		INNER JOIN FOXT.FORMSTATE AS F ON 
		C.Estado = F.formstateid
		WHERE c.FormaName = @forma
		and f.name = 'Aprobada'
		and datepart(year,c.FechaForma) = datepart(year,@fechaForma)
		and datepart(MONTH,c.FechaForma) = datepart(MONTH,@fechaForma)) > 0
		begin
			IF(@contrato IS NULL)
			BEGIN
				SELECT ID_Forma
				FROM FOXT.APROBACIONCARGA AS C 
				INNER JOIN FOXT.FORMSTATE AS F ON 
				C.Estado = F.formstateid
				WHERE 
				c.FormaName = @forma
				and f.name = 'Aprobada'
				and datepart(year,c.FechaForma) = datepart(year,@fechaForma)
				and datepart(MONTH,c.FechaForma) = datepart(MONTH,@fechaForma)	
			END
			ELSE
			BEGIN
				SELECT ID_Forma
				FROM FOXT.APROBACIONCARGA AS C 
				INNER JOIN FOXT.FORMSTATE AS F ON 
				C.Estado = F.formstateid
				WHERE 
				c.FormaName = @forma
				and c.contrato = @contrato
				and f.name = 'Aprobada'
				and datepart(year,c.FechaForma) = datepart(year,@fechaForma)
				and datepart(MONTH,c.FechaForma) = datepart(MONTH,@fechaForma)
			END
			
		end
		else if(DATEDIFF(MM, @fechaForma,getdate()) > 1)
		begin
			select 1 as cantidad
		end
	end
END