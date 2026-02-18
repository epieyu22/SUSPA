namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HISTORICO")]
    public partial class HISTORICO
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

        [Key]
        [Column(Order = 25)]
        public short Cod_Concepto { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string Fec_Novedad { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string Fec_Ing_Novedad { get; set; }

        [Key]
        [Column(Order = 3)]
        public double Val_Novedad { get; set; }

        [Key]
        [Column(Order = 4)]
        public float Dias_Novedad { get; set; }

        [Key]
        [Column(Order = 5)]
        public float Horas_Novedad { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Zona { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Sucursal { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Ccosto { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Depto { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Sub_Concepto { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(8)]
        public string Fec_Nomina { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(8)]
        public string Fec_Aprobado { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(10)]
        public string Cta_Contable { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Usuario { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(1)]
        public string Estado { get; set; }

        [StringLength(1)]
        public string Usado_BS { get; set; }

        [Key]
        [Column(Order = 16)]
        public float Porcentaje { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(20)]
        public string C1 { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(20)]
        public string C2 { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(20)]
        public string C3 { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(20)]
        public string C4 { get; set; }

        [Key]
        [Column(Order = 21)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Cargo { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(1)]
        public string ModuloPago { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(10)]
        public string NumAprobacion { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(10)]
        public string NumComprobante { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal AutoNum { get; set; }
    }
}
