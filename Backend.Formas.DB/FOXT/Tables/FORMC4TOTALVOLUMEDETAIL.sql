CREATE TABLE [FOXT].[FORMC4TOTALVOLUMEDETAIL] (
    [formc4totalvolumedetailid] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORMC4TOTALVOLUMEDETAIL_formc4totalvolumedetailid DEFAULT NEWID(),
    [formid]                    UNIQUEIDENTIFIER NULL,
    [formation]                 VARCHAR (60)    NULL,
    [activity]                  VARCHAR (30)    NULL,
    [totalvolume]               NUMERIC (20, 4) NULL,
    [row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
);

