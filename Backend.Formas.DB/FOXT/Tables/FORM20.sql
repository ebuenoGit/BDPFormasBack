CREATE TABLE [FOXT].[FORM20]
(
	[form20id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM20_form20id DEFAULT NEWID(),
	[formation] varchar(60),
    [block]     VARCHAR(50),
    [oilfield]  VARCHAR(50),
    [structure] VARCHAR(50),
    [member]    VARCHAR(50),
    [operador] VARCHAR(260),
    [operadorId] VARCHAR(30),
    [campo] VARCHAR(260),
    [campoId] VARCHAR(30),
    [contrato] VARCHAR(260),
    [contratoId] VARCHAR(30), 
    [formacion_id] VARCHAR(50) NULL, 
    [formacion_set_id] VARCHAR(50) NULL,
    [row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
);
