namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PARAMETROS_CERTLAB
    {
        [Key]
        public int Cod_Parametros_Certlab { get; set; }

        [StringLength(15)]
        public string Filter { get; set; }

        public short? Cod_Filter { get; set; }

        public bool? Otros_Devengos { get; set; }

        public bool? Horas_Extras { get; set; }

        public bool? Base_Salario { get; set; }

        public bool? Otros_Ingresos { get; set; }

        [StringLength(250)]
        public string Conceptos { get; set; }

        public short? Meses_Promedio { get; set; }
        public string Plantilla { get; set; }

        [Column(TypeName = "text")]
        public string Texto_Embajada { get; set; }

        [Column(TypeName = "text")]
        public string Texto_Viaje_Laboral { get; set; }

        public bool? Firma_Digital { get; set; }

        public short? Cod_Empleado_Autoriza { get; set; }

        public string Firma_Filename { get; set; }
    }
}
