using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
  public class TurnosViewModel
  {
    public int Cod_Empleado { get; set; }

    public string Cod_Tipo{ get; set; }

    public short Cod_Depto { get; set; }
    
    public DateTime Fecha_Turno { get; set; }

    public DateTime Fecha_Turno_Hasta { get; set; }

    public string Turno_Proyectado { get; set; }

    public bool Manual { get; set; }

    public string Turno_Real { get; set; }

    public string Hora_Ent { get; set; }

    public string Hora_Sal { get; set; }

    public string Hora_Ent_Real { get; set; }

    public string Hora_Sal_Real { get; set; }

    public string Tiempo_Retardo { get; set; }

    public string Tiempo_Laborado { get; set; }

    public int Cod_Observacion { get; set; }

    public char Tipo { get; set; }

    public char Estado { get; set; }

    public DateTime Fec_Nomina { get; set; }

    public int AutoNum { get; set; }

    public List<int> Cod_Empleados { get; set; }


  }
}
