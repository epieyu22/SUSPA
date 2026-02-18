IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Tipo_plantilla') 
BEGIN

INSERT INTO AspNetRoles values ('abd25157-ab95-4000-ascd-28d3ff910d49', 'nom_electronica', 'Acceso al modulo de nomina electronica', 'ApplicationRole' )
END;
GO
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE LOWER(TABLE_NAME) = LOWER('MAPEO_NOM_ELEC'))
BEGIN

  CREATE TABLE TIPO_MAPEO(
    Cod_Tipo INT PRIMARY KEY IDENTITY,
    Nombre VARCHAR(50)
  )

--TABLAS NOMINA ELECTRONICA
  CREATE TABLE MAPEO_NOM_ELEC(
    id INT PRIMARY KEY IDENTITY,
    Cod_Concepto int,
    Cod_Pt int,
    Dian_xml varchar(max),
    Cod_Alterno varchar(50),
    Cod_Alterno2 varchar(50),
    Descripcion varchar(500),
    Fec_vigencia date,
    Estado char(1),
    Cod_Tipo INT NOT NULL
  )

  EXEC('ALTER TABLE MAPEO_NOM_ELEC ADD CONSTRAINT fk_mapeo_tipo FOREIGN KEY (Cod_Tipo) REFERENCES TIPO_MAPEO(Cod_Tipo)') 
  EXEC('ALTER TABLE MAPEO_NOM_ELEC ADD CONSTRAINT uk_ConceptoMapeo UNIQUE(Cod_Concepto, Cod_Tipo)')

  --PT
  CREATE TABLE PT(
    Cod_Pt int,
    Nom_Pt varchar(50),
    Estado char(2),
  )

  CREATE TABLE HISTORICO_NOM_ELEC(
    Cod_Empleado int,
    Cod_Concepto int,
    Val_Novedad varchar (100),
    Porcentaje varchar(100),
    Fec_Nomina date,
    Fec_Envio date,
    Estado char(2),
    Tipo_Mapeo VARCHAR(50)
  )
END;

