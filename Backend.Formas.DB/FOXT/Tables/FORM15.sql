CREATE TABLE [FOXT].[FORM15]
(
	[form15id]  UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM15_form15id DEFAULT NEWID() CONSTRAINT PK_FORM15 PRIMARY KEY, 
    [formation] NVARCHAR(60) NULL, 
    [block] VARCHAR(50) NULL, 
    [oilfield] VARCHAR(50) NULL, 
    [structure] VARCHAR(50) NULL, 
    [member] VARCHAR(50) NULL, 
    [totalivdays] NUMERIC(20, 4) NULL, 
    [totalivaccumulateddays] NUMERIC(20, 4) NULL, 
    [totalivpoundsmonth] NUMERIC(20, 4) NULL, 
    [totalivpoundsaccumulateddays] NUMERIC(20, 4) NULL, 
    [totalivbtumonth] NUMERIC(20, 4) NULL, 
    [totalivbtuaccumulateddays] NUMERIC(20, 4) NULL, 
    [totalquality] NUMERIC(20, 4) NULL, 
    [totalmonthlyoil] NUMERIC(20, 4) NULL, 
    [totalaccumulatedoil] NUMERIC(20, 4) NULL, 
    [totalmonthlywater] NUMERIC(20, 4) NULL, 
    [totalaccumulatedwater] NUMERIC(20, 4) NULL,

)
