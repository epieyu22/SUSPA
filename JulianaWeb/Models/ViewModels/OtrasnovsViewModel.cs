using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
  public class OtrasnovsViewModel
  {
    public short Cod_Empleado { get; set; }
    public short Cod_Concepto { get; set; }
    public string Empleado { get; set; }
    public string Concepto { get; set; }
    public string Fecha { get; set; }
    public Double Val_OtrasNov { get; set; }
    public string Porc_OtrasNov { get; set; }
    public string Prioridad { get; set; }
    public short Coutas { get; set; }
    public string Tipo { get; set; }
    public string Devengo { get; internal set; }
    public string Nom_Sucursal { get; internal set; }
    public string Nom_Ccosto { get; internal set; }
    public string Documento { get; internal set; }
  }


  public class SolPersonalVM
  {
    public string Cod_Motivo { get; internal set; }
    public short Cod_Cargo { get; internal set; }
    public short Cod_Zona { get; internal set; }
    public short Cod_Sucursal { get; internal set; }
    public short Cod_Depto { get; internal set; }
    public string Descripcion { get; internal set; }
    public short Cod_Solicitante { get; internal set; }
  }
}
