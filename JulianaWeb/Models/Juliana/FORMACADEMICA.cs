namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FORMACADEMICA")]
    public partial class FORMACADEMICA
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Cod_HojaVida { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Institucion { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Titulo { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Nivel { get; set; }

        [Key]
        [Column(Order = 4)]
     
        public string Inicio { get; set; }

        [Key]
        [Column(Order = 5)]
   
        public string Salida { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Especialidad { get; set; }

        [Key]
        [Column(Order = 7)]
  
        public string Otra_Institucion { get; set; }

        [Key]
        [Column(Order = 8)]
      
        public string Otro_Titulo { get; set; }

        [Key]
        [Column(Order = 9)]

        public string Otra_Especialidad { get; set; }

        [Key]
        [Column(Order = 10)]
        public string Adjunto { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cod_FormAcademica { get; set; }

        
        [Column(Order = 12)]
        public string Estado { get; set; }
    }
}
