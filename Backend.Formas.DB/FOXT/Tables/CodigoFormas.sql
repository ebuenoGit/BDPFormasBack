CREATE TABLE [FOXT].[CodigoFormas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[NombreForma] [varchar](50) NOT NULL,
	[CodigoForma] [int] NOT NULL,
	[Estado] [bit] NOT NULL default(1)
) ON [PRIMARY]
GO