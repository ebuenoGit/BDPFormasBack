CREATE TABLE [FOXT].[FORMC7]
(
	[formc7id]      UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORMC7_formc7id DEFAULT NEWID(), 
    [formation]     VARCHAR(60) NULL, 
    [block]         VARCHAR(50) NULL, 
    [oilfield]      VARCHAR(60) NULL, 
    [structure]     VARCHAR(50) NULL, 
    [member]        VARCHAR(50) NULL,
    [operadorid] VARCHAR(260) NULL,
    [operador] VARCHAR(260) NULL,
    [contractid] VARCHAR(260) NULL,
    [contract] VARCHAR(260) NULL,
    [campoid] VARCHAR(260) NULL,
    [campo] VARCHAR(260) NULL,
    [row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
)
