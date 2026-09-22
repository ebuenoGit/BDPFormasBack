CREATE TABLE [FOXT].[Forma17CRDetalle](
	[id_detalle] UNIQUEIDENTIFIER NOT NULL ,
	[form_id] [uniqueidentifier] NOT NULL,
	[pozo] [varchar](100) NULL,
	[diasEnElMes] [decimal](20, 2) NULL,
	[diasAcumulados] [decimal](20, 2) NULL,
	[produccionGasMCPDiaria] [decimal](20, 2) NULL,
	[produccionGasMCPMensual] [decimal](20, 2) NULL,
	[produccionGasMCPAcumulada] [decimal](20, 2) NULL,
	[produccionAguaMensual] [decimal](20, 2) NULL,
	[produccionAguaAcumulada] [decimal](20, 2) NULL,
	[estadoPozosFinalMes] [VARCHAR](100) NULL,
	[fecha_creacion] [datetime] NULL,
	[usuario_creacion] [varchar](110) NULL,
	[pdenId] VARCHAR(50),
	[row_created_by]          varchar(60) NULL, 
	[row_created_date]        datetime NULL, 
	[row_changed_by]          varchar(60) NULL, 
	[row_changed_date]        datetime NULL
) ON [PRIMARY]
GO
