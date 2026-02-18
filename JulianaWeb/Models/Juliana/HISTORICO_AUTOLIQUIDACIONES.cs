namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HISTORICO_AUTOLIQUIDACIONES
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleador { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Concepto { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string Fec_Nomina { get; set; }

        [Key]
        [Column(Order = 4)]
        public double Val_Novedad { get; set; }

        [Key]
        [Column(Order = 5)]
        public double Val_IBC { get; set; }

        [Key]
        [Column(Order = 6)]
        public float Dias_Novedad { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Sub_Concepto { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Sub_Concepto1 { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(8)]
        public string Fec_Pago { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(30)]
        public string Num_Planilla { get; set; }
    }
}
