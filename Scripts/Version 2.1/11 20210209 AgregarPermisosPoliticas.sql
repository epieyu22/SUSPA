IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
BEGIN

  INSERT INTO AspNetRoles VALUES ('1habch12-1icv-094d-SNCD-Ahchbnsnm2','descargar.politicas','Acceso al modulo de politicas', 'ApplicationRole')

END;
