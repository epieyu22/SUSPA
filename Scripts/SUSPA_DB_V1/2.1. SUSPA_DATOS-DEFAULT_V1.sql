-- ETC
SET IDENTITY_INSERT [dbo].[ETC] ON 
INSERT [dbo].[ETC] ([idEtc], [entidad], [nombre]) VALUES (1, N'1', N'ETC RIOHACHA')
INSERT [dbo].[ETC] ([idEtc], [entidad], [nombre]) VALUES (2, N'2', N'CZ MANAURE')
INSERT [dbo].[ETC] ([idEtc], [entidad], [nombre]) VALUES (3, N'2', N'CZ RIOHACHA')
SET IDENTITY_INSERT [dbo].[ETC] OFF

SET IDENTITY_INSERT [dbo].[CONTRATOS] ON 
INSERT [dbo].[CONTRATOS] ([idContrato], [numContrato], [anioContrato], [idEmpresa], [idEtc]) VALUES (1, N'01231', 2026, 1, 1)
SET IDENTITY_INSERT [dbo].[CONTRATOS] OFF

SET IDENTITY_INSERT [dbo].[INSTITUCIONES] ON 
INSERT [dbo].[INSTITUCIONES] ([idInstitucion], [daneInstitucion], [nombreCompleto], [direccion], [nombreCorto], [nombreRector], [cedulaRector], [activo]) VALUES (1, N'24400100304201', N'INSTITUCION ETNOEDUCATIVA N 10 CUCURUMANA', N'Comunidad CUCURUMANA', N'CUCURUMANA', N'JHON JAIRO BONIVENTO VANGRIEKEN', N'84083195', 1)
INSERT [dbo].[INSTITUCIONES] ([idInstitucion], [daneInstitucion], [nombreCompleto], [direccion], [nombreCorto], [nombreRector], [cedulaRector], [activo]) VALUES (3, N'244001004812', N'INSTITUCION ETNOEDUCATIVA N 26 WAYENETAMANA', N'Comunidad WAYENETAMANA', N'WAYENETAMANA', N'KETTY PUSHAINA NZALEZ', N'56085428', 1)
SET IDENTITY_INSERT [dbo].[INSTITUCIONES] OFF

