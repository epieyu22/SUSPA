using JulianaWeb.Filters.Auth;
using JulianaWeb.Business;
using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JulianaWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [JulianaGenericAuthFilter]
  [RoutePrefix("API/Vacaciones")]
  public class VacacionesController : ApiController
  {

    string FILTER_EMPLEADO = "1";
    string FILTER_CARGO = "2";
    string FILTER_DEPTO = "3";
    string FILTER_CCOSTO = "4";
    string FILTER_SUCURSAL = "5";
    string FILTER_ZONA = "6";
    string FILTER_GENERAL = "9";

    //Constantes
    const int DIAS_LEY_BNP = 15;
    const int DIAS_BENEFICIO_BNP = 5;

    //Bussiness Object
    private readonly ReportesBO reportesBO = new ReportesBO();

    //Diccionario de estados.
    Dictionary<string, string> estadoDisplayName = new Dictionary<string, string>() {
      { "A",  "Aprobada" },
      { "A0",  "Aprobada Nivel 1" },
      { "A1",  "Aprobada Nivel 2" },
      { "A2",  "Aprobada Nivel 3" },
      { "A3",  "Aprobada Nivel 4" },
      { "A4",  "Aprobada Nivel 5" },
      { "A5",  "Aprobada Nivel 6" },
      { "A6",  "Aprobada Nivel 7" },
      { "A7",  "Aprobada Nivel 8" },
      { "A8",  "Aprobada Nivel 9" },
      { "A9",  "Aprobada Nivel 10" },
      { "AP", "Pagada" },
      { "P", "Pendiente Aprobación" },
      { "PR", "Pendiente Desaprobación" },
      { "R", "Rechazada" },
      { "D", "Eliminada" },
    };

    ///////////////////////
    //OBTENER VACACIONES//
    /////////////////////
    [JulianaGenericAuthFilter(IsOpen = true)]
    [Route("{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult GetVacacionesEmpleado(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      EMPLEADOS empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);
      
      var vacaciones = db.VACACIONES.Where(v => v.Cod_Empleado == Cod_Empleado).OrderByDescending(c => c.Desde);
      List<VACACIONES> causadas = vacaciones.Where(v => v.SubPeriodo == 0).OrderBy(v => v.Periodo).ToList();
      List<VACACIONES> tomadas = vacaciones.Where(v => v.SubPeriodo != 0).OrderBy(t => t.Desde).ToList();

      
      var solicitudes = db.SOLICITUDES
                          .Where(s => s.Cod_Empleado == empleado.Cod_Empleado && s.Tipo_Solicitud == "V")
                          .OrderByDescending(s => s.Fec_Solicitud);
      if (solicitudes != null) {
        foreach (SOLICITUDES s in solicitudes)
        {
          s.Modo_Vacaciones = s.Modo_Vacaciones == "T" ? "Tiempo" : s.Modo_Vacaciones;
        }
      }
      

      var estadosExcluidos = new string[] { "A", "AP", "D", "R" };
      var pendientes = solicitudes.Where(s => !estadosExcluidos.Contains(s.Estado));
      var aprobadas = solicitudes.Where(s => s.Estado == "A" || s.Estado == "PR");
      var Sabado = empleado.Sabado;

      var codsAprobadoresTerceros = from c in db.APROBADORES
                                    where
                                    (((c.Filtro == FILTER_EMPLEADO && c.Cod_Filtro == empleado.Cod_Empleado)
                                    || (c.Filtro == FILTER_CARGO && c.Cod_Filtro == empleado.Cod_Cargo)
                                    || (c.Filtro == FILTER_DEPTO && c.Cod_Filtro == empleado.Cod_Depto)
                                    || (c.Filtro == FILTER_CCOSTO && c.Cod_Filtro == empleado.Cod_Ccostos)
                                    || (c.Filtro == FILTER_SUCURSAL && c.Cod_Filtro == empleado.Cod_Sucursal)
                                    || (c.Filtro == FILTER_ZONA && c.Cod_Filtro == empleado.Cod_Zona))
                                    || (c.Filtro == FILTER_GENERAL))
                                    && c.Tipo_Aprobacion == "V"
                                    && c.Nivel == "0"
                                    && c.Estado != "R"
                                    orderby c.Filtro
                                    select c.Cod_Empleado;

      var aprobadores = db.TERCEROS.Where(t => codsAprobadoresTerceros.Contains(t.Cod_Tercero) && t.Documento != empleado.Cedula  );

      float diasCausados = 0;
      float diasTomados = 0;
      float diasPendientes = 0;
      float diasAprobados = 0;

      causadas.ToList().ForEach(v => diasCausados += v.Dias_Disponibles);
      pendientes.ToList().ForEach(v => diasPendientes += v.Cantidad);
      aprobadas.ToList().ForEach(s => diasAprobados += s.Cantidad);
      if (pendientes.Count() <= 0) { pendientes = null; }
      if (aprobadas.Count() <= 0) { aprobadas = null; }

      VacacionesTomadasViewModel oldHvm = new VacacionesTomadasViewModel();
      DateTime? oldFechaLllegada = null;
      VACACIONES lastPeriodo = tomadas.LastOrDefault();
      List<VacacionesTomadasViewModel> historico = new List<VacacionesTomadasViewModel>();
      foreach (VACACIONES v in tomadas)
      {
        VacacionesTomadasViewModel hvm = new VacacionesTomadasViewModel();
        if (v.Dias_Tiempo > 0 && v.Dias_Dinero == 0)
        {
          diasTomados += v.Dias_Tiempo;
          hvm.Cantidad = Convert.ToInt32(v.Dias_Tiempo);
          hvm.ModoVacaciones = "Tiempo";
        }
        else if (v.Dias_Dinero > 0 && v.Dias_Tiempo == 0)
        {
          diasTomados += v.Dias_Dinero;
          hvm.Cantidad = Convert.ToInt32(v.Dias_Dinero);
          hvm.ModoVacaciones = "Dinero";
        }
        else
        {
          diasTomados += v.Dias_Tiempo;
          hvm.Cantidad = Convert.ToInt32(v.Dias_Tiempo);
          hvm.ModoVacaciones = "Tiempo";

          diasTomados += v.Dias_Dinero;
          VacacionesTomadasViewModel PeriodoTiempo = new VacacionesTomadasViewModel
          {
            Cantidad = Convert.ToInt32(v.Dias_Dinero),
            ModoVacaciones = "Dinero",
            Desde = UtilHelper.getDate(v.Desde),
            Hasta = UtilHelper.getDate(v.Hasta)
          };
          historico.Add(PeriodoTiempo);
        }
        hvm.Desde = UtilHelper.getDate(v.Desde);
        hvm.Hasta = UtilHelper.getDate(v.Hasta);

        historico.Add(hvm);

        oldHvm = hvm;
      }


      historico = historico.OrderByDescending(h => h.Desde).ToList();
      float diasDisponibles = diasCausados - diasTomados - diasPendientes;
      float diasDisponiblesCausados = diasCausados - diasTomados - diasPendientes;

      int Dia_Cierre_Novedades = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
      var Dia_Cierre_Novedades_Var = db.VARIABLES_VACACIONES.Where(v => v.Uso == 0).ToList();
      if (Dia_Cierre_Novedades_Var.Count() > 0)
      {
        Dia_Cierre_Novedades = Convert.ToInt32(Dia_Cierre_Novedades_Var.Last().Tamano);
      }
      

      var mostrarDiasBNP = UtilHelper.ExisteVariable(db, 101);
      var hasBlockLeave = BlocLeaveHelper.HasBlockLeave(db, empleado);
      if (!mostrarDiasBNP)
      {
        hasBlockLeave = true;
      }

      

      // Calcular días ley y beneficios para BNP
      var DiasDisponiblesPeriodos = new List<float>();
      float DiasLey = 0;
      float DiasBeneficio = 0;
      var ddisponibles = diasTomados;

      for (int i = 0; i < causadas.Count(); i++)
      {
        DiasDisponiblesPeriodos.Add(causadas[i].Dias_Disponibles);
        if (i == causadas.Count() - 1)
        {
          break;
        }
        if (ddisponibles >= DIAS_LEY_BNP)
        {
          if (DiasDisponiblesPeriodos[i] >= DIAS_LEY_BNP)
          {
            DiasDisponiblesPeriodos[i] -= DIAS_LEY_BNP;
            ddisponibles -= DIAS_LEY_BNP;
          }
          else
          {
            var aux = (float)Math.Floor(DiasDisponiblesPeriodos[i]);
            DiasDisponiblesPeriodos[i] -= aux;
            ddisponibles -= aux;
          }
        }
        else
        {
          DiasDisponiblesPeriodos[i] -= ddisponibles;
          ddisponibles = 0;
        }
      }

      for (int i = 0; i < DiasDisponiblesPeriodos.Count(); i++)
      {
        if (i == causadas.Count() - 1)
        {
          break;
        }

        if (DiasDisponiblesPeriodos[i] == 0)
        {
          continue;
        }

        if (ddisponibles >= DIAS_BENEFICIO_BNP)
        {
          if (DiasDisponiblesPeriodos[i] >= DIAS_BENEFICIO_BNP)
          {
            DiasDisponiblesPeriodos[i] -= DIAS_BENEFICIO_BNP;
            ddisponibles -= DIAS_BENEFICIO_BNP;
          }
          else
          {
            var aux = (float)Math.Floor(DiasDisponiblesPeriodos[i]);
            DiasDisponiblesPeriodos[i] -= aux;
            ddisponibles -= aux;
          }
        }
        else
        {
          DiasDisponiblesPeriodos[i] -= ddisponibles;
          ddisponibles = 0;
        }
      }


      if (ddisponibles > 0)
      {
        DiasDisponiblesPeriodos[DiasDisponiblesPeriodos.Count() - 1] -= ddisponibles;
      }
      if (DiasDisponiblesPeriodos.Count() > 0 && DiasDisponiblesPeriodos.Last() < 0)
      {
        DiasDisponiblesPeriodos[DiasDisponiblesPeriodos.Count() - 1] = 0;
      }


      if (DiasDisponiblesPeriodos.Count() > 1)
      {
        for (int i = 0; i < DiasDisponiblesPeriodos.Count() - 1; i++)
        {
          if (DiasDisponiblesPeriodos[i] >= DIAS_BENEFICIO_BNP)
          {
            DiasLey += DiasDisponiblesPeriodos[i] - DIAS_BENEFICIO_BNP;
            DiasBeneficio += DIAS_BENEFICIO_BNP;
          }
          else
          {
            DiasBeneficio += DiasDisponiblesPeriodos[i];
          }
        }
        if (DiasDisponiblesPeriodos.Last() >= DIAS_LEY_BNP)
        {
          DiasLey += DiasDisponiblesPeriodos.Last() - DIAS_BENEFICIO_BNP;
          DiasBeneficio += DiasDisponiblesPeriodos.Last() - DiasLey;
        }
        else
        {
          DiasLey += DiasDisponiblesPeriodos.Last();
        }
      }
      else
      {
        if (DiasDisponiblesPeriodos.Count() > 0)
        {
          if (DiasDisponiblesPeriodos[0] >= DIAS_LEY_BNP)
          {
            DiasLey += DiasDisponiblesPeriodos[0] - DIAS_BENEFICIO_BNP;
            DiasBeneficio += DiasDisponiblesPeriodos[0] - DiasLey;
          }
          else
          {
            DiasLey += DiasDisponiblesPeriodos[0];
          }
        }
      }
      var DiasxAños = Math.Round(Convert.ToDouble(empleado.DiasVacAno) / 12, 2);
      var diffDias = DiasLey  + DiasBeneficio + DiasxAños;


      var fecNomina = db.HISTORICO.Where(h => h.Cod_Concepto == 40).Max(h => h.Fec_Nomina);
      ///--------------///
      DateTime fecCorte = new DateTime(2020, 12, 31); 
      if (fecNomina != null) {
         fecCorte = UtilHelper.getDate(fecNomina);
      }

      var diciembre31 = new DateTime(fecCorte.Year, 12, 31);
      var mesesdiff = diciembre31.Month - fecCorte.Month;

      if(mesesdiff == 0)
      {
        mesesdiff = 12;
      }
      

      var diasCausadosDiciembre31 = ((mesesdiff * DiasxAños) + diasCausados) - diasTomados - diasPendientes;

      var data = new
      {
        causadas,
        historico,
        pendientes,
        aprobadas,
        diasCausados,
        diasTomados,
        diasDisponibles,
        diasPendientes,
        diasAprobados,
        aprobadores,  
        tomadas,
        Dia_Cierre_Novedades,
        DiasDisponiblesPeriodos,
        DiasLey,
        DiasBeneficio,  
        hasBlockLeave,
        mostrarDiasBNP,
        diasCausadosDiciembre31,
        mesesdiff,
        diffDias,
        Sabado
      };
      return Json(data);
    }

    ////////////////////////////////////
    //OBTENER SOLICITUDES APROBADORES//
    //////////////////////////////////
    [Route("Aprobadores/{Empresa}/{Cod_Empleado}/Solicitudes/")]
    [HttpGet]
    public IHttpActionResult GetSolicitudesByAprobador(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var solicitudes = db.SOLICITUD_VACACIONES
                          .Select(s => new
                          {
                            s.Cod_Solicitud_Vacaciones,
                            s.Cantidad_Dias,
                            s.Cod_Aprobador,
                            s.Estado,
                            s.Fec_Llegada,
                            s.Fec_Salida,
                            s.Fec_Solicitud,
                            s.Modo_Vacaciones,
                            Empleado = s.EMPLEADOS.Empleado

                          })
                          .OrderByDescending(s => s.Fec_Solicitud)
                          .Where(s => s.Cod_Aprobador == Cod_Empleado && s.Estado == "P");
      return Json(solicitudes);


    }


    [Route("Aprobadores/{Empresa}/{Cod_Empleado}/Solicitudes/{Cod_Solicitud}")]
    [HttpGet]
    public IHttpActionResult GetSolicitudByAprobador(string Empresa, short Cod_Empleado, int Cod_Solicitud)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var solicitud = db.SOLICITUD_VACACIONES
                        .Select(s => new
                        {
                          s.Cod_Solicitud_Vacaciones,
                          s.Cantidad_Dias,
                          s.Cod_Aprobador,
                          s.Estado,
                          s.Fec_Llegada,
                          s.Fec_Salida,
                          s.Fec_Solicitud,
                          s.Modo_Vacaciones,
                          Empleado = s.EMPLEADOS.Empleado
                        })
                        .SingleOrDefault(s => s.Cod_Solicitud_Vacaciones == Cod_Solicitud && s.Cod_Aprobador == Cod_Empleado);

      if (solicitud != null)
      {
        return Json(solicitud);
      }
      else
      {
        return NotFound();
      }


    }

    [Route("Aprobadores/{Empresa}/{Cod_Empleado}/Solicitudes/All")]
    [HttpGet]
    public IHttpActionResult GetAllSolicitudesByAprobador(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var solicitudes = db.SOLICITUD_VACACIONES
                          .Select(s => new
                          {
                            s.Cod_Solicitud_Vacaciones,
                            s.Cantidad_Dias,
                            s.Cod_Aprobador,
                            s.Estado,
                            s.Fec_Llegada,
                            s.Fec_Salida,
                            s.Fec_Solicitud,
                            s.Modo_Vacaciones,
                            Empleado = s.EMPLEADOS.Empleado

                          })
                          .OrderByDescending(s => s.Fec_Solicitud)
                          .Where(s => s.Cod_Aprobador == Cod_Empleado && s.Estado != "D");
      return Json(solicitudes);

    }


    [Route("{Empresa}/{Cod_Empleado}/Aprobadores")]
    [HttpGet]
    public IHttpActionResult GetAprobadores(string Empresa, short Cod_Empleado)
    {

      JulianaContext db = new JulianaContext(Empresa);
      EMPLEADOS empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);
      var aprobadores = from c in db.APROBADORES
                        where
                        c.Tipo_Aprobacion == "V" &&
                        (c.Filtro == FILTER_EMPLEADO && c.Cod_Filtro == empleado.Cod_Empleado)
                        || (c.Filtro == FILTER_CARGO && c.Cod_Filtro == empleado.Cod_Cargo)
                        || (c.Filtro == FILTER_DEPTO && c.Cod_Filtro == empleado.Cod_Depto)
                        || (c.Filtro == FILTER_CCOSTO && c.Cod_Filtro == empleado.Cod_Ccostos)
                        || (c.Filtro == FILTER_SUCURSAL && c.Cod_Filtro == empleado.Cod_Sucursal)
                        || (c.Filtro == FILTER_ZONA && c.Cod_Filtro == empleado.Cod_Zona)
                        orderby c.Filtro
                        select c;
      return Ok(aprobadores);
    }

      /////////////////////////
     //REPORTE DE VACACIONES//
    /////////////////////////

    [JulianaGenericAuthFilter(IsOpen = true)]
    [Route("Reporte/{Empresa}")]
    [HttpPost]
    public IHttpActionResult Get_Reporte_Vacaciones(string Empresa, [FromBody] ReporteVacacionesVM reporte)
    {

      //if (!AuthManager.HasBusinessConnection(Empresa))
      //{
      //  return this.StatusCode(HttpStatusCode.OK);
      //}

      //if (!AuthManager.HasBusinessConnection(reporte?.Empresa))
      //{
      //  return this.StatusCode(HttpStatusCode.BadRequest);
      //}

      return Ok(reportesBO.GetReporteVacaciones(Empresa, reporte, incluirIncapacidad: true));
    }


    ////////////////////////////
    //HISTORICO DE VACACIONES//
    //////////////////////////

    [JulianaGenericAuthFilter(IsOpen = true)]
    [Route("ReporteHistorico/{Empresa}")]
    [HttpPost]
    public IHttpActionResult GetReporteHistoricoVacaciones(string Empresa, [FromBody] ReporteVacacionesVM reporte)
    {

      if (!AuthManager.HasBusinessConnection(Empresa))
      {
        return this.StatusCode(HttpStatusCode.Unauthorized);
      }

      if (!AuthManager.HasBusinessConnection(reporte?.Empresa))
      {
        return this.StatusCode(HttpStatusCode.BadRequest);
      }

      return Ok(reportesBO.GetReporteVacaciones(Empresa, reporte, true));
    }



    ////////////////////////////
    //METODOS DE VACACIONES////
    //////////////////////////
    public DateTime CalcularFechaLLegada(int dias, DateTime Fec_salida)
    {
      var Fec_Llegada = Fec_salida;
      var diasCopia = dias;
      while (diasCopia > 0)
      {
        Fec_Llegada.AddDays(1);
        diasCopia--;
      }
      return Fec_Llegada;
    }

  }
}
