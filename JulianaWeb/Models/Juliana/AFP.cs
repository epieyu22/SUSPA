namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AFP")]
    public partial class AFP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Afp { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom_Afp { get; set; }

        [StringLength(9)]
        public string Nit_Afp { get; set; }

        [StringLength(1)]
        public string Digito_Verificacion { get; set; }

        [Required]
        [StringLength(40)]
        public string Dir_Afp { get; set; }

        [Required]
        [StringLength(10)]
        public string Tel1_Afp { get; set; }

        [Required]
        [StringLength(10)]
        public string Tel2_Afp { get; set; }

        [Required]
        [StringLength(10)]
        public string Fax_Afp { get; set; }

        [Required]
        [StringLength(30)]
        public string Contacto_Afp { get; set; }

        public int Tot_Afiliados { get; set; }

        [StringLength(6)]
        public string Cod_Super { get; set; }

        public float? Porc_Pension { get; set; }

        public float Porc_Fondo { get; set; }

        public float Porc_Comision { get; set; }

        public double Tope_Seguro { get; set; }

        public float Porc_Seguro { get; set; }
    }
}
