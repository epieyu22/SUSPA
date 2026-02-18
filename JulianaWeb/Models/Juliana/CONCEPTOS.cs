namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CONCEPTOS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Concepto { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string Nom_Concepto { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string Devengo { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string BSCesantias { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string BSIntCesantia { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string BSPension { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string BSPrima { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string BSRetefuente { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string BSSalud { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string BSVacaciones { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(1)]
        public string BSSueldo { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string BSIncapacidad { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(1)]
        public string AfectaPres { get; set; }

        [Key]
        [Column(Order = 13)]
        public double Tot_Concepto { get; set; }

        [StringLength(10)]
        public string Cta { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(1)]
        public string Tipo_Concepto { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(1)]
        public string Acumula { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(1)]
        public string Jornada { get; set; }

        [StringLength(1)]
        public string BSAuxTransporte { get; set; }

        [StringLength(1)]
        public string BSIcbf { get; set; }

        [StringLength(1)]
        public string BSSena { get; set; }

        [StringLength(1)]
        public string BSCajaCompensacion { get; set; }

        [StringLength(1)]
        public string BSRiesgosProfesionales { get; set; }

        [StringLength(1)]
        public string BSPrimaVacaciones { get; set; }

        [StringLength(1)]
        public string BSIngresoTotal { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(1)]
        public string Aplica_Quincena { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(1)]
        public string Estado { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(1)]
        public string BSBenSalario { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(1)]
        public string BSBonoRetefuente { get; set; }

        [Key]
        [Column(Order = 21)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short UReportes { get; set; }

        [Key]
        [Column(Order = 22)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_ConceptoRelacion { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(1)]
        public string BSVacacionesDinero { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(1)]
        public string BSAuxilioPromedio { get; set; }

        [Key]
        [Column(Order = 25)]
        [StringLength(2)]
        public string CertificadoIngresos { get; set; }

        [Key]
        [Column(Order = 26)]
        [StringLength(1)]
        public string BSTopeExonerado { get; set; }

        [Key]
        [Column(Order = 27)]
        public double Por_Concepto { get; set; }

        [Key]
        [Column(Order = 28)]
        [StringLength(1)]
        public string DedSaludMesAct { get; set; }

        [Key]
        [Column(Order = 29)]
        [StringLength(1)]
        public string BSNoDescuentaDomingo { get; set; }

        [Key]
        [Column(Order = 30)]
        [StringLength(1)]
        public string BSTopeLey1393 { get; set; }

        [Key]
        [Column(Order = 31)]
        public bool OtrosIngCertLab { get; set; }

        [Key]
        [Column(Order = 32)]
        [StringLength(1)]
        public string BSBonoDesempeno { get; set; }


    [StringLength(1)]
    public string OrdenDevengo { get; set; }
  }
}
