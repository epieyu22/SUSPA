using System;
using System.Collections.Generic;

namespace JulianaWeb.Models
{
  public class GetReporteAprobadoresResponse
  {
    public List<AprobadoresViewModel> Items { get; set; }
    public int TotalRows { get; set; }
  }
}
