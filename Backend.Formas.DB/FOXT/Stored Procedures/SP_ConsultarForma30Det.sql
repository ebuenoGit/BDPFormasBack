CREATE PROCEDURE FOXT.SP_ConsultarForma30Det
	-- Add the parameters for the stored procedure here
	@id AS uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT [form30detail]
      ,[formid]
	  ,[campo_id] 
      ,[campo]
      ,[gasFormacionKPC]
      ,[contenidoPropano]
      ,[contenidoButano]
      ,[contenidoGasolinaNatural]
      ,[gasFormacionProcesado] AS GAS_PROCESADO
      ,[consumoEnCampo] AS CONSUMOS
      ,[generacionElectrica] AS GENERACION_ELECTRICA
      ,[otros] AS OTRAS_VENTAS
      ,[quemadoAire] AS GAS_QUEMADO
      ,[usadoBombeoNeumatico] AS GAS_NEUMATICO
      ,[inyectadoYacimiento] AS GAS_INYECTADO
      ,[productosObtenidosGasTotalGasProcesadoPlanta] AS GP_TRATOTAL_PROC_PLANTA
      ,[productosObtenidosGasPropano] AS PROPANO
      ,[productosObtenidosGasButano] AS BUTANO
      ,[productosObtenidosGasGasolina] AS GASOLINA
      ,[productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos]
      ,[productosGasProcesadoConsumoEnCampo] AS GP_CONSUMOS
      ,[productosGasProcesadoGasoductosUrbanos] AS GP_GASODUCTOS_URBANOS
      ,[productosGasProcesadoGeneracionElectrica] AS GP_GENERACION_ELECTRICA
      ,[productosGasProcesadoOtros] AS GP_OTRAS_VENTAS
      ,[productosGasProcesadoQuemadoAire] AS GP_QUEMADO
      ,[productosGasProcesadoUsadoBombeoNeumatico] AS GP_NEUMATICO
      ,[productosGasProcesadoInyectadoYacimiento] AS GP_INYECTADO
      ,[productosGasProcesadoObservaciones] AS OBSERVACIONES
      ,[REAL_TP]
      ,[REAL]
      ,[REAL_BAS]
      ,[REAL_INC]
      ,[GAS_PROCESADO_TP]
      ,[GAS_QUEMADO_TP]
      ,[GLP_TP]
      ,[GLP]
      ,[PROPANO_TP]
      ,[BUTANO_TP]
      ,[GASOLINA_TP]
      ,[APIASOL_TP]
      ,[APIASOL]
      ,[CONDENSADO_TP]
      ,[CONDENSADO]
      ,[CONSUMOS_TP]
      ,[GASODUCTOS_URBANOS_TP]
      ,[GASODUCTOS_URBANOS]
      ,[GENERACION_ELECTRICA_TP]
      ,[OTRAS_VENTAS_TP]
      ,[GAS_TRANFERENCIA_TP]
      ,[GAS_TRANFERENCIA]
      ,[GAS_NEUMATICO_TP]
      ,[GAS_INYECTADO_TP]
      ,[TOTAL_PROC_PLANTA_TP]
      ,[GP_TRATOTAL_PROC_PLANTA_TP]
      ,[GP_CONSUMOS_TP]
      ,[GP_GASODUCTOS_URBANOS_TP]
      ,[GP_GENERACION_ELECTRICA_TP]
      ,[GP_OTRAS_VENTAS_TP]
      ,[GP_NEUMATICO_TP]
      ,[GP_QUEMADO_TP]
      ,[GP_INYECTADO_TP]
      ,[REPRESENTA_OPER]
      ,[REPRESENTA_ANH]
      ,[IDFORMA]
	  , '' AS TOTAL_PROC_PLANTA
	  , '' AS GP_TRANSFORMADO_TP
	  , '' AS GP_TRANSFORMADO
	  , '' AS ESTADOFORMA
	  , '' AS ROW_CHANGED_BY
	  , GETDATE() AS ROW_CHANGED_DATE 
      , '' AS ROW_CREATED_BY
      , GETDATE() AS ROW_CREATED_DATE 
	  , pden_id
	  , volume_date
  FROM [FOXT].[FORM30DETAIL]
  WHERE formid = @id
  
END
GO