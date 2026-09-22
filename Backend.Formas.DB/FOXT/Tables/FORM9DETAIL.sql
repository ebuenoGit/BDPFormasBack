CREATE TABLE [FOXT].[FORM9DETAIL] (
    [form9detailid]             UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM9DETAIL_form9detailid DEFAULT NEWID(),
    [oilwell]                   VARCHAR (60)    NULL,
    [formation]                 VARCHAR (60)    NULL,
    [danecode]                  VARCHAR (20)    NULL,
    [productionmethod]          VARCHAR (5)     NULL,
    [monthdays]                 NUMERIC (20, 4) NULL,
    [accumulatedays]            NUMERIC (20, 4) NULL,
    [dailyoilproduction]        NUMERIC (20, 4) NULL,
    [monthlyoilproduction]      NUMERIC (20, 4) NULL,
    [accumulateoilproduction]   NUMERIC (20, 4) NULL,
    [correctionfactor]          NUMERIC (20, 5) NULL,
    [dailywaterproduction]      NUMERIC (20, 4) NULL,
    [monthlywaterproduction]    NUMERIC (20, 4) NULL,
    [accumulatewaterproduction] NUMERIC (20, 4) NULL,
    [dailygasproduction]        NUMERIC (20, 4) NULL,
    [montlhygasproduction]      NUMERIC (20, 4) NULL,
    [accumulategasproduction]   NUMERIC (20, 4) NULL,
    [bsw]                       NUMERIC (8, 5)  NULL,
    [apigrades]                 NUMERIC (15, 5) NULL,
    [rgp]                       NUMERIC (20, 5) NULL,
    [oilwellfinalstate]         VARCHAR (60)     NULL,
    [formid]                    UNIQUEIDENTIFIER    NULL,
    [poolname]                  VARCHAR (60)    NULL,
    [pdenid]                    varchar(60),
    [row_created_by] VARCHAR(60) NULL, 
    [row_created_date] datetime NULL, 
    [row_changed_by] VARCHAR(60) NULL, 
    [row_changed_date] datetime NULL

    CONSTRAINT [FORM9DETAIL_FORM9_FK1] FOREIGN KEY ([formid]) REFERENCES [FOXT].[FORM9] ([form9id]) ON DELETE CASCADE,
    CONSTRAINT [FORM9DETAIL_UK] UNIQUE NONCLUSTERED ([form9detailid] ASC)
);

