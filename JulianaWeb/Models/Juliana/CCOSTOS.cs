namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CCOSTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Ccosto { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom_Ccosto { get; set; }

        [StringLength(10)]
        public string Ccosto_Sec { get; set; }

        [Required]
        [StringLength(6)]
        public string Ppto_SAP { get; set; }

        [Required]
        [StringLength(12)]
        public string PMP { get; set; }

        [Required]
        [StringLength(10)]
        public string ProfitCenter { get; set; }

        [Required]
        [StringLength(1)]
        public string Sucursal { get; set; }
    }
}
