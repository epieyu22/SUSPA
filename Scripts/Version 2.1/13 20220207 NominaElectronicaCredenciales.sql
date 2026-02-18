IF NOT EXISTS(SELECT TOP 1 1 FROM INFORMATION_SCHEMA.TABLES WHERE LOWER(TABLE_NAME) = LOWER('parametros_generales'))
BEGIN
	CREATE TABLE PARAMETROS_GENERALES(
		Id INT PRIMARY KEY IDENTITY,
		Cod_Parametro VARCHAR(100),
		Valor VARCHAR(2000),
		Descripcion VARCHAR(2000)
	)

	ALTER TABLE PARAMETROS_GENERALES ADD CONSTRAINT uk_param_generales UNIQUE(Cod_Parametro)

END


DECLARE @code VARCHAR(100) = 'DataicoElectronicPayrollEndpoint'
DECLARE @value  VARCHAR(2000) = 'https://api.dataico.com/direct/payroll-api'

IF NOT EXISTS(SELECT TOP 1 1 FROM PARAMETROS_GENERALES WHERE LOWER(Cod_Parametro) = LOWER(@code))
BEGIN
	INSERT INTO PARAMETROS_GENERALES (Cod_Parametro, Valor)
	VALUES (@code, @value)
END
GO

DECLARE @code VARCHAR(100) = 'DataicoElectronicPayrollAccountId'
DECLARE @value  VARCHAR(2000) = '############'

IF NOT EXISTS(SELECT TOP 1 1 FROM PARAMETROS_GENERALES WHERE LOWER(Cod_Parametro) = LOWER(@code))
BEGIN
	INSERT INTO PARAMETROS_GENERALES (Cod_Parametro, Valor)
	VALUES (@code, @value)
END
GO


DECLARE @code VARCHAR(100) = 'DataicoElectronicPayrollAuthToken'
DECLARE @value  VARCHAR(2000) = '############'

IF NOT EXISTS(SELECT TOP 1 1 FROM PARAMETROS_GENERALES WHERE LOWER(Cod_Parametro) = LOWER(@code))
BEGIN
	INSERT INTO PARAMETROS_GENERALES (Cod_Parametro, Valor)
	VALUES (@code, @value)
END
GO


DECLARE @code VARCHAR(100) = 'DataicoElectronicPayrollDianTestId'
DECLARE @value  VARCHAR(2000) = '############'

IF NOT EXISTS(SELECT TOP 1 1 FROM PARAMETROS_GENERALES WHERE LOWER(Cod_Parametro) = LOWER(@code))
BEGIN
	INSERT INTO PARAMETROS_GENERALES (Cod_Parametro, Valor)
	VALUES (@code, @value)
END
GO

DECLARE @code VARCHAR(100) = 'DataicoElectronicPayrollDianId'
DECLARE @value  VARCHAR(2000) = '############'

IF NOT EXISTS(SELECT TOP 1 1 FROM PARAMETROS_GENERALES WHERE LOWER(Cod_Parametro) = LOWER(@code))
BEGIN
	INSERT INTO PARAMETROS_GENERALES (Cod_Parametro, Valor)
	VALUES (@code, @value)
END
GO


DECLARE @code VARCHAR(100) = 'DataicoElectronicPrefixes'
DECLARE @prefijoSoporteNomina VARCHAR(20) = 'N'
DECLARE @prefijoNotaAjusteRemplazo VARCHAR(20) = 'NR'
DECLARE @prefijoNotaAjusteEliminacion VARCHAR(20) = 'NE'

DECLARE @value  VARCHAR(2000) = '{
	  "soporteNomina": {
		  "prefijo": "' + @prefijoSoporteNomina + '",
		   "numeroActual": null
	  },
	  "notaAjusteRemplazo": {
		  "prefijo": "' + @prefijoNotaAjusteRemplazo + '",
		  "numeroActual": null
	  },
	  "notaAjusteEliminacion": {
		  "prefijo": "' + @prefijoNotaAjusteEliminacion + '",
		  "numeroActual": null
	  }
}'

IF NOT EXISTS(SELECT TOP 1 1 FROM PARAMETROS_GENERALES WHERE LOWER(Cod_Parametro) = LOWER(@code))
BEGIN
	INSERT INTO PARAMETROS_GENERALES (Cod_Parametro, Valor)
	VALUES (@code, @value)
END
GO

/*Para actualizar parametro de prefijos JSON
DECLARE @code VARCHAR(100) = 'DataicoElectronicPrefixes'
DECLARE @prefijoSoporteNomina VARCHAR(20) = '##'
DECLARE @prefijoNotaAjusteRemplazo VARCHAR(20) = '##'
DECLARE @prefijoNotaAjusteEliminacion VARCHAR(20) = '##'

DECLARE @value  VARCHAR(2000) = '{
	  "soporteNomina": {
		  "prefijo": "' + @prefijoSoporteNomina + '",
		   "numeroActual": null
	  },
	  "notaAjusteRemplazo": {
		  "prefijo": "' + @prefijoNotaAjusteRemplazo + '",
		  "numeroActual": null
	  },
	  "notaAjusteEliminacion": {
		  "prefijo": "' + @prefijoNotaAjusteEliminacion + '",
		  "numeroActual": null
	  }
}'
UPDATE PARAMETROS_GENERALES 
SET Valor = @value
WHERE LOWER(Cod_Parametro) = LOWER(@code)
*/


