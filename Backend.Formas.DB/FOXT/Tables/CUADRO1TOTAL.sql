CREATE TABLE FOXT.CUADRO1TOTAL
(
	id_cabecera UNIQUEIDENTIFIER NOT NULL,
	Forma_id UNIQUEIDENTIFIER NOT NULL,
	BLS60F decimal(20,2) null,
	BLSNetos decimal(20,2) null,
	recibidoBLS decimal(20,2) null,
	entregaBLS decimal(20,2) null,
	fecha_creacion datetime,
	usuario_crea varchar(100),
	[row_created_by]          varchar(60) NULL, 
	[row_created_date]        datetime NULL,
	[row_changed_by]          varchar(60) NULL, 
	[row_changed_date]        datetime NULL
)
