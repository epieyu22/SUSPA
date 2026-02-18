namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PARAMETROS_WEB
    {
        [Key]
        public int Cod_Parametros_Web { get; set; }

        public short? Ano_Compropago { get; set; }

        [StringLength(2)]
        public string Mes_Compropago { get; set; }

        public bool? Mostrar_Ano_Anterior { get; set; }

        public bool? Redondear_Renglones { get; set; }

        public bool? Unificar_Contratos { get; set; }

        public bool? Aprobar_Compropago { get; set; }

        public string Modo_Compropago { get; set; }
    }
}
