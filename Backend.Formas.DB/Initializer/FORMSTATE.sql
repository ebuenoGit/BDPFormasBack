IF (SELECT COUNT(*) FROM FOXT.FORMSTATE) <= 0 BEGIN
    INSERT INTO FOXT.FORMSTATE ([name])
    VALUES
    ('Histórico'),
    ('No Generada'),
    ('Generada'),
    ('Validada'),
    ('Descartada'),
    ('Aprobada'),
    ('No Aprobada'),
    ('No Aprobada por  el ministerio'),
    ('Autorizada para Regenerar'),
    ('Autorizada por Generar Extemporaneamente'),
    ('En Proceso de Aprobación'),
    ('Generada Temporalmente');
END