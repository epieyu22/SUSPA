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
  [RoutePrefix("API/SolPersonal")]
  public class RequerimientosController : ApiController
  {
    [Route("{Empresa}/Ingresar")]
    [HttpPost]
    public IHttpActionResult GuardarRequerimiento(string Empresa, [FromBody] SolPersonalVM data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      db.REQUERIMIENTOS.Add(new REQUERIMIENTOS
      {
        Cod_Usuario = 999,
        Fecha_Req = UtilHelper.getUnglyDate(DateTime.Now),
        Hora_Req = DateTime.Now.ToString("hh:mm"),
        Cod_Motivo = data.Cod_Motivo,
        Cod_Cargo = data.Cod_Cargo,
        Cod_Perfil = 0,
        Fecha_Ing_Aprox = null,
        Puntaje1 = 0,
        Puntaje2 = 0,
        Cod_Zona = data.Cod_Zona,
        Cod_Sucursal = data.Cod_Sucursal,
        Cod_Depto = data.Cod_Depto,
        Descripcion = data.Descripcion,
        Cod_Solicitante = data.Cod_Solicitante
      });
      db.SaveChanges();
      return Ok();
    }
  }
}
