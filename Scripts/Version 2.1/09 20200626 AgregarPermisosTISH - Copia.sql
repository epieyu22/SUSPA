IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
BEGIN
  INSERT INTO AspNetRoles VALUES ('2c26s6a1-9s25-95sc-1afc-a444ssc5dc','timesheet.horas','Acceso al modulo de ingreso de horas time sheet', 'ApplicationRole')
END;
