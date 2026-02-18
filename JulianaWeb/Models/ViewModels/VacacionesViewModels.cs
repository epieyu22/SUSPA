using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
    public class VacacionesRequestViewModel
    {
        public DateTime fechaSalida { get; set; }
        public int diasSolicita { get; set; }
        public string modoVacaciones { get; set; }
        public short codAprobador { get; set; }

    }

    public class ConfigAprobadoresViewmodel
    {
        public short Cod_Aprobador_Cesantias { get; set; }
        public short Cod_Aprobador_Vacaciones { get; set; }
    }

    public class VacacionesTomadasViewModel
    {
        public int Cantidad { get; set; }
        public string ModoVacaciones { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
    }


    public class AprobacionesSolicitudVm
    {
        public string Aprobador { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fec_Aprobacion { get; set; }
        public int Nivel { get; set; }
    }

    public class HistoricoVm
    {
        public string Empleado { get; set; }
        public string Tipo_Vacaciones { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
    }



  public class ReporteVacacionesVM
  {
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public string Filtro { get; set; }
    public string Empresa { get; set; }
  }


  public class SPReporteVacaciones
  {
    public short Cod_Empleado { get; set; }
    public string Estado { get; set; }
    public string Cedula { get; set; }
    public string Empleado { get; set; }
    public double Dias_Causados { get; set; }
    public double Dias_Tiempo { get; set; }
    public double Dias_Dinero { get; set; }
    public int? Dias_Pendientes { get; set; }
    public int? Dias_Aprobados { get; set; }
    public int? Dias_Pagados { get; set; }
    public int? Dias_Aprobados_Corte { get; set; }

    public DateTime? Fec_Ingreso { get; set; }
    public string Empresa { get; set; }


    public double? Dias_Causados_Futuro { get; set; }
    public double? Dias_Disponibles { get; set; }
    public double? Dias_Disponibles_Futuro { get; set; }
    public double? Dias_Anticipados { get; set; }
    public bool Util_A { get; set; }

  }
}
