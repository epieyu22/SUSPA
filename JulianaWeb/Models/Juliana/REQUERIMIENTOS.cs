namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class REQUERIMIENTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Requerimiento { get; set; }

        public short Cod_Usuario { get; set; }

        [Required]
        [StringLength(8)]
        public string Fecha_Req { get; set; }

        [Required]
        [StringLength(5)]
        public string Hora_Req { get; set; }

        [Required]
        [StringLength(1)]
        public string Cod_Motivo { get; set; }

        public short Cod_Cargo { get; set; }

        public short Cod_Perfil { get; set; }

        [Required]
        [StringLength(8)]
        public string Fecha_Ing_Aprox { get; set; }

        public short Puntaje1 { get; set; }

        public short Puntaje2 { get; set; }

        public short Cod_Zona { get; set; }

        public short Cod_Sucursal { get; set; }

        public short Cod_Ccosto { get; set; }

        public short Cod_Depto { get; set; }

        [Required]
        [StringLength(8)]
        public string Fecha_Evento { get; set; }

        [Required]
        [StringLength(8)]
        public string Fecha_Cierre { get; set; }

        [Required]
        [StringLength(1)]
        public string Estado { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Descripcion { get; set; }

        public short Cod_Solicitante { get; set; }
    }
}
