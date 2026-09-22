create table FOXT.FORM21
(
  [form21id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM21_forM21id DEFAULT NEWID() CONSTRAINT FORM21_PK PRIMARY KEY,
  [formation] VARCHAR(260) NULL,
  [block] VARCHAR(260) NULL,
  [oilfield] VARCHAR(260) NULL,
  [structure] VARCHAR(260) NULL,
  [member] VARCHAR(260) NULL,
  [operadorid] VARCHAR(260) NULL,
  [operador] VARCHAR(260) NULL,
  [contractid] VARCHAR(260) NULL,
  [contract] VARCHAR(260) NULL,
  [campoid] VARCHAR(260) NULL,
  [campo] VARCHAR(260) NULL,
  [row_created_by]          varchar(60) NULL, 
  [row_created_date]        datetime NULL, 
  [row_changed_by]          varchar(60) NULL, 
  [row_changed_date]        datetime NULL
)