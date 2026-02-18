using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models.Juliana.NominaElectronica
{

  public class Comprobante
  {
    [JsonProperty("prefijo")]
    public string Prefijo { get; set; }

    [JsonProperty("numeroActual")]
    public int? NumeroActual { get; set; }
  }

  public class TipoComprobante
  {
    [JsonProperty("soporteNomina")]
    public Comprobante SoporteNomina { get; set; }

    [JsonProperty("notaAjusteRemplazo")]
    public Comprobante NotaAjusteRemplazo { get; set; }

    [JsonProperty("notaAjusteEliminacion")]
    public Comprobante NotaAjusteEliminacion { get; set; }
  }
}
