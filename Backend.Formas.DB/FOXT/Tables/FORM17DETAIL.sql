CREATE TABLE [FOXT].[FORM17DETAIL]
(
	[form17detail]       UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM117DETAIL_form17detail DEFAULT NEWID(),
    [formid]             UNIQUEIDENTIFIER,
    [oilwell]            varchar(50),
    [formation]          varchar(60),
    [gpdays]             numeric(20,4),
    [gpaccumulated]      numeric(20,4),
    [gpmonthlywater]     numeric(20,4),
    [gpaccumulatedwater] numeric(20,4),
    [gpdailygas]         numeric(20,4),
    [gpmonthlygas]       numeric(20,4),
    [gpaccumulatedgas]   numeric(20,4),
    [oilwellfinalstate]  varchar(5),
    [poolname]           varchar(60),
    [gpdailywater]       numeric(20,4)
)
