namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VACACIONES
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Periodo { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short SubPeriodo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string Desde { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(8)]
        public string Hasta { get; set; }

        [Key]
        [Column(Order = 5)]
        public float Dias_Tiempo { get; set; }

        [Key]
        [Column(Order = 6)]
        public double Liq_Vacaciones { get; set; }

        [Key]
        [Column(Order = 7)]
        public float Dias_Disponibles { get; set; }

        [Key]
        [Column(Order = 8)]
        public float Dias_Anticipados { get; set; }

        [Key]
        [Column(Order = 9)]
        public float Dias_Dinero { get; set; }

        [StringLength(8)]
        public string Fec_Aprobado { get; set; }

        [StringLength(8)]
        public string Fec_Desaprobado { get; set; }

        [StringLength(1)]
        public string Estado { get; set; }

        [StringLength(8)]
        public string Fec_Pago { get; set; }

        public short? Cod_Usuario { get; set; }

        [StringLength(1)]
        public string Clase { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(1)]
        public string Normales { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string Descuentos { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(8)]
        public string Fec_Cierre { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(1)]
        public string Novedades { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(1)]
        public string ProporcionalesP { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(10)]
        public string NumComprobante { get; set; }

    [StringLength(1)]
    public string Util_A { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AutoNum { get; set; }
  }
}
