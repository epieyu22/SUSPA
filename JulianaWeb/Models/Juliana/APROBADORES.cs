namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class APROBADORES
    {
        [Key]
        public int Cod_Aprobador { get; set; }

        public short Cod_Empleado { get; set; }

        [Required]
        [StringLength(1)]
        public string Filtro { get; set; }

        [Required]
        [StringLength(1)]
        public string Tipo_Aprobacion { get; set; }

        public short Cod_Filtro { get; set; }

        [Required]
        [StringLength(1)]
        public string Estado { get; set; }


        public string Nivel { get; set; }
        public string Sub_Nivel { get; set; }

        [ForeignKey("Cod_Filtro")]
        public virtual EMPLEADOS empleado { get; set; }

        [ForeignKey("Cod_Empleado")]
        public virtual TERCEROS tercero { get; set; }


    }
}
