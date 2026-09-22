CREATE TABLE [FOXT].[FORM21INYECTIONDETAIL]
(
	[form21detailid]	UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM21INYECTIONDETAIL_form21detailid DEFAULT NEWID(),
	[formid]			UNIQUEIDENTIFIER  NULL,
    [oilwell]			VARCHAR(260) NULL,
    [zone]				VARCHAR(260) NULL,
	[gidays]			NUMERIC(20,4) NULL,
	[giaccumulateddays] NUMERIC(20,4) NULL,
	[pressure]          NUMERIC(20,4) NULL,
	[gidailygas]        NUMERIC(20,4) NULL,
	[gimonthlygas]      NUMERIC(20,4) NULL,
	[giaccumulatedgas]  NUMERIC(20,4) NULL,
	[oilwellfinalstate] varchar(260) NULL,
	[poolname]          VARCHAR(260) NULL,
	[pden_id]           VARCHAR(260) NULL,
	[danecode]          varchar(260) NULL,
	[row_created_by]          varchar(60) NULL, 
	[row_created_date]        datetime NULL, 
	[row_changed_by]          varchar(60) NULL, 
	[row_changed_date]        datetime NULL
)
