CREATE TABLE [FOXT].[FORM16]
(
     [form16id]  UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM16_form16 DEFAULT NEWID(),
     [formation] VARCHAR(260),
     [block]     VARCHAR(260),
     [oilfield]  VARCHAR(260),
     [structure] VARCHAR(260),
     [member]    VARCHAR(260),
     [operador] VARCHAR(260),
     [contractid] VARCHAR(260),
     [contract] VARCHAR(260),
     [campoid] VARCHAR(260),
     [campo] VARCHAR(260),
     [yacimiento] VARCHAR(260),
     [row_created_by]          varchar(60) NULL, 
     [row_created_date]        datetime NULL, 
     [row_changed_by]          varchar(60) NULL, 
     [row_changed_date]        datetime NULL
)
