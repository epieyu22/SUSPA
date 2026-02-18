using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JulianaWeb.Models.Juliana.Generals
{
  public class PARAMETROS_GENERALES
  {
    #region Columnas

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public string Cod_Parametro { get; set; }

    public string Valor { get; set; }

    public string Descripcion { get; set; }

    #endregion
  }
}
