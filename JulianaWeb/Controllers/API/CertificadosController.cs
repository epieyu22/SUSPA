using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Certificados")]
  public class CertificadosController : ApiController
  {
    string FILTER_EMPLEADO = "1";
    string FILTER_CARGO = "2";
    string FILTER_DEPTO = "3";
    string FILTER_CCOSTO = "4";
    string FILTER_SUCURSAL = "5";
    string FILTER_ZONA = "6";
    string FILTER_DEFAULT = "999";

    [Route("Certlaboral/{Empresa}/{tipo}/Plantillas")]
    [HttpGet]
    public IHttpActionResult GetPlantillas(string Empresa, string tipo)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var plantillas = db.PLANTILLAS_CERTIFICADOS.Where(x => x.Tipo_plantilla == "N").ToList();

      if (tipo == "otroscert")
      {
        var plantillasotras = db.PLANTILLAS_CERTIFICADOS.Where(x => x.Tipo_plantilla == "O").ToList();

        plantillas = plantillasotras;
      }

      if (tipo == "politicas")
      {
        var plantillasotras = db.PLANTILLAS_CERTIFICADOS.Where(x => x.Tipo_plantilla == "P").ToList();

        plantillas = plantillasotras;
      }

      return Ok(new { plantillas });
    }

    [Route("Certlaboral/{Empresa}/{Cod_Empleado}/{Cedula}/Config")]
    [HttpGet]
    public IHttpActionResult GetConfigEmpleado(string Empresa, short Cod_Empleado, int cedula)
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
  }
}
