namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PROFESIONES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Profesion { get; set; }

        [Required]
        [StringLength(30)]
        public string Descripcion { get; set; }
    }
}
