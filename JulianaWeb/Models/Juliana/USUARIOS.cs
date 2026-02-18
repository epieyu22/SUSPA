namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USUARIOS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Usuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Usuario { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string Nombre { get; set; }

        [StringLength(20)]
        public string Clave { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string Fec_Ingreso { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(8)]
        public string Fec_Ult_Acceso { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(8)]
        public string Fec_Actualizo_Clave { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(8)]
        public string Fec_Eliminado { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string Nivel { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string Mod_Nomina { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string Mod_Recursos { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(1)]
        public string Estado { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string AutorizaPagos { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(12)]
        public string Cedula { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(1)]
        public string AjustaProvisiones { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(1)]
        public string Tipousuario { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Val_Asignado { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(1)]
        public string VisualizaSalarios { get; set; }

        [Key]
        [Column("TipoUsuario", Order = 17)]
        [StringLength(1)]
        public string TipoUsuario1 { get; set; }
    }
}
