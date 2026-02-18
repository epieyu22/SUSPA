namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CIUDADES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Codigo { get; set; }

        [Required]
        [StringLength(2)]
        public string Codigo_Departamento { get; set; }

        [Required]
        [StringLength(8)]
        public string Codigo_Dane { get; set; }

        [Required]
        [StringLength(15)]
        public string Nom_Ciudad { get; set; }
    }
}
