using Aspose.Words.Saving;
using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JW3.Helpers;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace JulianaWeb.Controllers
{
  public class GenerarCertlaboralController : Controller
  {
    // GET: GenerarCertlaboral
    string FILTER_EMPLEADO = "1";
    string FILTER_CARGO = "2";
    string FILTER_DEPTO = "3";
    string FILTER_CCOSTO = "4";
    string FILTER_SUCURSAL = "5";
    string FILTER_ZONA = "6";
    string FILTER_DEFAULT = "999";

    public ActionResult Index(CertLaboralViewModel data)
    {

      TERCEROS Empleado_Firma = null;
      string Cargo_Empleado_Firma = "";
      string Dir_Elec_Empleado_Firma = "";
      string tipoContrato = "";
      string ContracTypeCert = "";
      string htmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/certlab.cshtml");
      string logoEmpresa = "Static/images/logos/" + data.DBName + ".png";
      string Plantilla_Certificado = System.IO.File.ReadAllText(htmlPath);
      // Generando codigo unicó de verificación
      string token = data.empleado.Cod_Empleador.ToString();
      token += data.empleado.Cod_Empleado.ToString();
      token += "_" + DateTime.Now.Ticks;

      CertLaboralTemplatewModel model = new CertLaboralTemplatewModel
      {
        empleado = data.empleado,
        dirigido = data.dirigido,
        logo = logoEmpresa,
        dirigidoEmbajada = data.dirigidoEmbajada,
        viajeLaboral = data.viajeLaboral,
        fecha = DateTime.Now,
        fechaIngreso = UtilHelper.getDate(data.empleado.Fec_Ingreso),
        token = token,
        autoriza = "Autoriza:",
        autorizaCargo = "Cargo:",
        salario = UtilHelper.numberToLetter(data.empleado.Salario.ToString())
      };
      model.salario = model.salario.Replace("  ", " ");

      //Determinando el tipo de contrato
      switch (data.empleado.Tipo_Contrato)
      {
        case "1":
          tipoContrato = "A TERMINO INDEFINIDO";
          ContracTypeCert = "INDEFINITE TERM";
          break;
        case "2":
          tipoContrato = "A TERMINO FIJO";
          ContracTypeCert = "FIXED TERM";
          break;
        case "3":
          tipoContrato = "DE APRENDIZAJE";
          ContracTypeCert = "LEARNING";
          break;
        case "4":
          tipoContrato = "COMO PRACTICANTE UNIVERSITARIO CON ARL";
          ContracTypeCert = "UNIVERSITY INTERNSHIP WITH ARL";
          break;
        case "5":
          tipoContrato = "COMO PRACTICANTE UNIVERSITARIO SIN APORTES";
          ContracTypeCert = "UNIVERSITY INTERNSHIP WITHOUT CONTRIBUTIONS";
          break;
        case "6":
          tipoContrato = "COMO ESTUDIANTE APRENDIZ";
          ContracTypeCert = "STUDENT APPRENTICE";
          break;
        case "7":
          tipoContrato = "PENSIONADO POR EMPRESA";
          ContracTypeCert = "PENSIONER BY COMPANY";
          break;
        case "8":
          tipoContrato = "DE OBRA O LABOR";
          ContracTypeCert = "WORK OR LABOR";
          break;
      }

      model.tipoContrato = tipoContrato;


      PARAMETROS_CERTLAB config = null;

      JulianaContext db = new JulianaContext(data.DBName);
      EMPRESAS Empresa = db.EMPRESAS.SingleOrDefault(e => e.Codigo == data.empleado.Cod_Empleador);
      CIUDADES ciudad = db.CIUDADES.SingleOrDefault(c => c.Codigo == Empresa.Cod_Ciudad);
      CARGOS cargo = db.CARGOS.SingleOrDefault(c => c.Cod_Cargo == data.empleado.Cod_Cargo);

      var configQuery = from c in db.PARAMETROS_CERTLAB
                        where
                        (c.Filter == FILTER_EMPLEADO && c.Cod_Filter == data.empleado.Cod_Empleado)
                        || (c.Filter == FILTER_CARGO && c.Cod_Filter == data.empleado.Cod_Cargo)
                        || (c.Filter == FILTER_DEPTO && c.Cod_Filter == data.empleado.Cod_Depto)
                        || (c.Filter == FILTER_CCOSTO && c.Cod_Filter == data.empleado.Cod_Ccostos)
                        || (c.Filter == FILTER_SUCURSAL && c.Cod_Filter == data.empleado.Cod_Sucursal)
                        || (c.Filter == FILTER_ZONA && c.Cod_Filter == data.empleado.Cod_Zona)
                        orderby c.Filter
                        select c;

      config = configQuery.FirstOrDefault();

      if (config == null)
      {
        config = db.PARAMETROS_CERTLAB.FirstOrDefault(c => c.Filter == FILTER_DEFAULT);
      }

      model.Firmar_Certlab = config.Firma_Digital;
      if (config.Firma_Filename != null)
      {

        //string firmaFilename = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/images/firmas/" + config.Firma_Filename);
        //model.firma = firmaFilename;
      }
      model.ShowotrosDevengos = config.Otros_Devengos.Value;
      model.ShowHorasExtras = config.Horas_Extras.Value;
      model.showBeneficios = config.Base_Salario.Value;
      model.showOtrosIngresos = config.Otros_Ingresos.Value;
      model.textEmbajada = config.Texto_Embajada;
      model.textViajeLaboral = config.Texto_Viaje_Laboral;
      model.mesesPromedio = config.Meses_Promedio;
      model.otrosConceptos = config.Conceptos;
      if (config.Cod_Empleado_Autoriza != null)
      {
        Empleado_Firma = db.TERCEROS.Where(t => t.Cod_Tercero == config.Cod_Empleado_Autoriza)
                                    .ToList().First();
        Dir_Elec_Empleado_Firma = Empleado_Firma.Dir_Elec.Trim();
      }
      List<short> codsConceptos = new List<short>();
      if (!string.IsNullOrWhiteSpace(model.otrosConceptos))
      {
        foreach (string cod in model.otrosConceptos.Split(','))
        {
          short codigo;

          if (short.TryParse(cod, out codigo))
            codsConceptos.Add(codigo);
        }
      }



      model.empresa = Empresa;
      model.ciudad = ciudad.Nom_Ciudad;
      model.cargo = cargo.Nom_Cargo;

      // Calculo de fechas para promedio de otros devengos
      int mesesPromedio = Convert.ToInt32(model.mesesPromedio);
      int diasXMes = 30;

      DateTime fechaActual = DateTime.Now;
      DateTime fechaIngreso = UtilHelper.getDate(data.empleado.Fec_Ingreso);
      DateTime fechaInicioPromedio = fechaActual.AddDays(-mesesPromedio * diasXMes);
      if (fechaIngreso > fechaInicioPromedio)
      {
        fechaInicioPromedio = fechaIngreso;
      }
      TimeSpan intervalo = fechaActual - fechaInicioPromedio;
      int diasPromedio = intervalo.Days;
      string fechaInicio = UtilHelper.getUnglyDate(fechaInicioPromedio);
      string fechaFinal = UtilHelper.getUnglyDate(fechaActual);

      // Horas extras
      if (model.ShowHorasExtras)
      {
        var qHorasExtras = from h in db.HISTORICO
                           join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                           where h.Cod_Empleado == data.empleado.Cod_Empleado
                               && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                               && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                               && c.Tipo_Concepto == "P"
                           select new
                           {
                             h.Val_Novedad
                           };

        var novedadesHorasExtras = qHorasExtras.ToList();
        double totalHorasExtras = 0;
        novedadesHorasExtras.ForEach(n => totalHorasExtras += n.Val_Novedad);
        double promedioHorasExtras = (totalHorasExtras / diasPromedio) * diasXMes;
        model.promedioHorasExtras = promedioHorasExtras;
        if (model.promedioHorasExtras <= 0)
        {
          model.ShowHorasExtras = false;
        }
      }
      else
      {
        model.promedioHorasExtras = 0;
      }

      // Beneficios salario, conceptos 8, 9, 0 que sean devengos
      string[] conceptosBeneficioSalario = new string[] { "8", "9", "0" };
      if (model.showBeneficios)
      {
        var qBeneficios = from h in db.HISTORICO
                          join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                          where h.Cod_Empleado == data.empleado.Cod_Empleado
                              && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                              && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                              && c.Devengo == "S"
                              && c.BSBenSalario == "S"
                          select new
                          {
                            h.Val_Novedad
                          };

        var novedadesBeneficios = qBeneficios.ToList();
        double totalBeneficios = 0;
        novedadesBeneficios.ForEach(b => totalBeneficios += b.Val_Novedad);
        double promedioBeneficios = totalBeneficios / mesesPromedio;
        model.promedioBeneficios = promedioBeneficios;
        if (model.promedioBeneficios <= 0)
        {
          model.showBeneficios = false;
        }
      }
      else
      {
        model.promedioBeneficios = 0;
      }

      var conceptosEspecificos = new Dictionary<string, double>();
      double promedioConceptosEspecificos = 0;
      // Conceptos especifico
      foreach (var cod in codsConceptos)
      {
        var concepto = db.CONCEPTOS.SingleOrDefault(c => c.Cod_Concepto == cod);
        var qconcepto = from h in db.HISTORICO
                        join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                        where h.Cod_Empleado == data.empleado.Cod_Empleado
                        && h.Cod_Concepto == concepto.Cod_Concepto
                        && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                        && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                        select h.Val_Novedad;
        double totalConcepto = 0;
        foreach (double valor in qconcepto)
        {
          totalConcepto += valor;
        }
        double promedioConcepto = totalConcepto / mesesPromedio;
        promedioConceptosEspecificos += promedioConcepto;
        if (promedioConcepto > 0)
        {
          conceptosEspecificos.Add(concepto.Nom_Concepto.Trim(), promedioConcepto);
        }
      }

      model.conceptosEspecificos = conceptosEspecificos;

      // otros ingresos
      string[] tipoConceptosSumados = new string[] { "8", "9", "0", "P" };
      codsConceptos.Add(1);
      codsConceptos.Add(2);
      if (model.showOtrosIngresos)
      {
        var qOtrosIngresos = from h in db.HISTORICO
                             join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                             where h.Cod_Empleado == data.empleado.Cod_Empleado
                                 && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                                 && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                                 && c.Devengo == "S"
                                 && !tipoConceptosSumados.Contains(c.Tipo_Concepto)
                                 && !codsConceptos.Contains(c.Cod_Concepto)
                             select new
                             {
                               h.Val_Novedad
                             };
        var novedadesOtroIngresos = qOtrosIngresos.ToList();
        double totalotrosIngresos = 0;
        novedadesOtroIngresos.ForEach(n => totalotrosIngresos += n.Val_Novedad);
        double promedioOtroIngresos = totalotrosIngresos / mesesPromedio;
        model.promedioOtroIngresos = promedioOtroIngresos;
        if (model.promedioOtroIngresos <= 0)
        {
          model.showOtrosIngresos = false;
        }
      }
      else
      {
        model.promedioOtroIngresos = 0;
      }

      model.netoAPagar = model.promedioBeneficios + model.promedioHorasExtras + model.promedioOtroIngresos + promedioConceptosEspecificos + data.empleado.Salario;

      model.ShowotrosDevengos = model.ShowHorasExtras
                                || model.showBeneficios
                                || model.showOtrosIngresos
                                || conceptosEspecificos.Count > 0;


      string Nom_Empleado = data.empleado.PNombre.Trim();
      Nom_Empleado += data.empleado.SNombre.Trim() != String.Empty ? " " + data.empleado.SNombre.Trim() : "";
      Nom_Empleado += " " + data.empleado.PApellido.Trim();
      Nom_Empleado += data.empleado.SApellido.Trim() != String.Empty ? " " + data.empleado.SApellido.Trim() : "";

      string Nom_Empleado_Firma = Empleado_Firma.Tercero;


      TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

      string Salario_Letras = UtilHelper.numberToLetter(data.empleado.Salario.ToString()).Replace("  ", " ").ToUpper();
      string Salario_Letras_English = UtilHelper.numberToLetter_English(data.empleado.Salario.ToString()).Replace("  ", " ").ToUpper();
      var plantilla_base = db.PLANTILLAS_CERTIFICADOS.SingleOrDefault(p => p.Autonum == data.Cod_Plantilla);

      //Cargue de la plantilla del documento, dentro de la plantilla mebreteada.
      Plantilla_Certificado = Plantilla_Certificado.Replace("@PLANTILLA", plantilla_base.Plantilla);

      //Info de la Empresa
      Plantilla_Certificado = Plantilla_Certificado.Replace("@LOGO", logoEmpresa);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[LOGO_EMPRESA]", "<img src='" + logoEmpresa + "'/>");
      Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_EMPRESA]", textInfo.ToTitleCase(Empresa.Nombre_Empresa.Trim().ToLower()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("@NOM_EMPRESA", textInfo.ToTitleCase(Empresa.Nombre_Empresa.Trim().ToLower()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("@NIT", Empresa.Num_Documento.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DOC_EMPRESA]", Empresa.Num_Documento.Trim() + "-" + Empresa.Digito_Verificacion.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIR_EMPRESA]", textInfo.ToTitleCase(Empresa.Direccion.Trim().ToLower()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[TEL_EMPRESA]", textInfo.ToTitleCase(Empresa.Tel.Trim()));

      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIRIGIDO]", data.dirigido);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[CIUDAD]", textInfo.ToTitleCase(ciudad.Nom_Ciudad.ToLower()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[FEC_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("D").ToLower()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIA_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("dd")));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIA_ACTUAL_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(DateTime.Now.Day.ToString())));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[MES_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("MMMM")));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[ANO_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("yyyy")));

      if (Empleado_Firma != null)
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_FIRMA]", Nom_Empleado_Firma);
        Plantilla_Certificado = Plantilla_Certificado.Replace("[CARGO_FIRMA]", Empleado_Firma.Cargo);
        Plantilla_Certificado = Plantilla_Certificado.Replace("[DOC_FIRMA]", Empleado_Firma.Documento.Trim());
      }
      else
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_FIRMA]", "");
        Plantilla_Certificado = Plantilla_Certificado.Replace("[CARGO_FIRMA]", "");
        Plantilla_Certificado = Plantilla_Certificado.Replace("[DOC_FIRMA]", "");
      }


      if (data.dirigidoEmbajada)
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[DIR_EMBAJADA]", config.Texto_Embajada);
      }
      else
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[DIR_EMBAJADA]", "");
      }

      if (config.Firma_Digital == true)
      {
        string img = String.Format("<img src='{0}'>", model.firma);
        Plantilla_Certificado = Plantilla_Certificado.Replace("[IMAGEN_FIRMA]", img);
      }
      else
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[IMAGEN_FIRMA]", "");
      }

      //AÑADIR EL RESTO
      Plantilla_Certificado = Plantilla_Certificado.Replace("[FUNCIONES]", cargo.Funciones);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[INFO_ADICIONAL]", "");

      short Cod_Lugar_Expedicion = data.empleado.Cod_Lugar_Expedicion.Value;
      var lug = db.CIUDADES.Where(c => c.Codigo == Cod_Lugar_Expedicion).ToList();
      string Lugar_Expedicion = lug.Count() > 0 ? lug.First().Nom_Ciudad : "";


      var salarioBeneficios = Convert.ToInt32(data.empleado.Salario + model.promedioBeneficios);



      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIA_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(data.empleado.Fec_Ingreso).ToString("dd").ToLower())));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[MES_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(data.empleado.Fec_Ingreso).ToString("MM").ToLower())));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[ANO_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(data.empleado.Fec_Ingreso).ToString("yyyy").ToLower())));

      Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_EMPLEADO]", textInfo.ToTitleCase((Nom_Empleado.ToLower())));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[TIPO_CONTRATO]", tipoContrato);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[CONTRACT_TYPE]", ContracTypeCert);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DOC_EMPLEADO]", data.empleado.Cedula.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[FEC_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(data.empleado.Fec_Ingreso).ToString("D").ToLower())));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO_LETRAS] ", textInfo.ToTitleCase(Salario_Letras.Trim().ToLower()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO_LETRAS_ENGLISH] ", textInfo.ToTitleCase(Salario_Letras_English.Trim().ToLower()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO]", data.empleado.Salario.ToString("N0"));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[CARGO_EMPLEADO]", textInfo.ToTitleCase(cargo.Nom_Cargo.ToLower().Trim()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[LUG_DOC_EMPLEADO]", textInfo.ToTitleCase(Lugar_Expedicion.ToLower().Trim()));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[COD_VERIFICACION]", model.token);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO_BENEFICIOS]", salarioBeneficios.ToString("N0"));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO_BENEFICIOS_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(salarioBeneficios.ToString())));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[OTROS_CONCEPTOS]", promedioConceptosEspecificos.ToString("N0"));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[OTROS_CONCEPTOS_LETRAS]", UtilHelper.numberToLetter(promedioConceptosEspecificos.ToString()));

      Plantilla_Certificado = Plantilla_Certificado.Replace("[BENEFICIOS]", model.promedioBeneficios.ToString("N0"));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[BENEFICIOS_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(model.promedioBeneficios.ToString())));


      string PDF = Plantilla_Certificado;
      string footerPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/certlab-footer.html");
      string footer = System.IO.File.ReadAllText(footerPath);
      footer = footer.Replace("@token", model.token);

      footer = footer.Replace("[NOM_EMPRESA]", Empresa.Nombre_Empresa.Trim());
      footer = footer.Replace("@NOM_EMPRESA", Empresa.Nombre_Empresa.Trim());
      footer = footer.Replace("@NIT", Empresa.Num_Documento.Trim());
      footer = footer.Replace("[DOC_EMPRESA]", Empresa.Num_Documento.Trim() + "-" + Empresa.Digito_Verificacion.Trim());
      footer = footer.Replace("[DIR_EMPRESA]", textInfo.ToTitleCase(Empresa.Direccion.Trim().ToLower()));
      footer = footer.Replace("[TEL_EMPRESA]", textInfo.ToTitleCase(Empresa.Tel.Trim()));

      footer = footer.Replace("[DIRIGIDO]", data.dirigido);
      footer = footer.Replace("[CIUDAD]", textInfo.ToTitleCase(ciudad.Nom_Ciudad.Trim().ToLower()));
      footer = footer.Replace("[FEC_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("D").ToLower()));
      footer = footer.Replace("[DIA_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("dd")));
      footer = footer.Replace("[MES_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("MMMM")));
      footer = footer.Replace("[ANO_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("yyyy")));





      byte[] pdfFileBytes = PdfHelper.newconvert("Certificado Laboral", PDF, data.empleado.Cedula.Trim());

      string copyPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/CertificadosBK/");
      var filename = model.token + ".pdf";
      copyPath += filename;
      System.IO.File.WriteAllBytes(copyPath, pdfFileBytes);

      if (data.Aprobacion)
      {
        MailHelper.EnviarSolicitudCertlaboral(
            Empleado_Firma.Tercero.Trim(),
            Empleado_Firma.Dir_Elec.Trim(),
            data.empleado.Empleado.Trim(),
            model.token,
            pdfFileBytes
            );

        db.SOLICITUDES.Add(new SOLICITUDES
        {
          Fec_Llegada = DateTime.Now,
          Fec_Salida = DateTime.Now,
          Fec_Solicitud = DateTime.Now,
          Cantidad = 1,
          Tipo_Solicitud = "O",
          Modo_Vacaciones = "O",
          Cod_Aprobador = 1,
          Cod_Empleado = data.empleado.Cod_Empleado,
          Observacion = "/Static/CertificadosBK/" + filename,
          Cod_Concepto = 0,
          Estado = "P"
        });
        db.SaveChanges();
        return null;
      }
      else
      {

        string mensaje = "Cod Empleado: {0} genero un nuevo Certificado laboral digido a {1}, cod verificación: {2}";
        mensaje = String.Format(mensaje, data.empleado.Cod_Empleado, data.dirigido, token);
        AuditoriaHelper.Log(db, "CERTLAB", "N", mensaje);
        return File(pdfFileBytes, "application/pdf", "Certificado Laboral");
      }

    }

    public ActionResult Prueba5(CertLaboralViewModel data)
    {
      JulianaContext db = new JulianaContext(data.DBName);
      var plantilla_base = db.PLANTILLAS_CERTIFICADOS.SingleOrDefault(p => p.Autonum == data.Cod_Plantilla);
      var filenamepath = System.Web.Hosting.HostingEnvironment.MapPath("~" + plantilla_base.Plantilla);
      if (!System.IO.File.Exists(filenamepath))
      {
        return Json(new { Error = "No existe el archivo", filenamepath });
      }

      var document = new Aspose.Words.Document(filenamepath);
      MemoryStream ms = new MemoryStream();

      PdfSaveOptions options = new PdfSaveOptions();

      // Create encryption details and set owner password.
      PdfEncryptionDetails encryptionDetails = new PdfEncryptionDetails("Prueba", "password", PdfEncryptionAlgorithm.RC4_128);

      // Start by disallowing all permissions.
      encryptionDetails.Permissions = PdfPermissions.DisallowAll;
      // Extend permissions to allow editing or modifying annotations.
      encryptionDetails.Permissions = PdfPermissions.ModifyAnnotations | PdfPermissions.DocumentAssembly;
      options.EncryptionDetails = encryptionDetails;


      document.Save(ms, options);
      return File(ms.ToArray(), "application/pdf", "CertLaboral.pdf");

    }

    //Generador de certificados laborales etc.
    public ActionResult Prueba2(CertLaboralViewModel data)
    {


      Debug.WriteLine("DBName: " + data.DBName);
      Debug.WriteLine("Cod_Plantilla: " + data.Cod_Plantilla);
      Debug.WriteLine("Empleado " + data.empleado);
      Debug.WriteLine("Cod_Empleado: " + data.empleado.Cod_Empleado);
      Debug.WriteLine("Dirigido: " + data.dirigido);
      Debug.WriteLine("Dirigido Embajada: " + data.dirigidoEmbajada);
      Debug.WriteLine("Viaje Laboral: " + data.viajeLaboral);
      Debug.WriteLine("Fecha Ingreso: " + data.empleado.Fec_Ingreso);
      Debug.WriteLine("Empleado Salario: " + data.empleado.Salario);
      Debug.WriteLine("Empleado Tipo de Contrato: " + data.empleado.Tipo_Contrato);
      Debug.WriteLine("Empleado codigo de cargo: " + data.empleado.Cod_Cargo);
      Debug.WriteLine("Empleado Codigo de Departamento:  " + data.empleado.Cod_Depto);
      Debug.WriteLine("Empleado Codigo de Costos: " + data.empleado.Cod_Ccostos);
      Debug.WriteLine("Empleado Codigo de sucursal: " + data.empleado.Cod_Sucursal);
      Debug.WriteLine("Empleado Codigo de zona: " + data.empleado.Cod_Zona);


      JulianaContext db = new JulianaContext(data.DBName);
      var plantilla_base = db.PLANTILLAS_CERTIFICADOS.SingleOrDefault(p => p.Autonum == data.Cod_Plantilla);
      Document document = new Document();
      var filenamepath = System.Web.Hosting.HostingEnvironment.MapPath("~" + plantilla_base.Plantilla);
      if (!System.IO.File.Exists(filenamepath))
      {
        return Json(new { Error = "No existe el archivo", filenamepath });
      }
      document.LoadFromFile(filenamepath);
      string htmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/certlab.cshtml");
      string logoEmpresa = "Static/images/logos/" + data.DBName + ".png";
      string Plantilla_Certificado = System.IO.File.ReadAllText(htmlPath);


      TERCEROS Empleado_Firma = null;
      string Cargo_Empleado_Firma = "";

      string Dir_Elec_Empleado_Firma = "";

      data.empleado = db.EMPLEADOS.Single(e => e.Cod_Empleado == data.empleado.Cod_Empleado);

      // Generando codigo unicó de verificación
      string token = data.empleado.Cod_Empleador.ToString();
      token += data.empleado.Cod_Empleado.ToString();
      token += "_" + DateTime.Now.Ticks;

      CertLaboralTemplatewModel model = new CertLaboralTemplatewModel
      {
        empleado = data.empleado,
        dirigido = data.dirigido,
        logo = logoEmpresa,
        dirigidoEmbajada = data.dirigidoEmbajada,
        viajeLaboral = data.viajeLaboral,
        fecha = DateTime.Now,
        fechaIngreso = UtilHelper.getDate(data.empleado.Fec_Ingreso),
        token = token,
        autoriza = "Autoriza:",
        autorizaCargo = "Cargo:",
        salario = UtilHelper.numberToLetter(data.empleado.Salario.ToString())
      };
      model.salario = model.salario.Replace("  ", " ");

      var currentDate = DateTime.Now.AddDays(-91).ToString("yyyyMM");
      var date = currentDate + "01";

      var comisiones = from H in db.HISTORICO
                       join C in db.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                       where
                         C.Nom_Concepto.Contains("COMISION") &&
                         String.Compare(H.Fec_Nomina, date) >= 0 &&
                         H.Estado == "P" &&
                         C.Devengo == "S" &&
                         H.Cod_Empleado == data.empleado.Cod_Empleado
                       group H by new
                       {
                         H.Cod_Empleado,
                         H.Cod_Concepto
                       } into g
                       select new
                       {
                         g.Key.Cod_Empleado,
                         g.Key.Cod_Concepto,
                         Val_Novedad = (double)g.Sum(p => p.Val_Novedad),
                         Promedio = (double)(g.Sum(p => p.Val_Novedad) / 90 * 30)
                       };
      var currentDates = DateTime.Now.AddDays(-91).ToString("yyyyMM");
      var dates = currentDates + "01";
      var transporte = from H in db.HISTORICO
                       join C in db.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                       where
                         C.Nom_Concepto.Contains("TRANSPORTE") &&
                         String.Compare(H.Fec_Nomina, dates) >= 0 &&
                         H.Estado == "P" &&
                         C.Devengo == "S" &&
                         H.Cod_Empleado == data.empleado.Cod_Empleado
                       group H by new
                       {
                         H.Cod_Empleado,
                         H.Cod_Concepto
                       } into g
                       select new
                       {
                         g.Key.Cod_Empleado,
                         g.Key.Cod_Concepto,
                         Val_Novedad = (double)g.Sum(p => p.Val_Novedad),
                         Promedio = (double)(g.Sum(p => p.Val_Novedad) / 90 * 30)
                       };

      double totalComisiones = 0;
      foreach (var valor in comisiones)
      {
        totalComisiones += valor.Promedio;
      }

      double totalTrasnporte = 0;
      foreach (var valor in transporte)
      {
        totalTrasnporte += valor.Val_Novedad;
      }
      string tipoContrato = "";
      string ContracTypeCert = "";
      
      switch (data.empleado.Tipo_Contrato)
      {
       case "1":
          tipoContrato = "A TERMINO INDEFINIDO";
          ContracTypeCert = "INDEFINITE TERM";
          break;
        case "2":
          tipoContrato = "A TERMINO FIJO";
          ContracTypeCert = "FIXED TERM";
          break;
        case "3":
          tipoContrato = "DE APRENDIZAJE";
          ContracTypeCert = "LEARNING";
          break;
        case "4":
          tipoContrato = "COMO PRACTICANTE UNIVERSITARIO CON ARL";
          ContracTypeCert = "UNIVERSITY INTERNSHIP WITH ARL";
          break;
        case "5":
          tipoContrato = "COMO PRACTICANTE UNIVERSITARIO SIN APORTES";
          ContracTypeCert = "UNIVERSITY INTERNSHIP WITHOUT CONTRIBUTIONS";
          break;
        case "6":
          tipoContrato = "COMO ESTUDIANTE APRENDIZ";
          ContracTypeCert = "STUDENT APPRENTICE";
          break;
        case "7":
          tipoContrato = "PENSIONADO POR EMPRESA";
          ContracTypeCert = "PENSIONER BY COMPANY";
          break;
        case "8":
          tipoContrato = "DE OBRA O LABOR";
          ContracTypeCert = "WORK OR LABOR";
          break;
      }

      model.tipoContrato = tipoContrato;


      PARAMETROS_CERTLAB config = null;

      EMPRESAS Empresa = db.EMPRESAS.SingleOrDefault(e => e.Codigo == data.empleado.Cod_Empleador);
      CIUDADES ciudad = db.CIUDADES.SingleOrDefault(c => c.Codigo == Empresa.Cod_Ciudad);
      CARGOS cargo = db.CARGOS.SingleOrDefault(c => c.Cod_Cargo == data.empleado.Cod_Cargo);

      var configQuery = from c in db.PARAMETROS_CERTLAB
                        where
                        (c.Filter == FILTER_EMPLEADO && c.Cod_Filter == data.empleado.Cod_Empleado)
                        || (c.Filter == FILTER_CARGO && c.Cod_Filter == data.empleado.Cod_Cargo)
                        || (c.Filter == FILTER_DEPTO && c.Cod_Filter == data.empleado.Cod_Depto)
                        || (c.Filter == FILTER_CCOSTO && c.Cod_Filter == data.empleado.Cod_Ccostos)
                        || (c.Filter == FILTER_SUCURSAL && c.Cod_Filter == data.empleado.Cod_Sucursal)
                        || (c.Filter == FILTER_ZONA && c.Cod_Filter == data.empleado.Cod_Zona)
                        orderby c.Filter
                        select c;

      config = configQuery.FirstOrDefault();

      if (config == null)
      {
        config = db.PARAMETROS_CERTLAB.FirstOrDefault(c => c.Filter == FILTER_DEFAULT);
      }

      model.Firmar_Certlab = config.Firma_Digital;
      if (config.Firma_Filename != null)
      {

        //string firmaFilename = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/images/firmas/" + config.Firma_Filename);
        //model.firma = firmaFilename;
      }
      model.ShowotrosDevengos = config.Otros_Devengos.Value;
      model.ShowHorasExtras = config.Horas_Extras.Value;
      model.showBeneficios = config.Base_Salario.Value;
      model.showOtrosIngresos = config.Otros_Ingresos.Value;
      model.textEmbajada = config.Texto_Embajada;
      model.textViajeLaboral = config.Texto_Viaje_Laboral;
      model.mesesPromedio = config.Meses_Promedio;
      model.otrosConceptos = config.Conceptos;
      //if (config.Cod_Empleado_Autoriza != null)
      //{
      //  Empleado_Firma = db.TERCEROS.Where(t => t.Cod_Tercero == config.Cod_Empleado_Autoriza)
      //                              .ToList().First();
      //  Dir_Elec_Empleado_Firma = Empleado_Firma.Dir_Elec.Trim();
      //}
      List<short> codsConceptos = new List<short>();
      if (!string.IsNullOrWhiteSpace(model.otrosConceptos))
      {
        foreach (string cod in model.otrosConceptos.Split(','))
        {
          short codigoShort;

          if (short.TryParse(cod, out codigoShort))
          {
            codsConceptos.Add(codigoShort);
          }

        }
      }



      model.empresa = Empresa;
      model.ciudad = ciudad.Nom_Ciudad;
      model.cargo = cargo.Nom_Cargo;

      // Calculo de fechas para promedio de otros devengos
      int mesesPromedio = Convert.ToInt32(model.mesesPromedio);
      int diasXMes = 30;

      DateTime fechaActual = DateTime.Now;
      DateTime fechaIngreso = UtilHelper.getDate(data.empleado.Fec_Ingreso);
      DateTime fechaInicioPromedio = fechaActual.AddDays(-mesesPromedio * diasXMes);
      if (fechaIngreso > fechaInicioPromedio)
      {
        fechaInicioPromedio = fechaIngreso;
      }
      TimeSpan intervalo = fechaActual - fechaInicioPromedio;
      int diasPromedio = intervalo.Days;
      string fechaInicio = UtilHelper.getUnglyDate(fechaInicioPromedio);
      string fechaFinal = UtilHelper.getUnglyDate(fechaActual);

      // Horas extras
      if (model.ShowHorasExtras)
      {
        var qHorasExtras = from h in db.HISTORICO
                           join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                           where h.Cod_Empleado == data.empleado.Cod_Empleado
                               && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                               && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                               && c.Tipo_Concepto == "P"
                           select new
                           {
                             h.Val_Novedad
                           };

        var novedadesHorasExtras = qHorasExtras.ToList();
        double totalHorasExtras = 0;
        novedadesHorasExtras.ForEach(n => totalHorasExtras += n.Val_Novedad);
        double promedioHorasExtras = (totalHorasExtras / diasPromedio) * diasXMes;
        model.promedioHorasExtras = promedioHorasExtras;
        if (model.promedioHorasExtras <= 0)
        {
          model.ShowHorasExtras = false;
        }
      }
      else
      {
        model.promedioHorasExtras = 0;
      }

      // Beneficios salario, conceptos 8, 9, 0 que sean devengos
      string[] conceptosBeneficioSalario = new string[] { "8", "9", "0" };
      if (model.showBeneficios)
      {
        var qBeneficios = from h in db.HISTORICO
                          join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                          where h.Cod_Empleado == data.empleado.Cod_Empleado
                              && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                              && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                              && c.Devengo == "S"
                              && c.BSBenSalario == "S"
                          select new
                          {
                            h.Val_Novedad
                          };

        var novedadesBeneficios = qBeneficios.ToList();
        double totalBeneficios = 0;
        novedadesBeneficios.ForEach(b => totalBeneficios += b.Val_Novedad);
        double promedioBeneficios = totalBeneficios / mesesPromedio;
        model.promedioBeneficios = promedioBeneficios;
        if (model.promedioBeneficios <= 0)
        {
          model.showBeneficios = false;
        }
      }
      else
      {
        model.promedioBeneficios = 0;
      }

      var conceptosEspecificos = new Dictionary<string, double>();
      double promedioConceptosEspecificos = 0;
      // Conceptos especifico
      foreach (var cod in codsConceptos)
      {
        var concepto = db.CONCEPTOS.SingleOrDefault(c => c.Cod_Concepto == cod);
        //add
        if (concepto != null)
        {

          var qconcepto = from h in db.HISTORICO
                          join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                          where h.Cod_Empleado == data.empleado.Cod_Empleado
                          && h.Cod_Concepto == concepto.Cod_Concepto
                          && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                          && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                          select h.Val_Novedad;
          double totalConcepto = 0;
          foreach (double valor in qconcepto)
          {
            totalConcepto += valor;
          }
          double promedioConcepto = totalConcepto / mesesPromedio;
          promedioConceptosEspecificos += promedioConcepto;
          if (promedioConcepto > 0)
          {
            conceptosEspecificos.Add(concepto.Nom_Concepto.Trim(), promedioConcepto);
          }
        } //end
      }

      // Conceptos PRACTIA
      var qAuxConec = from h in db.HISTORICO
                      join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                      where h.Cod_Empleado == data.empleado.Cod_Empleado
                      && h.Cod_Concepto == 250
                      && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                      && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                      select h.Val_Novedad;
      double VaAuxConec = 0;
      foreach (double valor in qAuxConec)
      {
        VaAuxConec += valor;
      }

      double ValAuxConec = VaAuxConec / qAuxConec.Count();
      if (qAuxConec.Count() < 0)
      {
        ValAuxConec = 0;
      }

      // end Conceptos PRACTIA

      model.conceptosEspecificos = conceptosEspecificos;

      // otros ingresos
      string[] tipoConceptosSumados = new string[] { "8", "9", "0", "P" };
      codsConceptos.Add(1);
      codsConceptos.Add(2);
      if (model.showOtrosIngresos)
      {
        var qOtrosIngresos = from h in db.HISTORICO
                             join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                             where h.Cod_Empleado == data.empleado.Cod_Empleado
                                 && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                                 && h.Fec_Nomina.CompareTo(fechaFinal) <= 0
                                 && c.Devengo == "S"
                                 && !tipoConceptosSumados.Contains(c.Tipo_Concepto)
                                 && !codsConceptos.Contains(c.Cod_Concepto)
                             select new
                             {
                               h.Val_Novedad
                             };
        var novedadesOtroIngresos = qOtrosIngresos.ToList();
        double totalotrosIngresos = 0;
        novedadesOtroIngresos.ForEach(n => totalotrosIngresos += n.Val_Novedad);
        double promedioOtroIngresos = totalotrosIngresos / mesesPromedio;
        model.promedioOtroIngresos = promedioOtroIngresos;
        if (model.promedioOtroIngresos <= 0)
        {
          model.showOtrosIngresos = false;
        }
      }
      else
      {
        model.promedioOtroIngresos = 0;
      }

      model.netoAPagar = model.promedioBeneficios + model.promedioHorasExtras + model.promedioOtroIngresos + promedioConceptosEspecificos + data.empleado.Salario;

      model.ShowotrosDevengos = model.ShowHorasExtras
                                || model.showBeneficios
                                || model.showOtrosIngresos
                                || conceptosEspecificos.Count > 0;


      //string PDF = Engine.Razor.RunCompile(htmlTemplate, "certlaboral", model.GetType(), model);


      var Fec_Nomina = UtilHelper.getUnglyDate(DateTime.Now);
      var cbeneficios = from n in db.OTRASNOV
                        join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                        where n.Cod_Empleado == data.empleado.Cod_Empleado
                        && n.Estado == "A"
                        && n.Tipo == "P"
                        && c.BSBenSalario == "S"
                        && c.BSIngresoTotal != "S"
                        && n.Vigencia.CompareTo(Fec_Nomina) >= 0
                        select new
                        {
                          n.Val_OtrasNov
                        };
      //var cbeneficiosNS = from n in db.VARIABLES
      //                  join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
      //                  where n.Cod_Empleado == data.empleado.Cod_Empleado
      //                  && n.Estado == "A"
      //                  && n.Tipo == "P"
      //                  && c.BSBenSalario == "S"
      //                  && n.Vigencia.CompareTo(Fec_Nomina) >= 0
      //                  select new
      //                  {
      //                    n.Val_OtrasNov
      //                  };
      string[] tipoConceptos_ns = new string[] { "8", "9", "0" };
      var cbeneficios_NS = from n in db.OTRASNOV
                           join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                           where n.Cod_Empleado == data.empleado.Cod_Empleado
                           && n.Estado == "A"
                           && c.Devengo == "S"
                           && c.BSBenSalario == "N"
                           && tipoConceptos_ns.Contains(c.Tipo_Concepto)
                           && n.Vigencia.CompareTo(Fec_Nomina) >= 0
                           select new
                           {
                             n.Val_OtrasNov
                           };
      var cbeneficios_NS_PAG = from n in db.OTRASNOV
                        join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                        where n.Cod_Empleado == data.empleado.Cod_Empleado
                        && n.Estado == "A"
                        && n.Tipo == "P"
                        && c.BSIngresoTotal == "S"
                        && n.Vigencia.CompareTo(Fec_Nomina) >= 0
                        select new
                        {
                          n.Val_OtrasNov
                        };
      double beneficios = 0;
      double beneficios_ns = 0;
      double beneficios_ns_pag = 0;
      if (cbeneficios.Count() > 0)
      {
        beneficios = cbeneficios.Sum(c => c.Val_OtrasNov);
      }
      if (cbeneficios_NS.Count() > 0)
      {
        beneficios_ns = cbeneficios_NS.Sum(c => c.Val_OtrasNov);
      }
      if (cbeneficios_NS_PAG.Count() > 0)
      {
        beneficios_ns_pag = cbeneficios_NS_PAG.Sum(c => c.Val_OtrasNov);
        beneficios_ns = beneficios_ns + beneficios_ns_pag;
      }

      string Nom_Empleado = data.empleado.PNombre.Trim();
      Nom_Empleado += data.empleado.SNombre.Trim() != String.Empty ? " " + data.empleado.SNombre.Trim() : "";
      Nom_Empleado += " " + data.empleado.PApellido.Trim();
      Nom_Empleado += data.empleado.SApellido.Trim() != String.Empty ? " " + data.empleado.SApellido.Trim() : "";


      TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

      string Salario_Letras = UtilHelper.numberToLetter(data.empleado.Salario.ToString()).Replace("  ", " ").ToUpper();
      string Salario_Letras_English = UtilHelper.numberToLetter_English(data.empleado.Salario.ToString()).Replace("  ", " ").ToUpper();

      //Cargue de la plantilla del documento, dentro de la plantilla mebreteada.
      Plantilla_Certificado = Plantilla_Certificado.Replace("@PLANTILLA", plantilla_base.Plantilla);

      //Info de la Empresa
      Plantilla_Certificado = Plantilla_Certificado.Replace("@LOGO", logoEmpresa);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[LOGO_EMPRESA]", "<img src='" + logoEmpresa + "'/>");
      Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_EMPRESA]", Empresa.Nombre_Empresa.Trim().ToLower());
      Plantilla_Certificado = Plantilla_Certificado.Replace("@NOM_EMPRESA", Empresa.Nombre_Empresa.Trim().ToLower());
      document.Replace("@NOM_EMPRESA", Empresa.Nombre_Empresa.Trim(), false, true);
      document.Replace("[NOM_EMPRESA]", Empresa.Nombre_Empresa.Trim(), false, true);
      document.Replace("@NIT", Empresa.Num_Documento.Trim(), false, true);
      document.Replace("[DOC_EMPRESA]", Empresa.Num_Documento.Trim() + "-" + Empresa.Digito_Verificacion.Trim(), false, true);
      document.Replace("[DIR_EMPRESA]", textInfo.ToTitleCase(Empresa.Direccion.Trim().ToLower()), false, true);
      document.Replace("[TEL_EMPRESA]", textInfo.ToTitleCase(Empresa.Tel.Trim()), false, true);
      if (data.dirigido != null)
      {
        document.Replace("[DIRIGIDO]", data.dirigido, false, true);
      }
      document.Replace("[DIRIGIDO]", "", false, true);

      document.Replace("[CIUDAD]", textInfo.ToTitleCase(ciudad.Nom_Ciudad.Trim().ToLower()), false, true);
      document.Replace("[FEC_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("D").ToLower()), false, true);
      document.Replace("[FEC_ACTUAL_ENGLISH]", textInfo.ToTitleCase(DateTime.Now.ToString("D", new CultureInfo("en-US")).ToLower()), false, true);
      document.Replace("[DIA_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("dd")), false, true);
      document.Replace("[DIA_ACTUAL_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(DateTime.Now.Day.ToString())), false, true);
      document.Replace("[MES_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("MMMM")), false, true);
      document.Replace("[ANO_ACTUAL]", textInfo.ToTitleCase(DateTime.Now.ToString("yyyy")), false, true);

      //add
      Debug.WriteLine("Empresa: " + Empresa.Nombre_Empresa);
      Debug.WriteLine("Empresa: " + Empresa.Num_Documento);
      Debug.WriteLine("Empresa: " + Empresa.Direccion);
      Debug.WriteLine("Empresa: " + Empresa.Tel);

      if (Empleado_Firma != null)
      {
        string Nom_Empleado_Firma = Empleado_Firma.Tercero;
        document.Replace("[NOM_FIRMA]", Nom_Empleado_Firma, false, true);
        document.Replace("[CARGO_FIRMA]", Empleado_Firma.Cargo, false, true);
      }
      if (data.empleado.Fec_Retiro.Trim() != "")
      {
        var FecRetiro = UtilHelper.getDate(data.empleado.Fec_Retiro);
        document.Replace("[FEC_RETIRO]", textInfo.ToTitleCase(FecRetiro.ToString("D")), false, true);
        document.Replace("[FEC_RETIRO_ENGLISH]", textInfo.ToTitleCase(FecRetiro.ToString("D", new CultureInfo("en-US")).ToLower()), false, true);
        document.Replace("[DIA_RETIRO]", textInfo.ToTitleCase(FecRetiro.ToString("dd")), false, true);
        document.Replace("[MES_RETIRO]", textInfo.ToTitleCase(FecRetiro.ToString("MMMM")), false, true);
        document.Replace("[ANO_RETIRO]", textInfo.ToTitleCase(FecRetiro.ToString("yyyy")), false, true);
      }
      else
      {
        document.Replace("[FEC_RETIRO]", "", false, true);
        document.Replace("[FEC_RETIRO_ENGLISH]", "", false, true);
      }

      if (data.dirigidoEmbajada)
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[DIR_EMBAJADA]", config.Texto_Embajada);
      }
      else
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[DIR_EMBAJADA]", "");
      }

      if (config.Firma_Digital == true)
      {
        string img = String.Format("<img src='{0}'>", model.firma);
        Plantilla_Certificado = Plantilla_Certificado.Replace("[IMAGEN_FIRMA]", img);
      }
      else
      {
        Plantilla_Certificado = Plantilla_Certificado.Replace("[IMAGEN_FIRMA]", "");
      }

      //AÑADIR EL RESTO
      document.Replace("[FUNCIONES]", cargo.Funciones, false, true);
      document.Replace("[INFO_ADICIONAL]", "", false, true);

      short Cod_Lugar_Expedicion = data.empleado.Cod_Lugar_Expedicion.Value;
      var lug = db.CIUDADES.Where(c => c.Codigo == Cod_Lugar_Expedicion).ToList();
      string Lugar_Expedicion = lug.Count() > 0 ? lug.First().Nom_Ciudad : "";


      var FecIngreso = data.empleado.Fec_Ingreso.Trim();
      var TipoDocumento = data.empleado.Tip_Documento;
      if (data.empleado.FecSubstitucionPatronal.Trim() != string.Empty && data.empleado.FecSubstitucionPatronal.Trim().CompareTo(FecIngreso) <= 0)
      {
        FecIngreso = data.empleado.FecSubstitucionPatronal.Trim();
      }
      if (data.empleado.Tip_Documento == "C")
      {
        TipoDocumento = "Cedula de ciudadania";
      }
      else if (data.empleado.Tip_Documento == "E")
      {
        TipoDocumento = "Cedula de extranjeria";
      }
      else if (data.empleado.Tip_Documento == "I")
      {
        TipoDocumento = "Permiso Especial Permanencia";
      }
      else if (data.empleado.Tip_Documento == "N")
      {
        TipoDocumento = "Nit";
      }
      else if (data.empleado.Tip_Documento == "P")
      {
        TipoDocumento = "Numero de pasaporte";
      }
      else if (data.empleado.Tip_Documento == "R")
      {
        TipoDocumento = "Reg.Civil";
      }
      else if (data.empleado.Tip_Documento == "S")
      {
        TipoDocumento = "Salvo Conducto de Permanencia";
      }
      else if (data.empleado.Tip_Documento == "T")
      {
        TipoDocumento = "T. Identidad";
      }
      else
      {
        TipoDocumento = "Cedula de ciudadania";
      }
      //GENERO
      string gender = data.empleado.Sexo;
      string GeneroSpanish = "";
      if (gender != "M")
        {gender = "She";
        GeneroSpanish = "la Señora";
      }
      else
      {gender = "He";
        GeneroSpanish = "el Señor";
      }
      var salarioBeneficios = Convert.ToInt32(data.empleado.Salario + beneficios);
      document.Replace("[DIA_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(FecIngreso).ToString("dd").ToLower())), false, true);
      document.Replace("[MES_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(FecIngreso).ToString("MMMM").ToLower())), false, true);
      document.Replace("[ANO_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(FecIngreso).ToString("yyyy").ToLower())), false, true);

      document.Replace("[NOM_EMPLEADO]", textInfo.ToTitleCase((Nom_Empleado.ToUpper())), false, true);
      document.Replace("[TIPO_CONTRATO]", tipoContrato, false, true);
      document.Replace("[CONTRACT_TYPE]", ContracTypeCert, false, true);

      document.Replace("[PRONOMBRE]", gender, false, true);
      document.Replace("[PRONOMBRE_SPANISH]", GeneroSpanish, false, true);

      document.Replace("[TIPO_DOCUMENTO]", TipoDocumento, false, true);
      document.Replace("[DOC_EMPLEADO]", data.empleado.Cedula.Trim(), false, true);
      document.Replace("[FEC_INGRESO]", textInfo.ToTitleCase((UtilHelper.getDate(FecIngreso).ToString("D").ToLower())), false, true);
      document.Replace("[FEC_INGRESO_ENGLISH]", textInfo.ToTitleCase((UtilHelper.getDate(FecIngreso).ToString("D", new CultureInfo("en-US")).ToLower())), false, true);
      document.Replace("[SALARIO_LETRAS]", textInfo.ToTitleCase(Salario_Letras.Trim().ToUpper()), false, true);
      document.Replace("[SALARIO_LETRAS_ENGLISH]", textInfo.ToTitleCase(Salario_Letras_English.Trim().ToUpper()), false, true);
      document.Replace("[SALARIO]", data.empleado.Salario.ToString("N0"), false, true);
      document.Replace("[COMISIONES]", totalComisiones.ToString("N0"), false, true);
      document.Replace("[COMISIONES_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(totalComisiones.ToString())), false, true);
      document.Replace("[TRASNPORTE]", totalTrasnporte.ToString("N0"), false, true);
      document.Replace("[TRASNPORTE_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(totalTrasnporte.ToString())), false, true);
      document.Replace("[CARGO_EMPLEADO]", textInfo.ToTitleCase(cargo.Nom_Cargo.ToUpper().Trim()), false, true);
      document.Replace("[LUG_DOC_EMPLEADO]", textInfo.ToTitleCase(Lugar_Expedicion.ToLower().Trim()), false, true);
      document.Replace("[COD_VERIFICACION]", model.token, false, true);
      document.Replace("[SALARIO_BENEFICIOS]", salarioBeneficios.ToString("N0"), false, true);
      document.Replace("[SALARIO_BENEFICIOS_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(salarioBeneficios.ToString())), false, true);
      document.Replace("[OTROS_CONCEPTOS]", promedioConceptosEspecificos.ToString("N0"), false, true);
      document.Replace("[OTROS_CONCEPTOS_LETRAS]", UtilHelper.numberToLetter(promedioConceptosEspecificos.ToString()), false, true);
      document.Replace("[BENEFICIOS]", beneficios.ToString("N0"), false, true);
      document.Replace("[BENEFICIOS_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(beneficios.ToString())), false, true);
      document.Replace("[BENEFICIOS_NS]", beneficios_ns.ToString("N0"), false, true);
      document.Replace("[BENEFICIOS_NS_LETRAS]", textInfo.ToTitleCase(UtilHelper.numberToLetter(beneficios_ns.ToString())), false, true);
      document.Replace("[BENEFICIO_PRACTIA]", ValAuxConec.ToString("N0"), false, true);
      //document.Replace("[BENEFICIO_PRACTIA_LETRA]", UtilHelper.numberToLetter(ValAuxConec.ToString()), false, true);
      //document.Replace("[BENEFICIO_PRACTIA_LETRA_ENGLISH]", UtilHelper.numberToLetter_English(ValAuxConec.ToString()), false, true);


      document.Replace("[token]", model.token, false, true);




      string copyPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/CertificadosBK/");
      var filename = model.token + ".pdf";
      ToPdfParameterList toPdf = new ToPdfParameterList();
      document.SaveToFile(copyPath + filename, toPdf);

      var path = copyPath + filename;


      var pdfFileBytes = getDocumentotoArray(document);

      return File(pdfFileBytes, "application/pdf", "CertLaboral.pdf");


    }

    public ActionResult Politicas(CertLaboralViewModel data)
    {
      JulianaContext db = new JulianaContext(data.DBName);
      var plantilla_base = db.PLANTILLAS_CERTIFICADOS.SingleOrDefault(p => p.Autonum == data.Cod_Plantilla);
      var filenamepath = Server.MapPath("~" + plantilla_base.Plantilla);

      return File(filenamepath, "application/pdf", "politicas.pdf");

    }
    public byte[] getDocumentotoArray(Document doc)
    {
      ToPdfParameterList toPdf = new ToPdfParameterList();
      using (MemoryStream ms = new MemoryStream())
      {
        doc.SaveToStream(ms, toPdf);
        return ms.ToArray();
      }
    }
  }
}
