CREATE TABLE FOXT.FORM20PRODUCTIONDETAIL
(
  form20productiondetailid UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM_form20productiondetailid DEFAULT NEWID(),
  oilwell                  VARCHAR(20),
  danecode                 VARCHAR(6),
  productionmethod         VARCHAR(5),
  monthdays                NUMERIC(20,4),
  accumulatedays           NUMERIC(20,4),
  dailyoilproduction       NUMERIC(20,4),
  monthlyoilproduction     NUMERIC(20,4),
  accumulateoilproduction  NUMERIC(20,4),
  correctionfactor         NUMERIC(8,5),
  dailywater               NUMERIC(20,4),
  monthlywater             NUMERIC(20,4),
  accumulatedwater         NUMERIC(20,4),
  pressure                 NUMERIC(12,2),
  oilwellfinalstate        VARCHAR(50),
  [row_created_by]          varchar(60) NULL, 
  [row_created_date]        datetime NULL, 
  [row_changed_by]          varchar(60) NULL, 
  [row_changed_date]        datetime NULL,
  formid                   UNIQUEIDENTIFIER NOT NULL
)
