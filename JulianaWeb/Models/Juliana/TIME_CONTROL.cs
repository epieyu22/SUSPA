namespace JulianaWeb.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class TIME_CONTROL
  {


    public int Cod_Empleado { get; set; }

    public DateTime? Fecha_Turno { get; set; }

    public string Turno_Proyectado { get; set; }

    public string Turno_Real { get; set; }

    public string Hora_Ent { get; set; }

    public string Hora_Sal { get; set; }

    public string Hora_Ent_Real { get; set; }

    public string Hora_Sal_Real { get; set; }

    public string Tiempo_Retardo { get; set; }

    public string Tiempo_Laborado { get; set; }

    public int Cod_Observacion { get; set; }

    public char Tipo { get; set; }

    public char Estado { get; set; }

    public DateTime Fec_Nomina { get; set; }

    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int AutoNum { get; set; }


  }
}
