using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
  public class FacturacionViewModel
  {
    public string Empresa { get; set; }
    public string Cod_Archivo { get; set; }
    public EMPLEADOS empleado { get; set; }
    public List<EMPLEADOS> empleados { get; set; }
    public DateTime HistoricDate { get; set; }
    public string mes { get; set; }
    public string ano { get; set; }

  }

  public class EmpleadosListViewModel
  {
    public short Cod_Empleado { get; set; }
    public string Cedula { get; set; }
    public string MinFec { get; set; }
    public string MaxFec { get; set; }
    public string Empleado { get; set; }
    public double Salario { get; set; }
    public double Devengo { get; set; }
    public double Deduccion { get; set; }
    public double Total { get; set; }
  }

}
