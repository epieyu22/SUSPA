namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class THS_AREA_CONCEPTOS
    {
        [Key]
        public int Id_Concepto { get; set; }

        public int Id_Area { get; set; }

        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }
    }
}
