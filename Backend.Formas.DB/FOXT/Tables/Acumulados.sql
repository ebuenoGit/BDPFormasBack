CREATE TABLE [FOXT].[Acumulados]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[Id_Forma] UNIQUEIDENTIFIER  NULL,
	[GasAcumulado] FLOAT NULL,
	[AguaAcumulado] FLOAT NULL,
	[CrudoAcumulado] FLOAT NULL
)
