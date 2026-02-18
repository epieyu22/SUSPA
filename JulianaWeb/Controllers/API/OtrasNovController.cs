using JulianaWeb.Helpers;
using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/OtrasNov")]
  public class OtrasNovController : ApiController
  {

    [Route("{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_Registros_Pendientes(string Empresa)
    {
      var db = new JulianaContext(Empresa);
      var data = db.OTRASNOV.Where(n => n.Cod_Usuario == "999").ToList();
      var novedades = new List<OtrasnovsViewModel>();
      foreach(var registro in data)
      {
        var empleado = db.EMPLEADOS.Where(e => e.Cod_Empleado == registro.Cod_Empleado).First();
        var Nom_Sucursal = db.SUCURSALES.Where(s => s.Cod_Sucursal == registro.Cod_Sucursal).First().Nom_Sucursal;
        var Nom_Ccosto = db.CCOSTOS.Where(cc => cc.Cod_Ccosto == registro.Cod_Ccosto).First().Nom_Ccosto;
        var Concepto = db.CONCEPTOS.Where(c => c.Cod_Concepto == registro.Cod_Concepto).First();
        var n = new OtrasnovsViewModel
        {
          Cod_Empleado = registro.Cod_Empleado,
          Cod_Concepto = registro.Cod_Concepto,
          Empleado = empleado.Empleado,
          Concepto = Concepto.Nom_Concepto,
          Fecha = UtilHelper.getDate(registro.Fec_Ing_Novedad).ToString("yyyy/MM/dd"),
          Val_OtrasNov = registro.Val_OtrasNov,
          Porc_OtrasNov = registro.Porc_OtrasNov.ToString(),
          Coutas = registro.Cuotas,
          Devengo = Concepto.Devengo == "S" ? "Si" : "No",
          Prioridad = registro.Prioridad,
          Documento = empleado.Cedula,
          Nom_Sucursal = Nom_Sucursal,
          Nom_Ccosto = Nom_Ccosto,
          Tipo = registro.Cuotas > 1 ? "Permanente" : "Ocacional"
        };
        novedades.Add(n);
      }
      return Json(novedades);
    }

    [Route("{Empresa}/Data")]
    [HttpGet]
    public IHttpActionResult Get_OtrasNov_Data(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var tipo_conceptos = new List<string>(){"3", "5", "7", "8", "9", "0", "V"};
      var conceptos = db.CONCEPTOS.Where(c => tipo_conceptos.Contains(c.Tipo_Concepto)).OrderBy(c => c.Nom_Concepto);
      var empleados = db.EMPLEADOS.OrderBy(e => e.Empleado).ToList();

      var LiqNomina = db.PARAMETROS.ToList().Last().LiqNomina;

      DateTime Fec_Actual = DateTime.Now;
      DateTime Fec_Activa = Fec_Actual.AddMonths(1);
      List<DateTime> Fecs_Nomina = new List<DateTime>();

      if (LiqNomina == 15 && Fec_Actual.Day <= LiqNomina)
      {
        Fecs_Nomina.Add(new DateTime(Fec_Actual.Year, Fec_Actual.Month, 15));
      }
      else
      {
        var diaNomina = Fec_Actual.Month == 2 ? DateTime.DaysInMonth(Fec_Actual.Year, Fec_Actual.Month) : 30;
        Fecs_Nomina.Add(new DateTime(Fec_Actual.Year, Fec_Actual.Month, diaNomina));
      }
      
      //if (LiqNomina == 15)
      //{
      //  if (Fec_Actual.Day < LiqNomina)
      //  {
      //    Fecs_Nomina.Add(new DateTime(Fec_Actual.Year, Fec_Actual.Month, 15));
      //  }
      //}
      //else
      //{
      //  if (Fec_Actual.Day < LiqNomina)
      //  {
      //    Fecs_Nomina.Add(
      //      new DateTime(
      //        Fec_Actual.Year,
      //        Fec_Actual.Month,
      //        Fec_Actual.Month == 2 ? DateTime.DaysInMonth(Fec_Activa.Year, Fec_Activa.Month) : 30
      //      )
      //    );
      //  }
      //}

      for (int i = 0; i < 12; i++)
      {
        if(LiqNomina == 15)
        {
          Fecs_Nomina.Add(new DateTime(Fec_Activa.Year, Fec_Activa.Month, 15));
        }
        DateTime f = new DateTime();
        if(Fec_Activa.Month == 2)
        {
          f = new DateTime(Fec_Activa.Year, Fec_Activa.Month, DateTime.DaysInMonth(Fec_Activa.Year, Fec_Activa.Month));
        }
        else
        {
          f = new DateTime(Fec_Activa.Year, Fec_Activa.Month, 30);
        }      
        Fecs_Nomina.Add(f);
        Fec_Activa = Fec_Activa.AddMonths(1);
      }     

      return Ok(new {conceptos, empleados, Fecs_Nomina, LiqNomina});
    }

    [Route("{Empresa}/upload")]
    [HttpPost]
    public IHttpActionResult UploadNovedades(string Empresa, [FromBody] List<OtrasnovsViewModel> data)
    {
      JulianaContext db = new JulianaContext(Empresa);

      foreach(OtrasnovsViewModel row in data)
      {

        int Cod_Empleado = Convert.ToInt16(row.Cod_Empleado);
        var Empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cod_Empleado == Cod_Empleado);
        if (Empleado == null)
        {
          Empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cod_Empleado.ToString() && e.Estado != "R");
        }

        // Añadir Auditoria quien lo hizo
        // AuditoriaHelper.Log()

        short Cuotas = row.Coutas;
        OTRASNOV nuevo = new OTRASNOV
        {
          Cod_Empleado = Empleado.Cod_Empleado,
          Cod_Zona = Empleado.Cod_Zona,
          Cod_Sucursal = Empleado.Cod_Sucursal,
          Cod_Ccosto = Empleado.Cod_Ccostos,
          Cod_Concepto = row.Cod_Concepto,
          Prioridad = row.Prioridad,
          Porc_OtrasNov = float.Parse(row.Porc_OtrasNov),
          Val_OtrasNov = row.Val_OtrasNov,
          Dias = 0,
          Tipo = Cuotas > 1 ? "P" : "O",
          Estado = "",
          Cuotas = 1,
          Vigencia = row.Fecha.Substring(0, 10).Replace("/", ""),
          Cod_Usuario = "999",
          Fec_Ing_Novedad = UtilHelper.getUnglyDate(DateTime.Now),
          NumCtaVoluntarios = "",
          Tipo_Especial = 1,
          VigenciaDesde = "",
          Fuente = "",
          Fec_Aprobado = "",
          Fec_Desaprobado = "",
          Aplica = ""
        };
        db.OTRASNOV.Add(nuevo);
        // Añadir Auditoria por registro
      }
      try
      {
        db.SaveChanges();
      }
      catch (DbEntityValidationException e)
      {
        return Json(e.EntityValidationErrors);
      }
      return Ok();
    }
  }
}
