namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class REFERENCIAS_PERSONALES
    {
        public short Cod_HojaVida { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Ocupacion { get; set; }

        [StringLength(14)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(1)]
        public string Verificado { get; set; }

        [Column(TypeName = "text")]
        public string Observaciones { get; set; }

        [Required]
        [StringLength(1)]
        public string Tipo_Referencia { get; set; }

        [Key]
        public int Cod_Referencia_Personal { get; set; }
    }
}
