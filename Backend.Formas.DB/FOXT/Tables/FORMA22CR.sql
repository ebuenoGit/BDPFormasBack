CREATE TABLE FOXT.FORMA22CR
(
		 form_id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DK_FORMA22CR_form_id DEFAULT NEWID() CONSTRAINT PK_FORMF22CR PRIMARY KEY
	    , compania_id  VARCHAR(50)
        , compania VARCHAR(50)
        , formacion  VARCHAR(50)
        , contrato_id VARCHAR(50)
        , contrato VARCHAR(50)
        , bloque VARCHAR(50)
        , campo_id VARCHAR(50)
        , campo VARCHAR(50)
        , yacimiento VARCHAR(50)
        , estructura VARCHAR(50)
        , mes VARCHAR(50)
        , anio VARCHAR(50)
        , pden_id VARCHAR(50)
		, FECHA_CREACION DATETIME
		, USUARIO_CREACION VARCHAR(50), 
    [yacimiento_id] VARCHAR(50) NULL
)