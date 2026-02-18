using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Helpers
{
  public class AuditoriaHelper
  {
    public static void Log(JulianaContext db, string tabla, string accion, string descripcion)
    {
      AUDITORIA data = new AUDITORIA
      {
        Cod_Usuario = 999,
        Fec_Transaccion = UtilHelper.getUnglyDate(DateTime.Now),
        Hora_Transaccion = DateTime.Now.ToString("HH:mm"),
        Tabla = tabla,
        Accion = accion,
        Descripcion = descripcion
      };
      db.AUDITORIA.Add(data);
      db.SaveChanges();
    }
  }
}
