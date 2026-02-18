/
  IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
  BEGIN
      ALTER TABLE dbo.PLANTILLAS_CERTIFICADOS ADD Tipo_plantilla VARCHAR(20) NULL
  END;

  IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
  BEGIN
      	  INSERT INTO PLANTILLAS_CERTIFICADOS VALUES('/Static/uploads/FormatoDeducibles.docx','Formato deducibles - 1',	0, 'O')

  END;

  IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
  BEGIN
   	  INSERT INTO PLANTILLAS_CERTIFICADOS VALUES('/Static/uploads/FormatoDeducibles2.docx','Formato deducibles - 2',	0, 'O')
  END;

SELECT * FROM PLANTILLAS_CERTIFICADOS
UPDATE PLANTILLAS_CERTIFICADOS SET Tipo_plantilla = 'N' WHERE Autonum IN (1,2,3,4,5)

