using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JulianaWeb.Models
{
  public partial class STATUS_NOM_ELEC
  {
    [Key]
    public int id { get; set; }

    public int Cod_Concepto { get; set; }

    public string Xml { get; set; }

    public string Pdf { get; set; }

    [Column("Cune", Order = 5)]
    public string Cune { get; set; }

    public short Number { get; set; }

    public string Qrcode { get; set; }

    public string DianStatus { get; set; }

    public string EmailStatus { get; set; }

    public string Fec_Nomina { get; set; }

    public int Consecutivo { get; set; }

    public string Prefix { get; set; }

    [NotMapped]
    public STATUS_NOM_ELEC Registro_Antiguo { get; set; }
  
  }
}
