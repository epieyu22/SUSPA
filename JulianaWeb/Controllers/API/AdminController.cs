using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JulianaWeb.Models;
using System.IO;
using JW3.Helpers;
using System.Diagnostics;
using System.Data.Entity.Validation;
using System.Data.Entity;
using JulianaWeb.Business.Generals;
using JulianaWeb.Resources;
using JulianaWeb.Models.Juliana.Generals;
using JulianaWeb.Business;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Admin")]
  public class AdminController : ApiController
  {   

    //Motodos para el modulo de aprobaciones.

    [Route("Aprobaciones/{Empresa}/Aprobadores")]
    [HttpGet]
    public IHttpActionResult Get_Aprobadores_Empresa(string empresa, [FromUri] int page, [FromUri] int count,
      [FromUri] string docEmpleado, [FromUri] string carIdEmpleado, [FromUri] string empleado,
      [FromUri] string docAprobador, [FromUri] string carIdAprobador, [FromUri] string aprobador)
    {
      ReportesBO reportesBO = new ReportesBO();
      return Ok(reportesBO.GetReporteAprobadores(empresa, page, count, docEmpleado, carIdEmpleado, empleado, docAprobador, carIdAprobador, aprobador));
    }


    [Route("Aprobaciones/{Empresa}/Aprobadores")]
    [HttpPost]
    public IHttpActionResult Add_Aprobadores_Empresa(string Empresa, [FromBody] AprobadoresDataVm data)
    {
      JulianaContext db = new JulianaContext(Empresa);

      var aprobadoresAEliminar = db.APROBADORES
                                    .Where(a => a.Filtro == data.Filtro && a.Cod_Filtro == data.Cod_Filtro && a.Tipo_Aprobacion == data.Tipo_Aprobacion
                                      && a.Estado == "A" && !data.Cod_Aprobadores.Any(c => c == a.Cod_Empleado))
                                    .ToList();
      foreach (var aprobador in aprobadoresAEliminar)
      {
        db.APROBADORES.Attach(aprobador);
        aprobador.Estado = "R";
        db.Entry(aprobador).State = EntityState.Modified;
        db.SaveChanges();
      }

      int i = 0;
      foreach (short Cod_Aprobador in data.Cod_Aprobadores)
      {
        var exist = db.APROBADORES.FirstOrDefault(a => a.Cod_Empleado == Cod_Aprobador && a.Nivel == "0" && a.Filtro == data.Filtro && a.Cod_Filtro == data.Cod_Filtro);
        if (exist == null)
        {
          db.APROBADORES.Add(new APROBADORES()
          {
            Cod_Empleado = Cod_Aprobador,
            Filtro = data.Filtro,
            Cod_Filtro = data.Cod_Filtro,
            Tipo_Aprobacion = data.Tipo_Aprobacion,
            Nivel = "0",
            Sub_Nivel = i.ToString(),
            Estado = "A",
          });
          db.SaveChanges();
        }
        else if (exist != null)
        {
          if (exist.Estado.Contains("R"))
          {
            db.APROBADORES.Attach(exist);
            exist.Estado = "A";
            db.Entry(exist).State = EntityState.Modified;
            db.SaveChanges();
          }
          i++;
        }
      }
      return Ok();
    }

    [Route("Aprobaciones/{Empresa}/Retirar")]
    [HttpPost]
    public IHttpActionResult Retirar_Aprobadores_Empresa(string Empresa, [FromBody] AprobadoresDataVm data)
    {
      JulianaContext db = new JulianaContext(Empresa);

      var aprobador = db.APROBADORES.FirstOrDefault(a => a.Cod_Empleado == data.Cod_Empleado && a.Cod_Filtro == data.Cod_Filtro && a.Estado == "A");

      try
      {
        if(aprobador == null)
        {
          throw new Exception("Aprobador no encontrado.");
        }
        db.APROBADORES.Attach(aprobador);
        aprobador.Estado = "R";
        db.Entry(aprobador).State = EntityState.Modified;
        db.SaveChanges();
        return Ok();
      }
      catch (DbEntityValidationException e)
      {
        return Ok(e.EntityValidationErrors);
      }
      catch(Exception)
      {
        return BadRequest("Ocurrio un error inesperado!");
      }

    }


    //Idiomas
    [Route("{Empresa}/{Cod_HojaVida}/IdiomasHojaVida/{Cod_Idioma_Hojavida}")]
    [HttpDelete]
    public IHttpActionResult DeleteIdiomasHojavida(string Empresa, short Cod_HojaVida, short Cod_Idioma_Hojavida)
    {
      var db = new JulianaContext(Empresa);
      IDIOMAS_HOJAVIDA idioma = db.IDIOMAS_HOJAVIDA.FirstOrDefault(i => i.Cod_Idioma_Hojavida == Cod_Idioma_Hojavida);
      db.IDIOMAS_HOJAVIDA.Remove(idioma);
      db.SaveChanges();
      return Ok();
    }


    [Route("Aprobaciones/{Empresa}/Config/{Tipo_Aprobacion}/{Filtro}/{Cod_Filtro}")]
    [HttpGet]
    public IHttpActionResult GetGet_Aprobacion_Config(string Empresa, string Filtro, string Tipo_Aprobacion, short Cod_Filtro)
    {
      JulianaContext db = new JulianaContext(Empresa);
     
      List<APROBADORES> aprobadores = db.APROBADORES
                              .Where(a => a.Filtro == Filtro && a.Cod_Filtro == Cod_Filtro && a.Tipo_Aprobacion == Tipo_Aprobacion)
                              .ToList();

      List<APROBADORES> aprobadoresActivos = aprobadores.Where(a => a.Estado.Contains('A')).ToList();
      return Ok(aprobadoresActivos);
    }


    /*
      * Devulve todos los registros de la tablas usadas para hacer los Filtros
      * en la configuración del certificado laboral y los aprobadores.
      * 
      * Una vez seleccionada la base de datos, 
      * busca por estos datos para aguilizar el proceso y no hacer una petición 
      * cada vez que se cambie el filtro.
      */
    [Route("FilterTables/{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_Filter_Tables_Data(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var data = new
      {
        zonas = db.ZONAS.ToList(),
        sucursales = db.SUCURSALES.ToList(),
        ccostos = db.CCOSTOS.ToList(),
        deptos = db.DEPTOS.ToList(),
        cargos = db.CARGOS.ToList(),
        empleados = db.EMPLEADOS.Where(e => e.Estado != "R").ToList(),
        terceros = db.TERCEROS.Where(t => t.Tipo_Tercero == "8").ToList()
      };
      return Ok(data);
    }


    [Route("ParametrosWeb/{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_Parametros_Web(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      PARAMETROS_WEB data = db.PARAMETROS_WEB.ToList().Last();
      return Ok(data);
    }


    [Route("Conceptos/Incapacidades/{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_Conceptos_Incapacidades(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      List<CONCEPTOS> data = db.CONCEPTOS.Where(c => c.Tipo_Concepto == "I").ToList();
      return Ok(data);
    }

    [Route("Search/Certlab/{Cod_verificacion}")]
    [HttpGet]
    public IHttpActionResult SearchCertlab(string Cod_verificacion)
    {
      string searchpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/CertificadosBK/");
      var getFileName = Directory.GetFiles(searchpath);  // Collection Of file name with extention.

      var getFileNameExcludeSomeExtension1 = from f in Directory.GetFiles(searchpath)
                                              where Path.GetExtension(f) != ".scc" && Path.GetExtension(f) != ".db"
                                              && Path.GetFileNameWithoutExtension(f) == Cod_verificacion
                                              select Path.GetFileNameWithoutExtension(f);

      return Json(getFileNameExcludeSomeExtension1);
    }


    [Route("Solicitud_Certificado_Laboral/{Empresa}/{Cedula}")]
    [HttpPost]
    public IHttpActionResult Solicitud_Certificado_Laboral(string Empresa,  string Cedula, [FromBody] Soliciud_Certtificado_Laboral_VM data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      EMPLEADOS Empleado = db.EMPLEADOS.Where(e => e.Estado != "R").ToList().First();


      MailHelper.Solicitud_Certificado_Laboral(Empleado, data.mensaje);
      return Ok();
    }

    [HttpGet]
    [Route("version")]
    public PARAMETROS_GENERALES GetVersion()
    {
      ParameterGetter getter = new ParameterGetter();

      return getter.Get(Parameters.Version);
    }
  }
}
