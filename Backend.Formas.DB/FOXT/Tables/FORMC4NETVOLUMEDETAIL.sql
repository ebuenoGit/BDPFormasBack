CREATE TABLE [FOXT].[FORMC4NETVOLUMEDETAIL] (
    [formc4netvolumedetailid] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORMC4NETVOLUMEDETAIL_formc4netvolumedetailid DEFAULT NEWID(),
    [formid]                  UNIQUEIDENTIFIER    NULL,
    [pdenid]                  VARCHAR (60)    NULL,
    [formation]               VARCHAR (60)    NULL,
    [municipality]            VARCHAR (60)    NULL,
    [danecode]                VARCHAR (12)    NULL,
    [activity]                VARCHAR (30)    NULL,
    [basica]                  VARCHAR (30)    NULL,
    [incremental]             VARCHAR (30)    NULL,
    [volume]                  NUMERIC (20, 4) NULL,
    [productiontype]          VARCHAR (30)    NULL,
    [row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
);
