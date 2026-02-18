
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Empleado] [nvarchar](max) NULL,
	[DBName] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/

CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/

CREATE TABLE [dbo].[ApplicationGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/



CREATE TABLE [dbo].[AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_IdentityUser_Id]
GO

/

CREATE TABLE [dbo].[ApplicationUserGroups](
	[UserId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationUserGroups] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationGroup_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ApplicationGroup] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationGroup_GroupId]
GO

ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.AspNetUsers_UserId]
GO

/

CREATE TABLE [dbo].[ApplicationRoleGroups](
	[RoleId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationRoleGroups] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.ApplicationGroup_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ApplicationGroup] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.ApplicationGroup_GroupId]
GO

ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.AspNetRoles_RoleId]
GO


/


CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[IdentityRole_Id] [nvarchar](128) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_IdentityRole_Id] FOREIGN KEY([IdentityRole_Id])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_IdentityRole_Id]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id]
GO

/

CREATE TABLE [dbo].[IdentityUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.IdentityUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[IdentityUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[IdentityUserClaims] CHECK CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id]
GO

/

CREATE TABLE [dbo].[RELACIONSOLVAC](
	[Cod_Solicitud] [int] NOT NULL,
	[Cod_Vacaciones] [int] NOT NULL
) ON [PRIMARY]
GO


--- INSERTAR VALORES TABLAS GENERICOS ---

INSERT INTO AspNetUsers
SELECT * FROM [NmBaseCopia].dbo.AspNetUsers;


INSERT INTO AspNetRoles
SELECT * FROM [NmBaseCopia].dbo.AspNetRoles;


INSERT INTO ApplicationRoleGroups
SELECT * FROM NmDDBCOL.dbo.ApplicationRoleGroups;

--Se tiene que activar el siguiente comando para hacer la insercion
SET IDENTITY_INSERT ApplicationGroup ON 
INSERT INTO ApplicationGroup (Id,Name) VALUES (1,'Administrador')
INSERT INTO ApplicationGroup (Id,Name)VALUES (2,'Empleado')
INSERT INTO ApplicationGroup (Id,Name)VALUES (9,'RRHH')
INSERT INTO ApplicationGroup (Id,Name)VALUES (10,'Aprobador')
INSERT INTO ApplicationGroup (Id,Name)VALUES (1010,'Aspirante')
SET IDENTITY_INSERT ApplicationGroup OFF

INSERT INTO ApplicationUserGroups(UserId,GroupId) VALUES ('bc9f1c5f-4683-4964-9fd2-f6258ee607c1',1)
INSERT INTO ApplicationUserGroups (UserId,GroupId)VALUES ('bc9f1c5f-4683-4964-9fd2-f6258ee607c1',2)

--Se hacen los siguientes delete
DELETE  FROM AspNetUsers where UserName <> 'Admin'

--Se tienen que validara los data tipe de las siguientes tablas

SOLICITUDES,
RELACIONSOLVAC

-- Se tienen que recrear la tabla 
DROP TABLE SOLICITUDES

CREATE TABLE [dbo].[SOLICITUDES](
	[Cod_Solicitud] [int] IDENTITY(1,1) NOT NULL,
	[Cod_Empleado] [smallint] NOT NULL,
	[Cod_Aprobador] [smallint] NOT NULL,
	[Fec_Solicitud] [datetime] NOT NULL,
	[Tipo_Solicitud] [varchar](2) NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Estado] [varchar](2) NULL,
	[Fec_Salida] [datetime] NULL,
	[Fec_Llegada] [datetime] NULL,
	[Modo_Vacaciones] [varchar](1) NULL,
	[Cod_Motivo_Rechazo] [smallint] NULL,
	[Observacion] [text] NULL,
	[Cod_Concepto] [smallint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SOLICITUDES] ADD  DEFAULT ((0)) FOR [Cod_Empleado]
GO

ALTER TABLE [dbo].[SOLICITUDES] ADD  DEFAULT ((0)) FOR [Cod_Aprobador]
GO

ALTER TABLE [dbo].[SOLICITUDES] ADD  DEFAULT ((0)) FOR [Fec_Solicitud]
GO

ALTER TABLE [dbo].[SOLICITUDES] ADD  DEFAULT ((0)) FOR [Tipo_Solicitud]
GO

ALTER TABLE [dbo].[SOLICITUDES] ADD  DEFAULT ((0)) FOR [Cantidad]
GO

ALTER TABLE [dbo].[SOLICITUDES] ADD  DEFAULT ((0)) FOR [Cod_Concepto]
GO


/

--Ejecutar SP VAcaciones

CREATE PROCEDURE [dbo].[SP_ReporteVacaciones]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM (
		SELECT EMPLEADOS.Cod_Empleado, EMPLEADOS.Estado,  Empleado, Cedula,
			SUM(CASE WHEN SubPeriodo = 0 THEN Dias_Disponibles ELSE 0 END) AS Dias_Causados,	
			SUM(CASE WHEN SubPeriodo <> 0 THEN Dias_Tiempo ELSE 0 END) AS Dias_Tiempo,
			SUM(CASE WHEN SubPeriodo <> 0 THEN Dias_Dinero ELSE 0 END) AS Dias_Dinero
		FROM EMPLEADOS
		JOIN VACACIONES on (EMPLEADOS.Cod_Empleado = VACACIONES.Cod_Empleado)
		WHERE EMPLEADOS.Estado <> 'R'
		GROUP BY EMPLEADOS.Cod_Empleado, Empleado, EMPLEADOS.Estado , Cedula
	) V LEFT JOIN (
		SELECT Cod_Empleado as Cod2,
			SUM(CASE WHEN SOLICITUDES.Estado not in ('A', 'AP', 'R', 'D') THEN Cantidad ELSE 0 END) AS Dias_Pendientes,
			SUM(CASE WHEN SOLICITUDES.Estado in ('A') THEN Cantidad ELSE 0 END) AS Dias_Aprobados,
			SUM(CASE WHEN SOLICITUDES.Estado in ('AP') THEN Cantidad ELSE 0 END) AS Dias_Pagado
		from SOLICITUDES
		GROUP BY Cod_Empleado
	) S on V.Cod_Empleado = S.Cod2
	ORDER BY V.Cod_Empleado
END



--- Validar la existencia de estos parametros y estas columnas para empresas que ya habian tenido el software

UPDATE PARAMETROS SET Usuario = 'julianaweb@systemsltda.com', Servidor = 'mail.systemsltda.com', Clave = 'Juliana2002', Remitente = 'julianaweb@systemsltda.com'
-------
ALTER TABLE VACACIONES ADD Util_A char (1) 
UPDATE VACACIONES SET Util_A = ' '


