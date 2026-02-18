namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NOVAUT")]
    public partial class NOVAUT
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Concepto { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string Desde { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string Hasta { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Dias { get; set; }

        public short? Cod_Diag { get; set; }

        public short? Cod_Imagen { get; set; }

        public short? Cod_Usuario { get; set; }

        [StringLength(8)]
        public string Fec_Aprobado { get; set; }

        [StringLength(8)]
        public string Fec_Desaprobado { get; set; }

        [StringLength(1)]
        public string Estado { get; set; }

        [StringLength(15)]
        public string Num_Autoriza { get; set; }

        public double? Val_Novedad { get; set; }

        public short? Dias_Pag_100 { get; set; }

        public double? Val_Reembolso_Eps { get; set; }

        public double? IBC { get; set; }

        public short? Dias_Pag_100_Ant { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string NumComprobante { get; set; }

        [Key]
        [Column(Order = 6)]
        public double Horas { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal AutoNum { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Prorroga { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string DescuentaAuxilio { get; set; }

        public string Adjunto { get; set; }
  }
}
