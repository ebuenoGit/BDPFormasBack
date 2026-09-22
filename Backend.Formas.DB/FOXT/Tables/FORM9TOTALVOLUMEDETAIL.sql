CREATE TABLE [FOXT].[FORM9TOTALVOLUMEDETAIL] (
    [form9tvdetailid]           UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM9TOTALVOLUMEDETAIL_form9tvdetailid DEFAULT NEWID(),
    [volumetype]                NUMERIC (38)     NULL,
    [accumulategasproduction]   NUMERIC (20, 4) NULL,
    [montlhygasproduction]      NUMERIC (20, 4) NULL,
    [dailygasproduction]        NUMERIC (20, 4) NULL,
    [accumulatewaterproduction] NUMERIC (20, 4) NULL,
    [monthlywaterproduction]    NUMERIC (20, 4) NULL,
    [dailywaterproduction]      NUMERIC (20, 4) NULL,
    [accumulateoilproduction]   NUMERIC (20, 4) NULL,
    [dailyoilproduction]        NUMERIC (20, 4) NULL,
    [monthlyoilproduction]      NUMERIC (20, 4) NULL,
    [bsw]                       NUMERIC (8, 5)  NULL,
    [apigrades]                 NUMERIC (8, 5)  NULL,
    [rgp]                       NUMERIC (20, 5) NULL,
    [formid]                    UNIQUEIDENTIFIER    NULL,
    [formation]                 VARCHAR (200)   NULL,
    [row_created_by] VARCHAR(60) NULL, 
    [row_created_date] datetime NULL, 
    [row_changed_by] VARCHAR(60) NULL, 
    [row_changed_date] datetime NULL
    CONSTRAINT [FORM9TOTALVOLUMEDETAIL_FORM9_FK1] FOREIGN KEY ([formid]) REFERENCES [FOXT].[FORM9] ([form9id]) ON DELETE CASCADE,
    CONSTRAINT [FORM9TOTALVOLUMEDETAIL_UK] UNIQUE NONCLUSTERED ([form9tvdetailid] ASC)
);

