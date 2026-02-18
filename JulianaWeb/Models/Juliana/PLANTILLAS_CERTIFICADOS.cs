namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PLANTILLAS_CERTIFICADOS
    {
        [Key]
        [Column(Order = 0)]
        public int Autonum { get; set; }

        [Column(Order = 1)]

        public string Plantilla { get; set; }

       
        [Column(Order = 2)]
        [StringLength(100)]
        public string Nomb_Plantilla { get; set; }

     
        [Column(Order = 3)] 
        public bool Aprobacion { get; set; }

        
        [Column("Tipo_plantilla")]
        public string Tipo_plantilla { get; set; }
  }
}
