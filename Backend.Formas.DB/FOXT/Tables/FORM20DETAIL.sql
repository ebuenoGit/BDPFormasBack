CREATE TABLE [FOXT].[FORM20DETAIL]
(
  [form20detailid]     UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM_formdetail20id DEFAULT NEWID(),
  [formid]             UNIQUEIDENTIFIER,
  [oilwell]            VARCHAR(260),
  [pdenId]             VARCHAR(22),
  [danecode]           VARCHAR(20),
  [zone]               VARCHAR(260),
  [widays]             NUMERIC(20,4),
  [wiaccumulateddays]  NUMERIC(20,4),
  [pressure]           NUMERIC(12,2),
  [widailywater]       NUMERIC(20,4),
  [wimonthlywater]     NUMERIC(20,4),
  [wiaccumulatedwater] NUMERIC(20,4),
  [poolname]           VARCHAR(260),
  [oilwellfinalstate]  VARCHAR(12),
  [row_created_by]          varchar(60) NULL, 
  [row_created_date]        datetime NULL, 
  [row_changed_by]          varchar(60) NULL, 
  [row_changed_date]        datetime NULL
);
