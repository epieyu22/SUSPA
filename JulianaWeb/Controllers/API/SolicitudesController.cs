using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JW3.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Solicitudes")]  
  public class SolicitudesController : ApiController
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
      { "A9",  "Aprobada Nivel 10"},
      { "AP", "Pagada" },
      { "P", "Pendiente Aprobación" },
      { "PR", "Pendiente Desaprobación" },
      { "R", "Rechazada" },
      { "D", "Eliminada" },
    };

    [Route("{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_All_Solicitudes(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var data = db.SOLICITUDES;
      /*foreach (var s in data)
      {
          s.Estado = estadoDisplayName[s.Estado];
      }*/
      var vacaciones = data.Where(d => d.Tipo_Solicitud == "V");
      var cesantias = data.Where(d => d.Tipo_Solicitud == "C");
      var licencias = data.Where(d => d.Tipo_Solicitud == "I");
      var certificados = data.Where(d => d.Tipo_Solicitud == "O");
      foreach (var v in vacaciones)
      {
        v.Modo_Vacaciones = v.Modo_Vacaciones == "T" ? "Tiempo" : "Dinero";
      }

      List<VACACIONES> vacacinesHistorico = db.VACACIONES
                                              .Where(v => v.SubPeriodo != 0 && v.Cod_Usuario != 999)
                                              .ToList();
      List<HistoricoVm> historico = new List<HistoricoVm>();
      return Json(new { vacaciones, cesantias, historico });
    }


    [Route("Solicitud/Cesntias/{Empresa}/{Cod_Solicitud}")]
    [HttpGet]

    public IHttpActionResult cesnatiassolicitud(string Empresa, int Cod_Solicitud)
    {
      return Ok();
    }


    [Route("Solicitud/{Empresa}/{Cod_Solicitud}")]
    [HttpGet]
    public IHttpActionResult Get_Solicitud_Data(string Empresa, int Cod_Solicitud)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var data = db.SOLICITUDES.Where(s => s.Cod_Solicitud == Cod_Solicitud).ToList();
      var concepto = new CONCEPTOS();
      if (data.Count() > 0)
      {
        SOLICITUDES solicitud = data.First();


        List<APROBACIONES> aprobacionesList = db.APROBACIONES
                                            .Where(a => a.Id_Solicitud == solicitud.Cod_Solicitud)
                                            .ToList();
        if (solicitud.Cod_Concepto != 0) {
           concepto = db.CONCEPTOS
                                          .Where(a => a.Cod_Concepto == solicitud.Cod_Concepto)
                                          .First();
        }
        DateTime hoy = DateTime.Now.Date;
        DateTime finMes = new DateTime(hoy.Year, hoy.Month, DateTime.DaysInMonth(hoy.Year, hoy.Month));
        bool Periodo_Cerrado = db.VARIABLES_VACACIONES.FirstOrDefault(v => DateTime.Now.Day > v.Tamano) != null && solicitud.Fec_Salida.Value <= finMes.Date;

        List<AprobacionesSolicitudVm> aprobaciones = new List<AprobacionesSolicitudVm>();

        foreach (APROBACIONES a in aprobacionesList)
        {
          var aprobador = db.APROBADORES.FirstOrDefault(t => t.Cod_Empleado == a.Id_Aprobador);
          var CodDelAprobador = 0;

          // SI NO LO ENCUENTRA COMO APROBADOR, BUSQUELO COMO EMPLEADO
          if (aprobador == null)
          {
            var aprobador2 = (from e in db.EMPLEADOS
                              join t in db.TERCEROS on e.Cedula equals t.Documento
                              where e.Cod_Empleado == a.Id_Aprobador
                              select new
                              {
                                t.Cod_Tercero
                              }).ToList();

            // Verificar si aprobador2 tiene elementos antes de acceder a Cod_Tercero
            if (aprobador2.Any())
            {
              CodDelAprobador = aprobador2.First().Cod_Tercero;
            }
            else
            {
              CodDelAprobador = 0; // O maneja el caso según sea necesario
            }
          }
          else
          {
            CodDelAprobador = aprobador.Cod_Empleado;
          }

          var tercero = db.TERCEROS.SingleOrDefault(s => s.Cod_Tercero == CodDelAprobador);

          AprobacionesSolicitudVm n = new AprobacionesSolicitudVm
          {
            Aprobador = tercero != null ? tercero.Tercero : "",
            Fec_Aprobacion = UtilHelper.getDate(a.Fecha_Aprobado),
            Nivel = a.Nivel + 1,
            Observaciones = a.Observaciones
          };

          aprobaciones.Add(n);
        }

        return Ok(new { solicitud, aprobaciones, concepto, Periodo_Cerrado });
      }
      else
      {
        return NotFound();
      }
    }

    [Route("SolicitudLicencia/{Empresa}/{Cod_Solicitud}")]
    [HttpGet]
    public IHttpActionResult Get_Solicitud_Licencias(string Empresa, int Cod_Solicitud)
    {

      JulianaContext db = new JulianaContext(Empresa);
      var data = db.SOLICITUDES.Where(s => s.Cod_Solicitud == Cod_Solicitud).ToList();
      if (data.Count() > 0)
      {
        SOLICITUDES solicitud = data.First();
        List<APROBACIONES> aprobacionesList = db.APROBACIONES
                                            .Where(a => a.Id_Solicitud == solicitud.Cod_Solicitud)
                                            .ToList();
        List<CONCEPTOS> concepto = db.CONCEPTOS
                                            .Where(a => a.Cod_Concepto == solicitud.Cod_Concepto)
                                            .ToList();

        List<AprobacionesSolicitudVm> aprobaciones = new List<AprobacionesSolicitudVm>();

       
        foreach (APROBACIONES a in aprobacionesList)
        {
          var aprobador = db.APROBADORES.FirstOrDefault(t => t.Cod_Aprobador == a.Id_Aprobador);
          var tercero = db.TERCEROS.SingleOrDefault(s => s.Cod_Tercero == aprobador.Cod_Empleado);
          AprobacionesSolicitudVm n = new AprobacionesSolicitudVm
          {
            Aprobador = tercero != null ? tercero.Tercero : "",
            Fec_Aprobacion = UtilHelper.getDate(a.Fecha_Aprobado),
            Nivel = a.Nivel + 1,
            Observaciones = a.Observaciones
          };
          aprobaciones.Add(n);
        }
   
        //String nomConcepto = concepto.IndexOf(x => x.Nom_Concepto);
        return Ok(new { solicitud, aprobaciones, concepto[0].Nom_Concepto });
      }
      else
      {
        return NotFound();
      }
    }


    //Nueva solicitud de vacaciones
    [Route("{Empresa}/{Cod_Empleado}")]
    [HttpPost]
    public IHttpActionResult NewSolicitud(string Empresa, short Cod_Empleado, [FromBody] SOLICITUDES data)
    {
      var db = new JulianaContext(Empresa);

      string alerta = "";
      EMPLEADOS Empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);
     
      if (data.Tipo_Solicitud == "V")
      {
       
        if (data.Modo_Vacaciones == "T")
        {
          data.Fec_Llegada = VacacionesHelper.Calcular_Fec_LLegada(Empleado, data.Cantidad, data.Fec_Salida.Value);
        }
        else
        {
          data.Fec_Llegada = data.Fec_Salida.Value;
        }
        var solicitudesAprobadas = db.SOLICITUDES.Where(
                                      x => x.Cod_Empleado == Cod_Empleado
                                      && (x.Estado != "AP" && x.Estado != "D")
                                      );

        var solicitudes = db.SOLICITUDES
                          .Where(
                              s => s.Cod_Empleado == Cod_Empleado
                              && (s.Estado != "AP" && s.Estado != "D" && s.Estado != "R")
                              && ((s.Fec_Salida <= data.Fec_Salida && s.Fec_Llegada >= data.Fec_Llegada) && (s.Fec_Salida <= data.Fec_Salida && s.Fec_Salida <= data.Fec_Llegada && s.Fec_Llegada <= data.Fec_Llegada)) 
                              && s.Tipo_Solicitud == data.Tipo_Solicitud
                            );
      
        if (data.Tipo_Solicitud == "T")
        {
          // Solo funcionara con solicitudes de vacaciones
          if (solicitudes.Count() > 0)
          {
            string error = "";
            if (solicitudes.First().Estado == "P")
            {
              error = "Usted posee una o más solicitudes de vacaciones PENDIENTE POR APROBAR en ese periodo";
            }
            else if (solicitudes.First().Estado == "A")
            {
              error = "Usted posee una o más solicitudes de vacaciones APROBADA en ese periodo";
            }
            if (error != "")
            {
              return BadRequest(error);
            }
          }

        }

        if (data.Tipo_Solicitud == "V")
        {
          // Solo funcionara con solicitudes de vacaciones
          if (solicitudes.Count() > 0)
          {
            string error = "";
            if (solicitudes.First().Estado == "P")
            {
              error = "Usted posee una o más solicitudes de vacaciones PENDIENTE POR APROBAR en ese periodo";
            }
            else if (solicitudes.First().Estado == "A")
            {
              error = "Usted posee una o más solicitudes de vacaciones APROBADA en ese periodo";
            }
            if (error != "")
            {
              return BadRequest(error);
            }
          }

        }

        var FecInicio = data.Fec_Salida.Value.ToString("yyyyMMdd");
        var FecFinal = data.Fec_Llegada.Value.ToString("yyyyMMdd");
        
        var Estados = new List<String> { "A", "T", "E", "P", "L", "W", "C", "" };
        var vacaciones = db.VACACIONES.Where(v =>
            ((v.Desde.CompareTo(FecInicio) >= 0 && v.Desde.CompareTo(FecFinal) < 0) || (v.Hasta.CompareTo(FecInicio) > 0 && v.Hasta.CompareTo(FecFinal) <= 0))
            && v.SubPeriodo != 0
            && v.Cod_Empleado == Empleado.Cod_Empleado
            && Estados.Contains(v.Estado));
        if (vacaciones.Count() > 0)
        {
          var msg = "Ya existe un registro de vacaciones que incluye la fecha seleccionada {0} - {1}";
          msg = String.Format(msg, data.Fec_Salida.Value.ToString("yyyy/MM/dd"), data.Fec_Llegada.Value.ToString("yyyy/MM/dd"));
          return BadRequest(msg);
        }

        data.Fec_Solicitud = DateTime.Now;
        data.Estado = "P";
        SOLICITUDES solicitud = db.SOLICITUDES.Add(data);
        db.SaveChanges();
        TERCEROS aprobador = db.TERCEROS.FirstOrDefault(a => a.Cod_Tercero == data.Cod_Aprobador);
        if (solicitud.Tipo_Solicitud == "V")
        {
          MailHelper.Nueva_Solicitud_Vacaciones(Empresa, aprobador, Empleado, solicitud, solicitud.Fec_Llegada.Value);
          string mensaje = "Codigo Empleado: {0} Genero un nueva solicitud de vacaciones. Cantidad: {1}, Fecha Salida: {2}, Fecha Llegada: {3}";
          mensaje = String.Format(mensaje, Empleado.Cod_Empleado, solicitud.Cantidad, UtilHelper.getUnglyDate(solicitud.Fec_Salida.Value), UtilHelper.getUnglyDate(solicitud.Fec_Salida.Value));
          AuditoriaHelper.Log(db, "SOLICITUDES", "N", mensaje);
        }
        if (alerta != string.Empty)
        {
          return Ok(new { alerta = alerta });
        }

      }
      return Ok();
    }

    [Route("Empleados/{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult GetSolicitudesByEmpleado(string Empresa, short Cod_Empleado)
    {
      try
      {
        JulianaContext db = new JulianaContext(Empresa);
        var solicitudes = db.SOLICITUDES.Where(s => s.Cod_Empleado == Cod_Empleado);
        var cesantias = solicitudes.Where(s => s.Tipo_Solicitud == "C");
        var vacaciones = solicitudes.Where(s => s.Tipo_Solicitud == "V");
        foreach (var v in vacaciones)
        {
          v.Modo_Vacaciones = v.Modo_Vacaciones == "T" ? "Tiempo" : "Dinero";
        }
        return Ok(new { cesantias, vacaciones });
      }
      catch (Exception e)
      {
        return NotFound();
      }

    }

    [Route("Aprobador/{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult GetSolicitudesByAprobador(string Empresa, short Cod_Empleado)
    {
      try
      {
        JulianaContext db = new JulianaContext(Empresa);
        var estadosExcluidos = new string[] { "P", "PR", "D", "R" };
        var solicitudes = db.SOLICITUDES.Where(s => s.Cod_Aprobador == Cod_Empleado && !estadosExcluidos.Contains(s.Estado));
        foreach (var s in solicitudes)
        {
          s.Estado = estadoDisplayName[s.Estado];
        }
        var cesantias = solicitudes.Where(s => s.Tipo_Solicitud == "C");
        var licencias = solicitudes.Where(s => s.Tipo_Solicitud == "L" || s.Tipo_Solicitud =="H");
        var vacaciones = solicitudes.Where(s => s.Tipo_Solicitud == "V");
        return Ok(new { cesantias, vacaciones, licencias });
      }
      catch (Exception e)
      {
        return NotFound();
      }

    }

    [Route("{Empresa}/{Cod_Solicitud}")]
    [HttpGet]
    public IHttpActionResult GetSolicitudDetail(string Empresa, int Cod_Solicitud)
    {
      try
      {
        JulianaContext db = new JulianaContext(Empresa);
        var solicitud = db.SOLICITUDES.SingleOrDefault(s => s.Cod_Solicitud == Cod_Solicitud);
        return Ok(solicitud);
      }
      catch (Exception e)
      {
        return NotFound();
      }

    }

    [Route("{Empresa}/{Cod_Empleado}/{Cod_Solicitud}/Aprobar")]
    [HttpPost]
    public IHttpActionResult AprobarSolicitud(string Empresa,int Cod_Empleado , int Cod_Solicitud)
    {
      // Initlize database connection
      JulianaContext db = new JulianaContext(Empresa);


      // Get Sick Leave Request by Id
      var solicitud = db.SOLICITUDES.SingleOrDefault(s => s.Cod_Solicitud == Cod_Solicitud);

      DateTime hoy = DateTime.Now.Date;
      DateTime finMes = new DateTime(hoy.Year, hoy.Month, DateTime.DaysInMonth(hoy.Year, hoy.Month));
      bool Periodo_Cerrado = db.VARIABLES_VACACIONES.FirstOrDefault(v => DateTime.Now.Day > v.Tamano) != null && solicitud.Fec_Salida.Value <= finMes.Date;
      if (Periodo_Cerrado)
      {
        return BadRequest("Periodo Cerrado.");
      }
      // SOLICITUDES PASADAS
      // Validación adicional: si la fecha de salida ya pasó (es decir, es menor a hoy)
      bool fechaSalidaPasada = solicitud.Fec_Salida.Value < new DateTime(hoy.Year, hoy.Month, 1);
      // Si se cumple alguna de las dos condiciones, se arroja el error
      if (fechaSalidaPasada)
      {
        return BadRequest("La fecha de salida fué el mes pasado. No se puede aprobar la solicitud.");
      }
      // check if the Request already Approved or Rejected
      if (solicitud.Estado == "A" || solicitud.Estado == "AP")
      {
        return Json(new { error = "Esta solicitud ya fue procesada" });
      }
      var DtFecInicio = solicitud.Fec_Salida.Value.Date;
      var DtFecFinal = solicitud.Fec_Llegada.Value.Date;

      var oldSolicituds = db.SOLICITUDES.Where(s => s.Cod_Empleado == solicitud.Cod_Empleado &&
      s.Estado != "D" &&
      s.Cod_Solicitud != solicitud.Cod_Solicitud &&
      s.Estado != "R" &&
      ((DbFunctions.AddMinutes(DbFunctions.TruncateTime(s.Fec_Salida.Value), 1) >= DtFecInicio &&
      DbFunctions.AddMinutes(DbFunctions.TruncateTime(s.Fec_Salida.Value), 1) <= DtFecFinal) ||
      (DbFunctions.AddMilliseconds(DbFunctions.TruncateTime(s.Fec_Llegada.Value), -3) >= DtFecInicio &&
      DbFunctions.AddMilliseconds(DbFunctions.TruncateTime(s.Fec_Llegada.Value), -3) <= DtFecFinal)))
      .ToList();

      if (oldSolicituds.Count > 0)
      {
        var msg = "Ya existe un registro de solicitud para el empleado {0} que incluye la fecha seleccionada {1} - {2}";
        msg = String.Format(msg, solicitud.empleado.Empleado.Trim(), solicitud.Fec_Salida.Value.ToString("yyyy/MM/dd"), solicitud.Fec_Llegada.Value.ToString("yyyy/MM/dd"));
        return BadRequest(msg);
      }

      var FecInicio = solicitud.Fec_Salida.Value.ToString("yyyyMMdd");
      var FecFinal = solicitud.Fec_Llegada.Value.ToString("yyyyMMdd");

      var oldNovauts = db.NOVAUT.SqlQuery($@"
      SELECT TOP 1 *
      FROM NOVAUT 
      WHERE Cod_Empleado = { solicitud.Cod_Empleado }
      AND Estado NOT IN ('D', 'R') 
      AND TRY_CAST(Desde AS DATETIME) IS NOT NULL 
      AND TRY_CAST(Hasta AS DATETIME) IS NOT NULL
      AND ((DATEADD(MI, 1, CAST(Desde AS DATETIME)) >=  CAST('{ FecInicio }' AS DATETIME) AND DATEADD(MI, 1, CAST(Desde AS DATETIME)) <=  CAST('{ FecFinal }' AS DATETIME))
      OR (DATEADD(MS, -3, CAST(Hasta AS DATETIME)) >=  CAST('{ FecInicio }' AS DATETIME) AND DATEADD(MS, -3, CAST(Hasta AS DATETIME)) <=  CAST('{ FecFinal }' AS DATETIME)))
      ").ToList();
      //var oldNovauts = db.NOVAUT.Where(n => n.Cod_Empleado == solicitud.Cod_Empleado &&
      //n.Estado != "D" &&
      //n.Estado != "R" &&
      //((Convert.ToDateTime($"{n.Desde.Substring(0, 4)}-{n.Desde.Substring(4, 2)}-{n.Desde.Substring(6, 2)}") >= DtFecInicio && n.Desde.CompareTo(FecFinal) <= 0) || (n.Hasta.CompareTo(FecInicio) >= 0 && n.Hasta.CompareTo(FecFinal) <= 0))).ToList();

      if (oldNovauts.Count > 0)
      {
        var msg = "Ya existe un registro de ausencia para el empleado {0} que incluye la fecha seleccionada {1} - {2}";
        msg = String.Format(msg, solicitud.empleado.Empleado.Trim(), solicitud.Fec_Salida.Value.ToString("yyyy/MM/dd"), solicitud.Fec_Llegada.Value.ToString("yyyy/MM/dd"));
        return BadRequest(msg);
      }

      var AprobadorDeSolcitud = (
          from e in db.EMPLEADOS
          join t in db.TERCEROS on e.Cedula equals t.Documento
          where e.Cod_Empleado == Cod_Empleado
          select new
          {
            t.Cod_Tercero
          }
      ).ToList();

      if (AprobadorDeSolcitud.Count == 0)
      {
        AprobadorDeSolcitud = (
          from e in db.TERCEROS
          where e.Cod_Tercero == solicitud.Cod_Aprobador
          select new
          {
            e.Cod_Tercero
          }
      ).ToList();
      }
      // Get the First Apprroval
      //  Aprobación Multinivel
      var aprobadorActual = db.APROBADORES.FirstOrDefault(a => a.Cod_Empleado == solicitud.Cod_Aprobador); // Aprobador Primer Nivel, Quien Aparece en la solicitud

      // Check if the Approval Null
      if (aprobadorActual == null)
      {
        return Ok("Nada");
      }

      // Get List Of Approvals without the First Apprroval
      List<APROBADORES> aprobadores = db.APROBADORES
                                        .Where(
                                            a => a.Filtro == aprobadorActual.Filtro
                                            && a.Cod_Filtro == aprobadorActual.Cod_Filtro
                                            && a.Sub_Nivel == aprobadorActual.Sub_Nivel
                                            && a.Cod_Aprobador != aprobadorActual.Cod_Aprobador
                                        ).ToList();

      aprobadores = aprobadores.FindAll(a => Convert.ToInt32(a.Nivel) > Convert.ToInt32(aprobadorActual.Nivel));

      if (aprobadores.Count() > 0)
      {
        // Update Request to Be Approved
        db.SOLICITUDES.Attach(solicitud);
        solicitud.Estado = "A" + aprobadorActual.Nivel.Trim(); // A0, A1, ... An
        solicitud.Cod_Aprobador = Convert.ToInt16(aprobadores[0].Cod_Empleado);

        // Send Email
        // Envia Correo Informando de la nueva solicitud al aprobador del nivel superior
        EMPLEADOS Empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == solicitud.Cod_Empleado);
        TERCEROS nuevoAprobador = db.TERCEROS.FirstOrDefault(a => a.Cod_Tercero == solicitud.Cod_Aprobador);

        if (solicitud.Tipo_Solicitud == "V")
        {
          MailHelper.Nueva_Solicitud_Vacaciones(Empresa, nuevoAprobador, Empleado, solicitud, solicitud.Fec_Llegada.Value);
        }

        db.SaveChanges();
      }

      else
      {
        var Estados = new List<String> { "A", "T", "E", "P", "L", "W", "C", "" };

        // Check if there is a vacancy submitted in that table or not 
        var vacaciones = db.VACACIONES.Where(v =>
            ((v.Desde.CompareTo(FecInicio) >= 0 && v.Desde.CompareTo(FecFinal) <= 0) || (v.Hasta.CompareTo(FecInicio) >= 0 && v.Hasta.CompareTo(FecFinal) <= 0))
            && v.SubPeriodo != 0
            && v.Cod_Empleado == solicitud.empleado.Cod_Empleado
            && Estados.Contains(v.Estado));

        // thorw validation if the above found 
        if (vacaciones.Count() > 0 && solicitud.Tipo_Solicitud == "T")
        {
          var msg = "Ya existe un registro de vacaciones para el empleado {0} que incluye la fecha seleccionada {1} - {2}";
          msg = String.Format(msg, solicitud.empleado.Empleado.Trim(), solicitud.Fec_Salida.Value.ToString("yyyy/MM/dd"), solicitud.Fec_Llegada.Value.ToString("yyyy/MM/dd"));
          return BadRequest(msg);
        }
        var Cod_Aprobador = AprobadorDeSolcitud.FirstOrDefault().Cod_Tercero;
        // update the request to be approved
        // Ultima Aprobación.
        db.SOLICITUDES.Attach(solicitud);
        solicitud.Estado = "A";
        solicitud.Cod_Aprobador = Convert.ToInt16(Cod_Aprobador);
        db.SaveChanges();

        NOVAUT New_Novaut = null;
        if (solicitud.Tipo_Solicitud != "V")
        {
            New_Novaut = new NOVAUT
            {
              Cod_Concepto = solicitud.Cod_Concepto,
              Cod_Empleado = solicitud.empleado.Cod_Empleado,
              Cod_Usuario = 999,
              Dias = (short)solicitud.Cantidad,
              Desde = FecInicio,
              Hasta = FecFinal,
              Cod_Diag = 0,
              NumComprobante = "",
              Horas = 0,
              Prorroga = 0,
              DescuentaAuxilio = "",
              Cod_Imagen = 0,
              Fec_Aprobado = "",
              Fec_Desaprobado = "",
              Estado = "",
              Num_Autoriza = "",
              Val_Novedad = 0,
              Dias_Pag_100 = 0,
              Val_Reembolso_Eps = 0,
              IBC = 0,
              Dias_Pag_100_Ant = 0
            };

            New_Novaut = db.NOVAUT.Add(New_Novaut);
            db.SaveChanges();
        }




        //Send Email for V
        if (solicitud.Tipo_Solicitud == "V")
        {
          EMPLEADOS empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == solicitud.Cod_Empleado);
          short Cod_Concepto = solicitud.Modo_Vacaciones == "T" ? (short)9 : (short)10;
          VacacionesHelper.InsertarRegistroVacaciones(db, Empresa, empleado, solicitud.Cod_Solicitud, Cod_Concepto, solicitud.Fec_Salida.Value.ToString("yyyyMMdd"), solicitud.Cantidad);        
          MailHelper.Solicitud_Vacaciones_Aprobada(empleado, solicitud);
        }

        //Send Email for L OR H
        else if (solicitud.Tipo_Solicitud == "L" || solicitud.Tipo_Solicitud == "H")
        {
          EMPLEADOS ee = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == solicitud.Cod_Empleado);
          CONCEPTOS concepto = db.CONCEPTOS.SingleOrDefault(c => c.Cod_Concepto == solicitud.Cod_Concepto);
          if (solicitud.Tipo_Solicitud == "L" && New_Novaut != null)
            RELACIONSOLVAC.GuardarRelacionVacaciones(Empresa, Cod_Solicitud, new List<int> { (int)New_Novaut.AutoNum });
          MailHelper.EnviarSolicitudLicenciaAprobada(ee, solicitud, concepto.Nom_Concepto);
        }

      }

      // Create New Record In Apporvals Table
      int nivel = Convert.ToInt32(aprobadorActual.Nivel);
      var aprobacion = new APROBACIONES();
      var id = db.APROBACIONES.Count() > 0 ? db.APROBACIONES.Max(a => a.Id_Aprobacion) : 0;
      aprobacion.Id_Aprobacion = id + 1;
      aprobacion.Id_Aprobador = AprobadorDeSolcitud.FirstOrDefault().Cod_Tercero;
      aprobacion.Id_Solicitud = solicitud.Cod_Solicitud;

      aprobacion.Observaciones = "APROBÓ";
      aprobacion.Fecha_Aprobado = UtilHelper.getUnglyDate(DateTime.Now);
      aprobacion.Depto_Sol = 0;
      aprobacion.Nivel = nivel;
      db.APROBACIONES.Add(aprobacion);
      db.SaveChanges();
      return Ok();

    }

    [Route("PedidoDesaprobacion/{Empresa}/{Cod_Solicitud}")]
    [HttpPost]
    public IHttpActionResult PedidoDesaprobacionSolicitud(string Empresa, int Cod_Solicitud)
    {
      try
      {
        JulianaContext db = new JulianaContext(Empresa);
        var solicitud = db.SOLICITUDES.SingleOrDefault(s => s.Cod_Solicitud == Cod_Solicitud);
        db.SOLICITUDES.Attach(solicitud);
        solicitud.Estado = "PR";
        db.SaveChanges();
        if (new string[] { "V", "L" }.Contains(solicitud.Tipo_Solicitud))
        {
          MailHelper.mailPedidoDesaprobacion(solicitud);
        }
        return Ok();
      }
      catch (Exception e)
      {
        return Ok(e);
      }

    }

    [Route("{Empresa}/{Cod_Empleado}/{Cod_Solicitud}/Rechazar")]
    [HttpPost]
    public IHttpActionResult RechazarSolicitud(string Empresa, int Cod_Empleado, int Cod_Solicitud, [FromBody] SOLICITUDES data, [FromUri] string Cedula="")
    {
      try
      {
        JulianaContext db = new JulianaContext(Empresa);
        var solicitud = db.SOLICITUDES.SingleOrDefault(s => s.Cod_Solicitud == Cod_Solicitud);

        var respuesta = new Dictionary<string, object> { {"status", ""}, { "message", "" }  };

        db.SOLICITUDES.Attach(solicitud);
        if (UtilHelper.EsIgualA(solicitud.Estado, "PR", "P"))
        {
          // Sí es un pedido de desaprobación borrara los registros previamente creados en la tabla vacaciones
          switch (solicitud.Tipo_Solicitud == "V" ?
            RELACIONSOLVAC.BorrarRelacionVacaciones(Empresa, solicitud.Cod_Solicitud) :
            RELACIONSOLVAC.BorrarRelacionLicencias(Empresa, solicitud.Cod_Solicitud))
          {
            case 0:
              solicitud.Estado = "R";
              solicitud.Cod_Motivo_Rechazo = data.Cod_Motivo_Rechazo;
              solicitud.Observacion = data.Observacion;
              Cedula = Cedula == null ? "" : Cedula;
              var PersonaSesion = db.EMPLEADOS.FirstOrDefault(b => b.Cedula == Cedula || b.Cod_Empleado == Cod_Empleado);
              var cedula = (Cedula ?? PersonaSesion?.Cedula).Trim();
              var TerceroSesion = db.TERCEROS.FirstOrDefault(c => c.Documento == cedula);
              var aprobador = db.APROBADORES.FirstOrDefault(a => a.Cod_Empleado == solicitud.Cod_Aprobador);
              var CodDeAprobador = 0;
              if (aprobador.Cod_Filtro != TerceroSesion.Cod_Tercero)
              { CodDeAprobador = TerceroSesion.Cod_Tercero; }
              else
              { CodDeAprobador = aprobador.Cod_Empleado; }
              

              int nivel = Convert.ToInt32(aprobador.Nivel);
              var aprobacion = new APROBACIONES();
              var id = db.APROBACIONES.Count() > 0 ? db.APROBACIONES.Max(a => a.Id_Aprobacion) : 0;
              aprobacion.Id_Aprobacion = id + 1;
              aprobacion.Id_Aprobador = CodDeAprobador;
              aprobacion.Id_Solicitud = solicitud.Cod_Solicitud;
              aprobacion.Observaciones = "RECHAZÓ";
              aprobacion.Fecha_Aprobado = UtilHelper.getUnglyDate(DateTime.Now);
              aprobacion.Depto_Sol = 0;
              aprobacion.Nivel = nivel;
              db.APROBACIONES.Add(aprobacion);

              respuesta["status"] = "OK";
              respuesta["message"] = "";

              if (solicitud.Tipo_Solicitud == "V")
              {
                MailHelper.mailRechzarSolicitudVacaciones(solicitud.empleado, solicitud);
              }
              else if(solicitud.Tipo_Solicitud == "H" || solicitud.Tipo_Solicitud == "L")
              {
                MailHelper.mailRechzarSolicitudLicencias(solicitud.empleado, solicitud);
              }

              break;

            case -1:
              solicitud.Estado = "A";

              respuesta["status"] = "OK";
              respuesta["message"] = "Su solicitud permanecerá aprobada, dado que existen vacaciones liquidadas.";

              break;

            case 1:
              respuesta["status"] = "BAD";
              respuesta["message"] = "Ocurrió un error al intentar rechazar su solicitud. Intentelo más tarde";
              break;
          }
        }
        // Si está en estado pendiente, se rechaza automaticamente
        else if(solicitud.Estado == "P")
        {
          solicitud.Estado = "R";
          solicitud.Cod_Motivo_Rechazo = data.Cod_Motivo_Rechazo;
          solicitud.Observacion = data.Observacion;


          var aprobador = db.APROBADORES.Where(a => a.Cod_Empleado == solicitud.Cod_Aprobador)
                                  .ToList().First();

          int nivel      = Convert.ToInt32(aprobador.Nivel);
          var aprobacion = new APROBACIONES();
          var id         = db.APROBACIONES.Count() > 0 ? db.APROBACIONES.Max(a => a.Id_Aprobacion) : 0;

          aprobacion.Id_Aprobacion  = id + 1;
          aprobacion.Id_Aprobador   = aprobador.Cod_Aprobador;
          aprobacion.Id_Solicitud   = solicitud.Cod_Solicitud;
          aprobacion.Observaciones  = "RECHAZÓ";
          aprobacion.Fecha_Aprobado = UtilHelper.getUnglyDate(DateTime.Now);
          aprobacion.Depto_Sol      = 0;
          aprobacion.Nivel          = nivel;
          db.APROBACIONES.Add(aprobacion);

          respuesta["status"] = "OK";
          respuesta["message"] = "";

          if (solicitud.Tipo_Solicitud == "V")
          {
            MailHelper.mailRechzarSolicitudVacaciones(solicitud.empleado, solicitud);
          }
          if (solicitud.Tipo_Solicitud == "H" || solicitud.Tipo_Solicitud == "L")
          {
            MailHelper.mailRechzarSolicitudLicencias(solicitud.empleado, solicitud);
          }

        }
        db.SaveChanges();

        return Ok(respuesta);
      }
      catch (Exception e)
      {
        return NotFound();
      }

    }


    [Route("Pendientes/Aprobador/{Empresa}/{Documento}")]
    [HttpGet]
    public IHttpActionResult GetSolicitudesPendientesByAprobador(string Empresa, string Documento)
    {
      try
      {
        JulianaContext db = new JulianaContext(Empresa);

        var estadosExcluidos = new string[] { "A", "AP", "D", "R" };
        var tercero = db.TERCEROS.Where(t => t.Documento == Documento && t.Tipo_Tercero.Trim() == "8").ToList().First();
        if (tercero == null)
        {
          return NotFound(); // Manejar si el tercero no existe.
        }

        bool aprobadorGeneral = db.APROBADORES.Any(q => q.Cod_Filtro == 999 && q.Cod_Empleado == tercero.Cod_Tercero);

        var solicitudes = db.SOLICITUDES.Where(s => (!estadosExcluidos.Contains(s.Estado))).ToList();

        if (!aprobadorGeneral)
        {
          solicitudes = db.SOLICITUDES.Where(s => s.Cod_Aprobador == tercero.Cod_Tercero && (!estadosExcluidos.Contains(s.Estado))).ToList();
        }

        foreach (var s in solicitudes)
          {
            s.Estado = estadoDisplayName[s.Estado];
          }

        var cesantias = solicitudes.Where(s => s.Tipo_Solicitud == "C");
        var licencias = solicitudes.Where(s => s.Tipo_Solicitud == "L" || s.Tipo_Solicitud == "H");
        var vacaciones = solicitudes.Where(s => s.Tipo_Solicitud == "V");
        var certificados = solicitudes.Where(s => s.Tipo_Solicitud == "O");

        return Ok(new { cesantias, vacaciones, licencias, certificados, });
        
      }
      catch (Exception e)
      {
        return NotFound();
      }

    }



    [Route("Mail_Solicitudes_Pendientes/{Empresa}")]
    [HttpGet]
    public IHttpActionResult Mail_Solicitudes_Pendientes(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var estadoFinales = new string[] { "AP", "A", "R", "D" };
      List<SOLICITUDES> sols = db.SOLICITUDES.Where(s => !estadoFinales.Contains(s.Estado) && s.Cod_Aprobador>0).ToList();
      foreach (SOLICITUDES s in sols)
      {
        MailHelper.Mail_Solicitudes_Pendientes(s);
      }
      return Ok();
    }





    [Route("{Empresa}/{Cod_Empleado}/Borrar/{Cod_Solicitud}")]
    [HttpGet]
    public IHttpActionResult DeleteSolicitud(string Empresa, short Cod_Empleado, short Cod_Solicitud)
    {
      var db = new JulianaContext(Empresa);
      var solicitud = db.SOLICITUDES.FirstOrDefault(s => s.Cod_Solicitud == Cod_Solicitud);
      if (solicitud != null)
      {
        var query = String.Format("update SOLICITUDES set Estado = 'D' where Cod_Solicitud = {0}", Cod_Solicitud);
        db.Database.ExecuteSqlCommand(query);
        db.SaveChanges();
      }
      return Ok();
    }

    ///////////////////////////
    //METODOS DE SOLICITUDES//
    /////////////////////////
    [HttpGet]
    [Route("{Empresa}/{Cod_Solicitud}/esRechazable/")]
    public IHttpActionResult ComprobarSolicitudRechazable(string Empresa, short Cod_Solicitud)
    {
      JulianaContext db = new JulianaContext();
      bool esRechazable;
      SOLICITUDES solicitud = db.SOLICITUDES.Where(s => s.Cod_Solicitud == Cod_Solicitud).SingleOrDefault();

      esRechazable = solicitud != null;

      if (esRechazable)
      {
        esRechazable = solicitud.Tipo_Solicitud == "V" ?
                       RELACIONSOLVAC.TieneVacacionesBorrables(Empresa, Cod_Solicitud) :
                       RELACIONSOLVAC.TieneLicenciaBorrable(Empresa, Cod_Solicitud);
      }
      
      return Ok(new {
        esRechazable = esRechazable,
        status = esRechazable ? "OK" : "BAD",
        vacaciones = RELACIONSOLVAC.ObtenerVacacionesSolicitud(Empresa, Cod_Solicitud),
        message = esRechazable ? "" : "No se puede desaprobar o eliminar la solicitud ya está en proceso"
      });
    }

    public void InsertarRegistroVacaciones(JulianaContext db, SOLICITUDES solicitud, short Cod_Empleado)
    {
      // Trae los huecos en el libro de vacaciones
      var periodosDisponibles = (from gv in (from a in db.VACACIONES
                                             where a.Cod_Empleado == Cod_Empleado
                                             orderby a.Periodo, a.SubPeriodo
                                             select new
                                             {
                                               a.Cod_Empleado,
                                               a.Periodo,
                                               a.SubPeriodo,
                                             } into b
                                             group b by new { b.Periodo, b.Cod_Empleado } into c
                                             select new
                                             {
                                               Periodo = c.Key.Periodo,
                                               Cod_Empleado = c.Key.Cod_Empleado,
                                               SubPeriodo = c.Max(d => d.SubPeriodo)
                                             })
                                 join v in db.VACACIONES
                                 on gv.Cod_Empleado equals v.Cod_Empleado
                                 where v.Cod_Empleado == Cod_Empleado
                                 && v.Periodo == gv.Periodo
                                 && v.SubPeriodo == gv.SubPeriodo
                                 && v.Dias_Disponibles != 0
                                 select v
          ).ToList();

      int cantidadDias = solicitud.Cantidad;
      DateTime? Fec_Salida = solicitud.Fec_Salida;
      foreach (var p in periodosDisponibles)
      {
        if (cantidadDias > p.Dias_Disponibles)
        {
          cantidadDias = Convert.ToInt32(cantidadDias - p.Dias_Disponibles);
          DateTime subFechallegada = solicitud.Fec_Llegada.Value.AddDays(-cantidadDias);
          VACACIONES nuevoPeriodo = new VACACIONES
          {
            Periodo = p.Periodo,
            SubPeriodo = p.SubPeriodo
          };
          nuevoPeriodo.SubPeriodo++;
          nuevoPeriodo.Cod_Empleado = p.Cod_Empleado;
          nuevoPeriodo.Desde = UtilHelper.getUnglyDate(solicitud.Fec_Salida.Value);
          nuevoPeriodo.Hasta = UtilHelper.getUnglyDate(subFechallegada);
          nuevoPeriodo.Cod_Usuario = 999;
          nuevoPeriodo.Dias_Disponibles = p.Dias_Disponibles - (solicitud.Cantidad - cantidadDias);
          nuevoPeriodo.Util_A = "N";
          if (solicitud.Modo_Vacaciones == "T")
          {
            nuevoPeriodo.Dias_Tiempo = solicitud.Cantidad - cantidadDias;
            nuevoPeriodo.Dias_Dinero = 0;
          }
          else if (solicitud.Modo_Vacaciones == "B")
          {
            nuevoPeriodo.Dias_Tiempo = solicitud.Cantidad - cantidadDias;
            nuevoPeriodo.Dias_Dinero = 0;
            nuevoPeriodo.Util_A = "S";
          }
          else if (solicitud.Modo_Vacaciones == "D")
          {
            nuevoPeriodo.Dias_Dinero = solicitud.Cantidad - cantidadDias;
            nuevoPeriodo.Dias_Tiempo = 0;
          }
          nuevoPeriodo.Normales = "";
          nuevoPeriodo.Descuentos = "";
          nuevoPeriodo.Fec_Cierre = "";
          nuevoPeriodo.Novedades = "";
          nuevoPeriodo.ProporcionalesP = "";
          nuevoPeriodo.NumComprobante = solicitud.Cod_Solicitud.ToString();
          nuevoPeriodo.Estado = "";
          nuevoPeriodo.Fec_Aprobado = "";
          nuevoPeriodo.Fec_Desaprobado = "";
          nuevoPeriodo.Estado = "";
          nuevoPeriodo.Fec_Pago = "";
          nuevoPeriodo.Clase = "";
          db.VACACIONES.Add(nuevoPeriodo); // to update dont move from here
          Fec_Salida = subFechallegada;
        }
        else
        {
          // funciona tanto como si divide la solicitud en subperiodo, como si no.
          VACACIONES nuevoPeriodo = new VACACIONES
          {
            Periodo = p.Periodo,
            SubPeriodo = p.SubPeriodo
          };
          nuevoPeriodo.SubPeriodo++;
          nuevoPeriodo.Cod_Empleado = p.Cod_Empleado;
          nuevoPeriodo.Desde = UtilHelper.getUnglyDate(Fec_Salida.Value);
          nuevoPeriodo.Hasta = UtilHelper.getUnglyDate(solicitud.Fec_Llegada.Value);
          nuevoPeriodo.Cod_Usuario = 999;
          nuevoPeriodo.Dias_Disponibles = p.Dias_Disponibles - cantidadDias;
          nuevoPeriodo.Util_A = "N";
          if (solicitud.Modo_Vacaciones == "T")
          {
            nuevoPeriodo.Dias_Tiempo = cantidadDias;
            nuevoPeriodo.Dias_Dinero = 0;
          }
          else if (solicitud.Modo_Vacaciones == "B")
          {
            nuevoPeriodo.Dias_Tiempo = cantidadDias;
            nuevoPeriodo.Dias_Dinero = 0;
            nuevoPeriodo.Util_A = "S";
          }
          else if (solicitud.Modo_Vacaciones == "D")
          {
            nuevoPeriodo.Dias_Dinero = cantidadDias;
            nuevoPeriodo.Dias_Tiempo = 0;
          }
          nuevoPeriodo.Normales = "";
          nuevoPeriodo.Descuentos = "";
          nuevoPeriodo.Fec_Cierre = "";
          nuevoPeriodo.Novedades = "";
          nuevoPeriodo.ProporcionalesP = "";
          nuevoPeriodo.NumComprobante = solicitud.Cod_Solicitud.ToString();
          nuevoPeriodo.Estado = "";
          nuevoPeriodo.Fec_Aprobado = "";
          nuevoPeriodo.Fec_Desaprobado = "";
          nuevoPeriodo.Fec_Pago = "";
          nuevoPeriodo.Clase = "";
          db.VACACIONES.Add(nuevoPeriodo);
          break;
        }
      }
      db.SaveChanges();
    }

    public int DiasHabilesEnRango(DateTime fecDesde, DateTime fecHasta, EMPLEADOS empleado)
    {
      if (fecDesde.Date > fecHasta.Date)
      {
        return 0;
      }

      int dias = (fecHasta - fecDesde).Days;
      var festivos = UtilHelper.getHolidays(fecDesde.Year);
      if (fecHasta.Year != fecDesde.Year)
      {
        festivos.Concat(UtilHelper.getHolidays(fecHasta.Year));
      }

      var fecAux = fecDesde;
      while (fecAux <= fecHasta)
      {
        if (festivos.Contains(fecAux))
        {
          dias--;
        }

        if (empleado.Sabado == "1" && fecAux.DayOfWeek == DayOfWeek.Sunday)
        {
          dias--;
        }

        if (fecAux.DayOfWeek == DayOfWeek.Sunday)
        {
          dias--;
        }

        fecAux = fecAux.AddDays(1);
      }
      if (dias <= 0)
      {
        return 0;
      }

      return dias;
    }

    void BorrarRegistroJuliana(JulianaContext db, SOLICITUDES solicittud)
    {
      var registros = db.VACACIONES.Where(v => v.Cod_Usuario == 999 && v.NumComprobante == solicittud.Cod_Solicitud.ToString());
      foreach (VACACIONES r in registros)
      {
        db.VACACIONES.Remove(r);
      }
      db.SaveChanges();
    }

  }
}
