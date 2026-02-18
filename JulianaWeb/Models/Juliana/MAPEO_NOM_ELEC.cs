namespace JulianaWeb.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class MAPEO_NOM_ELEC
  {
    [Key]
    public string Cod_Concepto { get; set; }
    public int Cod_Pt { get; set; }
    public string Dian_xml { get; set; }
    public string Cod_Alterno { get; set; }
    public string Cod_Alterno2 { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fec_vigencia { get; set; }
    public char Estado { get; set; }
    public int Cod_Tipo { get; set; }

    [ForeignKey("Cod_Tipo")]
    public virtual TIPO_MAPEO Tipo_Mapeo { get; set; }

  }

  public partial class TIPO_MAPEO
  {
    [Key]
    public int Cod_Tipo { get; set; }
    public string Nombre { get; set; }
  }

}
