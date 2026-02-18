
/****** Object:  Table [dbo].[VARIABLES]    Script Date: 16/12/2020 9:00:43 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE VARIABLES_VACACIONES(
	[Cod_Variable] [smallint] NOT NULL,
	[Descripcion] [char](50) NOT NULL,
	[Tipo_Dato] [char](20) NOT NULL,
	[Tamano] [int] NOT NULL,
	[Uso] [int] NOT NULL,
	[UsaConcepto] [char](1) NOT NULL,
	[Cod_Concepto] [smallint] NOT NULL,
	[Obligatoria] [bit] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ('') FOR [Cod_Variable]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ('') FOR [Descripcion]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ('') FOR [Tipo_Dato]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ((0)) FOR [Tamano]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ((0)) FOR [Uso]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ('N') FOR [UsaConcepto]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ((0)) FOR [Cod_Concepto]
GO

ALTER TABLE [dbo].[VARIABLES] ADD  DEFAULT ((0)) FOR [Obligatoria]
GO


