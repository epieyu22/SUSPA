namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SUCURSALES
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Sucursal { get; set; }

        public short Cod_Zona { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom_Sucursal { get; set; }

        [Required]
        [StringLength(1)]
        public string Cumplio_Meta { get; set; }

        public short Cod_CajaComp { get; set; }

        [Required]
        [StringLength(2)]
        public string Sucursal_Sec { get; set; }

        [Required]
        [StringLength(1)]
        public string EsEmpresa { get; set; }

        [Required]
        [StringLength(9)]
        public string Nit_Sucursal { get; set; }

        [Required]
        [StringLength(1)]
        public string Digito_Verificacion { get; set; }
    }
}
