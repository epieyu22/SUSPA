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
    Cod_Concepto VARCHAR(50),
    Cod_Pt int,
    Dian_xml varchar(max),
    Cod_Alterno varchar(50),
    Cod_Alterno2 varchar(50),
    Descripcion varchar(50),
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
    --Estatus del envío a la dian

    CREATE TABLE STATUS_NOM_ELEC(
    id INT PRIMARY KEY IDENTITY,
    Cod_Concepto INT,
    Xml VARCHAR (MAX),
    Pdf VARCHAR(MAX),
    Cune VARCHAR(MAX),
    Number VARCHAR(20),
    Qrcode VARCHAR(MAX),
	  DianStatus VARCHAR(50),
	  EmailStatus VARCHAR(30),
	  Fec_Nomina VARCHAR(10),
    Consecutivo VARCHAR(10),
    Prefix VARCHAR(10)
  )
END;
ALTER TABLE STATUS_NOM_ELEC
ALTER COLUMN Consecutivo int NOT NULL;

ALTER TABLE MAPEO_NOM_ELEC
ALTER COLUMN Descripcion VARCHAR(300) ;

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Cod_Tipo') 
BEGIN
    SET IDENTITY_INSERT TIPO_MAPEO ON
	    INSERT INTO TIPO_MAPEO (Cod_Tipo, Nombre)VALUES ('1','CONCEPTO')
	    INSERT INTO TIPO_MAPEO (Cod_Tipo, Nombre)VALUES ('2','TIPO_CONTRATO')
	    INSERT INTO TIPO_MAPEO (Cod_Tipo, Nombre)VALUES ('3','TIPO_DOCUMENTO')
	    INSERT INTO TIPO_MAPEO (Cod_Tipo, Nombre)VALUES ('4','TIPO_PAGO')
      INSERT INTO TIPO_MAPEO (Cod_Tipo, Nombre)VALUES ('5','TIPO_TRABAJADOR')

    SET IDENTITY_INSERT TIPO_MAPEO OFF
END;

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Cod_Concepto') 
BEGIN
    INSERT INTO MAPEO_NOM_ELEC VALUES ('U', '1','NIE000','NUIP', 'Mapeo documentos', '','2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('I', '1','NIE000','PEP', 'Mapeo documentos','', '2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('E', '1','NIE000','TARJETA_DE_EXTRANJERIA', '','Mapeo documentos', '2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('T', '1','NIE000','TARJETA_DE_IDENTIDAD', '','Mapeo documentos', '2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('P', '1','NIE000','PASAPORTE', 'Mapeo documentos','', '2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('C', '1','NIE000','CEDULA_DE_CIUDADANIA', '','Mapeo documentos', '2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('N', '1','NIE000','NIT', 'Mapeo documentos', '','2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('R', '1','NIE000','REGISTRO_CIVIL', 'Mapeo documentos','', '2021-12-30','A','3')
    INSERT INTO MAPEO_NOM_ELEC VALUES ('M', '1','NIE000','PEP', 'Mapeo documentos','', '2021-12-30','A','3')

END;
GO

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Cod_Concepto') 
BEGIN

    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('1', '1','NIE000','CHEQUE', '','Mapeo tipo de pago', '2021-12-30','A','4')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('2', '1','NIE000','TRANSFERENCIA_DEBITO_BANCARIA','','Mapeo tipo de pago', '2021-12-30','A','4')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('3', '1','NIE000','EFECTIVO','','Mapeo tipo de pago', '2021-12-30','A','4')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('4', '1','NIE000','TRANSFERENCIA_DEBITO','','Mapeo tipo de pago', '2021-12-30','A','4')

END;

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Cod_Concepto') 
BEGIN

    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('3', '1','NIE000','APRENDIZAJE', '','Mapeo tipo contrato', '2021-12-30','A','2')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('8', '1','NIE000','OBRA_LABOR', '','Mapeo tipo contrato', '2021-12-30','A','2')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('2', '1','NIE000','TERMINO_FIJO', '','Mapeo tipo contrato', '2021-12-30','A','2')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('1', '1','NIE000','TERMINO_INDEFINIDO','','Mapeo tipo contrato', '2021-12-30','A','2')   
   INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('7', '1','NIE000','TERMINO_INDEFINIDO','','Mapeo tipo contrato', '2021-12-30','A','2')   
END;

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'Cod_Concepto') 
BEGIN

    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('1', '1','NIE000','DEPENDIENTE', '','Mapeo tipo contrato', '2021-12-30','A','5')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('2', '1','NIE000','DEPENDIENTE', '','Mapeo tipo contrato', '2021-12-30','A','5')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('8', '1','NIE000','DEPENDIENTE', '','Mapeo tipo contrato', '2021-12-30','A','5')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('4', '1','NIE000','PRACTICAS_PASANTIAS', '','Mapeo tipo contrato', '2021-12-30','A','5')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('3', '1','NIE000','APRENDICES_DEL_SENA_EN_ETAPA_PRODUCTIVA', '','Mapeo tipo contrato', '2021-12-30','A','5')
    INSERT INTO MAPEO_NOM_ELEC (Cod_Concepto,Cod_Pt,Dian_xml,Cod_Alterno,Cod_Alterno2,Descripcion,Fec_vigencia,Estado,Cod_Tipo)
    VALUES ('7', '1','NIE000','PRE_PENSIONADO_CON_APORTE_VOLUNTARIO_A_SALUD', '','Mapeo tipo contrato', '2021-12-30','A','5')
END;

IF NOT EXISTS(SELECT 1 FROM sys.objects WHERE name = 'PARAMETROS_NOMINA_ELECTRONICA') 
BEGIN
CREATE TABLE PARAMETROS_NOMINA_ELECTRONICA(
    Id INT PRIMARY KEY IDENTITY (1, 1),
    Empresa VARCHAR(50),
	  AccountId VARCHAR(100),
	  AuthToken VARCHAR(100),
	  DianTestId VARCHAR(100),
    DianId VARCHAR(100),
    Prefix VARCHAR(10)
);END;
