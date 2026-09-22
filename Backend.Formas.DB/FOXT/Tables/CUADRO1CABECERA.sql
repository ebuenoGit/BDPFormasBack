
CREATE TABLE FOXT.CUADRO1CABECERA
(
	id_FormaCuadro1 UNIQUEIDENTIFIER NOT NULL,
	Forma_id UNIQUEIDENTIFIER NOT NULL,
	mes varchar(10), 
	anio varchar(10),
	compania_id int,
	compania varchar(100),
	contrato_id int,
	contrato varchar(100),
	campo_id int,
	campo varchar(100),
	lugar varchar(100),
	tanque  varchar(100),
	bateria varchar(100), 
	fecha_creacion datetime,
	usuario_crea varchar(100),
	id_tanque varchar(50),
	id_bateria varchar(50),
	[row_created_by]          varchar(60) NULL, 
	[row_created_date]        datetime NULL, 
	[row_changed_by]          varchar(60) NULL, 
	[row_changed_date]        datetime NULL
)
