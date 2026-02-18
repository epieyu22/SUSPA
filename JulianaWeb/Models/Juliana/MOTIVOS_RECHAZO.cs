namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MOTIVOS_RECHAZO
    {
        [Key]
        public short Cod_Motivo_Rechazo { get; set; }

        [StringLength(150)]
        public string Descripcion { get; set; }
    }
}
