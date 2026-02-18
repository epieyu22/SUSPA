namespace JulianaWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HOJAVIDA")]
    public partial class HOJAVIDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Cod_HojaVida { get; set; }
        
        public string PNombre { get; set; }

        public string SNombre { get; set; }
                
        public string PApellido { get; set; }
                
        public string SApellido { get; set; }
        
        public string Aspirante { get; set; }
        
        public string Doc_Identidad { get; set; }

        public short Cod_Nacionalidad { get; set; }

        
        public string Fec_Nacimiento { get; set; }
        
        public string Sexo { get; set; }

        public string Lib_Militar { get; set; }
        
        public string Distrito { get; set; }
        
        public string Est_Civil { get; set; }

        public string Direccion { get; set; }

        public short Cod_Pais { get; set; }
        
        public string Dir_Electronica { get; set; }

        public short Cod_Ciudad { get; set; }
        

        public string Tel1 { get; set; }

        public string Tel2 { get; set; }
        

        public string Celular { get; set; }

        public double Asp_Salarial { get; set; }
        

        public string PViaje { get; set; }
        
        

        public string PTraslado { get; set; }

        public short Cod_Profesion { get; set; }
        
        public string Idioma_Nativo { get; set; }
        
        public string Idioma1 { get; set; }

        public float Porc_Conocimiento_Idioma1 { get; set; }
        
        public string Idioma2 { get; set; }

        public float Porc_Conocimiento_Idioma2 { get; set; }
        
        public string Estudiante_Practica { get; set; }

        public int Anos_Experiencia { get; set; }
        
        public string Fec_Creacion { get; set; }
        
        public string Fec_Actualizacion { get; set; }

        public short Cod_Cargo_Aspira { get; set; }

        public string Sketch { get; set; }

        public short Cod_Personalidad { get; set; }
        
        public string Tipo_Documento { get; set; }
        
        public string Estado { get; set; }

        public short Cod_Lugar_Expedicion { get; set; }
        
        public string Nivel_Idioma1 { get; set; }
        
        public string Nivel_Idioma2 { get; set; }
        
        
        public string Unidad_Tiempo { get; set; }

        public string Imagen_Perfil { get; set; }
    }
}
