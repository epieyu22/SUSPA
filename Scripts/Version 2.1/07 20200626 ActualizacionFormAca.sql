

CREATE TABLE [dbo].[FORMACADEMICA](
	[Cod_HojaVida] [nvarchar](10) NOT NULL,
	[Cod_Institucion] [smallint] NOT NULL,
	[Cod_Titulo] [smallint] NOT NULL,
	[Cod_Nivel] [smallint] NOT NULL,
	[Inicio] [char](8) NOT NULL,
	[Salida] [char](8) NOT NULL,
	[Cod_Especialidad] [smallint] NOT NULL,
	[Otra_Institucion] [nvarchar](20) NOT NULL,
	[Otro_Titulo] [nvarchar](20) NOT NULL,
	[Otra_Especialidad] [nvarchar](20) NOT NULL,
	[Adjunto] [varchar](max) NULL,
	[Cod_FormAcademica] [int] IDENTITY(1,1) NOT NULL,
	[Estado] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Cod_HojaVida]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Cod_Institucion]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Cod_Titulo]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Cod_Nivel]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Inicio]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Salida]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Cod_Especialidad]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Otra_Institucion]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Otro_Titulo]
GO

ALTER TABLE [dbo].[FORMACADEMICA] ADD  DEFAULT ('') FOR [Otra_Especialidad]
GO




--DROP TABLE EXPLABORAL
--DROP TABLE FORMACADEMICA
