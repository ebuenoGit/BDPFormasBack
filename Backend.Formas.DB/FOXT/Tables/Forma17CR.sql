
CREATE TABLE FOXT.Forma17CR
(
		  form_id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_Forma17CR_form_id DEFAULT NEWID() CONSTRAINT form_id_PK PRIMARY KEY
		, id_detalle UniqueIdentifier not null
	    , concesion VARCHAR(100)
        , operador_id VARCHAR(100)
        , operador VARCHAR(100)
        , contrato_id VARCHAR(100)
        , contrato VARCHAR(100)
        , campo_id  VARCHAR(100)
        , campo VARCHAR(100)
        , estructura VARCHAR(100)
        , formacion VARCHAR(100)
        , bloque VARCHAR(100)
        , yacimiento VARCHAR(100)
        , mes VARCHAR(100)
        , anio VARCHAR(100)
		, fecha_creacion datetime
		, usuario_crea varchar(110)
        ,[row_created_by]          varchar(60) NULL, 
         [row_created_date]        datetime NULL, 
         [row_changed_by]          varchar(60) NULL, 
         [row_changed_date]        datetime NULL
)

