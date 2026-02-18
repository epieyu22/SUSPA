namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BANCOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Banco { get; set; }

        [Required]
        [StringLength(25)]
        public string Banco { get; set; }

        [StringLength(15)]
        public string Cta_Bancaria { get; set; }

        [Required]
        [StringLength(8)]
        public string Ult_Cheque { get; set; }

        [StringLength(8)]
        public string Ult_PagoElectronico { get; set; }

        [StringLength(10)]
        public string Cta_Contable { get; set; }

        [Required]
        [StringLength(25)]
        public string Contacto { get; set; }

        [Required]
        [StringLength(7)]
        public string Telefono { get; set; }

        [StringLength(1)]
        public string Tipo_Banco { get; set; }

        [StringLength(15)]
        public string Cod_Alterno { get; set; }

        [StringLength(1)]
        public string Tipo_Cta { get; set; }

        [Required]
        [StringLength(8)]
        public string Ult_PagoTarjeta { get; set; }

        [Required]
        [StringLength(1)]
        public string Tipo_Cta_Index { get; set; }

        [Required]
        [StringLength(15)]
        public string Cta_Bancaria_Index { get; set; }

        [StringLength(10)]
        public string Cta_PorPagar { get; set; }

        [Required]
        [StringLength(9)]
        public string Nit_Banco { get; set; }

        [Required]
        [StringLength(1)]
        public string Digito_Verificacion { get; set; }

        [Required]
        [StringLength(7)]
        public string Cod_Alterno_Bancolombia { get; set; }
    }
}
