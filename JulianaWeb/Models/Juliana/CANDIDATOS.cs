namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CANDIDATOS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Candidato { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Requerimiento { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string Fec_Cita { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string Hora_Cita { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(8)]
        public string Fecha_Llegada { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string Hora_Llegada { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cantidad_Entrevistas { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Calificacion1 { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Calificacion2 { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "text")]
        public string Comentario { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(8)]
        public string Fecha_Aprobacion { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Usuario { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(1)]
        public string Estado { get; set; }

        [Key]
        [Column(Order = 13)]
        public double Beneficios { get; set; }

        [Key]
        [Column(Order = 14)]
        public double Salario { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Dias_Contrato { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(1)]
        public string Tipo_Contrato { get; set; }
    }
}
