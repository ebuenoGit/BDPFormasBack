CREATE TABLE  FOXT.FORM20TOTALVOLUMEDETAIL
(
  form20tvdetailid         UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM_form20totaldetailid DEFAULT NEWID(),
  volumetype               NUMERIC(1),
  accumulatewaterinjection NUMERIC(20,4),
  monthlywaterinjection    NUMERIC(20,4),
  dailywaterinjection      NUMERIC(20,4),
  formid                   UNIQUEIDENTIFIER,
  formation                VARCHAR(60),
  [row_created_by]          varchar(60) NULL, 
  [row_created_date]        datetime NULL, 
  [row_changed_by]          varchar(60) NULL, 
  [row_changed_date]        datetime NULL
)
