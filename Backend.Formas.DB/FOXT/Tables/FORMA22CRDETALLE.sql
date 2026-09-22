CREATE TABLE FOXT.FORMA22CRDETALLE
(
  id_cabecera UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORMA22CRDETALLE_id_cabecera DEFAULT NEWID() CONSTRAINT PK_FORMF22CRDETALLE PRIMARY KEY
, form_id UniqueIdentifier  NULL
, mes VARCHAR(10)
, petroleoProducidoMensual DECIMAL(20,2)
, petroleoProducidoAcumulado DECIMAL(20,2)
, aguaInyectadoMensual DECIMAL(20,2)
, aguaInyectadoAcumulado DECIMAL(20,2)
, aguaProducidoMensual DECIMAL(20,2)
, aguaProducidoAcumulado DECIMAL(20,2)
, gasInyectadoMensual DECIMAL(20,2)
, gasInyectadoAcumulado DECIMAL(20,2)
, gasProducidoMensual DECIMAL(20,2)
, gasProducidoAcumulado DECIMAL(20,2)
, presionFondo DECIMAL(20,2)
, fecha_creacion DATETIME
, usuario_creacion VARCHAR(50)	
, pdenId VARCHAR(50)
, Pozo VARCHAR(50) NULL
)