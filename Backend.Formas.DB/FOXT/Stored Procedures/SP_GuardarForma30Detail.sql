CREATE PROCEDURE FOXT.SP_GuardarForma30Detail
		  @form30detail AS UniqueIdentifier
		, @formid AS UniqueIdentifier
		, @campo AS VARCHAR(50)
		, @gasFormacionKPC AS DECIMAL(20,2)
		, @contenidoPropano AS DECIMAL(20,2)
		, @contenidoButano AS DECIMAL(20,2)
		, @contenidoGasolinaNatural AS DECIMAL(20,2)
		, @gasFormacionProcesado AS DECIMAL(20,2)
		, @consumoEnCampo AS DECIMAL(20,2)
		, @generacionElectrica AS DECIMAL(20,2)
		, @otros AS DECIMAL(20,2)
		, @quemadoAire AS DECIMAL(20,2)
		, @usadoBombeoNeumatico AS DECIMAL(20,2)
		, @inyectadoYacimiento AS DECIMAL(20,2)
		, @productosObtenidosGasTotalGasProcesadoPlanta AS DECIMAL(20,2)
		, @productosObtenidosGasPropano AS DECIMAL(20,2)
		, @productosObtenidosGasButano AS DECIMAL(20,2)
		, @productosObtenidosGasGasolina AS DECIMAL(20,2)
		, @productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos AS DECIMAL(20,2)
		, @productosGasProcesadoConsumoEnCampo AS DECIMAL(20,2)
		, @productosGasProcesadoGasoductosUrbanos AS DECIMAL(20,2)
		, @productosGasProcesadoGeneracionElectrica AS DECIMAL(20,2)
		, @productosGasProcesadoOtros AS DECIMAL(20,2)
		, @productosGasProcesadoQuemadoAire AS DECIMAL(20,2)
		, @productosGasProcesadoUsadoBombeoNeumatico AS DECIMAL(20,2)
		, @productosGasProcesadoInyectadoYacimiento AS DECIMAL(20,2)
		, @productosGasProcesadoObservaciones AS VARCHAR(2000)
		, @REAL_TP AS VARCHAR(10) = ''
        , @REAL AS VARCHAR(10)  = ''
        , @REAL_BAS AS VARCHAR(10)  = ''
        , @REAL_INC AS VARCHAR(10)  = ''
        , @GAS_PROCESADO_TP AS VARCHAR(10)  = ''
        , @GAS_QUEMADO_TP AS VARCHAR(10)  = ''
        , @GLP_TP AS VARCHAR(10)  = ''
        , @GLP AS VARCHAR(10)  = ''
        , @PROPANO_TP AS VARCHAR(10)  = ''
        , @BUTANO_TP AS VARCHAR(10)  = ''
        , @GASOLINA_TP AS VARCHAR(10)  = ''
        , @APIASOL_TP AS VARCHAR(10)  = ''
        , @APIASOL AS VARCHAR(10)  = ''
        , @CONDENSADO_TP AS VARCHAR(10) = '' 
        , @CONDENSADO AS VARCHAR(10)  = ''
        , @CONSUMOS_TP AS VARCHAR(10)  = ''
        , @GASODUCTOS_URBANOS_TP AS VARCHAR(10)  = ''
        , @GASODUCTOS_URBANOS AS VARCHAR(10)  = ''
        , @GENERACION_ELECTRICA_TP AS VARCHAR(10)  = ''
        , @OTRAS_VENTAS_TP AS VARCHAR(10)  = ''
        , @GAS_TRANFERENCIA_TP AS VARCHAR(10)  = ''
        , @GAS_TRANFERENCIA AS VARCHAR(10)  = ''
        , @GAS_NEUMATICO_TP AS VARCHAR(10)  = ''
        , @GAS_INYECTADO_TP AS VARCHAR(10)  = ''
        , @TOTAL_PROC_PLANTA_TP AS VARCHAR(10)  = ''
        , @GP_TRATOTAL_PROC_PLANTA_TP AS VARCHAR(10) = '' 
        , @GP_TRATOTAL_PROC_PLANTA AS VARCHAR(10)  = ''
        , @GP_CONSUMOS_TP AS VARCHAR(10)  = ''
        , @GP_GASODUCTOS_URBANOS_TP AS VARCHAR(10)  = ''
        , @GP_GENERACION_ELECTRICA_TP AS VARCHAR(10)  = ''
        , @GP_OTRAS_VENTAS_TP AS VARCHAR(10)  = ''
        , @GP_NEUMATICO_TP AS VARCHAR(10)  = ''
        , @GP_QUEMADO_TP AS VARCHAR(10)  = ''
        , @GP_INYECTADO_TP AS VARCHAR(10)  = ''
        , @REPRESENTA_OPER AS VARCHAR(10)  = ''
        , @REPRESENTA_ANH AS VARCHAR(10)  = ''
        , @IDFORMA AS VARCHAR(10) = ''
		, @campo_id AS VARCHAR(10) = ''
		, @pden_id AS VARCHAR(50) = ''
		, @volume_date AS VARCHAR(10) = ''
		, @usuario AS VARCHAR(100) = ''
		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   INSERT INTO [FOXT].[FORM30DETAIL]
				([form30detail]
				,[formid]
				,[campo]
				,[gasFormacionKPC]
				,[contenidoPropano]
				,[contenidoButano]
				,[contenidoGasolinaNatural]
				,[gasFormacionProcesado]
				,[consumoEnCampo]
				,[generacionElectrica]
				,[otros]
				,[quemadoAire]
				,[usadoBombeoNeumatico]
				,[inyectadoYacimiento]
				,[productosObtenidosGasTotalGasProcesadoPlanta]
				,[productosObtenidosGasPropano]
				,[productosObtenidosGasButano]
				,[productosObtenidosGasGasolina]
				,[productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos]
				,[productosGasProcesadoConsumoEnCampo]
				,[productosGasProcesadoGasoductosUrbanos]
				,[productosGasProcesadoGeneracionElectrica]
				,[productosGasProcesadoOtros]
				,[productosGasProcesadoQuemadoAire]
				,[productosGasProcesadoUsadoBombeoNeumatico]
				,[productosGasProcesadoInyectadoYacimiento]
				,[productosGasProcesadoObservaciones]
				, [REAL_TP]  
				, [REAL] 
				, [REAL_BAS] 
				, [REAL_INC] 
				, [GAS_PROCESADO_TP] 
				, [GAS_QUEMADO_TP] 
				, [GLP_TP] 
				, [GLP] 
				, [PROPANO_TP] 
				, [BUTANO_TP] 
				, [GASOLINA_TP] 
				, [APIASOL_TP] 
				, [APIASOL] 
				, [CONDENSADO_TP] 
				, [CONDENSADO] 
				, [CONSUMOS_TP] 
				, [GASODUCTOS_URBANOS_TP] 
				, [GASODUCTOS_URBANOS] 
				, [GENERACION_ELECTRICA_TP] 
				, [OTRAS_VENTAS_TP] 
				, [GAS_TRANFERENCIA_TP] 
				, [GAS_TRANFERENCIA] 
				, [GAS_NEUMATICO_TP] 
				, [GAS_INYECTADO_TP] 
				, [TOTAL_PROC_PLANTA_TP] 
				, [GP_TRATOTAL_PROC_PLANTA_TP] 
				, [GP_TRATOTAL_PROC_PLANTA] 
				, [GP_CONSUMOS_TP] 
				, [GP_GASODUCTOS_URBANOS_TP] 
				, [GP_GENERACION_ELECTRICA_TP] 
				, [GP_OTRAS_VENTAS_TP] 
				, [GP_NEUMATICO_TP] 
				, [GP_QUEMADO_TP] 
				, [GP_INYECTADO_TP] 
				, [REPRESENTA_OPER] 
				, [REPRESENTA_ANH] 
				, [IDFORMA]
				, [campo_id]
				, [pden_id]
				, [volume_date]
				, [row_created_by]
				, [row_created_date]
				, [row_changed_by]
				, [row_changed_date])
     VALUES
           (
				  NEWID()
				, @formid
				, @campo
				, @gasFormacionKPC
				, @contenidoPropano
				, @contenidoButano
				, @contenidoGasolinaNatural
				, @gasFormacionProcesado
				, @consumoEnCampo
				, @generacionElectrica
				, @otros
				, @quemadoAire
				, @usadoBombeoNeumatico
				, @inyectadoYacimiento
				, @productosObtenidosGasTotalGasProcesadoPlanta
				, @productosObtenidosGasPropano
				, @productosObtenidosGasButano
				, @productosObtenidosGasGasolina
				, @productosObtenidosGasTransformadoGasolinaNaturalPropanosButanos
				, @productosGasProcesadoConsumoEnCampo
				, @productosGasProcesadoGasoductosUrbanos
				, @productosGasProcesadoGeneracionElectrica
				, @productosGasProcesadoOtros
				, @productosGasProcesadoQuemadoAire
				, @productosGasProcesadoUsadoBombeoNeumatico
				, @productosGasProcesadoInyectadoYacimiento
				, @productosGasProcesadoObservaciones
				, @REAL_TP  
				, @REAL 
				, @REAL_BAS 
				, @REAL_INC 
				, @GAS_PROCESADO_TP 
				, @GAS_QUEMADO_TP 
				, @GLP_TP 
				, @GLP 
				, @PROPANO_TP 
				, @BUTANO_TP 
				, @GASOLINA_TP 
				, @APIASOL_TP 
				, @APIASOL 
				, @CONDENSADO_TP 
				, @CONDENSADO 
				, @CONSUMOS_TP 
				, @GASODUCTOS_URBANOS_TP 
				, @GASODUCTOS_URBANOS 
				, @GENERACION_ELECTRICA_TP 
				, @OTRAS_VENTAS_TP 
				, @GAS_TRANFERENCIA_TP 
				, @GAS_TRANFERENCIA 
				, @GAS_NEUMATICO_TP 
				, @GAS_INYECTADO_TP 
				, @TOTAL_PROC_PLANTA_TP 
				, @GP_TRATOTAL_PROC_PLANTA_TP 
				, @GP_TRATOTAL_PROC_PLANTA 
				, @GP_CONSUMOS_TP 
				, @GP_GASODUCTOS_URBANOS_TP 
				, @GP_GENERACION_ELECTRICA_TP 
				, @GP_OTRAS_VENTAS_TP 
				, @GP_NEUMATICO_TP 
				, @GP_QUEMADO_TP 
				, @GP_INYECTADO_TP 
				, @REPRESENTA_OPER 
				, @REPRESENTA_ANH 
				, @IDFORMA
				, @campo_id
				, @pden_id
				, @volume_date
				, @usuario
				, GETDATE()
				, @usuario
				, GETDATE()
		   )

		SELECT *
		FROM [FOXT].[FORM30DETAIL]
		WHERE formid = @formid

END
GO
