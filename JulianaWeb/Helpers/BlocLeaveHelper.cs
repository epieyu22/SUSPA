using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Helpers
{
  public class BlocLeaveHelper
  {
    public static Boolean HasBlockLeave(JulianaContext db, EMPLEADOS Empleado) {
      var bl = db.VARIABLES.SingleOrDefault(v => v.Uso == 101);
      if (bl == null) return false;
      var Fec_Ingreso = UtilHelper.getDate(Empleado.Fec_Ingreso);

      /* Si tienen menos de un año trabajando no necesita que la alerta aparezca por eso lo marco como si ya hubiese tomado el beneficio */
      double anosDesdeIngreso = UtilHelper.GetYearsAhead(Fec_Ingreso);
      if (anosDesdeIngreso <= 1) {
        return true;
      }

      var Fec_Inicio = new DateTime(DateTime.Now.Year, Fec_Ingreso.Month, Fec_Ingreso.Day).ToString("yyyyMMdd");
      var Fec_Final = new DateTime(DateTime.Now.Year + 1, Fec_Ingreso.Month, Fec_Ingreso.Day).ToString("yyyyMMdd");
      if(Fec_Ingreso.Date > DateTime.Now.Date)
      {
        Fec_Inicio = new DateTime(DateTime.Now.Year - 1, Fec_Ingreso.Month, Fec_Ingreso.Day).ToString("yyyyMMdd");
        Fec_Final = new DateTime(DateTime.Now.Year, Fec_Ingreso.Month, Fec_Ingreso.Day).ToString("yyyyMMdd");
      }
      var vacaciones = from v in db.VACACIONES
                       where v.Cod_Empleado == Empleado.Cod_Empleado
                       && ((v.Desde.CompareTo(Fec_Inicio) >= 0 && v.Desde.CompareTo(Fec_Final) <= 0)
                || (v.Hasta.CompareTo(Fec_Inicio) >= 0 && v.Hasta.CompareTo(Fec_Final) <= 0))
                       && v.Util_A == "S"
                       select v;
      if (vacaciones.Count() > 0) return true;
      var solicitudes = db.SOLICITUDES.Where(s => s.Cod_Empleado == Empleado.Cod_Empleado && s.Modo_Vacaciones == "B" && s.Estado != "AP");
      if (solicitudes.Count() > 0) return true;


      var ausentismo = from v in db.NOVAUT
                       where v.Cod_Empleado == Empleado.Cod_Empleado
                       && ((v.Desde.CompareTo(Fec_Inicio) >= 0 && v.Desde.CompareTo(Fec_Final) <= 0)
                || (v.Hasta.CompareTo(Fec_Inicio) >= 0 && v.Hasta.CompareTo(Fec_Final) <= 0))
                       select v;
      if (ausentismo.Count() > 0)
      {
        foreach(var a in ausentismo)
        {
          if (a.Dias >= 15 || a.Dias_Pag_100 >= 15) return true;
        }
      }


      return false;
    }
  }
}
