namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class THS_PARAMETRO_GENERAL
    {
        [Key]
        public int Id_Parametro { get; set; }

        public int Id_Empleado { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; }

        public int ValorParametro { get; set; }

        public bool Estado { get; set; }
    }
}
