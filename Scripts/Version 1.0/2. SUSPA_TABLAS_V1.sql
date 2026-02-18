/* ==========================================================
   MODELO "Inicial" - V1
   ========================================================== */

SET NOCOUNT ON;
GO

/* =========================
   DROP (en orden por FKs)
   ========================= */

IF OBJECT_ID('dbo.CUPOS', 'U') IS NOT NULL DROP TABLE dbo.CUPOS;
IF OBJECT_ID('dbo.PERIODOS', 'U') IS NOT NULL DROP TABLE dbo.PERIODOS;
IF OBJECT_ID('dbo.SEDES', 'U') IS NOT NULL DROP TABLE dbo.SEDES;
IF OBJECT_ID('dbo.INSTITUCIONES', 'U') IS NOT NULL DROP TABLE dbo.INSTITUCIONES;
IF OBJECT_ID('dbo.CONTRATOS', 'U') IS NOT NULL DROP TABLE dbo.CONTRATOS;
--IF OBJECT_ID('dbo.EMPRESAS', 'U') IS NOT NULL DROP TABLE dbo.EMPRESAS;
IF OBJECT_ID('dbo.ETC', 'U') IS NOT NULL DROP TABLE dbo.ETC;
IF OBJECT_ID('dbo.LUGAR_ATENCION', 'U') IS NOT NULL DROP TABLE dbo.LUGAR_ATENCION;
IF OBJECT_ID('dbo.USUARIOS', 'U') IS NOT NULL DROP TABLE dbo.USUARIOS;
IF OBJECT_ID('dbo.DATABASES', 'U') IS NOT NULL DROP TABLE dbo.DATABASES;
GO

/* =========================
   MAESTRAS / BASE
   ========================= */
-- ETC
CREATE TABLE dbo.ETC (
    idEtc       INT IDENTITY(1,1) NOT NULL,
    entidad      NVARCHAR(20)      NOT NULL,
    nombre      NVARCHAR(40)      NOT NULL,
    CONSTRAINT PK_ETCS PRIMARY KEY CLUSTERED (idEtc),
    CONSTRAINT UX_ETCS_nombre UNIQUE (nombre)
);
GO

/* ==========================================================
   TABLA EMPRESAS legacy -> agregar idEmpresa y constraints
   ========================================================== */

-- 1.1 Agregar idEmpresa si no existe
IF COL_LENGTH('dbo.EMPRESAS', 'idEmpresa') IS NULL
BEGIN
    ALTER TABLE dbo.EMPRESAS
    ADD idEmpresa INT IDENTITY(1,1) NOT NULL;
END
GO

-- 1.2 Asegurar UNIQUE sobre Codigo (para mantener integridad legacy)
IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.EMPRESAS')
      AND name = 'UX_EMPRESAS_Codigo'
)
BEGIN
    CREATE UNIQUE INDEX UX_EMPRESAS_Codigo
    ON dbo.EMPRESAS (Codigo);
END
GO

-- 1.3 Crear PK sobre idEmpresa si NO existe PK actualmente
IF NOT EXISTS (
    SELECT 1
    FROM sys.key_constraints kc
    WHERE kc.[type] = 'PK'
      AND kc.parent_object_id = OBJECT_ID('dbo.EMPRESAS')
)
BEGIN
    ALTER TABLE dbo.EMPRESAS
    ADD CONSTRAINT PK_EMPRESAS PRIMARY KEY CLUSTERED (idEmpresa);
END
GO

-- CONTRATOS
CREATE TABLE dbo.CONTRATOS (
    idContrato    INT IDENTITY(1,1) NOT NULL,
    numContrato   NVARCHAR(100)      NOT NULL,
    anioContrato  INT               NULL,
    idEmpresa     INT               NOT NULL,
    idEtc         INT               NOT NULL,

    CONSTRAINT PK_CONTRATOS PRIMARY KEY CLUSTERED (idContrato),
    CONSTRAINT FK_CONTRATOS_EMPRESAS FOREIGN KEY (idEmpresa)
        REFERENCES dbo.EMPRESAS (idEmpresa),
    CONSTRAINT FK_CONTRATOS_ETCS FOREIGN KEY (idEtc)
        REFERENCES dbo.ETC (idEtc),
    CONSTRAINT CK_CONTRATOS_anio CHECK (anioContrato IS NULL OR (anioContrato BETWEEN 1900 AND 2100))
);
GO
CREATE INDEX IX_CONTRATOS_idEmpresa ON dbo.CONTRATOS (idEmpresa);
CREATE INDEX IX_CONTRATOS_idEtc     ON dbo.CONTRATOS (idEtc);
GO
---------------------------------------------------- OPERACION
-- INSTITUCIONES
CREATE TABLE dbo.INSTITUCIONES (
    idInstitucion     INT IDENTITY(1,1) NOT NULL,
    daneInstitucion   NVARCHAR(50)       NULL,
    nombreCompleto    NVARCHAR(400)      NOT NULL,
    direccion         NVARCHAR(300)      NULL,
    nombreCorto       NVARCHAR(200)      NULL,
    nombreRector      NVARCHAR(200)      NULL,
    cedulaRector      NVARCHAR(30)       NULL,
    activo            BIT               NOT NULL CONSTRAINT DF_INSTITUCIONES_activo DEFAULT (1)

    CONSTRAINT PK_INSTITUCIONES PRIMARY KEY CLUSTERED (idInstitucion)
);
GO

-- SEDES
CREATE TABLE dbo.SEDES (
    idSede        INT IDENTITY(1,1) NOT NULL,
    nombreSede    NVARCHAR(250)      NOT NULL,
    codDaneSede   NVARCHAR(50)       NULL,
    daneAnterior  NVARCHAR(50)       NULL,
    idTipo        INT               NULL,  -- se deja como en tu modelo (sin normalizar)
    idContrato    INT               NOT NULL,
    idInstitucion INT               NOT NULL,
    estado        NVARCHAR(50)       NULL,

    CONSTRAINT PK_SEDES PRIMARY KEY CLUSTERED (idSede),
    CONSTRAINT FK_SEDES_CONTRATOS FOREIGN KEY (idContrato)
        REFERENCES dbo.CONTRATOS (idContrato),
    CONSTRAINT FK_SEDES_INSTITUCIONES FOREIGN KEY (idInstitucion)
        REFERENCES dbo.INSTITUCIONES (idInstitucion)
);
GO
CREATE INDEX IX_SEDES_idContrato    ON dbo.SEDES (idContrato);
CREATE INDEX IX_SEDES_idInstitucion ON dbo.SEDES (idInstitucion);
GO


/* =========================
   DATABASES (ingestión / cargas)
   ========================= */

CREATE TABLE dbo.DATABASES (
    idDatabases        INT IDENTITY(1,1) NOT NULL,

    -- Archivo / almacenamiento
    rutaAlmacenamiento NVARCHAR(500)      NULL,
    nomArchivo         NVARCHAR(260)      NULL,
    tamanoBytes        BIGINT             NULL,
    hashArchivo        VARBINARY(32)      NULL, -- por ejemplo SHA-256 (32 bytes)
    mimeType           NVARCHAR(100)      NULL,

    -- Fechas y metadatos
    fechaCorte         DATE              NULL,
    fechaCargue        DATETIME2(0)       NOT NULL CONSTRAINT DF_DATABASES_fechaCargue DEFAULT (SYSUTCDATETIME()),
    usuarioCargue      NVARCHAR(100)      NULL,

    -- Identificación lógica del dataset
    nombre             NVARCHAR(200)      NULL,
    descripcion        NVARCHAR(1000)     NULL,

    -- Control de carga
    estadoCarga        NVARCHAR(30)       NOT NULL CONSTRAINT DF_DATABASES_estadoCarga DEFAULT ('RECEIVED'),
    filasLeidas        BIGINT             NULL,
    filasInsertadas    BIGINT             NULL,
    filasRechazadas    BIGINT             NULL,
    detalleError       NVARCHAR(2000)     NULL,

    -- Auditoría / trazabilidad (si luego agregas tabla AUDITORIAS, conviertes a FK)
    idAuditoria        INT               NULL,

    CONSTRAINT PK_DATABASES PRIMARY KEY CLUSTERED (idDatabases),
    CONSTRAINT CK_DATABASES_estadoCarga CHECK (estadoCarga IN ('RECEIVED','VALIDATING','PROCESSING','COMPLETED','FAILED')),
    CONSTRAINT CK_DATABASES_tamano CHECK (tamanoBytes IS NULL OR tamanoBytes >= 0)
);
GO

-- Índices típicos de consulta
CREATE INDEX IX_DATABASES_fechaCargue   ON dbo.DATABASES (fechaCargue DESC);
CREATE INDEX IX_DATABASES_estadoCarga   ON dbo.DATABASES (estadoCarga);
CREATE INDEX IX_DATABASES_fechaCorte    ON dbo.DATABASES (fechaCorte);
GO

-- PERIODOS
CREATE TABLE dbo.PERIODOS (
    idPeriodo        INT IDENTITY(1,1) NOT NULL,
    periodoYYYYMM    INT               NOT NULL,  -- 202502
    periodoNombre    NVARCHAR(50)       NOT NULL,  -- '2025-02', 'FEB-2025', etc.

    idDatabases      INT               NOT NULL,
    idContrato       INT               NOT NULL,

    CONSTRAINT PK_PERIODOS PRIMARY KEY CLUSTERED (idPeriodo),
    CONSTRAINT FK_PERIODOS_DATABASES FOREIGN KEY (idDatabases)
        REFERENCES dbo.DATABASES (idDatabases),
    CONSTRAINT FK_PERIODOS_CONTRATOS FOREIGN KEY (idContrato)
        REFERENCES dbo.CONTRATOS (idContrato),

    -- Valida YYYYMM: año 1900-2100 y mes 01-12
    CONSTRAINT CK_PERIODOS_yyyymm CHECK (
        periodoYYYYMM BETWEEN 190001 AND 210012
        AND (periodoYYYYMM % 100) BETWEEN 1 AND 12
    )
);
GO

-- Evita duplicar el mismo periodo para el mismo contrato (regla típica)
CREATE UNIQUE INDEX UX_PERIODOS_contrato_periodo
ON dbo.PERIODOS (idContrato, periodoYYYYMM);

-- Consultas comunes
CREATE INDEX IX_PERIODOS_idDatabases ON dbo.PERIODOS (idDatabases);
CREATE INDEX IX_PERIODOS_periodoYYYYMM ON dbo.PERIODOS (periodoYYYYMM DESC);
GO


-- USUARIOS
CREATE TABLE dbo.USUARIOS (
    idUsuario         INT IDENTITY(1,1) NOT NULL,
    personalId       NVARCHAR(50)       NULL,
    numeroDocumento  NVARCHAR(30)       NULL,
    tipoDocumento    NVARCHAR(30)       NULL,
    apellido1        NVARCHAR(100)      NULL,
    apellido2        NVARCHAR(100)      NULL,
    nombre1          NVARCHAR(100)      NULL,
    nombre2          NVARCHAR(100)      NULL,
    genero           NVARCHAR(20)       NULL,
    fechaNacimiento  DATE               NULL,
    etnia            NVARCHAR(50)       NULL,
    grado            NVARCHAR(50)       NULL,

    CONSTRAINT PK_USUARIOS PRIMARY KEY CLUSTERED (idUsuario)
);
GO
-- Documento+Tipo no debe repetirse:
CREATE UNIQUE INDEX UX_USUARIOS_doc ON dbo.USUARIOS (tipoDocumento, numeroDocumento);
GO

CREATE TABLE dbo.LUGAR_ATENCION (
    idLugarAtencion    INT IDENTITY(1,1) NOT NULL,
    nomLugarAtencion        NVARCHAR(200)      NULL,
    subgrupo        NVARCHAR(200)      NULL,
    grupo           NVARCHAR(200)      NULL,
    tipoAtencion    NVARCHAR(100)      NULL,
    idJornada       INT               NULL,
    idRuta          INT               NULL,
    identificador   NVARCHAR(100)      NULL,
    idMinuta        INT               NULL,

    CONSTRAINT PK_AGRUPACIONES PRIMARY KEY CLUSTERED (idLugarAtencion)
);
GO

-- CUPOS
CREATE TABLE dbo.CUPOS (
    idCupos        INT IDENTITY(1,1) NOT NULL,
    idUsuario       INT               NOT NULL,
    idDatabases    INT               NOT NULL,
    idSede         INT               NOT NULL,
    idLugarAtencion   INT               NOT NULL,

    CONSTRAINT PK_CUPOS PRIMARY KEY CLUSTERED (idCupos),

    CONSTRAINT FK_CUPOS_USUARIOS FOREIGN KEY (idUsuario)
        REFERENCES dbo.USUARIOS (idUsuario),

    CONSTRAINT FK_CUPOS_DATABASES FOREIGN KEY (idDatabases)
        REFERENCES dbo.DATABASES (idDatabases),

    CONSTRAINT FK_CUPOS_SEDES FOREIGN KEY (idSede)
        REFERENCES dbo.SEDES (idSede),

    CONSTRAINT FK_CUPOS_LUGAR_ATENCION FOREIGN KEY (idLugarAtencion)
        REFERENCES dbo.LUGAR_ATENCION (idLugarAtencion)
);
GO

CREATE INDEX IX_CUPOS_idAlumno     ON dbo.CUPOS (idUsuario);
CREATE INDEX IX_CUPOS_idDatabases  ON dbo.CUPOS (idDatabases);
CREATE INDEX IX_CUPOS_idSede       ON dbo.CUPOS (idSede);
CREATE INDEX IX_CUPOS_idAgrupacion ON dbo.CUPOS (idLugarAtencion);

-- Recomendado para evitar duplicidad lógica del mismo cupo en el mismo dataset:
CREATE UNIQUE INDEX UX_CUPOS_noDuplicados
ON dbo.CUPOS (idUsuario, idDatabases, idSede, idLugarAtencion);
GO
