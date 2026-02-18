namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VARIABLES_VACACIONES
  {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Variable { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Descripcion { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string Tipo_Dato { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Tamano { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Uso { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string UsaConcepto { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Concepto { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool Obligatoria { get; set; }
    }
}
