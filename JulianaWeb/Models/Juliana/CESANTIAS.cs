namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CESANTIAS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string Periodo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string Desde { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string Hasta { get; set; }

        public int? Dias_Base_Acumulados { get; set; }

        public double? Salario_Base_Acumulado { get; set; }

        [Key]
        [Column(Order = 4)]
        public double Valor { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(8)]
        public string Fec_Aprobado { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(8)]
        public string Fec_Desaprobado { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string Estado { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(8)]
        public string Fec_Liquidacion { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(10)]
        public string Resolucion { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(8)]
        public string Fec_Resolucion { get; set; }

        [StringLength(1)]
        public string Regimen { get; set; }

        [StringLength(1)]
        public string Tipo_Registro { get; set; }

        [StringLength(8)]
        public string Fec_Ing_Novedad { get; set; }

    
    public short Cod_Usuario { get; set; }
  }
}
