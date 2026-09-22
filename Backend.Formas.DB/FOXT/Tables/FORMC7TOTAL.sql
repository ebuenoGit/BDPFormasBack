CREATE TABLE [FOXT].[FORMC7TOTAL]
(
	[formac7totalid] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORMC7_formac7totalid DEFAULT NEWID() CONSTRAINT FORMC7TOTAL_PK PRIMARY KEY,
	[formid] UNIQUEIDENTIFIER NULL,
	[productionbbls] NUMERIC(20,4) NULL,
	[productionactivewellsartificial] NUMERIC(20,4) NULL,
	[productionInactiveOilWaterWell] NUMERIC(20,4) NULL,
	[productioninactivewellclosedtemp] NUMERIC(20,4) NULL,
	[productioninactivewellsmiscellaneous] NUMERIC(20,4) NULL,
	[productiontotalwellsassets] NUMERIC(20,4) NULL,
	[injectorswellsplugged] NUMERIC(20,4) NULL,
	[wellpluggeddry] NUMERIC(20,4) NULL,
	[wellspluggedabandoned] NUMERIC(20,4) NULL,
	[productionwellsassetflownatural] NUMERIC(20,4) NULL,
	[totalofficiallycompletedwells] NUMERIC(20,4) NULL,
	[wellstemporarilysuspended] NUMERIC(20,4) NULL,
	[wellssuspendeddryunfinished] NUMERIC(20,4) NULL,
	[row_created_by]          varchar(60) NULL, 
	[row_created_date]        datetime NULL, 
	[row_changed_by]          varchar(60) NULL, 
	[row_changed_date]        datetime NULL
)
