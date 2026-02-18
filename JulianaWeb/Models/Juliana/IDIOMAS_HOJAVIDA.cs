namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IDIOMAS_HOJAVIDA
    {
        [Key]
        public short Cod_Idioma_Hojavida { get; set; }

        public short? Cod_HojaVida { get; set; }

        public short? Cod_Idioma { get; set; }

        public string Nivel_Idioma { get; set; }
    }
}
