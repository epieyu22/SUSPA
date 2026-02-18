using System;
using System.Collections.Generic;

namespace JulianaWeb.Models
{
    public class AprobadoresViewModel
    {
        public int Cod_Aprobador { get; set; }
        public Int16? Cod_Empleado { get; set; }
        public string Aprobador { get; set; }
        public string Filtro { get; set; }
        public short Cod_Filtro { get; set; }
        public string Nom_Filtro { get; set; }
        public string Detalle_Filtro { get; set; }
        public string Tipo_Aprobacion { get; set; }
        public string Estado { get; set; } //ad code
        public int Nivel { get; set; }
        public int SubNivel { get; set; }
        public string Detalle_Tipo_Aprobacion { get; internal set; }
        public string Documento_Empleado { get; set; }
        public string Cod_Colaborador_Empleado { get; set; }
        public string Documento_Aprobador { get; set; }
        public string CareerID_Aprobador { get; set; }
  }


    public class UpdateAprobadoreVm
    {
        public List<AprobadoresViewModel> upsert { get; set; }
        public List<AprobadoresViewModel> delete { get; set; }
    }

  public class Soliciud_Certtificado_Laboral_VM
  {
    public string mensaje { get; set; }
  }

    public class AprobadoresDataVm
    {
        public int Cod_Aprobador { get; set; }
        public Int16? Cod_Empleado { get; set; }
        public string Filtro { get; set; }
        public short Cod_Filtro { get; set; }
        public string Tipo_Aprobacion { get; set; }
        public int Niveles { get; set; }
        public List<short> Cod_Aprobadores { get; set; }
    }


  public class PDF_Solicitud_Vacaciones
  {
    public string Empresa { get; set; }
    public int Cod_Solicitud { get; set; }
    public string Tipo_Solicitud { get; set; }
  }
}
