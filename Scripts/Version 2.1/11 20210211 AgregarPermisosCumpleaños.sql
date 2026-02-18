IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
BEGIN

  INSERT INTO AspNetRoles VALUES ('1habch12-1icv-094d-SNCD-Ahchbnsnm2','consultas.cumpleaños','Acceso al modulo de cumpleaños', 'ApplicationRole')

END;
