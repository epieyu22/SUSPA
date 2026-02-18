namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OTRASNOV")]
    public partial class OTRASNOV
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

        public short? Cod_Zona { get; set; }

        public short? Cod_Sucursal { get; set; }

        public short? Cod_Ccosto { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Concepto { get; set; }

        [StringLength(1)]
        public string Prioridad { get; set; }

        [Key]
        [Column(Order = 2)]
        public float Porc_OtrasNov { get; set; }

        [Key]
        [Column(Order = 3)]
        public double Val_OtrasNov { get; set; }

        [Key]
        [Column(Order = 4)]
        public float Dias { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(8)]
        public string Vigencia { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string Tipo { get; set; }

        [StringLength(1)]
        public string Estado { get; set; }

        [StringLength(8)]
        public string Fec_Aprobado { get; set; }

        [StringLength(8)]
        public string Fec_Desaprobado { get; set; }

        [StringLength(1)]
        public string Aplica { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(8)]
        public string Fec_Ing_Novedad { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal AutoNum { get; set; }

        [Key]
        [Column(Order = 9)]
        public float Tipo_Especial { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(20)]
        public string NumCtaVoluntarios { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cuotas { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(8)]
        public string VigenciaDesde { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(2)]
        public string Fuente { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(20)]
        public string Cod_Usuario { get; set; }
    }
}
