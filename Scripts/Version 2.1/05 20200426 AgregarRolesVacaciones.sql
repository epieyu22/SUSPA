IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
BEGIN
  SET IDENTITY_INSERT ApplicationGroup  ON   
	INSERT INTO ApplicationGroup (Id,Name)VALUES  (3,'Administrador Empresarial')
	INSERT INTO AspNetRoles VALUES('7237fxd8-as1d-6343-aas1-a42gjsdacr3c','reporte.vacaciones','Permite ver el reporte de vacaciones','ApplicationRole')
  SET IDENTITY_INSERT ApplicationGroup  OFF
END;
