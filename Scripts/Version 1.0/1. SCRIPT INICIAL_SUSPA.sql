/****** Object:  Table [dbo].[ApplicationGroup]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationRoleGroups]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationRoleGroups](
	[RoleId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationRoleGroups] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationUserGroups]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserGroups](
	[UserId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationUserGroups] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[IdentityRole_Id] [nvarchar](128) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EMPLEADOS]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMPLEADOS](
	[Cod_Empleado] [smallint] NULL,
	[Empleado] [varchar](100) NULL,
	[PNombre] [char](20) NULL,
	[SNombre] [char](30) NULL,
	[PApellido] [char](20) NULL,
	[SApellido] [char](30) NULL,
	[Tip_Documento] [char](1) NULL,
	[Cedula] [varchar](20) NULL,
	[Seguro_Soc] [char](12) NULL,
	[Fec_Nacimiento] [char](8) NULL,
	[Est_Civil] [char](1) NULL,
	[Sexo] [char](1) NULL,
	[Direccion] [char](45) NULL,
	[Estado] [char](1) NULL,
	[Telefono] [char](25) NULL,
	[Celular] [char](14) NULL,
	[Lib_Militar] [char](12) NULL,
	[Distrito] [char](3) NULL,
	[Dir_Elec] [varchar](100) NULL,
	[Cod_Profesion] [smallint] NULL,
	[Tipo_Contrato] [char](1) NULL,
	[Dias_Contrato] [smallint] NULL,
	[Periodo_Prueba] [smallint] NULL,
	[Tipo_Salario] [char](1) NULL,
	[Horas_Acumuladas] [numeric](18, 0) NULL,
	[Fec_Ingreso] [char](8) NULL,
	[Fec_Salario] [char](8) NULL,
	[Salario] [float] NULL,
	[MetodoRetefuente] [char](1) NULL,
	[Val_Retefuente] [float] NULL,
	[Por_Retefuente] [real] NULL,
	[Tipo_Retefuente] [char](1) NULL,
	[Deducible] [float] NULL,
	[DedSaludEdu] [float] NULL,
	[DedAFC] [float] NULL,
	[DedAlimentacion] [float] NULL,
	[Regimen] [char](1) NULL,
	[Fec_Retiro] [char](8) NULL,
	[Val_Liq_Contrato] [float] NULL,
	[FormaPago] [char](1) NULL,
	[Sabado] [char](1) NULL,
	[Cod_Ciudad] [smallint] NULL,
	[Cod_CajaCompensacion] [smallint] NULL,
	[Clasificacion_Contable] [char](1) NULL,
	[Tipo_Cta] [char](1) NULL,
	[Num_Cta] [char](20) NULL,
	[Banco] [char](15) NULL,
	[Cod_Zona] [smallint] NULL,
	[Cod_Sucursal] [smallint] NULL,
	[Cod_Ccostos] [smallint] NULL,
	[Cod_Depto] [smallint] NULL,
	[Cod_Cargo] [smallint] NULL,
	[Cod_Grupo] [smallint] NULL,
	[Cod_Arp] [smallint] NULL,
	[Cod_Eps] [smallint] NULL,
	[Cod_Afp] [smallint] NULL,
	[Cod_Afc] [smallint] NULL,
	[Cod_Escala] [smallint] NULL,
	[Cod_Tarifa_Plena] [smallint] NULL,
	[Modo_Costo_Hora] [char](1) NULL,
	[Cod_Empleador] [smallint] NULL,
	[Aux_Transporte] [char](1) NULL,
	[Cod_Pais_Nacimiento] [smallint] NULL,
	[Cod_Pais_Nacionalidad] [smallint] NULL,
	[Fec_Vence_Visa] [char](8) NULL,
	[Porc_Anticipo] [real] NULL,
	[Porc_Anticipo_Prima] [real] NULL,
	[ModoSalario] [char](1) NULL,
	[AnticipoPrima] [char](1) NULL,
	[Cod_NvaEps] [smallint] NULL,
	[Cod_NvaAfp] [smallint] NULL,
	[Cod_NvaAfc] [smallint] NULL,
	[Fec_CamEps] [char](8) NULL,
	[Fec_CamAfp] [char](8) NULL,
	[Fec_CamAfc] [char](8) NULL,
	[Tiempo_C] [char](1) NULL,
	[Clave] [varchar](30) NULL,
	[Saldo_Base_Aporte] [float] NULL,
	[Base_Aporte] [float] NULL,
	[Saldo_Dias_Aporte] [smallint] NULL,
	[Dias_Aporte] [smallint] NULL,
	[No_Hijos] [smallint] NULL,
	[LiqNomina] [smallint] NULL,
	[Estudiante] [char](1) NULL,
	[Practicante] [char](1) NULL,
	[MedicinaPrepagada] [char](1) NULL,
	[Telefono1] [char](12) NULL,
	[Porc_Embargo] [real] NULL,
	[Saldo_Ini_Horas] [float] NULL,
	[DiasVacAno] [smallint] NULL,
	[Cod_Causal_Retiro] [smallint] NULL,
	[Cod_Lugar_Expedicion] [smallint] NULL,
	[DiasVacPendientesDisfrute] [smallint] NULL,
	[DiasSalud] [int] NOT NULL,
	[FecLimiteDeducible] [char](8) NOT NULL,
	[Fec_Cambio_Tipo_Salario] [char](8) NULL,
	[GrupoSanguineo] [char](20) NOT NULL,
	[Cod_Colaborador] [char](20) NOT NULL,
	[Cod_Barrio] [smallint] NOT NULL,
	[Cod_Jefe] [smallint] NOT NULL,
	[Declarante] [char](1) NOT NULL,
	[VigenciaDependientes] [char](8) NOT NULL,
	[DiasVivienda] [smallint] NOT NULL,
	[DiasPrepagada] [smallint] NOT NULL,
	[DiasDependientes] [smallint] NOT NULL,
	[FecSubstitucionPatronal] [char](8) NOT NULL,
	[Cod_Alterno] [smallint] NOT NULL,
	[Cod_GrupoDif] [smallint] NOT NULL,
	[Tipo_Pension] [char](2) NOT NULL,
	[Tipo_Pensionado] [char](1) NOT NULL,
	[Pension_Compartida] [char](1) NOT NULL,
	[Cod_Agencia] [smallint] NOT NULL,
	[Regretted] [char](1) NOT NULL,
	[NoBeneficio558] [char](1) NOT NULL,
	[Regreted] [char](1) NOT NULL,
	[CodPractica] [char](50) NULL,
	[Transfer] [char](1) NOT NULL,
	[NumHorasMes] [char](5) NOT NULL,
	[NomCliente] [char](150) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EMPRESAS]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMPRESAS](
	[Codigo] [smallint] NOT NULL,
	[Tipo_Documento] [char](1) NOT NULL,
	[Num_Documento] [char](9) NOT NULL,
	[Digito_Verificacion] [char](1) NOT NULL,
	[Nombre_Empresa] [varchar](200) NULL,
	[Direccion] [char](40) NOT NULL,
	[Cod_Depto] [smallint] NOT NULL,
	[Cod_Ciudad] [smallint] NOT NULL,
	[Tel] [char](10) NOT NULL,
	[Fax] [char](10) NOT NULL,
	[Cod_Arp] [smallint] NOT NULL,
	[Cod_Sucursal_Pag] [smallint] NOT NULL,
	[Clase_Aportante] [char](1) NOT NULL,
	[Forma_Presenta] [char](1) NOT NULL,
	[Fec_Instalacion] [char](8) NOT NULL,
	[Fec_Ult_Acceso] [char](8) NOT NULL,
	[Dias_Vigencia] [smallint] NOT NULL,
	[Fec_Vence_Licencia] [char](8) NOT NULL,
	[Clave] [char](10) NOT NULL,
	[Codigo_Habilitacion] [char](10) NOT NULL,
	[Cod_Pais] [char](1) NOT NULL,
	[Licencias] [smallint] NOT NULL,
	[Servidor] [char](25) NOT NULL,
	[BaseDatos] [char](25) NOT NULL,
	[Usuario] [char](25) NOT NULL,
	[Passw] [char](25) NOT NULL,
	[Ruta] [char](25) NOT NULL,
	[ModNOM] [char](1) NOT NULL,
	[ModRH] [char](1) NOT NULL,
	[ModMIS] [char](1) NOT NULL,
	[Serial] [char](10) NOT NULL,
	[Cod_Local] [char](20) NOT NULL,
	[Logo] [varbinary](max) NULL,
	[LogoOpc] [varchar](max) NOT NULL,
	[CargaLogoOpc] [bit] NULL,
	[Estado] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PARAMETROS]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PARAMETROS](
	[Ano] [smallint] NOT NULL,
	[Salmin] [float] NOT NULL,
	[Salint] [float] NOT NULL,
	[SubTrans] [float] NOT NULL,
	[ApoSalud] [real] NOT NULL,
	[ApoFondoSalud] [real] NULL,
	[ApoFondoPen] [real] NOT NULL,
	[ApoFondoSolPen] [real] NOT NULL,
	[Cod_Arp] [smallint] NOT NULL,
	[ApoFondoArp] [real] NULL,
	[LiqNomina] [smallint] NOT NULL,
	[ApoPsalud] [real] NOT NULL,
	[ApoPFondoPen] [real] NOT NULL,
	[ApoPriesProf] [real] NOT NULL,
	[ApoCajaCom] [real] NOT NULL,
	[ApoIcbf] [real] NOT NULL,
	[ApoSena] [real] NOT NULL,
	[ACesantias] [real] NOT NULL,
	[AintCesantias] [real] NOT NULL,
	[Aprimas] [real] NOT NULL,
	[Avacaciones] [real] NOT NULL,
	[Fec_Inicial] [char](8) NULL,
	[Fec_Nomina] [char](8) NULL,
	[Tipo_Liquidacion] [char](1) NULL,
	[PorcSalNor] [real] NULL,
	[PorcSalInt] [real] NULL,
	[PorcSalEsc] [real] NULL,
	[TipoSabado] [char](1) NULL,
	[Codigo] [smallint] NULL,
	[CtaGtoCajas] [char](8) NULL,
	[CtaPasCajas] [char](8) NULL,
	[CtaGtoIcbf] [char](8) NULL,
	[CtaPasIcbf] [char](8) NULL,
	[CtaGtoSena] [char](8) NULL,
	[CtaPasSena] [char](8) NULL,
	[CtaGtoSalud] [char](8) NULL,
	[CtaPasSalud] [char](8) NULL,
	[CtaGtoPension] [char](8) NULL,
	[CtaPasPension] [char](8) NULL,
	[CtaGtoRiesProf] [char](8) NULL,
	[CtaPasRiesProf] [char](8) NULL,
	[CtaGtoCesantias] [char](8) NULL,
	[CtaPasCesantias] [char](8) NULL,
	[CtaGtoIntCesantias] [char](8) NULL,
	[CtaPasIntCesantias] [char](8) NULL,
	[CtaGtoPrimas] [char](8) NULL,
	[CtaPasPrimas] [char](8) NULL,
	[CtaGtoVacaciones] [char](8) NULL,
	[CtaPasVacaciones] [char](8) NULL,
	[Tipo_Interfase] [char](1) NULL,
	[DiasMaxHabiles] [int] NULL,
	[IntMora] [real] NULL,
	[Sal_Max_Asegur] [smallint] NULL,
	[ValMaxAnual] [float] NULL,
	[ValMaxMensual] [float] NULL,
	[PorcEduMensual] [real] NULL,
	[PorcPrimaSalNormal] [real] NULL,
	[PorcPrimaSalEscala] [real] NULL,
	[Hora_Ent] [char](10) NULL,
	[Hora_Sal] [char](10) NULL,
	[Tiempo_Alm] [char](10) NULL,
	[TopeMaxRentaExcenta] [float] NULL,
	[PorcMaxApoPen] [real] NULL,
	[TipoLiqFestivos] [char](1) NOT NULL,
	[Corte30Retefuente] [char](1) NOT NULL,
	[Corte30SegSocial] [char](1) NOT NULL,
	[Fec_Acumulado] [char](8) NOT NULL,
	[Val_Uvt] [float] NOT NULL,
	[PptoAnualCap] [float] NOT NULL,
	[Promedio_Cesantias] [char](1) NOT NULL,
	[Corte30Provisiones] [char](1) NOT NULL,
	[BasePrimaSubsidio] [char](1) NOT NULL,
	[Sal_Max_AsegurRie] [smallint] NOT NULL,
	[Calculo_Anticipo] [char](1) NOT NULL,
	[Pago_Vac_FecLlegada] [char](1) NOT NULL,
	[LogoSucursal] [char](1) NOT NULL,
	[DedFiscales] [char](1) NOT NULL,
	[CtaActGasAnt] [char](10) NOT NULL,
	[GastoAnticipadoSueldos] [char](1) NOT NULL,
	[DedFiscalSalud] [char](1) NOT NULL,
	[Servidor] [char](30) NOT NULL,
	[Usuario] [char](30) NOT NULL,
	[Clave] [char](20) NOT NULL,
	[Puerto] [char](6) NOT NULL,
	[Remitente] [char](60) NOT NULL,
	[Mensaje] [char](400) NOT NULL,
	[Por_Bonos] [real] NOT NULL,
	[CtaPasBonificaciones] [char](8) NOT NULL,
	[TopeSalud] [int] NOT NULL,
	[TopePension] [int] NOT NULL,
	[TopeRiesgos] [int] NOT NULL,
	[TopeCaja] [int] NOT NULL,
	[Vac100] [char](1) NOT NULL,
	[TopeMaximo] [real] NOT NULL,
	[DiasLic] [char](1) NOT NULL,
	[DiasCesantias] [smallint] NOT NULL,
	[Ley1429] [char](1) NOT NULL,
	[UVTActual] [char](1) NOT NULL,
	[BaseCesantiaSubsidio] [char](1) NOT NULL,
	[SubParcial] [char](1) NOT NULL,
	[SubCompleto] [char](1) NOT NULL,
	[SubDiasParcial] [char](1) NOT NULL,
	[SubDiasCompleto] [char](1) NOT NULL,
	[TopeMaxAportes] [float] NOT NULL,
	[TopeMaxSalud] [float] NOT NULL,
	[TopeMaxDependientes] [float] NOT NULL,
	[AportanteExoneradoCajaSalud] [char](1) NOT NULL,
	[RedoneaRetefuente] [char](1) NOT NULL,
	[SubPrimeraVariables] [char](1) NOT NULL,
	[DiasNoLabDerecho] [char](1) NOT NULL,
	[DiasTrans] [char](1) NOT NULL,
	[GastoVacacionesAnt] [char](1) NOT NULL,
	[DedSaludMesAct] [char](1) NOT NULL,
	[RedondeoSaludPension] [char](1) NOT NULL,
	[UsaConceptosSena] [char](1) NOT NULL,
	[SSL] [bit] NOT NULL,
	[LiquidaDiasDerecho] [char](1) NOT NULL,
	[Dia31] [char](1) NOT NULL,
	[IbcAnt] [char](1) NOT NULL,
	[PorcMinBaseRet] [float] NOT NULL,
	[Fec_Aplica_Reforma] [char](8) NOT NULL,
	[Autenticar] [bit] NOT NULL,
	[AuxTransRemoto] [numeric](18, 2) NOT NULL,
	[UvtDependientes] [numeric](18, 2) NOT NULL,
	[HorasTrabMes] [numeric](18, 0) NOT NULL,
	[FecNvaReforma] [varchar](8) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PARAMETROS_GENERALES]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PARAMETROS_GENERALES](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cod_Parametro] [varchar](100) NULL,
	[Valor] [varchar](2000) NULL,
	[Descripcion] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PERMISOS_WEB]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PERMISOS_WEB](
	[Usuario] [char](12) NOT NULL,
	[Page] [varchar](max) NOT NULL,
	[Permiso] [bit] NOT NULL,
	[Pagina] [varchar](max) NOT NULL,
	[Descripcion] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TERCEROS]    Script Date: 30/01/2026 2:58:47 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TERCEROS](
	[Cod_Tercero] [smallint] NOT NULL,
	[Documento] [char](15) NOT NULL,
	[Dv] [char](2) NOT NULL,
	[Tipo_Documento] [char](2) NOT NULL,
	[Tipo_Tercero] [char](10) NOT NULL,
	[PNombre] [char](30) NOT NULL,
	[SNombre] [char](30) NOT NULL,
	[PApellido] [char](30) NOT NULL,
	[SApellido] [char](30) NOT NULL,
	[Tercero] [char](120) NOT NULL,
	[Dir_Elec] [char](100) NOT NULL,
	[Cargo] [varchar](100) NOT NULL,
	[CareerID] [varchar](20) NOT NULL,
	[Estado] [int] NOT NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ApplicationGroup] ON 
GO
INSERT [dbo].[ApplicationGroup] ([Id], [Name]) VALUES (1, N'Administrador')
GO
INSERT [dbo].[ApplicationGroup] ([Id], [Name]) VALUES (2, N'Empleado')
GO
INSERT [dbo].[ApplicationGroup] ([Id], [Name]) VALUES (3, N'Aprobador')
GO
INSERT [dbo].[ApplicationGroup] ([Id], [Name]) VALUES (4, N'Administrador Empresarial')
GO
INSERT [dbo].[ApplicationGroup] ([Id], [Name]) VALUES (9, N'RRHH')
GO
INSERT [dbo].[ApplicationGroup] ([Id], [Name]) VALUES (1010, N'Aspirante')
GO
SET IDENTITY_INSERT [dbo].[ApplicationGroup] OFF
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'0e6a2caf-6b7f-4976-a165-f902e7bc7c14', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'0e6a2caf-6b7f-4976-a165-f902e7bc7c14', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'15f128b4-301c-4918-b2b0-7a1af93761a3', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'15f128b4-301c-4918-b2b0-7a1af93761a3', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'15f128b4-301c-4918-b2b0-7a1af93761a3', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'15f128b4-301c-4918-b2b0-7a1af93761a3', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'15f128b4-301c-4918-b2b0-7a1af93761a3', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'1habch12-1icv-094d-SNCD-Ahchbnsnm2', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'1habch12-1icv-094d-SNCD-Ahchbnsnm2', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'1habch12-1icv-094d-SNCD-Ahchbnsnm2', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2700343d-e885-4b56-bbe2-f838b1ce4f02', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2700343d-e885-4b56-bbe2-f838b1ce4f02', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2700343d-e885-4b56-bbe2-f838b1ce4f02', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2700343d-e885-4b56-bbe2-f838b1ce4f02', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2700343d-e885-4b56-bbe2-f838b1ce4f02', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2c26s6a1-9s25-9111-ABCD-a444ssc5dc', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2c26s6a1-9s25-9111-ABCD-a444ssc5dc', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'2c26s6a1-9s25-9111-ABCD-a444ssc5dc', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'30007571-999d-4eau-a12b-3c4e9421fbd', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'30007571-999d-4eau-a12b-3c4e9421fbd', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'39337578-119d-4e52-a0ab-3c4e9079fbd0', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'39337578-119d-4e52-a0ab-3c4e9079fbd0', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'39337578-119d-4e52-a0ab-3c4e9079fbd0', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'39337578-119d-4e52-a0ab-3c4e9079fbd0', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'39337578-119d-4e52-a0ab-3c4e9079fbd0', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'492015e4-d5b5-4461-98ea-25f5d0ff8394', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'492015e4-d5b5-4461-98ea-25f5d0ff8394', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'492015e4-d5b5-4461-98ea-25f5d0ff8394', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'492015e4-d5b5-4461-98ea-25f5d0ff8394', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'492015e4-d5b5-4461-98ea-25f5d0ff8394', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'492015e4-d5b5-4461-98ea-25f5d0ff8394', 1010)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'59e8428d-8186-4096-9340-82102520bdfc', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'59e8428d-8186-4096-9340-82102520bdfc', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'59e8428d-8186-4096-9340-82102520bdfc', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'59e8428d-8186-4096-9340-82102520bdfc', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'59e8428d-8186-4096-9340-82102520bdfc', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'69a5152f-2420-4056-a179-a78a355d0ee7', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'69a5152f-2420-4056-a179-a78a355d0ee7', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'6f57b492-b919-438c-b52e-effe182d2602', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'6f57b492-b919-438c-b52e-effe182d2602', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7237fxd8-as1d-6343-aas1-a42gjsdacr3c', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7237fxd8-as1d-6343-aas1-a42gjsdacr3c', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'732ca543-4d77-4de2-980c-b0d3ed1ecbb8', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7705535b-ed70-4259-a9a9-5545712dbee5', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7705535b-ed70-4259-a9a9-5545712dbee5', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7705535b-ed70-4259-a9a9-5545712dbee5', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7705535b-ed70-4259-a9a9-5545712dbee5', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7705535b-ed70-4259-a9a9-5545712dbee5', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7c1e7880-947b-4c5b-87da-e3222ba053b4', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7c1e7880-947b-4c5b-87da-e3222ba053b4', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7c1e7880-947b-4c5b-87da-e3222ba053b4', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7c1e7880-947b-4c5b-87da-e3222ba053b4', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7c1e7880-947b-4c5b-87da-e3222ba053b4', 1010)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7d0ae81a-6009-4cfc-8ea4-1104bdf7adcf', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7f2ea269-4074-4530-8371-18e1fca8c5c2', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7f2ea269-4074-4530-8371-18e1fca8c5c2', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7f2ea269-4074-4530-8371-18e1fca8c5c2', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7f2ea269-4074-4530-8371-18e1fca8c5c2', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'7f2ea269-4074-4530-8371-18e1fca8c5c2', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'91dc4721-c079-41c0-8bd4-b248c20e0939', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'91dc4721-c079-41c0-8bd4-b248c20e0939', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'91dc4721-c079-41c0-8bd4-b248c20e0939', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'91dc4721-c079-41c0-8bd4-b248c20e0939', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'91dc4721-c079-41c0-8bd4-b248c20e0939', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'93375728-459d-44c2-a0ab-a4gjsd4c5dc', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'93375728-459d-44c2-a0ab-a4gjsd4c5dc', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'93375728-459d-44c2-a0ab-a4gjsd4c5dc', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'93375728-459d-44c2-a0ab-a4gjsd4c5dc', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'93375728-459d-44c2-a0ab-a4gjsd4c5dc', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'976876ab-94d7-403f-b615-7faa6daa5577', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'a52483bb-5118-42b2-848a-d846a936b35a', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'a52483bb-5118-42b2-848a-d846a936b35a', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'a52483bb-5118-42b2-848a-d846a936b35a', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'a52483bb-5118-42b2-848a-d846a936b35a', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'a52483bb-5118-42b2-848a-d846a936b35a', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'abd25157-ab95-46ee-89cd-28d3ff910d49', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'abd25157-ab95-46ee-89cd-28d3ff910d49', 9)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'e41eed8b-ef28-4002-97a5-c74ffe23dde4', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'e5494def-50dd-4243-86f3-f95d428ea92f', 1)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'e5494def-50dd-4243-86f3-f95d428ea92f', 2)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'e5494def-50dd-4243-86f3-f95d428ea92f', 3)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'e5494def-50dd-4243-86f3-f95d428ea92f', 4)
GO
INSERT [dbo].[ApplicationRoleGroups] ([RoleId], [GroupId]) VALUES (N'e5494def-50dd-4243-86f3-f95d428ea92f', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'002ed7f8-71b3-4dff-867d-e861cf0cffc1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'00318d51-830a-4285-abd1-7ca2f442b79e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'004d3a04-e64a-4dee-9158-f5f470e3f4ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0085f46b-aed3-45e1-961a-c13e16ecf71c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'009cda04-bd5c-43ef-a756-bf90448c2c1b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'00c1d230-d6d5-4ad0-a2b7-ad1a79deed83', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'00e08ef5-4c83-4682-8733-afab44117d98', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'00e5a735-a831-489c-94e0-5913d16cf558', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'00f40479-a4ae-4eb4-999d-f9af076094ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'012ec088-a3dc-47c8-9827-3382d80798b3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0137f05f-3186-4d87-b0d1-ce4a92408620', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'013879fa-e00f-491d-85ba-89a82c59a259', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'016ce188-df30-4f81-9836-eab81d4ed3f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'01745941-4bb6-43dd-8586-7e5ec48b6ee9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'01a84235-e1a9-48e9-9a67-8bc8122047bd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'01daddb9-3124-4c49-9c82-182bc27dfdaf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'01ebb349-d122-4c99-bbb1-aed1c538bfa7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'01eeee66-0ea3-4878-b4c4-78d78683e7a0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'01f275a9-2175-472c-8a9e-bede816a7f5c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'02192d30-b0f8-4a64-b58c-60447c35028a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'022c659f-ef12-493f-82d1-dd596b6be272', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0251b206-f993-4433-a07f-d6975a0d61c4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'02853f60-6d42-48aa-ad83-078675b70b2e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'02acf056-35a3-4bd1-99be-0e6da848ec2f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'02cae97a-2c68-4407-83d4-38a404e9c61a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'02e990b6-bd4d-4852-ab86-f4867abbbad1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'02eff24e-c449-4a8d-9ddc-7ee73ab52bdc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0301555e-75cf-410a-a50d-1efb87803d47', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'030ca997-91c8-46cb-90b7-9480b0a555f6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'03353fd0-fc2e-4bdb-882d-fc3e0c6de8fc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'03462bcb-7ccd-46b3-88c7-c55640793ee4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'03aa51db-a346-487d-af70-0d6b18c1c7f0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'03d11210-8638-4443-915e-ac52bc108516', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'03f9fdd0-1eaf-456a-a344-405028866b1b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0406a21c-1434-4790-bd89-9d49edf7bac6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'041ee821-d4d1-44a5-8ac7-b7b1d93c865e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'042cd0fe-8fb0-48a5-ba5b-a4da0df8fe80', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0442f33a-ca6e-4945-bfbf-e8d7a1b820de', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'04647ee6-232c-475d-9eb0-ae750077e6ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'04c4e93c-945c-4ce2-b90e-7cb3ba2679ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'04e5dea8-af8d-4297-9a3b-602b7a10a1e2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'04ee2b7e-ca7e-4c50-aebe-96741ae8c99b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'05040c80-3c5b-4a7b-b00f-7c89ffe6d89d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'051a0bd3-108b-4f84-bc6d-df5cf505f2f8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0558d032-e404-41ca-aef3-b89693a04bbd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'05d3b087-2807-4eb3-947c-b386885847fc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'05e7f353-a201-484e-8914-e210ad62f1ad', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'05f53435-d30a-4bc8-b115-33bdbdb04e70', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'060d27d3-3939-4fa9-aa29-2d7491ac7a15', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06123faa-b584-4224-b88f-3eb37f03f0c1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'062c333d-fbe3-473e-a3ee-dcd5a7742ef4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06a96292-b431-4360-a834-0588aac80f07', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06c71ab7-e446-450e-824f-e242438be61f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06cc1002-88ad-4969-97da-ed0e2cdfbc14', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06cdcb90-f777-4520-a957-739b8b31087a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06d02127-235c-4fc6-9113-8400e2867785', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06dd22ce-c7b9-47cc-acf2-835adfdbc34d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06e5dcea-64b2-48df-96dc-9a48c8ff596c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'06eec40a-916f-4cb1-8d0e-97b48673065e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'07208459-6e85-4987-9716-a1d0060af292', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0741a16d-c212-4f5c-a67e-69e1e7d2e067', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'074e2911-e869-4296-8de0-ad1ed85fbf78', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0765aefa-66a7-4c25-84c9-a766cb389f12', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'07877ae4-d53d-4f25-8d7d-de2eb352aa81', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'07dbd02d-5993-45db-a0a9-1213a9c9f7f8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'07fb7b6a-f71b-4cbe-849d-eff2ab5b4463', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'080d21d5-a677-43f3-b8ca-52323c008b1e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0815e9c9-e651-4850-b771-0dd4fc1d77df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'081efc2e-9ee7-472b-b2f1-7e9cc580fb34', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'083f2cce-95e9-473c-8a63-6a2153ebdbd1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'08452c8a-518a-450d-be18-d2994ef21981', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0850b724-a6db-4020-8e77-1b228d4dd451', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0869f5fc-8938-4736-8cc2-cb9fa86471dd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'08eb16ef-c755-4a53-8dac-2cdea7d8e1d6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'08ff2993-eb5f-4e76-a743-cf18b181b7d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'090161a9-5542-45cb-a7b7-360762f831ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0972e5b1-5980-4b56-bb1c-f1588f6dab5c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0974a9b7-8655-478b-a53a-c586bf7d11b1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'097ae3a9-770b-40d3-bf61-75ff8c20705a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'09a77b22-0c4e-4b95-9b43-5d06754b62c6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'09f8d1ea-c32a-4f85-a76b-5dab55e2d1cf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0a0f4cc4-f146-42ff-a2ed-5b261e6a7a7d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0a828bac-7249-40fe-8b16-4dcd1c09515f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0a8c0c78-8452-4fce-a839-04fff538ba0c', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0a8edb08-f7a0-4a8b-97a6-96b6894818ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0aaf3e22-4d78-488e-b3e2-f29483d6a56d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0b121982-16db-4a4e-918b-7b86122293de', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0b276178-9bfd-4c0b-b99b-f2b46c6aa53b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0b55f392-318a-4682-a69f-27aac58504a2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0b62e7e3-bfe3-4118-bd3b-c1140ae85678', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0b7fe2cb-a1c8-4820-8340-0d517afdc077', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0b9af076-cc4e-466b-8242-afef8925860d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0bb77c22-f0c9-47bc-92c0-9618bc28b32c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0bc42a85-8eee-49ec-b879-f2c04c04e32b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0bc7112f-7505-4815-8478-86ac3b35378d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0bd25208-d64e-4bca-90ca-b049621a705a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0bee9d66-a411-4a9c-a851-61a77c7312d4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0bfbf51b-eadf-4749-841d-bee32de0ec07', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0c17e320-7f8e-4bc8-81ac-099624f10489', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0c26f5cc-23a3-46f1-be92-d0107521fbce', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0ca6352b-8270-434e-9a72-e5b60cb2b489', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0ca745da-92e5-4a42-83b4-405faf6036d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0ce0ddad-3660-4be4-b6ce-5dcb31f9171c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0d26985f-0787-48e2-9581-08fc8de60b16', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0d35ec40-76cf-4fbb-9372-3043932cef90', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0d48125e-5d9d-4c40-91d3-c9a9459e374f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0d698350-1bca-4f0b-97a4-44e9ea1e504a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0d892164-8d10-4c5f-8ea3-360e474e066a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0d9ac534-57dd-41ad-8f36-3754a3e065f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0daf8c50-efd1-49e0-99ff-8ec4121305a3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0db2eb33-a9ff-4c6c-8312-77b538f7e7bf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0dbd2de1-8cc6-4402-9c66-52a8930b1068', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0dc2f97b-064b-49ec-8ea2-ccf98550163a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0dd052d1-7704-44b5-af28-b007b3b73ca1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0dd6554e-1b89-4420-b0ed-04c697dbce16', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0e016839-d91c-4ecf-9afe-0f245f7be4a0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0e458905-96fb-460d-9910-829d9870cda1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0e4c7fcc-2558-482c-8e9e-53cc70e73255', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0e52078c-cce7-4221-9b08-1eda876c0e7b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0e8ae7de-3077-4d68-9c12-c4cb9afd1903', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0e8f80a4-c591-4d0c-a0d6-ee8ca46d1100', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f0fbe59-3726-43be-a173-1da5e900ef0d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f153ac0-a783-43ad-98ac-c117ebfb363f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f1764e8-06a6-41a7-9b3d-64eae199f0c8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f1f2c96-6255-4b66-9dc2-74067ad2e525', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f36b982-7fa0-43d9-aa6f-09116003f354', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f51a0fe-09bc-487e-9cba-d9825ce86f83', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f71862f-4d64-4149-89fe-b89e651941cf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f72c019-70d4-4fc3-8ea8-e9e70ed64e91', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0f790aef-15a1-4c5c-b0db-e0691a2e21d4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0fe392a3-96aa-4d1e-9606-0d660d378372', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0fe870bd-e35e-4e98-9a33-1670cc3fdfa7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'0ff855b1-789c-49dd-8a7b-d6c7e25e4684', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'10015b27-317a-4c28-830a-db3033e788e3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'10384f69-5547-4e5a-82a9-d9e5420af219', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'104d49f3-8e28-4ac4-8c6a-27a25cc89b72', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'10921501-a727-4c10-b12b-319adae5e0f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'10999064-def6-41cd-9969-9a9ac6471e77', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'111f0e7f-fe95-4f41-9e1c-29b6438a6f2d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'11473e86-a73d-46f8-9ba6-ba94e70b5817', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'11645717-e4b1-4be0-bca4-ff4186379f44', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'118bcd73-98c4-47dc-a7d3-5105683450db', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'11cc4b3b-cf21-49a0-b773-8f674ba6066f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'12407846-753d-4d18-831d-3cda0aa4234d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1269c9f5-0160-4688-a584-89fd3d59bb58', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1280e288-18f5-4c22-b6f5-f45d0444a307', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'12da6ece-0d6a-4839-b48a-f3beee50cc7c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'12fc20d2-bb6c-470f-a82b-2195c1ba0ddf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1333228e-93bc-4b5d-a9a4-db8b006da750', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1336ff05-fe97-4799-94bf-61398b30a5d2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'134ec88e-5150-455b-8c8a-6e072cd74d59', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1373f13a-918b-4ee9-84f6-115858615206', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'139f36db-2f39-40c3-a325-897f3eed1cad', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'13ecb04e-5fb1-498b-8fc2-79bd5f4013fe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'14230a23-ff1d-45b6-9375-93053772ab68', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'143d5ed8-36b1-4ee6-adf8-88628bd0169a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'14451db5-c9ac-4e52-b879-463af65957ec', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'147122ec-8c4e-4aec-9693-ffc44c64c625', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'14ade08c-bbf1-493f-ae03-c71db76d7d2a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'14de4cef-d7fd-4481-942e-16621d30ca44', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'158ec2ed-1fbe-4af8-b540-7b3de25b3661', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'15abb83a-d454-4a43-b48a-7064dacc8898', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'15bc5b03-fcba-48ec-b80a-4d8b46159e0a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'15c6dcf2-3e8d-4b4c-b347-b3ccaa35ab85', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'15f36f89-38c2-4991-8f84-f2c12742b73f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'16177fe7-539d-41db-90f7-dca1feaacac0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'162508f4-7b8f-4634-9c1f-09b1e768ec5f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'16423c95-cf12-4cf6-843c-fdd36e674031', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'169d8100-d30e-4868-a5d3-9d1c657d248e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'16cea183-5117-4297-bc64-c3eeb1554986', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'16d5c7e8-8ad2-40fb-bd4b-f2ad87f5108f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'16ede7ef-0574-4ce5-ad60-f279134b39ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'17037e1e-5c21-4947-9a21-6f875fa26ca7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1710a4a1-738c-441f-a9c8-61e0b7240160', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'17240a1e-23f2-42b3-be96-5c31f2bbc4c9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1747fb3d-54de-4f01-897b-2b79451505fe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'178fde79-fff9-49c1-96f5-29d88ecbc21f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'17968c62-24fa-4c8b-be6a-7fa756ae0221', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'17ab301a-4fd7-4ffc-8ba3-f790f7197285', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'17b2309a-bc42-446a-b3e5-fa17ee927200', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'17c8235d-145a-4a58-9708-13c7b4334a81', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'17e18563-489e-4642-b95c-891951abbb1c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'18238cc6-802b-4458-94b5-6adcca2ed048', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1872b085-18a5-4066-9d2b-d3722602d7e5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1879df3b-b61b-4381-b37b-62783ea2b976', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'18916791-2564-4c6f-9c97-0c6ccb5cdd62', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1893da7a-06ea-4443-8f87-aa28299439aa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'18951b4f-2c6a-4bd0-aea7-0a7d43cabb43', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'18cc3a5b-8a1d-4fde-a77c-71a20402b310', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'18ec5cea-76d4-4206-ab7d-d00096f80233', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'18f0a074-4a64-49ef-aa82-374fe27f7798', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1911718c-e39c-4672-ad65-03943eb4800f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1923329f-bd44-4ab8-8c0c-87b2f0d8a2f8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'193e4e30-31fb-4ce3-8ec5-37247712b0b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'19515083-6662-4c69-8603-77a372d986a1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1952bb1b-e9f1-4cf5-8a18-cd6f86e8c47d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1988cf6e-fd9c-4069-a105-6b5323143d9e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'198eef55-2fc6-4c36-91d4-610b16052eb8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1994a184-9a4e-4a36-b1cd-dfb0994a4176', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'19ad8a7e-7e96-4458-a06e-37ae4700dd6f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'19ae6b21-1fed-44c1-91e1-f07c02f1dc80', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'19afa855-7d31-4a65-89bb-fe9a50120f58', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'19bf28db-39ff-4dc5-ba02-0f525f5675cc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'19c1158d-7f0a-4fe6-b637-9ba92499ef92', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'19fd3365-6f2e-4276-9241-22dfeab3ca43', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1a5a404c-cba3-4e2c-abbc-4c0f29a7319b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1a8d5230-e47a-4b5c-b056-a5d287392909', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1ab138fc-a343-4b1d-b563-8faf08a9d021', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1ac0178b-077f-4dec-a20f-1aebeccd2f68', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1af99b21-b559-483d-ab32-5f238eaa4ed7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b0496ed-50fe-441f-a074-abeda9beb817', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b170a79-041d-4554-937a-f0e8f77456dd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b20923c-d564-4994-92a4-50e74eb31fdf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b42e62a-8ec2-4437-9dfa-2cb786f0f140', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b574a84-63ac-44fa-bdcc-8f3b9843e815', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b5c4d6e-83af-4023-a7da-726847d4023b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b7e6654-da23-45b1-823d-0a561a1028a6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b8574c0-f099-4a76-89b5-af04d484b824', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b8ef04f-e6fd-44f0-808c-75b3396664b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1b94b81f-caa0-4614-9b08-170d4e0fd7e3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1bc8cae9-4d2d-43fc-98e5-a8c899ab6e04', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1bee5aba-23b6-4e4b-8d92-9946011041cf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1c0904be-b4d5-49fe-ae51-90675bcddc91', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1c1d9479-8e3c-4254-8b56-5e5aa2f5d330', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1c3c9736-1723-407d-9a66-e91eb1b5a953', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1c47b5d3-d70a-49c0-8687-29d5e943cc65', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1c634aad-c954-477d-80a6-ce242944a28b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1c956361-7ef8-4f96-ab06-f53ca14295d9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1ca597e5-2980-4ad1-a876-9e014a425040', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1cbb8cd5-6261-4002-b76e-62b088a09cef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1cc75fd1-ae67-4d72-a9c9-459008e68360', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1cdfe435-2930-4df9-baee-fe78871602e2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1ceb95ad-1497-48f1-8774-edb1a705619c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1cf324de-b1ff-4668-8a67-57312cbc1403', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1cf5c42b-62c2-42d7-be1f-2976c85fc927', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1d106414-f27d-4ead-a4aa-e18f9e2624f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1d4c34bc-46f9-4193-8433-89068f0b88bf', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1d66bcb6-3bc6-4191-b789-43709a05ff6c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1d92010d-4b1f-43af-a453-e21bdffafe2d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1dbf5a80-ea80-452d-83f7-5d1dbf99b00b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1dd29a0d-d0f0-40f2-b5f7-47aed11b879d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1e23ad7c-d6f4-487a-8734-9fcaa46244c1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1e6eecd1-c4e4-43b1-9701-96a3927fc7ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1e8d8b8f-ca1c-44a9-aa50-e43cf2bf2b47', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1ec95265-db53-47d7-a560-6f3376841224', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1ef8e81b-2d5b-4679-b47c-e567d2efb375', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1f156922-58d8-421e-93f4-f3ff7ccf5274', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1f5e8ca1-1981-4697-8f3f-a1859d8959c8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1f638ae5-a53d-49ca-89bc-05963fc95022', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1f6b4f98-ea92-4056-b441-02fe70ec91b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1f71ff45-bdc3-400a-a0b5-98dc77c12f34', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'1f90f5f5-9456-43fe-9b41-03666d53b954', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'202ce336-20e7-46e7-99b9-1a9ccd4b6264', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2045bc98-5bab-4c9a-8572-4bf741ecc2b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2047e8ac-28e3-4be4-9e82-9b069de77876', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'20919c08-9089-4aa1-aa91-505a0a3df79f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'20a5b3b6-8164-4d37-9f8b-aaa711fe2007', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'20b4fc3b-a6cb-44d2-acbb-5c1fd8aa801a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'20cb51c5-3810-40e1-8924-c9cc669b1779', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'20ff42a2-29de-4af0-8c44-5711f04eb16a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2122e98c-9522-47e2-ad01-9406694806cf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2123f1dd-5dbb-4cdf-9c8b-bc9b38c5c7bc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'21415e3a-dd37-44ec-9b1b-2c1d0d1f5654', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'21549326-c691-40f1-969d-27523e958630', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'21556219-18c5-4e22-9197-908618b7d0e0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2183db8a-a4c0-472a-aeb1-a3f6f81e3606', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'21b1b47b-432e-4f04-a846-2f92af48cbc6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'21cccf1b-4dcf-40d2-bd18-4a70f3f5e041', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'21d5ad94-a939-4ae2-8a40-f2a981f5e5a7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'21e5737d-f2e0-47a7-b455-9a622d551574', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'22059f2b-22fe-4a78-824f-57c3cc0a9315', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'225bfaf7-1aa1-43df-83d9-c4406902ccd6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2285d938-d35c-45ae-be75-c6bbaf24c405', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'22cd3557-f9ae-4a76-a8c2-7e0719ef01c4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'230ca0df-0a7c-40ca-a49d-669cdb1a68c5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'23170667-9c68-4dc1-a202-0e6aed7bd3f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'231dfedf-bff6-4e53-8e7a-080343e46970', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2379dd99-7c1f-401c-b22b-af823e79e8c8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'238a8f3e-7882-49b2-a282-3c025f5912ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'23b7acd8-b0fb-48fd-a777-e4f92783f761', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'23c6e159-1ed4-45c0-a523-30d9d67a71ce', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2419216b-1795-4f98-88b7-621a2cda95ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'24339dfe-84fa-46a8-9871-fa7ff4df629f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2448629a-f025-437b-822b-f567dd1c10e4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'244db5e9-84d1-40f9-aae5-7d0a54ce72be', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'247c26b4-60bb-4f7a-a426-7ad852dc3abf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'247d9c78-0952-455f-a833-3cc31e6d42df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'24b3fe5d-e893-4e9d-a038-cc3ede31eb49', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'24e99cf6-3e5f-48f6-9418-72c56d81a042', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'24ec5c76-fee7-4a46-b4f6-7494b1e0f543', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'251196fe-d3fa-46fe-bb13-1fe1a82b6cab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2518d967-1d08-4593-aef3-bb5e79933564', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2523cf9d-71e2-472d-8dae-acc8822eabf7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'25292909-1bb0-4c3e-aa32-405554a7a1b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'253c19f7-fd5b-45c5-8b9a-015dcf15aedd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'254391ce-5110-40b6-90bc-0eea406e4b6d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'255fb24d-de6e-488d-8beb-5164b7b7e360', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'25714d13-b5e2-43b2-909e-cc7b47ce4a21', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'257906c7-9d4a-462e-ab95-7695dcf449a4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'258c2e42-3705-469d-989d-02a1fb2591e3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'25e40496-299f-4da9-a748-47171a5ff5ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'25e9e52e-c362-488d-a56d-eed849ba6d6e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'263804b5-dc13-4f3e-800a-708ece5b276e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'264c6b9c-81c3-4528-9f6f-8a677851cd0d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'26d072b8-b683-4158-baf9-c5d314f27d75', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'26f45d31-150a-4d96-899e-189c3af659ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'26fd11bc-6ffa-4f7a-b32d-fa39ff699237', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'27283e16-f6dd-4961-afda-af89df00cec5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2747ea69-0dce-4ace-bce8-fc45f58b18fa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'277497d9-a00e-410d-8a12-1c5ac90687cb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'27b83c75-138b-445b-af11-d9d557ec278d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'27da7cba-bf09-4c39-b8fb-c99b781930f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2815a296-5f88-4868-b5fb-2381fdb81478', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'28721240-f2af-42d0-95ed-f5cc9d7fdc88', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2888acba-3a96-4a11-bb09-f140893dc33f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'289374d5-8c5d-4e4f-82da-e4edb31db014', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'28b654c8-5f05-4ddb-abf2-fc7367049836', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'28dc0ba2-e05c-4b51-911f-b81cf3d361cc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'29362473-0547-455e-bcf3-b5bfc3c6ca3c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'295aa9e5-5c8c-47d2-87e0-2df1cd7ebe2a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2989cc25-28f3-4c8a-a19b-135fc794832e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'29a818bb-fec7-4efc-becd-f43b7c249b93', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'29c794bb-47a8-420a-99f2-a2d11785b285', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'29e36699-d144-40b5-b6a4-afabeddaac9d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2aa6b28e-45dd-4bae-a4fd-6a10b2890b00', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2ac438bf-ee1c-47c8-ac20-067f48201f33', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2b1910db-7529-431c-b0bf-54fa7a295ba3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2b28f0f6-a387-462b-bc1c-8d9b7af1201f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2b4e2761-bebb-42b1-9257-9915eec5b475', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2b6548b5-b620-4ae9-bf6d-9f13c2af62f9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2b938084-e478-48c4-891a-20a028480d6b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2bc0d4bb-75e6-43b6-ab88-13d21a303b05', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2be38efe-cfc6-4bcd-bafd-bdc111555af7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2be70484-9198-42d3-b722-cfe76f1b96bd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2bf85207-c15b-40c7-a052-d0df3fc90a35', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2c2dbff8-6902-4317-bc62-fdf65b6af726', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2c40d611-7f79-4938-a871-711673f5c19b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2cb60b2c-ad63-4bc5-8d15-29cc237b8f7f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2cb73006-457a-48bb-83f6-059ff46e4a07', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2cc1e763-accc-4a0b-ba11-9068681c27d6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2cc486a1-978f-44e2-b2bd-1b112e0ba28f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2cdcee60-66e2-4991-9e13-998603d6f893', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d0839da-3fa9-4f2f-a386-7b6af15b463d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d2ab48d-238d-4f8a-a235-866656982f37', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d2f5fde-b878-42aa-b53b-448b037c6e0a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d41d875-22a3-423b-808e-1ad3281c34fb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d481b2a-f4af-45e7-8935-8ca7340be6eb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d60e58f-f1ac-45f2-ab5d-fd21f8bfe24a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d809e17-b90d-41b9-93ef-5d76dfb205f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2d8e9f26-79d4-4566-ab3b-381aee130f46', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2dca8a9e-5c24-4c94-acda-ef6869958a25', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2dd06f56-c09b-475a-ab18-f21a33afb7a3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2dd31ea6-6c72-4e2d-8dfa-1b3619106fcd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2de6b527-ef2e-48fb-a4ed-8dc6c0068d64', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e14e759-5e67-4f9d-8be4-df752c3046b0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e1b75a7-ca50-4821-9dfe-5f4e14628da0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e3a1da3-6b63-4242-9d60-ffe7f7537fb0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e419b4f-ffa9-4292-b617-08f00147ee48', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e751b65-4b83-4cd9-9520-53b3eb6753b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e8189ca-6e64-4de4-9245-f43ff059de5d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e8fee6b-f37c-45f6-81c6-24402e1f170f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2e9e0729-32e0-46e6-8427-d5f940a9b426', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2ef0a451-8dbb-4bf2-bb49-7ff04f54142b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2f628854-acce-44ad-bc5b-d3ffdb418ea8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2f661e84-ac6e-444a-99b4-bd96af774b9e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2fe9bc19-00f4-4f01-9ef3-c9a946a37af4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'2ff6e7d1-e64b-4045-a036-bb553a6b01f6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'301e4e83-7858-4e5c-bcf5-3b265147be5f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'302bf9e6-047b-4771-99c7-06d26d993c32', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'30372fb3-c4c7-4e92-bd6d-779b1c9bbde5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'304d7396-7734-4237-a9f0-551913b7df2a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'304fa320-451d-4233-afc1-c709a21b1a59', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3070f967-80f5-4e0b-8fc9-b8441e7d4105', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'308caed7-d04d-4a20-93bf-f218da039b69', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'30b323d8-d6ca-4e4e-9142-325281dbaef0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'30cef1c7-e593-41b5-89a5-a5d1e1d2111b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'30e6ddee-63f6-49f2-a6a3-fc21cc4463e0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31429b6b-061e-432e-af25-9ebd16361053', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3152a5f4-dcf6-42c1-9805-6069b0fc83ec', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'317fc67b-f619-4e08-8884-a299d407adff', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31955d5e-b3b4-4f5a-b7c0-0ae470156e5d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31acaea1-1aa8-4102-ab1c-732d1ab328ad', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31af84ea-c73f-47fc-a35a-5914459e16a9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31d53e93-92f5-4d1c-a916-16ae2dc93c06', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31e275f0-6654-4b36-9f5b-dae5c5cae773', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31ea6941-dea4-4d3a-adee-aa76834c5f35', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'31f835c3-1387-4f99-aca0-4223ac92cad0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'32010401-dd90-4eef-93a7-57276912f27d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'322b8983-0103-44fd-9d03-98c72cf1a725', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3231dce1-676b-4398-a5ec-a389b5a96558', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'324cb54a-e060-4730-8e7f-533b9aa44692', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'326093b3-733a-4d8f-911d-6388d784b61e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'32cd941e-0b22-446f-918c-56a12124db97', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'32ceae07-7ccc-4613-ad55-d7f541c50e78', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'32e841e3-d4fb-4574-a828-dbe12e50954c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'32f53943-c497-4691-9f6e-f5a173d538b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'330f0b08-8bd7-4c51-808f-c68405472ac4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3347ff85-e509-45d9-88f4-32eef8f6b6a3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'336cc4f4-5ac7-4bd1-8069-3550baac02d4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'336fd4a5-4b7d-4ba3-afb5-b029ea652151', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'33745d9a-580e-4aac-9b34-741b79c7e7a2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'33973498-3378-431a-ba3a-01678ee3d34f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'339c83e1-f3dc-475f-b3c8-254972cb79a6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'33ddcee0-c675-4792-a782-b142e1a461f2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'33e76680-3e18-47ef-bd5c-4e8ffc1c6eaa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'33eba30b-0203-47ec-9e1c-449d90a2b739', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3420d846-447c-4f9e-82b6-19b5a666791e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3430ef5f-6da0-4fed-b7f7-85b695dc76b3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'34350ef2-8308-4691-8bed-070c83cddbec', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'343d2efa-9d5b-4baa-8b28-fc7f582524ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3495d88c-b56c-4c10-85fc-2335dc7d1e08', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'34969019-5f4e-470b-bcf9-36a8af5f17d5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'34ca70fd-d090-43dc-82bc-3cdaf17a8c50', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'34e515dc-166f-4bc8-80b2-c676ee907506', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'35039bbd-cccb-4a91-9131-0904630b3fd6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'35073088-50ce-4a1d-ae1f-b7abe32e76d0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'350e4929-df9e-44af-bc23-1b884bec8f36', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3511c92f-90e6-4745-bce5-983ff17414f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'355c2c37-e38c-4c81-9ced-4dc217a36779', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'35e4df5e-6d42-4b7c-86c6-b56c9a1f5eea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'35f1e645-536a-408b-af74-a206f6a9f9fe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'35f2a1db-a5ab-4f30-be94-fdac6882bab1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3615878f-5bd3-4fa9-a895-f867274e5b20', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'366d789b-06e6-4f2e-a094-37109573067e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3677852f-27b1-49f5-8aa5-6d6ee2ec5cb1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'369c5f2d-a64c-4d44-b1f9-e3f05a77775c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'36c9753f-a2c9-46a4-b3f8-265c0ca78bd0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'36d97914-9690-41c9-9d4a-a3377bf85559', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'36f74be1-ec19-427e-823a-5dd5a7b3c626', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'37095b9c-98ae-454e-9334-01d9cce8e73e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3735db94-1116-4c07-b3bb-624edbd80ebc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3739eca5-2052-4a49-a5fc-9e5315e19098', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3777147f-260b-463a-909c-55095ad8895e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'377bd265-f75d-48e9-971a-9527abfa0832', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'379524fd-6180-48c1-80d6-c935c20aefc7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'37972450-923b-4665-98ad-e031b02b46b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3802a61a-90ae-4134-bb84-24f26a9d77b3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'381b5fa5-d12e-48fb-abef-55d71f02436a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'381c0287-0f3c-4e32-be37-10a4027586cc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3828ec59-83a5-493d-a2da-1103f9d9fea7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38375baf-7264-45c4-afeb-448116c94bff', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'386ac94f-a20a-4c4d-bbd0-1efa8e7bbc6e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38a03eed-1ae5-4acf-a068-e3166a204b7e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38bd5a3c-fc66-461c-991c-d29595b7dbc8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38c05285-e6eb-4525-8881-3954f8392471', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38cb1536-3a33-4c17-80b7-c63ecad58269', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38e9c73a-2617-498c-bf61-745b98544e16', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38eac246-d917-4e86-8c4c-05acc2494dd6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'38f6cb3e-d79e-4571-9c7a-bc71935ce0da', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'39445d18-5c34-4906-943d-a0235146410c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'395af3d7-a9ec-4788-b7c3-39a8e1874045', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'39654d0e-531e-4a3a-8aaa-c50eefb1ba21', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'397320f7-ac3e-4b1c-8e7f-db2525d6285f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3979e0a6-391a-4a00-b4ad-0e7e68023da3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3a1a3101-0d29-4705-9b1e-2f2780d9f921', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3a3a7afa-6a3a-4a95-a85a-de7c76442355', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3aad60c5-6fbf-41aa-b96c-77150946adf3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3ab1d56c-b13f-48a4-8901-7040c23de037', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3ab65a22-2d7b-4e0f-871a-e2bfabf71b44', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3ac293c7-a264-46fc-a38b-5196628ae6e6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3ad00c5f-b6fd-4eb6-89ce-566700222d43', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3aed187b-dd25-4ef6-87a0-3598d5332022', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3aee2347-79b7-48a1-9ea1-e5ce9c373c86', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3b52b0da-b4b5-472b-8030-2ebf63a26136', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3b6e584a-8a95-4315-9a65-b202e22e2810', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3ba846b4-9768-4196-adc3-3526203f1908', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3bb4750b-fd22-434b-9d3e-d84d79ff99c1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3bc6ba1e-f01d-4b8b-96f7-a5e99fed31c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3bdc9924-2248-4e2d-bc59-3cb213db1f3f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3bef986e-522d-426b-be70-5218e74779e2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3c05759b-ce78-4d4d-b076-e01b0db134df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3c07ab90-6652-4313-aea8-4b91b2d1f570', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3c129a84-fbd8-407e-ad39-770855af8603', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3c256faf-b538-499e-be76-6d6f37d4d885', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3c763af9-90bc-48f5-85d7-ebde3afc6aa3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3c8810a5-f343-427b-8b46-1473d8bb98b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3ca47525-89d9-46fe-8894-d0b4e1c11136', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3cb1c3c9-cd67-4cc7-81c7-a97884e813c4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3cf556c4-834e-4fdc-a28b-fb1e822954fb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3d08d3bb-13f7-46bd-8cb5-0be32e6e1710', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3d4c4093-3860-4346-b363-ee8cbea9602a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3d73655f-4b73-4fa6-93b8-2f9565f0688e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3d73b463-8c99-4ad1-86cb-a0b6ff3fbe28', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3d7c4ec6-3401-4924-9cb2-ef5742e82b3e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3d8f3e9e-7cd3-4dda-9c86-40bce0701d63', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3dc156dd-6627-4670-84f9-8e0c5ee23e56', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3de81d39-fe5c-467f-82d7-e8b0a325559d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3dfc6290-2607-4dcc-9c25-c6246cb6d2eb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3dfd1d85-74bf-403f-b52b-77fe5c3766df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3e19b6c6-bf25-4b1c-8509-ee477bbf2f7c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3e1b7315-4e17-41b5-ab9b-ca3a3b05b7b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3e6a54e3-e320-4f60-9a42-5c1573e137b9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3e8934be-e49e-4e09-920a-4a65d87bde02', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3eae62e1-a134-4793-8918-be76277396c8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3ec5b532-3aff-4993-9c66-0e95242d3926', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3edb5ee6-8b85-4a1d-a0f1-5ef081532f7f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3f323ca6-b4ac-4a09-a1f4-4be36630d676', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3f34b0be-5855-49cb-9eb3-e1494e5e7b4e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3f62b813-50c8-4087-b9c9-4b6a7814200b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3f6ef0db-5dcb-4fa5-8474-b0d2218cdb4c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3f8b7319-d8fb-4059-ac48-8f660f831375', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'3fc78ec1-f1ed-4135-b9f5-45f711f0cb74', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'405c19d3-9ef1-4ae3-b34e-b72fc5728916', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'40819d83-730b-4048-ab08-895111da051f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4089ac5f-a1e7-47b3-b2b9-6530ed9eb0c1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'409d7a56-ca80-4676-b789-fb2ba761c852', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'40a7ce40-5062-4120-9029-ff71d0560f23', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4159ddf1-7031-49d5-9da0-ac075c2a9470', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4172174c-e192-4c58-9d47-2f77975d468b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4199392d-cb31-467d-b0cc-34ffaa9b3e8b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'41c253a4-eca3-485a-a62c-880baf8baf84', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'41d03530-c59a-48c1-aa68-89aac33e2495', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'41d824b7-d0ab-4b3f-913b-d628a6d9faa3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'41e23ff3-60ca-48a7-bd6e-c25cc59f372a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'42197170-f0ba-425d-bb0b-8f9681c42fff', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'423f225e-d198-4807-add2-88417553c8e5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'42405a06-c729-4051-937a-aa215158370f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'425a844a-797a-45f3-a409-4e1089b8ea61', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'426457c2-00bd-45d8-a07a-bd985dc793cc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'427ee2fd-ea91-4d62-9b99-58630ccb8c40', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'42913b54-b89c-4108-b2a3-285df2c0153f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4291772a-8d36-4483-b25b-cd4dbdca3445', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'429fcb92-13af-4e26-a762-8b397589f3c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'42c16452-fa7a-41a3-9202-ddacbd2a2be4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'431450a0-f88b-413a-8205-a6589ef615d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4347cbfd-842e-49c1-8400-8e094d661d11', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'434fe5b3-7750-4e07-9eb2-7882b5bb23fa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'436431ba-db52-4ec1-a2e8-ac82c94ae933', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'436f7f91-7edd-496e-88bd-59d7ab2a1832', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'436feb5c-2186-488b-8a94-c37dfa1758dc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'438d89f7-b2ac-496d-86a0-e4a1a83bc27d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'439c7d12-bf2a-4984-ac9d-9070874cf338', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'44127b4f-664d-42eb-8825-c7be102430a8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'441ca6e8-27ed-4318-9984-7cdcfeeb12e5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'44fe53a2-3393-4a00-a17e-83935b6d5820', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'450abdd8-5c61-4557-80b8-b778b7d4e86f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4527579b-a1d9-4cab-9b0e-388eb251019f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'453a7ea2-3a0a-4989-a6d9-2e8a28bd3326', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'453eaa6f-3a93-47a9-9800-c64f567be376', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'453fdd83-0198-4411-96f9-3c913d6ff329', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'45721c64-7bae-414b-a537-117fefffb9ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'45abe3ac-4be6-41c8-a94c-5ee1d6152e01', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'45d65706-0dd6-461a-99b4-6ff47708e866', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'45ef4c42-cb13-4253-bef3-f4e11ba59cac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'45f8b31b-9d1f-4c69-99c9-5059d4d1af51', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'46105c38-e0b8-4da4-ad85-de9183842b65', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'465058ab-a0e5-46a8-a510-33a7d5d75817', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4683ce64-d5c2-45a6-af9e-235e244f09dd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'469f907c-639f-4ffa-9235-73e6977dd579', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'46bfd7bb-b28d-4e81-b1b3-3fef29f6882c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'46d084f0-18f2-4f7b-a1e9-5bd07a1404c1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'46e1c57e-918b-4760-abac-442dffa58209', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'46eafe7f-938d-4ef1-abdb-a3fac65c094b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'46fc4ada-752e-45e4-8ffc-45c5ffa15ba8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'477ba15b-d770-47fe-9ab0-2d2e9573e137', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'47b1f3ef-a579-481c-9d2a-09cb789543dd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'47e388e9-aea7-4860-8aab-6c3f90a0ed0c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'47fab226-84ac-40b5-a5d0-68f39ff79532', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'47fcef22-bb97-4500-a106-a89822dcf51a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4804532b-eea8-4e69-84ee-a7939710f4d1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'480ce4bf-9258-4e9a-96ec-682ecf3687de', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'480d8554-cd52-4c52-9d7e-702ef2a26a24', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4813169f-530d-4c0f-9235-e8f6cac8bd49', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4818d851-7b7e-49a8-89e0-5df1d82f4ff0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'488781d8-4e70-419d-960f-50a7dbd7146d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'48b99da4-4bba-4a03-9fe4-8c2b7c86048a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'48d51a1f-687b-4aa1-85c8-126fe648950e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'48d80ba1-be4c-4e26-9ff4-645a40563cb3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'48e45cdd-20ba-4f2f-a810-e666ad292e1a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'49377f0f-3c6b-4607-9bb0-8892f3b2e769', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4986fac1-53fa-4338-a3f2-fd091a322a64', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'499ba5ef-a913-4e5b-a5a8-3e8c2c1ab1de', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'49a79e9b-9ea7-4b57-93c3-af50c6535cbf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'49aef5e5-b989-47ae-b89f-4e1f42b55588', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'49be520e-4fa0-4478-bf2e-ce45469feca9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'49c18946-a978-4563-bf24-1b15f2405b1d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4a063f1e-f49f-4c03-88d7-b21ce88253cb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4a0bd50c-500a-41ee-a8a2-358cbabdc550', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4a3dd9b2-a5dc-420b-9cf6-c1f3c8d1b67f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4a7c7904-3731-42d8-be1c-7c63f346a1d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4a97af9c-4aff-4342-aade-331d0088f8bf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4ad084b4-7bd8-4862-9b65-b8cad4d379c8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4af61cd4-a876-4865-930c-8ddc37d836b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4b3191f4-c321-44d9-b1d0-92d88788b56b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4b3630ae-e861-4567-ac9d-670d0a9fbaf4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4b38c5aa-f4b1-490f-bf96-1df2b2a7f006', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4b3a52d3-2354-43c6-97e0-ee37146b5c73', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4b4a56dc-840f-4d1d-8564-679b7c1c58b6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4b743341-c3d7-490d-b304-3b55c0ea1a7e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4b93b7d8-99fb-48c5-9fcf-dcd74dd5fbbe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4beaee9d-58f9-4cde-ac96-531582eb66a6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4c385682-f6aa-4a06-b096-9aa6be7c0188', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4c5227c9-6e36-48eb-be8f-63a6eeaa5771', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4c6c2d19-d490-4b9f-9845-35bff0029541', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4c76c657-954a-4e8c-9d98-fcfc4f48d743', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4c8838de-7a39-449d-8e82-84ccad63e346', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4ca61fcd-f95a-4284-af03-1caf6910b058', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4cc05910-3d20-4543-b5aa-921ad40f0746', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4d4d8e5d-03a3-402a-8d4e-8c6076b5d872', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4d82120e-73c5-41ba-b8da-dc5fe6558711', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4d85a221-b950-4fbc-923e-71ff9b4b83cb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4dad4f9a-1411-4b50-803c-143378c93b69', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4e0f77e9-188a-4dde-bdf0-9b592248df2f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4e2a1f6a-3759-496f-af0d-dac78368c120', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4e561db2-17e7-4848-a0db-35f80004816f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4e6b420d-9264-48c3-ba32-bc7580a9dd58', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4e744e41-9f08-4bab-ba44-aa6dd9e61982', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4e7e6c34-f661-4566-9d8d-4460ca02c6ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4e8c68e3-7fc8-4928-b801-764fde3000b3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f036689-76c5-49bf-b9ba-5238a633cf7d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f06d5b4-6ba5-447d-96b2-4349d5c723a1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f1073cd-93bb-4faa-ba89-55d5207d996c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f23ba84-5d2d-41c4-bc81-e4529f732ee1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f3ca79a-581d-4e81-92dd-bf24440968cb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f4c3cd7-c584-4c18-860e-2f2a21a8e279', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f5d4ce7-dbdd-4ee8-9226-032d441fdfb7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f5edf1e-2230-4005-9707-1e3d7e9e6b05', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f7475b5-e66e-43e2-ba17-63e0118f273f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f7fd610-78d4-4284-8be8-7bd6d4cf85a6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4f9b28cd-43a0-4759-95d2-b2bb077b5ce5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'4fca10aa-4927-486c-bbac-81624aa654e3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5047e7f6-3b4e-404a-bfd1-5441718cd4d0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'505db224-5331-41b0-9380-6d121a5657b1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'50748baa-6627-4a2a-b12b-624ea192c9e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'50849015-e579-44e6-a659-db8e019fa1fa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'50d6166e-95de-4c12-b193-d602859112a2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5131d598-ed22-46f4-ae63-e996d8449ea5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5135d1c3-93fe-4f32-a48b-721584f0c16f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'515eac8a-89b8-4ce7-820f-71c1b4b04471', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5167f62e-3f14-4513-bc05-cafb4f1d70d2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'51918a02-9b15-4af9-856e-7c2f260cd0ba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'51b35f70-734d-4c0e-a135-eb8d721b731b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'51de3b9c-4710-4e8c-a2f5-071604b1c651', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'51f1d146-75b6-4a7e-ae98-e5960b8895f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'52285a9f-8909-4fca-9316-429050f1bdae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5258b689-6960-4f6d-aa90-1753cea81e9f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5274db38-ef80-409a-b305-164d930d5080', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'52be6780-19b8-4107-a3d3-a2bf1921876f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'52c32623-cf17-4557-9e26-60f7f4dd16b2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'52fbac38-50b3-4fb0-aaa5-68cc86bf15b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5300edc2-fb0a-44d5-a69a-7b65d8a5f607', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'530e5f7f-4e41-45d6-ba30-c7f7bd7e0e44', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'530ed4ce-46e9-4a94-b65b-89a2f768caa1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'53212376-5fc1-4be8-9a93-b65d53073128', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'53323b75-3dc8-4311-8d27-6de95fa06e00', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5352e126-9b78-4b3e-b1c6-51a88473b4f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'535eb1f0-de43-4b64-b172-20110aa685fd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'53603684-62ba-41f0-91f8-319c6a6efe39', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'53ae3fca-945d-490c-9b86-b9929256cd98', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5422cdaa-7f5e-49ec-8514-946095979719', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5432a985-b00b-443f-b452-d014434f57e6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5450cd45-7dfa-465c-93c4-ebe3fe9b3e75', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'545b2c4e-ffa8-45bc-94d3-c1a35b20f425', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'545b9717-7e7a-40a7-bb25-ed197d6d5965', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'54704d56-94df-473e-9ba2-c382a478bc3e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5486a335-03e6-487c-8309-d16cd0518d75', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'548a0e78-310c-4954-b07d-3aa8d49215f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'54e0b7a2-1540-4e0e-8b05-c9ab3821e553', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'54f0b581-0fe3-42d2-8c0f-ae1e214fe110', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'54f8d263-410d-4419-90e9-419282937fe8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5511a563-2cb7-474f-9b2b-6f70922249ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5516910f-0cfb-4b87-ab38-f22406d2f0bc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'55313ce3-f126-4047-9aec-9d5028509ffb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'553b13dc-574d-4c5f-acdc-c3fd8d9fd9ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5548dcfd-951a-4987-ad4c-57f640458273', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'556a2967-ef3e-440c-ba87-2f2766a43054', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'55903b33-84d5-4668-9c08-96398233ad19', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5596e1e5-81be-4192-a71b-b81965d7553d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'559afa72-6911-4f63-930a-c4c537efbee5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'55be0632-1717-4877-91c7-390827ce301e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'55f5026d-5582-49ea-9209-439e31561b92', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5602b4e2-7223-4fa2-b6f0-6989e8f0171b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5602dcfa-acb2-4953-a984-4e6df4eb83c5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5603f9ca-ac7a-448a-8dee-826f64524289', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'560cb246-603b-46a1-8ad5-bee8f9faf452', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56116e86-2512-497e-88db-e6d5d7716a46', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'562b92e4-b78e-4866-9a36-9fbc398a5b08', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56390477-78a3-4ef3-89e4-c1bd7ce19a21', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56975b17-3b36-4fa7-ab0a-9c67b160194f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56ba18db-a3c5-4c88-89cf-9d7914d1f7e1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56c59d11-2a5e-4c3f-b9ca-87f3d7ec0626', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56cbc918-9088-41e8-81eb-da74bbc6233a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56d79a55-d128-4908-abb6-0110ec013cc9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'56efbfe5-eae1-41e7-92bf-0de9110a73b0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'571961e0-3f91-4b67-93d3-3dc6470aade8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5723a18b-99db-43d5-ac3a-3db4113ecaf2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'57405674-eceb-45cd-8464-c6573b2bff15', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'57499684-6cdb-4daa-8c38-6402c660f3bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'578898a1-491d-4215-8a6d-463d7ffa5bfb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'57bf346b-1277-490a-8402-a940c4a39a0b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'57c3423a-0311-4724-af51-5636be82b045', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'57dfb9d4-0b9a-4064-8f75-286c4d50a7ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'57f51047-5cd4-4d15-b72a-48bb611e3d7c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5827cb32-ed05-492d-b871-de0d80dafcab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5831c353-c735-4c6c-8bb2-f305c62896ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'586e3dec-34cf-4652-8f29-949362649b9a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'587c0f64-d815-4335-8025-b2e12f1af471', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5890bd98-0ee4-4724-8422-2ae6948015da', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'58a2063a-cb8c-452f-8c23-18a7b8bfd119', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'58a374de-8bac-4b2d-9849-f1a130172f95', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'58b46204-8f52-420b-8c9c-9d8f3dcf1a3b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'58bed37f-7ff2-464d-9842-e9b1a7cac7a8', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'58d7490d-a47b-4f0b-8788-597f645c3860', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'58dedee2-489f-4f02-834e-05c39ba0296e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'59111c61-9876-4850-a03f-7d3d8f1139f6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'591232e0-b14f-4567-b28c-e974e5ac55cd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5926feb9-c01f-4a84-ae5f-64f45381cfa4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5930d732-6f46-4d21-bfc7-ab80f0567503', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5931ef9c-c552-4bf5-8e24-c2610331630f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'593dde19-c972-4bb1-9e02-0fcaa2e515bf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'599f7927-b19d-49d5-bbca-3ce990301f85', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'59c4284f-174b-44f4-ac5e-5e3d87ad1029', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'59e90c8c-a193-4672-bcdd-f591736ffbac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5a10df70-f37a-4ef8-b519-3a6b89726530', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5a3714e3-b461-4fe7-a7c1-3faeeeaffd01', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5a41c978-03b8-4ffe-9dae-c3e8e3392556', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5a7d2fff-c65e-4962-a395-8e51ce7d2f14', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5ac12e6d-d2f3-4ed3-94cd-c85abc12c643', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5acac788-bff4-43a4-a70b-240f3438a47c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5ad56bd5-5cfb-4692-a553-6dd9071b5e41', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5b65dbed-e3c0-4811-a87f-57ecec7c5f00', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5b92142d-3e62-43f9-81d5-e1506e0e5e95', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5b9975e3-62e2-4e7a-a093-82e4aaec157d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5ba6e347-4707-44c7-85d2-9962dd3e0881', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5bbc2d9c-e515-422a-9534-9eb183e70254', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5bcfdac3-e40a-4906-abed-8e785d36f8d4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5bfc2f67-41b2-4786-8af9-199d357e05c8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5c11d802-ab2e-4ee9-9f79-43a9c6bc97b5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5c3c78cb-9d8f-4973-98ce-46e759e2c2ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5c46d15f-ec5f-4404-8c22-a5485314cc52', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5c4aba74-59f8-4b0f-9bfb-bc324f8d4de3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5c94d149-1c71-4818-b2ee-d6d34d3b51fb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5c969257-b8d0-40f1-9495-b1ba6cf9b1c7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5cab8a22-b991-48bb-9b3c-70f214afc43f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5cb38c12-77cb-4382-9205-5c96c8d8d7aa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5d465213-1ba6-499c-8066-8995710ceae7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5d53eead-e68d-4699-be5d-6d8ed46bf034', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5d984bc6-012b-4bd5-bd0c-db18e5a94a65', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5df59501-ddd2-4c37-b585-d0116f2875fe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5dfef15a-86ce-44de-9b88-884fdc4cd598', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5e64d17e-f25a-4222-9c46-96743218eb89', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5e66c6c1-08a3-4869-8b43-60712357559c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5ebe7d9e-5c16-47da-be6c-c1d5f098ff72', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5ee203d8-137f-4cbd-a6f7-cd247f0135e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5ee36e7e-5852-45db-bf31-7cb3ac4f18cc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5ef1466e-ab8f-4fdf-8b92-b32df761c0da', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5efde122-9ab8-44ca-99a9-df146fb58b7d', 4)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5f05a4f1-89ce-444e-b2a1-244a412d9610', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5f21a3f6-aefc-4215-9bd6-bb39cae47ca4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5f5eda6e-c25f-490a-8d31-ceabfc03d92d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5f65bf7b-c38e-4a8f-89e0-6dd89ac14427', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5fa4f10b-3084-401e-9b49-091b02ce5fbe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5fad8dbe-2226-47ac-bc2c-0e8c98cec433', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5fb2e416-8921-4330-ac0c-bac02b394dba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5fc45872-ef89-4dad-96e3-8b71b2a818ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'5fe88a29-4547-4516-b4d5-b3b336fde064', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'604f82ca-ae3b-461c-8cf4-561f04fabd65', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'60622353-8422-4dd0-9966-7042075fe092', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6062b6e6-872f-4bd5-8315-d8da7aa62af0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'608e13ab-ee3a-4946-a60f-296d0601c9b0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'609f2770-27bd-4851-bab4-f8663798a57f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'60a45620-0362-4099-a0aa-9540686a3528', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'60ed9083-dd33-4ec0-ad44-b4f7a2d22057', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'60eea472-65c4-4706-ab5e-4d740e77b7aa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'611d43b6-d2a9-4e5d-9bab-5538ed02bad3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'61317cbd-3890-4b2c-ae57-856ef58483b4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6146737e-1d4d-4c41-b884-4ce5cf5e0764', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'61615ab5-7f27-472f-adeb-4de937d55c43', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6164e199-3914-4931-9275-453ae9abc9ba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'619637b5-5c7a-4ee9-b20c-d949d7c99f9c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'61a0d53a-fa2d-4471-aa10-832268786b22', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'61cd2f11-955f-4c83-a8a8-4178ee1d45b1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'620c3ba5-d6b7-43b7-b9bb-67bf4c6764bf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'62321e5e-a457-4ac6-9e2b-ea183b3a56ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'624f3b29-0681-408a-ac3b-57da8d3f8aa8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'626383dc-f590-4945-8146-bd39c9329195', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'626bd881-7b0b-4796-ab1f-e2351908723a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6289dc19-be56-4f53-b68e-1431280d5023', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'62e67205-bedd-474c-a320-7000b3875787', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'634af7b7-01bd-4f56-9fea-cbadcffe9c56', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'635e8d8f-756f-479c-bf97-5f7b4110bc7f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'635efd41-ef47-4747-8fda-93b38cd3d989', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6360a555-8f68-4594-94e3-daffbfbe90c5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'638635cb-558a-4ec2-b131-ccb41e47a6eb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'63bfe171-4f63-4fda-8e0c-fe67beea1c69', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'63d8ab16-67dd-4718-adfa-d6848a02b2c4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'640b889a-f01d-4956-8f5b-9cc3b45e4530', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6410b8b7-5a8b-4055-bbc3-3bfee00da99d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'64178ef4-cd6a-4468-a553-641f6c276885', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'642942bc-b162-4201-a7ce-6b96bc74c3d8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'644cddf0-04d1-4892-87f9-1b5bac0c99b4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'64582479-f9c2-4ccd-861b-e67466920d97', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'64627ea9-c5a5-4964-b8c4-b465892602f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6492fca6-2c4a-4b3f-b046-a0de06e77a96', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'64d47076-64fc-438e-a1b2-0abb9fa5110b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'64ff5683-2ae4-446f-9f3a-5d74787bd3ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6506e9cc-b757-4c07-9db0-faab94e1fba9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'65184c89-ac21-4376-bb64-3f1f5cc721a3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'652d242c-fd9d-4e03-aeea-eef1315ceabb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'653c6955-6e47-46d5-b643-639fbaa7adc2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'65428911-5243-4da3-99d3-2da676055ec7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'655f4924-531a-4a73-b8a9-cc1106c3c50f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'658f547b-4710-40aa-9059-21aa37e2263e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'65c4260c-1a12-427b-b3e3-640c113e9c24', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66143896-bd18-4e19-bd8e-2986fc362dfc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6619cae0-a6bc-46cf-a95a-c2a24f2ddb62', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'663b591e-66a4-4b6a-9a2f-0f76447da8a3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66658dc3-8501-443d-a2de-8271a6afbfa7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66760177-b3fc-4d58-9486-ed8a6919516b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66b53202-2b36-4e97-a21f-308d768a828d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66cdba60-faaf-4da6-ac38-b715afd3ac40', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66ced622-3c1e-4007-b9cf-1c8cf4dc05ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66d22ea7-c6ba-4667-b196-2e53aaf41728', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'66f8d4f1-277b-4c9c-b40f-7e13537c9ec9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'67307489-2b2d-4b01-baf0-6ac26bbe96a1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'67bd03d3-8fca-4d4d-9826-57ba7bb7e2bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'67c4e9eb-39e6-4fca-ad97-b7b1f74d06e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'67cf12b3-fe97-4125-aed4-8ae63c7d9dbb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6804b463-4627-4f05-9857-3a0d1f19536f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'68085481-27b0-447c-9771-93ac43a509c6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'680d93ec-ba14-4f7a-8f39-92fb5ca5476b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'682acc50-335d-4bbf-9ac8-b66f8994e828', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6836d684-0677-41f4-a988-c2c25cfdb98f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'683d1fad-5ff9-4f53-a2f8-c69a4fd10714', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'684710c6-969c-429b-9801-f9d791009915', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'684a6452-91e5-4aee-a0d0-18e3e83be81f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'68568ab2-10cc-4336-9ff7-6b5b89dcedaf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'687e67cc-00f9-4fb7-a0f2-69f14159ac37', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6896d569-dea2-4eab-ad60-dea9308b055b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'689e08b7-0ef8-4be2-a96d-fbcb656508aa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'68ae38fc-58bb-4ece-b00e-87c15c0590b9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'68d850ef-ca9f-4a69-85ad-891e69c022f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'68f56a53-d8a7-435b-846d-80e1979e21b9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'68fb47be-86df-4e19-bca0-3762c34ac735', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6918cb70-a76e-4548-bda4-190381030108', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'691be1ce-b04c-4ed0-9288-ca4215dff909', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'693a76db-c6ab-472a-944f-b2838c07f8b9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'69470039-a9b0-4478-8f45-d67fe9ed55d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'69672001-eb91-469e-ade6-491d5b69ae87', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6976648e-fc82-4528-b57c-2607b840a8f8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6997adb3-9089-4552-b2f9-91143f684ea3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'69aedee1-a8b7-43f9-b27c-83259a2a705d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'69ca0834-fde8-44de-ac74-62f1a201290f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'69d8c621-7d94-48fe-9591-f0d1e310d5eb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'69e40a53-b5ad-40fc-8817-966988cf5df1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6a1aad76-f287-42b3-8d1c-971c7a045d79', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6a4c77eb-7446-42fd-bed2-6f272e0f55dd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6aa568ce-1979-4ad7-992c-7836a57259f0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6acf01a1-f43b-4edc-b9ea-6f91b5c994a2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6ada2c96-3b2e-4a1a-95c4-17855fc85b9f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6afaf2ef-b49c-4f90-bf07-a83b7ab942ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6b1d7690-d8d9-4698-9a2c-ead0d020d11b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6b372ec0-1b8c-400b-8502-8e5e8b30cddf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6b471b96-93d7-47e0-bb8d-99815a1da7ad', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6b489cc8-6536-49ff-989b-e130efe2513e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6b97344e-9986-4e64-90ff-a49918fda0f9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6b97fb92-e1e3-433e-b8af-74d025d6de6c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6bf35d9d-dff1-4ca7-b3da-1c01dbcc64bd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6c2b915f-137e-4434-9166-3e463e4b36e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6c5edea1-f724-4b76-9486-3e156979aa35', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6c774bbb-f586-4667-98db-4cf759fdda5a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6c785d95-aadd-465d-8d3c-138acaa9711d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6c78e314-6700-41ba-81d2-1c7b0bf81134', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6c8daff2-385c-4086-a935-57abc672cf7e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6ceb841b-757c-49ca-8a2c-51fb7b8c9f09', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6d30b118-7d23-4814-a10b-aca6e848a985', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6d484c8d-f379-42e4-9956-be46acc1fda2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6d9733e0-be3e-42f1-a14e-796b18ada7db', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6da22754-ee68-403d-b4d2-a9e0f23a3351', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6deaec60-e695-4b43-9a77-946f8cdf6e38', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6df5f1c8-6f40-4eab-91f9-933681f779ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6e0d62a8-02b4-454f-9fc5-42e924edd700', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6e1d3ace-63a5-4859-86b1-1e0448262b13', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6ef31c58-410f-468c-8ff8-b630c8eab74f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6ef80cb2-7608-47e2-b059-9e19a50ff47a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f02b99e-861f-4764-823b-afecd2af3ef2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f142d3f-ff34-4b16-b186-45e99d4a4a0a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f2c19ae-8b88-4db5-8c6b-d9c1548f2040', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f5aa4db-b0f9-464c-ab24-c3b77cf2bbfc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f6e5319-188b-4a68-baad-431701b1bca4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f7162e9-ac40-4462-8f29-1b1dc121ad54', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f82377e-bd2a-43cb-bef2-567c1fadefd7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6f98fcd9-3af2-4222-bd6e-60aad3be0aa1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6fae88cf-2078-4ef6-b0c2-eb3bea0e9a9e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6fbf1d86-b79b-40f1-a3e5-f6d89e975d9a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6fdf1566-8fc3-4c04-9384-c63141fb9da2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'6ff2328f-1b47-4804-afea-eea0c184f9d9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'70101902-2e74-40ea-bf70-a50e07e7e931', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'70347292-eb81-4264-ba29-085be44b22ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'705a2ad2-a6f9-43e0-9cf6-c61e5a62564e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'705dc633-efcd-4a68-b38c-e9556c45b059', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'70662129-b864-4de7-bfc1-0cfd837b37e1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'70821ac2-ef2b-4b47-8eb1-37dd668f6536', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'70c9ea43-8809-40b0-9c0b-473fc3f58f69', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'70d64491-d40e-4120-a55b-2988fa1fa585', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'70e54554-ee2e-44e8-981a-cbe7939ceec7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'71318ea6-dd6f-47a9-aea0-8652afb6ee7b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7150ce02-40c8-474a-900f-364314572f2d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'72177e80-bb91-43b7-8c61-a8183a53065e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7229ea7a-3a6d-42ec-b9f5-9c0fc35ef4a5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'722fbd5e-4cb5-4acb-9f86-e5bf0280eee2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'723896f2-0822-4815-b6fd-8d4a0c2f6393', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'726798bc-68e1-48ff-9fac-f690d2564692', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7271b261-6385-4c78-837e-9c221b96306c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'728874b8-78bd-4bca-b519-a561b7461a70', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'72a014db-fb65-468f-9e2c-cfb47ccc2f59', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'72c0906f-ff60-481f-b221-5977c281f3a1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'72f2b3e9-ff4e-49b4-bfc6-46a97da61983', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7313e7cb-dabc-4ae3-9ad0-d8fca82a8e72', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'732858f8-8a00-4915-b3d3-1f1a21dbd6c9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7353b0aa-0793-4acc-86f6-5d67ae521d0b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'736285f8-6032-4c41-8a97-71a4b5e13371', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'736ec2c7-b435-4174-812c-a3bef07f1be1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'737b3b19-b37c-4d0e-95f0-eb9b21ad985f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'737b5fa9-898c-4e5a-a17d-28978f9a33ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7389bd6c-64a1-49ac-8fda-c58e2e9bf991', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'73cfaf54-5274-4bea-aac7-5aae5e51aec9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'73e5556a-f831-4bfa-956f-9645b37e7c0f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'73ff56a5-4bb3-4783-8298-ee82decce625', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'742cf8a6-b8af-4f58-8c9d-109061e31578', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'747748d4-a8e1-49fe-868a-3a3c1cf5b0f3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'747d3f18-0d0f-4058-830f-e995f06669a0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'74c0a9a0-8470-40e0-b799-64e54393090b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'75090ab6-2f27-4a3f-a045-0880702fe3e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'750b9c22-47fc-4590-a487-9b69f983cb4c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7584c6bd-f4e6-4399-84ec-840a33837bfa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7591aeab-00a5-4393-b820-71e3c8e10714', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'75ae3af3-021c-4214-8087-00c998ee83f1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'75b973fc-7122-40f3-89bd-72249f9c1e7a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'75c78ea8-0aa5-4f8b-a07b-24adda016bbb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'761d64bf-cc35-497a-ac51-9eb36ae7b35a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7620d4f1-7d9c-40b9-b5e4-5c69dce6696e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7630db86-7e11-4f20-bc4c-798566b5ed2a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7673c531-da24-4603-8c94-e804172a6997', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'768518bd-c31a-4b01-a1b8-f7b8d1fed2ed', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'76944460-1d10-46e4-952a-a66d26114421', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'76a6dca2-6a38-4a41-8615-67e851d24d00', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'76c259b4-fc21-4253-adb0-63495eae1936', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'76dea2a6-dad0-4af9-baab-b348eb8f4193', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'76f394f7-78f8-4566-b8fb-86b3f2b6db40', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'77204a63-e5f3-458e-90bf-ac0d73b4b9ac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'772aa8eb-7735-43be-94cc-957221b9652e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'77d1933c-d1e7-4d80-92cd-bb942be6967f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'77e30cf4-c712-49f1-bd5f-bdc2a1ed157b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'77fd6fa7-1737-4c60-9d2e-5bfcec81afc1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'77fe8302-61da-4767-a690-48341186d6c2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'781c4284-b4cf-44a8-9f9e-933342495809', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'782de907-ffa7-42c1-893f-a9adb7c542ac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'783df8cb-24d4-4acb-940e-92bd0c529e22', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'784cdfe4-4ebf-463f-a4a0-cffe72477ce0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'784e6813-4af5-443b-b8df-ae48e6ca2853', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'78798247-eb93-4cb9-969e-fc48731532ec', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'78cad547-53ed-4bba-a2eb-c9d9ee31226a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'78e65c66-d805-43e7-b406-a5cc9c97bec6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'78ec4c87-a298-4462-b13b-bed17ed29028', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'78f30345-88c4-460a-94e6-28dd41ab3536', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'78fd1078-1efe-4311-921f-4b14e4dce994', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7957cc5b-d69c-4d0f-b43c-4f7b19ec5810', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7973094f-2158-4f1d-8d0a-95cd06c4b3ed', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'797e396f-2c3f-45e0-980e-f9b213f2a011', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'79887dd8-6154-4bd0-86b4-2d212bdd9007', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'79b42549-cede-4a49-8e7f-8d04d16991f0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'79c5e935-edb1-40b5-9887-34b56e3d70b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'79c81e52-6a9d-4c76-a574-e3c1b3f116d2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'79ded1e8-523b-48f1-bc2f-ff2d0c70c742', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7a0ba961-6fed-4fb5-be97-cbe8754c1bbb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7a26618b-fa39-478b-bf05-86d2dd5c53b0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7a283c47-28cb-4e73-a987-89f6567ef414', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7a31cf50-98ad-4f09-8487-a386152f47a2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7a64afdd-d286-456a-a97c-ad54a8e814ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7a98a3e1-bd0b-4ef2-b0dd-81b554ea4650', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7aa0988e-d1de-418f-85d2-1413be8a0ed1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7aa2b18e-2b29-4c75-8dd5-7501768d83b4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7ab1a189-d2ac-45f3-95ef-f99e0fbf89ed', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7ab277e3-ff8c-432c-927c-5945e8fb0504', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7ae4f1ac-ba41-406f-ad1b-06e2e7e0a3cd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7af3c879-2127-448a-aa94-b44c57cc7405', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7b05f1ac-4900-460c-bea3-2f076a611abf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7b146552-f4c5-4a00-bb81-3ea4b6599f1f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7b2c121d-041d-4de3-a8fa-4e0e5b41d3d8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7b3de494-251c-4625-9b3b-85132a39dfdd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7b4f489a-d5bf-4918-ad4f-2ba6d7aa53dd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7bb3c6bb-4765-4abf-b7ca-ac8e203286bf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7bc4a3f6-0175-4568-b70a-01cc7e376f0e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7bcc634b-e43b-47af-86df-440051ce6def', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7bcee407-63f0-4026-9313-3046f7291a01', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7be22737-e205-4eec-bff6-ef0c8c501d08', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7be8fa72-2078-4cd2-9167-52cd5aca5f40', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7c15abaa-984a-4cf9-95cd-7126fd0119d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7c51dc05-bd38-406d-9d8b-233f6b06a5e6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7c668da9-e938-4cb2-9abd-aaa76e1a4d74', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7cb2bfe2-033c-49cb-8f6b-47357a38529d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7d1821b9-ccd8-4e97-a268-6ad0a9155559', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7d31947a-cb32-4d2c-b241-0f91e05112ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7d9b0805-9ac1-44fd-a85c-9edba216241e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7d9bf96c-c9b4-4ca7-9a66-c6a6a0c7bc4c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7da08480-ff22-4b57-b323-e31d56cb8100', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7dcb1a51-1d9c-40b9-8388-8a43d7a7d985', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7dd3a061-2dd8-4c43-8b3f-26fc0edd2696', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7dd4e534-f671-477c-92e8-f80c68ad8020', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7de7a797-645b-4b8b-b076-d9bb02083191', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7dff567a-99e9-497c-b45d-a3d099ff9c5b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7e134d49-2d9e-405e-9e76-b13979e9dc82', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7e7f82bb-b01d-443f-83a5-20db070294a3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7e80837f-b6b6-4a16-b274-747d37687e7c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7e9dc84e-c6b8-4f8f-addc-eb2e18b21eec', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7ea88dd2-fa08-4e10-a9e3-420a834a7bdb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7eada8a4-e0d1-47be-9b8c-476769b2490c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7f373e4a-e87f-46b4-b07d-ddd9fd58a85d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7f7c1645-f227-44fb-b1a4-ab1528a5f98f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7f80d78e-3c90-4037-afa6-248955d0471c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7fb00353-1f2c-4af6-84cd-041e28fbcc0c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'7fd38ab8-3d0d-4d56-946d-929ac7db5a98', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'800398bc-6c43-44e1-a95d-99ab4a027442', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8075eed8-b6d6-4dc2-9e04-fbbd94592cbe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8088b0d8-0495-4400-a2f9-6b816a1f3a88', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'80ae228f-1837-491f-9725-2052832ee5df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'80b3513f-f218-47e3-a2cc-3e6465bfc7d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'80dbef98-d9d1-4569-a285-7d755afb1ffd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'80dc8980-94e8-4b89-b23a-4e0b69dc5528', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'80f92866-f81f-41fc-af4e-980ef561331a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8110c302-720a-4984-9858-92a8e48cdb11', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8124274f-ba1b-4bcc-8611-1cba52533cf9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'81538ede-1944-473e-89f4-c548b94e5875', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8168978c-4908-4d2c-84a5-fdecbdd3d5b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8175f989-ca16-451f-bc7b-ebabc2ddd5d5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'817ba003-84b6-470d-86e5-c7f51c6050d0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'81931ca2-17d5-4aff-a807-2a01bb28fce0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'81a07f85-fb7c-45c9-bd81-1055266558fb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'81de1e4e-872a-48bb-aa64-c1a37555e86d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'81e11ba8-fbc2-4db0-96ae-d0e73d2e5fb3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'82135dfc-ff77-4cdc-8a61-6d391c9f3cf0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'822b8d40-00b9-4ee3-a5f9-515ff952bf3c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8241832c-c353-492a-b689-bfc15e653041', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'82840a50-e568-436e-9653-3fbd3f84f931', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'828ff7c1-ab88-4582-afb5-8b8889ceed15', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'82b9254d-a375-46db-91a7-c13afe9df7c9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'82e297b9-4dd0-452d-9ab9-562f056a251a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'83057380-1aab-4fca-bfd2-1bc41e8500ec', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'83148b53-fc44-452d-ac40-461df974a4eb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8318b9e6-2ff6-4eff-8987-3e210749d53d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'83226d6c-bf9e-47a9-9cf6-dec9e32fe7f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8329489f-28ec-4e7a-b23f-ab669e0294d8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'833aa7b5-c68e-4875-ba7b-47253aaeb3de', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8377f135-6120-45e2-8b9a-1f21af19dc4b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'83c15906-f5e3-4004-9698-63c15d1810ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'83d05dce-abc3-419b-b122-1ef0953e8390', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8415fc52-7cbc-42bc-8165-b4f52db2f7f7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'841c480a-788e-47bc-8bce-2f865866cb6b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'843a450e-5160-4215-a3b8-a88a61028f1d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'849076b7-42cf-4e5e-9603-8f955a93043f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'84a93d31-0913-40bd-8a6f-8719ac8063ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'856c2730-ffeb-4a11-964d-1ce33c55a9d7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8573e826-239e-404e-a5e3-a1c9663e7efe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8583d00f-c05f-47e6-bad6-e356e1a8fa25', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'85901ebc-ba64-46d3-bbff-6169152f8ae1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'85c04152-37e5-4a50-812e-bcd977df592c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'863a08e3-801d-4691-bb83-1f813a3ec127', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'86477b44-10f8-4cfd-a0ca-0f77276f2642', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'865cd8f4-b0d6-4b33-9fc7-6b6ea68e7587', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8660c13b-0f1f-4fc9-8548-b46a9ef3508f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'868257d7-440c-4d13-a3c5-c78a3e9e8717', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'869ec75d-c418-4892-aff8-89df9763a875', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'86a1647f-8013-42f2-9adb-51c1244e3d88', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'86aca396-7b6b-48c8-9323-51e1a8a70ad1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'86b26b1d-d308-4df6-bd64-0a0cb5c0ab88', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'86cb77c0-69ce-4f0d-a1bd-30022c43bef0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'86ceb91c-ae60-43b5-911a-e6e328a45605', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'86d05956-6eb0-4d73-a5b0-b43c5f6d61b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'870fd12a-e8fa-4e63-841e-a59f7b6435db', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'87118707-6a5b-4982-b50d-0506089d357c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'872a7f45-a870-4a01-bd73-2a12636e0152', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'873a986b-004d-448d-bec9-2add6c4f21b0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'876cdd7b-30b3-40d7-ba4f-b3f8c40a3a58', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'87768912-22fe-4228-bea6-81134d5962f2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'87783cf9-aeef-43dc-911e-8889f6935550', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'879544a7-4a8b-413e-a226-c4c7015c1165', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'879d0a6c-b094-4145-b2c5-a8b6cf10e258', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'87b2f135-14fd-4343-b8a4-29b73e114e4b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'87cc385a-707e-4579-b2b1-e3ebc25e5646', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88208fd6-8be1-4761-a66d-82b4c4c606b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8826e873-39d2-40e9-9176-f4106f807e14', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'882812b4-4da1-431d-bb5b-e4464071f12c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8839ff9d-0d05-4f9a-a883-4f2e831d6f86', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88428724-7335-4b00-b6a9-20500eda7979', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88485f88-4de0-406a-be31-c119944d4fef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'886cf3d5-4a80-48f1-9580-fa1a571a174a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88844747-b268-4267-8713-1c756245ca0c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88af393b-4fbb-49f4-9773-8436ff5c75b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88c5d45e-4fff-48f0-a641-9f3c48dbfb38', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88eca5f2-6af5-4887-9c7c-c14835cc3aa8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'88f312cf-d94a-4915-a730-0d1881ace1bd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8924346d-b71b-4a27-8d2d-2cf90e58937b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8948e628-890a-4b77-a538-05f306c59956', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'894d92f9-360d-46d9-98ed-154edcdeeeea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'89664329-97c8-40e0-983f-3fd450c1c096', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8986bdae-61c7-4b3b-952c-dcf593b56656', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'898c348c-0289-442f-a7a0-b5d4fee49c0d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8999cb7f-1451-458e-ad99-a35e4cc205bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'89c3334c-8467-43ce-bd7d-19596b1107ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'89db1873-88d9-451d-9044-73e18c73e6f6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'89f6467f-092c-4b28-bd03-8c9c2efdd6e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8a08208e-bcd9-4393-8fe0-960ccc70359b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8a240274-98b8-42b0-a1e2-fd24d3237731', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8a2b34be-91c2-4d8e-b7f3-25639e8deea7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8a90830c-3c1e-4e55-8936-3e42bcca8261', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8aa06c6f-94d7-4000-81cc-411b2e6eec2d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8ae865ea-4b60-415c-94b9-309ecd548ef9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8b2bd660-8009-487c-a819-04c618f036e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8b2d7670-d5db-4139-997e-f8b0553fb485', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8b5b90d8-1308-4b89-80c9-c46b7bc4df4e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8b5eba6e-2afe-46d7-8d78-7672c7c8cb19', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8b6b8a8d-e9da-451c-a54a-659b59c5f8cb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8bcf9e2e-44f1-4b04-9e92-88c051a6967c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8bcfb080-2594-4b07-b93e-50b936fc2439', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8bf4572a-e5dd-419f-ade2-a2ca28a6b794', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8bfc49e0-6cf0-4cb8-a8d3-18e4901733f3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8c2c949c-72b3-445e-b31b-dc6064ff7676', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8c4203a2-5c28-4ffe-a10f-0a6494df068f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8c445419-9123-4b62-b4c4-df78961f2e74', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8c549011-1169-45df-b5c7-c9f525c1dce8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8c71539d-4311-4acd-9d4e-82ced6d8486e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8c8a13b5-562c-413c-a19c-4f0a89a4996d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8ca8b4ce-be4c-476c-857a-b03b9fcc3f1b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8cb95d13-f6b3-4afb-bf5f-6c11f483a72a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8ce115d2-be09-4877-a94c-a4f03cc239c1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8ce1290d-7a91-4564-aacd-45f1259f7410', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8cef11ed-8752-4a6e-bfd4-f3f692c0ca2f', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8d252542-d9db-420d-877a-a569e489ef74', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8d29c143-762d-4279-9e21-86135e0e7d3a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8d5497d7-d4ca-44d5-a06d-ac7332ea296c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8d61683d-ee50-4ab3-a9b0-2bbd19a70758', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8da41948-0a48-4eb9-a263-75bf1fe1bf57', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8da426ed-a40c-499f-ac66-23ae963f16a9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8dd57701-6293-4279-8ba5-018d3d1df7ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8dd9e58d-c583-41df-8e4e-80df2225632b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8e49f492-fb23-4d44-80d1-59f27126fea5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8e5e09cb-ea77-40a1-bb53-0d84a0f50885', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8e99648e-de1c-4c45-bfac-ec4248260838', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8ec756fe-1572-4fe4-94b3-046a29139046', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8eecb671-9f2e-4634-8d0b-6d5baca1baba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8f0a15c5-bebb-48f3-957d-9f1ca3cae0b5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8f88c45d-1970-4497-bc09-8c27c75ba3e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8f93ed7c-fd27-4fe4-b323-00e8462466e6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8fc72754-d2cf-4991-a61a-b12449646712', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'8fcc784c-33e1-44ca-b230-916a47675d68', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'900799cb-faba-444a-91e2-7ce7d5ef5f56', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9012735e-bd65-4ede-a817-a18b0b344219', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'903d01c9-48e8-4a91-9374-16a4c4b932ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'907377f2-9b9e-4cbf-a6df-6e29029a8c1c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'90a9e7de-c6f9-46e8-9120-c8835fac0666', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'90afaf59-b87d-453f-9cbf-10bd39eff0ba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'913b2d83-cf5e-4fb6-9739-99a3afc557bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'913e24bf-9da8-48bc-af62-63759d03ad7e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'91591968-c229-4c71-ae4c-3e4c8881778f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'91ab4393-69b1-4441-8cb0-33fe63e6679f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'91dbe75f-a5dd-4fe6-888d-00f15cd09318', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'92015778-278b-4090-93da-14436b380cc1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'926f6f60-83bc-4f94-8fdd-8f12788c3006', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9292aa6a-4c39-4c4e-850f-eb9fe3fd0d37', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'92a04a94-1788-4a3f-9c8a-70e9526f56cd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'92aee59e-7ea1-44dc-a6b0-8a10816f9cf2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'92b130e1-1682-47b8-b2b8-252867ec2500', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'92ca5f87-6891-4274-96cf-790241214851', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'92d99886-5568-4c3b-8db2-dba1a9eb2354', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'92f692c2-8dd5-4319-8f99-a13955bfe620', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'93555d63-43a2-4b2f-bb11-c29b63c01fbc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'93a9b79f-9aee-49cc-96f9-ea6a62021a01', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'93fd38df-0520-47f7-8575-1ee949bdbb88', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'94151606-dd59-4391-a04a-3154822f44ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9489d231-5642-4aea-912c-75d355b9b2b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'948a8a19-c4dd-4cf0-8938-8a3c65cc702c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'94968bcf-d69d-4eeb-89e5-c6de88cb3dcf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9498b42b-2c39-4505-a53b-1e85314a53dc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'94eec915-0543-4595-a5af-a3faf9b52417', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'950b2faf-1e07-41f7-9d8a-ee241521ed3d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'95310c63-264d-4b6d-a038-090696937a8a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'956136a2-e002-45d3-95d9-5d7695fda972', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'957e92e7-3d65-4069-aef7-8d0552367309', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9583dba4-d33a-44ef-9aa9-53fd13b39463', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'959a43f3-402a-49ab-9b56-3c6306173296', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'95cc4497-2e56-4478-865b-ad25c4a10cb8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'95e25a26-cb63-418f-8fb5-0eca6854f002', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'961b9c5e-cce1-4b1e-b915-27f0eaeeb10a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9641d4d7-7057-4507-a2b2-edf8294cc531', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'96573e45-2ab1-47fc-8616-27e52a31b474', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'965b18b0-5e96-47e7-b808-74d7bfe23d4d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'965f8a8b-8ac0-41c4-aec1-b4ac71753b05', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'96bc516c-4109-4e8d-98e0-869bf30fa9a0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'976d1d9c-0a7b-421c-a733-9aa5a3c3ed8e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'97727533-1316-4b1c-ab33-93f5c4738aaa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'979ac9bb-dffa-4e48-b005-8f1d51f204f6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'97a7405e-e843-4b55-9658-c1f4dabc7474', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'97c67d30-a711-4be5-8a5f-dd731eac5d40', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'97dafdd0-1d91-4de9-8880-25fbb91c86b2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'97e12616-2678-4097-9cbb-be27d2365bd6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'980d5790-8780-4ece-9f8d-e72815a2678e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'980f8709-f321-4471-b338-ea57d0de7f0a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9814e76f-6b5b-4d48-a8e3-4788c8b5c43a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'988c31c5-b300-470b-a94e-039598bb4917', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'98b5e68c-e50f-4547-9f52-3ece60c6707e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'98bd6482-bfcd-4841-afc7-43e4d192f4dc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'98c4c056-454f-410b-a77c-0d9015abe57d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'98d58d32-270f-45df-8e86-c25d2bffa021', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'98e80d35-e221-42b6-b87b-3c93a2a6cc15', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'990dfdf2-09fd-4aa8-b9ac-2185cd4c3d7f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9927d9a9-760b-48d1-a26d-7b0c24caaf89', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'993cfafa-52fb-4665-a5b6-22ca36c14cc2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'994c77b0-1796-4a22-9689-f1309e927636', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9968d798-8d09-4410-acb5-f7a8418e8466', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'99702035-4705-4e93-92ff-f44a0c8a8150', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'997c99e2-056f-4b8c-b262-c1c0e412745c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'998cea03-1472-4322-bfe5-ac3a12bbe783', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'99d77de9-94d8-41ad-bc8d-baea601fdd42', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'99e0c3c3-411e-434a-a9c7-d9064427fa79', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'99e71ebe-1845-4212-a258-4125a550f163', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'99f7e62b-ef02-411b-8b2d-4f197faa523f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9a2c337c-17b4-4290-abf5-8188ceaa6b54', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9a9b19b2-3a68-4e90-9076-2b757595d769', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9aaf8655-c335-4487-9722-3b903efc3d2f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9acd27d9-a54a-4feb-b67f-431e3768c5f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9ad32f6a-e51b-4224-9c8e-a13a28661bdf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9afbe86e-9374-44f4-bfb0-150e8ade0a6a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b0a2298-554e-4c14-aa0e-50af4990ec1b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b1db317-5c4b-4fb4-8eb5-ac658599f263', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b20f1bc-0d98-4406-bbd6-cd371b7ec946', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b28319a-7639-4577-807a-aaa5ed420099', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b608f4c-4622-4b02-86e2-ccee4ce5ffb7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b637916-2ee7-4194-b225-5835804be4f0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b78ae47-855d-403c-b9c3-f1f5584306f5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b8b1d24-8e5d-4a16-bfe2-8d723762febd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b8b5ceb-32b0-4b89-906f-676fdd2edb55', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9b91e083-0e7c-4587-ab6a-0ca00764e243', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9bb2edbd-4177-43e4-8dbc-5aa0a8af4158', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9bdb9b39-df3b-4f61-9ae5-f843f53357ac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9be4257c-d208-4b3d-a172-196f61ea45f1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9c542675-928f-48ec-bcf6-c744529cb944', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9c6a9a82-21fd-4286-918c-22ef67ed04ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9c95fe44-b6b4-4aaa-b1fd-1ee216f7a658', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9ccb6356-b9f2-4dd7-a959-1866730de326', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9cf0472b-689b-4408-876b-b509cf38700e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9d92e31e-d447-4406-a2ba-28432d590a3e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9da3fa3b-fd0e-4ecd-bad8-c98e2c9e7126', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9dae0d7c-f463-456a-b369-ccba3f7fdf20', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9dd6f41b-199b-425c-9114-585752e8168f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9dec07f9-8a22-43d0-8c2f-169ba17ca7af', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9dfb8a7c-8e0e-4c49-9834-0dd96f30b076', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9e02143a-a6e9-444a-a640-7d7c87248074', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9e11763f-643f-4e33-b319-454d2d066501', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9e195781-7cb4-493c-b296-01ab98bc415d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9e21108e-afec-4239-8008-3597c5a465da', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9ea394c5-b54b-4707-ac75-22bb7fc12826', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9eb007a0-6c97-449a-a786-1bcd19a661ff', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9ee97ab2-769c-4314-a218-4bd19848b8f3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9efcf2fd-cd14-45dc-9cd7-e25320eccaaa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9f132165-fe5f-48e7-9ef2-af3e3fe509b9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9f41c4ab-2c08-4b0c-94d2-87cf89c02ed5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9f54d861-66e4-457d-a604-583013ebe283', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9f5576b2-9781-4725-91c4-db16be573f51', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9f6ba85b-f5c7-443e-bcbd-08299bcd6d7e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9fb3017b-8acb-4853-9a4a-0a7bf84035d6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9fc7c2e8-9e85-4643-b3c8-e7e87cec3945', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9fca32a0-5ca5-4b15-8f6d-5cf8d433bdc4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9fe0cd53-eecb-48af-afa4-0be61535b9ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'9ff98b11-3026-4481-ba4d-46b2aaad017e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a01e60f4-7d65-4ed5-abca-23242d0486ba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a022f728-97b7-4c46-9d14-8e4ff4f03e3a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a03d09ae-1408-40a5-888a-795a8b71c4e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a0472671-c4fc-48d9-8e52-0960ca75d60d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a04ba9e7-f54e-4003-a4b3-ce050a9f4921', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a06871aa-f454-4381-8d7d-c76476fa06e4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a069d889-3ccc-49e1-b5a3-e0bd8c382ae4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a0b30389-cb35-411e-a0de-5a94672b4f97', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a0b74f7e-ac13-42ad-870a-4e835eccae36', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a11c2f28-c378-4a68-b2ff-47588f3519a4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a155b876-258d-4335-8911-2ff67d5c4c2e', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a174e866-2f36-4cff-a8fc-be0bb95d7ea0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a187f9d8-f0d2-4087-9daf-2a545c25f8f1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a1ae8c8e-48d9-4d4c-ac8c-bc2ee19a9f4d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a1bae9c7-c518-4504-8dd6-10e29986dd59', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a1cb01ee-c212-48d7-b534-1598c848477b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a1ffe8da-eb62-488c-b289-16bb632b29b3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a2490bfa-9d95-40e3-beb2-d21c3e0c54f9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a26484cf-0fc8-4e2f-9315-051ca7b3c33e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a27043fb-a53c-4925-b71f-0320a84f936b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a2706f1c-5125-41f3-84d0-5ad4420f46bf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a2d629b5-5583-4f5e-b912-e3f050095d09', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a2e4f0d3-509f-482d-a954-e177786a17fc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a33a53d5-948b-4f0c-9555-78d81d0f0d3b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a34acfb7-ea5e-4045-8fac-591bcf8e8731', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a3586e96-a474-49b3-a73f-0ad5d1744a94', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a35d2244-e0ca-4956-9159-2affef8e1ee2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a35dc56e-7200-498f-8219-c105aa98ab0d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a35ebb8c-ba0e-451f-83cf-7806c147f0fa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a36a8b32-be35-4b7c-9baf-34c592656413', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a36ae028-f2c7-4153-b5a3-81dd4849e3df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a3748d7c-e498-4b17-9153-1f75d06aa73e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a38a474e-d7af-4bfb-abb0-6817433e7b3f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a3b0da68-cd36-4d5c-8194-10e0a17a649b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a3b26ee0-dd70-4aae-8410-197a04eff291', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a3cd36c6-88d1-4870-aaac-cb4bb7fba593', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a40b7305-5685-48d5-856d-e5fd25a2b13c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a4127dd3-9300-42a9-a700-00da00826ab2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a4162b52-f6ff-4d48-b30d-3937270a8e8d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a4223d8e-f931-45c4-9eb9-0fe5bcd176ea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a4812273-e4d0-4e73-9dc3-0bbabbb70505', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a483ab3c-1c18-4de4-86ed-ff67aa2e809d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a49950c7-1be6-472d-a9aa-454dff567e50', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a4dda455-00d4-47fb-a02b-3230770b824f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a4fdad7a-a235-4779-b659-b7adeb533aa6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a50905b6-d0ea-4f9d-81d8-faa51cc7022e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a51d1dc7-2f7f-42a9-b247-847ec1ccd2c4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a544c5ac-bb32-4eae-b387-49158fa0b80e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a54c6a0e-15f1-4bec-90de-623adeaef3d9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a5919532-6d66-4e38-8b34-83ae16037586', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a5af67c4-a8b6-4565-a19d-6ca112a6713f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a5c3ecf4-01c9-4524-8734-ba4907d4b7f3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a5ea4644-9b55-4b12-b86c-9fd0d16ae26f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a60c41ab-a2a0-4afb-af67-8dd6ecca0b83', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a639758a-dc13-46ab-b3a0-a2fd28ff4a9f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a64a2202-aaf2-4e5e-82e8-9bc5dbfec165', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a64c83fb-ac48-4a5d-a04c-4f8eacc42afa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a651b486-77ed-4ef1-9d84-4f3b6ec1c13d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a685c664-5d0c-437f-b975-65f8da0f8926', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a6e8a480-9c7f-44cc-b8eb-aa64b30f19e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a6eecc31-2111-4412-848e-1d508c794912', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a741a98b-0894-4293-8c8f-ec72eb318911', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a75a0dd3-0ae4-4805-af03-80da44c0533f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a78045bd-c4bf-41ee-8751-7af9dde34f76', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a7838bc9-fd6d-43a6-ade4-bcc32ae6c92d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a7856b96-ab1a-4817-9bc2-c3699509fd22', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a78f3a8a-9b71-43a7-8780-1d1c26592b1d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a793c308-873f-4d7e-aa2e-2b59aa4f624f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a7a6fd7e-6708-4d3a-ab05-87b97cafb152', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a7a8eb91-024d-4b91-9a01-6e1f67e78119', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a7d2b14b-b184-4951-82c6-d93780aa3469', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a7fdd912-7e4c-4f36-a74f-c84433723ce0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a801f987-735d-4c29-abb0-07fe8fad2bcb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a8085754-7e31-4da7-a4c8-48f587308991', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a834395e-4d34-4c43-a4e3-d9490d9b019d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a8612407-f82f-43b2-af23-25a5b9080297', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a89e138c-7d51-4a9f-9d5a-9489da37e3e7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a8b479f3-ef9d-4352-9844-a35f2512d04e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a8f24ff8-02fa-4154-8091-ab49f3e16b4c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a901df9d-c650-40cd-9cbd-3760538aaaf5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a906cf53-b6e3-442c-a15c-045542087c7e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a9468b81-7af5-42a7-829d-5246a73094af', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a947d7ad-d854-4a9e-ad7b-e046524f7fe6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a97de451-8681-43e5-ab83-c0cc67d97649', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a9bb4498-b0d0-4892-bb85-16b1a1f92a44', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a9df9133-92bc-407c-8f7e-daf864ac4b73', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a9f78eaf-e48d-412e-8758-2bf65e82f2c7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'a9fef615-57ed-42e1-bbf6-ae9aea627b18', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aa0194f5-5855-4650-9751-bc72ed0b69b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aa0aac22-1ead-4cf7-8736-e8611b73e477', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aa18fe5e-5084-4e65-9918-62c0ed19d326', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aa1ce9be-d604-499f-879f-ff1c1ae64fda', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aa5f4593-6d3a-4e4b-9264-b0984f3dcae6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aa7dfad2-e0c7-45a5-8246-1e742c42876b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aaa02398-3981-4b64-b5c0-bef3a656d793', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aae48f5e-ead1-4cef-a775-6825cf3f31c4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aaf5e7d4-e838-44af-8933-51ca533e7a13', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ab1833da-8399-458d-baf8-009710d76ba9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ab1ca0a9-8c26-4706-a7fc-4f2e268ff60c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ab31c788-17d7-48ac-bdd8-84f8071f8902', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ab3d4428-2889-4198-84d8-fd6a6e8ee92e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ab9e0607-adc9-448f-b128-16a0d99d29ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'abc1cb6c-ddfd-4806-ba6a-f81b0a4e16d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'abe58b1a-92a7-4971-b4b6-2d4c54e1a427', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ac5ffc19-66e3-4ec0-b0d9-74615cbd74ac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ac651985-c934-4145-aaed-e5899f1cafa3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ac8ac32f-439d-4c37-b520-ec4b0fc3f4c0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'acbd8844-6d7c-476f-9cfa-783e425e04ae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'acf6088c-6e9b-469a-a723-ac35f7ff7809', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ad3f3726-8656-4ce2-8668-94f129313880', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ad4c06c9-4dfc-43be-a018-f0daeeb4f064', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ad79782b-345f-4e99-a018-1ccf62c7da23', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ad8781f7-2e37-47c1-acc7-27f9812596c9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'adb9b71b-afa7-4d59-b4ee-55722333c411', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ade7f5f8-d29f-4abf-8137-6c0bae9f9599', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ae0e25de-274f-4232-8290-9601ecf783ed', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ae6be7ef-e756-463e-bdbc-992a0a5e0415', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aeb5c266-5b5c-4037-8c34-a3591e0b42d6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aebfa258-9666-4c8f-8a04-e0a2c59c9557', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aee84026-ce68-402a-acd7-a22221facf6c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'aef8916c-8be8-4c92-9ae0-127121a51932', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'af17d3c0-c9b0-4b20-b9fb-31f375a01719', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'af19f208-647c-41c2-bff9-5cc82c065f26', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'af461dcc-2751-4fb5-943f-2fdcb0142ed8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'afb7e6f1-b5a3-49fa-9a27-0e5b2b706c2b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'afba29df-faa1-46de-85f3-10d6553934f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'afc1a68e-ae58-4824-b45d-488c789ca181', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'affc7311-8241-41ce-a427-7e4ed4cfca39', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b012ecac-c15b-43ee-a53f-fda7881ef122', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b020db8c-d2b3-4572-8884-d68124f91199', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b0308939-d86c-452e-8a7c-691bcf820c57', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b0331495-ff6a-4af5-9ea9-eb56b7f0a035', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b037a3fe-c3c8-4bd8-9f16-0a2cb9ea255c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b040559e-d7b3-4d41-aa6f-c7b09374f87f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b09c36ca-9b80-4754-8ab4-c32c8c873a31', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b0b824a8-9f88-494b-9d6b-4d752881040c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b0ca8984-eb32-467c-b702-3699828fdd11', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b0d4f9f3-24b9-497f-be10-e03fd393da66', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1258391-138d-41c8-8dd0-0349f11525ff', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b16928cb-4e95-4d45-8b73-9dca027d395a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1896fde-f90d-4d4d-8b3d-456d31667ed7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1967cf9-6e36-43c1-94cc-737f6a2270c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1a4ff8a-e101-45a2-b7d3-b2a8321a9425', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1aaf43a-485d-41a6-8b7e-6f277b47d2bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1b8eae2-f963-4d7f-83e3-75e7f0cb7430', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1c5ee35-deaa-48d2-8cdf-fd4d5d7e9ed2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b1cb8776-6491-4bb1-8724-deb549b928b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b2057a00-e76a-420c-99f4-3634204ab13b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b25e150b-3f47-40f9-bebc-2b8d608089c6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b2745c3f-e56f-4d41-b8e7-2f5fbf0f9e90', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b2b3c952-91d9-4b6a-ac4b-be4e9754f99e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b2c2b645-56de-459e-801f-e78cfb564bde', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b37ec656-26fe-4b1c-8e8b-db0e1750fa41', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b3967007-0044-46f3-bbee-c172355fb140', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b3c0dbce-07f6-41c7-90c9-5ae1131d8a62', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b3cb7024-c2b8-4c35-8aca-b081022aa92b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b41d38f2-21aa-4d8e-b9de-93bf244af488', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b4214b02-e39f-4eab-8c7a-c4ba479ae9ed', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b4347547-489d-43da-98f4-ec2e7fe9aef8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b490dc25-414a-4a13-b1f0-543ce4e9b6cc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b5108255-66d9-4c43-978d-5e38dba8ca75', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b51ec822-d4ea-4154-be79-213cdbf1204e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b55c6d03-ae93-477d-9343-a1af2ae23e21', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b5645e85-fd18-4fd7-82ee-a4e4cde9e7be', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b58bc411-1281-4485-8f72-7ff6d90b006a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b5afd658-ec3d-438d-8f2a-495e3a1a103d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b5c1d7f4-26d0-4780-8953-921ab6ebfa9a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b5ce60a6-a7e0-4505-b2b7-4b37f038d970', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b5e433a7-4bcc-4cdb-ab8e-8d85bdb49e3e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b60fcfcc-940a-4445-bb5a-276eb02bb011', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b668a702-a6f3-4915-8fe7-e29ad962b26d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b67638d4-7b72-429d-952b-e88bc6f85a1c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b6eace3d-c5b1-4629-907b-3efcdc4b5de5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b6f395db-ae81-473e-9a4c-acc1b59425d2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b7089e47-060c-4e63-8ab2-f37717109dd6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b71056f3-3998-4baa-9a3b-efa10e005a00', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b7242802-359f-4c73-a518-53e5a573a011', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b728ad6a-0d88-41e9-87a5-8d6b6141be91', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b7356fd2-5eb1-4683-8618-1b8f1a7550ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b77701dd-4e7f-44d6-96cb-49de18d4857f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b78abc4f-b99d-49c6-9b14-78cec093ff36', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b793d59c-fa89-4339-90e0-64bb53898e82', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b798e63d-d739-4d8a-89f7-cf72c53994a6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b7a852bb-a685-4f96-8678-88a59e9e8847', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b7c4520a-7204-42df-b1d5-11ebacaa6370', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b81547a8-1b05-4392-84e0-1e0e4aa76788', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b81cb23e-ee56-4b24-bae4-b9182d4f5630', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b8477f81-88ec-4f41-be7c-eb03ffe0970d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b8766315-4c3f-44a2-975a-358d108df22c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b8b304ee-3c00-4f1e-b9db-8c7714e302d7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b8b4d03e-314e-43da-982d-06a3dc585061', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b8df164f-9b6b-4e81-adc5-41d44978024e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b9221f66-fcef-4966-a9d0-eb948ff26d98', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b92e2ffe-146a-45e1-ae19-5a1c91240688', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b93dba6f-f87b-4949-9400-60e85fd9209e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b9583057-0cb2-4ef3-8433-12407da44452', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b9938e9e-c9cd-4f49-b951-1f928cbbf83b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b9a40b61-7095-4760-b5e9-8f83348792ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'b9d1a29f-dadc-4451-9779-d093587215ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ba0bc8ad-f80f-4952-9fee-28e1821dd29c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ba2ca7b7-beff-494d-87fb-139c6f46d82d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ba4e5853-0068-4c39-8fbf-d0386e8bd42e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ba7752c3-f029-4ec2-b0d9-a71acb4ea089', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ba7facdc-47b0-4b1a-812b-8bebf0839daa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ba823eee-82fe-43de-8b78-58c68aa48578', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ba8fa585-10f1-4bad-a4af-7b1bffeabc00', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bac54704-67da-4d38-9c7f-94e5b715ce89', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'badb13fd-15ec-43a6-a778-683c209c45df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'baeeb37a-ae34-4229-b50c-459a0ee6b13c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bb021a5e-89b6-46d9-b7bd-a54fea17189a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bb1c2ccf-96d1-4b50-86ae-7cb770e0bcf8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bb1d887b-c380-417f-9951-7d4563d97686', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bb618605-bf94-46a2-8b6d-ef2e481f8567', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bb75e9d6-6b57-4d01-aa9d-e366af956249', 4)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bbc9f87e-3789-4220-9255-1a7498e92300', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bbe1cb28-8409-4204-8f76-c935166e4ad1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bbe2a7dd-cc63-40e0-8be5-b6168f8a7ff9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bc28a033-5c78-4e3d-ae57-7be9f26d8009', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bc4d6d93-14b7-4114-a16c-d33f9e6acf41', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bc6a951a-1e28-4360-b237-3f9e765e6143', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bc739cc9-1843-42cf-8124-c501dc352292', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bc9f1c5f-4683-4964-9fd2-f6258ee607c1', 1)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bca79139-bd61-48e2-8ae8-596b4a714e22', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bcbb95cb-f36a-4ec4-b52b-b779ba2ab6d5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bcec6a82-3280-4dfd-a643-6bb9f75ec95e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bcf2e883-5ee8-4a92-9229-cd8cca3e4803', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bcf6a487-af7e-4821-a47a-94183d39e1a8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd0e4f37-6a6b-4b1d-bcd7-412774887123', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd0f411b-19bb-4d9f-a4a4-1c7d87cf4d2e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd198063-b653-4778-b88a-00fc1f3c60e5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd2db588-35a0-4ef2-9bb4-850302b577f1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd66a47e-3d6b-4f1e-b303-abd32e554926', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd6ded86-6496-410c-99fe-68ad7f8fcda1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd763e2b-b3fd-4fda-9e2f-c3bc11843d20', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd7c9cdf-7b2a-418f-93ea-00ac61f0948b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bd8eb225-c7c7-452a-89ec-74dbdc682ad0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bda12b53-d4ef-4145-9583-f904e9bbf7e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bdc3d737-da30-48e0-aedc-bb438771ab36', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bdd6453a-10ed-45b6-8273-50c241bbd6b5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bddac423-9067-46f8-a09e-b4d8c039b196', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bde45a07-2a99-441d-9b8e-a892439dd8b5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be089201-f23c-4d4b-aa6b-f3432f73077b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be13378e-ae31-48e2-9136-1ab1937a701f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be20a035-655e-4236-b61c-ff7383870492', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be2b46df-8fa5-460d-b700-693dc0ff7d81', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be5a8962-3f96-4144-81ad-7a1d116fcd75', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be676238-cb4d-496e-924e-5b5683fb95f1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be749662-a4e1-47e3-9dcb-17b2bce3c85f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'be835392-80a6-4498-a2cf-be613b96814d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bee3aee2-1c3e-4104-ad21-722721e1228f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bf0818f2-9c3c-4133-9dc9-dc8adb893f6c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bf1e99a1-a5ef-4fc0-b62d-104fc3f2478c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bf24ce67-fc1a-400d-9dbd-4432e4c037e3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bf35d87e-aae7-4d02-966c-a740324cb831', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bf898510-b8af-49d6-b192-de020a2036fa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bf8a0e20-2bee-4e3e-96ed-447b98a918b8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bf9694e6-af6a-4dba-9842-5970ba8886c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bfa87778-00dd-4f54-82c2-9f0375955d25', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'bfbebc7e-6dd8-49b8-9b83-f0a770fa2f30', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c000e9da-9bd7-4c9a-9cf5-8b5c3706bea2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c007314d-77bc-4419-acf2-505b032e66de', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c00c10e8-755f-4ff1-99c2-8d7b01e8a200', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c06c6cd4-fdfd-4445-b296-0858f331dfcf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c071b249-2b47-405b-96d6-66b7427426b6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c071e393-1de1-4162-b56c-2ffa74dee986', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c083a69c-95b5-4039-8d64-b7685377662a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c0892348-4845-4b56-88e4-39999eab577e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c0a1d110-b9bb-4173-ae41-db1fd0b5c5a1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c0abb574-f6fb-4479-b04b-ab644e193355', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c0e13500-8e0d-4e74-a5f5-c7377b4c7d53', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c1090855-79de-4cbe-82aa-69ab00219384', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c1198f65-86a2-43e4-b2cc-6d60586e08b2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c1303837-ef99-427e-8459-9b2d03f34c79', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c15ee325-aad1-4f28-bd0f-c218fd437824', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c1747bb6-1aef-4594-aca8-7938db116b79', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c174e034-dd4b-4bb6-b68e-2dd4c9e6448c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c178730f-3863-4c0c-a01c-6200edeb1a8a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c1bae8bf-c50f-4a47-9df2-feef08ac4990', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c1bccc03-4a15-431a-83cc-1b14e9d18ff1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c1c06365-a417-42ce-b6c7-45f5cbbc2d8d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c21b0a6f-5405-46a7-a2ae-84cb0d5b9d39', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c23cdbb9-f8e4-4095-a250-13b6a201509a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c24b4b2c-e298-4331-a526-9a576c8ee0f9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c255efc5-ceab-484e-9dc8-95f720982700', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c2910962-58cd-410d-b09f-771774015cdf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c29dc5f9-3a8d-47b6-b95c-92695ecbeb71', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c30b4019-c662-41e2-ac0a-f6fe27802fa3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c33addec-cf43-4919-bb3f-593baa27eb6d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c34c5634-3daa-41d5-9323-0f4992a6de22', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c369625a-0459-47ee-8afd-4f7056a51945', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c37df7c6-364a-45e2-91e2-403976ac4290', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c3b89861-0053-489e-8d6e-759e855bdeb4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c3cdef78-2bcd-4aa1-a0f6-32102d0299f2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c3e37f4b-89fc-42a5-b6f4-a92c9f3ed14f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c410982d-2038-463e-a447-1dba8cf34929', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c415fd29-78ae-44a0-9f6e-129d8555c474', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c42dc30f-a5fa-4164-877a-d0f5ce88d2a1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c474b57c-88c8-4311-9275-5150b73117d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c494e7f1-55ac-412e-a6d6-b4bc30a585d9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c4ba671c-a016-4bbd-8a5c-8db69215638a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c4e7794e-b08d-43f4-aea3-7ae9fbbcc1da', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c50943ff-5b8a-4488-a63e-70774e80369f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c50edf81-ec11-49ac-ad9e-6956e12c94bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c51c59bd-7bc4-4fef-954d-07ca164476b4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c539caf1-996a-4839-84e0-7cb7fa427e93', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c566a76b-d1f3-49c4-977f-27cef375a34c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c57c5105-5c5a-4b6d-8cbb-904893c1b5f4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c5bc9756-9f48-4a5e-ab4d-aa9e58d75fdb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c5bdf44b-299d-4baa-adfc-7c6c868fe63c', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c5dc281c-da91-459a-97d4-fe62e0fd45ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c617593d-bdd3-45b2-97ff-14cea15bfe5e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c629b7ea-a2e5-4692-9785-17f6ada38047', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c648d1af-8ae9-429a-ac87-51a52ff2c90d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c64d3d65-f4bc-48ab-822f-e52bf235a9bf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c66ec27c-aa49-485a-9301-212937428c3e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c6b20d32-8331-4d51-95c4-b0a692065700', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c6b52251-5d61-406c-922c-ccd28d6870e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c6e38555-1a95-491b-ae6f-3aa070db6f41', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c715d33d-7e02-49cb-bb25-ba235fdc5d42', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c7613a77-8aea-4e6b-964a-8d1e40734a86', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c794de74-9f7c-44c1-93a9-bd6656f81f00', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c7e19b7f-0925-4396-abb3-54fd552172c6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c7e5a967-18e7-4a04-8c57-feac9c2800fa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c7efc0e8-31b9-41a0-946e-618fa8cbcfc2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c7f372de-f348-440b-9850-2ff019f69849', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c8050e86-635c-46fe-be41-d9910589501b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c819c9cf-04e0-4c38-98be-c6736f31bcfb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c81df318-3a35-48bd-a44f-595b1ec3cd67', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c84ec39f-d2e4-4bd4-a621-dcc8b58c465a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c8766367-07ba-4d31-a768-0a5902547b0e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c88cd177-f552-49b9-ba18-6f92e4446992', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c8968957-9dea-4b8b-9fe9-b6c53a0ce2dc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c89b4fdd-047b-4ca2-8c25-e3b93e3e4735', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c8b96f53-b87c-4572-8e69-9d749d8feb3e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c8f2a02c-5ef4-4cbd-af16-65a4af54ed9f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c8f36992-20c7-4f51-a4b8-0c2d3556e589', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c8fdcb47-f3d3-48d7-abde-72113881f2a4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c910c0f8-9489-447b-81de-99320bb770c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c928f3bf-78f6-4b0f-85ad-4f7dd69ec6a2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c9709f2f-36ce-4de0-a71c-95bee2bb7ec0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c9e88da4-de86-4f5a-b081-5041a750a5e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'c9f3e00d-af3f-4516-9045-042ca5f89eb7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ca0a53f0-0e5e-46c1-899b-2c73c7d2c6a9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ca0c5857-86f9-46bc-aa33-cd91696eed97', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ca11c377-6105-407c-a885-40b27a29f03e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ca575c54-861f-4fa8-8fe3-ca8780961a7d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ca74b2ac-d711-483a-823f-05248019a7b1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cae7ab3c-1ad1-4aeb-bdf2-d8239e9db7e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'caf88ddf-d11f-4fb6-b3fa-8d61738bd17e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cb066b4e-18d8-41c7-a5ab-202581dcaea8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cb0f06e3-acf6-45c2-8722-66545fb5b6f8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cb5eade0-267d-4c7f-82a0-71a59408162c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cb9a903f-c7fa-4497-8fce-fbd5bc9850ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cbb09822-3946-4571-98f0-4d20cbf37da1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cbc212f2-b056-4dfd-9972-ca40c0934b6e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cbf42d33-c8e4-4fa2-a736-70d636491fe1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cc15e945-aeaa-4e00-a6fd-b068bc88d4ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cc300a05-e8cc-455e-a3ee-af0176848ba0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cc5c48ea-5043-4a76-90cd-0c6269f7d9fd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cc8c1e33-9cd2-4dac-a9a3-7d4182cb7800', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ccb60b24-c005-40a1-bfae-78563ad3bcb0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ccb78e70-06fd-44ef-ae31-fc52f61d4318', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ccf22310-f944-42cf-8e72-9b6bda7b5469', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ccf6641b-a41b-4533-b470-a08356b7b4f0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ccf77434-32bb-4f17-8497-0c55c54ff447', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cd0ac114-61ce-4a9f-9833-00bc4b12f91f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cd0ca811-9ab9-4d0a-a5b3-58a16279e67c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cd1cfe7c-1f20-457a-8da9-0f54c42a9e0e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cd49512a-0ea3-405c-8310-c954e444b447', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cd791228-8bd5-4fe2-a7d9-f391bd0aaba6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cd8ec480-f0cd-4b71-9c03-3de6745980e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cda899da-cc61-4275-8cf3-6fe01acaafab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cdb3601c-c071-4306-adf3-230fd505bca9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cdd7efb4-4cb4-42ad-8efe-9542f12e6086', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cdf07793-aa33-4404-ac91-c558d35a9488', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cdfce7bb-ef46-45b7-ac32-6af9c9e4946c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ce3e9798-6526-41e0-a90b-884171a2aa42', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ce4b1c44-9bc3-43f0-a38d-44873e3d9bbb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ce72e2be-1848-42fb-a6bb-9eeddc4dc858', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ce951ef6-99ef-4961-a0a7-372b0608ddee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ceee2ae4-2b30-473e-8ca0-e0acd2379508', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cf0c0b00-10e0-4b4d-b0d4-4939ec647804', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cf1732b6-c9d5-4290-889b-7ac029764ea8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cf58c485-a3ef-4bb9-8723-2606a6658cd5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cf92b583-7dfa-4736-af05-c793870986dc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cfb1ab64-f024-4d8f-a883-91119bec0427', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cfb3f023-6348-4a3e-995f-a56a7109285a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cfe71d9a-78b3-404a-987f-19e5382e1b4d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cff0884f-36ae-410d-90a6-a3774f47fcd6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cff121f4-28a2-4f59-87ce-bf9acab632d0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'cffa267d-2e0d-433f-8197-81f54b851097', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd00dd89f-8508-404c-846c-69020028d0d4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd013273d-38d5-4072-b263-6663d0c9a5f8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd01d101c-c3a1-4e58-8a59-16439e959d71', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd028666d-dec4-4276-88d6-b2f2c849a9a7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd04321b2-9655-4db1-900c-3edfec16365d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd05fed8f-4548-46ca-b435-f2dab83c51d3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd065cce3-9006-4629-aea4-dcaa86c6015d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd07fda5b-8bde-4d9c-8c29-35cd41e73c54', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd08561fb-4049-457b-a9dd-e5d7b7897289', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd0b82d97-339b-4e30-853e-028e5f9144f9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd0ba2507-4b5f-4dc1-8e3d-cdcf14120799', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd0c5fcb7-35d7-4c4b-8ac1-1dbb0f8fa105', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd0fc903a-e1ed-479d-b3b6-1bf98122e9ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd11cbbc3-389b-4385-b117-6f006b807ca4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd19b8435-e085-4797-889a-8b8626ced596', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd1a45cdb-3aa9-400e-a379-86f770f7433a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd1c3bfee-8432-40d7-a9b1-71ff03d207e0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd1c5a171-8534-4ea0-9be9-c98de0236d08', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd1ec7f03-427b-4bb4-9c34-34bc33567b8d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd1fdc1bf-ae6c-4fde-932d-79b91b249264', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd22d24ab-9ad4-435a-b08a-5f93f0a4caea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd24d6b89-adfa-486f-85f4-63d072d9561c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd25e0128-68c0-4ff5-baf2-d2b3fc2e9099', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd2682c8e-8b43-49ba-a226-954b8af76a29', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd284bfbb-e03c-4be4-ae91-2a824ccf7158', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd28fbf07-4a27-4258-b78d-1e7a789f7e21', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd292f50e-a118-4f0b-a5da-e98362b413bd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd299de3e-5078-4342-ac45-144ac253add2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd2ad8703-af1c-4fe3-867e-609ec2aaec72', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd2ae0fee-cf0d-4bd2-a9e7-604facca541e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd2bd1e6b-12db-47b7-bd34-f0d938f1f5e7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd2cedbc0-e96c-49ef-a23f-1cc5e4f842b1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd2dd4ca4-536a-45de-bcd5-6b27bbebb9a7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd2e5778d-d776-4b3e-afe3-64bd0cd2ab1b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd32a7cb2-e7d3-4e80-8d9b-849acf9005bc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd33177fb-ec4e-419c-99ce-2aae9ef7ec6e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd332a4e6-5189-48c5-8e19-422a8d1612ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd333ac59-2414-471c-bcd6-eb17359df1c2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd34667e5-f53e-48d8-b8d1-db0c7cf719ca', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd3534267-c054-4f22-8824-12c1b4cf23af', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd356ede0-9894-4cf1-b6f0-0a77063df385', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd377451c-05b2-4085-8a0a-ee30cfeb384a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd37d55c6-3680-48b0-9ef2-314c7d581d5a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd3b592e4-7645-43a4-bcf6-bff4fdd73b30', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd3d0d8d0-b7da-4664-bdcf-b6d4e6f43a15', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd3d1fc5f-60c5-4ebe-970a-9270685a9f64', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd3d35bfa-9e6d-40b2-b6cb-fb87da2cd2ba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd42c452d-c44c-4c54-96f3-27642925abc5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd488afdf-ee33-4e42-a775-5375b40bee89', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd4b16cda-67f2-4ebb-adcf-b573b834b8a4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd4df45c0-0e59-4301-8165-14e2e9021aef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd4faf987-f8ae-4aca-8bf2-12e7a9b427a5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd4fb7648-c61d-4f09-92fb-8332ab730d0d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd53b7183-463d-4e53-a90f-28b0ae5b7f74', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd569d0b1-236b-484e-aaa1-192bcc17b56e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd5722698-26dc-489d-b3ae-2b70d335a3d5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd5758777-622f-4694-8d57-e1f7d86abb30', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd576a81f-70a2-4f76-82fd-d12ff61d00a3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd57cbe50-77c4-4e21-aec7-e6eebad22a9c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd5a274d0-1ba7-4707-8ddd-5bb68a0d7e85', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd5c50671-39fa-4e08-b1ec-d5b7c8011069', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd5dad615-483b-4c49-8768-fbd9b299cc7c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd6119fca-4748-4e6d-9fd3-9b17121e8973', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd62b7245-2e95-4c99-b297-02b8c0e453c9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd62c04b4-6186-4c51-92b7-39e134a60d6c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd64f4351-0fbe-41bf-acdf-cae7c13358ff', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd6872ded-a920-433a-ae07-5a55252c8c75', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd68e9a71-df4a-46c9-bb0e-83c2d36d605a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd6c47905-58e9-4653-9941-1042de577585', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd6cb44a3-a4e9-48cb-8ab5-bb388d1961ce', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd6d79286-f915-4034-9a6b-efab88589d6e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd6faa5c9-c42b-4d96-a3ba-b7ca23fa2ae4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd728932f-d649-4fd5-97d7-f2407794ef46', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd753b2f1-2857-4afc-bf60-6f513597f15f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd77190d4-c6b5-44a7-b2b8-933c543f5837', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd7727f26-4a97-4b2b-bdd7-54dbd0edefb3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd7a1cf69-431a-4ee9-ad43-f70111f5e5e3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd7b851ff-4e03-4329-81bf-c47ca45aef9a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd7f1d131-d078-4da2-9d5d-d2eb5152cb06', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd866d32b-5f68-4feb-9388-3b5ebf9efaea', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd8768d67-2d2d-48b6-83d8-30cede04b014', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd88418f2-c2c2-439d-9b53-0b1f2149d4fc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd8fdf878-133b-4696-8a2f-10e4c28d376f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd9252f33-d9b1-425f-84db-13cd01bd3d18', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd937d39c-6d58-4af7-9cd3-535ffa86e29b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd98e4e70-1735-4b13-a467-898f0464cb1b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd99fb515-f494-4cd8-a9a7-7ffa0c55c451', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd9c53d10-d330-4822-ab63-5a4a8c725255', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd9d28cd5-f987-4c8e-ac9c-3b5f4b5f90d1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'd9f70bdd-55ff-43cd-890d-908a80d7e389', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'da0d92d4-578b-4c80-8f41-a51f31f2ca98', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'da72f48e-1f28-4a4b-a71d-e7c4bc2c9f9c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'daa46f54-b02c-4a5b-bca6-547648f49d4e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dac2bad1-477a-4c2d-819e-2c508d021adb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'daccd87b-b6a5-4a1d-baa5-af35f444f2a6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dadc0b1f-57a5-47a1-bfdf-529888f28a4a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'db193308-f77f-4933-a2cd-a28698c76980', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'db309e40-e66e-4b6b-9c40-1ebe46e85e75', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'db4c3bce-24f3-445f-80cd-3d257e83ad9f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'db5efda3-65f6-4494-a995-ee850781c922', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dbaadfc5-1ad2-49a4-ab57-5468edbad23c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dbc1d3ea-5052-41d7-8142-e02018490b92', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dbf8fefd-f3c6-49ff-b529-b7a7736adfaa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc05cbfd-5228-4164-b776-5006744391d0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc43cdf3-0177-4c50-83c1-ea52a87b9e48', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc454cce-3fc6-4aa6-afd2-9b48a7d74932', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc701df3-7ac5-4458-ade9-e5453d49462b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc78f855-3e24-4efe-8d50-ec90549daee9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc8943cb-b155-4449-809a-731f05ad5d2e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc8eaa21-e643-4b9d-b3cf-31ba8f878051', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dc921025-c9a7-4e15-9bfc-03efa07cf9d5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dcf97d18-2b1e-4058-9049-5fac901846d5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dd2bbff9-c578-4735-8241-ee4a5714cbfa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'dda544ce-26fc-4db8-9f27-e5ee40b17946', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ddab2c8c-a72e-486c-bec9-dceb657961bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ddc94de8-c5bb-4097-9ee9-e69417f489bb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ddccb3d3-6a34-4d66-97fd-dc265b2d8905', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ddcd2d7e-5c64-491c-83d6-5d2076932572', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'de1ea41c-7a7a-4951-bff6-b08cc1d474e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'de2c8c45-d0a7-4955-b12a-d17e9c01c8ee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'de486d9e-7ee4-42c8-98bc-e70ef576d219', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'de55862a-91c2-418f-917c-182c21b04494', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'de973d48-b6ee-4a46-a312-8a74ee8ec9eb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'debbd1b4-f1f6-444d-b81c-c6da53606aad', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'def2482d-6e10-4c0d-8a22-3a8061395dac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'def4e9f8-93b1-4f16-b271-a32aca9b9462', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'df2a2f2e-45aa-4ba4-9aad-aa37bf3c749a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'df54995f-22b4-491f-ac7b-1c397d0c8b33', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'df5ee2fb-0d06-4010-a9ff-42fd0b85026a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'df8c7da1-3b2e-4374-a522-99508463ed5b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e019da3e-7d59-4a6c-b098-b074ef61649f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e0724303-bc05-45e0-ab06-b137bbe761c7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e08eefbe-6360-4eb4-a542-f1c147821342', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e0ac0cf7-9c04-4493-9f5d-82be7941e311', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e0b25ad5-1355-4fb4-b9de-b4537be317d1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e0cb25b6-fed2-4a7e-a2f6-b0be34d566b7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e0ec01af-18f3-41ce-851e-71882b0ff87a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e0ed8871-2a5a-4adb-af16-a5aafc4faf06', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e14b247d-0c09-478f-9712-b0e5ffda5609', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e15c40f2-ad11-43e6-951f-01717a64b887', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e16216b1-a20c-48e2-954d-0ce743fc65e1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e1bcfa6f-2df5-4f7f-b6f0-1906aaa0649c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e1cc86b8-4164-49ee-a2cc-a6c4d4534c7b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e1d07708-63c8-405e-96e0-cf85ddb30de8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e1eaab53-563d-4961-962e-7e519be5fe6b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e2041636-a2e8-453d-bfbe-a1e77fd2b777', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e20f566f-62be-4e17-bcae-505f81b7bea0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e214956e-f911-4fdb-8c00-6a5b4637f73c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e2450092-f43a-4655-9b1f-2966e61a8ac2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e25c5b86-1105-4117-8df0-ee021db571f1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e2737c1b-335f-4537-9c7d-9612b453195c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e274a021-9efb-4281-bb46-f1f76ada6cdc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e27a6dfe-3191-4947-95af-af6fc32e8a70', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e28776c3-8df7-4dab-80fa-fe68c9f3c82f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e2c4635d-bf28-4069-9c81-570c4011cc4d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e2c5bf61-718f-4420-9f9f-d66f6519eef2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e2d9bad4-0e59-412b-85f6-78cd0d5cc4e8', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3246455-a155-45dd-a0ba-de0f85ba8f95', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e34efb5c-3e28-4e5a-8ac6-67f6fe6377e2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e35ac4dc-276d-413a-977c-debece800b18', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e35ce484-88e2-4ff6-8f72-a4a126ae18df', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3719eac-4620-4323-b4d9-7e9ebc92aefa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e375388c-e6fa-4c23-868b-4889dcb0b0ef', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3853f1d-4c95-4235-8552-7e03bfe1a1e3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3876245-3c74-429e-a3ab-cff1228fe191', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e38ac3b3-f2f3-4340-a4c0-2d1ab57e3aba', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3b0cd61-1962-4200-82f9-01c24ad1b960', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3b575f1-02bd-473f-b337-b7a027e9e0d5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3ebb6db-b0a8-4916-91d8-60f57aeb68a1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e3f5120a-c229-460c-b90d-992b8ae3bba8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e436bedf-1ff6-403f-8807-ec96eebe9088', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e460312e-6221-4dd5-b5ae-77fe28d7a333', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e48f2edc-968b-457b-9270-49695bc4597b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e4c96538-0f27-4ebe-9350-747f0d0ea2fe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e4ebab8b-14c2-42c3-86b3-b8450244be61', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e544258c-039e-4e86-8019-2c9b75a7f472', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e587f541-539f-4617-ae67-11825c2dec3c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e5a9f0c6-9cf1-4297-ab8c-cb01519e3f44', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e5b58121-7796-4758-921e-b805c3ec94b9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e5c1f8f2-99eb-46b2-9b95-43b0a124bbc7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e5dc247a-9679-4588-8309-2b34ec6d80a7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e5f54824-b13a-4df7-becf-f008c0380b62', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e61bd281-d5bb-4465-8f61-958bca489358', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e6305999-e457-4fa2-b773-c9c759c673e6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e65fa1af-2865-4eea-80f2-f4621acaa5cf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e66bc8ec-4265-4656-99fd-51d5f1defb43', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e671e525-b12c-4262-bf48-a097c795367a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e698be54-8310-4493-9fce-43d81e47cb4e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e6af1254-9bcb-4177-98b8-9056181cc1e4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e6c0ec65-bacc-4c00-83d9-851f0a905da1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e6c85c95-7d1b-44a8-8ab5-0f3c35636bee', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e747f81e-0d0b-4f1e-a75c-ee58313931cb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e74fb973-9783-4ded-943f-7154591547f0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e75a1278-0318-4fe5-a4e1-d59cab930f13', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e7a560d5-20f4-40a1-848c-83c099c19f10', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e7bd37ae-2eba-4ea8-b60d-9b4c5ed2a7a0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e7ca14cb-827a-44f0-afc1-8cb5b878cd9f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e7e52145-2191-4969-adc0-f311b8b156bc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e800febf-e4c7-4943-a686-9ef38e1a3b59', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e80e406a-959b-4c8e-967a-d90e846e0310', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e82cdb7f-1cc9-4633-a281-9b50c4cc0674', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e856ed3a-47dd-4a23-8e1f-d18db2aa304f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e8807be4-f615-424e-9930-33421a0f9e17', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e8876955-5c12-468f-9545-4b486c5eba1a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e8ae1d55-8955-4f9f-ab07-86ae5cc38e6c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e8e1f26b-cd6d-49e7-bba5-726343a87be3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e8ec1963-9914-49ca-ab22-4c2c38d830e1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e916620b-539b-4a4c-addd-e20997b5a277', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e94eedc5-ec3b-4930-9658-5c49954528be', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e992cca4-6c95-4ba7-a3d7-97913d820faf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e9bb8417-cec8-483e-9b9a-3f4c3e82bd17', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e9cb817c-6e60-4ccd-ac64-adf52cc4ca90', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e9d559b1-5c05-45d4-b2be-bc98137b2989', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e9ef5d93-0ff2-421f-b18d-c078cb827138', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'e9f1e990-2eb8-4d2a-ae3d-b82e623d5cae', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ea10e8d0-6f9c-4601-ad0c-591dadc86409', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ea234cc5-b7ad-4aad-9355-247fd9f3871c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ea5457e8-106b-4595-b660-e0e806aea35a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ea928fe9-adb0-462f-8536-a6fb297600c7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eaaa585c-4fcc-48e9-b775-33bd348905a4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eacfd83e-10a6-497d-883c-1485cb5ef501', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eadab71f-442d-4599-8e36-445abeea3f56', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eadd3348-09bf-4971-b051-98ab627843e5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eaef01b7-64ec-4166-9918-cd5cd358eb06', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eafd4665-213a-4a5f-8f2e-66ef8c68a9dc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eb52355e-b8bb-4331-9248-79c9d0bd3464', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eb5311e2-b650-4f86-83ea-5c194511170f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eb89b247-2116-4860-a1c2-77a5321318db', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ebc72c05-02cc-4c7f-ab6c-e95d46f9a53e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ebc870da-8b42-4a54-80d6-2d6350c2b3ab', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ebca1e86-16a7-4065-a337-33eab070ed53', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ebfad227-512d-4a9b-89b1-feb3b3683d65', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ebfbf736-0a55-4e8e-aabb-c1b68650a11a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ec02cfd2-045d-4fa8-9c34-7d8fd32a92b9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ec37aaf5-932a-41ab-9c05-77e1dfa18cfd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ec37d363-8d08-4a0f-bbf1-a40779f4f59f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ec481520-9ab5-4109-839f-c75881de9e23', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ec89d74c-9a6a-45c8-942f-1d851342e6ff', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eca65a32-1c87-4772-b5fb-547d6ea7bdf0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ecaea193-9544-447d-8691-f7970203cce5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ecb90fca-9b41-4680-9b2a-60bd10753abf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ecea6895-a9ab-4c68-88c7-9105ce59a408', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ecfce968-ce43-451a-ba51-7de3dac8e25b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ecfdd9de-d778-4c0f-a92f-515378f49ad4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed06b43f-e9ec-49cc-b920-099a0f2e2f1e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed388b2c-464d-4411-bacd-c13c32600eaa', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed589b92-08eb-497c-b562-4ee9c8aba9ac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed59e414-64af-4d0a-88a5-51432b6d41c4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed7c1047-96a2-44b4-914d-4a21312cd354', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed7e3844-9e70-43d4-b57a-f6d9b79f2a57', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed9c937f-849a-4564-9d8f-5d479cbb5ff0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ed9d1850-9990-4dcc-953c-455f0215c93b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'edb84458-d50d-4ae2-ba92-b7d4731c0967', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'edcb97e3-caac-4b2a-bd60-6a5b54179594', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'edfb2477-2c20-483a-bf9a-0cdd85e289d9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ee3b7ce9-50a4-4fbf-89cc-608fa7eeaf8a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ee6f4cf6-e685-47ad-8812-bc8ac2566e94', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eea49e0c-90a6-40b1-b824-217b6b8cd4fe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eea5a153-4793-4ef6-972a-5e0c5407f7b3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eeb313c8-14de-4cb4-8d0d-758e1409c212', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eecc1bc2-7e0b-4582-b9d3-f57ebaa7d743', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eed32785-6ca3-4e91-955a-daefe63fa963', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eedeea3b-9187-462d-928d-c32bce979964', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ef26b1d7-4459-4fcb-b71d-85994e043078', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ef431a0e-0c61-42a1-ab95-ac606f116311', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ef5d784b-9469-4d50-b47e-b5fc95d26ace', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ef8f56c6-cf83-43a5-9c6c-cc3fdf3c9765', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ef8f76a5-20da-4854-b04f-fe2bb8b9621e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ef9c093c-ef02-48dd-bfe1-94389da7b724', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'efc0c091-f520-4474-a91a-3d44038c1863', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'efccf0dc-2cb5-4cf9-9081-e2eb79bc4892', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'eff364e4-6dbe-490d-a8a4-fc105964bcaf', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f0096d04-5631-4db4-91ea-0110dbdd1558', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f01ecfcc-97ff-4534-8322-4e250a219f50', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f04e7590-0521-41e0-b470-51a7d3e1d750', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f0533ac5-e39a-4b5b-b433-10e97a42ee3e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f078fcb6-fdb3-435d-affc-9f0c99c17cd7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f0984f01-ff62-4dfd-bb02-a088052fda51', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f098fc81-ea56-40ca-af2e-08198d62a215', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f0d1f5bc-1173-4749-a98f-a9f9f8cdb27d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f0e1b5a0-a487-46a5-a33a-1022361a781c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f0fa65a6-a05c-460c-a06d-e77ae7bbe099', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f100bdae-7920-450e-8dba-85c41a1da302', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f10b9388-efeb-4ce9-b69b-c163f0d9ff2d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f1126fea-e1ed-41d3-a198-bcb8d66308fc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f1745cc9-4742-4f07-87f8-3a9ffb42ecb7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f194e4dc-acb3-4910-88e4-c72b0a9217fd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f198099a-f2c5-463f-8a02-5d532eedc286', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f1bddbc8-8782-4f38-b716-076a9561ed62', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f1d3c68c-b7e1-431d-89ea-e5bb426aa29c', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f202d49c-eabf-4a81-8c50-84a25fcc296a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f2060f4f-1323-44c3-b7a7-243cccfceb9f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f20b4fdb-21d6-4698-b5a4-9f1b5748f200', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f2278c6c-39b6-44f1-902c-30136c1d6255', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f23806c3-1e4c-40cc-8431-f29fc5832e65', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f2476472-5208-44bd-9c74-88903cd4c458', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f24c5715-d78f-4662-8656-ee96d0175e07', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f27415cb-1d53-4254-9f64-852a7346cb41', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f27e4afb-d729-493f-aaa3-bce61a6deb12', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f2ab6935-affb-476c-b093-98bffd6449c8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f2f1b80a-ef3d-4d62-94bc-d723818b7870', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f319f5cc-afb4-46c4-b904-ab97f04e1ea4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f3423615-aa0c-49fc-a2af-9f44cefbe5d1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f38e3d3b-5e7d-4996-8274-a57d3a5e56c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f3911542-dcc8-494c-80d1-b2e16b4c63f7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f3c0badc-fade-46cf-b46d-68b91754ed3c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f3c31096-f391-4101-aebe-83605c5bfd4f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f3dedb93-f800-40d1-9cdc-9a64e3535f13', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f3e713ab-32d0-418c-b1c6-a896304ef69a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f3f286f9-8a57-48a3-a27d-713a6610c9d6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f40d48d5-eb71-4cde-b72b-1ebf4cf84ba9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f42ae7c1-2289-4329-9861-556bb7ab9ff8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f436e449-b66d-445c-8f97-8b03ed1676de', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f453887f-4e7c-4665-a41f-a86e2785b403', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f465f31f-51b6-4282-b5a5-a9a496abb3f6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f517a708-39f7-48ab-ab9f-e067d55c7afe', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f532a3f2-c771-4497-b206-a42a13e565c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f5382d73-6ca1-443f-9cc3-c9d3a3974997', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f55b544f-416e-4e58-b410-c36797bd2fed', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f57889a8-0391-4b81-81e9-bb6e43584e4b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f589a6cd-7fcc-449f-95b2-7e01d920fe15', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f5b8f0b4-fb84-42b4-a106-f6f00f3cfaa0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f5bce1ec-8e4e-4d5e-ad18-d03895f4451b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f5d38847-fe65-4a90-aa55-22b9325cf2d4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f5df4cc3-1584-4638-8030-8205f3fc17a2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f5e9d986-e411-4d99-858c-bf4cd0738de1', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f5eaa210-188c-4c36-91ab-fb423cadaa1c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f6054d3b-f03d-4dd4-851a-e6cd60c0374e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f64a19d2-0598-4121-9f3c-b5d3c1669b41', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f6a0501f-bf25-4ae4-91cf-e80a6536cca3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f6a732c6-1d68-4872-8b77-f66150d28e74', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f6dcd2bf-2207-4d64-8e2a-1dd451b2707e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f6f75c51-bc6f-4ffd-9859-42752e464d62', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f754e036-fee5-4c34-adcd-9e1e75821cc5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f76f582a-93bc-428f-bee9-899a47e5fed4', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f798f773-d471-480b-b3d4-4b7979dd016f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f7aaadee-145a-4330-b858-430fe1a9fb69', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f7cb15cc-fa32-4b6e-ac95-c963941cb3a7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f814c40d-463e-479c-9df6-f714ce18aa4c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f822056e-8050-49e3-9534-77c2facf5e5a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f8413095-9215-4d96-a8b1-11ac8b787061', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f8533285-4610-42f8-890d-8057c3bc6cbd', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f870a754-cb32-401e-85f9-aa414c924f70', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f87127b6-dc0d-40ba-9366-0e16fd35142d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f8730c95-a154-4de4-a9f1-ca66f80aeb9a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f87317d3-1496-45d9-b784-09adbdd4a1e8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f88634f1-fd84-49f0-95d8-be1b7127296f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f888434b-4023-40fb-9504-b05286425f5c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f913a399-422a-422a-988c-a9296b830b5c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f920c3c1-2c3c-4f68-b627-0eaca2da641e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f929db0d-91cc-4c85-854e-5168d96e37c0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f9490e91-5d42-46a8-b924-c548207aee8d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f98686ae-e1c1-456b-89f0-f58ed1c69fe3', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'f9b03063-7fc1-4392-9382-0631157b0291', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa0a1690-11bd-4aef-9cf3-60ff729b698c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa310595-7ca6-4615-b1ca-298b7ab472d9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa3d44dc-2af6-4dc9-bb14-b21e8629b326', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa442b79-b3cf-4f07-9d70-66d0feabfe31', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa4447ba-1e8f-435d-bf5e-53d3b01d0fcc', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa5ce2a9-80ba-404c-92e4-a49b3b76b423', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa5f309c-73cc-4dc5-9c1d-b28f279c077e', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa617e49-1316-4f31-ac93-9a3af54c32e6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fa73bd66-c7ec-4f17-97df-704f552ed16a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fac364af-8863-4447-9234-b2eb554c7825', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fad5fd8f-1540-4607-b66a-88d8f6f21a49', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fade7586-7570-4a62-b1b0-7fa1d5d3ff1c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'faf65e0a-3553-42b1-8a6f-abfe486be5be', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'faff2892-baca-45d2-a380-b5240d215b2f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fb3c4d45-526c-4292-8012-dac193ed19ed', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fb9f9c59-a8b6-4098-b575-26162b51fc9b', 9)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fbb903ea-f467-42e8-8257-4fba39cdcb73', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fbea4ab4-75ef-46ae-a938-264072ce4514', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fbf175da-6ef8-41cf-958b-bb66854bd512', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fc283f4a-6264-48a2-a1b3-7464057fca88', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fc2f893e-c746-4fa3-9281-473de4010cd7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fc858131-9d76-4220-80ae-252b8ec19b3f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fca66056-f78a-45b9-9013-36236cf17d8a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd113b90-b99d-404c-b028-3f34806dd06f', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd1f26f5-b978-4ece-b391-7336cd5fdd09', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd3594fb-ad63-4218-95ee-8ce7b6d01a57', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd419655-e4ff-4d2c-8a37-387d36755353', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd780af7-e85a-4caf-a6b6-49568b546268', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd822c8c-3903-440b-b8f5-b19291ddb3d8', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd8715d4-594f-431f-9568-3fffc713c435', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd96fc18-f075-46c4-958f-551dd639de78', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fd994bff-7288-41a2-b869-afe3bb0490b5', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fdab8aa3-194d-4f9c-ae45-c97481b9e4e9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fdbbad31-21ce-4dc9-948c-c6bdd2192be0', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fe06537d-68eb-422f-95e2-4650c0becbd7', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fe0671c9-f914-4afd-99eb-13b9df5823c3', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fe41708b-06fe-4baa-98b6-57dbbd92709d', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fe64281f-e040-44d9-9847-064f487d6c5b', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fe741568-6c42-4f51-865c-d56893d3da46', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fe97dc44-ceb7-43f2-9e11-872d1da9a244', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'feb3f443-93ef-4ee5-8164-361ce7a33de2', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fecc3769-d229-4009-a4f0-6e5808961f42', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fee73381-3081-4862-9712-43aa17d1f1d6', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ff194208-6db5-48ae-880a-c81e04b57d93', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ff1f5bb5-068c-48a3-b52e-2dbc26fc026c', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ff57edb1-5b40-4cc1-b170-2017a4d53da9', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ff80eb22-36ae-4324-b364-7ec1d48ad9ac', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ff9a7943-d6d0-475a-99e9-11899b990c7a', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'ffc65ad6-20d9-4829-a8dc-2c091653bffb', 2)
GO
INSERT [dbo].[ApplicationUserGroups] ([UserId], [GroupId]) VALUES (N'fffd8f3c-024b-477a-a9e6-b8f458fba7e4', 2)
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'0e6a2caf-6b7f-4976-a165-f902e7bc7c14', N'retefuente.admin', N'Generar Certificados de Ingresos y Retenciones Masivos', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'15f128b4-301c-4918-b2b0-7a1af93761a3', N'consultas.vacaciones', N'Consulta  y Solicitud de Vacaciones', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'1habch12-1icv-094d-SNCD-Ahchbnsnm2', N'consultas.cumpleaños', N'Acceso al modulo de cumpleaños', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'2700343d-e885-4b56-bbe2-f838b1ce4f02', N'consultas.seguridadSocial', N'Consulta  de Fondos de Seguridad Social', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'2c26s6a1-9s25-9111-ABCD-a444ssc5dc', N'descargar.politicas', N'Descargar las politicas de la empresa', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'30007571-999d-4eau-a12b-3c4e9421fbd', N'certificados.otroscert', N'Acceso al modulo de otros certificados', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'39337578-119d-4e52-a0ab-3c4e9079fbd0', N'certificados.certlab', N'Generar Certificado Laboral', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'492015e4-d5b5-4461-98ea-25f5d0ff8394', N'consultas.hojavida', N'Consulta  y Modificación de Hoja de Vida', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'59e8428d-8186-4096-9340-82102520bdfc', N'certificados', N'Acceso al Modulo de Certificados y Comprobante', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'69a5152f-2420-4056-a179-a78a355d0ee7', N'certlaboral.admin', N'Generar Certificados Laboral Masivos', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'6f57b492-b919-438c-b52e-effe182d2602', N'compropago.admin', N'Generar Comprobantes de Pago Masivos', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'7237fxd8-as1d-6343-aas1-a42gjsdacr3c', N'reporte.vacaciones', N'Permite ver el reporte de vacaciones', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'732ca543-4d77-4de2-980c-b0d3ed1ecbb8', N'retefuente.config', N'Configuración de Certificados de Ingresos y Retenciones', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'7337fxd8-459d-62c2-a1ab-a4gjsd4c5dc', N'consultas.licencias', N'Acceso al modulo de incapacidades', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'7705535b-ed70-4259-a9a9-5545712dbee5', N'aprobar', N'Aprobar Solicitudes de Vacaciones y Cesantias', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'7c1e7880-947b-4c5b-87da-e3222ba053b4', N'consultas', N'Acceso al Modulo de Consultas y Solicitudes', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'7d0ae81a-6009-4cfc-8ea4-1104bdf7adcf', N'compropago.config', N'Configuración de Comprobantes de Pago', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'7f2ea269-4074-4530-8371-18e1fca8c5c2', N'consultas.histosalario', N'Consulta de Historia Salarial', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'91dc4721-c079-41c0-8bd4-b248c20e0939', N'certificados.compropago', N'Generar Comprobante de Pago', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'93375728-459d-44c2-a0ab-a4gjsd4c5dc', N'consultas.incapacidades', N'Acceso al modulo de incapacidades', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'976876ab-94d7-403f-b615-7faa6daa5577', N'certlaboral.config', N'Configuración de Certificados Laborales', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'a52483bb-5118-42b2-848a-d846a936b35a', N'consultas.cesantias', N'Consulta  y Solicitud de Cesantias', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'abd25157-ab95-4000-ascd-28d3ff910d49', N'nom_electronica', N'Acceso al modulo de nomina electronica', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'abd25157-ab95-46ee-89cd-28d3ff910d49', N'settings', N'Acceso al Modulo de Configuración del Sistema', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'e41eed8b-ef28-4002-97a5-c74ffe23dde4', N'settings.permissions', N'Acceso al Modulo de Configuración de Permisos', N'ApplicationRole')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [Description], [Discriminator]) VALUES (N'e5494def-50dd-4243-86f3-f95d428ea92f', N'certificados.retefuente', N'Generar Certificado de Ingresos y Retenciones', N'ApplicationRole')
GO

CREATE TABLE [dbo].[USUARIOS_WEB](
	[Usuario] [char](20) NULL,
	[Clave] [varchar](20) NOT NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[IdentityUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.IdentityUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdentityUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[IdentityUserClaims] CHECK CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id]
GO


delete AspNetUsers
IF NOT EXISTS (select * from AspNetUsers WHERE UserName='Admin') BEGIN INSERT INTO AspNetUsers VALUES ('bc9f1c5f-4683-4964-9fd2-f6258ee607c1','NULL','0','AOxLNme1r1Xkfi1hKCBKvZlvDtpKLkwpwUjcqOQ4nOqSv4EqLimhp6gFDT//jmTRiA==','b128c21e-9a2c-4ad7-9de6-99c41aa05d94','','0','0','','0','0','Admin','Administrador','DefaultConnection','ApplicationUser') END

UPDATE PARAMETROS SET Usuario = 'julianaweb@systemsltda.com', Servidor = 'mail.systemsltda.com', Clave = 'Juliana2002', Remitente = 'julianaweb@systemsltda.com'


INSERT [dbo].[EMPLEADOS] ([Cod_Empleado], [Empleado], [PNombre], [SNombre], [PApellido], [SApellido], [Tip_Documento], [Cedula], [Seguro_Soc], [Fec_Nacimiento], [Est_Civil], [Sexo], [Direccion], [Estado], [Telefono], [Celular], [Lib_Militar], [Distrito], [Dir_Elec], [Cod_Profesion], [Tipo_Contrato], [Dias_Contrato], [Periodo_Prueba], [Tipo_Salario], [Horas_Acumuladas], [Fec_Ingreso], [Fec_Salario], [Salario], [MetodoRetefuente], [Val_Retefuente], [Por_Retefuente], [Tipo_Retefuente], [Deducible], [DedSaludEdu], [DedAFC], [DedAlimentacion], [Regimen], [Fec_Retiro], [Val_Liq_Contrato], [FormaPago], [Sabado], [Cod_Ciudad], [Cod_CajaCompensacion], [Clasificacion_Contable], [Tipo_Cta], [Num_Cta], [Banco], [Cod_Zona], [Cod_Sucursal], [Cod_Ccostos], [Cod_Depto], [Cod_Cargo], [Cod_Grupo], [Cod_Arp], [Cod_Eps], [Cod_Afp], [Cod_Afc], [Cod_Escala], [Cod_Tarifa_Plena], [Modo_Costo_Hora], [Cod_Empleador], [Aux_Transporte], [Cod_Pais_Nacimiento], [Cod_Pais_Nacionalidad], [Fec_Vence_Visa], [Porc_Anticipo], [Porc_Anticipo_Prima], [ModoSalario], [AnticipoPrima], [Cod_NvaEps], [Cod_NvaAfp], [Cod_NvaAfc], [Fec_CamEps], [Fec_CamAfp], [Fec_CamAfc], [Tiempo_C], [Clave], [Saldo_Base_Aporte], [Base_Aporte], [Saldo_Dias_Aporte], [Dias_Aporte], [No_Hijos], [LiqNomina], [Estudiante], [Practicante], [MedicinaPrepagada], [Telefono1], [Porc_Embargo], [Saldo_Ini_Horas], [DiasVacAno], [Cod_Causal_Retiro], [Cod_Lugar_Expedicion], [DiasVacPendientesDisfrute], [DiasSalud], [FecLimiteDeducible], [Fec_Cambio_Tipo_Salario], [GrupoSanguineo], [Cod_Colaborador], [Cod_Barrio], [Cod_Jefe], [Declarante], [VigenciaDependientes], [DiasVivienda], [DiasPrepagada], [DiasDependientes], [FecSubstitucionPatronal], [Cod_Alterno], [Cod_GrupoDif], [Tipo_Pension], [Tipo_Pensionado], [Pension_Compartida], [Cod_Agencia], [Regretted], [NoBeneficio558], [Regreted], [CodPractica], [Transfer], [NumHorasMes], [NomCliente]) VALUES (634, N'SARMIENTO BOHORQUEZ MARYURY (R)              ', N'MARYURY             ', N'                              ', N'SARMIENTO           ', N'BOHORQUEZ                     ', N'C', N'53050484    ', N'            ', N'19831113', N'1', N'F', N'TRAV 85 Nº 24C - 59                          ', N'R', N'7522029                  ', N'3144044011    ', N'            ', N'   ', N'                                             ', 1, N'3', 0, 0, N'1', CAST(0 AS Numeric(18, 0)), N'20090309', N'20100101', 515000, N'2', 0, 0, N'1', 0, 0, 0, 0, N'1', N'20100311', 141625, N'2', N'1', 591, 0, N'1', N'2', N'006270491977        ', N'051            ', 1, 2, 1, 21, 66, 1, 1, 1, 0, 0, 0, 0, N' ', 19, N'N', 1, 1, N'        ', 0, 0, N'F', N'S', 0, 0, 0, N'        ', N'        ', N'        ', N'1', N'          ', 0, 0, 0, 0, 0, 0, N'S', N'S', N'N', N'            ', 0, 0, 15, 7, 591, 0, 0, N'        ', N'        ', N'                    ', N'                    ', 0, 0, N' ', N'        ', 0, 0, 0, N'        ', 0, 1, N'  ', N' ', N'1', 1, N' ', N' ', N' ', N'                                                  ', N' ', N'     ', N'                                                                                                                                                      ')
GO
INSERT [dbo].[EMPLEADOS] ([Cod_Empleado], [Empleado], [PNombre], [SNombre], [PApellido], [SApellido], [Tip_Documento], [Cedula], [Seguro_Soc], [Fec_Nacimiento], [Est_Civil], [Sexo], [Direccion], [Estado], [Telefono], [Celular], [Lib_Militar], [Distrito], [Dir_Elec], [Cod_Profesion], [Tipo_Contrato], [Dias_Contrato], [Periodo_Prueba], [Tipo_Salario], [Horas_Acumuladas], [Fec_Ingreso], [Fec_Salario], [Salario], [MetodoRetefuente], [Val_Retefuente], [Por_Retefuente], [Tipo_Retefuente], [Deducible], [DedSaludEdu], [DedAFC], [DedAlimentacion], [Regimen], [Fec_Retiro], [Val_Liq_Contrato], [FormaPago], [Sabado], [Cod_Ciudad], [Cod_CajaCompensacion], [Clasificacion_Contable], [Tipo_Cta], [Num_Cta], [Banco], [Cod_Zona], [Cod_Sucursal], [Cod_Ccostos], [Cod_Depto], [Cod_Cargo], [Cod_Grupo], [Cod_Arp], [Cod_Eps], [Cod_Afp], [Cod_Afc], [Cod_Escala], [Cod_Tarifa_Plena], [Modo_Costo_Hora], [Cod_Empleador], [Aux_Transporte], [Cod_Pais_Nacimiento], [Cod_Pais_Nacionalidad], [Fec_Vence_Visa], [Porc_Anticipo], [Porc_Anticipo_Prima], [ModoSalario], [AnticipoPrima], [Cod_NvaEps], [Cod_NvaAfp], [Cod_NvaAfc], [Fec_CamEps], [Fec_CamAfp], [Fec_CamAfc], [Tiempo_C], [Clave], [Saldo_Base_Aporte], [Base_Aporte], [Saldo_Dias_Aporte], [Dias_Aporte], [No_Hijos], [LiqNomina], [Estudiante], [Practicante], [MedicinaPrepagada], [Telefono1], [Porc_Embargo], [Saldo_Ini_Horas], [DiasVacAno], [Cod_Causal_Retiro], [Cod_Lugar_Expedicion], [DiasVacPendientesDisfrute], [DiasSalud], [FecLimiteDeducible], [Fec_Cambio_Tipo_Salario], [GrupoSanguineo], [Cod_Colaborador], [Cod_Barrio], [Cod_Jefe], [Declarante], [VigenciaDependientes], [DiasVivienda], [DiasPrepagada], [DiasDependientes], [FecSubstitucionPatronal], [Cod_Alterno], [Cod_GrupoDif], [Tipo_Pension], [Tipo_Pensionado], [Pension_Compartida], [Cod_Agencia], [Regretted], [NoBeneficio558], [Regreted], [CodPractica], [Transfer], [NumHorasMes], [NomCliente]) VALUES (635, N'ORDOÑEZ GALAN JOAN LEONARDO (R)              ', N'JOAN                ', N'LEONARDO                      ', N'ORDOÑEZ             ', N'GALAN                         ', N'C', N'1032430345  ', N'            ', N'19890623', N'1', N'M', N'CLL 66SUR Nº 81 - F 09 BOSA                  ', N'R', N'7102539                  ', N'3133001656    ', N'1032430345  ', N'3  ', N'                                             ', 1, N'3', 0, 0, N'1', CAST(0 AS Numeric(18, 0)), N'20090324', N'20090324', 496900, N'1', 0, 0, N'2', 0, 0, 0, 0, N'1', N'20091122', 86957, N'2', N'1', 591, 0, N'1', N'2', N'006270491068        ', N'051            ', 1, 3, 1, 21, 66, 1, 1, 2, 0, 0, 0, 0, N' ', 19, N'N', 1, 1, N'        ', 0, 0, N'F', N'S', 0, 0, 0, N'        ', N'        ', N'        ', N'1', N'          ', 0, 0, 0, 0, 0, 0, N'S', N'S', N'N', N'            ', 0, 0, 15, 0, 591, 0, 0, N'        ', N'        ', N'                    ', N'                    ', 0, 0, N' ', N'        ', 0, 0, 0, N'        ', 0, 1, N'  ', N' ', N'1', 3, N' ', N' ', N' ', N'                                                  ', N' ', N'     ', N'                                                                                                                                                      ')
GO
INSERT [dbo].[PARAMETROS] ([Ano], [Salmin], [Salint], [SubTrans], [ApoSalud], [ApoFondoSalud], [ApoFondoPen], [ApoFondoSolPen], [Cod_Arp], [ApoFondoArp], [LiqNomina], [ApoPsalud], [ApoPFondoPen], [ApoPriesProf], [ApoCajaCom], [ApoIcbf], [ApoSena], [ACesantias], [AintCesantias], [Aprimas], [Avacaciones], [Fec_Inicial], [Fec_Nomina], [Tipo_Liquidacion], [PorcSalNor], [PorcSalInt], [PorcSalEsc], [TipoSabado], [Codigo], [CtaGtoCajas], [CtaPasCajas], [CtaGtoIcbf], [CtaPasIcbf], [CtaGtoSena], [CtaPasSena], [CtaGtoSalud], [CtaPasSalud], [CtaGtoPension], [CtaPasPension], [CtaGtoRiesProf], [CtaPasRiesProf], [CtaGtoCesantias], [CtaPasCesantias], [CtaGtoIntCesantias], [CtaPasIntCesantias], [CtaGtoPrimas], [CtaPasPrimas], [CtaGtoVacaciones], [CtaPasVacaciones], [Tipo_Interfase], [DiasMaxHabiles], [IntMora], [Sal_Max_Asegur], [ValMaxAnual], [ValMaxMensual], [PorcEduMensual], [PorcPrimaSalNormal], [PorcPrimaSalEscala], [Hora_Ent], [Hora_Sal], [Tiempo_Alm], [TopeMaxRentaExcenta], [PorcMaxApoPen], [TipoLiqFestivos], [Corte30Retefuente], [Corte30SegSocial], [Fec_Acumulado], [Val_Uvt], [PptoAnualCap], [Promedio_Cesantias], [Corte30Provisiones], [BasePrimaSubsidio], [Sal_Max_AsegurRie], [Calculo_Anticipo], [Pago_Vac_FecLlegada], [LogoSucursal], [DedFiscales], [CtaActGasAnt], [GastoAnticipadoSueldos], [DedFiscalSalud], [Servidor], [Usuario], [Clave], [Puerto], [Remitente], [Mensaje], [Por_Bonos], [CtaPasBonificaciones], [TopeSalud], [TopePension], [TopeRiesgos], [TopeCaja], [Vac100], [TopeMaximo], [DiasLic], [DiasCesantias], [Ley1429], [UVTActual], [BaseCesantiaSubsidio], [SubParcial], [SubCompleto], [SubDiasParcial], [SubDiasCompleto], [TopeMaxAportes], [TopeMaxSalud], [TopeMaxDependientes], [AportanteExoneradoCajaSalud], [RedoneaRetefuente], [SubPrimeraVariables], [DiasNoLabDerecho], [DiasTrans], [GastoVacacionesAnt], [DedSaludMesAct], [RedondeoSaludPension], [UsaConceptosSena], [SSL], [LiquidaDiasDerecho], [Dia31], [IbcAnt], [PorcMinBaseRet], [Fec_Aplica_Reforma], [Autenticar], [AuxTransRemoto], [UvtDependientes], [HorasTrabMes], [FecNvaReforma]) VALUES (2025, 1423500, 18505500, 200000, 4, 0, 4, 1, 1, 0, 30, 8.5, 12, 0.522, 4, 3, 2, 8.66, 8.66, 4.17, 4.17, N'19920101', N'20251230', N'2', 50, 0, 0, N'1', 1, N'        ', N'23701001', N'        ', N'23701001', N'        ', N'23701001', N'        ', N'23700501', N'        ', N'23803001', N'        ', N'23700601', N'        ', N'26100501', N'        ', N'26101001', N'        ', N'26102001', N'        ', N'26101501', N'6', 0, 0, 25, 66730660, 4979900, 10, 0, 0, N'0830 AM   ', N'0530 PM   ', N'0800      ', 39341210, 30, N'1', N'N', N'N', N'20251130', 49799, 2011, N'N', N'S', N'S', 25, N'N', N'S', N'N', N'S', N'17059505  ', N'S', N'N', N'167.246.42.250                ', N'                              ', N'                    ', N'25    ', N'Nomina@col-resources.com                                    ', N'                                                                                                                                                                                                                                                                                                                                                                                                                ', 0, N'        ', 1900000, 6000000, 450000, 1000000, N'N', 40, N'S', 89, N'S', N'N', N'N', N' ', N' ', N' ', N' ', 189236200, 796784, 1593568, N'S', N'N', N'S', N' ', N'N', N'S', N'S', N'S', N'N', 0, N'N', N'N', N'N', 60, N'20170101', 0, CAST(140030.00 AS Numeric(18, 2)), CAST(6.00 AS Numeric(18, 2)), CAST(220 AS Numeric(18, 0)), N'20250625')
GO
SET IDENTITY_INSERT [dbo].[PARAMETROS_GENERALES] ON 
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1, N'Version', N'2.1.0', N'Version actual de la aplicacion y base de datos')
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (2, N'CONCEPTOS_26', N'{ "Tipo": "INCAPACIDAD_GENERAL", "Validar_anexo": true, "Diagnostico": true, "Tercero_informar": 9 }', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (4, N'CONCEPTOS_178', N'{ "Tipo": "CUMPLEANIOS", "Maximo_dias": 1, "Maximo_por_anio": 1, "Dias_habiles": 8 }', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (5, N'CONCEPTOS_243', N'{ "Tipo": "DIA_FAMILIA", "Maximo_dias": 1, "Maximo_por_anio": 2, "Maximo_por_semestre": 1, "Invisible": false }', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (6, N'CONCEPTOS_33', N'{ "Tipo": "INCAPACIDAD_PROFESIONAL", "Tercero_informar": 9 }', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1008, N'CONCEPTOS_22', N'{ "Tipo": "LICENCIA_MATERNIDAD", "Tercero_informar": 9 }', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1009, N'CONCEPTOS_56', N'{ "Tipo": "LICENCIA_PATERNIDAD", "Tercero_informar": 9 }', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1010, N'CONCEPTOS_24', N'{ "Tipo": "LICENCIA_REMUNERADA", "Ignorar_Finde": true }', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1011, N'CONCEPTOS_260', N'{ "Tipo": "LICENCIA_LUTO", "Validar_anexo": true, "Maximo_dias": 5, "Tercero_informar": 9, "Ignorar_Finde": true, "Permitir_fecha_futura": false}', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1012, N'DataicoElectronicPayrollEndpoint', N'https://api.dataico.com/direct/payroll-api', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1013, N'DataicoElectronicPayrollAccountId', N'ec1062d5-19c0-4549-ab81-9f9615d9a937', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1014, N'DataicoElectronicPayrollAuthToken', N'fd51481e17d3f70a66bb348f05d6f339', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1015, N'DataicoElectronicPayrollDianTestId', N'38b85f18-b95d-41b3-9b55-a96891f49780', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1016, N'DataicoElectronicPayrollDianId', N'd0e88268-a4ab-447d-918c-19c1c248b5c3', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1017, N'DataicoElectronicPrefixes', N'{
	  "soporteNomina": {
		  "prefijo": "MMS",
		   "numeroActual": null
	  },
	  "notaAjusteRemplazo": {
		  "prefijo": "NR",
		  "numeroActual": null
	  },
	  "notaAjusteEliminacion": {
		  "prefijo": "NE",
		  "numeroActual": null
	  }
}', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1018, N'CONCEPTOS_25', N'{"Tipo":"LICENCIA_NO_REMUNERADA","Invisible": true}', NULL)
GO
INSERT [dbo].[PARAMETROS_GENERALES] ([Id], [Cod_Parametro], [Valor], [Descripcion]) VALUES (1019, N'CONCEPTOS_29', N'{"Tipo":"SANCION_LABORAL","Invisible": true}', NULL)
GO
SET IDENTITY_INSERT [dbo].[PARAMETROS_GENERALES] OFF
GO
INSERT [dbo].[TERCEROS] ([Cod_Tercero], [Documento], [Dv], [Tipo_Documento], [Tipo_Tercero], [PNombre], [SNombre], [PApellido], [SApellido], [Tercero], [Dir_Elec], [Cargo], [CareerID], [Estado]) VALUES (7, N'1014243253     ', N'  ', N'E ', N'8         ', N'CARLOS                        ', N'MARIO                         ', N'SALGADO                       ', N'BURGOS                        ', N'SALGADO BURGOS CARLOS MARIO                                                                                             ', N'carlos.salgado@publicisgroupe.com                                                                   ', N'CAMPAIGN MANAGER', N'', 1)
GO
INSERT [dbo].[TERCEROS] ([Cod_Tercero], [Documento], [Dv], [Tipo_Documento], [Tipo_Tercero], [PNombre], [SNombre], [PApellido], [SApellido], [Tercero], [Dir_Elec], [Cargo], [CareerID], [Estado]) VALUES (513, N'1000135625     ', N'  ', N'C ', N'8         ', N'ANA                           ', N'MARIA                         ', N'CASTRO                        ', N'CUERVO                        ', N'CASTRO CUERVO ANA MARIA                                                                                                 ', N'anacastr3@publicisgroupe.net                                                                        ', N'', N'10237030', 0)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [uk_param_generales]    Script Date: 30/01/2026 2:58:48 p. m. ******/
ALTER TABLE [dbo].[PARAMETROS_GENERALES] ADD  CONSTRAINT [uk_param_generales] UNIQUE NONCLUSTERED 
(
	[Cod_Parametro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  CONSTRAINT [DF__EMPLEADOS__Telef__01F34141]  DEFAULT ('') FOR [Telefono1]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  CONSTRAINT [DF__EMPLEADOS__Porc___02E7657A]  DEFAULT ('') FOR [Porc_Embargo]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  CONSTRAINT [DF__EMPLEADOS__Saldo__3AAC9BB0]  DEFAULT ('') FOR [Saldo_Ini_Horas]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  CONSTRAINT [DF__EMPLEADOS__Cod_C__4341E1B1]  DEFAULT ('') FOR [Cod_Causal_Retiro]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  CONSTRAINT [DF__EMPLEADOS__Cod_L__4DBF7024]  DEFAULT ('') FOR [Cod_Lugar_Expedicion]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  CONSTRAINT [DF__EMPLEADOS__DiasV__76C185B7]  DEFAULT ('') FOR [DiasVacPendientesDisfrute]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [DiasSalud]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [FecLimiteDeducible]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Fec_Cambio_Tipo_Salario]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [GrupoSanguineo]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Cod_Colaborador]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ((0)) FOR [Cod_Barrio]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ((0)) FOR [Cod_Jefe]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Declarante]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [VigenciaDependientes]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [DiasVivienda]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [DiasPrepagada]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [DiasDependientes]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [FecSubstitucionPatronal]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Cod_Alterno]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ((1)) FOR [Cod_GrupoDif]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Tipo_Pension]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Tipo_Pensionado]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ((1)) FOR [Pension_Compartida]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ((0)) FOR [Cod_Agencia]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Regretted]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [NoBeneficio558]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Regreted]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [CodPractica]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [Transfer]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [NumHorasMes]
GO
ALTER TABLE [dbo].[EMPLEADOS] ADD  DEFAULT ('') FOR [NomCliente]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [Licencias]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [Servidor]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [BaseDatos]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [Usuario]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [Passw]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [Ruta]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [ModNOM]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [ModRH]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [ModMIS]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [Serial]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [Cod_Local]
GO
ALTER TABLE [dbo].[EMPRESAS] ADD  DEFAULT ('') FOR [LogoOpc]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TipoLiqFestivos]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Corte30Retefuente]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Corte30SegSocial]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Fec_Acumulado]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Val_Uvt]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [PptoAnualCap]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Promedio_Cesantias]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Corte30Provisiones]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [BasePrimaSubsidio]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((20)) FOR [Sal_Max_AsegurRie]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [Calculo_Anticipo]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Pago_Vac_FecLlegada]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [LogoSucursal]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('S') FOR [DedFiscales]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [CtaActGasAnt]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [GastoAnticipadoSueldos]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [DedFiscalSalud]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Servidor]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Usuario]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Clave]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Puerto]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Remitente]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Mensaje]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [Por_Bonos]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [CtaPasBonificaciones]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TopeSalud]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TopePension]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TopeRiesgos]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TopeCaja]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [Vac100]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((0)) FOR [TopeMaximo]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('S') FOR [DiasLic]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((89)) FOR [DiasCesantias]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [Ley1429]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [UVTActual]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('S') FOR [BaseCesantiaSubsidio]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [SubParcial]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('S') FOR [SubCompleto]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [SubDiasParcial]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('S') FOR [SubDiasCompleto]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TopeMaxAportes]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TopeMaxSalud]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [TopeMaxDependientes]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [AportanteExoneradoCajaSalud]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [RedoneaRetefuente]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('S') FOR [SubPrimeraVariables]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [DiasNoLabDerecho]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [DiasTrans]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('S') FOR [GastoVacacionesAnt]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [DedSaludMesAct]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [RedondeoSaludPension]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [UsaConceptosSena]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((0)) FOR [SSL]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [LiquidaDiasDerecho]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [Dia31]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('N') FOR [IbcAnt]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('60') FOR [PorcMinBaseRet]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('20170101') FOR [Fec_Aplica_Reforma]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((0)) FOR [Autenticar]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((0)) FOR [AuxTransRemoto]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((0)) FOR [UvtDependientes]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ((240)) FOR [HorasTrabMes]
GO
ALTER TABLE [dbo].[PARAMETROS] ADD  DEFAULT ('') FOR [FecNvaReforma]
GO
ALTER TABLE [dbo].[PERMISOS_WEB] ADD  DEFAULT ('') FOR [Usuario]
GO
ALTER TABLE [dbo].[PERMISOS_WEB] ADD  DEFAULT ('') FOR [Page]
GO
ALTER TABLE [dbo].[PERMISOS_WEB] ADD  DEFAULT ('') FOR [Permiso]
GO
ALTER TABLE [dbo].[PERMISOS_WEB] ADD  DEFAULT ('') FOR [Pagina]
GO
ALTER TABLE [dbo].[PERMISOS_WEB] ADD  DEFAULT ('') FOR [Descripcion]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Cod_Tercero]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Documento]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Dv]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Tipo_Documento]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Tipo_Tercero]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [PNombre]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [SNombre]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [PApellido]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [SApellido]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Tercero]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Dir_Elec]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [Cargo]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ('') FOR [CareerID]
GO
ALTER TABLE [dbo].[TERCEROS] ADD  DEFAULT ((1)) FOR [Estado]
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
ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationGroup_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[ApplicationGroup] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.ApplicationGroup_GroupId]
GO
--ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.AspNetUsers_UserId]
--GO
--ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_IdentityUser_Id]
--GO
--ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_IdentityRole_Id]
--GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id]
GO
update EMPLEADOS set Dir_Elec='carlos.epieyu@systemsltda.com'
update TERCEROS set Dir_Elec='carlos.epieyu@systemsltda.com'
update AspNetUsers set DBName='SUSPA_DB'
INSERT [dbo].[EMPRESAS] ([Codigo], [Tipo_Documento], [Num_Documento], [Digito_Verificacion], [Nombre_Empresa], [Direccion], [Cod_Depto], [Cod_Ciudad], [Tel], [Fax], [Cod_Arp], [Cod_Sucursal_Pag], [Clase_Aportante], [Forma_Presenta], [Fec_Instalacion], [Fec_Ult_Acceso], [Dias_Vigencia], [Fec_Vence_Licencia], [Clave], [Codigo_Habilitacion], [Cod_Pais], [Licencias], [Servidor], [BaseDatos], [Usuario], [Passw], [Ruta], [ModNOM], [ModRH], [ModMIS], [Serial], [Cod_Local], [Logo], [LogoOpc], [CargaLogoOpc], [Estado]) 
VALUES (1, N'N', N'000000', N'2', N'UNION TEMPORAL ANOUIJA 2026', N'CARRERA 4 # 27A - 57', 4, 591, N'', N'', 1, 2, N'G', N'S', N'20090824', N'20251230', 7200, N'20290511', N'          ', N'', N'1', 3, N'', N'', N'', N'', N'', N'S', N'S', N'S', N'          ', N'                    ', NULL, N'', NULL, N'A')
GO
update EMPRESAS set BaseDatos='SUSPA_DB', Servidor='', Clave='', Passw=''
GO
CREATE TABLE [dbo].[AUDITORIA](
	[Cod_Usuario] [smallint] NOT NULL,
	[Fec_Transaccion] [char](8) NOT NULL,
	[Hora_Transaccion] [char](8) NOT NULL,
	[Tabla] [char](30) NULL,
	[Accion] [char](1) NOT NULL,
	[Descripcion] [text] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

