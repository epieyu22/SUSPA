namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CAJASCOMP")]
    public partial class CAJASCOMP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Codigo { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom_Caja { get; set; }

        [StringLength(30)]
        public string Contacto { get; set; }

        [StringLength(30)]
        public string Direccion { get; set; }

        public float? TotAfiliados { get; set; }

        [StringLength(10)]
        public string Tel1 { get; set; }

        [StringLength(10)]
        public string Tel2 { get; set; }

        [StringLength(10)]
        public string Fax { get; set; }

        [Required]
        [StringLength(1)]
        public string Dig_Verificacion { get; set; }

        [Required]
        [StringLength(6)]
        public string Cod_Super { get; set; }
    }
}
