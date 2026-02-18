using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
  public class IncapacidadesViewModel
  {
    public string Empleado { get; set; }
    public string Concepto { get; set; }
    public short Dias { get; set; }
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
    public string Adjunto { get; set; }
    public string Estado { get; set; } = "";
    public int Cod_Novaut { get; set; } = -1;
  }

  public class NewIncapacidadesViewModel{
    public short Cod_Concepto { get; set; }
    public short Cod_Diagnostico { get; set; }
    public short Cod_Aprobador { get; set; }
    public short Cod_Empelado { get; set; }
    public short Dias { get; set; }
    public DateTime Desde { get; set;}

    public string Descripcion { get; set; }
    public string Nom_Diagnostico { get; set; }
  }
}
