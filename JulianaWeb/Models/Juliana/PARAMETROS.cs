namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PARAMETROS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Ano { get; set; }

        [Key]
        [Column(Order = 1)]
        public double Salmin { get; set; }

        [Key]
        [Column(Order = 2)]
        public double Salint { get; set; }

        [Key]
        [Column(Order = 3)]
        public double SubTrans { get; set; }

        [Key]
        [Column(Order = 4)]
        public float ApoSalud { get; set; }

        public float? ApoFondoSalud { get; set; }

        [Key]
        [Column(Order = 5)]
        public float ApoFondoPen { get; set; }

        [Key]
        [Column(Order = 6)]
        public float ApoFondoSolPen { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Arp { get; set; }

        public float? ApoFondoArp { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short LiqNomina { get; set; }

        [Key]
        [Column(Order = 9)]
        public float ApoPsalud { get; set; }

        [Key]
        [Column(Order = 10)]
        public float ApoPFondoPen { get; set; }

        [Key]
        [Column(Order = 11)]
        public float ApoPriesProf { get; set; }

        [Key]
        [Column(Order = 12)]
        public float ApoCajaCom { get; set; }

        [Key]
        [Column(Order = 13)]
        public float ApoIcbf { get; set; }

        [Key]
        [Column(Order = 14)]
        public float ApoSena { get; set; }

        [Key]
        [Column(Order = 15)]
        public float ACesantias { get; set; }

        [Key]
        [Column(Order = 16)]
        public float AintCesantias { get; set; }

        [Key]
        [Column(Order = 17)]
        public float Aprimas { get; set; }

        [Key]
        [Column(Order = 18)]
        public float Avacaciones { get; set; }

        [StringLength(8)]
        public string Fec_Inicial { get; set; }

        [StringLength(8)]
        public string Fec_Nomina { get; set; }

        [StringLength(1)]
        public string Tipo_Liquidacion { get; set; }

        public float? PorcSalNor { get; set; }

        public float? PorcSalInt { get; set; }

        public float? PorcSalEsc { get; set; }

        [StringLength(1)]
        public string TipoSabado { get; set; }

        public short? Codigo { get; set; }

        [StringLength(10)]
        public string CtaGtoCajas { get; set; }

        [StringLength(10)]
        public string CtaPasCajas { get; set; }

        [StringLength(10)]
        public string CtaGtoIcbf { get; set; }

        [StringLength(10)]
        public string CtaPasIcbf { get; set; }

        [StringLength(10)]
        public string CtaGtoSena { get; set; }

        [StringLength(10)]
        public string CtaPasSena { get; set; }

        [StringLength(10)]
        public string CtaGtoSalud { get; set; }

        [StringLength(10)]
        public string CtaPasSalud { get; set; }

        [StringLength(10)]
        public string CtaGtoPension { get; set; }

        [StringLength(10)]
        public string CtaPasPension { get; set; }

        [StringLength(10)]
        public string CtaGtoRiesProf { get; set; }

        [StringLength(10)]
        public string CtaPasRiesProf { get; set; }

        [StringLength(10)]
        public string CtaGtoCesantias { get; set; }

        [StringLength(10)]
        public string CtaPasCesantias { get; set; }

        [StringLength(10)]
        public string CtaGtoIntCesantias { get; set; }

        [StringLength(10)]
        public string CtaPasIntCesantias { get; set; }

        [StringLength(10)]
        public string CtaGtoPrimas { get; set; }

        [StringLength(10)]
        public string CtaPasPrimas { get; set; }

        [StringLength(10)]
        public string CtaGtoVacaciones { get; set; }

        [StringLength(10)]
        public string CtaPasVacaciones { get; set; }

        [StringLength(2)]
        public string Tipo_Interfase { get; set; }

        public int? DiasMaxHabiles { get; set; }

        public float? IntMora { get; set; }

        public short? Sal_Max_Asegur { get; set; }

        public double? ValMaxAnual { get; set; }

        public double? ValMaxMensual { get; set; }

        public float? PorcEduMensual { get; set; }

        public float? PorcPrimaSalNormal { get; set; }

        public float? PorcPrimaSalEscala { get; set; }

        [StringLength(10)]
        public string Hora_Ent { get; set; }

        [StringLength(10)]
        public string Hora_Sal { get; set; }

        [StringLength(10)]
        public string Tiempo_Alm { get; set; }

        public double? TopeMaxRentaExcenta { get; set; }

        public float? PorcMaxApoPen { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(1)]
        public string TipoLiqFestivos { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(1)]
        public string Corte30Retefuente { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(1)]
        public string Corte30SegSocial { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(8)]
        public string Fec_Acumulado { get; set; }

        [Key]
        [Column(Order = 23)]
        public double Val_Uvt { get; set; }

        [Key]
        [Column(Order = 24)]
        public double PptoAnualCap { get; set; }

        [Key]
        [Column(Order = 25)]
        [StringLength(1)]
        public string Promedio_Cesantias { get; set; }

        [Key]
        [Column(Order = 26)]
        [StringLength(1)]
        public string Corte30Provisiones { get; set; }

        [Key]
        [Column(Order = 27)]
        [StringLength(1)]
        public string BasePrimaSubsidio { get; set; }

        [Key]
        [Column(Order = 28)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Sal_Max_AsegurRie { get; set; }

        [Key]
        [Column(Order = 29)]
        [StringLength(1)]
        public string Calculo_Anticipo { get; set; }

        [Key]
        [Column(Order = 30)]
        [StringLength(1)]
        public string Pago_Vac_FecLlegada { get; set; }

        [Key]
        [Column(Order = 31)]
        [StringLength(1)]
        public string LogoSucursal { get; set; }

        [Key]
        [Column(Order = 32)]
        [StringLength(1)]
        public string DedFiscales { get; set; }

        [Key]
        [Column(Order = 33)]
        [StringLength(10)]
        public string CtaActGasAnt { get; set; }

        [Key]
        [Column(Order = 34)]
        [StringLength(1)]
        public string GastoAnticipadoSueldos { get; set; }

        [Key]
        [Column(Order = 35)]
        [StringLength(1)]
        public string DedFiscalSalud { get; set; }

        [Key]
        [Column(Order = 36)]
        [StringLength(30)]
        public string Servidor { get; set; }

        [StringLength(60)]
        public string Usuario { get; set; }

        [Key]
        [Column(Order = 37)]
        [StringLength(20)]
        public string Clave { get; set; }

        [Key]
        [Column(Order = 38)]
        [StringLength(6)]
        public string Puerto { get; set; }

        [Key]
        [Column(Order = 39)]
        [StringLength(60)]
        public string Remitente { get; set; }

        [Key]
        [Column(Order = 40)]
        [StringLength(400)]
        public string Mensaje { get; set; }

        [Key]
        [Column(Order = 41)]
        public float Por_Bonos { get; set; }

        [Key]
        [Column(Order = 42)]
        [StringLength(8)]
        public string CtaPasBonificaciones { get; set; }

        [Key]
        [Column(Order = 43)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TopeSalud { get; set; }

        [Key]
        [Column(Order = 44)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TopePension { get; set; }

        [Key]
        [Column(Order = 45)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TopeRiesgos { get; set; }

        [Key]
        [Column(Order = 46)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TopeCaja { get; set; }

        [Key]
        [Column(Order = 47)]
        [StringLength(1)]
        public string Vac100 { get; set; }

        [Key]
        [Column(Order = 48)]
        public float TopeMaximo { get; set; }

        [Key]
        [Column(Order = 49)]
        [StringLength(1)]
        public string DiasLic { get; set; }

        [Key]
        [Column(Order = 50)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short DiasCesantias { get; set; }

        [Key]
        [Column(Order = 51)]
        [StringLength(1)]
        public string Ley1429 { get; set; }

        [Key]
        [Column(Order = 52)]
        [StringLength(1)]
        public string UVTActual { get; set; }

        [Key]
        [Column(Order = 53)]
        [StringLength(1)]
        public string BaseCesantiaSubsidio { get; set; }

        [Key]
        [Column(Order = 54)]
        [StringLength(1)]
        public string SubParcial { get; set; }

        [Key]
        [Column(Order = 55)]
        [StringLength(1)]
        public string SubCompleto { get; set; }

        [Key]
        [Column(Order = 56)]
        [StringLength(1)]
        public string SubDiasParcial { get; set; }

        [Key]
        [Column(Order = 57)]
        [StringLength(1)]
        public string SubDiasCompleto { get; set; }

        [Key]
        [Column(Order = 58)]
        public double TopeMaxAportes { get; set; }

        [Key]
        [Column(Order = 59)]
        public double TopeMaxSalud { get; set; }

        [Key]
        [Column(Order = 60)]
        public double TopeMaxDependientes { get; set; }

        [Key]
        [Column(Order = 61)]
        [StringLength(1)]
        public string AportanteExoneradoCajaSalud { get; set; }

        [Key]
        [Column(Order = 62)]
        [StringLength(1)]
        public string RedoneaRetefuente { get; set; }

        [Key]
        [Column(Order = 63)]
        [StringLength(1)]
        public string SubPrimeraVariables { get; set; }

        [Key]
        [Column(Order = 64)]
        [StringLength(1)]
        public string DiasNoLabDerecho { get; set; }

        [Key]
        [Column(Order = 65)]
        [StringLength(1)]
        public string DiasTrans { get; set; }

        [Key]
        [Column(Order = 66)]
        [StringLength(1)]
        public string GastoVacacionesAnt { get; set; }

        [Key]
        [Column(Order = 67)]
        [StringLength(1)]
        public string DedSaludMesAct { get; set; }

        [Key]
        [Column(Order = 68)]
        [StringLength(1)]
        public string RedondeoSaludPension { get; set; }

        [Key]
        [Column(Order = 69)]
        [StringLength(1)]
        public string UsaConceptosSena { get; set; }

        [Key]
        [Column(Order = 70)]
        public bool SSL { get; set; }

        [Key]
        [Column(Order = 71)]
        [StringLength(1)]
        public string LiquidaDiasDerecho { get; set; }

        [Key]
        [Column(Order = 72)]
        [StringLength(1)]
        public string Dia31 { get; set; }

        [Key]
        [Column(Order = 73)]
        [StringLength(1)]
        public string IbcAnt { get; set; }
    }
}
