
CREATE TABLE [dbo].[EXPLABORAL](
	[Cod_HojaVida] [varchar](max) NOT NULL,
	[Empresa] [varchar](50) NOT NULL,
	[Telefono] [varchar](14) NOT NULL,
	[Industria] [varchar](max) NOT NULL,
	[Sector] [varchar](50) NOT NULL,
	[Cargo] [varchar](50) NOT NULL,
	[AreaTrabajo] [varchar](100) NOT NULL,
	[Fec_Ingreso] [varchar](50) NOT NULL,
	[Fec_Retiro] [varchar](14) NOT NULL,
	[Funciones] [varchar](50) NOT NULL,
	[Jefe_Inmediato] [varchar](50) NOT NULL,
	[Logros] [varchar](50) NOT NULL,
	[Opinion] [varchar](50) NOT NULL,
	[Verificado] [varchar](1) NOT NULL,
	[Cod_ExpLaboral] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Cod_HojaVida]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Empresa]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Telefono]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Industria]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Sector]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Cargo]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [AreaTrabajo]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Fec_Ingreso]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Fec_Retiro]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Funciones]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Jefe_Inmediato]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Logros]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Opinion]
GO

ALTER TABLE [dbo].[EXPLABORAL] ADD  DEFAULT ('') FOR [Verificado]
GO


