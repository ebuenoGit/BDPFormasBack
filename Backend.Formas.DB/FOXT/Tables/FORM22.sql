CREATE TABLE [FOXT].[FORM22]
(
	[form22id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM22_form22id DEFAULT NEWID(),
	[formation] varchar(60),
    [block]     VARCHAR(50),
    [oilfield]  VARCHAR(50),
    [structure] VARCHAR(50),
    [member]    VARCHAR(50),
    [row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
)
