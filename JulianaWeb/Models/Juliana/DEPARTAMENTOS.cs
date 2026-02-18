namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DEPARTAMENTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Codigo { get; set; }

        [Required]
        [StringLength(4)]
        public string Codigo_Dane { get; set; }

        [Required]
        [StringLength(15)]
        public string Nom_Depto { get; set; }
    }
}
