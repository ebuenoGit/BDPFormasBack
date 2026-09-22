CREATE TABLE [FOXT].[FORM23]
(
	  [form23id]   UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORM23_form20id DEFAULT NEWID(),
	  [formation]  varchar(260),
	  [block]      VARCHAR(260),
	  [oilfield]   VARCHAR(260),
	  [structure]  VARCHAR(260),
	  [member]     VARCHAR(260),
	  [operadorid] VARCHAR(260),
	  [operador]  VARCHAR(260),
	  [contractid] VARCHAR(260),
	  [contract]   VARCHAR(260),
	  [campoid] VARCHAR(260),
	  [campo] VARCHAR(260),
	  [row_created_by]          varchar(60) NULL, 
	  [row_created_date]        datetime NULL, 
      [row_changed_by]          varchar(60) NULL, 
	  [row_changed_date]        datetime NULL
)
