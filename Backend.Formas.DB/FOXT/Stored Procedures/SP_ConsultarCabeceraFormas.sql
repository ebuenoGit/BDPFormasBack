CREATE PROCEDURE FOXT.SP_ConsultarCabeceraFormas
	-- Add the parameters for the stored procedure here
	@id AS DECIMAL
AS
BEGIN

	SELECT --CD.CodigoForma AS FORMA_CODIGO,
		'20' AS FORMA_CODIGO,
        AP.FormaName AS FORMA_NOMBRE,
        C.month AS MES,
        C.year AS ANIO,
        F.OPERADORID AS OPERADOR_ID,
        AP.operador AS OPERADOR,
        F.CONTRATOID AS CONTRATO_ID,
        AP.contrato AS CONTRATO,
        F.CAMPOID AS CAMPO_ID,
        AP.campo AS CAMPO,
        '' AS BATERIA_ID,
        '' AS BATERIA,
        '' AS TANQUE_ID,
        '' AS TANQUE,
        '' AS ESTRUCTURA_ID,
        '' AS ESTRUCTURA,
        '' AS BLOQUE_ID,
        '' AS BLOQUE,
        f.formacion_id AS FORMACION_ID,
        f.formacion_set_id AS FORMACION_SET_ID,
        f.formation AS FORMACION,
        '' AS MIEMBRO_ID,
        '' AS MIEMBRO,
        '' AS YACIMIENTO_ID,
        '' AS YACIMIENTO,
        '' AS MODALIDADEXPLOTACION_ID,
        'Comercial' AS MODALIDADEXPLOTACION,
        'Representante Test OPERADOR' AS REPRESENTANTE_OPERADOR_NM,
        '123456789' AS REPRESENTANTE_OPERADOR_TP,
        'Representante Test ANH' AS REPRESENTANTE_ANH_NAME,
        '123456' AS REPRESENTANTE_ANH_TP,
        'AVM' AS GENERADO_DESDE,
        'AVM' AS ROW_CHANGED_BY,
        AP.FechaActualizacion AS ROW_CHANGED_DATE,
        AP.FechaCarga AS ROW_CREATED_BY,
        AP.UsuarioNombre AS ROW_CREATED_DATE,
        '' AS OBSERVACIONES
	FROM FOXT.APROBACIONCARGA AS AP (NOLOCK)
	INNER JOIN FOXT.CONCRETEFORM AS C (NOLOCK) ON 
		AP.ID_Forma = C.formid
	INNER JOIN FOXT.FORM20 AS F ON 
		AP.ID_Forma = F.form20id
	WHERE AP.Id = @id

END
GO