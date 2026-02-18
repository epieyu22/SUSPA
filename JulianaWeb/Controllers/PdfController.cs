using JulianaWeb.Helpers;
using JulianaWeb.Interfaces.Aspects;
using JulianaWeb.Models;
using JulianaWeb.Models.ViewModels;
using JulianaWeb.Services.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RazorEngine;
using RazorEngine.Compilation.ImpromptuInterface;
using RazorEngine.Templating;
using Spire.Doc;
using Spire.Pdf.General.Render.Decode.Jpeg2000.j2k.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;

namespace JulianaWeb.Controllers
{
  public class PdfController : Controller
  {

    string FILTER_EMPLEADO = "1";
    string FILTER_CARGO = "2";
    string FILTER_DEPTO = "3";
    string FILTER_CCOSTO = "4";
    string FILTER_SUCURSAL = "5";
    string FILTER_ZONA = "6";
    string FILTER_DEFAULT = "999";



    Dictionary<string, string> estadoDisplayName = new Dictionary<string, string>() {
      { "A",  "Aprobada" },
      { "A0",  "Aprobada Nivel 1" },
      { "A1",  "Aprobada Nivel 2" },
      { "A2",  "Aprobada Nivel 3" },
      { "A3",  "Aprobada Nivel 4" },
      { "A4",  "Aprobada Nivel 5" },
      { "A5",  "Aprobada Nivel 6" },
      { "A6",  "Aprobada Nivel 7" },
      { "A7",  "Aprobada Nivel 8" },
      { "A8",  "Aprobada Nivel 9" },
      { "A9",  "Aprobada Nivel 10"},
      { "AP", "Pagada" },
      { "P", "Pendiente Aprobación" },
      { "PR", "Pendiente Desaprobación" },
      { "R", "Rechazada" },
      { "D", "Eliminada" },
    };

    /* el convertidor de HTLM to PDF generará error si hay más de un espacio en blanco para separar un palabra de otra
        recordar siempre hacer Trim,  a los valores esto lo hago en el template, 
        sin embargo en algunaos casos es necesario hacer replace esto se hace desde este archivo. */

    private readonly ILoggerService<PdfController> _logger;

    public PdfController()
    {
      _logger = LoggerServiceFactory.Get<PdfController>();
    }

    public ActionResult CertLaboral(CertLaboralViewModel data)
    {
      string htmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/certlab.cshtml");

      string logoEmpresa = "Static/images/logos/" + data.DBName + ".png";
      string Plantilla_Certificado = System.IO.File.ReadAllText(htmlPath);


      TERCEROS Empleado_Firma = null;
      string Cargo_Empleado_Firma = "";

      string Dir_Elec_Empleado_Firma = "";
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
      //model.calidad = imgCalidad;
      //model.calidad2 = imgCalidad2;
      
      
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
        string firmaFilename = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/images/firmas/" + config.Firma_Filename);
        model.firma = firmaFilename;
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
      if (model.otrosConceptos != null)
      {
        foreach (string cod in model.otrosConceptos.Split(','))
        {
          codsConceptos.Add(short.Parse(cod));
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
        model.BeneficioNoSalarial = totalBeneficios;
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


      //string PDF = Engine.Razor.RunCompile(htmlTemplate, "certlaboral", model.GetType(), model);


      string Nom_Empleado = data.empleado.PNombre.Trim();
      Nom_Empleado += data.empleado.SNombre.Trim() != String.Empty ? " " + data.empleado.SNombre.Trim() : "";
      Nom_Empleado += " " + data.empleado.PApellido.Trim();
      Nom_Empleado += data.empleado.SApellido.Trim() != String.Empty ? " " + data.empleado.SApellido.Trim() : "";

      string Nom_Empleado_Firma = Empleado_Firma.PNombre.Trim();
      Nom_Empleado_Firma += Empleado_Firma.SNombre.Trim() != String.Empty ? " " + Empleado_Firma.SNombre.Trim() : "";
      Nom_Empleado_Firma += " " + Empleado_Firma.PApellido.Trim();
      Nom_Empleado_Firma += Empleado_Firma.SApellido.Trim() != String.Empty ? " " + Empleado_Firma.SApellido.Trim() : "";

      string Salario_Letras = UtilHelper.numberToLetter(data.empleado.Salario.ToString()).Replace("  ", " ").ToUpper();

      string Dias_Letras = UtilHelper.numberToLetter(DateTime.Now.ToString("dd").ToUpper());

      //Cargue de la plantilla del documento, dentro de la plantilla mebreteada.
      Plantilla_Certificado = Plantilla_Certificado.Replace("@PLANTILLA", config.Plantilla);

      //Info de la Empresa
      Plantilla_Certificado = Plantilla_Certificado.Replace("@LOGO", logoEmpresa);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[LOGO_EMPRESA]", "<img src='" + logoEmpresa + "'/>");
      Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_EMPRESA]", Empresa.Nombre_Empresa.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("@NOM_EMPRESA", Empresa.Nombre_Empresa.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("@NIT", Empresa.Num_Documento.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DOC_EMPRESA]", Empresa.Num_Documento.Trim() + "-" + Empresa.Digito_Verificacion.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIR_EMPRESA]", Empresa.Direccion.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[TEL_EMPRESA]", Empresa.Tel.Trim());


      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIRIGIDO]", data.dirigido);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[CIUDAD]", ciudad.Nom_Ciudad);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[FEC_ACTUAL]", DateTime.Now.ToString("D").ToUpper());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIA_ACTUAL]", DateTime.Now.ToString("dd").ToUpper());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DIA_ACTUAL_LETRAS]", Dias_Letras);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[MES_ACTUAL]", DateTime.Now.ToString("MMMM").ToUpper());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[ANO_ACTUAL]", DateTime.Now.ToString("yyyy").ToUpper());




      Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_FIRMA]", Nom_Empleado_Firma);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[CARGO_FIRMA]", Empleado_Firma.Cargo);

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

      //AÑADIR EL RESTo
      Plantilla_Certificado = Plantilla_Certificado.Replace("[FUNCIONES]", cargo.Funciones);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[INFO_ADICIONAL]", "");


      short Cod_Lugar_Expedicion = data.empleado.Cod_Lugar_Expedicion.Value;
      var lug = db.CIUDADES.Where(c => c.Codigo == Cod_Lugar_Expedicion).ToList();
      string Lugar_Expedicion = lug.Count() > 0 ? lug.First().Nom_Ciudad : "";

      Plantilla_Certificado = Plantilla_Certificado.Replace("[NOM_EMPLEADO]", Nom_Empleado);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[TIPO_CONTRATO]", tipoContrato);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[CONTRACT_TYPE]", ContracTypeCert);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[DOC_EMPLEADO]", data.empleado.Cedula.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[FEC_INGRESO]", UtilHelper.getDate(data.empleado.Fec_Ingreso).ToString("D").ToUpper());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[FEC_RETIRO]", UtilHelper.getDate(data.empleado.Fec_Retiro).ToString("D").ToUpper());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO_LETRAS]", Salario_Letras);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO]", data.empleado.Salario.ToString("N0"));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[CARGO_EMPLEADO]", cargo.Nom_Cargo.Trim());
      Plantilla_Certificado = Plantilla_Certificado.Replace("[LUG_DOC_EMPLEADO]", Lugar_Expedicion);
      Plantilla_Certificado = Plantilla_Certificado.Replace("[COD_VERIFICACION]", model.token);
      //Plantilla_Certificado = Plantilla_Certificado.Replace("[SALARIO_BENEFICIOS]", (data.empleado.Salario + model.promedioBeneficios).ToString("N0"));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[BENEFICIOS]", (model.promedioBeneficios).ToString("N0"));
      Plantilla_Certificado = Plantilla_Certificado.Replace("[BENEFICIOS_NS]", (model.BeneficioNoSalarial).ToString("N0"));
      

      string PDF = Plantilla_Certificado;
      string footerPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/certlab-footer.html");
      string footer = System.IO.File.ReadAllText(footerPath);
      footer = footer.Replace("@token", model.token);
      byte[] pdfFileBytes = PdfHelper.newconvert("Certificado Laboral", PDF, model.empleado.Cedula.Trim());

      string copyPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/CertificadosBK/");
      copyPath += model.token + ".pdf";
      System.IO.File.WriteAllBytes(copyPath, pdfFileBytes);

      /*MailHelper.SendCertlabCopyMail(
          model.autoriza,
          mailEmpleadoAutoriza,
          data.empleado.Empleado.Trim(),
          model.token,
          pdfFileBytes
          );*/

      string mensaje = "Cod Empleado: {0} genero un nuevo Certificado laboral digido a {1}, cod verificación: {2}";
      mensaje = String.Format(mensaje, data.empleado.Cod_Empleado, data.dirigido, token);
      AuditoriaHelper.Log(db, "CERTLAB", "N", mensaje);
      return File(pdfFileBytes, "application/pdf", "Certificado Laboral");
    }

    public ActionResult Compropago(CompropagoViewModel data)
    {
      string htmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/compropago.cshtml");
      string logoEmpresa = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/logos/" + data.DBName + ".PNG");
      string htmlTemplate = System.IO.File.ReadAllText(htmlPath);

      // Fechas 
      int day;
      if (data.quincena == "1")
      {
        day = 15;
      }
      else if (data.mes == "02")
      {
        if (Int32.Parse(data.ano) % 4 == 0)
        {
          day = 29;
        }
        else
        {
          day = 28;
        }
      }
      else
      {
        day = 30;
      }

      DateTime fechaNominaActual = UtilHelper.getDate(data.ano + data.mes + day.ToString());
      string dia = day > 9 ? day.ToString() : "0" + day;
      string fechaNomina = data.ano + data.mes + dia;
      string fecchaInicioMes = data.ano + data.mes + "01";
      string fechaNominaTemplate = data.ano + "/" + data.mes + "/" + dia;
      
      CompropagoDataModel4 model = new CompropagoDataModel4
      {
        empleado = data.empleado
      };

      var p1 = new SqlParameter[] { };
      string q1 = string.Format("Update {0}.dbo.CONCEPTOS Set Devengo = 'P' Where Devengo = ''", data.DBName);
      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        SqlHelper.ExecuteNonQuery(conn, q1, p1);
      }

      using (JulianaContext db = new JulianaContext(data.DBName))
      {
        EMPRESAS empresa = db.EMPRESAS.SingleOrDefault(e => e.Codigo == data.empleado.Cod_Empleador);

        CARGOS cargo = db.CARGOS.Single(c => c.Cod_Cargo == data.empleado.Cod_Cargo);
        BANCOS banco = db.BANCOS.FirstOrDefault(b => b.Cod_Alterno == data.empleado.Banco);
        CCOSTOS ccosto = db.CCOSTOS.SingleOrDefault(c => c.Cod_Ccosto == data.empleado.Cod_Ccostos);
        EPS eps = db.EPS.SingleOrDefault(e => e.Cod_Eps == data.empleado.Cod_Eps);
        AFP afp = db.AFP.SingleOrDefault(a => a.Cod_Afp == data.empleado.Cod_Afp);
        PARAMETROS parametros = db.PARAMETROS.SingleOrDefault(p => p.Ano == fechaNominaActual.Year);



        model.general = new CompropagoDataModel2
        {
          razonSocial = empresa.Nombre_Empresa,
          nit = empresa.Num_Documento,
          afp = afp != null ? afp.Nom_Afp : "",
          eps = eps != null ? eps.Nom_Eps : "",
          banco = banco != null ? banco.Banco : "",
          ccosto = ccosto.Nom_Ccosto,
          cargo = cargo.Nom_Cargo,
          fechaNomina = fechaNominaTemplate,
          logo = logoEmpresa
        };


        var query = from h in db.HISTORICO
                    join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                    where
                        h.Fec_Nomina == fechaNomina
                        && h.Cod_Empleado == data.empleado.Cod_Empleado
                        && h.Estado == "P"
                    select new
                    {
                      h.Cod_Concepto,
                      c.Nom_Concepto,
                      c.Devengo,
                      c.Por_Concepto,
                      c.BSPension,
                      c.BSBenSalario,
                      c.Tipo_Concepto,
                      h.Dias_Novedad,
                      h.Horas_Novedad,
                      h.Val_Novedad
                    } into g
                    group g by new { g.Cod_Concepto, g.Nom_Concepto, g.Devengo, g.Tipo_Concepto, g.BSPension, g.Por_Concepto, g.BSBenSalario, } into x
                    orderby x.Key.Devengo descending, x.Key.Cod_Concepto
                    select new CompropagoDataModel1
                    {
                      Cod_Concepto = x.Key.Cod_Concepto,
                      Nom_Concepto = x.Key.Nom_Concepto,
                      Devengo = x.Key.Devengo,
                      Tipo_Concepto = x.Key.Tipo_Concepto,
                      Dias_Novedad = x.Sum(i => i.Dias_Novedad),
                      Horas_novedad = x.Sum(i => i.Horas_Novedad),
                      Valor_Novedad = x.Sum(i => i.Val_Novedad),
                      BSPension = x.Key.BSPension,
                      Por_Concepto = x.Key.Por_Concepto,
                      BSBenSalario = x.Key.BSBenSalario
                    };

        var compropago = query.ToList();
        var otrosPagosConceptos = new string[] { "0", "7", "8", "9" };
        model.otrosPagos = compropago.FindAll(c => (otrosPagosConceptos.Contains(c.Tipo_Concepto) || c.BSBenSalario == "S") && c.Devengo == "S");
        model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto) && c.BSBenSalario != "S");


        var ultimaFec_nomina = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Cod_Empleado == data.empleado.Cod_Empleado).Max(h => h.Fec_Nomina);

        var query2 = from h in db.HISTORICO_AUTOLIQUIDACIONES
                     join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                     where
                       h.Fec_Nomina == ultimaFec_nomina
                       && h.Cod_Empleado == data.empleado.Cod_Empleado
                     orderby h.Fec_Nomina, h.Cod_Concepto
                     select new CompropagoDataModel3
                     {
                       Cod_Concepto = h.Cod_Concepto,
                       Nom_Concepto = c.Nom_Concepto,
                       Val_IBC = h.Val_IBC,
                       Dias_Novedad = h.Dias_Novedad,
                       Val_Novedad = h.Val_Novedad,
                       Devengo = c.Devengo
                     };
        model.general.ultimaFecNomina = UtilHelper.getDate(ultimaFec_nomina);
        var aportes = query2.ToList();
        var aportesAMostrar = new string[] { "6", "7", "11", "46" };
        model.aportes = aportes.FindAll(a => aportesAMostrar.Contains(a.Cod_Concepto.ToString()));
        model.aporteSalud = parametros.ApoSalud;
        model.aportePension = parametros.ApoFondoPen;
        model.aporteRiesgo = parametros.ApoPriesProf;

        double basePension = 0;
        foreach (var c in compropago)
        {
          if (c.Devengo == "S" || c.Devengo == "P")
          {
            if (c.BSPension == "S")
            {
              basePension += c.Valor_Novedad;
            }
          }
        }

        if (basePension > (parametros.Salmin * 4))
        {
          if (data.empleado.Tipo_Salario == "2")
          {
            basePension *= 0.7;
          }
          double cantidadSalMins = Math.Truncate(basePension / parametros.Salmin);
          var rango = db.RANGOSSOLPENSION
                          .Where(r => cantidadSalMins >= r.Desde && cantidadSalMins <= r.Hasta)
                          .OrderByDescending(r => r.Desde);
          if (rango != null)
          {
            model.PorSolPen = rango.First().Porc_Sol_Pen;
          }

        }
        else
        {
          model.PorSolPen = 0;
        }
      }

      var p2 = new SqlParameter[] { };
      string q2 = string.Format("Update {0}.dbo.CONCEPTOS Set Devengo = '' Where Devengo = 'P'", data.DBName);
      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        SqlHelper.ExecuteNonQuery(conn, q2, p2);
      }
      string PDF = Engine.Razor.RunCompile(htmlTemplate, "compropago", model.GetType(), model);
      byte[] pdfFileBytes = PdfHelper.newconvert("Comprobante pago", PDF);
      return File(pdfFileBytes, "application/pdf", "Comprobante pago");
    }

    public ActionResult CompropagoNew(CompropagoViewModel data)
    {
      string logoEmpresa = "Static/images/logos/" + data.DBName + ".png";
      string PDF = "";

      // Fechas
      int diaQuincena;
      if (data.quincena == "1") { diaQuincena = 15; }
      else if (data.mes == "02")
      {
        if (DateTime.IsLeapYear(Convert.ToInt32(data.ano))) { diaQuincena = 29; }
        else { diaQuincena = 28; }  
      }
      else { diaQuincena = 30; }

      DateTime fechaNominaActual = UtilHelper.getDate(data.ano + data.mes + diaQuincena.ToString());
      string dia = diaQuincena > 9 ? diaQuincena.ToString() : "0" + diaQuincena;
      string fechaNomina = data.ano + data.mes + dia;
      string fecchaInicioMes = data.ano + data.mes + "01";
      string fechaNominaTemplate = data.ano + "/" + data.mes + "/" + dia;


      JulianaContext db = new JulianaContext(data.DBName);

      PARAMETROS parametros = db.PARAMETROS.SingleOrDefault(p => p.Ano == fechaNominaActual.Year);

      foreach (EMPLEADOS empleado in data.empleados)
      {
        CompropagoDataModel4 model = new CompropagoDataModel4
        {
          empleado = empleado
        };

        EMPRESAS empresa = db.EMPRESAS.SingleOrDefault(e => e.Codigo == empleado.Cod_Empleador);
        CARGOS cargo = db.CARGOS.FirstOrDefault(c => c.Cod_Cargo == empleado.Cod_Cargo);
        BANCOS banco = db.BANCOS.FirstOrDefault(b => b.Cod_Alterno == empleado.Banco);
        CCOSTOS ccosto = db.CCOSTOS.SingleOrDefault(c => c.Cod_Ccosto == empleado.Cod_Ccostos);
        EPS eps = db.EPS.SingleOrDefault(e => e.Cod_Eps == empleado.Cod_Eps);
        AFP afp = db.AFP.SingleOrDefault(a => a.Cod_Afp == empleado.Cod_Afp);

        double Salario = empleado.Salario;
        //var querySalario = db.SALARIOS
        //              .Where(s => s.Fec_Salario.CompareTo(fechaNomina) <= 0 && s.Cod_Empleado == empleado.Cod_Empleado)
        //              .OrderByDescending(s => s.AutoNum);
        var querySalario = db.SALARIOS
              .Where(s => s.Fec_Salario.CompareTo(fechaNomina) <= 0 && s.Cod_Empleado == empleado.Cod_Empleado)
              .OrderByDescending(s => s.Fec_Salario)
              .ThenByDescending(s => s.AutoNum);

        if (querySalario.Count() > 0)
        {
          Salario = querySalario.First().Salario;
        }

        model.general = new CompropagoDataModel2
        {
          razonSocial = empresa.Nombre_Empresa,
          nit = empresa.Num_Documento,
          afp = afp != null ? afp.Nom_Afp : "",
          eps = eps != null ? eps.Nom_Eps : "",
          banco = banco != null ? banco.Banco : "",
          ccosto = ccosto.Nom_Ccosto,
          cargo = cargo.Nom_Cargo,
          fechaNomina = fechaNominaTemplate,
          fechaNominaActual = fechaNominaActual,
          logo = logoEmpresa,
          Salario = Salario
        };

        var query = from h in db.HISTORICO
                    join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                    where
                      h.Fec_Nomina == fechaNomina
                      && h.Cod_Empleado == empleado.Cod_Empleado
                      && h.Estado == "P"
                    select new
                    {
                      h.Cod_Concepto,
                      c.Nom_Concepto,
                      c.Devengo,
                      c.Por_Concepto,
                      c.BSPension,
                      c.BSBenSalario,
                      c.Tipo_Concepto,
                      c.OrdenDevengo,
                      h.Dias_Novedad,
                      h.Horas_Novedad,
                      h.Val_Novedad,
                      h.Cod_Sub_Concepto,
                      h.Porcentaje,
                    } into g
                    group g by new { g.Cod_Concepto, g.Nom_Concepto, g.Devengo, g.OrdenDevengo, g.Cod_Sub_Concepto, g.Tipo_Concepto, g.BSPension, g.Por_Concepto, g.BSBenSalario, g.Porcentaje } into x
                    orderby x.Key.OrdenDevengo, x.Key.Nom_Concepto
                    select new CompropagoDataModel1
                    {
                      Cod_Concepto = x.Key.Cod_Concepto,
                      Nom_Concepto = x.Key.Nom_Concepto,
                      Devengo = x.Key.Devengo,
                      Tipo_Concepto = x.Key.Tipo_Concepto,
                      Dias_Novedad = x.Sum(i => i.Dias_Novedad),
                      Horas_novedad = x.Sum(i => i.Horas_Novedad),
                      Valor_Novedad = x.Sum(i => i.Val_Novedad),
                      BSPension = x.Key.BSPension,
                      Por_Concepto = x.Key.Por_Concepto,
                      BSBenSalario = x.Key.BSBenSalario,
                      OrdenDevengo = x.Key.OrdenDevengo,
                      Porcentaje = x.Key.Porcentaje,
                      Cod_Sub_Concepto = x.Key.Cod_Sub_Concepto
                    };

        var compropago = query.ToList();
        var NoSalarial = query.ToList();
        var bonificationConcepts = new string[] { "0", "9", "8" };
        compropago = compropago.FindAll(c => !bonificationConcepts.Contains(c.Tipo_Concepto) && c.BSBenSalario != "S");
        compropago = compropago.OrderBy(c => c.Cod_Concepto).ToList();


        model.bonificationsNonSalarial = NoSalarial.FindAll(c => bonificationConcepts.Contains(c.Tipo_Concepto) && c.BSBenSalario == "N" && c.Devengo == "S");
        model.bonificationsSalarial = NoSalarial.FindAll(c => bonificationConcepts.Contains(c.Tipo_Concepto) && c.BSBenSalario == "S");

        string archivoPlantillaCertificado = "";
       
        if (data.quincena == "1" && compropago.All(x =>x.Cod_Concepto == 16))
        { 
          archivoPlantillaCertificado = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/compropago-quincena.cshtml");
        }
        else
        {
          archivoPlantillaCertificado = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/compropago.cshtml");
        }
        string plantillaCertificado = System.IO.File.ReadAllText(archivoPlantillaCertificado);
        //Generacion del comprobante de pago.

        var otrosPagosConceptos = new string[] { "0", "7", "8", "9" };
        model.otrosPagos = compropago.FindAll(c => (otrosPagosConceptos.Contains(c.Tipo_Concepto) || c.BSBenSalario == "S"));
        model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto) && c.BSBenSalario != "S");//Actual
        var VariableCuerpoCompropago = db.VARIABLES.Where(v => v.Uso == 33);
        if (VariableCuerpoCompropago.Count() > 0)
        {
          otrosPagosConceptos = new string[] { "0", "9" }; // Aridana
          model.otrosPagos = compropago.FindAll(c => c.Cod_Concepto == 999); // to get a empty list
          model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto)); // Parametro si
        }


        var retefuente = model.pagos.FirstOrDefault(p => p.Cod_Concepto == 5);
        if (retefuente != null)
        {
          model.general.Por_Retefuente = retefuente.Porcentaje;
        }
        var cod_eps = compropago.SingleOrDefault(e => e.Cod_Concepto == 6);
        if (cod_eps != null)
        {
          var epsfecha = db.EPS.SingleOrDefault(e => e.Cod_Eps == cod_eps.Cod_Sub_Concepto);
          model.general.eps = epsfecha != null ? epsfecha.Nom_Eps.Trim() : "";
        }

        var cod_afp = compropago.FirstOrDefault(e => e.Cod_Concepto == 7);
        if (cod_afp != null)
        {
          var afpfecha = db.AFP.SingleOrDefault(e => e.Cod_Afp == cod_afp.Cod_Sub_Concepto);
          model.general.afp = afpfecha != null ? afpfecha.Nom_Afp.Trim() : "";
        }

        var autoliquidaciones = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Fec_Nomina.CompareTo(fechaNomina) <= 0 && h.Cod_Empleado == empleado.Cod_Empleado);

        if (autoliquidaciones.Count() > 0)
        {
          var ultimaFec_nomina = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Fec_Nomina.CompareTo(fechaNomina) <= 0 && h.Cod_Empleado == empleado.Cod_Empleado).Max(h => h.Fec_Nomina);

          var query2 = from h in db.HISTORICO_AUTOLIQUIDACIONES
                       join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                       where
                       h.Fec_Nomina == ultimaFec_nomina
                       && h.Cod_Empleado == empleado.Cod_Empleado
                       orderby h.Fec_Nomina, h.Cod_Concepto
                       select new CompropagoDataModel3
                       {
                         Cod_Concepto = h.Cod_Concepto,
                         Nom_Concepto = c.Nom_Concepto,
                         Val_IBC = h.Val_IBC,
                         Dias_Novedad = h.Dias_Novedad,
                         Val_Novedad = h.Val_Novedad,
                         Devengo = c.Devengo
                       };
          var aportes = query2.ToList();
          var aportesAMostrar = new string[] { "6", "7", "11", "46" };
          model.aportes = aportes.FindAll(a => aportesAMostrar.Contains(a.Cod_Concepto.ToString()));
          model.general.ultimaFecNomina = UtilHelper.getDate(ultimaFec_nomina);
        }
        else
        {
          model.aportes = new List<CompropagoDataModel3>();
          model.general.ultimaFecNomina = UtilHelper.getDate(fechaNomina);
        }

        model.aporteSalud = parametros.ApoSalud;
        model.aportePension = parametros.ApoFondoPen;
        model.aporteRiesgo = parametros.ApoPriesProf;

        double basePension = 0;
        foreach (var c in compropago)
        {
          if (c.OrdenDevengo == "1" || c.OrdenDevengo == "2")
          {
 
            if (c.BSPension == "S" || c.BSPension == "N")
            {
              basePension += c.Valor_Novedad;
            }
          }
        }

        if (basePension > (parametros.Salmin * 4))
        {
          if (empleado.Tipo_Salario == "2")
          {
            basePension *= 0.7;
          }
          double cantidadSalMins = Math.Truncate(basePension / parametros.Salmin);
          var rango = db.RANGOSSOLPENSION
                          .Where(r => cantidadSalMins >= r.Desde && cantidadSalMins <= r.Hasta)
                          .OrderByDescending(r => r.Desde);
          if (rango.Count() > 0)
          {
            model.PorSolPen = rango.First().Porc_Sol_Pen;
          }
        }
        else
        {
          model.PorSolPen = 0;
        }

        PDF += Engine.Razor.RunCompile(plantillaCertificado, DateTime.Now.Ticks.ToString(), model.GetType(), model);
        string auditoria = "Cod Empleado: {0} generó un nuevo comprobante de pago de la fecha de nómina {1}";
        auditoria = String.Format(auditoria, model.empleado.Cod_Empleado, fechaNomina);
        AuditoriaHelper.Log(db, "COMPROPAGO", "N", auditoria);
      }
      
      byte[] pdfFileBytes = PdfHelper.newconvert("Comprobante pago", PDF, data.empleados[0].Cedula.Trim());
      return File(pdfFileBytes, "application/pdf", "Comprobante pago");
    }

    public ActionResult CompropagoZip(CompropagoViewModel data)
    {
      using (var compressedFileStream = new MemoryStream())
      {
        //Create an archive and store the stream in memory.
        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
        {


          string logoEmpresa = "Static/images/logos/" + data.DBName + ".png";
          string PDF = "";

          // Fechas 
          int diaQuincena;
          if (data.quincena == "1")
          {
            diaQuincena = 15;
          }
          else if (data.mes == "02")
          {
            if (DateTime.IsLeapYear(Convert.ToInt32(data.ano))) { diaQuincena = 29; }
            else { diaQuincena = 28; }
          }
          else { diaQuincena = 30; }

          DateTime fechaNominaActual = UtilHelper.getDate(data.ano + data.mes + diaQuincena.ToString());
          string dia = diaQuincena > 9 ? diaQuincena.ToString() : "0" + diaQuincena;
          string fechaNomina = data.ano + data.mes + dia;
          string fecchaInicioMes = data.ano + data.mes + "01";
          string fechaNominaTemplate = data.ano + "/" + data.mes + "/" + dia;

          JulianaContext db = new JulianaContext(data.DBName);

          // Necesario luego esto valores se reversan
          //string sql = @"update CONCEPTOS set Devengo='P' where Devengo='' and Tipo_Concepto = 5";
          //db.Database.ExecuteSqlCommand(sql);


          PARAMETROS parametros = db.PARAMETROS.SingleOrDefault(p => p.Ano == fechaNominaActual.Year);

          foreach (EMPLEADOS empleado in data.empleados)
          {
            CompropagoDataModel4 model = new CompropagoDataModel4
            {
              empleado = empleado
            };

            EMPRESAS empresa = db.EMPRESAS.SingleOrDefault(e => e.Codigo == empleado.Cod_Empleador);
            CARGOS cargo = db.CARGOS.Single(c => c.Cod_Cargo == empleado.Cod_Cargo);
            BANCOS banco = db.BANCOS.FirstOrDefault(b => b.Cod_Alterno == empleado.Banco);
            CCOSTOS ccosto = db.CCOSTOS.SingleOrDefault(c => c.Cod_Ccosto == empleado.Cod_Ccostos);
            EPS eps = db.EPS.SingleOrDefault(e => e.Cod_Eps == empleado.Cod_Eps);
            AFP afp = db.AFP.SingleOrDefault(a => a.Cod_Afp == empleado.Cod_Afp);


            var Salario = db.SALARIOS
                          .Where(s => s.Fec_Salario.CompareTo(fechaNomina) <= 0 && s.Cod_Empleado == empleado.Cod_Empleado)
                          .OrderByDescending(s => s.AutoNum)
                          .Select(s => s.Salario)
                          .First();



            model.general = new CompropagoDataModel2
            {
              razonSocial = empresa.Nombre_Empresa,
              nit = empresa.Num_Documento,
              afp = afp != null ? afp.Nom_Afp : "",
              eps = eps != null ? eps.Nom_Eps : "",
              banco = banco != null ? banco.Banco : "",
              ccosto = ccosto.Nom_Ccosto,
              cargo = cargo.Nom_Cargo,
              fechaNomina = fechaNominaTemplate,
              fechaNominaActual = fechaNominaActual,
              logo = logoEmpresa,
              Salario = Salario
            };

            var query = from h in db.HISTORICO
                        join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                        join e in db.EMPLEADOS on h.Cod_Empleado equals e.Cod_Empleado
                        where
                          h.Fec_Nomina == fechaNomina
                          && h.Cod_Empleado == empleado.Cod_Empleado
                          && h.Estado == "P"
                        select new
                        {
                          e.PNombre,
                          e.SNombre,
                          e.PApellido,
                          e.SApellido,
                          e.Cedula,
                          h.Cod_Concepto,
                          c.Nom_Concepto,
                          c.Devengo,
                          c.Por_Concepto,
                          c.BSPension,
                          c.BSBenSalario,
                          c.Tipo_Concepto,
                          c.OrdenDevengo,
                          h.Cod_Sub_Concepto,
                          h.Dias_Novedad,
                          h.Horas_Novedad,
                          h.Val_Novedad,
                          h.Porcentaje,
                        } into g
                        group g by new { g.Cod_Concepto, g.Nom_Concepto, g.Devengo, g.OrdenDevengo, g.Tipo_Concepto, g.BSPension, g.Por_Concepto, g.BSBenSalario, g.Porcentaje, g.Cod_Sub_Concepto } into x
                        orderby x.Key.OrdenDevengo, x.Key.Cod_Concepto
                        select new CompropagoDataModel1
                        {
                          Cod_Concepto = x.Key.Cod_Concepto,
                          Nom_Concepto = x.Key.Nom_Concepto,
                          Devengo = x.Key.Devengo,
                          Tipo_Concepto = x.Key.Tipo_Concepto,
                          Dias_Novedad = x.Sum(i => i.Dias_Novedad),
                          Horas_novedad = x.Sum(i => i.Horas_Novedad),
                          Valor_Novedad = x.Sum(i => i.Val_Novedad),
                          BSPension = x.Key.BSPension,
                          Por_Concepto = x.Key.Por_Concepto,
                          BSBenSalario = x.Key.BSBenSalario,
                          OrdenDevengo = x.Key.OrdenDevengo,
                          Porcentaje = x.Key.Porcentaje,
                          Cod_Sub_Concepto = x.Key.Cod_Sub_Concepto
                        };

            var compropago = query.ToList();
            compropago = compropago.FindAll(c => c.Tipo_Concepto != "0" && c.BSBenSalario != "S");
            compropago = compropago.FindAll(c => c.Tipo_Concepto != "9" && c.BSBenSalario != "S");

            string archivoPlantillaCertificado = "";

            

            if (data.quincena == "1" && compropago.All(x => x.Cod_Concepto == 16))
            {
              archivoPlantillaCertificado = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/compropago-quincena.cshtml");
            }
            else
            {
              archivoPlantillaCertificado = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/compropago.cshtml");
            }
            string plantillaCertificado = System.IO.File.ReadAllText(archivoPlantillaCertificado);

            

            var otrosPagosConceptos = new string[] { "0", "7", "8", "9" };
            model.otrosPagos = compropago.FindAll(c => (otrosPagosConceptos.Contains(c.Tipo_Concepto) || c.BSBenSalario == "S") /*&& c.Devengo == "S"*/);
            model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto) && c.BSBenSalario != "S");//Actual
            var VariableCuerpoCompropago = db.VARIABLES.Where(v => v.Uso == 33);
            if (VariableCuerpoCompropago.Count() > 0)
            {
              otrosPagosConceptos = new string[] { "0", "9" }; // Aridana
              model.otrosPagos = compropago.FindAll(c => c.Cod_Concepto == 999); // to get a empty list
              model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto)); // Parametro si
            }

            var retefuente = model.pagos.FirstOrDefault(p => p.Cod_Concepto == 5);
            if (retefuente != null)
            {
              model.general.Por_Retefuente = retefuente.Porcentaje;
            }
            var cod_eps = compropago.FirstOrDefault(e => e.Cod_Concepto == 6);
            if (cod_eps != null)
            {
              var epsfecha = db.EPS.SingleOrDefault(e => e.Cod_Eps == cod_eps.Cod_Sub_Concepto);
              model.general.eps = epsfecha != null ? epsfecha.Nom_Eps.Trim() : "";
            }

            var cod_afp = compropago.FirstOrDefault(e => e.Cod_Concepto == 7);
            if (cod_afp != null)
            {
              var afpfecha = db.AFP.SingleOrDefault(e => e.Cod_Afp == cod_afp.Cod_Sub_Concepto);
              model.general.afp = afpfecha != null ? afpfecha.Nom_Afp.Trim() : "";
            }

            //model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto)); // Parametro si
            var autoliquidaciones = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Fec_Nomina.CompareTo(fechaNomina) <= 0 && h.Cod_Empleado == empleado.Cod_Empleado);

            //var cod_eps = compropago.SingleOrDefault(e => e.Cod_Concepto == 6);
            //if(cod_eps != null)
            //{
            //  var epsfecha = db.EPS.SingleOrDefault(e => e.Cod_Eps == cod_eps.Cod_Sub_Concepto);
            //  model.general.eps = epsfecha != null ?  epsfecha.Nom_Eps.Trim() : "";
            //}



            if (autoliquidaciones.Count() > 0)
            {
              var ultimaFec_nomina = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Fec_Nomina.CompareTo(fechaNomina) <= 0 && h.Cod_Empleado == empleado.Cod_Empleado).Max(h => h.Fec_Nomina);

              var query2 = from h in db.HISTORICO_AUTOLIQUIDACIONES
                           join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                           where
                           h.Fec_Nomina == ultimaFec_nomina
                           && h.Cod_Empleado == empleado.Cod_Empleado
                           orderby h.Fec_Nomina, h.Cod_Concepto
                           select new CompropagoDataModel3
                           {
                             Cod_Concepto = h.Cod_Concepto,
                             Nom_Concepto = c.Nom_Concepto,
                             Val_IBC = h.Val_IBC,
                             Dias_Novedad = h.Dias_Novedad,
                             Val_Novedad = h.Val_Novedad,
                             Devengo = c.Devengo
                           };
              var aportes = query2.ToList();
              var aportesAMostrar = new string[] { "6", "7", "11", "46" };
              model.aportes = aportes.FindAll(a => aportesAMostrar.Contains(a.Cod_Concepto.ToString()));
              model.general.ultimaFecNomina = UtilHelper.getDate(ultimaFec_nomina);
            }
            else
            {
              model.aportes = new List<CompropagoDataModel3>();
              model.general.ultimaFecNomina = UtilHelper.getDate(fechaNomina);
            }
            model.aporteSalud = parametros.ApoSalud;
            model.aportePension = parametros.ApoFondoPen;
            model.aporteRiesgo = parametros.ApoPriesProf;

            double basePension = 0;
            foreach (var c in compropago)
            {
              if (c.OrdenDevengo == "1" || c.OrdenDevengo == "2" )
              {
                if (c.BSPension == "S" || c.BSPension == "N")
                {
                  basePension += c.Valor_Novedad;
                }
              }
            }

            if (basePension > (parametros.Salmin * 4))
            {
              if (empleado.Tipo_Salario == "2")
              {
                basePension *= 0.7;
              }
              double cantidadSalMins = Math.Truncate(basePension / parametros.Salmin);
              var rango = db.RANGOSSOLPENSION
                              .Where(r => cantidadSalMins >= r.Desde && cantidadSalMins <= r.Hasta)
                              .OrderByDescending(r => r.Desde);
              if (rango.Count() > 0)
              {
                model.PorSolPen = rango.First().Porc_Sol_Pen;
              }
            }
            else
            {
              model.PorSolPen = 0;
            }

            PDF += Engine.Razor.RunCompile(plantillaCertificado, DateTime.Now.Ticks.ToString(), model.GetType(), model);

            //_logger.LogDebug($"Vouy por acá - Valor de plantilla {plantillaCertificado}");
            //_logger.LogDebug($"Vouy por acá - Valor de plantilla {PDF}");

            string auditoria = "Cod Empleado: {0} generó un nuevo comprobante de pago de la fecha de nómina {1}";
            auditoria = String.Format(auditoria, model.empleado.Cod_Empleado, fechaNomina);
            AuditoriaHelper.Log(db, "COMPROPAGO", "N", auditoria);

            string Filename = model.empleado.Empleado.Trim() + " - " + model.empleado.Cedula.Trim() + " - " + model.empleado.Cod_Empleado.ToString();
            byte[] pdfFileBytes = PdfHelper.newconvert(Filename, PDF);
            var zipEntry = zipArchive.CreateEntry(Filename + ".pdf");

            //Get the stream of the attachment
            using (var originalFileStream = new MemoryStream(pdfFileBytes))
            {
              using (var zipEntryStream = zipEntry.Open())
              {
                //Copy the attachment stream to the zip entry stream
                originalFileStream.CopyTo(zipEntryStream);
              }
            }
            PDF = "";
          }
        }
        return new FileContentResult(compressedFileStream.ToArray(), "application/zip") { FileDownloadName = "Filename.zip" };
      }
    }
    // GET: Pdf
    public ActionResult ReteFuente(RetefuenteViemModel data)
    {
      try
      {
        JulianaContext db = new JulianaContext(data.dbname.Trim());
        string templateFilename = String.Format("~/Static/templates/certificados/dian_202_{0}.html", data.anoContable);
        string htmlPath = System.Web.Hosting.HostingEnvironment.MapPath(templateFilename);
        string htmlTemplate = System.IO.File.ReadAllText(htmlPath);
        string baseURL = UtilHelper.ROOTURL;
        htmlTemplate = htmlTemplate.Replace("@dian", baseURL + "Static/images/dian.PNG");
        htmlTemplate = htmlTemplate.Replace("@muisca", baseURL + "Static/images/muisca.PNG");
        htmlTemplate = htmlTemplate.Replace("@retenedor", baseURL + "Static/images/retenedor.PNG");
        htmlTemplate = htmlTemplate.Replace("@empleado", baseURL + "Static/images/empleado.png");
        htmlTemplate = htmlTemplate.Replace("@baseURL", baseURL);

        string PDF = "";
        string title = "ReteFuente" + data.anoContable + data.dbname;
        double valUvt;

        // necesario para hacer los calculos correctos
        var p1 = new SqlParameter[] { };
        string q1 = string.Format("Update {0}.dbo.CONCEPTOS Set Tipo_Concepto = '1', Devengo = 'S' Where Cod_Concepto In(97,98,100,99)", data.dbname);
        using (var conn = new SqlConnection(SqlHelper.GetConnectionString())) { SqlHelper.ExecuteNonQuery(conn, q1, p1); }

        // Información general de la Empresa.
        EMPRESAS empresa = db.EMPRESAS.FirstOrDefault(e => e.BaseDatos == data.dbname);
        htmlTemplate = htmlTemplate.Replace("@linea5", empresa.Num_Documento.ToString());
        htmlTemplate = htmlTemplate.Replace("@6", empresa.Digito_Verificacion.ToString());
        htmlTemplate = htmlTemplate.Replace("@linea11", empresa.Nombre_Empresa);

        CIUDADES ciudad = db.CIUDADES.FirstOrDefault(c => c.Codigo == empresa.Cod_Ciudad);
        htmlTemplate = htmlTemplate.Replace("@linea33", ciudad.Nom_Ciudad);
        string codsDane = ciudad.Codigo_Dane.ToString();
        htmlTemplate = htmlTemplate.Replace("@34", codsDane.Substring(0, 2));
        htmlTemplate = htmlTemplate.Replace("@linea35", codsDane.Substring(2, 3));

        var ano = Convert.ToInt32(data.anoContable);
        var parametros = db.PARAMETROS.Single(p => p.Ano == ano);

        string cedula_empleado_anterior = "";
        DateTime FechaIngresoMultiple = DateTime.Now;
        DateTime FechaRetiroMultiple = DateTime.Now;
        foreach (EMPLEADOS e in data.empleados)
        {
          if ((cedula_empleado_anterior != e.Cedula && data.ByCedula) || (!data.ByCedula))
          {
            if (data.anoContable == "2015" || data.anoContable == "2016")
            {
              PDF = Retefuente2015(data, e, htmlTemplate, parametros.Val_Uvt);
            }
            else if (data.anoContable == "2017")
            {
              PDF = Retefuente2017(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2018")
            {
              PDF = Retefuente2017(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2019")
            {
              PDF = Retefuente2018(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2020")
            {
              PDF = Retefuente2020(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2021")
            {
              PDF = Retefuente2021(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2022")
            {
              PDF = Retefuente2022(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2023")
            {
              PDF = Retefuente2023(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2024")
            {
              PDF = Retefuente2024(data, e, htmlTemplate, parametros);
            }
            else if (data.anoContable == "2025")
            {
              PDF = Retefuente2025(data, e, htmlTemplate, parametros);
            }
            string Filename = e.Empleado.Trim() + " - " + e.Cedula.Trim() + " - Cod: " + e.Cod_Empleado.ToString();

            string mensaje = "Nuevo Certificado de ingresos y retenciones - Cod Empleado: {0} - Año {1}";
            mensaje = String.Format(mensaje, e.Cod_Empleado, data.anoContable);
            AuditoriaHelper.Log(db, "RETEFUENTE", "N", mensaje);
          }
          cedula_empleado_anterior = e.Cedula;
        }

        //Response.AddHeader("content-disposition", "attachment; filename=" + title);
        byte[] pdfFileBytes = PdfHelper.newconvert(title, PDF, cedula_empleado_anterior.Trim());

        return File(pdfFileBytes, "application/pdf", title);
      }
      catch (Exception e)
      {
        //Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
        return Json(new { Message = e.ToString() });
      }

    }

    public ActionResult RetefuenteZip2(RetefuenteViemModel data)
    {
      using (var compressedFileStream = new MemoryStream())
      {
        //Create an archive and store the stream in memory.
        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
        {
          string templateFilename = String.Format("~/Static/templates/certificados/dian_202_{0}.html", data.anoContable);
          string htmlPath = System.Web.Hosting.HostingEnvironment.MapPath(templateFilename);
          string dianLogo = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/dian.PNG");
          string muiscaLogo = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/muisca.PNG");
          string retenedorLogo = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/retenedor.PNG");
          string empleadoLogo = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/empleado.png");
          string htmlTemplate = System.IO.File.ReadAllText(htmlPath);
          htmlTemplate = htmlTemplate.Replace("@dian", dianLogo);
          htmlTemplate = htmlTemplate.Replace("@muisca", muiscaLogo);
          htmlTemplate = htmlTemplate.Replace("@retenedor", retenedorLogo);
          htmlTemplate = htmlTemplate.Replace("@empleado", empleadoLogo);

          string PDF = "";
          string title = "ReteFuente" + data.anoContable + data.dbname;

          string CodCiudad;
          double valUvt;
          double mSalarioBaseBonoCanasta = 0.0;
          double mValOtrosIngresos = 0;
          double mValSaludPension = 0;
          double mValSolPension = 0;
          double mRentaExcenta = 0;
          double mValRetencion = 0;
          double mTotalBonos = 0;
          double mRedondeoSalarioBase = 0;
          double mValorDeducibleBonos = 0;
          double mTopeBonosCanasta = 0;


          // necesario para hacer los calculos correctos
          var p1 = new SqlParameter[] { };
          string q1 = string.Format("Update {0}.dbo.CONCEPTOS Set Tipo_Concepto = '1', Devengo = 'S' Where Cod_Concepto In(97,98,100,99)", data.dbname);
          using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            SqlHelper.ExecuteNonQuery(conn, q1, p1);
          }

          // información global de las empresas
          var p2 = new[] {
                  new SqlParameter("@codigo", data.dbname)
              };
          string q2 = String.Format("SELECT * FROM {0}.dbo.EMPRESAS WHERE BaseDatos=@codigo", data.dbname);
          using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            var empresaReader = SqlHelper.ExecuteReader(conn, CommandType.Text, q2, p2);
            empresaReader.Read();
            // Información de la empresa
            htmlTemplate = htmlTemplate.Replace("@linea5", empresaReader["Num_Documento"].ToString());
            htmlTemplate = htmlTemplate.Replace("@6", empresaReader["Digito_Verificacion"].ToString());
            htmlTemplate = htmlTemplate.Replace("@linea11", empresaReader["Nombre_Empresa"].ToString());
            CodCiudad = empresaReader["Cod_Ciudad"].ToString();
          }
          var p3 = new[] {
                  new SqlParameter("@codigo", CodCiudad)
              };
          string q3 = String.Format("SELECT * FROM {0}.dbo.CIUDADES WHERE Codigo=@codigo", data.dbname);
          using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            var ciudadReader = SqlHelper.ExecuteReader(conn, CommandType.Text, q3, p3);
            ciudadReader.Read();
            htmlTemplate = htmlTemplate.Replace("@linea33", ciudadReader["Nom_Ciudad"].ToString());
            string codsDane = ciudadReader["Codigo_Dane"].ToString();
            htmlTemplate = htmlTemplate.Replace("@34", codsDane.Substring(0, 2));
            htmlTemplate = htmlTemplate.Replace("@linea35", codsDane.Substring(2, 3));
          }
          var p4 = new[] {
                  new SqlParameter("@ano", data.anoContable)
              };
          string q4 = String.Format("Select Val_Uvt From {0}.dbo.PARAMETROS Where Ano=@ano", data.dbname);
          using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            var parametrosReader = SqlHelper.ExecuteReader(conn, CommandType.Text, q4, p4);
            parametrosReader.Read();
            valUvt = Double.Parse(parametrosReader["Val_Uvt"].ToString());
          }

          string cedula_empleado_anterior = "";
          DateTime FechaIngresoMultiple = DateTime.Now;
          DateTime FechaRetiroMultiple = DateTime.Now;
          foreach (EMPLEADOS e in data.empleados)
          {
            string htmlCode = htmlTemplate;

            // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se nifican los certificado
            DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
            DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
            DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
            DateTime FecSubstitucionPatronal;
            DateTime today = DateTime.Now;
            DateTime maxDate = new DateTime(today.Year, 3, 15);
            e.FecSubstitucionPatronal = e.FecSubstitucionPatronal.Trim();
            if (e.FecSubstitucionPatronal != "")
            {
              FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
            }
            else
            {
              FecSubstitucionPatronal = DateTime.Now;
            }
            if (FecSubstitucionPatronal != DateTime.Now)
            {
              if (fechaIngreso > desde)
              {
                desde = fechaIngreso;
              }
            }
            else
            {
              if (fechaIngreso > desde)
              {
                desde = fechaIngreso;
              }
            }
            if (e.Estado == "R")
            {
              DateTime fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
              if (fechaRetiro < hasta)
              {
                hasta = fechaRetiro;
              }
            }


            if (today > maxDate)
            {
              today = maxDate;
            }


            // información del empleado
            htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
            htmlCode = htmlCode.Replace("@linea25", e.Cedula);
            htmlCode = htmlCode.Replace("@linea26", e.PApellido);
            htmlCode = htmlCode.Replace("@linea27", e.SApellido);
            htmlCode = htmlCode.Replace("@linea28", e.PNombre);
            htmlCode = htmlCode.Replace("@linea29", e.SNombre);

            // renglones
            string retefuenteQuery;
            SqlParameter[] retefuenteParams = new SqlParameter[] { };
            if (data.ByCedula)
            {
              retefuenteParams = new[] {
                          new SqlParameter("@cedula", e.Cedula.ToString()),
                          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
                          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
                      };
              retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, CONCEPTOS.Cod_Concepto ";
              retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
              retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado = 'P' and  EMPLEADOS.Cedula = @cedula group by CertificadoIngresos, Cedula, CONCEPTOS.Cod_Concepto, Devengo";
              retefuenteQuery = String.Format(retefuenteQuery, data.dbname);


              using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
              {
                var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
                if (retefuenteReader.HasRows)
                {
                  double linea37 = 0.0, linea38 = 0.0, linea39 = 0.0, linea40 = 0.0, linea41 = 0.0, linea43 = 0.0, linea44 = 0.0, linea45 = 0.0, linea46 = 0.0;
                  mSalarioBaseBonoCanasta = 0.0;
                  mValOtrosIngresos = 0;
                  mValSaludPension = 0;
                  mValSolPension = 0;
                  mRentaExcenta = 0;
                  mValRetencion = 0;
                  mTotalBonos = 0;
                  mRedondeoSalarioBase = 0;
                  mValorDeducibleBonos = 0;
                  mTopeBonosCanasta = 0;
                  if (cedula_empleado_anterior != e.Cedula)
                  {
                    while (retefuenteReader.Read())
                    {
                      Double Val_Novedad = 0;
                      switch (retefuenteReader["CertificadoIngresos"].ToString())
                      {
                        case "37":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea37 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                          break;
                        case "38":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea38 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                          break;
                        case "39":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea39 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                          break;
                        case "40":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                          break;
                        case "43":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea43 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                          break;
                        case "44":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea44 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                          break;
                        case "45":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea45 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                          break;
                        case "46":
                          Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                          linea46 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                          break;
                      }
                    }


                    // Puta Linea 41
                    var p10 = new[] {
                                      new SqlParameter("@cedula", e.Cedula.ToString()),
                                      new SqlParameter("@fecha_desde", data.anoContable+"0101"),
                                      new SqlParameter("@fecha_hasta", data.anoContable+"1231")
                                  };
                    string q10 = "Select Fec_Nomina,Val_Novedad,HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado = 'P' And CertificadoIngresos = '41' Order By Fec_Nomina,BSBonoRetefuente Desc";
                    //string q10 = "select sum(Val_Novedad) as Val_Novedad, CertificadoIngresos from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto)  WHERE substring(Fec_Nomina,1,4)= @ano And HISTORICO.Estado = 'P' and  HISTORICO.Cod_Empleado = @cod_empleado  and CertificadoIngresos = 41  group by CertificadoIngresos";
                    q10 = String.Format(q10, data.dbname);
                    using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
                    {
                      var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, q10, p10);
                      if (historicoReader.HasRows)
                      {
                        string mFechaNominaSal = "";
                        while (historicoReader.Read())
                        {
                          if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                          {
                            string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                            if (mFechaNominaSal != nFechaNomina)
                            {
                              var p11 = new[] {
                                                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                                                          new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                                                      };
                              string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                              q11 = String.Format(q11, data.dbname);
                              using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                              {
                                var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                                if (salarioReader.HasRows)
                                {
                                  salarioReader.Read();
                                  mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                                }
                                else
                                {
                                  var p12 = new[] {
                                                                  new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                                                                  new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                                                              };
                                  string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                                  q12 = String.Format(q12, data.dbname);
                                  using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                                  {
                                    var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                                    if (salarioReader2.HasRows)
                                    {
                                      salarioReader2.Read();
                                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                                    }
                                  }
                                }

                                ///
                                mTopeBonosCanasta = Math.Round((valUvt * 310), 0);
                                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                                {
                                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                                }
                                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                                {
                                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                                }
                                else
                                {
                                  mValorDeducibleBonos = Math.Round((valUvt * 41), 0);
                                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                                  {
                                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                                  }
                                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                                  if (mTotalBonos > mValorDeducibleBonos)
                                  {
                                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                                  }
                                }
                                mFechaNominaSal = nFechaNomina;
                                mTotalBonos = 0;
                              }
                            }
                            mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                          }
                          else
                          {
                            if (historicoReader["Devengo"].ToString() == "S")
                            {
                              mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                            }
                            else if (historicoReader["Devengo"].ToString() == "N")
                            {
                              mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                            }
                          }
                        }

                        if (mTotalBonos != 0)
                        {
                          mTopeBonosCanasta = Math.Round((valUvt * 310), 0);
                          mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                          if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                          {
                            mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                          }
                          mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                          mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                          mTopeBonosCanasta = mTopeBonosCanasta * 100;
                          if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                          {
                            mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                          }
                          else
                          {
                            mValorDeducibleBonos = Math.Round((valUvt * 41), 0);
                            mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                            if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                            {
                              mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                            }
                            mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                            mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                            mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                            if (mTotalBonos > mValorDeducibleBonos)
                            {
                              mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                            }
                          }
                        }
                        linea41 = mValOtrosIngresos;
                      }
                    }

                    double linea42 = linea37 + linea38 + linea39 + linea40 + linea41;
                    if (data.redondear)
                    {
                      linea37 = UtilHelper.round(linea37);
                      linea38 = UtilHelper.round(linea38);
                      if (linea39 > 0.0)
                      {
                        linea39 = UtilHelper.round(linea39);
                      }

                      if (linea40 > 0.0)
                      {
                        linea40 = UtilHelper.round(linea40);
                      }

                      linea41 = UtilHelper.round(linea41);
                      linea42 = UtilHelper.round(linea42);
                      linea43 = UtilHelper.round(linea43);
                      linea44 = UtilHelper.round(linea44);
                      linea45 = UtilHelper.round(linea45);
                      linea46 = UtilHelper.round(linea46);
                    }

                    htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
                    htmlCode = htmlCode.Replace("@30mm", "01");
                    htmlCode = htmlCode.Replace("@30dd", "01");
                    htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
                    htmlCode = htmlCode.Replace("@31mm", "12");
                    htmlCode = htmlCode.Replace("@31dd", "31");
                    htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
                    htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
                    htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));

                    htmlCode = htmlCode.Replace("@linea37", linea37.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea38", linea38.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea39", linea39.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea40", linea40.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea41", linea41.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea42", linea42.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea43", linea43.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea44", linea44.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea45", linea45.ToString("N0"));
                    htmlCode = htmlCode.Replace("@linea46", linea46.ToString("N0"));
                    PDF += htmlCode;


                    string Filename = e.Empleado.Trim() + " - " + e.Cedula.Trim() + " - " + e.Cod_Empleado.ToString();
                    byte[] pdfFileBytes = PdfHelper.newconvert(Filename, htmlCode);
                    var zipEntry = zipArchive.CreateEntry(Filename + ".pdf");

                    //Get the stream of the attachment
                    using (var originalFileStream = new MemoryStream(pdfFileBytes))
                    {
                      using (var zipEntryStream = zipEntry.Open())
                      {
                        //Copy the attachment stream to the zip entry stream
                        originalFileStream.CopyTo(zipEntryStream);
                      }
                    }
                  }

                }
              }
            }
            else
            {
              retefuenteParams = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_desde", desde),
                          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
                      };
              retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, CONCEPTOS.Cod_Concepto, HISTORICO.Cod_Empleado ";
              retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
              retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado = 'P' and  HISTORICO.Cod_Empleado = @cod_empleado group by CertificadoIngresos, HISTORICO.Cod_Empleado, CONCEPTOS.Cod_Concepto, Devengo";
              retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

              using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
              {
                var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
                if (retefuenteReader.HasRows)
                {
                  double linea37 = 0.0, linea38 = 0.0, linea39 = 0.0, linea40 = 0.0, linea41 = 0.0, linea43 = 0.0, linea44 = 0.0, linea45 = 0.0, linea46 = 0.0;
                  mSalarioBaseBonoCanasta = 0.0;
                  mValOtrosIngresos = 0;
                  mValSaludPension = 0;
                  mValSolPension = 0;
                  mRentaExcenta = 0;
                  mValRetencion = 0;
                  mTotalBonos = 0;
                  mRedondeoSalarioBase = 0;
                  mValorDeducibleBonos = 0;
                  mTopeBonosCanasta = 0;
                  while (retefuenteReader.Read())
                  {
                    Double Val_Novedad = 0;
                    switch (retefuenteReader["CertificadoIngresos"].ToString())
                    {
                      case "37":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea37 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        break;
                      case "38":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea38 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        break;
                      case "39":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea39 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        break;
                      case "40":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        break;
                      case "43":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea43 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                        break;
                      case "44":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea44 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                        break;
                      case "45":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea45 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                        break;
                      case "46":
                        Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                        linea46 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                        break;
                    }
                  }


                  // Otra vez la PUTA linea 41
                  var p10 = new[] {
                      new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                      new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
                      new SqlParameter("@fecha_hasta", data.anoContable+"1231")
                  };
                  string q10 = "Select Fec_Nomina,Val_Novedad,HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado = 'P' And CertificadoIngresos = '41' Order By Fec_Nomina,BSBonoRetefuente Desc";
                  //string q10 = "select sum(Val_Novedad) as Val_Novedad, CertificadoIngresos from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto)  WHERE substring(Fec_Nomina,1,4)= @ano And HISTORICO.Estado = 'P' and  HISTORICO.Cod_Empleado = @cod_empleado  and CertificadoIngresos = 41  group by CertificadoIngresos";
                  q10 = String.Format(q10, data.dbname);
                  using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, q10, p10);
                    if (historicoReader.HasRows)
                    {
                      string mFechaNominaSal = "";
                      while (historicoReader.Read())
                      {
                        if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                        {
                          string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                          if (mFechaNominaSal != nFechaNomina)
                          {
                            var p11 = new[] {
                                      new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                                      new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                                  };
                            string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                            q11 = String.Format(q11, data.dbname);
                            using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                            {
                              var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                              if (salarioReader.HasRows)
                              {
                                salarioReader.Read();
                                mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                              }
                              else
                              {
                                var p12 = new[] {
                                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                                              new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                                          };
                                string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                                q12 = String.Format(q12, data.dbname);
                                using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                                {
                                  var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                                  if (salarioReader2.HasRows)
                                  {
                                    salarioReader2.Read();
                                    mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                                  }
                                }
                              }

                              ///
                              mTopeBonosCanasta = Math.Round((valUvt * 310), 0);
                              mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                              if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                              {
                                mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                              }
                              mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                              mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                              mTopeBonosCanasta = mTopeBonosCanasta * 100;
                              if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                              {
                                mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                              }
                              else
                              {
                                mValorDeducibleBonos = Math.Round((valUvt * 41), 0);
                                mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                                if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                                {
                                  mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                                }
                                mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                                mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                                mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                                if (mTotalBonos > mValorDeducibleBonos)
                                {
                                  mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                                }
                              }
                              mFechaNominaSal = nFechaNomina;
                              mTotalBonos = 0;
                            }
                          }
                          mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                        }
                        else
                        {
                          if (historicoReader["Devengo"].ToString() == "S")
                          {
                            mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                          }
                          else if (historicoReader["Devengo"].ToString() == "N")
                          {
                            mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                          }
                        }
                      }

                      if (mTotalBonos != 0)
                      {
                        mTopeBonosCanasta = Math.Round((valUvt * 310), 0);
                        mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                        if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                        {
                          mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                        }
                        mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                        mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                        mTopeBonosCanasta = mTopeBonosCanasta * 100;
                        if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                        {
                          mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                        }
                        else
                        {
                          mValorDeducibleBonos = Math.Round((valUvt * 41), 0);
                          mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                          if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                          {
                            mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                          }
                          mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                          mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                          mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                          if (mTotalBonos > mValorDeducibleBonos)
                          {
                            mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                          }
                        }
                      }
                      linea41 = mValOtrosIngresos;
                    }
                  }

                  double linea42 = linea37 + linea38 + linea39 + linea40 + linea41;
                  if (data.redondear)
                  {
                    linea37 = UtilHelper.round(linea37);
                    linea38 = UtilHelper.round(linea38);
                    if (linea39 > 0.0)
                    {
                      linea39 = UtilHelper.round(linea39);
                    }

                    if (linea40 > 0.0)
                    {
                      linea40 = UtilHelper.round(linea40);
                    }

                    linea41 = UtilHelper.round(linea41);
                    linea42 = UtilHelper.round(linea42);
                    linea43 = UtilHelper.round(linea43);
                    linea44 = UtilHelper.round(linea44);
                    linea45 = UtilHelper.round(linea45);
                    linea46 = UtilHelper.round(linea46);
                  }


                  htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
                  htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
                  htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));
                  htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
                  htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
                  htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));
                  htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
                  htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
                  htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));

                  htmlCode = htmlCode.Replace("@linea37", linea37.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea38", linea38.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea39", linea39.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea40", linea40.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea41", linea41.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea42", linea42.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea43", linea43.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea44", linea44.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea45", linea45.ToString("N0"));
                  htmlCode = htmlCode.Replace("@linea46", linea46.ToString("N0"));
                  PDF += htmlCode;


                  string Filename = e.Empleado.Trim() + " - " + e.Cedula.Trim() + " - " + e.Cod_Empleado.ToString();
                  byte[] pdfFileBytes = PdfHelper.newconvert(Filename, htmlCode);
                  var zipEntry = zipArchive.CreateEntry(Filename + ".pdf");

                  //Get the stream of the attachment
                  using (var originalFileStream = new MemoryStream(pdfFileBytes))
                  {
                    using (var zipEntryStream = zipEntry.Open())
                    {
                      //Copy the attachment stream to the zip entry stream
                      originalFileStream.CopyTo(zipEntryStream);
                    }
                  }
                }
              }
            }
            cedula_empleado_anterior = e.Cedula;
          }
        }

        return new FileContentResult(compressedFileStream.ToArray(), "application/zip") { FileDownloadName = "Filename.zip" };
      }
    }

    //Admin
    public ActionResult RetefuenteZip(RetefuenteViemModel data)
    {
      byte[] compressedBytes = null;
      using (var compressedFileStream = new MemoryStream())
      {
        //Create an archive and store the stream in memory.
        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, true))
        {
          JulianaContext db = new JulianaContext(data.dbname.Trim());
          string templateFilename = String.Format("~/Static/templates/certificados/dian_202_{0}.html", data.anoContable);
          string htmlPath = System.Web.Hosting.HostingEnvironment.MapPath(templateFilename);
          string htmlTemplate = System.IO.File.ReadAllText(htmlPath);
          string baseURL = UtilHelper.ROOTURL;
          htmlTemplate = htmlTemplate.Replace("@dian", baseURL + "Static/images/dian.PNG");
          htmlTemplate = htmlTemplate.Replace("@muisca", baseURL + "Static/images/muisca.PNG");
          htmlTemplate = htmlTemplate.Replace("@retenedor", baseURL + "Static/images/retenedor.PNG");
          htmlTemplate = htmlTemplate.Replace("@empleado", baseURL + "Static/images/empleado.png");
          htmlTemplate = htmlTemplate.Replace("@baseURL", baseURL);

          string PDF = "";
          string title = "ReteFuente" + data.anoContable + data.dbname;
          double valUvt;
          
          // necesario para hacer los calculos correctos
          var p1 = new SqlParameter[] { };
          string q1 = string.Format("Update {0}.dbo.CONCEPTOS Set Tipo_Concepto = '1', Devengo = 'S' Where Cod_Concepto In(97,98,100,99)", data.dbname);
          using (var conn = new SqlConnection(SqlHelper.GetConnectionString())) { SqlHelper.ExecuteNonQuery(conn, q1, p1); }

          // Información general de la Empresa.
          EMPRESAS empresa = db.EMPRESAS.FirstOrDefault(e => e.BaseDatos == data.dbname);
          htmlTemplate = htmlTemplate.Replace("@linea5", empresa.Num_Documento.ToString());
          htmlTemplate = htmlTemplate.Replace("@6", empresa.Digito_Verificacion.ToString());
          htmlTemplate = htmlTemplate.Replace("@linea11", empresa.Nombre_Empresa);

          CIUDADES ciudad = db.CIUDADES.FirstOrDefault(c => c.Codigo == empresa.Cod_Ciudad);
          htmlTemplate = htmlTemplate.Replace("@linea33", ciudad.Nom_Ciudad);
          string codsDane = ciudad.Codigo_Dane.ToString();
          htmlTemplate = htmlTemplate.Replace("@34", codsDane.Substring(0, 2));
          htmlTemplate = htmlTemplate.Replace("@linea35", codsDane.Substring(2, 3));

          var ano = Convert.ToInt32(data.anoContable);
          var parametros = db.PARAMETROS.Single(p => p.Ano == ano);

          string cedula_empleado_anterior = "";
          DateTime FechaIngresoMultiple = DateTime.Now;
          DateTime FechaRetiroMultiple = DateTime.Now;

          foreach (EMPLEADOS e in data.empleados)
          {
            try
            {
              if ((cedula_empleado_anterior != e.Cedula && data.ByCedula) || (!data.ByCedula))
              {
                if (data.anoContable == "2015" || data.anoContable == "2016")
                {
                  PDF = Retefuente2015(data, e, htmlTemplate, parametros.Val_Uvt);
                }
                else if (data.anoContable == "2017" || data.anoContable == "2018")
                {
                  PDF = Retefuente2017(data, e, htmlTemplate, parametros);
                }
                else if (data.anoContable == "2019")
                {
                  PDF = Retefuente2018(data, e, htmlTemplate, parametros);
                }
                else if (data.anoContable == "2020")
                {
                  PDF = Retefuente2020(data, e, htmlTemplate, parametros);
                }
                else if (data.anoContable == "2021")
                {
                  PDF = Retefuente2021(data, e, htmlTemplate, parametros);
                }
                else if (data.anoContable == "2022")
                {
                  PDF = Retefuente2022(data, e, htmlTemplate, parametros);
                }
                else if (data.anoContable == "2023")
                {
                  PDF = Retefuente2023(data, e, htmlTemplate, parametros);
                }
                else if (data.anoContable == "2024")
                {
                  PDF = Retefuente2024(data, e, htmlTemplate, parametros);
                }
                else if (data.anoContable == "2025")
                {
                  PDF = Retefuente2025(data, e, htmlTemplate, parametros);
                }
                string Filename = e.Empleado.Trim() + " - " + e.Cedula.Trim() + " - Cod: " + e.Cod_Empleado.ToString();
                byte[] pdfFileBytes = PdfHelper.newconvert(Filename, PDF);
                var zipEntry = zipArchive.CreateEntry(Filename + ".pdf", CompressionLevel.Fastest);

               
                BinaryWriter writer = new BinaryWriter(zipEntry.Open());
                writer.Write(pdfFileBytes, 0, pdfFileBytes.Length);
                writer.Close();

                string mensaje = "Nuevo Certificado de ingresos y retenciones - Cod Empleado: {0} - Año {1}";
                mensaje = String.Format(mensaje, e.Cod_Empleado, data.anoContable);
                AuditoriaHelper.Log(db, "RETEFUENTE", "N", mensaje);
              }
              cedula_empleado_anterior = e.Cedula;
            }
            catch (Exception exe)
            {
              return Json(new { exe, e });
            }
          }
        }
        compressedBytes = compressedFileStream.ToArray();
      }
      var p2 = new SqlParameter[] { };
      string q2 = string.Format("Update {0}.dbo.CONCEPTOS Set Tipo_Concepto = '6', Devengo = '' Where Cod_Concepto In(97,98,100,99)", data.dbname);
      using (var conn = new SqlConnection(SqlHelper.GetConnectionString())) { SqlHelper.ExecuteNonQuery(conn, q2, p2); }
      return new FileContentResult(compressedBytes, MediaTypeNames.Application.Zip) { FileDownloadName = "Filename.zip" };
    }

    public ActionResult Solicitud_Vacaciones(PDF_Solicitud_Vacaciones data)
    {
      string Path = "";
      JulianaContext db = new JulianaContext(data.Empresa);
      SOLICITUDES solicitud = db.SOLICITUDES.Where(s => s.Cod_Solicitud == data.Cod_Solicitud).ToList().First();
      if (solicitud.Tipo_Solicitud == "H")
      {
        Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/solicitud_licencia.html");
      }
      else
      {
        Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/solicitud_vacaciones.html");
      }

      string logoEmpresa = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/logos/" + data.Empresa + ".PNG");
      EMPRESAS empresa = db.EMPRESAS.SingleOrDefault(e => e.Codigo == solicitud.empleado.Cod_Empleador);
      string Plantilla = System.IO.File.ReadAllText(Path);
      Plantilla = Plantilla.Replace("@LOGO", logoEmpresa);
      Plantilla = Plantilla.Replace("@Estado", estadoDisplayName[solicitud.Estado.Trim()]);
      Plantilla = Plantilla.Replace("@NOM_EMPRESA", empresa.Nombre_Empresa.Trim());


      if (solicitud.Modo_Vacaciones == "T")
      {
        Plantilla = Plantilla.Replace("@Fec_Salida", solicitud.Fec_Salida.Value.ToString("D"));
        Plantilla = Plantilla.Replace("@Fec_Llegada", solicitud.Fec_Llegada.Value.ToString("D"));
        Plantilla = Plantilla.Replace("@Modo_Vacaciones", "Tiempo");
      }
      else if(solicitud.Modo_Vacaciones != null)
      {
        Plantilla = Plantilla.Replace("@Fec_Salida", solicitud.Fec_Salida.Value.ToString("D"));
        Plantilla = Plantilla.Replace("@Fec_Llegada", "NA");
        Plantilla = Plantilla.Replace("@Modo_Vacaciones", "Dinero");
      }
      else if (solicitud.Tipo_Solicitud == "H")
      { 
        Plantilla = Plantilla.Replace("@Fec_Salida", solicitud.Fec_Salida.Value.ToString("D"));
        Plantilla = Plantilla.Replace("@Fec_Llegada", "NA");
        Plantilla = Plantilla.Replace("@Modo_licencia", solicitud.Tipo_Solicitud);
        Plantilla = Plantilla.Replace("@Descripcion", solicitud.Descripcion);
      }
      else
      {
        Plantilla = Plantilla.Replace("@Fec_Salida", solicitud.Fec_Salida.Value.ToString("D"));
        Plantilla = Plantilla.Replace("@Fec_Llegada", "NA");
        Plantilla = Plantilla.Replace("@Modo_licencia", "Asentismo");
      }

      Plantilla = Plantilla.Replace("@NIT", empresa.Num_Documento);
      Plantilla = Plantilla.Replace("@Empleado", solicitud.empleado.Empleado);
      Plantilla = Plantilla.Replace("@Cantidad", solicitud.Cantidad.ToString());

      Plantilla = Plantilla.Replace("@Fec_Solicitud", solicitud.Fec_Solicitud.ToString("D"));

      Plantilla = Plantilla.Replace("@NOM_EMPRESA", empresa.Nombre_Empresa.Trim());


      var aprobaciones = db.APROBACIONES.Where(a => a.Id_Solicitud == solicitud.Cod_Solicitud);
      string registro = "";
      foreach (var ap in aprobaciones)
      {
        var dee = db.APROBADORES.SingleOrDefault(aaa => aaa.Cod_Aprobador == ap.Id_Aprobador);
        string msg = "<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
        var aprobador = db.TERCEROS.SingleOrDefault(t => t.Cod_Tercero == dee.Cod_Empleado);
        registro += String.Format(msg, aprobador.Tercero, UtilHelper.getDate(ap.Fecha_Aprobado).ToString("D"), ap.Nivel + 1);
      }
      Plantilla = Plantilla.Replace("@aprobaciones", registro);



      var vacaciones = db.VACACIONES.Where(v => v.Cod_Empleado == solicitud.empleado.Cod_Empleado);
      var causadas = vacaciones.Where(v => v.SubPeriodo == 0).ToList();
      var tomadas = vacaciones.Where(v => v.SubPeriodo != 0).OrderBy(t => t.Desde).ToList();

      float diasCausados = 0;
      foreach (var f in causadas)
      {
        diasCausados += f.Dias_Disponibles;
      }
      float diasTomados = 0;
      foreach (var v in tomadas)
      {
        diasTomados += v.Dias_Dinero + v.Dias_Tiempo;
      }
      Plantilla = Plantilla.Replace("@Disponibles", (diasCausados - diasTomados).ToString());

      string PDF = Plantilla;
      string footerPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/certlab-footer.html");
      byte[] pdfFileBytes = PdfHelper.newconvert("Certificado Laboral", PDF);

      //string copyPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/CertificadosBK/");
      return File(pdfFileBytes, "application/pdf", "Solicitud de Vacaciones");
    }

    public ActionResult SRIRetefuente(SRIRetefuenteVM data)
    {



      JulianaContext db = new JulianaContext(data.Empresa);
      EMPLEADOS empleado = data.Empleados[0];
      string Path = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/sri_107.html");
      string Plantilla = System.IO.File.ReadAllText(Path);

      var Nom_Empresa = db.EMPRESAS.FirstOrDefault(e => e.Codigo == empleado.Cod_Empleador).Nombre_Empresa;
      var ano = DateTime.Now.Year;
      var mes = String.Format("{0:00}", DateTime.Now.Month);
      var dia = String.Format("{0:00}", DateTime.Now.Day);

      Plantilla = Plantilla.Replace("@Empresa", Nom_Empresa.Trim());
      Plantilla = Plantilla.Replace("@Empleado", empleado.Empleado.Trim());
      Plantilla = Plantilla.Replace("@Cedula", empleado.Cedula.Trim());
      Plantilla = Plantilla.Replace("@a1", ano.ToString().Substring(0, 1));
      Plantilla = Plantilla.Replace("@a2", ano.ToString().Substring(1, 1));
      Plantilla = Plantilla.Replace("@a3", ano.ToString().Substring(2, 1));
      Plantilla = Plantilla.Replace("@a4", ano.ToString().Substring(3, 1));
      Plantilla = Plantilla.Replace("@m1", mes.Substring(0, 1));
      Plantilla = Plantilla.Replace("@m2", mes.Substring(1, 1));
      Plantilla = Plantilla.Replace("@d1", dia.Substring(0, 1));
      Plantilla = Plantilla.Replace("@d2", dia.Substring(1, 1));


      string PDF = Plantilla;
      byte[] pdfFileBytes = PdfHelper.newconvert("Formulario 107", PDF);

      string copyPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/CertificadosBK/");
      return File(pdfFileBytes, "application/pdf", "Solicitud de Vacaciones");
    }

    public static byte[] GenerarCompropago(CompropagoViewModel data)
    {
      string logoEmpresa = "Static/images/logos/" + data.DBName + ".png";
      string PDF = "";

      // Fechas 
      int diaQuincena;
      if (data.quincena == "1") { diaQuincena = 15; }
      else if (data.mes == "02")
      {
        if (DateTime.IsLeapYear(Convert.ToInt32(data.ano))) { diaQuincena = 29; }
        else { diaQuincena = 28; }
      }
      else { diaQuincena = 30; }

      DateTime fechaNominaActual = UtilHelper.getDate(data.ano + data.mes + diaQuincena.ToString());
      string dia = diaQuincena > 9 ? diaQuincena.ToString() : "0" + diaQuincena;
      string fechaNomina = data.ano + data.mes + dia;
      string fecchaInicioMes = data.ano + data.mes + "01";
      string fechaNominaTemplate = data.ano + "/" + data.mes + "/" + dia;


      JulianaContext db = new JulianaContext(data.DBName);


      PARAMETROS parametros = db.PARAMETROS.SingleOrDefault(p => p.Ano == fechaNominaActual.Year);

      foreach (EMPLEADOS empleado in data.empleados)
      {
        CompropagoDataModel4 model = new CompropagoDataModel4
        {
          empleado = empleado
        };

        EMPRESAS empresa = db.EMPRESAS.SingleOrDefault(e => e.Codigo == empleado.Cod_Empleador);
        CARGOS cargo = db.CARGOS.Single(c => c.Cod_Cargo == empleado.Cod_Cargo);
        BANCOS banco = db.BANCOS.FirstOrDefault(b => b.Cod_Alterno == empleado.Banco);
        CCOSTOS ccosto = db.CCOSTOS.SingleOrDefault(c => c.Cod_Ccosto == empleado.Cod_Ccostos);
        EPS eps = db.EPS.SingleOrDefault(e => e.Cod_Eps == empleado.Cod_Eps);
        AFP afp = db.AFP.SingleOrDefault(a => a.Cod_Afp == empleado.Cod_Afp);


        double Salario = empleado.Salario;
        var querySalario = db.SALARIOS
                      .Where(s => s.Fec_Salario.CompareTo(fechaNomina) <= 0 && s.Cod_Empleado == empleado.Cod_Empleado)
                      .OrderByDescending(s => s.Fec_Salario);
        if (querySalario.Count() > 0)
        {
          Salario = querySalario.First().Salario;
        }


        model.general = new CompropagoDataModel2
        {
          razonSocial = empresa.Nombre_Empresa,
          nit = empresa.Num_Documento,
          afp = afp != null ? afp.Nom_Afp : "",
          eps = eps != null ? eps.Nom_Eps : "",
          banco = banco != null ? banco.Banco : "",
          ccosto = ccosto.Nom_Ccosto,
          cargo = cargo.Nom_Cargo,
          fechaNomina = fechaNominaTemplate,
          fechaNominaActual = fechaNominaActual,
          logo = logoEmpresa,
          Salario = Salario
        };

        var query = from h in db.HISTORICO
                    join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                    where
                      h.Fec_Nomina == fechaNomina
                      && h.Cod_Empleado == empleado.Cod_Empleado
                      && h.Estado == "P"
                    select new
                    {
                      h.Cod_Concepto,
                      c.Nom_Concepto,
                      c.Devengo,
                      c.Por_Concepto,
                      c.BSPension,
                      c.BSBenSalario,
                      c.Tipo_Concepto,
                      c.OrdenDevengo,
                      h.Dias_Novedad,
                      h.Horas_Novedad,
                      h.Val_Novedad,
                      h.Porcentaje,
                    } into g
                    group g by new { g.Cod_Concepto, g.Nom_Concepto, g.Devengo, g.OrdenDevengo, g.Tipo_Concepto, g.BSPension, g.Por_Concepto, g.BSBenSalario, g.Porcentaje } into x
                    orderby x.Key.OrdenDevengo, x.Key.Nom_Concepto
                    select new CompropagoDataModel1
                    {
                      Cod_Concepto = x.Key.Cod_Concepto,
                      Nom_Concepto = x.Key.Nom_Concepto,
                      Devengo = x.Key.Devengo,
                      Tipo_Concepto = x.Key.Tipo_Concepto,
                      Dias_Novedad = x.Sum(i => i.Dias_Novedad),
                      Horas_novedad = x.Sum(i => i.Horas_Novedad),
                      Valor_Novedad = x.Sum(i => i.Val_Novedad),
                      BSPension = x.Key.BSPension,
                      Por_Concepto = x.Key.Por_Concepto,
                      BSBenSalario = x.Key.BSBenSalario,
                      OrdenDevengo = x.Key.OrdenDevengo,
                      Porcentaje = x.Key.Porcentaje
                    };

        var compropago = query.ToList();
        compropago = compropago.OrderBy(c => c.Cod_Concepto).ToList();

        string archivoPlantillaCertificado = "";
        string archivoPlantillaCertificado2 = "";
        string plantillaCertificado = "";
        string plantillaCertificado2 = "";

        if (data.quincena == "1") // && compropago.All(x => x.Cod_Concepto == 16))
        {
          archivoPlantillaCertificado = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/compropago-quincena.cshtml");
        }
        else
        {
          archivoPlantillaCertificado2 = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/certificados/compropago.cshtml");
        }
        if (archivoPlantillaCertificado != "")
        {
          plantillaCertificado = System.IO.File.ReadAllText(archivoPlantillaCertificado);

        }
        else if (archivoPlantillaCertificado2 != null)
        {

          plantillaCertificado2 = System.IO.File.ReadAllText(archivoPlantillaCertificado2);
        }

        var otrosPagosConceptos = new string[] { "0", "7", "8", "9" };
        model.otrosPagos = compropago.FindAll(c => (otrosPagosConceptos.Contains(c.Tipo_Concepto) || c.BSBenSalario == "S") && c.Devengo == "S");
        model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto) && c.BSBenSalario != "S");//Actual
        var VariableCuerpoCompropago = db.VARIABLES.Where(v => v.Uso == 33);
        if (VariableCuerpoCompropago.Count() > 0)
        {
          otrosPagosConceptos = new string[] { "0", "9" }; // Aridana
          model.otrosPagos = compropago.FindAll(c => c.Cod_Concepto == 999); // to get a empty list
          model.pagos = compropago.FindAll(c => !otrosPagosConceptos.Contains(c.Tipo_Concepto)); // Parametro si
        }


        var retefuente = model.pagos.FirstOrDefault(p => p.Cod_Concepto == 5);
        if (retefuente != null)
        {
          model.general.Por_Retefuente = retefuente.Porcentaje;
        }


        var autoliquidaciones = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Fec_Nomina.CompareTo(fechaNomina) <= 0 && h.Cod_Empleado == empleado.Cod_Empleado);

        if (autoliquidaciones.Count() > 0)
        {
          var ultimaFec_nomina = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Fec_Nomina.CompareTo(fechaNomina) <= 0 && h.Cod_Empleado == empleado.Cod_Empleado).Max(h => h.Fec_Nomina);

          var query2 = from h in db.HISTORICO_AUTOLIQUIDACIONES
                       join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                       where
                       h.Fec_Nomina == ultimaFec_nomina
                       && h.Cod_Empleado == empleado.Cod_Empleado
                       orderby h.Fec_Nomina, h.Cod_Concepto
                       select new CompropagoDataModel3
                       {
                         Cod_Concepto = h.Cod_Concepto,
                         Nom_Concepto = c.Nom_Concepto,
                         Val_IBC = h.Val_IBC,
                         Dias_Novedad = h.Dias_Novedad,
                         Val_Novedad = h.Val_Novedad,
                         Devengo = c.Devengo
                       };
          var aportes = query2.ToList();
          var aportesAMostrar = new string[] { "6", "7", "11", "46" };
          model.aportes = aportes.FindAll(a => aportesAMostrar.Contains(a.Cod_Concepto.ToString()));
          model.general.ultimaFecNomina = UtilHelper.getDate(ultimaFec_nomina);
        }
        else
        {
          model.aportes = new List<CompropagoDataModel3>();
          model.general.ultimaFecNomina = UtilHelper.getDate(fechaNomina);
        }

        model.aporteSalud = parametros.ApoSalud;
        model.aportePension = parametros.ApoFondoPen;
        model.aporteRiesgo = parametros.ApoPriesProf;

        double basePension = 0;
        foreach (var c in compropago)
        {
          if (c.OrdenDevengo == "1" || c.OrdenDevengo == "2" || c.OrdenDevengo == "3") 
          {
            if (c.BSPension == "S" || c.BSPension == "N")
            {
              basePension += c.Valor_Novedad;
            }
          }
        }

        if (basePension > (parametros.Salmin * 4))
        {
          if (empleado.Tipo_Salario == "2")
          {
            basePension *= 0.7;
          }
          double cantidadSalMins = Math.Truncate(basePension / parametros.Salmin);
          var rango = db.RANGOSSOLPENSION
                          .Where(r => cantidadSalMins >= r.Desde && cantidadSalMins <= r.Hasta)
                          .OrderByDescending(r => r.Desde);
          if (rango != null)
          {
            model.PorSolPen = rango.First().Porc_Sol_Pen;
          }
        }
        else
        {
          model.PorSolPen = 0;
        }

        // vraiable 39

        if (plantillaCertificado != "")
        {
          PDF += Engine.Razor.RunCompile(plantillaCertificado, "compropagoPlantilla", model.GetType(), model);
        }
        else
        {
          PDF += Engine.Razor.RunCompile(plantillaCertificado2, "comprobantedepago", model.GetType(), model);
        }
        string auditoria = "Cod Empleado: {0} generó un nuevo comprobante de pago de la fecha de nómina {1}";
        auditoria = String.Format(auditoria, model.empleado.Cod_Empleado, fechaNomina);
        AuditoriaHelper.Log(db, "COMPROPAGO", "N", auditoria);
      }
      //string sql2 = @"update CONCEPTOS set Devengo='' where Devengo='P' and Tipo_Concepto = 5";
      //db.Database.ExecuteSqlCommand(sql2);
      return PdfHelper.newconvert("Comprobante pago", PDF, data.empleados[0].Cedula.Trim());

    }

    // Certificados de ingresos y retenciones.
    public static string Retefuente2015(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, double valUvt)
    {
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se nifican los certificado
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = e.FecSubstitucionPatronal.Trim() != "" ? UtilHelper.getDate(e.FecSubstitucionPatronal) : FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (fechaIngreso > desde)
        {
          desde = fechaIngreso;
        }
      }
      else
      {
        if (fechaIngreso > desde)
        {
          desde = fechaIngreso;
        }
      }
      if (e.Estado == "R")
      {
        DateTime fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro < hasta)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // renglones
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
        new SqlParameter("@cedula", e.Cedula.ToString()),
        new SqlParameter("@fecha_desde", data.anoContable+"0101"),
        new SqlParameter("@fecha_hasta", data.anoContable+"1231")
      };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado = 'P' and  EMPLEADOS.Cedula = @cedula group by CertificadoIngresos, Cedula";
      }
      else
      {
        retefuenteParams = new[] {
        new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
        new SqlParameter("@fecha_desde", desde),
        new SqlParameter("@fecha_hasta", data.anoContable+"1231")
      };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, HISTORICO.Cod_Empleado ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado = 'P' and  HISTORICO.Cod_Empleado = @cod_empleado group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          double linea37 = 0.0;
          double linea38 = 0.0;
          double linea39 = 0.0;
          double linea40 = 0.0;
          double linea41 = 0.0;
          double linea43 = 0.0;
          double linea44 = 0.0;
          double linea45 = 0.0;
          double linea46 = 0.0;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea37 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea38 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea39 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea43 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea44 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea45 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea46 += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
            }
          }

          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
            new SqlParameter("@cedula", e.Cedula.ToString()),
            new SqlParameter("@fecha_desde", data.anoContable+"0101"),
            new SqlParameter("@fecha_hasta", data.anoContable+"1231")
          };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado = 'P' And CertificadoIngresos = '41' Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
            new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
            new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
            new SqlParameter("@fecha_hasta", data.anoContable+"1231")
          };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado = 'P' And CertificadoIngresos = '41' Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  if (mFechaNominaSal != nFechaNomina)
                  {
                    var p11 = new[] {
                            new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                            new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                        };
                    string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                    q11 = String.Format(q11, data.dbname);
                    using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                    {
                      var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                      if (salarioReader.HasRows)
                      {
                        salarioReader.Read();
                        mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                      }
                      else
                      {
                        var p12 = new[] {
                                    new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                                    new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                                };
                        string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                        q12 = String.Format(q12, data.dbname);
                        using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                        {
                          var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                          if (salarioReader2.HasRows)
                          {
                            salarioReader2.Read();
                            mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                          }
                        }
                      }

                      ///
                      mTopeBonosCanasta = Math.Round((valUvt * 310), 0);
                      mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                      if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                      {
                        mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                      }
                      mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                      mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                      mTopeBonosCanasta = mTopeBonosCanasta * 100;
                      if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                      }
                      else
                      {
                        mValorDeducibleBonos = Math.Round((valUvt * 41), 0);
                        mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                        if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                        {
                          mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                        }
                        mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                        mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                        mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                        if (mTotalBonos > mValorDeducibleBonos)
                        {
                          mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                        }
                      }
                      mFechaNominaSal = nFechaNomina;
                      mTotalBonos = 0;
                    }
                  }
                  mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((valUvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((valUvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              linea41 = mValOtrosIngresos;
            }
          }


          double linea42 = linea37 + linea38 + linea39 + linea40 + linea41;
          if (data.redondear)
          {
            linea37 = UtilHelper.round(linea37);
            linea38 = UtilHelper.round(linea38);
            if (linea39 > 0.0)
            {
              linea39 = UtilHelper.round(linea39);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            linea41 = UtilHelper.round(linea41);
            linea42 = UtilHelper.round(linea42);
            linea43 = UtilHelper.round(linea43);
            linea44 = UtilHelper.round(linea44);
            linea45 = UtilHelper.round(linea45);
            linea46 = UtilHelper.round(linea46);
          }

          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", "01");
          htmlCode = htmlCode.Replace("@30dd", "01");
          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", "12");
          htmlCode = htmlCode.Replace("@31dd", "31");
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));

          htmlCode = htmlCode.Replace("@linea37", linea37.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea38", linea38.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea39", linea39.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea40", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea41", linea41.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea42", linea42.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea43", linea43.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea44", linea44.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea45", linea45.ToString("N0"));
          htmlCode = htmlCode.Replace("@linea46", linea46.ToString("N0"));
        }
      }
      return htmlCode;
    }
    public static string Retefuente2017(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se nifican los certificado
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;

      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }

      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // renglones
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P', 'C') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P', 'C') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double afc = 0.0;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
            }
          }

          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where  HISTORICO.Cod_Concepto In(9,10,19,100)  and Cedula = @cedula and HISTORICO.Estado in ('P', 'C') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where  HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P', 'C')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var rre = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (rre.Read())
          {
            if (Convert.ToDouble(rre["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(rre["Val_Novedad"]);

              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
              //var negativeNumber = Double.Parse(prestaciones, format); // -14.3

            }
          }

          // Cesantias Consignadas.
          SqlParameter[] pa = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
          string cesantiasstrinfg = "Select Valor_Consignado From {0}.dbo.CESANTIAS Where Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro = 'C'";
          var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
          cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
          var rr = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa);
          while (rr.Read())
          {
            if (Convert.ToDouble(rr["Valor_Consignado"]) > 0)
            {
              cesantias += Convert.ToDouble(rr["Valor_Consignado"]);
            }
          }



          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40;
          if (data.redondear)
          {
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            comisiones = UtilHelper.round(comisiones);

            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            linea48 = UtilHelper.round(linea48);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            retefuente = UtilHelper.round(retefuente);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);
          }


          if (e.Estado == "R" && fechaRetiro.Year < desde.Year)
          {
            hasta = new DateTime(Convert.ToInt32(data.anoContable), 1, 1);
            hasta = new DateTime(Convert.ToInt32(data.anoContable), 1, 1);
          }

          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));
          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));

          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));
          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", retefuente.ToString("N0"));
        }
      }
      return htmlCode;
    }
    public static string Retefuente2018(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se unifican los certificados
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;

      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }

      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // renglones
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P','J') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P','J') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double afc = 0.0;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
            }
          }

          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where  HISTORICO.Cod_Concepto In(9,10,19,100)  and Cedula = @cedula and HISTORICO.Estado in ('P','C','J') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where  HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P','C','J')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var rre = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (rre.Read())
          {
            if (Convert.ToDouble(rre["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(rre["Val_Novedad"]);
              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
            }
          }

          // Cesantias Consignadas.

          if (data.ByCedula)
          {
            JulianaContext dbContext = new JulianaContext(data.dbname);

            List<EMPLEADOS> bycedula = dbContext.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                   .ToList();
            foreach (var contratos in bycedula)
            {
          SqlParameter[] pa = new[] {
              new SqlParameter("@cod_Empleado", contratos.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
              string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
              var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
              cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
              var rr = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa);
              while (rr.Read())
              {
                if (Convert.ToDouble(rr["Valor_Consignado"]) > 0)
                {
                  cesantias += Convert.ToDouble(rr["Valor_Consignado"]);
                }
              }
            }            
          }
          else
          {
            // Cesantias Consignadas.
            SqlParameter[] pa = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
            string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
          var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
          cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
          var rr = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa);
          while (rr.Read())
          {
            if (Convert.ToDouble(rr["Valor_Consignado"]) > 0)
            {
              cesantias += Convert.ToDouble(rr["Valor_Consignado"]);
            }
          }

          }


          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            ///---------------------------------------------------///
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40;

          if (data.redondear)
          {
            retefuente = UtilHelper.round(retefuente);
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            //linea48 = UtilHelper.round(linea48);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);

            //Calculo de fechas con redondeo
            JulianaContext fecdbo = new JulianaContext(data.dbname);
            List<EMPLEADOS> numeroContratos = fecdbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                        .ToList();
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(2019, 1, 1, 12, 0, 0);

            if (data.ByCedula)
            {

              if (UtilHelper.getDate(fechDesde) < date2)
              {
                fechDesde = data.anoContable + "0101";
              }
              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  ")) {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else
              {
                foreach (var contratos in numeroContratos)
                {
                  if (contratos.Estado == "R")
                  {
                    if (contratos.Fec_Ingreso.Contains("2019") && contratos.Fec_Retiro.Contains("2019"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }

                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    else if (contratos.Fec_Ingreso.Contains("2019") && contratos.Fec_Retiro.Contains("2020"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else if (contratos.Fec_Retiro.Contains("2020"))
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }
                  else if (contratos.Estado == "A")
                  {
                    if (contratos.Fec_Ingreso.Contains("2019"))
                    {

                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                      else
                      {
                        if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                        {
                          hasta = UtilHelper.getDate(fechRet);
                        }
                        else
                        {
                          hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                        }
                      }
                    }

                  }
                }
              }
              
            }else
            {

              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
              else if (e.Fec_Retiro.Contains("2020"))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
              {
                hasta = UtilHelper.getDate(e.Fec_Retiro);

              }
              else
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
            }
            linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40;

          }
          JulianaContext dbo = new JulianaContext(data.dbname);

          if (data.ByCedula)
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();

            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(2019, 1, 1, 12, 0, 0);
            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
            }
            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else
            {
              foreach (var contratos in numeroContratos)
              {
                if (contratos.Estado == "R")
                {
                  if (contratos.Fec_Ingreso.Contains("2019") && contratos.Fec_Retiro.Contains("2019"))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }

                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                  else if (contratos.Fec_Ingreso.Contains("2019") && contratos.Fec_Retiro.Contains("2020"))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else if (contratos.Fec_Retiro.Contains("2020"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                }
                else if (contratos.Estado == "A")

                {
                  if (contratos.Fec_Ingreso.Contains("2019"))
                  {

                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }

                }
              }
            } 
          }else
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();
            
            
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(2019, 1, 1, 12, 0, 0);

            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
         
            }

            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");

            }
            else if (e.Fec_Retiro.Contains("2020"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
            else
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
          }

          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));

          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));

          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));
          if (salarios.ToString("NO") != null) {
            htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          }
          else{
            htmlCode = htmlCode.Replace("@37", " ");
          }
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));
          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", retefuente.ToString("N0"));
        }
      }
      return htmlCode;
    }
    public static string Retefuente2020(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {


      #region PARAMETROS INICIALES
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se unifican los certificados
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;
      #endregion

      #region DEVENGOS Y OTROS RENGLONES
      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }
      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // Se llenan los valores de los rengones y las novedades
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P','J') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P','J') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          JulianaContext dbb = new JulianaContext(data.dbname);

          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double bonos = 0;

          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea37 = 0.0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double afc = 0.0;
          double TopeUvtSal = parametros.Val_Uvt * 310;
          double TopeUvtBono = parametros.Val_Uvt * 41;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "52":
                string consultaBonos = "";
                short empleado = 0;
                if (linea37 != 0)
                {
                  break;
                }
               
                empleado = Convert.ToInt16(e.Cod_Empleado);
                var salarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Salario;
                var bonosEmpleado = (from H in dbb.HISTORICO
                                     join C in dbb.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                                     where
                                      H.Cod_Empleado == empleado &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "0101") >= 0 &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "1231") <= 0 &&
                                       C.CertificadoIngresos == "52" &&
                                       (new string[] { "P", "J" }).Contains(H.Estado)
                                     group new { H, C } by new
                                     {
                                       H.Cod_Concepto,
                                       C.Devengo,
                                       C.BSBonoRetefuente,
                                       H.Fec_Nomina
                                     } into g
                                     orderby
                                       g.Key.Fec_Nomina,
                                       g.Key.BSBonoRetefuente descending
                                     select new
                                     {
                                       Val_Novedad = (double?)g.Sum(p => p.H.Val_Novedad),
                                       g.Key.Cod_Concepto,
                                       g.Key.Devengo,
                                       g.Key.BSBonoRetefuente,
                                       g.Key.Fec_Nomina
                                     }).ToList();

               
                  foreach (var bono in bonosEmpleado)
                  {
                  if (bono.BSBonoRetefuente == "N")
                  {
                    Val_Novedad += Convert.ToDouble(bono.Val_Novedad);
                    linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    //break;
                  }
                  else if (bono.BSBonoRetefuente == "S")
                  {
                    double valorDevengo = Convert.ToDouble(bono.Val_Novedad);
                    if (salarioEmpleado > TopeUvtSal)
                    {
                      Val_Novedad += valorDevengo;
                      linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    }
                    else if (salarioEmpleado <= TopeUvtSal)
                    {
                      if (valorDevengo >= TopeUvtBono)
                      {
                        //El valor del devengo - el TopeUVTsal
                        Val_Novedad += valorDevengo - TopeUvtBono;
                        linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        break;

                      }                     

                    }
                  }                                    
                 }                               
                break;
            }
          }
          #endregion

      #region PRESTACIONES DE EMPLEADOS.
          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where  HISTORICO.Cod_Concepto In(9,10,19,100)  and Cedula = @cedula and HISTORICO.Estado in ('P','C','J') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where  HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P','C','J')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var rre = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (rre.Read())
          {
            if (Convert.ToDouble(rre["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(rre["Val_Novedad"]);
              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
            }
          }

         
          #endregion

          #region CESANTIAS CONSIGNADAS Y OTROS PAGOS
          // Cesantias Consignadas.

          if (data.ByCedula)
          {
            JulianaContext dbContext = new JulianaContext(data.dbname);

            List<EMPLEADOS> bycedula = dbContext.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                   .ToList();
            foreach (var contratos in bycedula)
            {
              SqlParameter[] pa = new[] {
              new SqlParameter("@cod_Empleado", contratos.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
              string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
              var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
              cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
              var rr = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa);
              while (rr.Read())
              {
                if (Convert.ToDouble(rr["Valor_Consignado"]) > 0)
                {
                  cesantias += Convert.ToDouble(rr["Valor_Consignado"]);
                }
              }
            }
          }
          else
          {
            // Cesantias Consignadas.
            SqlParameter[] pa = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
            string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
            var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
            cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
            var rr = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa);
            while (rr.Read())
            {
              if (Convert.ToDouble(rr["Valor_Consignado"]) > 0)
              {
                cesantias += Convert.ToDouble(rr["Valor_Consignado"]);
              }
            }

          }


          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            ///---------------------------------------------------///
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40 + linea37;

          if (data.redondear)
          {
            retefuente = UtilHelper.round(retefuente);
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            comisiones = UtilHelper.round(comisiones);
            linea37 = UtilHelper.round(linea37);


            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);
            afc = UtilHelper.round(afc);
            comisiones = UtilHelper.round(comisiones);

            #endregion

            #region CALCULO DE FECHAS Y GENERACION DE CERT GENERAL
            //Calculo de fechas con redondeo
            JulianaContext fecdbo = new JulianaContext(data.dbname);
            List<EMPLEADOS> numeroContratos = fecdbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                        .ToList();
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(2020, 1, 1, 12, 0, 0);

            if (data.ByCedula)
            {

              if (UtilHelper.getDate(fechDesde) < date2)
              {
                fechDesde = data.anoContable + "0101";
          }
              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else
              {
                foreach (var contratos in numeroContratos)
                {
                  if (contratos.Estado == "R")
                  { 
                    if (contratos.Fec_Ingreso.Contains("2020") && contratos.Fec_Retiro.Contains("2020"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }

                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    else if (contratos.Fec_Ingreso.Contains("2020") && contratos.Fec_Retiro.Contains("2021"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else if (contratos.Fec_Retiro.Contains("2021"))
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }
                  else if (contratos.Estado == "A")
                  {
                    if (contratos.Fec_Ingreso.Contains("2020"))
                    {

                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
          {
                        desde = UtilHelper.getDate(fechDesde);
          }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                      else
                      {
                        if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                        {
                          hasta = UtilHelper.getDate(fechRet);
                        }
                        else
                        {
                          hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                        }
                      }
                    }

                  }
                }
              }

            }
            else
            {

              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
              else if (e.Fec_Retiro.Contains("2020"))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (e.Fec_Retiro.Contains("2021"))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (e.Fec_Retiro.Contains("2022"))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
              {
                hasta = UtilHelper.getDate(e.Fec_Retiro);

              }
              else
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
            }
            linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40 + linea37;

          }
          JulianaContext dbo = new JulianaContext(data.dbname);
          #endregion

      #region GENERACION DE CERTIFICADOS POR CEDULA
          if (data.ByCedula)
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();

            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(2020, 1, 1, 12, 0, 0);
            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
            }
            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else
            {
              foreach (var contratos in numeroContratos)
              {
                if (contratos.Estado == "R")
                {
                  if (contratos.Fec_Ingreso.Contains("2020") && contratos.Fec_Retiro.Contains("2020"))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }

                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                  else if (contratos.Fec_Ingreso.Contains("2020") && contratos.Fec_Retiro.Contains("2021"))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else if (contratos.Fec_Retiro.Contains("2020"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                }
                else if (contratos.Estado == "A")

                {
                  if (contratos.Fec_Ingreso.Contains("2020"))
                  {

                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }

                }
              }
            }
          }
          else
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();


            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(2020, 1, 1, 12, 0, 0);

            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";

            }

            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");

            }
            else if (e.Fec_Retiro.Contains("2021") || e.Fec_Retiro.Contains("2022"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (e.Fec_Retiro.Contains("2021"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (e.Fec_Retiro.Contains("2022"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
            else
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
          }
          #endregion

          #region FAMILIARES DEL EMPLEADO          
          var familiar = dbo.FAMILIARES.Where(x => x.Cod_Empleado == e.Cod_Empleado).ToList();
          
          

        
          #endregion

          #region LLENAR LOS CAMPOS FALTANTES DEL DOCUMENTO
          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));

          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));

          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));
          if (salarios.ToString("NO") != null)
          {
            htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@37", "0");
          }
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));

          if (linea37 != 0.0)
          {
            htmlCode = htmlCode.Replace("@54", linea37.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@54", "0");

          }

          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", retefuente.ToString("N0"));
          if (familiar != null)
          {
            htmlCode = htmlCode.Replace("@75", " ");
            htmlCode = htmlCode.Replace("@76", " ");
            htmlCode = htmlCode.Replace("@77", " ");
            htmlCode = htmlCode.Replace("@78", " ");

          }
          foreach (var item in familiar)
          {

            var TipoDocumento = "";
            switch (item.Tipo_Documento)
            {
              case "C":
                TipoDocumento = "13";
                break;
              case "N":
                TipoDocumento = "31";
                break;
              case "T":
                TipoDocumento = "12";
                break;
              case "P":
                TipoDocumento = "41";
                break;
              case "E":
                TipoDocumento = "22";
                break;
            }
            htmlCode = htmlCode.Replace("@75", TipoDocumento);
            if (item.Cedula == "")
            {
              htmlCode = htmlCode.Replace("@76", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@76", item.Cedula);

            }
            if (item.Nombre == "")
            {
              htmlCode = htmlCode.Replace("@77", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@77", item.Nombre);

            }
            if (item.Parentesco == "")
            {
              htmlCode = htmlCode.Replace("@78", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@78", item.Parentesco);

            }        
          }
         

          #endregion
        }
      }
      return htmlCode;
    }
    public static string Retefuente2021(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {
          #region PARAMETROS INICIALES
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se unifican los certificados
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;
      #endregion

          #region DEVENGOS Y OTROS RENGLONES
      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }
      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // Se llenan los valores de los rengones y las novedades
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P','J') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P','J') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          JulianaContext dbb = new JulianaContext(data.dbname);

          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double bonos = 0;

          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea37 = 0.0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double cesantiasConsignadas = 0.0;
          double Variable80 = 0.0;
          double afc = 0.0;
          double TopeUvtSal = parametros.Val_Uvt * 310;
          double TopeUvtBono = parametros.Val_Uvt * 41;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "52":
                string consultaBonos = "";
                short empleado = 0;
                if (linea37 != 0)
                {
                  break;
                }
                empleado = Convert.ToInt16(e.Cod_Empleado);
                var salarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Salario;
                var TiposalarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Tipo_Salario;
                var bonosEmpleado = (from H in dbb.HISTORICO
                                     join C in dbb.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                                     where
                                      H.Cod_Empleado == empleado &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "0101") >= 0 &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "1231") <= 0 &&
                                       C.CertificadoIngresos == "52" &&
                                       (new string[] { "P", "J" }).Contains(H.Estado)
                                     group new { H, C } by new
                                     {
                                       H.Cod_Concepto,
                                       C.Devengo,
                                       C.BSBonoRetefuente,
                                       H.Fec_Nomina
                                     } into g
                                     orderby
                                       g.Key.Fec_Nomina,
                                       g.Key.BSBonoRetefuente descending
                                     select new
                                     {
                                       Val_Novedad = (double?)g.Sum(p => p.H.Val_Novedad),
                                       g.Key.Cod_Concepto,
                                       g.Key.Devengo,
                                       g.Key.BSBonoRetefuente,
                                       g.Key.Fec_Nomina
                                     }).ToList();

                foreach (var bono in bonosEmpleado)
                {
                  if (bono.BSBonoRetefuente == "N")
                  {
                    Val_Novedad += Convert.ToDouble(bono.Val_Novedad);
                    linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    //break;
                  }
                  else if (bono.BSBonoRetefuente == "S")
                  {
                    double valorDevengo = Convert.ToDouble(bono.Val_Novedad);
                    if (TiposalarioEmpleado == "2" )
                    {
                      salarioEmpleado = Math.Round((salarioEmpleado / 1.3));
                    }

                    if (salarioEmpleado > TopeUvtSal)
                    {
                      Val_Novedad += valorDevengo;
                      linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    }
                    else if (salarioEmpleado <= TopeUvtSal)
                    {
                      if (valorDevengo >= TopeUvtBono)
                      {
                        //El valor del devengo - el TopeUVTsal
                        Val_Novedad += valorDevengo - TopeUvtBono;
                        linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        //break;
                      }
                    }
                  }


                  //else if bono.BSBonoRetefuente == "S")
                  //  {
                  //    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);


                  //    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                  //    {
                  //      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                  //    }
                  //    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                  //    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                  //    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                  //    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                  //    {
                  //      mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                  //    }
                  //    else
                  //    {
                  //      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  //      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  //      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  //      {
                  //        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  //      }
                  //      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  //      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  //      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  //      if (mTotalBonos > mValorDeducibleBonos)
                  //      {
                  //        mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  //      }
                  //    }
                  //  }

                  //    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                  //  }
                }
                break;
            }
          }
          #endregion

          #region PRESTACIONES DE EMPLEADOS.
          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where HISTORICO.Cod_Concepto In(9,10,19,100) and Cedula = @cedula and HISTORICO.Estado in ('P','C','J') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P','C','J')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var presta = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (presta.Read())
          {
            if (Convert.ToDouble(presta["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(presta["Val_Novedad"]);
              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
            }
          }


          #endregion

          #region CESANTIAS CONSIGNADAS Y OTROS PAGOS
          
          // Cesantias Consignadas.

          if (data.ByCedula)
          {
            JulianaContext dbContext = new JulianaContext(data.dbname);

            List<EMPLEADOS> bycedula = dbContext.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                   .ToList();
            foreach (var contratos in bycedula)
            {
              SqlParameter[] pa4 = new[] {
              new SqlParameter("@cod_Empleado", contratos.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
              string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
              var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
              cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
              var rr4 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa4);
              while (rr4.Read())
              {
                if (Convert.ToDouble(rr4["Valor_Consignado"]) > 0)
                {
                  cesantiasConsignadas += Convert.ToDouble(rr4["Valor_Consignado"]);
                }
              }
            }
          }
          else
          {
            // Cesantias Consignadas.
            SqlParameter[] pa3 = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
            string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
            var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
            cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
            var rr3 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa3);
            while (rr3.Read())
            {
              if (Convert.ToDouble(rr3["Valor_Consignado"]) > 0)
              {
                cesantiasConsignadas += Convert.ToDouble(rr3["Valor_Consignado"]);
              }
            }
          }

          ////Linea 56 Pasivos laborales reales consolidados en cabeza del trabajador
          //SqlParameter[] pa = new[] {
          //new SqlParameter("@mcodemp", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal", Convert.ToInt32(data.anoContable).ToString() + "1230")
          //};
          //string mPasivoPrestaciones = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp And H.Estado = 'I' And Fec_Nomina = @mFechaFinal And Cod_Concepto Not In(9,52,100,98)";
          //var connVar80 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones = String.Format(mPasivoPrestaciones, data.dbname);
          //var rr = SqlHelper.ExecuteReader(connVar80, CommandType.Text, mPasivoPrestaciones, pa);
          //while (rr.Read())
          //{
          //  //if (Convert.ToDouble(rr["Val_Novedad"]) > 0)
          //  //{
          //    Variable80 += (Convert.ToDouble(rr["Val_Novedad"]) * -1);
          //  //}
          //}

          //SqlParameter[] pa2 = new[] {
          //new SqlParameter("@mcodemp2", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal2", (Convert.ToInt32(data.anoContable) + 1).ToString() + "0115")
          //};
          //string mPasivoPrestaciones2 = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp2 And H.Estado = 'S' And Fec_Nomina = @mFechaFinal2 And Cod_Concepto Not In(52)";
          //var connVar80_2 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones2 = String.Format(mPasivoPrestaciones2, data.dbname);
          //var rr2 = SqlHelper.ExecuteReader(connVar80_2, CommandType.Text, mPasivoPrestaciones2, pa2);
          //while (rr2.Read())
          //{
          //  if (Convert.ToDouble(rr2["Val_Novedad"]) > 0)
          //  {
          //    Variable80 += Convert.ToDouble(rr2["Val_Novedad"]);
          //  }
          //}

          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            ///---------------------------------------------------///
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }
          

          //double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40 + mValOtrosIngresos;
          double linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;


          if (data.redondear)
          {
            retefuente = UtilHelper.round(retefuente);
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            comisiones = UtilHelper.round(comisiones);
            linea37 = UtilHelper.round(linea37);
            cesantiasConsignadas = UtilHelper.round(cesantiasConsignadas);

            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);
            afc = UtilHelper.round(afc);

            #endregion

          #region CALCULO DE FECHAS Y GENERACION DE CERT GENERAL
            //Calculo de fechas con redondeo
            JulianaContext fecdbo = new JulianaContext(data.dbname);
            List<EMPLEADOS> numeroContratos = fecdbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                        .ToList();
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (data.ByCedula)
            {

              if (UtilHelper.getDate(fechDesde) < date2)
              {
                fechDesde = data.anoContable + "0101";
              }

              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else
              {
                foreach (var contratos in numeroContratos)
                {
                  if (contratos.Estado == "R")
                  {
                    if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }

                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable+"0101"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    //DG-CE
                    if (hasta <= UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "0101");
                    }
                    else if (contratos.Estado == "A")
                    {
                      if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                      {

                        if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                        {
                          desde = UtilHelper.getDate(fechDesde);
                        }
                        else
                        {
                          desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                        }
                        if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                        {
                          hasta = UtilHelper.getDate(data.anoContable + "1231");
                        }
                        else
                        {
                          if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                          {
                            hasta = UtilHelper.getDate(fechRet);
                          }
                          else
                          {
                            hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            else
            {
              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
              else if (e.Fec_Retiro.StartsWith(data.anoContable))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
              {
                hasta = UtilHelper.getDate(e.Fec_Retiro);

              }
              else
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
            }
            linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;
          }
          JulianaContext dbo = new JulianaContext(data.dbname);
          #endregion

          #region GENERACION DE CERTIFICADOS POR CEDULA
          if (data.ByCedula)
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();

            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);
            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
            }
            if (UtilHelper.getDate(fechDesde) <= date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else
            {
              foreach (var contratos in numeroContratos)
              {
                if (contratos.Estado == "R")
                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }

                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                  else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && (contratos.Fec_Retiro.StartsWith("2022")))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet) && numeroContratos.Count > 1)
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                      //hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else if (UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "1231"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                }
                else if (contratos.Estado == "A")

                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                  {

                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }

                }
              }
            }
          }
          else
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();


            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";

            }

            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");

            }
            else if (e.Fec_Retiro.StartsWith(data.anoContable) || e.Fec_Retiro.StartsWith("2022"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
            else
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
          }

          #endregion


            #region LLENAR LOS CAMPOS FALTANTES DEL DOCUMENTO
            htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));

          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));

          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));
          if (salarios.ToString("NO") != null)
          {
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@37", "0");
          }
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));

          if (mValOtrosIngresos != 0.0)
          {
            htmlCode = htmlCode.Replace("@54", linea37.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@54", "0");

          }

          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@79", Math.Abs(cesantiasConsignadas).ToString("N0")); 
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", Math.Abs(retefuente).ToString("N0"));
          htmlCode = htmlCode.Replace("@80", Math.Abs(Variable80).ToString("N0"));
          #region FAMILIARES DEL EMPLEADO          
          var familiar = dbo.FAMILIARES.Where(x => x.Cod_Empleado == e.Cod_Empleado).ToList();
          foreach (var item in familiar)
          {
            var TipoDocumento = "";
            switch (item.Tipo_Documento)
            {
              case "C":
                TipoDocumento = "13";
                break;
              case "N":
                TipoDocumento = "31";
                break;
              case "T":
                TipoDocumento = "12";
                break;
              case "P":
                TipoDocumento = "41";
                break;
              case "E":
                TipoDocumento = "22";
                break;
              case "R":
                TipoDocumento = "11";
                break;
            }
            htmlCode = htmlCode.Replace("@75", TipoDocumento);
            if (item.Cedula == "")
            {
              htmlCode = htmlCode.Replace("@76", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@76", item.Cedula);

            }
            if (item.Nombre == "")
            {
              htmlCode = htmlCode.Replace("@77", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@77", item.Nombre);

            }
            if (item.Parentesco == "")
            {
              htmlCode = htmlCode.Replace("@78", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@78", item.Parentesco);

            }
          }
          if (familiar.Count == 0)
          {
            htmlCode = htmlCode.Replace("@75", " ");
            htmlCode = htmlCode.Replace("@76", " ");
            htmlCode = htmlCode.Replace("@77", " ");
            htmlCode = htmlCode.Replace("@78", " ");
          }
          #endregion

          #endregion
        }
      }
      return htmlCode;
    }
    public static string Retefuente2022(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {
      #region PARAMETROS INICIALES
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se unifican los certificados
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;
      #endregion

      #region DEVENGOS Y OTROS RENGLONES
      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }
      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // Se llenan los valores de los rengones y las novedades
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P','J') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P','J') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          JulianaContext dbb = new JulianaContext(data.dbname);

          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double bonos = 0;

          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea37 = 0.0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double cesantiasConsignadas = 0.0;
          double Variable80 = 0.0;
          double afc = 0.0;
          double TopeUvtSal = parametros.Val_Uvt * 310;
          double TopeUvtBono = parametros.Val_Uvt * 41;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "52":
                string consultaBonos = "";
                short empleado = 0;
                if (linea37 != 0)
                {
                  break;
                }
                empleado = Convert.ToInt16(e.Cod_Empleado);
                var salarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Salario;
                var TiposalarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Tipo_Salario;
                var bonosEmpleado = (from H in dbb.HISTORICO
                                     join C in dbb.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                                     where
                                      H.Cod_Empleado == empleado &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "0101") >= 0 &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "1231") <= 0 &&
                                       C.CertificadoIngresos == "52" &&
                                       (new string[] { "P", "J" }).Contains(H.Estado)
                                     group new { H, C } by new
                                     {
                                       H.Cod_Concepto,
                                       C.Devengo,
                                       C.BSBonoRetefuente,
                                       H.Fec_Nomina
                                     } into g
                                     orderby
                                       g.Key.Fec_Nomina,
                                       g.Key.BSBonoRetefuente descending
                                     select new
                                     {
                                       Val_Novedad = (double?)g.Sum(p => p.H.Val_Novedad),
                                       g.Key.Cod_Concepto,
                                       g.Key.Devengo,
                                       g.Key.BSBonoRetefuente,
                                       g.Key.Fec_Nomina
                                     }).ToList();

                foreach (var bono in bonosEmpleado)
                {
                  if (bono.BSBonoRetefuente == "N")
                  {
                    Val_Novedad += Convert.ToDouble(bono.Val_Novedad);
                    linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    //break;
                  }
                  else if (bono.BSBonoRetefuente == "S")
                  {
                    double valorDevengo = Convert.ToDouble(bono.Val_Novedad);
                    if (TiposalarioEmpleado == "2")
                    {
                      salarioEmpleado = Math.Round((salarioEmpleado / 1.3));
                    }

                    if (salarioEmpleado > TopeUvtSal)
                    {
                      Val_Novedad += valorDevengo;
                      linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    }
                    else if (salarioEmpleado <= TopeUvtSal)
                    {
                      if (valorDevengo >= TopeUvtBono)
                      {
                        //El valor del devengo - el TopeUVTsal
                        Val_Novedad += valorDevengo - TopeUvtBono;
                        linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        //break;
                      }
                    }
                  }


                  //else if bono.BSBonoRetefuente == "S")
                  //  {
                  //    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);


                  //    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                  //    {
                  //      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                  //    }
                  //    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                  //    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                  //    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                  //    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                  //    {
                  //      mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                  //    }
                  //    else
                  //    {
                  //      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  //      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  //      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  //      {
                  //        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  //      }
                  //      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  //      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  //      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  //      if (mTotalBonos > mValorDeducibleBonos)
                  //      {
                  //        mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  //      }
                  //    }
                  //  }

                  //    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                  //  }
                }
                break;
            }
          }
          #endregion

          #region PRESTACIONES DE EMPLEADOS.
          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where HISTORICO.Cod_Concepto In(9,10,19,100) and Cedula = @cedula and HISTORICO.Estado in ('P','C','J') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P','C','J')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var presta = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (presta.Read())
          {
            if (Convert.ToDouble(presta["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(presta["Val_Novedad"]);
              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
            }
          }


          #endregion

          #region CESANTIAS CONSIGNADAS Y OTROS PAGOS

          // Cesantias Consignadas.

          if (data.ByCedula)
          {
            JulianaContext dbContext = new JulianaContext(data.dbname);

            List<EMPLEADOS> bycedula = dbContext.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                   .ToList();
            foreach (var contratos in bycedula)
            {
              SqlParameter[] pa4 = new[] {
              new SqlParameter("@cod_Empleado", contratos.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
              string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
              var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
              cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
              var rr4 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa4);
              while (rr4.Read())
              {
                if (Convert.ToDouble(rr4["Valor_Consignado"]) > 0)
                {
                  cesantiasConsignadas += Convert.ToDouble(rr4["Valor_Consignado"]);
                }
              }
            }
          }
          else
          {
            // Cesantias Consignadas.
            SqlParameter[] pa3 = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
            string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
            var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
            cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
            var rr3 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa3);
            while (rr3.Read())
            {
              if (Convert.ToDouble(rr3["Valor_Consignado"]) > 0)
              {
                cesantiasConsignadas += Convert.ToDouble(rr3["Valor_Consignado"]);
              }
            }
          }

          ////Linea 56 Pasivos laborales reales consolidados en cabeza del trabajador
          //SqlParameter[] pa = new[] {
          //new SqlParameter("@mcodemp", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal", Convert.ToInt32(data.anoContable).ToString() + "1230")
          //};
          //string mPasivoPrestaciones = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp And H.Estado = 'I' And Fec_Nomina = @mFechaFinal And Cod_Concepto Not In(9,52,100,98)";
          //var connVar80 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones = String.Format(mPasivoPrestaciones, data.dbname);
          //var rr = SqlHelper.ExecuteReader(connVar80, CommandType.Text, mPasivoPrestaciones, pa);
          //while (rr.Read())
          //{
          //  //if (Convert.ToDouble(rr["Val_Novedad"]) > 0)
          //  //{
          //    Variable80 += (Convert.ToDouble(rr["Val_Novedad"]) * -1);
          //  //}
          //}

          //SqlParameter[] pa2 = new[] {
          //new SqlParameter("@mcodemp2", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal2", (Convert.ToInt32(data.anoContable) + 1).ToString() + "0115")
          //};
          //string mPasivoPrestaciones2 = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp2 And H.Estado = 'S' And Fec_Nomina = @mFechaFinal2 And Cod_Concepto Not In(52)";
          //var connVar80_2 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones2 = String.Format(mPasivoPrestaciones2, data.dbname);
          //var rr2 = SqlHelper.ExecuteReader(connVar80_2, CommandType.Text, mPasivoPrestaciones2, pa2);
          //while (rr2.Read())
          //{
          //  if (Convert.ToDouble(rr2["Val_Novedad"]) > 0)
          //  {
          //    Variable80 += Convert.ToDouble(rr2["Val_Novedad"]);
          //  }
          //}

          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            ///---------------------------------------------------///
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }


          //double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40 + mValOtrosIngresos;
          double linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;


          if (data.redondear)
          {
            retefuente = UtilHelper.round(retefuente);
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            comisiones = UtilHelper.round(comisiones);
            linea37 = UtilHelper.round(linea37);
            cesantiasConsignadas = UtilHelper.round(cesantiasConsignadas);

            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);
            afc = UtilHelper.round(afc);

            #endregion

            #region CALCULO DE FECHAS Y GENERACION DE CERT GENERAL
            //Calculo de fechas con redondeo
            JulianaContext fecdbo = new JulianaContext(data.dbname);
            List<EMPLEADOS> numeroContratos = fecdbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                        .ToList();
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (data.ByCedula)
            {

              if (UtilHelper.getDate(fechDesde) < date2)
              {
                fechDesde = data.anoContable + "0101";
              }

              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else
              {
                foreach (var contratos in numeroContratos)
                {
                  if (contratos.Estado == "R")
                  {
                    if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }

                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (UtilHelper.getDate(contratos.Fec_Retiro) < UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                    }
                    //DG-CE
                    if (hasta <= UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "0101");
                    }
                    else if (contratos.Estado == "A")
                    {
                      if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                      {

                        if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                        {
                          desde = UtilHelper.getDate(fechDesde);
                        }
                        else
                        {
                          desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                        }
                        if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                        {
                          hasta = UtilHelper.getDate(data.anoContable + "1231");
                        }
                        else
                        {
                          if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                          {
                            hasta = UtilHelper.getDate(fechRet);
                          }
                          else
                          {
                            hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            else
            {
              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
              else if (e.Fec_Retiro.StartsWith(data.anoContable))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
              {
                hasta = UtilHelper.getDate(e.Fec_Retiro);

              }
              else
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
            }
            linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;
          }
          JulianaContext dbo = new JulianaContext(data.dbname);
          #endregion

          #region GENERACION DE CERTIFICADOS POR CEDULA
          if (data.ByCedula)
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();

            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);
            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
            }
            if (UtilHelper.getDate(fechDesde) <= date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else
            {
              foreach (var contratos in numeroContratos)
              {
                if (contratos.Estado == "R")
                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }

                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                  else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && (contratos.Fec_Retiro.StartsWith("2022")))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet) && numeroContratos.Count > 1)
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                      //hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else if (UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "1231"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                }
                else if (contratos.Estado == "A")

                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                  {

                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }

                }
              }
            }
          }
          else
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();


            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";

            }

            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");

            }
            else if (e.Fec_Retiro.StartsWith(data.anoContable) || e.Fec_Retiro.StartsWith("2022"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
            else
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
          }

          #endregion


          #region LLENAR LOS CAMPOS FALTANTES DEL DOCUMENTO
          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));

          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));

          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));
          if (salarios.ToString("NO") != null)
          {
            htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@37", "0");
          }
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));

          if (mValOtrosIngresos != 0.0)
          {
            htmlCode = htmlCode.Replace("@54", linea37.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@54", "0");

          }

          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@79", Math.Abs(cesantiasConsignadas).ToString("N0"));
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", Math.Abs(retefuente).ToString("N0"));
          htmlCode = htmlCode.Replace("@80", Math.Abs(Variable80).ToString("N0"));
          #region FAMILIARES DEL EMPLEADO          
          var familiar = dbo.FAMILIARES.Where(x => x.Cod_Empleado == e.Cod_Empleado).ToList();
          foreach (var item in familiar)
          {
            var TipoDocumento = "";
            switch (item.Tipo_Documento)
            {
              case "C":
                TipoDocumento = "13";
                break;
              case "N":
                TipoDocumento = "31";
                break;
              case "T":
                TipoDocumento = "12";
                break;
              case "P":
                TipoDocumento = "41";
                break;
              case "E":
                TipoDocumento = "22";
                break;
              case "R":
                TipoDocumento = "11";
                break;
            }
            htmlCode = htmlCode.Replace("@75", TipoDocumento);
            if (item.Cedula == "")
            {
              htmlCode = htmlCode.Replace("@76", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@76", item.Cedula);

            }
            if (item.Nombre == "")
            {
              htmlCode = htmlCode.Replace("@77", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@77", item.Nombre);

            }
            if (item.Parentesco == "")
            {
              htmlCode = htmlCode.Replace("@78", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@78", item.Parentesco);

            }
          }
          if (familiar.Count == 0)
          {
            htmlCode = htmlCode.Replace("@75", " ");
            htmlCode = htmlCode.Replace("@76", " ");
            htmlCode = htmlCode.Replace("@77", " ");
            htmlCode = htmlCode.Replace("@78", " ");
          }
          #endregion

          #endregion
        }
      }
      return htmlCode;
    }
    public static string Retefuente2023(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {
      #region PARAMETROS INICIALES
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se unifican los certificados
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;
      #endregion

      #region DEVENGOS Y OTROS RENGLONES
      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }
      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // Se llenan los valores de los rengones y las novedades
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P','J') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P','J') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          JulianaContext dbb = new JulianaContext(data.dbname);

          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double bonos = 0;

          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea37 = 0.0;
          double linea37_2 = 0.0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double cesantiasConsignadas = 0.0;
          double Variable80 = 0.0;
          double afc = 0.0;
          double TopeUvtSal = parametros.Val_Uvt * 310;
          double TopeUvtBono = parametros.Val_Uvt * 41;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "52":
                string consultaBonos = "";
                short empleado = 0;
                if (linea37 != 0)
                {
                  break;
                }
                empleado = Convert.ToInt16(e.Cod_Empleado);
                var salarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Salario;
                var TiposalarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Tipo_Salario;
                var bonosEmpleado = (from H in dbb.HISTORICO
                                     join C in dbb.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                                     where
                                      H.Cod_Empleado == empleado &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "0101") >= 0 &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "1231") <= 0 &&
                                       C.CertificadoIngresos == "52" &&
                                       (new string[] { "P", "J" }).Contains(H.Estado)
                                     group new { H, C } by new
                                     {
                                       H.Cod_Concepto,
                                       C.Devengo,
                                       C.BSBonoRetefuente,
                                       H.Fec_Nomina
                                     } into g
                                     orderby
                                       g.Key.Fec_Nomina,
                                       g.Key.BSBonoRetefuente descending
                                     select new
                                     {
                                       Val_Novedad = (double?)g.Sum(p => p.H.Val_Novedad),
                                       g.Key.Cod_Concepto,
                                       g.Key.Devengo,
                                       g.Key.BSBonoRetefuente,
                                       g.Key.Fec_Nomina
                                     }).ToList();

                if (TiposalarioEmpleado == "2")
                {
                  salarioEmpleado = Math.Round((salarioEmpleado / 1.3));
                }

                foreach (var bono in bonosEmpleado)
                {
                  if (bono.BSBonoRetefuente == "N")
                  {
                    Val_Novedad += Convert.ToDouble(bono.Val_Novedad);
                    linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    //break;
                  }
                  else if (bono.BSBonoRetefuente == "S")
                  {
                    double valorDevengo = Convert.ToDouble(bono.Val_Novedad);
                    if (salarioEmpleado > TopeUvtSal)
                    {
                      Val_Novedad += valorDevengo;
                      linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    }
                    else if (salarioEmpleado <= TopeUvtSal)
                    {
                      if (valorDevengo >= TopeUvtBono)
                      {
                        //El valor del devengo - el TopeUVTsal
                        Val_Novedad += valorDevengo - TopeUvtBono;
                        linea37_2 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        //break;
                      }
                    }
                  }


                  //else if bono.BSBonoRetefuente == "S")
                  //  {
                  //    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);


                  //    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                  //    {
                  //      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                  //    }
                  //    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                  //    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                  //    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                  //    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                  //    {
                  //      mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                  //    }
                  //    else
                  //    {
                  //      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  //      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  //      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  //      {
                  //        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  //      }
                  //      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  //      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  //      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  //      if (mTotalBonos > mValorDeducibleBonos)
                  //      {
                  //        mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  //      }
                  //    }
                  //  }

                  //    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                  //  }
                }
                break;
            }
          }
          #endregion

          #region PRESTACIONES DE EMPLEADOS.
          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where HISTORICO.Cod_Concepto In(9,10,19,100) and Cedula = @cedula and HISTORICO.Estado in ('P','C','J') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P','C','J')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var presta = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (presta.Read())
          {
            if (Convert.ToDouble(presta["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(presta["Val_Novedad"]);
              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
            }
          }


          #endregion

          #region CESANTIAS CONSIGNADAS Y OTROS PAGOS

          // Cesantias Consignadas.

          if (data.ByCedula)
          {
            JulianaContext dbContext = new JulianaContext(data.dbname);

            List<EMPLEADOS> bycedula = dbContext.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                   .ToList();
            foreach (var contratos in bycedula)
            {
              SqlParameter[] pa4 = new[] {
              new SqlParameter("@cod_Empleado", contratos.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
              string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
              var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
              cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
              var rr4 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa4);
              while (rr4.Read())
              {
                if (Convert.ToDouble(rr4["Valor_Consignado"]) > 0)
                {
                  cesantiasConsignadas += Convert.ToDouble(rr4["Valor_Consignado"]);
                }
              }
            }
          }
          else
          {
            // Cesantias Consignadas.
            SqlParameter[] pa3 = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
            string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
            var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
            cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
            var rr3 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa3);
            while (rr3.Read())
            {
              if (Convert.ToDouble(rr3["Valor_Consignado"]) > 0)
              {
                cesantiasConsignadas += Convert.ToDouble(rr3["Valor_Consignado"]);
              }
            }
          }
          //INGRESOS PROMEDIOS ULTIMOS 6 MESES LINEA 59
          double LINEA59 = 0.0;
          SqlParameter[] PaProm = new[] {
            new SqlParameter("@cesantias_pagadas", Convert.ToInt32(cesantias)),
            new SqlParameter("@cesantias_consignadas", Convert.ToInt32(cesantiasConsignadas)),
            new SqlParameter("@cod_Empleado", e.Cod_Empleado),
            new SqlParameter("@estado", e.Estado),
            new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
            new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
          };
          var connParam = new SqlConnection(SqlHelper.GetConnectionString());
          var PromReader = SqlHelper.ExecuteReader(connParam, CommandType.StoredProcedure, "SP_CIR_2023L59", PaProm);
          PromReader.Read();
          LINEA59 = Convert.ToDouble(PromReader[0]);        
          //while (PromReader.Read()) { rta = Convert.ToString(PromReader[0]); }



          ////Linea 56 Pasivos laborales reales consolidados en cabeza del trabajador
          //SqlParameter[] pa = new[] {
          //new SqlParameter("@mcodemp", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal", Convert.ToInt32(data.anoContable).ToString() + "1230")
          //};
          //string mPasivoPrestaciones = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp And H.Estado = 'I' And Fec_Nomina = @mFechaFinal And Cod_Concepto Not In(9,52,100,98)";
          //var connVar80 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones = String.Format(mPasivoPrestaciones, data.dbname);
          //var rr = SqlHelper.ExecuteReader(connVar80, CommandType.Text, mPasivoPrestaciones, pa);
          //while (rr.Read())
          //{
          //  //if (Convert.ToDouble(rr["Val_Novedad"]) > 0)
          //  //{
          //    Variable80 += (Convert.ToDouble(rr["Val_Novedad"]) * -1);
          //  //}
          //}

          //SqlParameter[] pa2 = new[] {
          //new SqlParameter("@mcodemp2", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal2", (Convert.ToInt32(data.anoContable) + 1).ToString() + "0115")
          //};
          //string mPasivoPrestaciones2 = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp2 And H.Estado = 'S' And Fec_Nomina = @mFechaFinal2 And Cod_Concepto Not In(52)";
          //var connVar80_2 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones2 = String.Format(mPasivoPrestaciones2, data.dbname);
          //var rr2 = SqlHelper.ExecuteReader(connVar80_2, CommandType.Text, mPasivoPrestaciones2, pa2);
          //while (rr2.Read())
          //{
          //  if (Convert.ToDouble(rr2["Val_Novedad"]) > 0)
          //  {
          //    Variable80 += Convert.ToDouble(rr2["Val_Novedad"]);
          //  }
          //}

          // Línea 41 Otros Pagos
            string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            ///---------------------------------------------------///
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }


          //double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40 + mValOtrosIngresos;
          double linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;


          if (data.redondear)
          {
            retefuente = UtilHelper.round(retefuente);
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            comisiones = UtilHelper.round(comisiones);
            linea37 = UtilHelper.round(linea37);
            cesantiasConsignadas = UtilHelper.round(cesantiasConsignadas);

            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);
            afc = UtilHelper.round(afc);

            #endregion

            #region CALCULO DE FECHAS Y GENERACION DE CERT GENERAL
            //Calculo de fechas con redondeo
            JulianaContext fecdbo = new JulianaContext(data.dbname);
            List<EMPLEADOS> numeroContratos = fecdbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula).OrderBy(t => t.Cod_Empleado)
                                        .ToList();
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;

            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (data.ByCedula)
            {

              if (UtilHelper.getDate(fechDesde) < date2)
              {
                fechDesde = data.anoContable + "0101";
              }

              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else
              {
                foreach (var contratos in numeroContratos)
                {
                  if (contratos.Estado == "R")
                  {
                    if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }

                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (UtilHelper.getDate(contratos.Fec_Retiro) < UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                    }
                    //DG-CE
                    if (hasta <= UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "0101");
                    }
                    else if (contratos.Estado == "A")
                    {
                      if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                      {

                        if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                        {
                          desde = UtilHelper.getDate(fechDesde);
                        }
                        else
                        {
                          desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                        }
                        if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                        {
                          hasta = UtilHelper.getDate(data.anoContable + "1231");
                        }
                        else
                        {
                          if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                          {
                            hasta = UtilHelper.getDate(fechRet);
                          }
                          else
                          {
                            hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            else
            {
              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
              else if (e.Fec_Retiro.StartsWith(data.anoContable))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
              {
                hasta = UtilHelper.getDate(e.Fec_Retiro);

              }
              else
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
            }
            linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;
          }
          JulianaContext dbo = new JulianaContext(data.dbname);
          #endregion

          #region GENERACION DE CERTIFICADOS POR CEDULA
          if (data.ByCedula)
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();

            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);
            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
            }
            if (UtilHelper.getDate(fechDesde) <= date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else
            {
              foreach (var contratos in numeroContratos)
              {
                if (contratos.Estado == "R")
                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }

                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                  else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && (contratos.Fec_Retiro.StartsWith("2022")))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet) && numeroContratos.Count > 1)
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                      //hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else if (UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "1231"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                }
                else if (contratos.Estado == "A")

                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                  {

                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }

                }
              }
            }
          }
          else
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();


            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";

            }

            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");

            }
            else if (e.Fec_Retiro.StartsWith(data.anoContable) || e.Fec_Retiro.StartsWith("2022"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
            else
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
          }

          #endregion


          #region LLENAR LOS CAMPOS FALTANTES DEL DOCUMENTO
          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));

          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));

          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));
          if (salarios.ToString("NO") != null)
          {
            htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@37", "0");
          }
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));

          if (linea37 != 0.0)
          {
            htmlCode = htmlCode.Replace("@54", linea37.ToString("N0"));
            htmlCode = htmlCode.Replace("@linea38", "0");
            
          }
          else
          {
            htmlCode = htmlCode.Replace("@54", "0");
            htmlCode = htmlCode.Replace("@linea38", linea37_2.ToString("N0"));

          }

          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@79", Math.Abs(cesantiasConsignadas).ToString("N0"));
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", Math.Abs(retefuente).ToString("N0"));
          htmlCode = htmlCode.Replace("@80", Math.Abs(Variable80).ToString("N0"));
          htmlCode = htmlCode.Replace("@59", LINEA59.ToString("N0"));
          
          #region FAMILIARES DEL EMPLEADO          
          var familiar = dbo.FAMILIARES.Where(x => x.Cod_Empleado == e.Cod_Empleado).ToList();
          foreach (var item in familiar)
          {
            var TipoDocumento = "";
            switch (item.Tipo_Documento)
            {
              case "C":
                TipoDocumento = "13";
                break;
              case "N":
                TipoDocumento = "31";
                break;
              case "T":
                TipoDocumento = "12";
                break;
              case "P":
                TipoDocumento = "41";
                break;
              case "E":
                TipoDocumento = "22";
                break;
              case "R":
                TipoDocumento = "11";
                break;
            }
            htmlCode = htmlCode.Replace("@75", TipoDocumento);
            if (item.Cedula == "")
            {
              htmlCode = htmlCode.Replace("@76", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@76", item.Cedula);

            }
            if (item.Nombre == "")
            {
              htmlCode = htmlCode.Replace("@77", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@77", item.Nombre);

            }
            if (item.Parentesco == "")
            {
              htmlCode = htmlCode.Replace("@78", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@78", item.Parentesco);

            }
          }
          if (familiar.Count == 0)
          {
            htmlCode = htmlCode.Replace("@75", " ");
            htmlCode = htmlCode.Replace("@76", " ");
            htmlCode = htmlCode.Replace("@77", " ");
            htmlCode = htmlCode.Replace("@78", " ");
          }
          #endregion

          #endregion
        }
      }
      return htmlCode;
    }
    public static string Retefuente2024(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {
      #region PARAMETROS INICIALES
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se unifican los certificados
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;
      #endregion

      #region DEVENGOS Y OTROS RENGLONES
      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }
      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // Se llenan los valores de los rengones y las novedades
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P','J') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P','J') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          JulianaContext dbb = new JulianaContext(data.dbname);

          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double bonos = 0;

          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea37 = 0.0;
          double linea37_2 = 0.0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double cesantiasConsignadas = 0.0;
          double Variable80 = 0.0;
          double afc = 0.0;
          double TopeUvtSal = parametros.Val_Uvt * 310;
          double TopeUvtBono = parametros.Val_Uvt * 41;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "52":
                string consultaBonos = "";
                short empleado = 0;
                if (linea37 != 0)
                {
                  break;
                }
                empleado = Convert.ToInt16(e.Cod_Empleado);
                var salarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Salario;
                var TiposalarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Tipo_Salario;
                var bonosEmpleado = (from H in dbb.HISTORICO
                                     join C in dbb.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                                     where
                                      H.Cod_Empleado == empleado &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "0101") >= 0 &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "1231") <= 0 &&
                                       C.CertificadoIngresos == "52" &&
                                       (new string[] { "P", "J" }).Contains(H.Estado)
                                     group new { H, C } by new
                                     {
                                       H.Cod_Concepto,
                                       C.Devengo,
                                       C.BSBonoRetefuente,
                                       H.Fec_Nomina
                                     } into g
                                     orderby
                                       g.Key.Fec_Nomina,
                                       g.Key.BSBonoRetefuente descending
                                     select new
                                     {
                                       Val_Novedad = (double?)g.Sum(p => p.H.Val_Novedad),
                                       g.Key.Cod_Concepto,
                                       g.Key.Devengo,
                                       g.Key.BSBonoRetefuente,
                                       g.Key.Fec_Nomina
                                     }).ToList();

                if (TiposalarioEmpleado == "2")
                {
                  salarioEmpleado = Math.Round((salarioEmpleado / 1.3));
                }

                foreach (var bono in bonosEmpleado)
                {
                  if (bono.BSBonoRetefuente == "N")
                  {
                    Val_Novedad += Convert.ToDouble(bono.Val_Novedad);
                    linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    //break;
                  }
                  else if (bono.BSBonoRetefuente == "S")
                  {
                    double valorDevengo = Convert.ToDouble(bono.Val_Novedad);
                    if (salarioEmpleado > TopeUvtSal)
                    {
                      Val_Novedad += valorDevengo;
                      linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    }
                    else if (salarioEmpleado <= TopeUvtSal)
                    {
                      if (valorDevengo >= TopeUvtBono)
                      {
                        //El valor del devengo - el TopeUVTsal
                        Val_Novedad += valorDevengo - TopeUvtBono;
                        linea37_2 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        //break;
                      }
                    }
                  }


                  //else if bono.BSBonoRetefuente == "S")
                  //  {
                  //    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);


                  //    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                  //    {
                  //      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                  //    }
                  //    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                  //    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                  //    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                  //    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                  //    {
                  //      mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                  //    }
                  //    else
                  //    {
                  //      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  //      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  //      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  //      {
                  //        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  //      }
                  //      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  //      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  //      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  //      if (mTotalBonos > mValorDeducibleBonos)
                  //      {
                  //        mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  //      }
                  //    }
                  //  }

                  //    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                  //  }
                }
                break;
            }
          }
          #endregion

          #region PRESTACIONES DE EMPLEADOS.
          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where HISTORICO.Cod_Concepto In(9,10,19,100) and Cedula = @cedula and HISTORICO.Estado in ('P','C','J') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P','C','J')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var presta = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (presta.Read())
          {
            if (Convert.ToDouble(presta["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(presta["Val_Novedad"]);
              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
            }
          }


          #endregion

          #region CESANTIAS CONSIGNADAS Y OTROS PAGOS

          // Cesantias Consignadas.

          if (data.ByCedula)
          {
            JulianaContext dbContext = new JulianaContext(data.dbname);

            List<EMPLEADOS> bycedula = dbContext.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                   .ToList();
            foreach (var contratos in bycedula)
            {
              SqlParameter[] pa4 = new[] {
              new SqlParameter("@cod_Empleado", contratos.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
              string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
              var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
              cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
              var rr4 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa4);
              while (rr4.Read())
              {
                if (Convert.ToDouble(rr4["Valor_Consignado"]) > 0)
                {
                  cesantiasConsignadas += Convert.ToDouble(rr4["Valor_Consignado"]);
                }
              }
            }
          }
          else
          {
            // Cesantias Consignadas.
            SqlParameter[] pa3 = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
            string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
            var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
            cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
            var rr3 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa3);
            while (rr3.Read())
            {
              if (Convert.ToDouble(rr3["Valor_Consignado"]) > 0)
              {
                cesantiasConsignadas += Convert.ToDouble(rr3["Valor_Consignado"]);
              }
            }
          }
          //INGRESOS PROMEDIOS ULTIMOS 6 MESES LINEA 59
          double LINEA59 = 0.0;
          SqlParameter[] PaProm = new[] {
            new SqlParameter("@cesantias_pagadas", Convert.ToInt32(cesantias)),
            new SqlParameter("@cesantias_consignadas", Convert.ToInt32(cesantiasConsignadas)),
            new SqlParameter("@cod_Empleado", e.Cod_Empleado),
            new SqlParameter("@estado", e.Estado),
            new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
            new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
          };
          var connParam = new SqlConnection(SqlHelper.GetConnectionString());
          var PromReader = SqlHelper.ExecuteReader(connParam, CommandType.StoredProcedure, "SP_CIR_2023L59", PaProm);
          PromReader.Read();
          LINEA59 = Convert.ToDouble(PromReader[0]);
          //while (PromReader.Read()) { rta = Convert.ToString(PromReader[0]); }



          ////Linea 56 Pasivos laborales reales consolidados en cabeza del trabajador
          //SqlParameter[] pa = new[] {
          //new SqlParameter("@mcodemp", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal", Convert.ToInt32(data.anoContable).ToString() + "1230")
          //};
          //string mPasivoPrestaciones = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp And H.Estado = 'I' And Fec_Nomina = @mFechaFinal And Cod_Concepto Not In(9,52,100,98)";
          //var connVar80 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones = String.Format(mPasivoPrestaciones, data.dbname);
          //var rr = SqlHelper.ExecuteReader(connVar80, CommandType.Text, mPasivoPrestaciones, pa);
          //while (rr.Read())
          //{
          //  //if (Convert.ToDouble(rr["Val_Novedad"]) > 0)
          //  //{
          //    Variable80 += (Convert.ToDouble(rr["Val_Novedad"]) * -1);
          //  //}
          //}

          //SqlParameter[] pa2 = new[] {
          //new SqlParameter("@mcodemp2", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal2", (Convert.ToInt32(data.anoContable) + 1).ToString() + "0115")
          //};
          //string mPasivoPrestaciones2 = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp2 And H.Estado = 'S' And Fec_Nomina = @mFechaFinal2 And Cod_Concepto Not In(52)";
          //var connVar80_2 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones2 = String.Format(mPasivoPrestaciones2, data.dbname);
          //var rr2 = SqlHelper.ExecuteReader(connVar80_2, CommandType.Text, mPasivoPrestaciones2, pa2);
          //while (rr2.Read())
          //{
          //  if (Convert.ToDouble(rr2["Val_Novedad"]) > 0)
          //  {
          //    Variable80 += Convert.ToDouble(rr2["Val_Novedad"]);
          //  }
          //}

          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            ///---------------------------------------------------///
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }


          //double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40 + mValOtrosIngresos;
          double linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;


          if (data.redondear)
          {
            retefuente = UtilHelper.round(retefuente);
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            comisiones = UtilHelper.round(comisiones);
            linea37 = UtilHelper.round(linea37);
            cesantiasConsignadas = UtilHelper.round(cesantiasConsignadas);

            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);
            afc = UtilHelper.round(afc);

            #endregion

            #region CALCULO DE FECHAS Y GENERACION DE CERT GENERAL
            //Calculo de fechas con redondeo
            JulianaContext fecdbo = new JulianaContext(data.dbname);
            List<EMPLEADOS> numeroContratos = fecdbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula).OrderBy(t => t.Cod_Empleado)
                                        .ToList();
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;

            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (data.ByCedula)
            {

              if (UtilHelper.getDate(fechDesde) < date2)
              {
                fechDesde = data.anoContable + "0101";
              }

              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else
              {
                foreach (var contratos in numeroContratos)
                {
                  if (contratos.Estado == "R")
                  {
                    if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }

                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (UtilHelper.getDate(contratos.Fec_Retiro) < UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                    }
                    //DG-CE
                    if (hasta <= UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "0101");
                    }
                    else if (contratos.Estado == "A")
                    {
                      if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                      {

                        if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                        {
                          desde = UtilHelper.getDate(fechDesde);
                        }
                        else
                        {
                          desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                        }
                        if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                        {
                          hasta = UtilHelper.getDate(data.anoContable + "1231");
                        }
                        else
                        {
                          if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                          {
                            hasta = UtilHelper.getDate(fechRet);
                          }
                          else
                          {
                            hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            else
            {
              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
              else if (e.Fec_Retiro.StartsWith(data.anoContable))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
              {
                hasta = UtilHelper.getDate(e.Fec_Retiro);

              }
              else
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
            }
            linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;
          }
          JulianaContext dbo = new JulianaContext(data.dbname);
          #endregion

          #region GENERACION DE CERTIFICADOS POR CEDULA
          if (data.ByCedula)
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();

            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);
            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
            }
            if (UtilHelper.getDate(fechDesde) <= date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else
            {
              foreach (var contratos in numeroContratos)
              {
                if (contratos.Estado == "R")
                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }

                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                  else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && (contratos.Fec_Retiro.StartsWith("2022")))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet) && numeroContratos.Count > 1)
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                      //hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else if (UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "1231"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                }
                else if (contratos.Estado == "A")

                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                  {

                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }

                }
              }
            }
          }
          else
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();


            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";

            }

            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");

            }
            else if (e.Fec_Retiro.StartsWith(data.anoContable) || e.Fec_Retiro.StartsWith("2022"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
            else
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
          }

          #endregion


          #region LLENAR LOS CAMPOS FALTANTES DEL DOCUMENTO
          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));

          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));

          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));
          if (salarios.ToString("NO") != null)
          {
            htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@37", "0");
          }
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));

          if (linea37 != 0.0)
          {
            htmlCode = htmlCode.Replace("@54", linea37.ToString("N0"));
            htmlCode = htmlCode.Replace("@linea38", "0");

          }
          else
          {
            htmlCode = htmlCode.Replace("@54", "0");
            htmlCode = htmlCode.Replace("@linea38", linea37_2.ToString("N0"));

          }

          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@79", Math.Abs(cesantiasConsignadas).ToString("N0"));
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", Math.Abs(retefuente).ToString("N0"));
          htmlCode = htmlCode.Replace("@80", Math.Abs(Variable80).ToString("N0"));
          htmlCode = htmlCode.Replace("@59", LINEA59.ToString("N0"));

          #region FAMILIARES DEL EMPLEADO          
          var familiar = dbo.FAMILIARES.Where(x => x.Cod_Empleado == e.Cod_Empleado).ToList();
          foreach (var item in familiar)
          {
            var TipoDocumento = "";
            switch (item.Tipo_Documento)
            {
              case "C":
                TipoDocumento = "13";
                break;
              case "N":
                TipoDocumento = "31";
                break;
              case "T":
                TipoDocumento = "12";
                break;
              case "P":
                TipoDocumento = "41";
                break;
              case "E":
                TipoDocumento = "22";
                break;
              case "R":
                TipoDocumento = "11";
                break;
            }
            htmlCode = htmlCode.Replace("@75", TipoDocumento);
            if (item.Cedula == "")
            {
              htmlCode = htmlCode.Replace("@76", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@76", item.Cedula);

            }
            if (item.Nombre == "")
            {
              htmlCode = htmlCode.Replace("@77", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@77", item.Nombre);

            }
            if (item.Parentesco == "")
            {
              htmlCode = htmlCode.Replace("@78", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@78", item.Parentesco);

            }
          }
          if (familiar.Count == 0)
          {
            htmlCode = htmlCode.Replace("@75", " ");
            htmlCode = htmlCode.Replace("@76", " ");
            htmlCode = htmlCode.Replace("@77", " ");
            htmlCode = htmlCode.Replace("@78", " ");
          }
          #endregion

          #endregion
        }
      }
      return htmlCode;
    }
    public static string Retefuente2025(RetefuenteViemModel data, EMPLEADOS e, string htmlCode, PARAMETROS parametros)
    {
      #region PARAMETROS INICIALES
      double mSalarioBaseBonoCanasta = 0.0;
      double mValOtrosIngresos = 0;
      double mValSaludPension = 0;
      double mValSolPension = 0;
      double mRentaExcenta = 0;
      double mValRetencion = 0;
      double mTotalBonos = 0;
      double mRedondeoSalarioBase = 0;
      double mValorDeducibleBonos = 0;
      double mTopeBonosCanasta = 0;

      // fechas, no se reemplaza hasta el final para hacer bien los calculos cuando se unifican los certificados
      DateTime desde = UtilHelper.getDate(data.anoContable + "0101");
      DateTime hasta = UtilHelper.getDate(data.anoContable + "1231");
      DateTime fechaIngreso = UtilHelper.getDate(e.Fec_Ingreso);
      DateTime FecSubstitucionPatronal = DateTime.Now;
      DateTime today = DateTime.Now;
      DateTime maxDate = new DateTime(today.Year, 3, 15);
      DateTime fechaRetiro = desde;
      #endregion

      #region DEVENGOS Y OTROS RENGLONES
      if (e.FecSubstitucionPatronal.Trim() != "")
      {
        FecSubstitucionPatronal = UtilHelper.getDate(e.FecSubstitucionPatronal);
      }
      if (FecSubstitucionPatronal != DateTime.Now)
      {
        if (FecSubstitucionPatronal.Date > fechaIngreso.Date && FecSubstitucionPatronal.Year == Convert.ToInt32(data.anoContable))
        {
          fechaIngreso = FecSubstitucionPatronal;
        }
      }
      if (fechaIngreso.Date > desde.Date)
      {
        desde = fechaIngreso;
      }
      if (e.Estado == "R")
      {
        fechaRetiro = UtilHelper.getDate(e.Fec_Retiro);
        if (fechaRetiro.Date < hasta.Date)
        {
          hasta = fechaRetiro;
        }
      }
      if (today.Date > maxDate.Date)
      {
        today = maxDate;
      }

      htmlCode = htmlCode.Replace("@linea24", e.getTipoDocDane());
      htmlCode = htmlCode.Replace("@linea25", e.Cedula);
      htmlCode = htmlCode.Replace("@linea26", e.PApellido);
      htmlCode = htmlCode.Replace("@linea27", e.SApellido);
      htmlCode = htmlCode.Replace("@linea28", e.PNombre);
      htmlCode = htmlCode.Replace("@linea29", e.SNombre);

      // Se llenan los valores de los rengones y las novedades
      string retefuenteQuery = "";
      SqlParameter[] retefuenteParams = new SqlParameter[] { };
      if (data.ByCedula)
      {
        retefuenteParams = new[] {
          new SqlParameter("@cedula", e.Cedula.ToString()),
          new SqlParameter("@fecha_desde", data.anoContable+"0101"),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, CertificadoIngresos, Cedula, Tipo_Concepto, Nom_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta) And HISTORICO.Estado in ('P','J') and  EMPLEADOS.Cedula = @cedula  and HISTORICO.Cod_Concepto not In(9,10,19,100) group by CertificadoIngresos, Nom_Concepto, Cedula, Tipo_Concepto, Devengo";
      }
      else
      {
        retefuenteParams = new[] {
          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
          new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
          new SqlParameter("@fecha_hasta", data.anoContable+"1231")
        };
        retefuenteQuery = "SELECT sum(Val_Novedad) as Val_Novedad, Devengo, Nom_Concepto, CertificadoIngresos, HISTORICO.Cod_Empleado, Tipo_Concepto ";
        retefuenteQuery += "FROM {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS ON(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) ";
        retefuenteQuery += "WHERE (Fec_Nomina BETWEEN @fecha_desde and @fecha_hasta)  And HISTORICO.Estado in ('P','J') and  HISTORICO.Cod_Empleado = @cod_empleado  and HISTORICO.Cod_Concepto not In(9,10,19,100)  group by CertificadoIngresos, HISTORICO.Cod_Empleado, Devengo, Nom_Concepto, Tipo_Concepto";
      }
      retefuenteQuery = String.Format(retefuenteQuery, data.dbname);

      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        var retefuenteReader = SqlHelper.ExecuteReader(conn, CommandType.Text, retefuenteQuery, retefuenteParams);
        if (retefuenteReader.HasRows)
        {
          JulianaContext dbb = new JulianaContext(data.dbname);

          double salarios = 0.0;
          double honorarios = 0;
          double servicios = 0;
          double comisiones = 0;
          double prestaciones = 0;
          double bonos = 0;

          double viaticos = 0;
          double gastosRepresentacion = 0.0;
          double comp_cooperativas = 0;
          double linea37 = 0.0;
          double linea37_2 = 0.0;
          double linea40 = 0.0;
          double otros_pagos = 0.0;
          double aposalud = 0.0;
          double apo_pension = 0.0;
          double apo_vol_pension = 0.0;
          double retefuente = 0.0;
          double cesantias = 0.0;
          double cesantiasConsignadas = 0.0;
          double Variable80 = 0.0;
          double afc = 0.0;
          double TopeUvtSal = parametros.Val_Uvt * 310;
          double TopeUvtBono = parametros.Val_Uvt * 41;

          mSalarioBaseBonoCanasta = 0.0;
          mValOtrosIngresos = 0;
          mValSaludPension = 0;
          mValSolPension = 0;
          mRentaExcenta = 0;
          mValRetencion = 0;
          mTotalBonos = 0;
          mRedondeoSalarioBase = 0;
          mValorDeducibleBonos = 0;
          mTopeBonosCanasta = 0;
          while (retefuenteReader.Read())
          {
            Double Val_Novedad = 0;
            switch (retefuenteReader["CertificadoIngresos"].ToString())
            {
              case "37":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                salarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "38":
                // CertificadoIngresos es igual a 38, sin embargo hace referencia a la linea 46 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                // más abajo se añaden el valor de las cesantias consinadas
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                cesantias += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "39":
                // CertificadoIngresos es igual a 39, sin embargo hace referencia a la linea 43 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                gastosRepresentacion += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "40":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                linea40 += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "43":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                aposalud += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "44":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                apo_pension += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "45":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                if (Convert.ToString(retefuenteReader["Nom_Concepto"]).Contains("AFC"))
                {
                  afc += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                else
                {
                  apo_vol_pension += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                }
                break;
              case "46":
                Val_Novedad = Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                retefuente += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "47":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 38 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                honorarios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "48":
                // CertificadoIngresos es igual a 48, sin embargo hace referencia a la linea 39 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                servicios += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "49":
                // CertificadoIngresos es igual a 49, sin embargo hace referencia a la linea 40 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comisiones += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "50":
                // CertificadoIngresos es igual a 50, sin embargo hace referencia a la linea 42 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                viaticos += retefuenteReader["Devengo"].ToString() == "S" ? Val_Novedad : -Val_Novedad;
                break;
              case "51":
                // CertificadoIngresos es igual a 51, sin embargo hace referencia a la linea 44 del nuevo certificado
                // se deja de esta manera para generar problemas con la programación de la años anteriores.
                Val_Novedad += Double.Parse(retefuenteReader["Val_Novedad"].ToString());
                comp_cooperativas += retefuenteReader["Devengo"].ToString() == "S" ? -Val_Novedad : Val_Novedad;
                break;
              case "52":
                string consultaBonos = "";
                short empleado = 0;
                if (linea37 != 0)
                {
                  break;
                }
                empleado = Convert.ToInt16(e.Cod_Empleado);
                var salarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Salario;
                var TiposalarioEmpleado = (from ex in dbb.EMPLEADOS where ex.Cod_Empleado == e.Cod_Empleado select ex).First().Tipo_Salario;
                var bonosEmpleado = (from H in dbb.HISTORICO
                                     join C in dbb.CONCEPTOS on new { Cod_Concepto = H.Cod_Concepto } equals new { Cod_Concepto = C.Cod_Concepto }
                                     where
                                      H.Cod_Empleado == empleado &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "0101") >= 0 &&
                                       String.Compare(H.Fec_Nomina, data.anoContable + "1231") <= 0 &&
                                       C.CertificadoIngresos == "52" &&
                                       (new string[] { "P", "J" }).Contains(H.Estado)
                                     group new { H, C } by new
                                     {
                                       H.Cod_Concepto,
                                       C.Devengo,
                                       C.BSBonoRetefuente,
                                       H.Fec_Nomina
                                     } into g
                                     orderby
                                       g.Key.Fec_Nomina,
                                       g.Key.BSBonoRetefuente descending
                                     select new
                                     {
                                       Val_Novedad = (double?)g.Sum(p => p.H.Val_Novedad),
                                       g.Key.Cod_Concepto,
                                       g.Key.Devengo,
                                       g.Key.BSBonoRetefuente,
                                       g.Key.Fec_Nomina
                                     }).ToList();

                if (TiposalarioEmpleado == "2")
                {
                  salarioEmpleado = Math.Round((salarioEmpleado / 1.3));
                }

                foreach (var bono in bonosEmpleado)
                {
                  if (bono.BSBonoRetefuente == "N")
                  {
                    Val_Novedad += Convert.ToDouble(bono.Val_Novedad);
                    linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    //break;
                  }
                  else if (bono.BSBonoRetefuente == "S")
                  {
                    double valorDevengo = Convert.ToDouble(bono.Val_Novedad);
                    if (salarioEmpleado > TopeUvtSal)
                    {
                      Val_Novedad += valorDevengo;
                      linea37 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                    }
                    else if (salarioEmpleado <= TopeUvtSal)
                    {
                      if (valorDevengo >= TopeUvtBono)
                      {
                        //El valor del devengo - el TopeUVTsal
                        Val_Novedad += valorDevengo - TopeUvtBono;
                        linea37_2 = bono.Devengo.ToString() == "S" ? Val_Novedad : -Val_Novedad;
                        //break;
                      }
                    }
                  }


                  //else if bono.BSBonoRetefuente == "S")
                  //  {
                  //    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);


                  //    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                  //    {
                  //      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                  //    }
                  //    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                  //    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                  //    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                  //    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                  //    {
                  //      mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                  //    }
                  //    else
                  //    {
                  //      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  //      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  //      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  //      {
                  //        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  //      }
                  //      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  //      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  //      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  //      if (mTotalBonos > mValorDeducibleBonos)
                  //      {
                  //        mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  //      }
                  //    }
                  //  }

                  //    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                  //  }
                }
                break;
            }
          }
          #endregion

          #region PRESTACIONES DE EMPLEADOS.
          // Prestaciones Empleados
          string fec_retiro = "";
          if (e.Estado == "R" && fechaRetiro.Year == Convert.ToInt32(data.anoContable))
          {
            DateTime ff = UtilHelper.getDate(e.Fec_Retiro);
            if (parametros.LiqNomina == 15 && ff.Day <= 15)
            {
              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, 15));
            }
            else
            {
              int dia = 30;
              if (ff.Month == 2)
              {
                dia = DateTime.DaysInMonth(ff.Year, ff.Month);
              }

              fec_retiro = UtilHelper.getUnglyDate(new DateTime(ff.Year, ff.Month, dia));
            }
          }
          else
          {
            fec_retiro = hasta.ToString("yyyyMMdd");
          }
          string consultaPrestaciones = "";
          SqlParameter[] parametrosPrestaciones = new SqlParameter[] { };
          if (data.ByCedula)
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde",  data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on (EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where HISTORICO.Cod_Concepto In(9,10,19,100) and Cedula = @cedula and HISTORICO.Estado in ('P','C','J') and (Fec_Nomina between @fecha_desde and @fecha_hasta)";
          }
          else
          {
            parametrosPrestaciones = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaPrestaciones = "select * from {0}.dbo.HISTORICO join {0}.dbo.CONCEPTOS on (HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Concepto In(9,10,19,100)  and (Fec_Nomina between @fecha_desde and @fecha_hasta) and Cod_Empleado = @cod_empleado and HISTORICO.Estado in ('P','C','J')";
          }
          var connCesantias33 = new SqlConnection(SqlHelper.GetConnectionString());
          consultaPrestaciones = String.Format(consultaPrestaciones, data.dbname);
          var presta = SqlHelper.ExecuteReader(connCesantias33, CommandType.Text, consultaPrestaciones, parametrosPrestaciones);
          while (presta.Read())
          {
            if (Convert.ToDouble(presta["Val_Novedad"]) != 0)
            {
              prestaciones += Convert.ToDouble(presta["Val_Novedad"]);
              var format = new System.Globalization.NumberFormatInfo();
              format.NegativeSign = "-";
              format.NumberDecimalSeparator = ".";
            }
          }


          #endregion

          #region CESANTIAS CONSIGNADAS Y OTROS PAGOS

          // Cesantias Consignadas.

          if (data.ByCedula)
          {
            JulianaContext dbContext = new JulianaContext(data.dbname);

            List<EMPLEADOS> bycedula = dbContext.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                   .ToList();
            foreach (var contratos in bycedula)
            {
              SqlParameter[] pa4 = new[] {
              new SqlParameter("@cod_Empleado", contratos.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
              string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
              var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
              cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
              var rr4 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa4);
              while (rr4.Read())
              {
                if (Convert.ToDouble(rr4["Valor_Consignado"]) > 0)
                {
                  cesantiasConsignadas += Convert.ToDouble(rr4["Valor_Consignado"]);
                }
              }
            }
          }
          else
          {
            // Cesantias Consignadas.
            SqlParameter[] pa3 = new[] {
              new SqlParameter("@cod_Empleado", e.Cod_Empleado),
              new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
              new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
            };
            string cesantiasstrinfg = "SELECT Valor_Consignado From {0}.dbo.CESANTIAS WHERE Cod_Empleado = @cod_Empleado and (Desde between @fecha_desde and @fecha_hasta) AND Tipo_Registro IN ('C','I')";
            var connCesantias = new SqlConnection(SqlHelper.GetConnectionString());
            cesantiasstrinfg = String.Format(cesantiasstrinfg, data.dbname);
            var rr3 = SqlHelper.ExecuteReader(connCesantias, CommandType.Text, cesantiasstrinfg, pa3);
            while (rr3.Read())
            {
              if (Convert.ToDouble(rr3["Valor_Consignado"]) > 0)
              {
                cesantiasConsignadas += Convert.ToDouble(rr3["Valor_Consignado"]);
              }
            }
          }
          //INGRESOS PROMEDIOS ULTIMOS 6 MESES LINEA 59
          double LINEA59 = 0.0;
          SqlParameter[] PaProm = new[] {
            new SqlParameter("@cesantias_pagadas", Convert.ToInt32(cesantias)),
            new SqlParameter("@cesantias_consignadas", Convert.ToInt32(cesantiasConsignadas)),
            new SqlParameter("@cod_Empleado", e.Cod_Empleado),
            new SqlParameter("@estado", e.Estado),
            new SqlParameter("@fecha_desde", (Convert.ToInt32(data.anoContable) - 1).ToString() + "0101"),
            new SqlParameter("@fecha_hasta", (Convert.ToInt32(data.anoContable) - 1).ToString() + "1231")
          };
          var connParam = new SqlConnection(SqlHelper.GetConnectionString());
          var PromReader = SqlHelper.ExecuteReader(connParam, CommandType.StoredProcedure, "SP_CIR_2023L59", PaProm);
          PromReader.Read();
          LINEA59 = Convert.ToDouble(PromReader[0]);
          //while (PromReader.Read()) { rta = Convert.ToString(PromReader[0]); }



          ////Linea 56 Pasivos laborales reales consolidados en cabeza del trabajador
          //SqlParameter[] pa = new[] {
          //new SqlParameter("@mcodemp", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal", Convert.ToInt32(data.anoContable).ToString() + "1230")
          //};
          //string mPasivoPrestaciones = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp And H.Estado = 'I' And Fec_Nomina = @mFechaFinal And Cod_Concepto Not In(9,52,100,98)";
          //var connVar80 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones = String.Format(mPasivoPrestaciones, data.dbname);
          //var rr = SqlHelper.ExecuteReader(connVar80, CommandType.Text, mPasivoPrestaciones, pa);
          //while (rr.Read())
          //{
          //  //if (Convert.ToDouble(rr["Val_Novedad"]) > 0)
          //  //{
          //    Variable80 += (Convert.ToDouble(rr["Val_Novedad"]) * -1);
          //  //}
          //}

          //SqlParameter[] pa2 = new[] {
          //new SqlParameter("@mcodemp2", e.Cod_Empleado),
          //new SqlParameter("@mFechaFinal2", (Convert.ToInt32(data.anoContable) + 1).ToString() + "0115")
          //};
          //string mPasivoPrestaciones2 = "Select Val_Novedad From {0}.dbo.HISTORICO H Where H.Cod_Empleado = @mcodemp2 And H.Estado = 'S' And Fec_Nomina = @mFechaFinal2 And Cod_Concepto Not In(52)";
          //var connVar80_2 = new SqlConnection(SqlHelper.GetConnectionString());
          //mPasivoPrestaciones2 = String.Format(mPasivoPrestaciones2, data.dbname);
          //var rr2 = SqlHelper.ExecuteReader(connVar80_2, CommandType.Text, mPasivoPrestaciones2, pa2);
          //while (rr2.Read())
          //{
          //  if (Convert.ToDouble(rr2["Val_Novedad"]) > 0)
          //  {
          //    Variable80 += Convert.ToDouble(rr2["Val_Novedad"]);
          //  }
          //}

          // Línea 41 Otros Pagos
          string consultaOtrosPagos = "";
          SqlParameter[] p10 = new SqlParameter[] { };
          if (data.ByCedula)
          {
            p10 = new[] {
              new SqlParameter("@cedula", e.Cedula.ToString()),
              new SqlParameter("@fecha_desde", data.anoContable+"0101"),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto,BSBonoRetefuente,Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) join {0}.dbo.EMPLEADOS on(EMPLEADOS.Cod_Empleado = HISTORICO.Cod_Empleado) Where Cedula = @cedula And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100) Order By Fec_Nomina, BSBonoRetefuente Desc";
          }
          else
          {
            p10 = new[] {
              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
              new SqlParameter("@fecha_desde", desde.ToString("yyyyMMdd")),
              new SqlParameter("@fecha_hasta", data.anoContable+"1231")
            };
            consultaOtrosPagos = "Select Fec_Nomina, Val_Novedad, HISTORICO.Cod_Concepto, BSBonoRetefuente, Devengo From {0}.dbo.HISTORICO Join {0}.dbo.CONCEPTOS On(HISTORICO.Cod_Concepto = CONCEPTOS.Cod_Concepto) Where HISTORICO.Cod_Empleado = @cod_empleado And (Fec_Nomina BETWEEN @fecha_desde And @fecha_hasta) And HISTORICO.Estado In('P','C','J')  And CertificadoIngresos = '41' and HISTORICO.Cod_Concepto not In(9,10,19,100)  Order By Fec_Nomina, BSBonoRetefuente Desc";
          }

          consultaOtrosPagos = String.Format(consultaOtrosPagos, data.dbname);
          using (var conn2 = new SqlConnection(SqlHelper.GetConnectionString()))
          {
            ///---------------------------------------------------///
            var historicoReader = SqlHelper.ExecuteReader(conn2, CommandType.Text, consultaOtrosPagos, p10);
            if (historicoReader.HasRows)
            {
              string mFechaNominaSal = "";
              while (historicoReader.Read())
              {
                if (historicoReader["BSBonoRetefuente"].ToString() == "S")
                {
                  string nFechaNomina = historicoReader["Fec_Nomina"].ToString().Substring(0, 6);
                  //if (mFechaNominaSal != nFechaNomina)
                  //{
                  var p11 = new[] {
                              new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                              new SqlParameter("@fecha_nomina", nFechaNomina+"01")
                          };
                  string q11 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                  q11 = String.Format(q11, data.dbname);
                  using (var conn3 = new SqlConnection(SqlHelper.GetConnectionString()))
                  {
                    var salarioReader = SqlHelper.ExecuteReader(conn3, CommandType.Text, q11, p11);
                    if (salarioReader.HasRows)
                    {
                      salarioReader.Read();
                      mSalarioBaseBonoCanasta = Double.Parse(salarioReader["Salario"].ToString());
                    }
                    else
                    {
                      var p12 = new[] {
                          new SqlParameter("@cod_empleado", e.Cod_Empleado.ToString()),
                          new SqlParameter("@fecha_nomina", nFechaNomina+"30")
                        };
                      string q12 = "Select Salario From {0}.dbo.SALARIOS Where Fec_Salario <= @fecha_nomina And Cod_Empleado = @cod_empleado order by Fec_Salario desc";
                      q12 = String.Format(q12, data.dbname);
                      using (var conn4 = new SqlConnection(SqlHelper.GetConnectionString()))
                      {
                        var salarioReader2 = SqlHelper.ExecuteReader(conn4, CommandType.Text, q12, p12);
                        if (salarioReader2.HasRows)
                        {
                          salarioReader2.Read();
                          mSalarioBaseBonoCanasta = Double.Parse(salarioReader2["Salario"].ToString());
                        }
                      }
                    }

                    ///
                    mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                    mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                    if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                    {
                      mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                    }
                    mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                    mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                    mTopeBonosCanasta = mTopeBonosCanasta * 100;
                    if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                    {
                      mValOtrosIngresos = mValOtrosIngresos + Convert.ToDouble(historicoReader["Val_Novedad"]);
                    }
                    else
                    {
                      mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                      mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                      if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                      {
                        mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                      }
                      mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                      mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                      mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                      if (mTotalBonos > mValorDeducibleBonos)
                      {
                        mValOtrosIngresos = mValOtrosIngresos + (Convert.ToDouble(historicoReader["Val_Novedad"]) - mValorDeducibleBonos);
                      }
                    }
                    mFechaNominaSal = nFechaNomina;
                    mTotalBonos = 0;
                  }
                  //}
                  //mTotalBonos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                }
                else
                {
                  if (historicoReader["Devengo"].ToString() == "S")
                  {
                    mValOtrosIngresos += Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                  else if (historicoReader["Devengo"].ToString() == "N")
                  {
                    mValOtrosIngresos -= Double.Parse(historicoReader["Val_Novedad"].ToString());
                  }
                }
              }

              if (mTotalBonos != 0)
              {
                mTopeBonosCanasta = Math.Round((parametros.Val_Uvt * 310), 0);
                mRedondeoSalarioBase = Double.Parse(mTopeBonosCanasta.ToString().Substring(mTopeBonosCanasta.ToString().Length - 3));
                if (mRedondeoSalarioBase <= 50 && mRedondeoSalarioBase > 0)
                {
                  mTopeBonosCanasta = mTopeBonosCanasta - mRedondeoSalarioBase;
                }
                mTopeBonosCanasta = (mTopeBonosCanasta / 100);
                mTopeBonosCanasta = Math.Round(mTopeBonosCanasta, 0);
                mTopeBonosCanasta = mTopeBonosCanasta * 100;
                if ((mSalarioBaseBonoCanasta / 1.3) > mTopeBonosCanasta)
                {
                  mValOtrosIngresos = mValOtrosIngresos + mTotalBonos;
                }
                else
                {
                  mValorDeducibleBonos = Math.Round((parametros.Val_Uvt * 41), 0);
                  mRedondeoSalarioBase = Double.Parse(mValorDeducibleBonos.ToString().Substring(mValorDeducibleBonos.ToString().Length - 4));
                  if (mRedondeoSalarioBase <= 500 && mRedondeoSalarioBase > 0)
                  {
                    mValorDeducibleBonos = mValorDeducibleBonos - mRedondeoSalarioBase;
                  }
                  mValorDeducibleBonos = (mValorDeducibleBonos / 1000);
                  mValorDeducibleBonos = Math.Round(mValorDeducibleBonos, 0);
                  mValorDeducibleBonos = mValorDeducibleBonos * 1000;
                  if (mTotalBonos > mValorDeducibleBonos)
                  {
                    mValOtrosIngresos = mValOtrosIngresos + (mTotalBonos - mValorDeducibleBonos);
                  }
                }
              }
              otros_pagos = mValOtrosIngresos;
            }
          }

          apo_vol_pension = Math.Abs(apo_vol_pension);
          viaticos = Math.Abs(viaticos);
          if (salarios < 0)
          {
            salarios = 0;
          }

          if (cesantias < 0)
          {
            cesantias = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }

          if (honorarios < 0)
          {
            honorarios = 0;
          }

          if (comisiones < 0)
          {
            comisiones = 0;
          }

          if (prestaciones < 0)
          {
            prestaciones = 0;
          }

          if (viaticos < 0)
          {
            viaticos = 0;
          }

          if (comp_cooperativas < 0)
          {
            comp_cooperativas = 0;
          }

          if (otros_pagos < 0)
          {
            otros_pagos = 0;
          }

          if (linea40 < 0)
          {
            linea40 = 0;
          }

          if (gastosRepresentacion < 0)
          {
            gastosRepresentacion = 0;
          }


          //double linea48 = salarios + cesantias + gastosRepresentacion + honorarios + comisiones + prestaciones + viaticos + comp_cooperativas + otros_pagos + linea40 + mValOtrosIngresos;
          double linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;


          if (data.redondear)
          {
            retefuente = UtilHelper.round(retefuente);
            salarios = UtilHelper.round(salarios);
            cesantias = UtilHelper.round(cesantias);
            comisiones = UtilHelper.round(comisiones);
            linea37 = UtilHelper.round(linea37);
            cesantiasConsignadas = UtilHelper.round(cesantiasConsignadas);

            if (gastosRepresentacion > 0.0)
            {
              gastosRepresentacion = UtilHelper.round(gastosRepresentacion);
            }

            if (linea40 > 0.0)
            {
              linea40 = UtilHelper.round(linea40);
            }

            otros_pagos = UtilHelper.round(otros_pagos);
            aposalud = UtilHelper.round(aposalud);
            apo_pension = UtilHelper.round(apo_pension);
            apo_vol_pension = UtilHelper.round(apo_vol_pension);
            viaticos = UtilHelper.round(viaticos);
            prestaciones = UtilHelper.round(prestaciones);
            afc = UtilHelper.round(afc);

            #endregion

            #region CALCULO DE FECHAS Y GENERACION DE CERT GENERAL
            //Calculo de fechas con redondeo
            JulianaContext fecdbo = new JulianaContext(data.dbname);
            List<EMPLEADOS> numeroContratos = fecdbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula).OrderBy(t => t.Cod_Empleado)
                                        .ToList();
            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;

            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (data.ByCedula)
            {

              if (UtilHelper.getDate(fechDesde) < date2)
              {
                fechDesde = data.anoContable + "0101";
              }

              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else
              {
                foreach (var contratos in numeroContratos)
                {
                  if (contratos.Estado == "R")
                  {
                    if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }

                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                    else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                      {
                        desde = UtilHelper.getDate(fechDesde);
                      }
                      else
                      {
                        desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                      }
                      if (UtilHelper.getDate(contratos.Fec_Retiro) < UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(data.anoContable + "1231");
                      }
                    }
                    //DG-CE
                    if (hasta <= UtilHelper.getDate(data.anoContable + "0101"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "0101");
                    }
                    else if (contratos.Estado == "A")
                    {
                      if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                      {

                        if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                        {
                          desde = UtilHelper.getDate(fechDesde);
                        }
                        else
                        {
                          desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                        }
                        if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                        {
                          hasta = UtilHelper.getDate(data.anoContable + "1231");
                        }
                        else
                        {
                          if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                          {
                            hasta = UtilHelper.getDate(fechRet);
                          }
                          else
                          {
                            hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            else
            {
              if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
              {
                desde = UtilHelper.getDate(data.anoContable + "0101");
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
              else if (e.Fec_Retiro.StartsWith(data.anoContable))
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");
              }
              else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
              {
                hasta = UtilHelper.getDate(e.Fec_Retiro);

              }
              else
              {
                hasta = UtilHelper.getDate(data.anoContable + "1231");

              }
            }
            linea48 = salarios + honorarios + servicios + comisiones + prestaciones + viaticos + linea37 + comp_cooperativas + otros_pagos + cesantias + cesantiasConsignadas + linea40;
          }
          JulianaContext dbo = new JulianaContext(data.dbname);
          #endregion

          #region GENERACION DE CERTIFICADOS POR CEDULA
          if (data.ByCedula)
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();

            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);
            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";
            }
            if (UtilHelper.getDate(fechDesde) <= date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else
            {
              foreach (var contratos in numeroContratos)
              {
                if (contratos.Estado == "R")
                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && contratos.Fec_Retiro.StartsWith(data.anoContable))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }

                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                  else if (contratos.Fec_Ingreso.StartsWith(data.anoContable) && (contratos.Fec_Retiro.StartsWith("2022")))
                  {
                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet) && numeroContratos.Count > 1)
                    {
                      hasta = UtilHelper.getDate(fechRet);
                    }
                    else if (contratos.Fec_Retiro.StartsWith(data.anoContable))
                    {
                      hasta = UtilHelper.getDate(fechRet);
                      //hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else if (UtilHelper.getDate(contratos.Fec_Retiro) > UtilHelper.getDate(data.anoContable + "1231"))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                    }
                  }
                }
                else if (contratos.Estado == "A")

                {
                  if (contratos.Fec_Ingreso.StartsWith(data.anoContable))
                  {

                    if (UtilHelper.getDate(contratos.Fec_Ingreso) >= UtilHelper.getDate(fechDesde))
                    {
                      desde = UtilHelper.getDate(fechDesde);
                    }
                    else
                    {
                      desde = UtilHelper.getDate(contratos.Fec_Ingreso);
                    }
                    if (!string.IsNullOrEmpty(contratos.Fec_Retiro))
                    {
                      hasta = UtilHelper.getDate(data.anoContable + "1231");
                    }
                    else
                    {
                      if (UtilHelper.getDate(contratos.Fec_Retiro) <= UtilHelper.getDate(fechRet))
                      {
                        hasta = UtilHelper.getDate(fechRet);
                      }
                      else
                      {
                        hasta = UtilHelper.getDate(contratos.Fec_Retiro);
                      }
                    }
                  }

                }
              }
            }
          }
          else
          {
            List<EMPLEADOS> numeroContratos = dbo.EMPLEADOS.Where(t => t.Cedula == e.Cedula)
                                      .ToList();


            var fechDesde = numeroContratos[0].Fec_Ingreso;
            var fechRet = numeroContratos[0].Fec_Retiro;
            DateTime date2 = new DateTime(Convert.ToInt16(data.anoContable), 1, 1, 12, 0, 0);

            if (UtilHelper.getDate(fechDesde) < date2)
            {
              fechDesde = data.anoContable + "0101";

            }

            if (UtilHelper.getDate(fechDesde) < date2 && fechRet.Contains("  "))
            {
              desde = UtilHelper.getDate(data.anoContable + "0101");
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            if (string.IsNullOrEmpty(e.Fec_Retiro) || e.Fec_Retiro.Contains(" "))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");

            }
            else if (e.Fec_Retiro.StartsWith(data.anoContable) || e.Fec_Retiro.StartsWith("2022"))
            {
              hasta = UtilHelper.getDate(data.anoContable + "1231");
            }
            else if (UtilHelper.getDate(e.Fec_Retiro) > date2)
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
            else
            {
              hasta = UtilHelper.getDate(e.Fec_Retiro);

            }
          }

          #endregion


          #region LLENAR LOS CAMPOS FALTANTES DEL DOCUMENTO
          htmlCode = htmlCode.Replace("@30aaaa", desde.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@30mm", desde.ToString("MM"));
          htmlCode = htmlCode.Replace("@30dd", desde.ToString("dd"));

          htmlCode = htmlCode.Replace("@31aaaa", hasta.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = htmlCode.Replace("@31dd", hasta.ToString("dd"));

          htmlCode = data.ByCedula ? htmlCode.Replace("@31mm", "12") : htmlCode.Replace("@31mm", hasta.ToString("MM"));
          htmlCode = data.ByCedula ? htmlCode.Replace("@31dd", "31") : htmlCode.Replace("@31dd", hasta.ToString("dd"));
          htmlCode = htmlCode.Replace("@32aaaa", today.ToString("yyyy"));
          htmlCode = htmlCode.Replace("@32mm", today.ToString("MM"));
          htmlCode = htmlCode.Replace("@32dd", today.ToString("dd"));
          if (salarios.ToString("NO") != null)
          {
            htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          }
          else
          {
            htmlCode = htmlCode.Replace("@37", "0");
          }
          htmlCode = htmlCode.Replace("@37", salarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@38", honorarios.ToString("N0"));
          htmlCode = htmlCode.Replace("@39", servicios.ToString("N0"));
          htmlCode = htmlCode.Replace("@40", comisiones.ToString("N0"));

          if (linea37 != 0.0)
          {
            htmlCode = htmlCode.Replace("@54", linea37.ToString("N0"));
            htmlCode = htmlCode.Replace("@linea38", "0");

          }
          else
          {
            htmlCode = htmlCode.Replace("@54", "0");
            htmlCode = htmlCode.Replace("@linea38", linea37_2.ToString("N0"));

          }

          htmlCode = htmlCode.Replace("@41", prestaciones.ToString("N0"));
          htmlCode = htmlCode.Replace("@42", viaticos.ToString("N0"));
          htmlCode = htmlCode.Replace("@43", gastosRepresentacion.ToString("N0"));
          htmlCode = htmlCode.Replace("@44", comp_cooperativas.ToString("N0"));
          htmlCode = htmlCode.Replace("@45", otros_pagos.ToString("N0"));
          htmlCode = htmlCode.Replace("@46", cesantias.ToString("N0"));
          htmlCode = htmlCode.Replace("@79", Math.Abs(cesantiasConsignadas).ToString("N0"));
          htmlCode = htmlCode.Replace("@47", linea40.ToString("N0"));
          htmlCode = htmlCode.Replace("@48", linea48.ToString("N0"));
          htmlCode = htmlCode.Replace("@49", aposalud.ToString("N0"));
          htmlCode = htmlCode.Replace("@50", Math.Abs(apo_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@51", Math.Abs(apo_vol_pension).ToString("N0"));
          htmlCode = htmlCode.Replace("@52", Math.Abs(afc).ToString("N0"));
          htmlCode = htmlCode.Replace("@53", Math.Abs(retefuente).ToString("N0"));
          htmlCode = htmlCode.Replace("@80", Math.Abs(Variable80).ToString("N0"));
          htmlCode = htmlCode.Replace("@59", LINEA59.ToString("N0"));

          #region FAMILIARES DEL EMPLEADO          
          var familiar = dbo.FAMILIARES.Where(x => x.Cod_Empleado == e.Cod_Empleado).ToList();
          foreach (var item in familiar)
          {
            var TipoDocumento = "";
            switch (item.Tipo_Documento)
            {
              case "C":
                TipoDocumento = "13";
                break;
              case "N":
                TipoDocumento = "31";
                break;
              case "T":
                TipoDocumento = "12";
                break;
              case "P":
                TipoDocumento = "41";
                break;
              case "E":
                TipoDocumento = "22";
                break;
              case "R":
                TipoDocumento = "11";
                break;
            }
            htmlCode = htmlCode.Replace("@75", TipoDocumento);
            if (item.Cedula == "")
            {
              htmlCode = htmlCode.Replace("@76", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@76", item.Cedula);

            }
            if (item.Nombre == "")
            {
              htmlCode = htmlCode.Replace("@77", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@77", item.Nombre);

            }
            if (item.Parentesco == "")
            {
              htmlCode = htmlCode.Replace("@78", " ");

            }
            else
            {
              htmlCode = htmlCode.Replace("@78", item.Parentesco);

            }
          }
          if (familiar.Count == 0)
          {
            htmlCode = htmlCode.Replace("@75", " ");
            htmlCode = htmlCode.Replace("@76", " ");
            htmlCode = htmlCode.Replace("@77", " ");
            htmlCode = htmlCode.Replace("@78", " ");
          }
          #endregion

          #endregion
        }
      }
      return htmlCode;
    }


    [HttpGet]
    public ActionResult TestDocx()
    {
      Document document = new Document();
      document.LoadFromFile(@"C:\Users\crowl\Documents\DOCS JULIANAWEB\ccs.doc");
      document.Replace("[NOM_EMPLEADO]", "Manjarres Sandoval Adrian Rafael", false, true);

      document.Replace("[token]", DateTime.Now.Ticks.ToString(), false, true);

      document.Replace("[FEC_ACTUAL]", DateTime.Now.ToString("D"), false, true);
      document.Replace("[NOM_EMPRESA]", "BNP PARIBAS COLOMBIA CORPORACION FINANCIERA S.A. ", false, true);

      ToPdfParameterList toPdf = new ToPdfParameterList();
      document.SaveToFile(@"C:\Users\crowl\Documents\DOCS JULIANAWEB\pdfprueba.pdf", toPdf);

      MemoryStream mss = new MemoryStream();
      //PdfDocument doc = new PdfDocument();
      //doc.LoadFromFile(@"C:\Users\crowl\Documents\DOCS JULIANAWEB\pdfprueba.pdf");
      //doc.Security.UserPassword = "Prueba";
      //doc.SaveToStream(mss);
      document.SaveToStream(mss, toPdf);
      return File(mss.ToArray(), "application/msword", "CertLaboral.docx");



    }

  }







}
