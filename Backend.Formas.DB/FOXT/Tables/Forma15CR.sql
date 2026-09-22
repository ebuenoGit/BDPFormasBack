CREATE TABLE [FOXT].[Forma15CR]
(
	 form_id UniqueIdentifier not null,
	id_detalle UniqueIdentifier not null,
    compania_id            varchar(50),
    compania        varchar(50),
    concesion             varchar(50),
    contrato_id     varchar(50),
    contrato     varchar(50),
    campo_id varchar(50),
    campo         varchar(50),
    mes        varchar(50),
    anio    varchar(50),
	[row_created_by]          varchar(60) NULL, 
    [row_created_date]        datetime NULL, 
    [row_changed_by]          varchar(60) NULL, 
    [row_changed_date]        datetime NULL
)