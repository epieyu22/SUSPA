namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CARGOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Cargo { get; set; }

        [Required]
        [StringLength(40)]
        public string Nom_Cargo { get; set; }

        public double Sue_Disponible { get; set; }

        public short Cod_Alterno { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Objetivo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Funciones { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Perfil { get; set; }

        [Required]
        [StringLength(1)]
        public string AutorizaSalarios { get; set; }

        [Required]
        [StringLength(1)]
        public string RecibeMail { get; set; }

        [Required]
        [StringLength(1)]
        public string TieneJefe { get; set; }

        public short Cod_CargoJefe { get; set; }

        public bool Firma_Doc { get; set; }

        public byte[] Firma_Digital { get; set; }

        [Required]
        [StringLength(50)]
        public string Docu_Firma { get; set; }

        [Required]
        [StringLength(4)]
        public string Cod_Oficio { get; set; }
    }
}
