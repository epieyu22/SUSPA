namespace JulianaWeb.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EMPRESAS
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Codigo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string Tipo_Documento { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(9)]
        public string Num_Documento { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(1)]
        public string Digito_Verificacion { get; set; }

        [StringLength(70)]
        public string Nombre_Empresa { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(40)]
        public string Direccion { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Depto { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Ciudad { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(10)]
        public string Tel { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(10)]
        public string Fax { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Arp { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Sucursal_Pag { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string Clase_Aportante { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(1)]
        public string Forma_Presenta { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(8)]
        public string Fec_Instalacion { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(8)]
        public string Fec_Ult_Acceso { get; set; }

        [Key]
        [Column(Order = 15)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Dias_Vigencia { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(8)]
        public string Fec_Vence_Licencia { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(10)]
        public string Clave { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(10)]
        public string Codigo_Habilitacion { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(1)]
        public string Cod_Pais { get; set; }

        [Key]
        [Column(Order = 20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Licencias { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(25)]
        public string Servidor { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(25)]
        public string BaseDatos { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(25)]
        public string Usuario { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(25)]
        public string Passw { get; set; }

        [StringLength(30)]
        public string Ruta { get; set; }

        [Key]
        [Column(Order = 25)]
        [StringLength(1)]
        public string ModNOM { get; set; }

        [Key]
        [Column(Order = 26)]
        [StringLength(1)]
        public string ModRH { get; set; }

        [Key]
        [Column(Order = 27)]
        [StringLength(1)]
        public string ModMIS { get; set; }

        [Key]
        [Column(Order = 28)]
        [StringLength(10)]
        public string Serial { get; set; }

        [Key]
        [Column(Order = 29)]
        [StringLength(20)]
        public string Cod_Local { get; set; }
        [JsonIgnore]
        public byte[] Logo { get; set; }

        [Key]
        [Column(Order = 30)]
        public string LogoOpc { get; set; }

        public bool? CargaLogoOpc { get; set; }

        [StringLength(1)]
        public string Estado { get; set; }
    }
}
