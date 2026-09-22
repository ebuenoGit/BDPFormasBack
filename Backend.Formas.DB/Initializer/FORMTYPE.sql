IF (SELECT COUNT(*) FROM FOXT.FORMTYPE) <= 0 BEGIN
    INSERT INTO FOXT.FORMTYPE ([name])
    VALUES
    ('Cuadro 4'),
    ('Cuadro 7'),
    ('Forma 9'),
    ('Forma 15'),
    ('Forma 16'),
    ('Forma 17'),
    ('Forma 20'),
    ('Forma 21'),
    ('Forma 22'),
    ('Forma 25'),
    ('Forma 30'),
    ('Forma 23'),
    ('Cuadro 1');
END
