using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JulianaWeb.Models;
using JulianaWeb.Helpers;
using JW3.Helpers;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Empleados")]
  public class EmpleadosController : ApiController
  {
    [Route("{Empresa}/{Cod_Empleado}/Fondos")]
    [HttpGet]
    public IHttpActionResult GetFondos(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      EMPLEADOS empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);
      AFP afc = db.AFP.SingleOrDefault(a => a.Cod_Afp == empleado.Cod_Afc);
      AFP afp = db.AFP.FirstOrDefault(a => a.Cod_Afp == empleado.Cod_Afp);
      ARP arp = db.ARP.SingleOrDefault(a => a.Cod_Arp == empleado.Cod_Arp);
      EPS eps = db.EPS.SingleOrDefault(e => e.Cod_Eps == empleado.Cod_Eps);
      CAJASCOMP cajascomp = db.CAJASCOMP.SingleOrDefault(c => c.Codigo ==  empleado.Cod_CajaCompensacion);

      var fecPago = db.HISTORICO_AUTOLIQUIDACIONES.Where(h => h.Cod_Empleado == empleado.Cod_Empleado).Max(h => h.Fec_Nomina);
      var aportesAMostrar = new short[] { 6, 7, 11, 46 };
      var aportes = from h in db.HISTORICO_AUTOLIQUIDACIONES
                    join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                    where
                      h.Fec_Nomina == fecPago
                      && h.Cod_Empleado ==empleado.Cod_Empleado
                      && aportesAMostrar.Contains(c.Cod_Concepto)
                    orderby h.Fec_Nomina, h.Cod_Concepto
                    select new CompropagoDataModel3
                    {
                        Cod_Concepto = h.Cod_Concepto,
                        Nom_Concepto = c.Nom_Concepto,
                        Val_IBC = h.Val_IBC,
                        Dias_Novedad = h.Dias_Novedad,
                        Val_Novedad = h.Val_Novedad,
                        Devengo = c.Devengo
                    };


        return Json(new {afp, arp, afc, eps, cajascomp, fecPago, aportes });
    }
    
    /*
    [Route("{Empresa}/{CodEmpleado}/Vacaciones")]
    [HttpGet]
    public IHttpActionResult GetVacaciones(string Empresa, short CodEmpleado)
    {
        using (JulianaContext db = new JulianaContext(Empresa))
        {
            EMPLEADOS empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == CodEmpleado);
            var vacaciones = db.VACACIONES.Where(v => v.Cod_Empleado == CodEmpleado).OrderBy(c => c.Desde);
            List<VACACIONES> causadas = vacaciones.Where(v => v.SubPeriodo == 0).ToList();
            List<VACACIONES> tomadas = vacaciones.Where(v => v.SubPeriodo != 0).ToList();
            var solicitudes = db.SOLICITUD_VACACIONES.Where(s => s.Cod_Empleado == empleado.Cod_Empleado && s.Estado == "P").ToList();
            var qAprobadores = from a in db.APROBADORES
                               where a.Cod_Depto == empleado.Cod_Depto
                               || a.Cod_Ccosto == empleado.Cod_Ccostos
                               || a.Cod_Sucursal == empleado.Cod_Sucursal
                               || a.Cod_Zona == empleado.Cod_Zona
                               select a.Cod_Empleado;

            var codsAprobadores = qAprobadores.ToList();
            var aprobadores = from e in db.EMPLEADOS
                              where codsAprobadores.Contains(e.Cod_Empleado)
                              select new
                              {
                                  e.Cod_Empleado, e.Empleado, e.Dir_Elec
                              };

            float diasCausados = 0;
            float diasTomados = 0;
            float diasPendientes = 0;
            causadas.ForEach(v => diasCausados += v.Dias_Disponibles);
            solicitudes.ForEach(s => diasPendientes += s.Cantidad_Dias);
            foreach (VACACIONES v in tomadas)
            {
                if (v.Dias_Tiempo > 0)
                {
                    diasTomados += v.Dias_Tiempo;
                }
                else
                {
                    diasTomados += v.Dias_Dinero;
                }
            }
            float diasDisponibles = diasCausados - diasTomados - diasPendientes;

            var data = new
            {
                causadas = causadas,
                tomadas = tomadas,
                diasCausados = diasCausados,
                diasTomados = diasTomados,
                diasDisponibles = diasDisponibles,
                solicitudes = solicitudes.ToList(),
                aprobadores = aprobadores.ToList()
            };

            return Json(data);
        }
    }
    */

    [Route("alertablockleav/{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult AlertaBlockLeave(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var blockleave = db.VARIABLES.FirstOrDefault(v => v.Uso == 101);
      if (blockleave == null) return Json("no variable");

      var empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cod_Empleado == Cod_Empleado);
      if (empleado == null) return Json("No empleado");
      var anos = UtilHelper.GetYearsAhead(UtilHelper.getDate(empleado.Fec_Ingreso));
      if (anos < 1) return Json("año menor");

      // TODO: Investigar si el block leave es por año fiscal o por periodo.
      string FecBuscarBlockLeave = "";
      FecBuscarBlockLeave = (DateTime.Now.Year - 1).ToString() + empleado.Fec_Ingreso.Substring(4, 4);
      var cc = db.VACACIONES
                 .Where(v => v.Desde.CompareTo(FecBuscarBlockLeave) >= 0 && v.Cod_Empleado == Cod_Empleado && v.SubPeriodo != 0 && v.Util_A == "S");
      //return Json(cc);
     //if (empleado.Fec_Ingreso.CompareTo(DateTime.Now.ToString("yyyyMMdd")) > 0)
     // {
     //   return Json(false);
     // }
      var vacaciones = db.VACACIONES.Where(v => v.Cod_Empleado == Cod_Empleado);
      var causados = vacaciones.Where(v => v.SubPeriodo == 0).Sum(v => v.Dias_Disponibles);
      var ddinero = vacaciones.Where(v => v.SubPeriodo != 0).Sum(v => v.Dias_Dinero);
      var dtiempo = vacaciones.Where(v => v.SubPeriodo != 0).Sum(v => v.Dias_Tiempo);
      var tomados = ddinero + dtiempo;
      var disponibles = causados - tomados;
      return Json(new {causados, tomados, disponibles });
    }

    [Route("{Empresa}/Alertas/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult Alertas(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cod_Empleado == Cod_Empleado);
      if (empleado == null) return BadRequest("No se ha Encontrado al Empleado");
      var alertaBlockLeave = !BlocLeaveHelper.HasBlockLeave(db, empleado);
      var alertaVacacionesVencidads = false;
      //MailHelper.EnviarAlertaBlockLeave(empleado);
      return Ok(new { alertaBlockLeave, alertaVacacionesVencidads });
    }
    
  }
}
