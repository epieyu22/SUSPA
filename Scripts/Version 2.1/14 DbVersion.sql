
DECLARE @code VARCHAR(100) = 'Version'
DECLARE @value  VARCHAR(2000) = '2.1.0'
DECLARE @description VARCHAR(2000) = 'Version actual de la aplicacion y base de datos'

IF NOT EXISTS(SELECT TOP 1 1 FROM PARAMETROS_GENERALES WHERE LOWER(Cod_Parametro) = LOWER(@code))
BEGIN
	INSERT INTO PARAMETROS_GENERALES (Cod_Parametro, Valor, Descripcion)
	VALUES (@code, @value, @description)
END
ELSE
BEGIN
  UPDATE PARAMETROS_GENERALES
    SET Valor = @value, Descripcion = @description
	WHERE Cod_Parametro = @code
END
GO

