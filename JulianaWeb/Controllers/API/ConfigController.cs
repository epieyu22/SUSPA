using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Config")]
  public class ConfigController : ApiController
  {
    Dictionary<string, string> FILTERS = new Dictionary<string, string>() {
            { "Empleados",  "1" },
            { "Cargos", "2" },
            { "Deptos", "3" },
            { "Ccostos", "4" },
            { "Sucursales", "5" },
            { "Zonas", "6" },
            { "999", "999" },
        };

    string FILTER_EMPLEADO = "1";
    string FILTER_CARGO = "2";
    string FILTER_DEPTO = "3";
    string FILTER_CCOSTO = "4";
    string FILTER_SUCURSAL = "5";
    string FILTER_ZONA = "6";
    string FILTER_DEFAULT = "999";

    PrincipalContext pc = null;
    UserPrincipal principal = null;

    [Route("ADLogin")]
    [HttpGet]
    public IHttpActionResult ADLogin()
    {
      //var username = Thread.CurrentPrincipal.Identity.Name;
      var username = System.Web.HttpContext.Current.User.Identity.Name;
      //var username = System.Security.Principal.WindowsIdentity.GetCurrent();
      //string Name = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()).Identity.Name;
      //return Json(username);
      pc = new PrincipalContext(ContextType.Domain, "systemssas.local");
      principal = UserPrincipal.FindByIdentity(pc, username);

      var firstName = principal.GivenName ?? string.Empty;
      var lastName = principal.Surname ?? string.Empty;

      string company = String.Empty;

      if (principal.GetUnderlyingObjectType() == typeof(DirectoryEntry))
      {
        // Transition to directory entry to get other properties
        using (var entry = (DirectoryEntry)principal.GetUnderlyingObject())
        {
          if (entry.Properties["docIdentidad"] != null)
            company = entry.Properties["docIdentidad"].Value.ToString();
        }
      }


      return Ok(string.Format("Hello {0} {1} - {2}!", firstName, lastName, company));

    }

    [Route("{Empresa}")]
    [HttpGet]
    public IHttpActionResult GetConfig(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      PARAMETROS_WEB config = db.PARAMETROS_WEB.FirstOrDefault();
      List<short> anos = db.PARAMETROS.Select(p => p.Ano).ToList();
      return Json(new { config, anos });
    }

    [Route("{Empresa}")]
    [HttpPost]
    public IHttpActionResult setCompropagoConfig(string Empresa, [FromBody] PARAMETROS_WEB data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      db.Entry(data).State = EntityState.Modified;
      db.SaveChanges();
      return Ok();
    }


    [Route("{Empresa}/Anos")]
    [HttpGet]
    public IHttpActionResult GetAnosParamtros(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      List<short> anos = db.PARAMETROS.Select(p => p.Ano).ToList();
      return Json(anos);
    }

    [Route("{Empresa}/Compropago")]
    [HttpGet]
    public IHttpActionResult GetCompropagoConfig(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var parametros = db.PARAMETROS.ToList().Last();
      var parametros_web = db.PARAMETROS_WEB.FirstOrDefault();
      List<short> anos = db.PARAMETROS.Select(p => p.Ano).ToList();
      //var Aprobacio_Pagos = db.APROBACION_PAGOS.ToList();
      string Fec_Nomnia = String.Empty;
      string aux = String.Empty;

      Fec_Nomnia = db.HISTORICO.Max(h => h.Fec_Nomina);

      //string Ult_Pago = db.HISTORICO.Where(h => h.Estado == "P").Select(h => h.Fec_Nomina).Distinct().Max();
      return Json(new
      {
        liqNomina = parametros.LiqNomina,
        data = parametros_web,
        Fec_Nomnia = Fec_Nomnia,
        Ult_Pago = parametros.Fec_Nomina,
        anos = anos
      });
    }

    [Route("{Empresa}/{Cod_Empleado}/Compropago/AprobarUltPago")]
    [HttpPost]
    public IHttpActionResult SetLatsFecNomina(string Empresa, short Cod_Empleado, [FromBody] string Fec_Nomina)
    {
      JulianaContext db = new JulianaContext(Empresa);
      db.APROBACION_PAGOS.Add(new APROBACION_PAGOS
      {
        Fecha_Nomina = Fec_Nomina,
        Cod_Empleado = Cod_Empleado,
        Fecha_Aprobacion = DateTime.Now

      });
      db.SaveChanges();
      return Ok();
    }

    [Route("{Empresa}/{Cod_Empleado}/Compropago/ReversasUltPago")]
    [HttpPost]
    public IHttpActionResult ReverseLatsFecNomina(string Empresa, short Cod_Empleado, [FromBody] string Fec_Nomina)
    {
      JulianaContext db = new JulianaContext(Empresa);
      db.APROBACION_PAGOS.Where(e => e.Fecha_Nomina == Fec_Nomina);
      db.SaveChanges();
      return Ok();
    }



    [Route("certlab/Default/{Empresa}")]
    [HttpGet]
    public IHttpActionResult GetDefaultCertLabConfig(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var config = db.PARAMETROS_CERTLAB.FirstOrDefault(c => c.Filter == FILTER_DEFAULT);
      var parametros = db.PARAMETROS_CERTLAB.ToList();
      var empleados = db.EMPLEADOS.Select(e => new { e.Cod_Empleado, e.Empleado, e.Estado }).ToList();
      var zonas = db.ZONAS.ToList();
      var surcusales = db.SUCURSALES.Select(s => new { s.Cod_Sucursal, s.Cod_Zona, s.Nom_Sucursal }).ToList();
      var deptos = db.DEPTOS.ToList();
      var ccostos = db.CCOSTOS.Select(c => new { c.Nom_Ccosto, c.Cod_Ccosto }).ToList();
      var conceptos = db.CONCEPTOS.ToList();
      var terceros = db.TERCEROS.ToList();

      return Ok(new { zonas, surcusales, deptos, ccostos, conceptos, empleados, config, parametros, terceros });
    }

    [Route("certlab/{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult GetCertlabConfig(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);
      var configQuery = from c in db.PARAMETROS_CERTLAB
                        where
                        (c.Filter == FILTER_EMPLEADO && c.Cod_Filter == empleado.Cod_Empleado)
                        || (c.Filter == FILTER_CARGO && c.Cod_Filter == empleado.Cod_Cargo)
                        || (c.Filter == FILTER_DEPTO && c.Cod_Filter == empleado.Cod_Depto)
                        || (c.Filter == FILTER_CCOSTO && c.Cod_Filter == empleado.Cod_Ccostos)
                        || (c.Filter == FILTER_SUCURSAL && c.Cod_Filter == empleado.Cod_Sucursal)
                        || (c.Filter == FILTER_ZONA && c.Cod_Filter == empleado.Cod_Zona)
                        orderby c.Filter
                        select c;

      var config = configQuery.FirstOrDefault();

      if (config == null)
      {
        config = db.PARAMETROS_CERTLAB.FirstOrDefault(c => c.Filter == FILTER_DEFAULT);
      }
      return Ok(config);
    }



    [Route("{Empresa}/Retefuente")]
    [HttpGet]
    public IHttpActionResult GetRetefuenteConfig(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var parametros = db.PARAMETROS_WEB.FirstOrDefault();
      return Json(parametros);
    }

    [Route("Certlab/FirmaDigital/{Empresa}/{Filter}/{Cod_Filter}/")]
    [HttpPost]
    public IHttpActionResult UploadFirmaDigital(string Empresa, string Filter, short Cod_Filter)
    {
      JulianaContext db = new JulianaContext(Empresa);
      string uploadpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/firmas/");
      var data = System.Web.HttpContext.Current.Request.Form;
      //var filter = FILTERS[Filter];
      var parametros = db.PARAMETROS_CERTLAB
                          .Where(p => p.Filter == Filter
                          && p.Cod_Filter == Cod_Filter);
      PARAMETROS_CERTLAB config;
      if (parametros.Count() > 0)
      {
        config = parametros.First();
      }
      else
      {
        parametros = db.PARAMETROS_CERTLAB
                            .Where(p => p.Filter == "999");
        var defaultConfig = parametros.First();
        config = new PARAMETROS_CERTLAB
        {
          Filter = Filter,
          Cod_Filter = Cod_Filter,
          Base_Salario = defaultConfig.Base_Salario,
          Conceptos = defaultConfig.Conceptos,
          Horas_Extras = defaultConfig.Horas_Extras,
          Meses_Promedio = defaultConfig.Meses_Promedio,
          Otros_Devengos = defaultConfig.Otros_Devengos,
          Otros_Ingresos = defaultConfig.Otros_Ingresos,
          Texto_Embajada = defaultConfig.Texto_Embajada,
          Texto_Viaje_Laboral = defaultConfig.Texto_Viaje_Laboral
        };
      }
      config.Firma_Digital = Convert.ToBoolean(data["Firma_Digital"]);
      config.Cod_Empleado_Autoriza = Convert.ToInt16(data["Cod_Empleado_Autoriza"]);
      if (System.Web.HttpContext.Current.Request.Files.Count > 0 && config.Firma_Digital == true)
      {
        var firmaFile = System.Web.HttpContext.Current.Request.Files[0];
        config.Firma_Filename = firmaFile.FileName;
        firmaFile.SaveAs(uploadpath + firmaFile.FileName);
      }
      db.Entry(config).State = config.Cod_Parametros_Certlab == 0 ?
                             EntityState.Added :
                             EntityState.Modified;
      db.SaveChanges();
      return Ok();
    }

    [Route("Certlab/{Empresa}/{Filter}/{Cod_Filter}/")]
    [HttpGet]
    public IHttpActionResult GetCertlabConfig(string Empresa, string Filter, short Cod_Filter)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var parametros = db.PARAMETROS_CERTLAB
                          .Where(p => p.Filter == Filter
                          && p.Cod_Filter == Cod_Filter);
      PARAMETROS_CERTLAB config;
      if (parametros.Count() > 0)
      {
        config = parametros.First();
      }
      else
      {
        config = db.PARAMETROS_CERTLAB
                        .FirstOrDefault(p => p.Filter == "999"
                        && p.Cod_Filter == 999);
      }
      return Ok(config);
    }

    [Route("Certlab/{Empresa}/{Filter}/{Cod_Filter}/")]
    [HttpPost]
    public IHttpActionResult SetCertlabConfig(string Empresa, string Filter, short Cod_Filter, [FromBody] PARAMETROS_CERTLAB data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var parametros = db.PARAMETROS_CERTLAB
                          .Where(p => p.Filter == Filter
                          && p.Cod_Filter == Cod_Filter);
      if (parametros.Count() > 0)
      {
        db.PARAMETROS_CERTLAB.Attach(data);
        db.Entry(data).State = EntityState.Modified; ;
        db.SaveChanges();
      }
      else
      {
        db.PARAMETROS_CERTLAB.Add(data);
        db.SaveChanges();
      }
      return Ok();
    }


    [Route("Aprobadores/{Empresa}/{Filtro}/{Cod_Filtro}")]
    [HttpGet]
    public IHttpActionResult GetAprobadoresConfig(string Empresa, string filtro, short Cod_Filtro)
    {
      filtro = FILTERS[filtro];
      JulianaContext db = new JulianaContext(Empresa);
      var aprobadores = db.APROBADORES
                          .Where(p => p.Filtro == filtro
                          && p.Cod_Filtro == Cod_Filtro);
      if (aprobadores.Count() > 0)
      {
        var vacaciones = aprobadores.SingleOrDefault(a => a.Tipo_Aprobacion == "V");
        var cesantias = aprobadores.SingleOrDefault(a => a.Tipo_Aprobacion == "C");
        short? Cod_Aprobador_Vacaciones = null;
        short? Cod_Aprobador_Cesantias = null;
        if (vacaciones != null) { Cod_Aprobador_Vacaciones = vacaciones.Cod_Empleado; }
        if (cesantias != null) { Cod_Aprobador_Cesantias = vacaciones.Cod_Empleado; }
        return Ok(new { Cod_Aprobador_Vacaciones, Cod_Aprobador_Cesantias });
      }
      else
      {
        return Ok();
      }
    }

    [Route("HojavidaDocuments/{Empresa}/{Cedula}")]
    [HttpPost]
    public IHttpActionResult uploadHojavidaDocuments(string Empresa, string Cedula)
    {
      /* Se usa la cedula en vez del código de usuario  para poder encontrar la hoja de vida en la base de datos*/
      string uploadpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/uploads/");
      string year = DateTime.Now.ToString("yyyy");
      string month = DateTime.Now.ToString("MM");
      string day = DateTime.Now.ToString("dd");
      string fileUploadPath = uploadpath + "/" + year + "/" + month + "/" + day + "/";
      if (System.Web.HttpContext.Current.Request.Files.Count > 0)
      {
        new FileInfo(fileUploadPath).Directory.Create();
        var fileToUpload = System.Web.HttpContext.Current.Request.Files[0];
        var filename = Empresa + "_" + Cedula + "_" + fileToUpload.FileName;
        fileToUpload.SaveAs(fileUploadPath + filename);
        // Save in DB

      }
      return Ok();
    }


    [Route("Aprobadores/{Empresa}/{Filtro}/{Cod_Filtro}")]
    [HttpPost]
    public IHttpActionResult setAprobadoresConfig(string Empresa, string filtro, short Cod_Filtro,
        [FromBody] ConfigAprobadoresViewmodel data)
    {
      filtro = FILTERS[filtro];
      JulianaContext db = new JulianaContext(Empresa);
      var aprobadores = db.APROBADORES
                          .Where(p => p.Filtro == filtro
                          && p.Cod_Filtro == Cod_Filtro);
      if (aprobadores.Count() > 0)
      {
        var vacaciones = aprobadores.SingleOrDefault(a => a.Tipo_Aprobacion == "V");
        var cesantias = aprobadores.SingleOrDefault(a => a.Tipo_Aprobacion == "C");
        if (vacaciones != null)
        {
          vacaciones.Cod_Empleado = data.Cod_Aprobador_Vacaciones;
          db.APROBADORES.Attach(vacaciones);
        }
        else
        {
          APROBADORES v = new APROBADORES()
          {
            Filtro = filtro,
            Cod_Filtro = Cod_Filtro,
            Tipo_Aprobacion = "V",
            Cod_Empleado = data.Cod_Aprobador_Vacaciones,
            Estado = "A"
          };
          db.APROBADORES.Add(v);
        }
        if (cesantias != null)
        {
          cesantias.Cod_Empleado = data.Cod_Aprobador_Cesantias;
          db.APROBADORES.Attach(cesantias);
        }
        else
        {
          APROBADORES c = new APROBADORES()
          {
            Filtro = filtro,
            Cod_Filtro = Cod_Filtro,
            Tipo_Aprobacion = "C",
            Cod_Empleado = data.Cod_Aprobador_Vacaciones,
            Estado = "A"
          };
          db.APROBADORES.Add(c);
        }
        db.SaveChanges();
        return Ok();
      }
      else
      {
        APROBADORES v = new APROBADORES()
        {
          Filtro = filtro,
          Cod_Filtro = Cod_Filtro,
          Tipo_Aprobacion = "V",
          Cod_Empleado = data.Cod_Aprobador_Vacaciones,
          Estado = "A"
        };
        db.APROBADORES.Add(v);
        APROBADORES c = new APROBADORES()
        {
          Filtro = filtro,
          Cod_Filtro = Cod_Filtro,
          Tipo_Aprobacion = "C",
          Cod_Empleado = data.Cod_Aprobador_Vacaciones,
          Estado = "A"
        };

        db.APROBADORES.Add(c);
        db.SaveChanges();
        return Ok();
      }
    }
  }
}
