using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JulianaWeb.Helpers
{
  public class VacacionesHelper
  {

    const int COD_VACACIONES_TIEMPO = 9;
    const int COD_VACACIONES_DINERO = 10;

    public static void InsertarRegistroVacaciones(JulianaContext db, string empresa, EMPLEADOS empleado, int Cod_Solicitud,  short Cod_Concepto, string Desde, int dias)
    {
      string msg = "USUARIO - {0} Cod_Empelado: {1}, Cod_Concepto: {2}, Dias: {3}, Desde: {4}";
      msg = String.Format(msg, "", empleado.Cod_Empleado, Cod_Concepto, dias, Desde);
      AuditoriaHelper.Log(db, "VACACIONES", "N", msg);
      //AuditoriaHelper.NewLog(db, "VACACIONES", "N", msg);

      List<int> CodsVacaciones = new List<int>();

      var registros = VacacionesHelper.DividirVacacionesUltimoDiaMes(db, empleado, Cod_Concepto, Desde, dias);

      // Trae los huecos en el libro de vacaciones            
      var Ulttima_Fec_Salida = UtilHelper.getDate(Desde);
      foreach (var r in registros)
      {
        if (r.Cantidad == 0)
        {
          break;
        }

        var UltimosSubPeriodos = from v in db.VACACIONES
                                 where v.Cod_Empleado == empleado.Cod_Empleado
                                 select new
                                 {
                                   v.Periodo,
                                   v.SubPeriodo
                                 } into b
                                 group b by new { b.Periodo } into c
                                 select new
                                 {
                                   Periodo = c.Key.Periodo,
                                   SubPeriodo = c.Max(d => d.SubPeriodo)
                                 };

        List<VACACIONES> SubPeriodosATenerEnCuenta = new List<VACACIONES>();
        foreach (var UltimoSubPeriodo in UltimosSubPeriodos)
        {
          var Periodo = db.VACACIONES.SingleOrDefault(v => v.Cod_Empleado == empleado.Cod_Empleado
                                     && v.Periodo == UltimoSubPeriodo.Periodo
                                     && v.SubPeriodo == UltimoSubPeriodo.SubPeriodo
                                     && v.Dias_Disponibles != 0);
          if (Periodo != null)
          {
            SubPeriodosATenerEnCuenta.Add(Periodo);
          }
        }

        int i = 0;
        int CantidadDiasRestantesPorIngresar = r.Cantidad;
        DateTime Fec_Salida = Ulttima_Fec_Salida;
        DateTime Fec_Llegada = r.Fec_Llegada;
        foreach (var p in SubPeriodosATenerEnCuenta)
        {
          i++;
          /*
           * Si solo hay un SubPeriodo dispoinble o la cantidad de días solicitados supera los días disponibles,
           * se obliga  metar estos días en el ultimo SubPeriodo
           */
          if (CantidadDiasRestantesPorIngresar > p.Dias_Disponibles && p.Dias_Disponibles > 0 && i != SubPeriodosATenerEnCuenta.Count())
          {
            CantidadDiasRestantesPorIngresar = Convert.ToInt32(CantidadDiasRestantesPorIngresar - Math.Floor(p.Dias_Disponibles));
            /*
             * Si entra aqui los diás que se ingresaran en cada SubPeridod serán los que este tenga disponibles.
             */



            int DiasEnEsteSubPeriodo = Convert.ToInt32(Math.Floor(p.Dias_Disponibles));
            /*
             * Aqui mirar que la subfecha de llegada no sea un festivo, sin modificcar la columna Desde
             * */
            DateTime subFechallegada = Calcular_Fec_LLegada(empleado, DiasEnEsteSubPeriodo, Fec_Salida);
            VACACIONES nuevoPeriodo = new VACACIONES
            {
              Periodo = p.Periodo,
              SubPeriodo = p.SubPeriodo
            };
            nuevoPeriodo.SubPeriodo++;
            nuevoPeriodo.Cod_Empleado = p.Cod_Empleado;
            nuevoPeriodo.Cod_Usuario = 999;
            /*
             * Si entra dentro de esta condición quiere decir que no tiene suficientes días disponibles para abarcar la solicitud,
             * por lo que  los días restantes deben ser cero
             */
            nuevoPeriodo.Dias_Disponibles = 0;
            if (Cod_Concepto == COD_VACACIONES_TIEMPO)
            {
              nuevoPeriodo.Desde = UtilHelper.getUnglyDate(Fec_Salida);
              nuevoPeriodo.Hasta = UtilHelper.getUnglyDate(subFechallegada);
              nuevoPeriodo.Dias_Tiempo = DiasEnEsteSubPeriodo;
              nuevoPeriodo.Dias_Dinero = 0;
            }
            else if (Cod_Concepto == COD_VACACIONES_DINERO)
            {
              nuevoPeriodo.Desde = UtilHelper.getUnglyDate(Ulttima_Fec_Salida);
              nuevoPeriodo.Hasta = UtilHelper.getUnglyDate(Ulttima_Fec_Salida);
              nuevoPeriodo.Dias_Dinero = DiasEnEsteSubPeriodo;
              nuevoPeriodo.Dias_Tiempo = 0;
            }
            nuevoPeriodo.Normales = "";
            nuevoPeriodo.Descuentos = "";
            nuevoPeriodo.Fec_Cierre = "";
            nuevoPeriodo.Novedades = "";
            nuevoPeriodo.ProporcionalesP = "";
            nuevoPeriodo.NumComprobante = "";
            nuevoPeriodo.Estado = "";
            nuevoPeriodo.Fec_Aprobado = "";
            nuevoPeriodo.Fec_Desaprobado = "";
            nuevoPeriodo.Fec_Pago = "";
            nuevoPeriodo.Clase = "";
            nuevoPeriodo.Util_A = "N";
            db.VACACIONES.Add(nuevoPeriodo); // to update dont move from here
            db.SaveChanges();
            Fec_Salida = subFechallegada;
            CodsVacaciones.Add(nuevoPeriodo.AutoNum);
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
            nuevoPeriodo.Cod_Usuario = 999;
            /*
             * Si no tiene suficientes días quedará en negativo
             */
            nuevoPeriodo.Dias_Disponibles = p.Dias_Disponibles - CantidadDiasRestantesPorIngresar;
            if (Cod_Concepto == COD_VACACIONES_TIEMPO)
            {
              nuevoPeriodo.Desde = UtilHelper.getUnglyDate(Fec_Salida);
              nuevoPeriodo.Hasta = UtilHelper.getUnglyDate(Fec_Llegada);
              nuevoPeriodo.Dias_Tiempo = CantidadDiasRestantesPorIngresar;
              nuevoPeriodo.Dias_Dinero = 0;
            }
            else if (Cod_Concepto == COD_VACACIONES_DINERO)
            {
              nuevoPeriodo.Desde = UtilHelper.getUnglyDate(Ulttima_Fec_Salida);
              nuevoPeriodo.Hasta = UtilHelper.getUnglyDate(Ulttima_Fec_Salida);
              nuevoPeriodo.Dias_Dinero = CantidadDiasRestantesPorIngresar;
              nuevoPeriodo.Dias_Tiempo = 0;
            }
            nuevoPeriodo.Normales = "";
            nuevoPeriodo.Descuentos = "";
            nuevoPeriodo.Fec_Cierre = "";
            nuevoPeriodo.Novedades = "";
            nuevoPeriodo.ProporcionalesP = "";
            nuevoPeriodo.NumComprobante = "";
            nuevoPeriodo.Estado = "";
            nuevoPeriodo.Fec_Aprobado = "";
            nuevoPeriodo.Fec_Desaprobado = "";
            nuevoPeriodo.Fec_Pago = "";
            nuevoPeriodo.Clase = "";
            nuevoPeriodo.Util_A = "N";
            db.VACACIONES.Add(nuevoPeriodo);
            db.SaveChanges();
            CodsVacaciones.Add(nuevoPeriodo.AutoNum);

            string msg2 = "SISTEMA - Autonum: {0}, Cod_Empelado: {1}, Cod_Concepto: {2}, Dias Tiempo: {3}, Dias Dinero: {4}, Desde: {5}, Hasta: {6}";
            msg2 = String.Format(msg2, nuevoPeriodo.AutoNum, empleado.Cod_Empleado, Cod_Concepto, nuevoPeriodo.Dias_Tiempo, nuevoPeriodo.Dias_Dinero, nuevoPeriodo.Desde, nuevoPeriodo.Hasta);
            AuditoriaHelper.Log(db, "VACACIONES", "N", msg2);
            //AuditoriaHelper.NewLog(db, "VACACIONES", "N", msg2);
            break;
          }
        }
        Ulttima_Fec_Salida = r.Fec_Llegada;
      }

      RELACIONSOLVAC.GuardarRelacionVacaciones(empresa, Cod_Solicitud, CodsVacaciones);
    }




    /*
    revisra dia 01
     */
    public static DateTime Calcular_Fec_LLegada(EMPLEADOS empleado, int dias, DateTime Fec_Salida)
    {
      DateTime Fec_Llegada = Fec_Salida;
      int Cantidad = dias;
      int CantidadActual = Cantidad;
      var festivos = UtilHelper.getHolidays(Fec_Salida.Year);
      festivos.AddRange(UtilHelper.getHolidays(Fec_Salida.Year + 1));


      var FecInicioFestivo = FecEsFestivo(Fec_Llegada);
      while (FecInicioFestivo)
      {
        Fec_Llegada = Fec_Llegada.AddDays(1);
        FecInicioFestivo = FecEsFestivo(Fec_Llegada);
      }

      while (Cantidad > 0)
      {
        Fec_Llegada = Fec_Llegada.AddDays(1);
        if ((Fec_Llegada.DayOfWeek != DayOfWeek.Saturday && empleado.Sabado == "1") && Fec_Llegada.DayOfWeek != DayOfWeek.Sunday)
        {
          bool esFestivo = false;
          foreach (var f in festivos)
          {
            if (f.Date == Fec_Llegada.Date)
            {
              esFestivo = true;
              break;
            }
          }
          if (!esFestivo)
          {
            Cantidad--;
          }
        }
        else if (empleado.Sabado == "2" && Fec_Llegada.DayOfWeek != DayOfWeek.Sunday)
        {
          bool esFestivo = false;
          foreach (var f in festivos)
          {
            if (f.Date == Fec_Llegada.Date)
            {
              esFestivo = true;
              break;
            }
          }
          if (!esFestivo)
          {
            Cantidad--;
          }
        }
      };

      return Fec_Llegada;
    }

    public static int Dias_Especiales_EntreFechas(EMPLEADOS empleado, DateTime fecha1, DateTime fecha2)
    {
      if (fecha1 > fecha2)
        return Dias_Especiales_EntreFechas(empleado, fecha2, fecha1);

      var festivos = UtilHelper.getHolidays(fecha1.Year);
      festivos.AddRange(UtilHelper.getHolidays(fecha1.Year + 1));
      int dias = 0;
      do
      {
        fecha1 = fecha1.Date.AddDays(1);
        if ((fecha1.Date.DayOfWeek == DayOfWeek.Saturday && empleado.Sabado == "1") ||
          fecha1.Date.DayOfWeek == DayOfWeek.Sunday ||
          festivos.Contains(fecha1.Date))
          dias++;
      } while (fecha1 < fecha2);
      return dias;
    }

    public sealed class DivisionVacaciones
    {
      internal DateTime Fec_Llegada { get; set; }
      internal int Cantidad { get; set; }
    }

    public static List<DivisionVacaciones> DividirVacacionesUltimoDiaMes(JulianaContext db, EMPLEADOS empleado, short Cod_Concepto, string Desde, int dias)
    {
      DateTime Fec_Salida = UtilHelper.getDate(Desde);
      DateTime Fec_Llegada = Fec_Salida;
      int Cantidad = dias;
      int CantidadActual = Cantidad;
      var festivos = UtilHelper.getHolidays(Fec_Salida.Year);
      festivos.AddRange(UtilHelper.getHolidays(Fec_Salida.Year + 1));


      var registros = new List<DivisionVacaciones>();
      // vacaciones en Dinero
      if (Cod_Concepto == COD_VACACIONES_DINERO)
      {
        registros.Add(new DivisionVacaciones { Fec_Llegada = Fec_Salida, Cantidad = Cantidad });
        return registros;
      }

      if (Fec_Llegada.Day == DateTime.DaysInMonth(Fec_Llegada.Year, Fec_Llegada.Month))
      {
        //Cantidad--;
        registros.Add(new DivisionVacaciones { Fec_Llegada = Fec_Salida.AddDays(1), Cantidad = 1 });
        CantidadActual = Cantidad - 1;
      }
      while (Cantidad > 0)
      {
        Fec_Llegada = Fec_Llegada.AddDays(1);
        if ((Fec_Llegada.DayOfWeek != DayOfWeek.Saturday && empleado.Sabado == "1") && Fec_Llegada.DayOfWeek != DayOfWeek.Sunday)
        {
          bool esFestivo = FecEsFestivo(Fec_Llegada);
          if (!esFestivo)
          {
            Cantidad--;
          }
        }
        if (empleado.Sabado == "2" && Fec_Llegada.DayOfWeek != DayOfWeek.Sunday)
        {
          bool esFestivo = FecEsFestivo(Fec_Llegada);
          if (!esFestivo)
          {
            Cantidad--;
          }
        }
        if (Fec_Llegada.Day == DateTime.DaysInMonth(Fec_Llegada.Year, Fec_Llegada.Month) && Cantidad > 0)
        {
          registros.Add(new DivisionVacaciones { Fec_Llegada = Fec_Llegada.AddDays(1), Cantidad = CantidadActual - Cantidad + 1 });
          CantidadActual = Cantidad - 1;
        }
      }
      registros.Add(new DivisionVacaciones { Fec_Llegada = Fec_Llegada, Cantidad = CantidadActual });
      return registros;
    }

    public static bool FecEsFestivo(DateTime Fec)
    {
      var festivos = UtilHelper.getHolidays(Fec.Year);
      festivos.AddRange(UtilHelper.getHolidays(Fec.Year + 1));
      foreach (var f in festivos)
      {
        if (f.Date == Fec.Date)
        {
          return true;
        }
      }
      return false;
    }


  }
}
