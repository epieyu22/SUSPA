using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulianaWeb.Controllers
{
  [RoutePrefix("API/SolPersonal")]
  public class SolPersonalController : ApiController
  {

    [Route("{Empresa}/Data")]
    public IHttpActionResult Get_Data_SolPersonal(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var cargos = db.CARGOS.ToList();
      var ccostos = db.CCOSTOS.ToList();
      var zonas = db.ZONAS.ToList();
      var deptos = db.DEPTOS.ToList();
      var sucursal = db.SUCURSALES.ToList();
      return Json(new { cargos, ccostos, zonas, deptos, sucursal });
    }
  }
}
