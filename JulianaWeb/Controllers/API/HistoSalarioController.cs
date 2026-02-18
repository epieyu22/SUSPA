using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JulianaWeb.Models;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/HistoSalario")]
  public class HistoSalarioController : ApiController
  {
    [Route("{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult GetHistoSalario(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var historicos = db.SALARIOS.Where(s => s.Cod_Empleado == Cod_Empleado).OrderByDescending(s => s.Fec_Salario);
      double salarioActual = historicos.First().Salario;
      string ultimaModificacion = historicos.First().Fec_Salario;

      var fec = DateTime.Now;
      var FecInicio = new DateTime(fec.Year, fec.Month, 1).ToString("yyyyMMdd");
      var FecFinal = new DateTime(fec.Year, fec.Month, DateTime.DaysInMonth(fec.Year, fec.Month)).ToString("yyyyMMdd");
      var beneficios = from otn in db.OTRASNOV
                       join c in db.CONCEPTOS on otn.Cod_Concepto equals c.Cod_Concepto
                       where c.BSBenSalario == "S"
                            && c.Devengo == "S"
                            && otn.Tipo == "P"
                            && otn.Estado == "A"
                            && otn.Cod_Empleado == Cod_Empleado
                            && otn.Vigencia.CompareTo(FecInicio) >= 0
                   
                        select new
                        {
                            Valor = otn.Val_OtrasNov,
                            Concepto = c.Nom_Concepto
                        };

      double totalBeneficios = 0;
      if(beneficios.Count() > 0)
      {
        foreach (var b in beneficios)
        {
          totalBeneficios += b.Valor;
        }
      }

      foreach (var s in historicos)
      {
        switch (s.Mot_Cambio_Sal)
        {
          case "N":
              s.Mot_Cambio_Sal = "Salario inicial";
              break;
          case "A":
              s.Mot_Cambio_Sal = "Aumento";
              break;
          case "I":
              s.Mot_Cambio_Sal = "Incremento de Ley";
              break;
          case "R":
              s.Mot_Cambio_Sal = "Reclasificación";
              break;
          case "P":
              s.Mot_Cambio_Sal = "Cab. Salario Integral";
              break;
        };
      }

      return Json(new {
          historicos,
          salarioActual,
          ultimaModificacion,
          totalBeneficios,
          beneficios, FecInicio, FecFinal
      });
    }
  }
}
