using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
    public class SolicitudCesantiasViewModels
    {
        public short Cod_Aprobador { get; set; }
        public int Cantidad { get; set; }
    }

    public class HistoricoCesantiasViewModel
    {
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
        public double Valor { get; set; }
        public string Tipo_Registro { get; set; }
    }

    public class PlanillaNomiinaViewModel
    {
        public string Num_Documento { get; set; }
        public string Empleado { get; set; }
        public double Salario { get; set; }
        public double Total_Devengos { get; internal set; }
        public string cargo { get; internal set; }
        public double Total_Deducidos { get; internal set;}
        public double Retefuente { get; internal set; }
        public double ApoPension { get; internal set; }
        public double ApoSolPension { get; internal set; }
        public double AuxTransporte { get; internal set; }
        public double Otros_Devengos { get; internal set; }
        public double Otros_Deducidos { get; internal set; }
        public double ApoSalud { get; internal set; }
    public string Ccosto { get; internal set; }
    public string Sucursal { get; internal set; }
    public double Total_General { get; internal set; }
    public double Vacaciones { get; internal set; }
    public double Cesantias { get; internal set; }
    public double Intereses_Cesantias { get; internal set; }
    public float Dias_Trabajados { get; internal set; }
    public float Dias_Vacaciones { get; internal set; }
    public double Comisiones { get; internal set; }
  }
}
