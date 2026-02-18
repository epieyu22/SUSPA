namespace JulianaWeb.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class THS_HORAS
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int Id_Reporte { get; set; }
    public int Id_Empleado { get; set; }

    public int Id_Cliente { get; set; }
    public string Fecha { get; set; }
    public int Cant_Horas { get; set; }
    public int Id_Area { get; set; }
    public int Id_Concepto { get; set; }
    public string Descripcion { get; set; }

    public int? Req { get; set; }

    [ForeignKey("Id_Cliente")]
    public virtual CLIENTES clientes { get; set; }

    [ForeignKey("Id_Area")]
    public virtual THS_AREA areas { get; set; }

    [ForeignKey("Id_Concepto")]
    public virtual THS_AREA_CONCEPTOS conceptos { get; set; }

  }
}
