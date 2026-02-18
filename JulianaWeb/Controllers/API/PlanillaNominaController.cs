using JulianaWeb.Helpers;
using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/PlanillaNomina")]
  public class PlanillaNominaController : ApiController
  {



    [Route("{Empresa}/FecsIngNovedad/{Fec_Nomina}")]
    [HttpGet]
    public IHttpActionResult Get_Fecs_Ing_Novedad(string Empresa, string Fec_Nomina)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var Fechas = (from h in db.HISTORICO
                    join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                    where h.Fec_Nomina == Fec_Nomina
                    && c.Tipo_Concepto != "6"
                    && c.Tipo_Concepto != "9"
                    select h.Fec_Ing_Novedad).Distinct();



      var FecsIngNovedad = new List<DateTime>();
      foreach (var f in Fechas)
      {
        FecsIngNovedad.Add(UtilHelper.getDate(f));
      }
      return Ok(FecsIngNovedad);
    }

    [Route("Datos/{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_Datos_Plantilla_Nomina(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var fecs = db.HISTORICO.OrderByDescending(h => h.Fec_Nomina).Select(h => h.Fec_Nomina).Distinct();
      var Fecs_Nomina = new List<DateTime>();
      foreach (var f in fecs)
      {
        Fecs_Nomina.Add(UtilHelper.getDate(f));
      }
      Fecs_Nomina.OrderByDescending(f => f);

      var Prenomina_Disponible = db.NOVEDADES.Count() > 0 ? true : false;
      return Json(new { Fecs_Nomina, Prenomina_Disponible });
    }

    [Route("{Empresa}/Prenomina/")]
    [HttpGet]
    public IHttpActionResult Prenomina(string Empresa)
    {
      var db = new JulianaContext(Empresa);

      var historico = from h in db.NOVEDADES
                      join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                      where h.Estado == "A"
                          && c.Tipo_Concepto != "6"
                          && c.Tipo_Concepto != "9"
                      select new
                      {
                        h.Cod_Empleado,
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
                      };

      List<PlanillaNomiinaViewModel> data = new List<PlanillaNomiinaViewModel>();

      // Para distinguir cada empleado al cual fue realizado un pago en esa fecha
      var cod_empleados = historico.Select(e => e.Cod_Empleado).Distinct().ToList();
      foreach (var Cod_Empleado in cod_empleados)
      {
        PlanillaNomiinaViewModel dvm = new PlanillaNomiinaViewModel();
        var empleado = (
          from e in db.EMPLEADOS
          where e.Cod_Empleado == Cod_Empleado
          select new { e.Empleado, e.Cedula, e.Salario, e.Cod_Cargo, e.Cod_Sucursal, e.Cod_Ccostos }
        ).Single();

        dvm.Empleado = empleado.Empleado.Trim();
        dvm.Num_Documento = empleado.Cedula.Trim();
        dvm.Salario = empleado.Salario;
        dvm.cargo = db.CARGOS.First(x => x.Cod_Cargo == empleado.Cod_Cargo).Nom_Cargo.Trim();
        dvm.Ccosto = db.CCOSTOS.First(cc => cc.Cod_Ccosto == empleado.Cod_Ccostos).Nom_Ccosto.Trim();
        dvm.Sucursal = db.SUCURSALES.First(s => s.Cod_Sucursal == empleado.Cod_Sucursal).Nom_Sucursal.Trim();

        var pagos = historico.Where(h => h.Cod_Empleado == Cod_Empleado);
        foreach (var p in pagos)
        {
          if (p.Devengo == "S")
          {
            dvm.Total_Devengos += p.Val_Novedad;
          }
          else
          {
            dvm.Total_Deducidos += p.Val_Novedad;
          }
          if (p.Cod_Concepto == 1)
          {
            continue;
          }
          if (p.Cod_Concepto == 3)
          {
            dvm.AuxTransporte = p.Val_Novedad;
            continue;
          }
          if (p.Cod_Concepto == 5)
          {
            dvm.Retefuente = p.Val_Novedad;
            continue;
          }
          if (p.Cod_Concepto == 6)
          {
            dvm.ApoSalud = p.Val_Novedad;
            continue;
          }
          if (p.Cod_Concepto == 7)
          {
            dvm.ApoPension = p.Val_Novedad;
            continue;
          }
          if (p.Cod_Concepto == 11)
          {
            dvm.ApoSolPension = p.Val_Novedad;
            continue;
          }

          // Vacciones
          if (p.Cod_Concepto == 9)
          {
            dvm.ApoSolPension = p.Val_Novedad;
            continue;
          }
          if (p.Cod_Concepto == 10)
          {
            dvm.ApoSolPension = p.Val_Novedad;
            continue;
          }

          // Otros devengos y deducidos
          if (p.Devengo == "S")
          {
            dvm.Otros_Devengos += p.Val_Novedad;
          }
          else
          {
            dvm.Otros_Deducidos += p.Val_Novedad;
          }
        }
        data.Add(dvm);
      }
      data = data.OrderBy(d => d.Empleado).ToList();
      return Ok(data);
      return Ok();
    }



    [Route("{Empresa}/{Fec_Nomina}/{Fec_Ing_Novedad}")]
    [HttpGet]
    public IHttpActionResult GetPlanillaNomina(string Empresa, string Fec_Nomina, string Fec_Ing_Novedad)
    {
      var db = new JulianaContext(Empresa);

      var historico = from h in db.HISTORICO
                      join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                      where
                          h.Fec_Nomina == Fec_Nomina
                          && h.Estado == "P"
                          && c.Tipo_Concepto != "9"
                      select new
                      {
                        h.Cod_Empleado,
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
                      };
      if (Fec_Ing_Novedad != "all")
      {
        historico = from h in db.HISTORICO
                    join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                    where
                          h.Fec_Nomina == Fec_Nomina
                          && h.Fec_Ing_Novedad == Fec_Ing_Novedad
                          && h.Estado == "P"
                          && c.Tipo_Concepto != "9"
                    select new
                    {
                      h.Cod_Empleado,
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
                    };
      }





      List<PlanillaNomiinaViewModel> data = new List<PlanillaNomiinaViewModel>();

      // Para distinguir cada empleado al cual fue realizado un pago en esa fecha
      var cod_empleados = historico.Select(e => e.Cod_Empleado).Distinct().ToList();
      foreach (var Cod_Empleado in cod_empleados)
      {
        PlanillaNomiinaViewModel datosEmpleado = new PlanillaNomiinaViewModel();
        //var empleado = (
        //  from e in db.EMPLEADOS
        //  where e.Cod_Empleado == Cod_Empleado
        //  select new { e.Empleado, e.Cedula, e.Salario, e.Cod_Cargo, e.Cod_Sucursal, e.Cod_Ccostos }
        //).Single();

        var empleado = db.EMPLEADOS.Select(e => new { e.Cod_Empleado, e.Empleado, e.Cedula, e.Salario, e.Cod_Cargo, e.Cod_Sucursal, e.Cod_Ccostos }).First(r => r.Cod_Empleado == Cod_Empleado);

        datosEmpleado.Empleado = empleado.Empleado.Trim();
        datosEmpleado.Num_Documento = empleado.Cedula.Trim();

        datosEmpleado.cargo = db.CARGOS.First(x => x.Cod_Cargo == empleado.Cod_Cargo).Nom_Cargo.Trim();
        datosEmpleado.Ccosto = db.CCOSTOS.First(cc => cc.Cod_Ccosto == empleado.Cod_Ccostos).Nom_Ccosto.Trim();
        datosEmpleado.Sucursal = db.SUCURSALES.First(s => s.Cod_Sucursal == empleado.Cod_Sucursal).Nom_Sucursal.Trim();

        datosEmpleado.Vacaciones = 0;
        datosEmpleado.Cesantias = 0;
        datosEmpleado.Intereses_Cesantias = 0;
        datosEmpleado.Comisiones = 0;
        datosEmpleado.Dias_Trabajados = 0;
        datosEmpleado.Dias_Vacaciones = 0;

        var Mostrar_Cesantias = false;
        var Mostrar_Int_Cesantias = false;

        var pagos = historico.Where(h => h.Cod_Empleado == Cod_Empleado);
        foreach (var p in pagos)
        {
          if (p.Devengo != "N")
          {
            datosEmpleado.Total_Devengos += p.Val_Novedad;
          }
          else
          {
            datosEmpleado.Total_Deducidos += p.Val_Novedad;
          }

          switch (p.Cod_Concepto)
          {
            // Salario
            case 1:
              datosEmpleado.Salario = Math.Round(p.Val_Novedad);
              datosEmpleado.Dias_Trabajados = p.Dias_Novedad;
              datosEmpleado.Dias_Vacaciones += 30 - datosEmpleado.Dias_Trabajados;
              break;
            case 2:
              datosEmpleado.Salario = p.Val_Novedad;
              datosEmpleado.Dias_Trabajados = p.Dias_Novedad;
              datosEmpleado.Dias_Vacaciones += 30 - datosEmpleado.Dias_Trabajados;
              break;
            // CUOTA DE SOSTENIMIENTO
            case 95:
              datosEmpleado.Salario = Math.Round(p.Val_Novedad);
              datosEmpleado.Dias_Trabajados = p.Dias_Novedad;
              datosEmpleado.Dias_Vacaciones += 30 - datosEmpleado.Dias_Trabajados;
              break;
            // Aux Transporte
            case 3:
              datosEmpleado.AuxTransporte = Math.Round(p.Val_Novedad);
              break;
            //Retefuente
            case 5:
              datosEmpleado.Retefuente = Math.Round(p.Val_Novedad);
              break;
            // Apo Salud
            case 6:
              datosEmpleado.ApoSalud = Math.Round(p.Val_Novedad);
              break;
            // Apo Pension 
            case 7:
              datosEmpleado.ApoPension = Math.Round(p.Val_Novedad);
              break;
            // Apo Sol Pension
            case 11:
              datosEmpleado.ApoSolPension = Math.Round(p.Val_Novedad);
              break;
            // comisiones
            case 14:
              datosEmpleado.Comisiones = Math.Round(p.Val_Novedad);
              break;
            // Vacciones
            case 9:
              datosEmpleado.Vacaciones += Math.Round(p.Val_Novedad);
              break;
            case 10:
              datosEmpleado.Vacaciones += Math.Round(p.Val_Novedad);
              break;
            case 100:
              datosEmpleado.Vacaciones += Math.Round(p.Val_Novedad);
              break;
            // Cesantias
            case 20:
              datosEmpleado.Cesantias += Math.Round(p.Val_Novedad);
              Mostrar_Cesantias = true;
              break;
            case 23:
              datosEmpleado.Cesantias += Math.Round(p.Val_Novedad);
              Mostrar_Cesantias = true;
              break;
            case 98:
              datosEmpleado.Cesantias = Math.Round(p.Val_Novedad);
              Mostrar_Cesantias = true;
              break;
            // interesas de cesantias
            case 21:
              datosEmpleado.Intereses_Cesantias += Math.Round(p.Val_Novedad);
              Mostrar_Int_Cesantias = true;
              break;
            case 97:
              datosEmpleado.Intereses_Cesantias += Math.Round(p.Val_Novedad);
              Mostrar_Int_Cesantias = true;
              break;
            default:
              // Otros devengos y deducidos
              if (p.Devengo != "N")
              {
                datosEmpleado.Otros_Devengos += Math.Round(p.Val_Novedad);
              }
              else
              {
                datosEmpleado.Otros_Deducidos += Math.Round(p.Val_Novedad);
              }
              break;
          }




        }
        datosEmpleado.Total_General = datosEmpleado.Total_Devengos - datosEmpleado.Total_Deducidos;

        data.Add(datosEmpleado);
      }
      data = data.OrderBy(d => d.Empleado).ToList();
      return Ok(data);
    }



  }


}
