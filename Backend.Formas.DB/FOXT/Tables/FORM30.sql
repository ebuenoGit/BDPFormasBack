CREATE TABLE [FOXT].[FORM30] (
    [form30id]                      UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM30_form30id DEFAULT NEWID(),
    [operador]       VARCHAR(50) NULL,
    [contrato] VARCHAR(50) NULL,
    [date]            DATE NULL,
    [uid]               VARCHAR(50) NULL,
    [lastModified]                NUMERIC (20, 4) NULL,
    [lastModifiedDate]              DATETIME NULL,
    [name]    VARCHAR(50) NULL,
    [size]      DECIMAL(20) NULL,
    [type]    VARCHAR(50) NULL,
    [percent]            DECIMAL(20) NULL,
    [originFileObj_uid]                VARCHAR(50) NULL, 
    [operador_id] VARCHAR(10) NULL, 
    [contrato_id] VARCHAR(10) NULL,
    [row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
);

