namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EXPLABORAL")]
    public partial class EXPLABORAL
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Cod_HojaVida { get; set; }

        
        [Column(Order = 2)]
        public string Empresa { get; set; }

        [Column(Order = 3)]
        public string Telefono { get; set; }

        
        [Column(Order = 4)]
        public string Industria { get; set; }

        
        [Column(Order = 5)]
        public string Sector { get; set; }

        
        [Column(Order = 6)]
        public string Cargo { get; set; }

        
        [Column(Order = 7)]
        public string AreaTrabajo { get; set; }

        
        [Column(Order = 8)]
        public string Fec_Ingreso { get; set; }

        
        [Column(Order = 9)]
        public string Fec_Retiro { get; set; }

        
        [Column(Order = 10)]
        public string Funciones { get; set; }

        
        [Column(Order = 11)]
        public string Jefe_Inmediato { get; set; }

        [Key]
        [Column(Order = 12)]
        public string Logros { get; set; }

        
        [Column(Order = 13)]
        public string Opinion { get; set; }

        [Key]
        [Column(Order = 14)]
        public string Verificado { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cod_ExpLaboral { get; set; }
    }
}
