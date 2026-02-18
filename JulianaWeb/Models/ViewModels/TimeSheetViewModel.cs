using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
  public class TimeSheetViewModel
  {
    public int num_horas { get; set; }
    public int Id_Area { get; set; }
    public int Id_Concepto { get; set; }
    public DateTime fecha { get; set; }
    public string Descripcion { get; set; }

    public int numReq { get; set; }
    public int Id_Cliente { get; set; }


  }
}
