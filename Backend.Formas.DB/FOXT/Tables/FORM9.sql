CREATE TABLE [FOXT].[FORM9] (
    [form9id]   UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM9_form9id DEFAULT NEWID(),
    [block]     VARCHAR (50) NULL,
    [oilfield]  VARCHAR (50) NULL,
    [structure] VARCHAR (50) NULL,
    [member]    VARCHAR (50) NULL,
    [operadorId] VARCHAR(50) NULL,
    [campoId] VARCHAR(50) NULL,
    [bloqueId] VARCHAR(50) NULL,
    [formacionId] Varchar(50) NULL,
    [formacionSetId] Varchar(50) NULL,
    [yacimientoId] VARCHAR(50) NULL,
    [campo] VARCHAR(50) NULL,
    [bloque] VARCHAR(50) NULL,
    [yacimiento] VARCHAR(50) NULL,
    [formacion] VARCHAR(50) NULL,
    [contratoId] VARCHAR(50) NULL,
    [row_created_by] VARCHAR(60) NULL, 
    [row_created_date] datetime NULL, 
    [row_changed_by] VARCHAR(60) NULL, 
    [row_changed_date] datetime NULL
    CONSTRAINT [PK_FORM9] PRIMARY KEY CLUSTERED ([form9id] ASC)
);

