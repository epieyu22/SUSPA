IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Descripcion') 
BEGIN
    ALTER TABLE SOLICITUDES
    ADD Descripcion VARCHAR(250)	
END;


