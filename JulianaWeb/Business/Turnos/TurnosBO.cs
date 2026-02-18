using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;
using JulianaWeb.Models.ViewModels;

namespace JulianaWeb.Business
{
  public class TurnosBO : ApiController
  {
    private const string TimeHourFormat = "HH:mm";
    private const string TimeSpanHourFormat = @"hh\:mm";

    public dynamic AddTurno(string Empresa, TurnosViewModel data)
    {
      JulianaContext db = new JulianaContext();
      var newTurno = new TIME_CONTROL();
      var TurnoManual = data.Turno_Real;
      DateTime fechaInicio = data.Fecha_Turno;
      DateTime fechaFin = data.Fecha_Turno_Hasta;

      bool check = false;

      if (TurnoManual != null & data.Manual == check)
      {
        int turnoTemplate = Convert.ToInt32(data.Turno_Real);
        TURNOS Turno = db.TURNOS.SingleOrDefault(e => e.Cod_Turno == turnoTemplate);
        data.Hora_Ent = Turno.Hora_Ent;
        data.Hora_Sal = Turno.Hora_Sal;

      }
      else if (data.Manual == true)
      {
        DateTime Entrada = DateTime.Parse(data.Hora_Ent);
        data.Hora_Ent = Entrada.ToString("HHmm");

        DateTime Salida = DateTime.Parse(data.Hora_Sal);
        data.Hora_Sal = Salida.ToString("HHmm");
        data.Turno_Real = "M";
      }
      if (data.Cod_Tipo == "1")
      {
        List<EMPLEADOS> empleadosDepto = db.EMPLEADOS.Where(e => e.Cod_Depto == data.Cod_Depto).OrderBy(e => e.Empleado).ToList();
        foreach (var Cod_Empleado in empleadosDepto)
        {
          var TurnosAsignados = db.TIME_CONTROL
            .Where(x => x.Cod_Empleado == Cod_Empleado.Cod_Empleado).ToList();

          List<DateTime> FechasIngresadas = new List<DateTime>();

          for (DateTime date = fechaInicio; date.Date <= fechaFin; date = date.AddDays(1))
          {
            FechasIngresadas.Add(date);
          }

          if (TurnosAsignados != null)
          {
            if (true)
            {

              foreach (var item in FechasIngresadas)
              {
                var TurnosAConsultar = db.TIME_CONTROL
                 .Where(x => x.Cod_Empleado == Cod_Empleado.Cod_Empleado && x.Fecha_Turno == item.Date).ToList();

                if (TurnosAConsultar.Count == 0)
                {
                  newTurno = new TIME_CONTROL
                  {
                    Cod_Empleado = Cod_Empleado.Cod_Empleado,
                    Fecha_Turno = item.Date,
                    Turno_Proyectado = data.Turno_Real,
                    Turno_Real = data.Turno_Real,
                    Hora_Ent = data.Hora_Ent,
                    Hora_Sal = data.Hora_Sal,
                    Hora_Ent_Real = data.Hora_Ent_Real,
                    Hora_Sal_Real = data.Hora_Sal_Real,
                    Tiempo_Retardo = data.Tiempo_Retardo,
                    Cod_Observacion = data.Cod_Observacion,
                    Tipo = data.Tipo,
                    Estado = data.Estado,
                    Fec_Nomina = data.Fec_Nomina,
                  };
                  db.TIME_CONTROL.Add(newTurno);
                  db.SaveChanges();
                }
                else
                {
                  foreach (var TurnoEncontrado in TurnosAConsultar)
                  {
                    if (TurnoEncontrado.Fecha_Turno == item.Date)
                    {
                      TurnoEncontrado.Cod_Empleado = Cod_Empleado.Cod_Empleado;
                      TurnoEncontrado.Fecha_Turno = item.Date;
                      TurnoEncontrado.Turno_Proyectado = data.Turno_Proyectado;
                      TurnoEncontrado.Turno_Real = data.Turno_Real;
                      TurnoEncontrado.Hora_Ent = data.Hora_Ent;
                      TurnoEncontrado.Hora_Sal = data.Hora_Sal;
                      TurnoEncontrado.Hora_Ent_Real = data.Hora_Ent_Real;
                      TurnoEncontrado.Hora_Sal_Real = data.Hora_Sal_Real;
                      TurnoEncontrado.Tiempo_Retardo = data.Tiempo_Retardo;
                      TurnoEncontrado.Cod_Observacion = data.Cod_Observacion;
                      TurnoEncontrado.Tipo = data.Tipo;
                      TurnoEncontrado.Estado = data.Estado;
                      TurnoEncontrado.Fec_Nomina = data.Fec_Nomina;
                      db.TIME_CONTROL.Attach(TurnoEncontrado);
                      db.Entry(TurnoEncontrado).State = EntityState.Modified;
                      try
                      {
                        data.Fecha_Turno = data.Fecha_Turno.AddDays(1);
                        db.SaveChanges();
                      }

                      catch (DbEntityValidationException e)
                      {
                        return Ok(e.EntityValidationErrors);
                      }

                    }
                  }
                }
              }
            }
          }

          else
          {

            foreach (var item in FechasIngresadas)
            {
              var TurnosAConsultar = db.TIME_CONTROL
              .Where(x => x.Cod_Empleado == Cod_Empleado.Cod_Empleado && x.Fecha_Turno == item.Date).ToList();

              if (TurnosAConsultar.Count == 0)
              {
                newTurno = new TIME_CONTROL
                {
                  Cod_Empleado = Cod_Empleado.Cod_Empleado,
                  Fecha_Turno = item.Date,
                  Turno_Proyectado = data.Turno_Proyectado,
                  Turno_Real = data.Turno_Real,
                  Hora_Ent = data.Hora_Ent,
                  Hora_Sal = data.Hora_Sal,
                  Hora_Ent_Real = data.Hora_Ent_Real,
                  Hora_Sal_Real = data.Hora_Sal_Real,
                  Tiempo_Retardo = data.Tiempo_Retardo,
                  Cod_Observacion = data.Cod_Observacion,
                  Tipo = data.Tipo,
                  Estado = data.Estado,
                  Fec_Nomina = data.Fec_Nomina,
                };
                db.TIME_CONTROL.Add(newTurno);
                db.SaveChanges();
              }
            }
          }
        }
      }
      else
      {
        foreach (int Cod_Empleado in data.Cod_Empleados)
        {
          var TurnosAsignados = db.TIME_CONTROL
            .Where(x => x.Cod_Empleado == Cod_Empleado).ToList();

          List<DateTime> FechasIngresadas = new List<DateTime>();

          for (DateTime date = fechaInicio; date.Date <= fechaFin; date = date.AddDays(1))
          {
            FechasIngresadas.Add(date);
          }

          if (TurnosAsignados != null)
          {
            if (true)
            {

              foreach (var item in FechasIngresadas)
              {
                var TurnosAConsultar = db.TIME_CONTROL
                 .Where(x => x.Cod_Empleado == Cod_Empleado && x.Fecha_Turno == item.Date).ToList();

                if (TurnosAConsultar.Count == 0)
                {
                  newTurno = new TIME_CONTROL
                  {
                    Cod_Empleado = Cod_Empleado,
                    Fecha_Turno = item.Date,
                    Turno_Proyectado = data.Turno_Proyectado,
                    Turno_Real = data.Turno_Real,
                    Hora_Ent = data.Hora_Ent,
                    Hora_Sal = data.Hora_Sal,
                    Hora_Ent_Real = data.Hora_Ent_Real,
                    Hora_Sal_Real = data.Hora_Sal_Real,
                    Tiempo_Retardo = data.Tiempo_Retardo,
                    Cod_Observacion = data.Cod_Observacion,
                    Tipo = data.Tipo,
                    Estado = data.Estado,
                    Fec_Nomina = data.Fec_Nomina,
                  };
                  db.TIME_CONTROL.Add(newTurno);
                  db.SaveChanges();
                }
                else
                {

                  foreach (var TurnoEncontrado in TurnosAConsultar)
                  {
                    string workDelayed, timeWorked;
                    RecalculateTime(data, TurnoEncontrado, out workDelayed, out timeWorked);

                    if (TurnoEncontrado.Fecha_Turno == item.Date)
                    {
                      TurnoEncontrado.Cod_Empleado = Cod_Empleado;
                      TurnoEncontrado.Fecha_Turno = item.Date;
                      TurnoEncontrado.Turno_Proyectado = data.Turno_Proyectado;
                      TurnoEncontrado.Turno_Real = data.Turno_Real;
                      TurnoEncontrado.Hora_Ent = data.Hora_Ent;
                      TurnoEncontrado.Hora_Sal = data.Hora_Sal;
                      TurnoEncontrado.Hora_Ent_Real = TurnoEncontrado.Hora_Ent_Real;
                      TurnoEncontrado.Hora_Sal_Real = TurnoEncontrado.Hora_Sal_Real;
                      TurnoEncontrado.Tiempo_Laborado = timeWorked;
                      TurnoEncontrado.Tiempo_Retardo = workDelayed;
                      TurnoEncontrado.Cod_Observacion = data.Cod_Observacion;
                      TurnoEncontrado.Tipo = data.Tipo;
                      TurnoEncontrado.Estado = data.Estado;
                      TurnoEncontrado.Fec_Nomina = data.Fec_Nomina;
                      db.TIME_CONTROL.Attach(TurnoEncontrado);
                      db.Entry(TurnoEncontrado).State = EntityState.Modified;

                      try
                      {
                        data.Fecha_Turno = data.Fecha_Turno.AddDays(1);
                        db.SaveChanges();
                      }

                      catch (DbEntityValidationException e)
                      {
                        return Ok(e.EntityValidationErrors);
                      }

                    }
                  }
                }
              }
            }
          }

          else
          {

            foreach (var item in FechasIngresadas)
            {
              var TurnosAConsultar = db.TIME_CONTROL
              .Where(x => x.Cod_Empleado == Cod_Empleado && x.Fecha_Turno == item.Date).ToList();

              if (TurnosAConsultar.Count == 0)
              {

                newTurno = new TIME_CONTROL
                {
                  Cod_Empleado = Cod_Empleado,
                  Fecha_Turno = item.Date,
                  Turno_Proyectado = data.Turno_Proyectado,
                  Turno_Real = data.Turno_Real,
                  Hora_Ent = data.Hora_Ent,
                  Hora_Sal = data.Hora_Sal,
                  Hora_Ent_Real = data.Hora_Ent_Real,
                  Hora_Sal_Real = data.Hora_Sal_Real,
                  Tiempo_Laborado = "",
                  Tiempo_Retardo = data.Tiempo_Retardo,
                  Cod_Observacion = data.Cod_Observacion,
                  Tipo = data.Tipo,
                  Estado = data.Estado,
                  Fec_Nomina = data.Fec_Nomina,
                };
                db.TIME_CONTROL.Add(newTurno);
                db.SaveChanges();
              }
            }
          }
        }
      }
      return new { newTurno };
    }

    private static void RecalculateTime(TurnosViewModel data, TIME_CONTROL TurnoEncontrado, out string workDelayed, out string timeWorked)
    {
      DateTime realStart = DateTime.Parse(TurnoEncontrado.Hora_Ent_Real);
      DateTime realEnd = DateTime.Parse(TurnoEncontrado.Hora_Sal_Real);
      DateTime start = DateTime.Parse(data.Hora_Ent);
      workDelayed = (realStart - start).ToString(TimeSpanHourFormat);
      timeWorked = (realStart - realEnd).ToString(TimeSpanHourFormat);
    }

    public dynamic Get_Data_Turnos(string Usuario)
    {
      JulianaContext db = new JulianaContext();
      AuthContext authContext = new AuthContext();
      var tercero = db.TERCEROS.SingleOrDefault(x => x.Documento == Usuario);
      var departamentos = db.DEPTOS.ToList();
      var turnos = db.TURNOS.ToList();

      IQueryable<EMPLEADOS> empleados = null;
      if (tercero != null)
      {
        empleados = from A in db.APROBADORES
                    join E in db.EMPLEADOS
                    on A.Cod_Filtro equals E.Cod_Empleado
                    where A.Tipo_Aprobacion == "O"
                    && A.Cod_Empleado == tercero.Cod_Tercero
                    select
                        E;
      }

      int tipoGuardado = 0;


      var trabajador = authContext.AspNetUsers.SingleOrDefault(x => x.UserName == Usuario);
      var trabajadorId = trabajador.Id.ToString();
      var tipoTrabajador = authContext.ApplicationUserGroup.SingleOrDefault(x => x.UserId == trabajadorId);
      if (tipoTrabajador != null)
      {
        tipoGuardado = tipoTrabajador.GroupId;
      }
      return new { empleados, turnos, departamentos, tipoGuardado };
    }

    public dynamic GetDataEmpleadosPorDepto(short Cod_Depto)
    {
      JulianaContext db = new JulianaContext();

      var empleadosDepto = db.EMPLEADOS.Where(e => e.Cod_Depto == Cod_Depto).OrderBy(e => e.Empleado).ToList();

      return new { empleadosDepto };
    }

    public dynamic GetShiftConfig(string Empresa, string Filtro, string Tipo_Aprobacion, short Cod_Filtro)
    {
      JulianaContext db = new JulianaContext(Empresa);

      List<APROBADORES> aprobadores = db.APROBADORES
                              .Where(a => a.Filtro == Filtro && a.Cod_Filtro == Cod_Filtro && a.Tipo_Aprobacion == Tipo_Aprobacion)
                              .ToList();

      List<APROBADORES> aprobadoresActivos = aprobadores.Where(a => a.Estado.Contains('A')).ToList();
      return Ok(aprobadoresActivos);
    }

    public List<TIME_CONTROL> GetTimeControls(TimeControlFilterViewModel filters)
    {
      JulianaContext db = new JulianaContext(filters.Business);
      IQueryable<TIME_CONTROL> query = db.TIME_CONTROL;

      if (filters.EmployeeId > 0)
      {
        query = query.Where(t => t.Cod_Empleado == filters.EmployeeId);
      }

      if (filters.Month != DateTime.MinValue)
      {
        query = query.Where(t => t.Fecha_Turno == filters.Month);
      }

      return query.ToList();
    }

    public void RegisterPunch(TimeControlFilterViewModel payload)
    {
      JulianaContext db = new JulianaContext(payload.Business);

      DateTime currentDate = DateTime.Now;

      DateTime startDate = currentDate.Date;
      DateTime endDate = currentDate.AddDays(1).AddTicks(-1);

      TIME_CONTROL registry = db.TIME_CONTROL
        .FirstOrDefault(
          t => t.Cod_Empleado == payload.EmployeeId &&
          t.Fecha_Turno >= startDate && t.Fecha_Turno <= endDate
        );

      if (registry == null)
      {
        registry = new TIME_CONTROL
        {
          Cod_Empleado = payload.EmployeeId,
          Fecha_Turno = currentDate,
          Estado = 'A'
        };

        db.TIME_CONTROL.Add(registry);
      }

      switch (payload.PunchType)
      {
        case "punch-in":
          registry.Hora_Ent_Real = currentDate.ToString(TimeHourFormat);
          break;

        case "punch-out":
          registry.Hora_Sal_Real = currentDate.ToString(TimeHourFormat);
          break;
      }

      registry.Tiempo_Laborado = GetWorkedHours(registry);
      registry.Tiempo_Retardo = GetDelayedHours(registry);

      db.SaveChanges();
    }

    private string GetWorkedHours(TIME_CONTROL registry)
    {
      if (string.IsNullOrWhiteSpace(registry.Hora_Ent_Real) || string.IsNullOrWhiteSpace(registry.Hora_Sal_Real))
      {
        return string.Empty;
      }

      DateTime realStart = DateTime.Parse(registry.Hora_Ent_Real);
      DateTime realEnd = DateTime.Parse(registry.Hora_Sal_Real);

      return (realStart - realEnd).ToString(TimeSpanHourFormat);

    }

    private string GetDelayedHours(TIME_CONTROL registry)
    {
      if (string.IsNullOrWhiteSpace(registry.Hora_Ent_Real) || string.IsNullOrWhiteSpace(registry.Hora_Ent))
      {
        return string.Empty;
      }

      DateTime start = DateTime.Parse(registry.Hora_Ent);
      DateTime realStart = DateTime.Parse(registry.Hora_Ent_Real);

      return (realStart - start).ToString(TimeSpanHourFormat);
    }

    internal object GetDataTimeSheet()
    {
      throw new NotImplementedException();
    }

  }

}
