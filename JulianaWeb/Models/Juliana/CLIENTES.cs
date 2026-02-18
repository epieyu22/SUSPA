namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CLIENTES
    {
        [Key]
        public int Id_Cliente { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }

        public int Id_Sec { get; set; }
    }
}
