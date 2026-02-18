namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class COMPETENCIAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Competencia { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom_Competencia { get; set; }
    }
}
