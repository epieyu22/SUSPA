namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class APROBACIONES
    {
        [Key]
        [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id_Aprobacion { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Solicitud { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_Aprobador { get; set; }

        [Key]
        [Column(Order = 3)]
        public string Observaciones { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(8)]
        public string Fecha_Aprobado { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Depto_Sol { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Nivel { get; set; }
    }
}
