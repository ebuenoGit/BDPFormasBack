CREATE TABLE [FOXT].[FORM21TOTALVOLUMEDETAIL]
(
	[form21tvdetailid]       UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM21TOTALVOLUMEDETAIL_form21tvdetailid DEFAULT NEWID(),
	[volumetype]             NUMERIC(1) NULL,
	[accumulategasinjection] NUMERIC(20,4) NULL,
	[monthlygasinjection]    NUMERIC(20,4) NULL,
	[dailygasinjection]      NUMERIC(20,4) NULL,
	[formid]                 UNIQUEIDENTIFIER NULL,
	[formation]              VARCHAR(260) NULL,
	[row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
)
