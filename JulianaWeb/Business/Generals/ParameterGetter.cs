using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JulianaWeb.Models;
using JulianaWeb.Models.Juliana.Generals;

namespace JulianaWeb.Business.Generals
{
  // TODO: Deuda tecnica - se debe extraer interfaz
  public class ParameterGetter
  {
    public PARAMETROS_GENERALES Get(string code)
    {
      JulianaContext db = new JulianaContext();

      return db.PARAMETROS_GENERALES.FirstOrDefault(p => p.Cod_Parametro == code);
    }

    public IQueryable<PARAMETROS_GENERALES> GetLike(string code)
    {
      JulianaContext db = new JulianaContext();

      return db.PARAMETROS_GENERALES
                .Where(p => p.Cod_Parametro.ToLower().Contains(code.ToLower()));
    }
  }
}
