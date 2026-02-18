using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JW3.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API")]
  public class UtilController : ApiController
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

    [Route("util/deptos")]
    [HttpGet]
    public IHttpActionResult GetDeptos()
    {
      JulianaContext db = new JulianaContext();
      var deptos = db.DEPARTAMENTOS.ToList();
      return Json(deptos);
    }

    [Route("util/deptos/{cod_depto}/ciudades")]
    [HttpGet]
    public IHttpActionResult GetDeptos(string cod_depto)
    {
      JulianaContext db = new JulianaContext();
      var ciudades = db.CIUDADES.Where(c => c.Codigo_Departamento == cod_depto);
      return Json(ciudades);
    }

    [Route("Festivos/{year}")]
    [HttpGet]
    public IHttpActionResult GetDiasFestivos(int year)
    {
      var festivos = UtilHelper.getHolidays(year - 1);
      festivos.AddRange(UtilHelper.getHolidays(year));
      festivos.AddRange(UtilHelper.getHolidays(year + 1));
      //DIAS ADICIONALES
      //festivos.AddRange(new List<DateTime>() { new DateTime(year, 12, 26), new DateTime(year + 1, 1, 2) });
      return Json(festivos);
    }

    [Route("MotivosRechazo/{Empresa}")]
    [HttpGet]
    public IHttpActionResult GetMotivosRechazo(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var data = db.MOTIVOS_RECHAZO.Select(e => new { e.Cod_Motivo_Rechazo, e.Descripcion}).ToList();
      return Json(data);
    }

    [Route("Conceptos/{Empresa}")]
    [HttpGet]
    public IHttpActionResult GetConceptos(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var conceptos = db.CONCEPTOS
                        .Where(c => c.Cod_Concepto != 1
                         && c.Cod_Concepto != 2
                         && c.BSBenSalario != "S"
                         && c.Devengo == "S"
                         && c.Tipo_Concepto != "P")
                         .OrderBy(c => c.Nom_Concepto)
                        .ToList();
      return Json(conceptos);
    }

    [Route("SendMailCompropago/{Empresa}/{Fec_Nomina}")]
    [HttpPost]
    public IHttpActionResult SendMailCompropago(string Empresa, string Fec_Nomina, [FromBody] List<EMPLEADOS> empleados)
    {
      JulianaContext db = new JulianaContext(Empresa);

      var Estados = new List<dynamic>();
      //new Thread(delegate () {
      foreach (var e in empleados)
      {
        var data = new CompropagoViewModel
        {
          ano = Fec_Nomina.Substring(0, 4),
          mes = Fec_Nomina.Substring(4, 2),
          quincena = Fec_Nomina.Substring(6, 2) == "29" ? "2" : "1",
          empleados = new List<EMPLEADOS>() { e },
          DBName = Empresa
        };
        var pdf = PdfController.GenerarCompropago(data);
        var estado = MailHelper.EnviarCorreoCompropago(e, pdf, Fec_Nomina);
        Estados.Add(new { Dir_Elec = e.Dir_Elec, Empleado = e.Empleado, Estado = estado });

      }
      // Variable Uso 120, Enviar confirmación envio satisfactorio correos
      var EnviarConfirmacion = db.VARIABLES.FirstOrDefault(v => v.Uso == 120);
      if (EnviarConfirmacion != null)
      {
        // usar correo de la variable
        //MailHelper.EnviarCorreoEstadoEnvios(Estados, "and7702@gmail.com");
      }
      //}).Start();
      return Ok(Estados);
    }


    /*
    [Route("Certlab/Config/{Empresa}")]
    [HttpPost]
    public IHttpActionResult SetCertlabConfig(string Empresa, [FromBody]PARAMETROS_CERTLAB data)
    {
        JulianaContext db = new JulianaContext(Empresa);
        // set Cod_Parametros_Certlab to null in frontend produce 0 in C#
        data.Filter = FILTERS[data.Filter];
        if (data.Cod_Parametros_Certlab != 0)
        {
            db.Entry(data).State = EntityState.Modified;
        }
        else
        {
            db.PARAMETROS_CERTLAB.Add(data);
        }
        db.SaveChanges();
        return Ok();

    }*/


    [Route("Certlab/Config")]
    [HttpGet]
    public IHttpActionResult GetCertlabConfig()
    {
      using (JulianaContext db = new JulianaContext())
      {
        List<PARAMETROS_CERTLAB> parametros = db.PARAMETROS_CERTLAB.ToList();
        if (parametros.Count() > 0)
        {
          return Json(new { parametros = parametros });
        }
        else
        {
          return NotFound();
        }
      }
    }



    [Route("Certlab/Config/{Empresa}/")]
    [HttpPost]
    public IHttpActionResult SetFilterCertlab(string Empresa, [FromBody] PARAMETROS_CERTLAB data)
    {

      JulianaContext db = new JulianaContext(Empresa);
      data.Filter = FILTERS[data.Filter];
      var parametros = db.PARAMETROS_CERTLAB
                          .Where(p => p.Filter == data.Filter
                          && p.Cod_Filter == data.Cod_Filter);
      PARAMETROS_CERTLAB config;
      if (parametros.Count() > 0)
      {
        config = parametros.First();
        config.Base_Salario = data.Base_Salario;
        config.Conceptos = data.Conceptos;
        config.Horas_Extras = data.Horas_Extras;
        config.Meses_Promedio = data.Meses_Promedio;
        config.Otros_Devengos = data.Otros_Devengos;
        config.Otros_Ingresos = data.Otros_Ingresos;
        config.Texto_Embajada = config.Texto_Viaje_Laboral;
      }
      else
      {
        config = data;
      }
      db.Entry(config).State = config.Cod_Parametros_Certlab == 0 ?
                             EntityState.Added :
                             EntityState.Modified;
      db.SaveChanges();
      return Ok();

    }

    [Route("Certlab/Config/{Empresa}/{filter}/{CodFilter}")]
    [HttpGet]
    public PARAMETROS_CERTLAB GetFilterCertlabConfig(string Empresa, string filter, short CodFilter)
    {

      JulianaContext db = new JulianaContext(Empresa);
      if (filter == "Default")
      {
        var defaultP = db.PARAMETROS_CERTLAB
                            .Where(p => p.Filter == "999");
        return defaultP.First();
      }
      filter = FILTERS[filter];
      var parametros = db.PARAMETROS_CERTLAB
                          .Where(p => p.Filter == filter
                          && p.Cod_Filter == CodFilter);

      if (parametros.Count() > 0)
      {
        return parametros.First();
      }
      else
      {
        parametros = db.PARAMETROS_CERTLAB
                            .Where(p => p.Filter == "999");
        return parametros.First();
      }

    }
  }
}
