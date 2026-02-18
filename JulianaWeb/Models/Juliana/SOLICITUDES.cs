namespace JulianaWeb.Models
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public partial class SOLICITUDES
  {
    [Key]
    public int Cod_Solicitud { get; set; }
    public short Cod_Empleado { get; set; }

    [Required]
    [StringLength(2)]
    public string Tipo_Solicitud { get; set; }
    public short Cod_Concepto { get; set; }
    public short Cod_Aprobador { get; set; }
    public DateTime Fec_Solicitud { get; set; }

    public int Cantidad { get; set; }

    [Required]
    [StringLength(2)]
    public string Estado { get; set; }

    public DateTime? Fec_Salida { get; set; }

    public DateTime? Fec_Llegada { get; set; }

    [StringLength(1)]
    public string Modo_Vacaciones { get; set; }

    public short? Cod_Motivo_Rechazo { get; set; }

    public string Observacion { get; set; }

    public string Descripcion { get; set; }

    [ForeignKey("Cod_Empleado")]
    public virtual EMPLEADOS empleado { get; set; }

    [ForeignKey("Cod_Aprobador")]
    public virtual TERCEROS aprobador { get; set; }

    [NotMapped]
    public string Nom_Concepto { get; set; }

    public string Adjunto { get; set; }
  }
}
