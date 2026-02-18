namespace JulianaWeb.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class TURNOS
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]

    public Int16 Cod_Turno { get; set; }

    public string Nom_Turno { get; set; }

    public string Hora_Ent { get; set; }

    public string Hora_Sal { get; set; }

    public string Tiempo_Alm { get; set; }

    public string Estado_Tur { get; set; }

    public string Tipo { get; set; }

    public string Recargo { get; set; }

    public Int16? Cod_Depto { get; set; }



  }
}
