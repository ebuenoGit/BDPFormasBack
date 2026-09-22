CREATE TABLE [FOXT].[FORMA17]
(
	[forma17id]  UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM117_form17 DEFAULT NEWID(),
	[totalgpdays]             numeric(20,4),
    [totalgpaccumulated]      numeric(20,4),
    [totalgpmonthlywater]     numeric(20,4),
    [totalgpaccumulatedwater] numeric(20,4),
    [totalgpdailygas]         numeric(20,4),
    [totalgpmonthlygas]       numeric(20,4),
    [totalgpaccumulatedgas]   numeric(20,4),
    [formation]               varchar(60),
    [block]                   varchar(50),
    [oilfield]                varchar(50),
    [structure]               varchar(50),
    [member]                  varchar(50)
)
