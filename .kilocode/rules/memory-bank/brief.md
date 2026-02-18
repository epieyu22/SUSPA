# Arquitectura y Funcionalidades de JulianaWeb

## Descripción General
JulianaWeb es una aplicación web de gestión de recursos humanos desarrollada en ASP.NET MVC 5 con .NET Framework 4.8. Proporciona funcionalidades para la administración de empleados, nóminas, certificados, solicitudes de vacaciones, incapacidades y otros procesos relacionados con la gestión del talento humano.

## Arquitectura Técnica

### Backend
- **Framework**: ASP.NET MVC 5 (.NET Framework 4.8)
- **ORM**: Entity Framework 6.1.1
- **Base de Datos**: SQL Server
- **Autenticación**: Owin con OpenID Connect (Auth0) y ASP.NET Identity
- **API**: ASP.NET WebAPI 2
- **Logging**: NLog
- **Generación de PDFs**: Select.HtmlToPdf, TuesPechkin
- **Manipulación de Excel**: EPPlus
- **Envío de Correos**: MailKit

### Frontend
- **Framework**: AngularJS 1.x
- **UI Framework**: Bootstrap, Angular Material
- **Bundling**: AngularTemplates.Compile
- **Gestión de Estado**: ngStorage
- **Gráficos y Tablas**: jqWidgets, ng-table, md-data-table

### Configuración
- **Owin**: Configuración de middleware para autenticación y CORS
- **Bundles**: Optimización de recursos estáticos
- **Rutas**: Configuración de rutas MVC y WebAPI

## Estructura del Proyecto

### Directorios Principales
- `JulianaWeb/`: Código fuente del backend
  - `App_Start/`: Configuraciones iniciales (bundles, rutas, WebAPI)
  - `Business/`: Lógica de negocio (BO - Business Objects)
  - `Controllers/`: Controladores MVC y API
  - `Models/`: Modelos de datos y ViewModels
  - `Client/ng-app/`: Código fuente del frontend AngularJS
  - `Static/`: Recursos estáticos (imágenes, plantillas)
- `packages/`: Dependencias NuGet
- `Scripts/`: Scripts de compilación y utilidades

### Módulos de AngularJS
La aplicación frontend está modularizada en los siguientes módulos:
- `app.core`: Servicios core (autenticación, permisos, utilidades)
- `app.dashboard`: Dashboard principal
- `app.consultas`: Consultas de información (salarios, seguridad social)
- `app.certificados`: Generación de certificados (laborales, retención en la fuente, comprobantes de pago)
- `app.hojavida`: Gestión de hojas de vida de empleados
- `app.login`: Módulo de autenticación
- `app.vacaciones`: Gestión de vacaciones
- `app.admin`: Administración (roles, usuarios, aprobaciones)
- `app.solicitudes`: Solicitudes (vacaciones, incapacidades, licencias)
- `app.adlogin`: Login con Active Directory
- `app.incapacidades`: Gestión de incapacidades
- `app.otrasnov`: Otras novedades
- `app.solpersonal`: Solicitudes personales
- `app.planilla_nomina`: Planilla de nómina
- `app.timesheet`: Control de tiempo
- `app.bioseguridad`: Módulo de bioseguridad
- `app.cumpleanos`: Gestión de cumpleaños
- `app.facElectronica`: Facturación electrónica
- `app.turnos`: Gestión de turnos
- `app.dashboardBi`: Dashboard de business intelligence

## Funcionalidades Principales

### 1. Gestión de Beneficiarios
- Registro y control de información de Niños beneficiarios
- Gestion de cupos escolares y primera infancia por contrato
- Vinculacion y desvinculacion de nucleo familiar
- Gestión de documentos adjuntos

### 2. Gestión Menus de alimentos
- Registro y control de información de ciclos de menu

### 3. Gestión Pedidos de alimentos
- Gestion de calendario
-Periodicidad de entrega alimentaria

### 4. Registro y control de medidas antropometricas

## Permisos y Seguridad
El sistema implementa un sistema de permisos granular basado en roles:
- `consultas`: Acceso a módulos de consultas
- `consultas.vacaciones`: Gestión de vacaciones
- `consultas.cesantias`: Gestión de cesantías
- `settings`: Configuración del sistema

## API Endpoints
La aplicación expone una API RESTful para integración con otros sistemas. Algunos endpoints clave:
- `GET /API/Vacaciones/{Empresa}/{Cod_Empleado}/`: Obtiene información de vacaciones
- `GET /API/Cesantias/{Empresa}/{Cod_Empleado}/`: Obtiene información de cesantías
- `POST /API/Vacaciones/{Empresa}/{Cod_Empleado}/Solicitudes/`: Crea solicitud de vacaciones

## Instalación y Configuración
Para instalar la aplicación:
1. Ejecutar scripts de base de datos (ActFiles, SeedLogin.sql, SeedTablasJulianaWeb.sql)
2. Crear archivo `connectionstrings.config` con las conexiones a BD
3. Configurar parámetros de Auth0 en `Web.config`
4. Verificar existencia de especialidades en BD

## Tecnologías Adicionales
- **PDF Generation**: wkhtmltox para conversión HTML a PDF
- **Charts**: jqWidgets para gráficos interactivos
- **File Upload**: Directivas personalizadas para manejo de archivos
- **Date Handling**: Globalize.js para internacionalización de fechas
- **UI Components**: Angular Material, Bootstrap

Esta arquitectura permite una gestión integral de recursos humanos con una interfaz moderna y funcionalidades robustas para empresas colombianas.
