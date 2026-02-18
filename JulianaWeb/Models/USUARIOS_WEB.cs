namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USUARIOS_WEB
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(12)]
        public string Usuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Clave { get; set; }
    }
}
