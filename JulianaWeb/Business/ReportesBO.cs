using JulianaWeb.Helpers;
using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace JulianaWeb.Business
{
  public class ReportesBO
  {
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
      { "-", "No Definido" }
    };

    Dictionary<string, string> tipoSolicitud = new Dictionary<string, string>()
    {
      { "V", "Vacaciones" },
      { "I", "Incapacidad" },
      { "L", "Licencia" }
    };

    Dictionary<string, string> FILTERS = new Dictionary<string, string>() {
            { "1", "Empleados"},
            { "2", "Cargos"},
            { "3", "Deptos" },
            { "4", "Ccostos"},
            { "5", "Sucursales"},
            { "6", "Zonas"},
            { "9", "General" },
        };

    public dynamic GetReporteVacaciones(string Empresa, ReporteVacacionesVM reporte, bool soloHistoricos = false, bool incluirIncapacidad = false)
    {

      JulianaContext db = new JulianaContext(Empresa);
      List<SOLICITUDES> solicitudes = new List<SOLICITUDES>();
      if (!soloHistoricos)
      {
        if (reporte.Filtro == "Proyeccion")
        {
          reporte.Desde = new DateTime(reporte.Desde.Value.Year, reporte.Desde.Value.Month, 1);
          reporte.Hasta = reporte.Desde.Value.AddMonths(1).AddDays(-1);
          reporte.Filtro = "Fec_Salida";
        }

        switch (reporte.Filtro)
        {
          case "None":
            solicitudes = db.SOLICITUDES.ToList();
            break;

          case "Fec_Salida":
            if (reporte.Desde != null && reporte.Hasta != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Salida >= reporte.Desde.Value && s.Fec_Salida <= reporte.Hasta.Value).ToList();
            }
            else if (reporte.Desde != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Salida >= reporte.Desde.Value).ToList();
            }
            else if (reporte.Hasta != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Salida <= reporte.Hasta.Value).ToList();
            }
            else
            {
              solicitudes = db.SOLICITUDES.ToList();
            }

            break;
          case "Fec_Llegada":
            if (reporte.Desde != null && reporte.Hasta != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Llegada >= reporte.Desde.Value && s.Fec_Llegada <= reporte.Hasta.Value).ToList();
            }
            else if (reporte.Desde != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Llegada >= reporte.Desde.Value).ToList();
            }
            else if (reporte.Hasta != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Llegada <= reporte.Hasta.Value).ToList();
            }
            else
            {
              solicitudes = db.SOLICITUDES.ToList();
            }

            break;
          case "Fec_Solicitud":
            if (reporte.Desde != null && reporte.Hasta != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Solicitud >= reporte.Desde.Value && s.Fec_Solicitud <= reporte.Hasta.Value).ToList();
            }
            else if (reporte.Desde != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Solicitud >= reporte.Desde.Value).ToList();
            }
            else if (reporte.Hasta != null)
            {
              solicitudes = db.SOLICITUDES.Where(s => s.Fec_Solicitud <= reporte.Hasta.Value).ToList();
            }
            else
            {
              solicitudes = db.SOLICITUDES.ToList();
            }
            break;
        }

        if (incluirIncapacidad)
        {
          var query = (from nov in db.NOVAUT
                       join con in db.CONCEPTOS on nov.Cod_Concepto equals con.Cod_Concepto
                       join emp in db.EMPLEADOS on nov.Cod_Empleado equals emp.Cod_Empleado
                       where con.Tipo_Concepto.Trim() == "I" 
                       select new
                       {
                         Cod_Concepto = nov.Cod_Concepto,
                         Cod_Empleado = nov.Cod_Empleado,
                         Cantidad = nov.Dias,
                         Tipo_Solicitud = con.Tipo_Concepto,
                         Descripcion = con.Nom_Concepto,
                         Fec_Solicitud = nov.Desde,
                         Fec_Salida = nov.Desde,
                         Fec_Llegada = nov.Hasta,
                         Estado = nov.Estado,
                         Empleado = emp,
                         Adjunto = nov.Adjunto
                       });

          if (reporte.Desde.HasValue && reporte.Hasta.HasValue)
          {
            string fechaInicio = UtilHelper.getUnglyDate(reporte.Desde.Value);
            string fechaFin = UtilHelper.getUnglyDate(reporte.Hasta.Value);

            query = query.Where(q => String.Compare(q.Fec_Solicitud, fechaInicio) >= 0 &&
                                     String.Compare(q.Fec_Solicitud, fechaFin) <= 0);
          }
          else if (reporte.Desde.HasValue)
          {
            string fechaInicio = UtilHelper.getUnglyDate(reporte.Desde.Value);
            query = query.Where(q => String.Compare(q.Fec_Solicitud, fechaInicio) >= 0);
          }
          else if (reporte.Hasta.HasValue)
          {
            string fechaFin = UtilHelper.getUnglyDate(reporte.Hasta.Value);
            query = query.Where(q => String.Compare(q.Fec_Solicitud, fechaFin) <= 0);
          }

          var queryResult = query.ToList();

          solicitudes = solicitudes.Concat(queryResult.Select(x => new SOLICITUDES()
          {
            Cod_Concepto = x.Cod_Concepto,
            Cod_Empleado = x.Cod_Empleado,
            Cantidad = x.Cantidad,
            Tipo_Solicitud = x.Tipo_Solicitud,
            Descripcion = x.Descripcion,
            Fec_Solicitud = UtilHelper.getDate(x.Fec_Solicitud),
            Fec_Salida = UtilHelper.getDate(x.Fec_Salida),
            Fec_Llegada = UtilHelper.getDate(x.Fec_Llegada),
            Estado = x.Estado.Trim(),
            empleado = x.Empleado,
            Adjunto = x.Adjunto
          })).ToList();

        }
        List<short> cod_conceptos = solicitudes.Where(x => x.Cod_Concepto > 0).Select(x => x.Cod_Concepto).Distinct().ToList();
        Dictionary<short, CONCEPTOS> dataConceptos = db.CONCEPTOS.Where(x => cod_conceptos.Contains(x.Cod_Concepto)).ToDictionary(x => x.Cod_Concepto);

        foreach (var s in solicitudes)
        {
          s.Modo_Vacaciones = s.Modo_Vacaciones == "T" ? "Tiempo" : "Dinero";
          s.Estado = estadoDisplayName.ContainsKey(s.Estado) ? estadoDisplayName[s.Estado] : "";
          s.Tipo_Solicitud = tipoSolicitud.ContainsKey(s.Tipo_Solicitud) ? tipoSolicitud[s.Tipo_Solicitud] : "";
          s.Nom_Concepto = dataConceptos.ContainsKey(s.Cod_Concepto) ? dataConceptos[s.Cod_Concepto].Nom_Concepto : "";
          s.Tipo_Solicitud = s.Tipo_Solicitud.ToUpper();
        }

      }

      var fecNomina = db.HISTORICO.Where(h => h.Cod_Concepto == 40).Max(h => h.Fec_Nomina);
      DateTime fecCorte = UtilHelper.getDate(fecNomina);

      string diasFuturosCausados = reporte.Desde.Value.ToString("yyyyMMdd");
      SqlParameter fechaCorte = new SqlParameter()
      {
        ParameterName = "@Fecha_Corte",
        DbType = DbType.String, 
        Value = diasFuturosCausados
      };
      object[] parametros = new object[] { fechaCorte };

      var his = db.Database.SqlQuery<SPReporteVacaciones>("exec SP_ReporteVacaciones @Fecha_Corte", parametros).ToList();

      if (reporte.Desde == null)
      {
        reporte.Desde = fecCorte;
      }
      
      var difMeses = ((reporte.Desde.Value.Year - fecCorte.Year) * 12) + (reporte.Desde.Value.Month - fecCorte.Month);

      var historicoEmp = from h in his
                         join e in db.EMPLEADOS.ToList() on h.Cod_Empleado equals e.Cod_Empleado
                         select new
                         {
                           Cod_Empleado = h.Cod_Empleado,
                           Estado = h.Estado,
                           Cedula = h.Cedula,
                           Empleado = h.Empleado,
                           Dias_Causados = h.Dias_Causados,
                           Dias_Tiempo = h.Dias_Tiempo,
                           Dias_Dinero = h.Dias_Dinero,
                           Dias_Pendientes = h.Dias_Pendientes,
                           Dias_Aprobados = h.Dias_Aprobados,
                           Dias_Pagados = h.Dias_Pagados,
                           Dias_Aprobados_Corte = h.Dias_Aprobados_Corte,
                           Fec_Ingreso = h.Fec_Ingreso,
                           Empresa = h.Empresa,
                           Dias_Causados_Futuro = h.Dias_Causados_Futuro,
                           Dias_Disponibles = h.Dias_Disponibles,
                           Dias_Disponibles_Futuro = h.Dias_Disponibles_Futuro,
                           Dias_Anticipados = h.Dias_Anticipados,
                           Util_A = h.Util_A,
                           EmpleadoObj = e
                         };
      his = historicoEmp.Select(x =>
      {
        SPReporteVacaciones h = new SPReporteVacaciones();
        h.Cod_Empleado = x.Cod_Empleado;
        h.Estado = x.Estado;
        h.Cedula = x.Cedula;
        h.Empleado = x.Empleado;
        h.Dias_Causados = x.Dias_Causados;
        h.Dias_Tiempo = x.Dias_Tiempo;
        h.Dias_Dinero = x.Dias_Dinero;
        h.Dias_Pendientes = x.Dias_Pendientes;
        h.Dias_Aprobados = x.Dias_Aprobados;
        h.Dias_Pagados = x.Dias_Pagados;
        h.Dias_Aprobados_Corte = x.Dias_Aprobados_Corte;
        h.Fec_Ingreso = x.Fec_Ingreso;
        h.Dias_Causados_Futuro = x.Dias_Causados_Futuro;
        h.Dias_Disponibles = x.Dias_Disponibles;
        h.Dias_Disponibles_Futuro = x.Dias_Disponibles_Futuro;
        h.Dias_Anticipados = x.Dias_Anticipados;
        h.Util_A = x.Util_A;
        h.Empresa = Empresa;
        if (!string.IsNullOrWhiteSpace(x.EmpleadoObj.Fec_Ingreso)) h.Fec_Ingreso = UtilHelper.getDate(x.EmpleadoObj.Fec_Ingreso);
        if (h.Dias_Pendientes == null) h.Dias_Pendientes = 0;
        if (h.Dias_Aprobados == null) h.Dias_Aprobados = 0;
        h.Dias_Tiempo = h.Dias_Tiempo - h.Dias_Aprobados.Value;
        h.Dias_Disponibles = h.Dias_Causados - h.Dias_Tiempo - h.Dias_Dinero - h.Dias_Pendientes - h.Dias_Aprobados;
        h.Dias_Anticipados = h.Dias_Disponibles < 0 ? Math.Abs(h.Dias_Disponibles.Value) : 0;
        h.Dias_Causados_Futuro = h.Dias_Causados + (1.25 * difMeses);
        h.Dias_Disponibles_Futuro = h.Dias_Causados_Futuro - h.Dias_Tiempo - h.Dias_Dinero - h.Dias_Pendientes.Value - h.Dias_Aprobados_Corte;
        h.Dias_Disponibles = h.Dias_Disponibles < 0 ? 0 : h.Dias_Disponibles;

        return h;
      }).ToList();

      if (soloHistoricos) return new { his };

      return new { solicitudes, his };
    }

    public GetReporteAprobadoresResponse GetReporteAprobadores(string Empresa, int? page = null, int? count = null, string docEmpleado = null, string carIdEmpleado = null, string empleado = null, string docAprobador = null, string carIdAprobador = null, string aprobador = null)
    {
      JulianaContext db = new JulianaContext(Empresa);

      //List<APROBADORES> aprobadores = db.APROBADORES
      //                                .Where(

      //                                    a => a.Estado == "A"
      //                                ).ToList();

      List<AprobadoresViewModel> data = (from A in db.APROBADORES
                                         join T in db.TERCEROS on A.Cod_Empleado equals T.Cod_Tercero
                                         join E in db.EMPLEADOS on A.Cod_Filtro equals E.Cod_Empleado
                                         where A.Estado == "A" && E.Estado != "R"
                                         && (string.IsNullOrEmpty(docEmpleado) || E.Cedula.Contains(docEmpleado))
                                          && (string.IsNullOrEmpty(carIdEmpleado) || E.Cod_Colaborador.Contains(carIdEmpleado))
                                          && (string.IsNullOrEmpty(empleado) || E.Empleado.Contains(empleado))
                                          && (string.IsNullOrEmpty(docAprobador) || T.Documento.Contains(docAprobador))
                                          && (string.IsNullOrEmpty(carIdAprobador) || T.CareerID.Contains(carIdAprobador))
                                          && (string.IsNullOrEmpty(aprobador) || T.Tercero.Contains(aprobador))
                                         select new AprobadoresViewModel
                                         {
                                           Cod_Aprobador = A.Cod_Aprobador,
                                           Cod_Empleado = A.Cod_Empleado,
                                           Aprobador = T.Tercero,
                                           Filtro = A.Filtro,
                                           Cod_Filtro = A.Cod_Filtro,
                                           Tipo_Aprobacion = A.Tipo_Aprobacion,
                                           Estado = A.Estado,//add code
                                           Detalle_Tipo_Aprobacion = A.Tipo_Aprobacion == "V" ? "Vacaciones" : "Cesantias",
                                           Documento_Empleado = E.Cedula,
                                           Cod_Colaborador_Empleado = E.Cod_Colaborador,
                                           Documento_Aprobador = T.Documento,
                                           CareerID_Aprobador = T.CareerID
                                         }).ToList();
          int totalRows = data.Count;

          if (page.HasValue)
          {
            data = data.OrderByDescending(e => e.Tipo_Aprobacion)
              .ThenByDescending(e => e.Filtro)
              .ThenBy(e => e.Nivel)
              .Skip(((int)page - 1) * (int)page)
              .Take((int)count)
              .ToList();
          }
          else
          {
            data = data.OrderByDescending(e => e.Tipo_Aprobacion)
            .ThenByDescending(e => e.Filtro)
            .ThenBy(e => e.Nivel)
            .ToList();
          }


      //List<AprobadoresViewModel> data = new List<AprobadoresViewModel>();



      foreach (var a in data)
      {
        // o tercero
        //TERCEROS tercero = db.TERCEROS.First(e => e.Cod_Tercero == a.Cod_Empleado);
        //AprobadoresViewModel avm = new AprobadoresViewModel
        //{
        //  Cod_Aprobador = a.Cod_Aprobador,
        //  Cod_Empleado = a.Cod_Empleado,
        //  Aprobador = tercero.Tercero,
        //  Filtro = a.Filtro,
        //  Cod_Filtro = a.Cod_Filtro,
        //  Nom_Filtro = FILTERS[a.Filtro]
        //};
        switch (a.Filtro)
        {
          case "6":
            a.Detalle_Filtro = db.ZONAS.First(z => z.Cod_Zona == a.Cod_Filtro).Nom_Zona;
            break;
          case "5":
            a.Detalle_Filtro = db.SUCURSALES.First(s => s.Cod_Sucursal == a.Cod_Filtro).Nom_Sucursal;
            break;
          case "4":
            a.Detalle_Filtro = db.CCOSTOS.First(c => c.Cod_Ccosto == a.Cod_Filtro).Nom_Ccosto;
            break;
          case "3":
            a.Detalle_Filtro = db.DEPTOS.First(d => d.Cod_Depto == a.Cod_Filtro).Nombre_Depto;
            break;
          case "2":
            a.Detalle_Filtro = db.CARGOS.First(c => c.Cod_Cargo == a.Cod_Filtro).Nom_Cargo;
            break;
          case "1":
            a.Detalle_Filtro = db.EMPLEADOS.First(e => e.Cod_Empleado == a.Cod_Filtro).Empleado;
            break;
        }
        a.Nom_Filtro = FILTERS[a.Filtro];
        //a.Nivel = Convert.ToInt32(a.Nivel);
        //a.SubNivel = Convert.ToInt32(a.Sub_Nivel);
        //a.Tipo_Aprobacion = a.Tipo_Aprobacion;
        //a.Estado = a.Estado;//add code
        //a.Detalle_Tipo_Aprobacion = a.Tipo_Aprobacion == "V" ? "Vacaciones" : "Cesantias";
        //data.Add(avm);
        
      }
      return new GetReporteAprobadoresResponse
      {
        Items = data,
        TotalRows = totalRows
      };
    }
  }
}
