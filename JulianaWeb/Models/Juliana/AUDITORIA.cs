namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AUDITORIA")]
    public partial class AUDITORIA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Usuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string Fec_Transaccion { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(8)]
        public string Hora_Transaccion { get; set; }

        [StringLength(30)]
        public string Tabla { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string Accion { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "text")]
        public string Descripcion { get; set; }
    }
}
