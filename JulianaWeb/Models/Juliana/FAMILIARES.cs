namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FAMILIARES
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_Empleado { get; set; }

  
        [StringLength(45)]
        public string Nombre { get; set; }


        [StringLength(25)]
        public string Parentesco { get; set; }

     
        [StringLength(3)]
        public string Edad { get; set; }

  
        [StringLength(35)]
        public string Profesion { get; set; }

        [StringLength(35)]
        public string Empresa { get; set; }

        [StringLength(25)]
        public string Cargo { get; set; }

        [StringLength(8)]
        public string FechaNacimiento { get; set; }

 
        [StringLength(20)]
        public string PNombre { get; set; }

  
        [StringLength(30)]
        public string SNombre { get; set; }

 
        [StringLength(20)]
        public string PApellido { get; set; }


        [StringLength(30)]
        public string SApellido { get; set; }


        [StringLength(12)]
        public string Cedula { get; set; }

 
        [StringLength(1)]
        public string Tipo_Documento { get; set; }


        public double Upc { get; set; }
    }
}
