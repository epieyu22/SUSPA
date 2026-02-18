using JulianaWeb.Models;
using System;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Business
{
  public class TimeSheetBO: ApiController
  {
    public dynamic AddDataTimeSheet(string Cedula, TimeSheetViewModel data)
    {
      JulianaContext db = new JulianaContext();
      bool EstadoActivo = true;
      string error = "";
      string fechaIngresada = data.fecha.ToString("yyyy-MM-dd");

      var empleado = db.EMPLEADOS.AsNoTracking().FirstOrDefault(e => e.Cedula == Cedula);

      var parametro = db.PARAMETROS_GENERAL.AsNoTracking().FirstOrDefault(x => x.Estado == EstadoActivo);
      var HorasRegistradas = db.THS_HORAS.Where(x => x.Id_Empleado == empleado.Cod_Empleado && x.Fecha == fechaIngresada).ToList();
      var nuevaHora = new THS_HORAS
      {
        Id_Empleado = empleado.Cod_Empleado,
        Id_Cliente = data.Id_Cliente,
        Fecha = fechaIngresada,
        Cant_Horas = data.num_horas,
        Id_Area = data.Id_Area,
        Id_Concepto = data.Id_Concepto,
        Descripcion = data.Descripcion,
        Req = data.numReq,
      };
      if (HorasRegistradas.Count() == 0)
      {
        db.THS_HORAS.Add(nuevaHora);
        db.SaveChanges();
      }
      else
      {
        foreach (var horasReg in HorasRegistradas)
        {
          int HorasConteo = +horasReg.Cant_Horas + data.num_horas;
          if (HorasConteo <= parametro.ValorParametro)
          {
            db.THS_HORAS.Add(nuevaHora);
            db.SaveChanges();
          }
          else
          {
            BadRequest("Ya inserto una hora en este periodo");
          }        
        }
      }
      return new { nuevaHora };
    }

    public dynamic GetDataTimeSheet(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);

      bool EstadoActivo = true;
      var clientes = db.CLIENTES.ToList();
      var areas = db.THS_AREA.ToList();
      var areas_con = db.THS_AREA_CONCEPTOS.ToList();
      var deptos = db.DEPTOS.ToList();
      var sucursal = db.SUCURSALES.ToList();
      var ths_report = db.THS_HORAS.ToList();


      var parametro = db.PARAMETROS_GENERAL.Where(x => x.Estado == EstadoActivo);

      return new { clientes, areas, areas_con, deptos, sucursal, ths_report, parametro };
    }
    public dynamic GetParametrosTimeSheet(string Cedula)
    {
      return null;
    }

    internal object GetDataTimeSheet()
    {
      throw new NotImplementedException();
    }
  }
}
