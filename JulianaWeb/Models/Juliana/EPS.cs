namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EPS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Eps { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom_Eps { get; set; }

        [StringLength(9)]
        public string Nit_Eps { get; set; }

        [StringLength(1)]
        public string Digito_Verificacion { get; set; }

        [Required]
        [StringLength(40)]
        public string Dir_Eps { get; set; }

        [Required]
        [StringLength(10)]
        public string Tel1_Eps { get; set; }

        [Required]
        [StringLength(10)]
        public string Tel2_Eps { get; set; }

        [Required]
        [StringLength(10)]
        public string Fax_Eps { get; set; }

        [Required]
        [StringLength(30)]
        public string Contacto_Eps { get; set; }

        public int Tot_Afiliados { get; set; }

        [StringLength(6)]
        public string Cod_Super { get; set; }
    }
}
