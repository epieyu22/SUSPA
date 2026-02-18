namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SOLICITUD_VACACIONES
    {
        [Key]
        public int Cod_Solicitud_Vacaciones { get; set; }

        public short Cod_Empleado { get; set; }

        public short Cod_Aprobador { get; set; }

        public DateTime Fec_Solicitud { get; set; }

        public DateTime Fec_Salida { get; set; }

        public DateTime Fec_Llegada { get; set; }

        [Required]
        [StringLength(2)]
        public string Modo_Vacaciones { get; set; }

        public short Cantidad_Dias { get; set; }

        [Required]
        [StringLength(2)]
        public string Estado { get; set; }

        public virtual EMPLEADOS EMPLEADOS { get; set; }
    }
}
