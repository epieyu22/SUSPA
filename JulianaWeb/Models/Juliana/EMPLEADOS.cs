namespace JulianaWeb.Models
{
  using Newtonsoft.Json;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;
  using System.Data.SqlClient;

  public partial class EMPLEADOS
  {


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public short Cod_Empleado { get; set; }

    [Required]
    [StringLength(45)]
    public string Empleado { get; set; }

    [Required]
    [StringLength(15)]
    public string PNombre { get; set; }

    [Required]
    [StringLength(15)]
    public string SNombre { get; set; }

    [Required]
    [StringLength(15)]
    public string PApellido { get; set; }

    [Required]
    [StringLength(15)]
    public string SApellido { get; set; }

    [Required]
    [StringLength(1)]
    public string Tip_Documento { get; set; }

    [Required]
    [StringLength(12)]
    public string Cedula { get; set; }


    [StringLength(12)]
    public string Seguro_Soc { get; set; }

    [Required]
    [StringLength(8)]
    public string Fec_Nacimiento { get; set; }

    [Required]
    [StringLength(1)]
    public string Est_Civil { get; set; }

    [Required]
    [StringLength(1)]
    public string Sexo { get; set; }

    [Required]
    public string Direccion { get; set; }

    [Required]
    [StringLength(1)]
    public string Estado { get; set; }

    [Required]
    [StringLength(25)]
    public string Telefono { get; set; }

    [StringLength(14)]
    public string Celular { get; set; }

    [StringLength(12)]
    public string Lib_Militar { get; set; }

    [StringLength(3)]
    public string Distrito { get; set; }

    [StringLength(150)]
    public string Dir_Elec { get; set; }

    public short Cod_Profesion { get; set; }

    [Required]
    [StringLength(1)]
    public string Tipo_Contrato { get; set; }

    public short Dias_Contrato { get; set; }

    public short Periodo_Prueba { get; set; }

    [Required]
    [StringLength(1)]
    public string Tipo_Salario { get; set; }



    [Required]
    [StringLength(8)]
    public string Fec_Ingreso { get; set; }

    [Required]
    [StringLength(8)]
    public string Fec_Salario { get; set; }

    public double Salario { get; set; }

    [Required]
    [StringLength(1)]
    public string MetodoRetefuente { get; set; }

    public double Val_Retefuente { get; set; }

    public float Por_Retefuente { get; set; }

    [Required]
    [StringLength(1)]
    public string Tipo_Retefuente { get; set; }

    public double Deducible { get; set; }

    public double DedSaludEdu { get; set; }

    public double DedAFC { get; set; }

    public double DedAlimentacion { get; set; }

    [Required]
    [StringLength(1)]
    public string Regimen { get; set; }

    [StringLength(8)]
    public string Fec_Retiro { get; set; }

    public double Val_Liq_Contrato { get; set; }

    [Required]
    [StringLength(1)]
    public string FormaPago { get; set; }

    [Required]
    [StringLength(1)]
    public string Sabado { get; set; }

    public short Cod_Ciudad { get; set; }

    public short Cod_CajaCompensacion { get; set; }

    [Required]
    [StringLength(1)]
    public string Clasificacion_Contable { get; set; }

    [Required]
    [StringLength(1)]
    public string Tipo_Cta { get; set; }

    [Required]
    [StringLength(20)]
    public string Num_Cta { get; set; }

    [Required]
    [StringLength(15)]
    public string Banco { get; set; }

    public short Cod_Zona { get; set; }

    public short Cod_Sucursal { get; set; }

    public short Cod_Ccostos { get; set; }

    public short Cod_Depto { get; set; }

    public short Cod_Cargo { get; set; }

    public short Cod_Grupo { get; set; }

    public short Cod_Arp { get; set; }

    public short Cod_Eps { get; set; }

    public short Cod_Afp { get; set; }

    public short Cod_Afc { get; set; }

    public short Cod_Escala { get; set; }

    public short Cod_Tarifa_Plena { get; set; }


    [StringLength(1)]
    public string Modo_Costo_Hora { get; set; }

    public short Cod_Empleador { get; set; }


    [StringLength(1)]
    public string Aux_Transporte { get; set; }

    public short Cod_Pais_Nacimiento { get; set; }

    public short Cod_Pais_Nacionalidad { get; set; }

    [StringLength(8)]
    public string Fec_Vence_Visa { get; set; }

    public float Porc_Anticipo { get; set; }

    public float Porc_Anticipo_Prima { get; set; }

    [Required]
    [StringLength(1)]
    public string ModoSalario { get; set; }

    [StringLength(1)]
    public string AnticipoPrima { get; set; }

    public short Cod_NvaEps { get; set; }

    public short Cod_NvaAfp { get; set; }

    public short Cod_NvaAfc { get; set; }

    [StringLength(8)]
    public string Fec_CamEps { get; set; }

    [StringLength(8)]
    public string Fec_CamAfp { get; set; }

    [StringLength(8)]
    public string Fec_CamAfc { get; set; }

    [Required]
    [StringLength(1)]
    public string Tiempo_C { get; set; }

    [StringLength(10)]
    public string Clave { get; set; }

    public double Saldo_Base_Aporte { get; set; }

    public double Base_Aporte { get; set; }

    public short Saldo_Dias_Aporte { get; set; }

    public short Dias_Aporte { get; set; }

    public short No_Hijos { get; set; }

    public short LiqNomina { get; set; }

    [StringLength(1)]
    public string Estudiante { get; set; }

    [StringLength(1)]
    public string Practicante { get; set; }


    [StringLength(1)]
    public string MedicinaPrepagada { get; set; }

    [StringLength(12)]
    public string Telefono1 { get; set; }

    public float Porc_Embargo { get; set; }

    public double? Saldo_Ini_Horas { get; set; }

    public short DiasVacAno { get; set; }

    public short? Cod_Causal_Retiro { get; set; }

    public short? Cod_Lugar_Expedicion { get; set; }

    public short? DiasVacPendientesDisfrute { get; set; }

    public int DiasSalud { get; set; }

    [StringLength(8)]
    public string FecLimiteDeducible { get; set; }

    [StringLength(8)]
    public string Fec_Cambio_Tipo_Salario { get; set; }

    [Required]
    [StringLength(20)]
    public string GrupoSanguineo { get; set; }



    public short Cod_Barrio { get; set; }

    public short Cod_Jefe { get; set; }

    [StringLength(1)]
    public string Declarante { get; set; }


    [StringLength(8)]
    public string VigenciaDependientes { get; set; }

    public short DiasVivienda { get; set; }

    public short DiasPrepagada { get; set; }

    public short DiasDependientes { get; set; }

    [StringLength(8)]
    public string FecSubstitucionPatronal { get; set; }

    public short Cod_Alterno { get; set; }

    public short Cod_GrupoDif { get; set; }


    [StringLength(2)]
    public string Tipo_Pension { get; set; }


    [StringLength(1)]
    public string Tipo_Pensionado { get; set; }


    [StringLength(1)]
    public string Pension_Compartida { get; set; }

    [StringLength(20)]
    public string Cod_Colaborador { get; set; }

    [ForeignKey("Cod_Cargo")]
    [NotMapped]
    public virtual CARGOS Cargo { get; set; }

    [ForeignKey("Cod_Depto")]
    [NotMapped]
    public virtual DEPARTAMENTOS Departamento { get; set; }

    [ForeignKey("Cod_Depto")]
    [NotMapped]
    public virtual CIUDADES Ciudad { get; set; }

    internal string getTipoDocDane()
    {
      switch (Tip_Documento)
      {
        case "C": return "13";
        case "N": return "31";
        case "T": return "12";
        case "P": return "41";
        case "E": return "22";
        default: return "&nbsp;";
      }
    }
    internal void setData(SqlDataReader reader)
    {
      Cod_Empleado = short.Parse(reader["Cod_Empleado"].ToString());
      Empleado = reader["Empleado"].ToString().Trim();
      PNombre = reader["Pnombre"].ToString();
      SNombre = reader["Snombre"].ToString();
      PApellido = reader["PApellido"].ToString();
      SApellido = reader["SApellido"].ToString();
      Cedula = reader["Cedula"].ToString().Trim();
      Dir_Elec = reader["Dir_Elec"].ToString().Trim();
      Tip_Documento = reader["Tip_Documento"].ToString();
      Estado = reader["Estado"].ToString();
      Cod_Empleador = short.Parse(reader["Cod_Empleador"].ToString());
      Fec_Ingreso = reader["Fec_Ingreso"].ToString();
      Fec_Retiro = reader["Fec_Retiro"].ToString();
      FecSubstitucionPatronal = reader["FecSubstitucionPatronal"].ToString().Trim();
    }
  }
}
