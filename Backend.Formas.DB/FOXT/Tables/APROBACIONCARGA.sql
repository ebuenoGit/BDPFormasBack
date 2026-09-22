CREATE TABLE [FOXT].[APROBACIONCARGA] (
    [Id]                 NUMERIC (38) IDENTITY (1, 1) NOT NULL,
    [ID_Forma]           UNIQUEIDENTIFIER  NULL,
    [FechaForma]         DATETIME     NULL,
    [Usuario]            VARCHAR(50) NULL,
    [UsuarioNombre]      VARCHAR(150) NULL,
    [FechaCarga]         DATETIME     NULL,
    [Estado]             UNIQUEIDENTIFIER  NULL,
    [ComparativoAgua]    NUMERIC (38) NULL,
    [ComparativoGas]     NUMERIC (38) NULL,
    [ComparativoCrudo]   NUMERIC (38) NULL,
    [FechaActualizacion] DATETIME     NULL,
    [UsuarioAprobador]   VARCHAR(50) NULL,
    [UsuarioNombreAprobador]      VARCHAR(150) NULL,
    [FormTypeID] UNIQUEIDENTIFIER NULL ,
    [FormEntidad] VARCHAR(50), 
    [FormaName] VARCHAR(50) NULL, 
    [UrlForma] VARCHAR(150) NULL, 
    [Operador] VARCHAR(50) NULL, 
    [Contrato] VARCHAR(50) NULL, 
    [Campo] VARCHAR(50) NULL
);

