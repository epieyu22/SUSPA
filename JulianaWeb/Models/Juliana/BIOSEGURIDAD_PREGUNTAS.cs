namespace JulianaWeb.Models
{
  using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class BIOSEGURIDAD_PREGUNTAS
  {
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    
    [Required]
    public int id_bioseguirdad { get; set; }

    [Required]
    public string Cod_Empleado { get; set; }
    
    [Required]
    public int pregunta_1 { get; set; }

    [Required]
    public int pregunta_2 { get; set; }

    [Required]
    public int pregunta_3 { get; set; }

    [Required]
    public int pregunta_4 { get; set; }

    [Required]
    public int pregunta_5 { get; set; }

    [Required]
    public int pregunta_6 { get; set; }

    [Required]
    public int pregunta_7 { get; set; }

    [Required]
    public int pregunta_8 { get; set; }

    [Required]
    public int pregunta_9 { get; set; }

    [Required]
    public int pregunta_10 { get; set; }

    [Required]
    public string fecha { get; set; }
  
  }
}
