using JulianaWeb.Helpers;
using JulianaWeb.Interfaces.Aspects;
using JulianaWeb.Models;
using JulianaWeb.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{

  [RoutePrefix("API/Cesantias")]
  public class CesantiasController : ApiController
  {
    string FILTER_EMPLEADO = "1";
    string FILTER_CARGO = "2";
    string FILTER_DEPTO = "3";
    string FILTER_CCOSTO = "4";
    string FILTER_SUCURSAL = "5";
    string FILTER_ZONA = "6";
    string FILTER_GENERAL = "9";

    private readonly ILoggerService<CesantiasController> _logger;

    public CesantiasController()
    {
      _logger = LoggerServiceFactory.Get<CesantiasController>();
    }

    [Route("{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult GetCesantiasEmpleado(string Empresa, short Cod_Empleado)
    {
      try
      {
        JulianaContext db = new JulianaContext(Empresa);
        EMPLEADOS empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);
        var cesantias = db.CESANTIAS.Where(c => c.Cod_Empleado == Cod_Empleado).OrderByDescending(c => c.Desde);
        var anticipos = cesantias.Where(c => c.Cod_Empleado == Cod_Empleado && c.Tipo_Registro == "A");
        PARAMETROS parametros = db.PARAMETROS.ToList().Last();
        var cesantiasPagadas = cesantias.Where(c => c.Estado == "P" && c.Tipo_Registro != "A");
        var cesantiasConsolidadas = cesantias.Where(c => c.Estado == "S" && c.Tipo_Registro != "A");
        double valorCesantiasConsolidadas = 0;
        double valorInteresesConsolidados = 0;
        if (cesantiasConsolidadas.Count() > 0)
        {

          var cdc = cesantiasConsolidadas
              .Where(c => c.Tipo_Registro == "C");

          if (cdc.Count() > 0)
          {
            valorCesantiasConsolidadas = cdc.ToList().Last().Valor;

          }
          var cdi = cesantiasConsolidadas
              .Where(c => c.Tipo_Registro == "I");
          if (cdi.Count() > 0)
          {
            valorInteresesConsolidados = cdi.ToList().Last().Valor;

          }
        }
        List<HistoricoCesantiasViewModel> historicoCesantias = new List<HistoricoCesantiasViewModel>();
        foreach (CESANTIAS c in cesantiasPagadas)
        {
          HistoricoCesantiasViewModel hcvm = new HistoricoCesantiasViewModel
          {
            Desde = UtilHelper.getDate(c.Desde),
            Hasta = UtilHelper.getDate(c.Hasta),
            Tipo_Registro = c.Tipo_Registro == "C" ? "Cesantias" : "Intereses",
            Valor = c.Valor
          };
          historicoCesantias.Add(hcvm);
        }

        List<HistoricoCesantiasViewModel> historicoAnticipos = new List<HistoricoCesantiasViewModel>();
        foreach (CESANTIAS c in anticipos)
        {
          HistoricoCesantiasViewModel hcvm = new HistoricoCesantiasViewModel
          {
            Desde = UtilHelper.getDate(c.Desde),
            Hasta = UtilHelper.getDate(c.Hasta),
            Tipo_Registro = "Anticipo",
            Valor = c.Valor
          };
          historicoAnticipos.Add(hcvm);
        }

        var aprobadores = from c in db.APROBADORES
                          where
                          ((c.Filtro == FILTER_EMPLEADO && c.Cod_Filtro == empleado.Cod_Empleado)
                          || (c.Filtro == FILTER_CARGO && c.Cod_Filtro == empleado.Cod_Cargo)
                          || (c.Filtro == FILTER_DEPTO && c.Cod_Filtro == empleado.Cod_Depto)
                          || (c.Filtro == FILTER_CCOSTO && c.Cod_Filtro == empleado.Cod_Ccostos)
                          || (c.Filtro == FILTER_SUCURSAL && c.Cod_Filtro == empleado.Cod_Sucursal)
                          || (c.Filtro == FILTER_ZONA && c.Cod_Filtro == empleado.Cod_Zona))
                          || (c.Filtro == FILTER_GENERAL)
                          && c.Tipo_Aprobacion == "V"
                          orderby c.Filtro
                          select c;

        var solicitudesPendientes = from s in db.SOLICITUDES
                                    where s.Tipo_Solicitud == "C"
                                     && s.Cod_Empleado == Cod_Empleado
                                     && s.Estado == "P"
                                    select s;

        var solicitudesAprobadas = from s in db.SOLICITUDES
                                   where
                                    s.Tipo_Solicitud == "C"
                                    && s.Cod_Empleado == Cod_Empleado
                                    && (s.Estado == "A" || s.Estado == "PR")
                                   select s;


        DateTime fechaDesde = new DateTime(DateTime.Now.Year, 1, 1);
        DateTime fechaIngreso = UtilHelper.getDate(empleado.Fec_Ingreso);
        if (fechaIngreso > fechaDesde)
        {
          fechaDesde = fechaIngreso;
        }
        DateTime fechaHasta = DateTime.Now;
        DateTime FechaHace3Meses = DateTime.Now.AddDays(-90);

        string fechaInicio = UtilHelper.getUnglyDate(fechaDesde);
        string fechaFin = UtilHelper.getUnglyDate(fechaHasta);

        IQueryable<double> historico;

        double base1 = 0;
        var historico1 = from h in db.HISTORICO
                         join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                         where h.Cod_Empleado == Cod_Empleado
                             && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                             && h.Fec_Nomina.CompareTo(fechaFin) <= 0
                             && c.BSCesantias == "S"
                         select new { h.Val_Novedad, h.Dias_Novedad };
        double diasSiVariable = 0;

        // Calculo de cesantias e intereses causados depéndiendo del tipo de salario
        // "F" fijo y "V" variable
        if (empleado.ModoSalario == "F")
        {
          string fechaInicioSalario = UtilHelper.getUnglyDate(FechaHace3Meses);
          string fechaFinSalario = UtilHelper.getUnglyDate(DateTime.Now);
          var salarios = from s in db.SALARIOS
                         where s.Cod_Empleado == Cod_Empleado
                         && s.Fec_Salario.CompareTo(fechaInicioSalario) >= 0
                         && s.Fec_Salario.CompareTo(fechaFinSalario) <= 0
                         select s;

          if (salarios.Count() > 0)
          {
            // sumar todo como salario variable
            historico = from h in db.HISTORICO
                        join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                        where h.Cod_Empleado == Cod_Empleado
                            && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                            && h.Fec_Nomina.CompareTo(fechaFin) <= 0
                            && c.BSCesantias == "S"
                        select h.Val_Novedad;

            foreach (var h in historico1)
            {
              base1 += h.Val_Novedad;
              diasSiVariable += (double)h.Dias_Novedad;
            }
            base1 = (base1 / diasSiVariable) * 30;
          }
          else
          {
            base1 = empleado.Salario;
            // buscar horas extras y demas que haga base para cesantias
            historico = from h in db.HISTORICO
                        join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                        where h.Cod_Empleado == Cod_Empleado
                            && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                            && h.Fec_Nomina.CompareTo(fechaFin) <= 0
                            && c.BSCesantias == "S"
                            && c.Cod_Concepto != 1
                        select h.Val_Novedad;
          }
        }
        else
        {
          
          // tomar todo del historico
          historico = from h in db.HISTORICO
                      join c in db.CONCEPTOS on h.Cod_Concepto equals c.Cod_Concepto
                      where h.Cod_Empleado == Cod_Empleado
                          && h.Fec_Nomina.CompareTo(fechaInicio) >= 0
                          && h.Fec_Nomina.CompareTo(fechaFin) <= 0
                          && c.BSCesantias == "S"
                      select h.Val_Novedad;
        }

        var baseCesantias = base1;


        int diasTrabjados = DateTime.Now.DayOfYear - fechaDesde.DayOfYear;
        var Fec_Nomina = UtilHelper.getDate(parametros.Fec_Nomina);
        var inicio = fechaDesde;
        var diasT = 0;

        while (inicio.Date <= Fec_Nomina.Date)
        {
          var diasMes = DateTime.DaysInMonth(inicio.Year, inicio.Month);
          diasT += diasMes != 30 ? 30 : diasMes;
          inicio = inicio.AddMonths(1);
        }
        var valor = (baseCesantias * diasT) / 360;
        var valor2 = (baseCesantias / 360) * diasT;
        double totalBaseCesantias = 0;
        historico.ToList().ForEach(h => totalBaseCesantias += h);
        double promedioCausadas = totalBaseCesantias > 0 ? (totalBaseCesantias / DateTime.Now.DayOfYear) * 30 : 0;
        double baseCesantiasCausadas = empleado.Salario + promedioCausadas;

        baseCesantiasCausadas = baseCesantias;

        diasTrabjados = diasT;

        if (parametros.BaseCesantiaSubsidio == "N" && baseCesantiasCausadas < (parametros.Salmin * 2))
        {
          baseCesantiasCausadas += parametros.SubTrans;
        }
        double valorCesantiasCausadas = ((baseCesantiasCausadas * diasTrabjados) / 360) - (historicoAnticipos.Count > 0 ? historicoAnticipos[0].Valor : 0);
        double valorIntereseCausados = (valorCesantiasCausadas * 0.12 * diasTrabjados) / 360;
        double totalDisponible = valorCesantiasCausadas + valorCesantiasConsolidadas;

        //LOGGER
        _logger.LogDebug($"Base cesantias: {baseCesantias}");
        _logger.LogDebug($"Dias de trabajo: {diasTrabjados}");
        _logger.LogDebug($"TOTAL CESANTIAS: {totalDisponible}");

        return Json(new
        {
          valorCesantiasCausadas,
          valorIntereseCausados,
          valorCesantiasConsolidadas,
          valorInteresesConsolidados,
          cesantiasConsolidadas,
          historicoCesantias,
          totalDisponible,
          historicoAnticipos,
          solicitudesPendientes,
          solicitudesAprobadas,
          valor,
          diasTrabjados,
          aprobadores,
          diasSiVariable,
          diasT
        });
      }
      catch (Exception ex)
      {
        return Json(ex);
      }
    }

    [Route("Solicitudes/{Empresa}/{Cod_Empleado}")]
    [HttpPost]
    public IHttpActionResult CreateSolicitudCesantias(string Empresa, short Cod_Empleado,
        [FromBody] SolicitudCesantiasViewModels data)
    {
      SOLICITUDES solicitud = new SOLICITUDES
      {
        Cod_Aprobador = data.Cod_Aprobador,
        Cod_Empleado = Cod_Empleado,
        Cantidad = data.Cantidad,
        Fec_Solicitud = DateTime.Now,
        Estado = "P",
        Tipo_Solicitud = "C"
      };
      JulianaContext db = new JulianaContext(Empresa);
      db.SOLICITUDES.Add(solicitud);
      db.SaveChanges();
      return Ok();
    }

    [Route("Solicitudes/{Cod_Solicitud}/{Empresa}")]
    [HttpDelete]
    public IHttpActionResult DeleteSolicitudCesantias(int Cod_Solicitud, string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      SOLICITUDES solicitud = db.SOLICITUDES.SingleOrDefault(s => s.Cod_Solicitud == Cod_Solicitud);
      db.SOLICITUDES.Attach(solicitud);
      // Por motivos de auditoria no se borra de la base de datos.
      // Otro enfoque podria ser crear un rechazo o aprobación.
      solicitud.Estado = "D";
      db.SaveChanges();
      return Ok();
    }


    [Route("Cesantias/Agregar/{Empresa}/{Cod_Empleado}")]
    [HttpPost]
    public void Agregar_Cesantias(string Empresa, short Cod_Empleado, SOLICITUDES data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      CESANTIAS nueva = new CESANTIAS
      {
        Cod_Empleado = Cod_Empleado,
        Fec_Ing_Novedad = UtilHelper.getUnglyDate(DateTime.Now),
        Valor = data.Cantidad,
        Estado = "A",
        Tipo_Registro = "C",
        Cod_Usuario = 999
      };
      db.CESANTIAS.Add(nueva);
      db.SaveChanges();
    }

  }
}
