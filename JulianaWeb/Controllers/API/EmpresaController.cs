using JulianaWeb.Helpers;
using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Empresas")]
  public class EmpresaController : ApiController
  {
    [Route("")]
    [HttpGet]
    public IHttpActionResult Get()
    {
      using(JulianaContext db = new JulianaContext())
      {
        var empresas = db.EMPRESAS.Where(e => e.Estado == "A").ToList();
        return Json(empresas);
      }
    }

    [Route("Usuarios/{Cedula}")]
    [HttpGet]
    public IHttpActionResult GetEmpresasUsuarios(string Cedula)
    {
      using (JulianaContext db = new JulianaContext())
      {
          var emps = db.EMPRESAS.Where(e => e.Estado == "A").ToList();
          return Ok(emps);
      }
    }

    [Route("{Empresa}/Empleados")]
    [HttpGet]
    public List<EMPLEADOS> GetEmpleados(string Empresa)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {
        return db.EMPLEADOS.Where(e => e.Estado != "R").OrderBy(e => e.Empleado).ToList();
      }
    }

    [Route("{Empresa}/Empleados/Retirados")]
    [HttpGet]
    public List<EMPLEADOS> GetEmpleadosActivos(string Empresa)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {
        return db.EMPLEADOS.Where(e => e.Estado == "R").OrderBy(e => e.Empleado).ToList();
      }
    }

    [Route("{Empresa}/Empleados/Retefuente/{ano}")]
    [HttpGet]
    public IHttpActionResult GetEmpleadosRetefuente(string Empresa, string ano)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {
        var cod_empleados =
            db.HISTORICO
            .Where(h => h.Fec_Nomina.Substring(0, 4) == ano && !(h.Estado=="C"))
            .Select(h => h.Cod_Empleado)
            .Distinct();
        var empleados = db.EMPLEADOS
                          .Where(e => cod_empleados.Contains(e.Cod_Empleado) && !(e.Estado == "R" && e.Fec_Retiro == ""))
                          .OrderBy(e => e.Empleado)
                          .ToList();                
        return Json(new {  empleados });
      }
    }

    [Route("{Empresa}/Empleados/{Cedula}/Retefuente/{ano}")]
    [HttpGet]
    public IHttpActionResult GetEmpleadoByCedulaRetefuente(string Empresa, string Cedula, string ano)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {
        var allContratos = 
            db.EMPLEADOS
                .Where(e => e.Cedula == Cedula)
                .Select(e =>e.Cod_Empleado )
                .ToList();
        var cod_empleados =
            db.HISTORICO
                .Where(h => h.Fec_Nomina.Substring(0, 4) == ano && allContratos.Contains(h.Cod_Empleado))
                .Select(h => h.Cod_Empleado)
                .Distinct();
        var empleados = db.EMPLEADOS.Where(e => cod_empleados.Contains(e.Cod_Empleado)).OrderBy(c=> c.Estado).ToList();
        return Json(new { empleados = empleados });
      }
    }

    [Route("{Empresa}/Empleados/compropago/{ano}/{mes}/{quincena}")]
    [HttpGet]
    public IHttpActionResult GetEmpleadosCompropago(string Empresa, string ano, string mes, int quincena)
    {
      int day;
      if (quincena == 1)
      {
        day = 15;
      }
      else if (mes == "02")
      {
        if (Int32.Parse(ano) % 4 == 0)
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

      DateTime fechaNominaActual = UtilHelper.getDate(ano + mes + day.ToString());
      string dia = day > 9 ? day.ToString() : "0" + day;
      string fechaNomina = ano + mes + dia;
      var db = new JulianaContext(Empresa);
      var empleados = from h in db.HISTORICO
                      join e in db.EMPLEADOS on h.Cod_Empleado equals e.Cod_Empleado
                      where h.Fec_Nomina == fechaNomina
                      orderby e.Empleado
                      select e;
      empleados = empleados.Distinct();
      return Json(empleados.ToList());
    }

    [Route("{Empresa}/Empleados/{cedula}")]
    [HttpGet]
    public IHttpActionResult GetEmpleadoByCedula(string Empresa, string cedula)
    {      
      using (var db = new JulianaContext(Empresa))
      {
        string Cedula = "";
        var queryalias = db.USUARIOS_WEB.FirstOrDefault(u => u.Clave == cedula);
        if (queryalias != null)
        {
          Cedula = queryalias.Usuario.Trim();
        }
        else
        {
          Cedula = cedula;
        }

        EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cedula && e.Estado != "R");
        if(empleado != null)
        {
          return Json(new { empleado = empleado });
        }

        TERCEROS tercero = db.TERCEROS.FirstOrDefault(e => e.Documento.Trim() == Cedula);
        if (tercero != null)
        {
          return Json(new { empleado = new EMPLEADOS
          {
            Empleado = tercero.Tercero,
            Cedula = tercero.Documento,
            Dir_Elec = tercero.Dir_Elec
          }});
        }

        return NotFound();
      }
    }

    [Route("{Empresa}/Terceros/{cedula}")]
    [HttpGet]
    public IHttpActionResult GetTerceroByCedula(string Empresa, string cedula)
    {
      using (var db = new JulianaContext(Empresa))
      {
        string Cedula = "";
        var queryalias = db.USUARIOS_WEB.FirstOrDefault(u => u.Clave == cedula);
        if (queryalias != null)
        {
          Cedula = queryalias.Usuario.Trim();
        }
        else
        {
          Cedula = cedula;
        }

        TERCEROS tercero = db.TERCEROS.FirstOrDefault(e => e.Documento.Trim() == Cedula);
        if (tercero != null)
        {
          return Json(new { tercero = tercero });
        }

        return NotFound();
      }
    }

    [Route("{Empresa}/Cargos")]
    [HttpGet]
    public List<CARGOS> GetCargos(string Empresa)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {
        return db.CARGOS.OrderBy(c => c.Nom_Cargo).ToList();
      }
    }

    [Route("{Empresa}/Deptos")]
    [HttpGet]
    public List<DEPTOS> GetDeptos(string Empresa)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {
        return db.DEPTOS.OrderBy(d => d.Nombre_Depto).ToList();
      }
    }

    [Route("{Empresa}/Ccostos")]
    [HttpGet]
    public List<CCOSTOS> GetCcostos(string Empresa)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {
        return db.CCOSTOS.OrderBy(c => c.Nom_Ccosto).ToList();
      }
    }

    [Route("{Empresa}/Sucursales")]
    [HttpGet]
    public List<SUCURSALES> GetSucursales(string Empresa)
    {
        using (JulianaContext db = new JulianaContext(Empresa))
        {
            return db.SUCURSALES.OrderBy(s => s.Nom_Sucursal).ToList();                
        }
    }

    [Route("{Empresa}/Zonas")]
    [HttpGet]
    public List<ZONAS> GetZonas(string Empresa)
    {
        using (JulianaContext db = new JulianaContext(Empresa))
        {
              return db.ZONAS.OrderBy(z => z.Nom_Zona).ToList();
        }
    }

    [Route("{Empresa}/Aprobadores/{cedula}")]
    [HttpGet]
    public IHttpActionResult GetAprobadorByCedula(string Empresa, string cedula)
    {
      using (var db = new JulianaContext(Empresa))
      {
        string Cedula = "";
        var queryalias = db.USUARIOS_WEB.FirstOrDefault(u => u.Clave == cedula);
        if (queryalias != null)
        {
          Cedula = queryalias.Usuario.Trim();
        }
        else
        {
          Cedula = cedula;
        }

        var aprobador = (from A in db.APROBADORES
                     join T in db.TERCEROS on A.Cod_Empleado equals T.Cod_Tercero
                     where T.Documento == Cedula && A.Cod_Filtro == 999
                         select new
                     {
                       Cod_Empleado = A.Cod_Empleado,
                       Cod_Filtro = A.Cod_Filtro,
                       Filtro = A.Filtro
                     }).FirstOrDefault();

        if (aprobador != null)
        {
          return Json(new { aprobador = aprobador });
        }
        else
        {
          return NotFound();
        }
      }
    }

  }
}
