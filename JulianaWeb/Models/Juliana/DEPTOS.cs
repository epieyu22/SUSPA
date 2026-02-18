namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DEPTOS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Depto { get; set; }

        [Required]
        [StringLength(30)]
        public string Nombre_Depto { get; set; }

        public int Num_Personas { get; set; }

        [Required]
        [StringLength(30)]
        public string Jefe { get; set; }

        [Required]
        [StringLength(20)]
        public string Cod_Alterno { get; set; }

        public short Cod_Secundario { get; set; }

        public short Turnos { get; set; }

        [Required]
        [StringLength(50)]
        public string Numero_Alterno { get; set; }
    }
}
