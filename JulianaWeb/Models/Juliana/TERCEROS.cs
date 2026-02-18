using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JulianaWeb.Models
{
  
  public partial class TERCEROS
  {
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public short Cod_Tercero { get; set; }


    [Column(Order = 1)]
    [StringLength(15)]
    public string Documento { get; set; }


    [Column(Order = 2)]
    [StringLength(2)]
    public string Dv { get; set; }


    [Column(Order = 3)]
    [StringLength(2)]
    public string Tipo_Documento { get; set; }


    [Column(Order = 4)]
    [StringLength(10)]
    public string Tipo_Tercero { get; set; }


    [Column(Order = 5)]
    [StringLength(30)]
    public string PNombre { get; set; }


    [Column(Order = 6)]
    [StringLength(30)]
    public string SNombre { get; set; }


    [Column(Order = 7)]
    [StringLength(30)]
    public string PApellido { get; set; }


    [Column(Order = 8)]
    [StringLength(30)]
    public string SApellido { get; set; }


    [Column(Order = 9)]
    [StringLength(120)]
    public string Tercero { get; set; }


    [Column(Order = 10)]
    [StringLength(100)]
    public string Dir_Elec { get; set; }

    [Column(Order = 11)]
    public string Cargo { get; set; }

    [Column(Order = 12)]
    public string CareerID { get; set; }
  }
}
