using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models.Juliana
{
  public class CUMPLEANOS
  {
    public CUMPLEANOS(string empleado, DateTime cumpleanos)
    {
      Empleado = empleado;
      Cumpleanos = cumpleanos;
    }

    public string Empleado { get; set; }
    public DateTime Cumpleanos { get; set; }
  }
}
