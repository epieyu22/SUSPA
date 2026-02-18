CREATE TABLE CLIENTES (
    Id_Cliente int NOT NULL PRIMARY KEY IDENTITY,
    Nombre varchar(255) NOT NULL,
    Id_Sec int NOT NULL    
);

CREATE TABLE THS_AREA (
    Id_Area int NOT NULL PRIMARY KEY IDENTITY,
    Nombre varchar(255) NOT NULL,
);


CREATE TABLE THS_AREA_CONCEPTOS (
    Id_Concepto int NOT NULL PRIMARY KEY IDENTITY,
    Id_Area int NOT NULL,
    Nombre varchar(255) NOT NULL    
);

CREATE TABLE THS_HORAS (
	Id_Reporte int NOT NULL PRIMARY KEY IDENTITY,
    Id_Empleado int NOT NULL,
    Id_Cliente int NOT NULL,
    Fecha varchar(8) NOT NULL,    
	Cant_Horas int NOT NULL,
	Id_Area int NOT NULL,
	Id_Concepto int NOT NULL,
	Descripcion varchar(255) NOT NULL,
	Req int NULL
);

CREATE TABLE THS_PARAMETRO_GENERAL (
	Id_Parametro int NOT NULL PRIMARY KEY IDENTITY,
    Id_Empleado int NOT NULL,
    Descripcion varchar NOT NULL,
	Parametro int NOT NULL
);
--Laves foraneas
ALTER TABLE THS_HORAS 
ADD FOREIGN KEY (Id_Cliente)
REFERENCES CLIENTES(Id_Cliente)

ALTER TABLE THS_HORAS 
ADD FOREIGN KEY (Id_Area)
REFERENCES THS_AREA(Id_Area)


ALTER TABLE THS_HORAS 
ADD FOREIGN KEY (Id_Concepto)
REFERENCES THS_AREA_CONCEPTOS(Id_Concepto)


ALTER TABLE THS_AREA 
ADD FOREIGN KEY (Id_Area)
REFERENCES THS_AREA(Id_Area)
