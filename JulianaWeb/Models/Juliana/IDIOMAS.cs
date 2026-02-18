namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IDIOMAS
    {
        [Key]
        public short Cod_Idioma { get; set; }

        [StringLength(35)]
        public string Nom_Idioma { get; set; }

        [StringLength(3)]
        public string ISO639 { get; set; }
    }
}
