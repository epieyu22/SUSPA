namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ZONAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Zona { get; set; }

        [Required]
        [StringLength(25)]
        public string Nom_Zona { get; set; }

        public short? Tipo_Zona { get; set; }
    }
}
