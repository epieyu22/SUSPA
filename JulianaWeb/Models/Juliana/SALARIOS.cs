namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SALARIOS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

        [Key]
        [Column(Order = 1)]
        public double Salario { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string Fec_Salario { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string Aprobado { get; set; }

        public float? Por_Aumento { get; set; }

        [Key]
        [Column(Order = 4)]
        public double Aum_Propuesto { get; set; }

        

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string Mot_Cambio_Sal { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Por_Salario { get; set; }

        [Key]
        [Column(Order = 8)]
        public double Porc_Salario { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(8)]
        public string FechaAplicaRetroactivo { get; set; }

        [Key]
        [Column(Order = 10)]
        public double Beneficios { get; set; }

        [Column(Order = 11)]        
        public decimal? AutoNum { get; set; }
  }
}
