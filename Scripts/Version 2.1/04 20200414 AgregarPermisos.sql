IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
BEGIN

  INSERT INTO AspNetRoles VALUES ('93375728-459d-44c2-a0ab-a4gjsd4c5dc','consultas.incapacidades','Acceso al modulo de incapacidades', 'ApplicationRole')
  INSERT INTO AspNetRoles VALUES ('7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc','consultas.licencias','Acceso al modulo de incapacidades', 'ApplicationRole')
  INSERT INTO AspNetRoles VALUES ('30007571-999d-4eau-a12b-3c4e9421fbd','certificados.otroscert','Acceso al modulo de otros certificados', 'ApplicationRole')

END;
