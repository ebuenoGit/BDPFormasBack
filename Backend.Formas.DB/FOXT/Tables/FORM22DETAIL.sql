CREATE TABLE [FOXT].[FORM22DETAIL]
(
	[form22detail] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM22DETAIL_form22id DEFAULT NEWID(),
	[mes] VARCHAR(20),
	[petroleoProducionMensual] numeric(20,4),
	[petroleoProducionAcumulado] numeric(20,4),
	[aguaInyectadoMenual] numeric(20,4),
	[aguaInyectadoAcumulado] numeric(20,4),
	[aguaProducidoMensual] numeric(20,4),
	[aguaProducidoAcumulado] numeric(20,4),
	[gasInyectadoMenual] numeric(20,4),
	[gasInyectadoAcumulado] numeric(20,4),
	[gasProducidoMensual] numeric(20,4),
	[gasProducidoAcumulado] numeric(20,4),
	[precion] numeric(20,4),
	[pdenId] VARCHAR(50),
	[row_created_by]          varchar(60) NULL, 
	[row_created_date]        datetime NULL, 
	[row_changed_by]          varchar(60) NULL, 
	[row_changed_date]        datetime NULL,
	[FormId] UNIQUEIDENTIFIER
)
