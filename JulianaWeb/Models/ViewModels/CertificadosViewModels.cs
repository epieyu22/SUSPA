using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
    public class RetefuenteViemModel
    {
        public string dbname { get; set; }
        public EMPLEADOS[] empleados { get; set; }
        public string anoContable { get; set; }
        public bool ByCedula { get; set; }
        public bool redondear { get; set; }
    }

    public class CompropagoViewModel{
        public string ano { get; set; }
        public string mes { get; set; }
        public string quincena { get; set; }
        public string DBName { get; set; }
        public EMPLEADOS empleado { get; set; }
        public List<EMPLEADOS> empleados { get; set; }
    }

    public class CompropagoDataModel1
    {
        public short? Cod_Concepto { get; set; }
        public string Nom_Concepto { get; set; }
        public string Devengo { get; set; }
        public string Tipo_Concepto { get; set; }
        public float Dias_Novedad { get; set; }
        public float Horas_novedad { get; set; }
        public double Valor_Novedad { get; set; }
        public string BSPension { get; set; }
        public double Por_Concepto { get; internal set; }
        public string BSBenSalario { get; internal set; }
    public string OrdenDevengo { get; internal set; }
    public float Porcentaje { get; internal set; }
    public short Cod_Sub_Concepto { get; internal set; }
  }

    public class CompropagoDataModel2
    {
        public string logo;

        public string razonSocial { get; set; }
        public string nit { get; set; }
        public string afp { get; set; }
        public string eps { get; set; }
        public string banco { get; set; }
        public string ccosto { get; set; }
        public string cargo { get; set; }
        public string fechaNomina { get; set; }
        public DateTime ultimaFecNomina { get; internal set; }
        public DateTime fechaNominaActual { get; internal set; }
    public double Salario { get; internal set; }
    public float Por_Retefuente { get; internal set; }
    public float Metodo { get; internal set; }
  }

    public class CompropagoDataModel3 {
        public short? Cod_Concepto { get; set; }
        public string Nom_Concepto { get; set; }
        public float Dias_Novedad { get; set; }
        public double Val_IBC { get; set; }
        public double Val_Novedad { get; set; }
        public string Devengo { get; internal set; }
        public string ultima_Fec_Nomina { get; internal set; }
    }

    public class CompropagoDataModel4 {
        public float aporteSalud;
        public float aportePension;
        public float aporteRiesgo;

        public EMPLEADOS empleado { get; set; }
        public CompropagoDataModel2 general { get; set; }
        public List<CompropagoDataModel1> pagos { get; set; }
        public List<CompropagoDataModel1> otrosPagos { get; set; }

        public List<CompropagoDataModel3> aportes { get; set; }
        public float PorSolPen { get;  set; }
        public List<CompropagoDataModel1> bonificationsNonSalarial { get; set; }
        public List<CompropagoDataModel1> bonificationsSalarial { get; set; }
  }


    public class CertLaboralViewModel
    {
        public EMPLEADOS empleado { get; set; }
        public string dirigido { get; set; }
        public bool dirigidoEmbajada { get; set; }
        public bool viajeLaboral { get; set; }
        public string DBName { get; set; }
    public int Cod_Plantilla { get;  set; }
    public bool Aprobacion { get; set; }
  }

    public class CertLaboralTemplatewModel
    {
        public double promedioHorasExtras { get; set; }
        public EMPLEADOS empleado { get; set; }
        public string dirigido { get; set; }
        public string logo { get; set; }
        public string salario { get; set; }
        public bool dirigidoEmbajada { get; set; }
        public bool viajeLaboral { get; set; }
        public EMPRESAS empresa { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaIngreso { get; set; }
        public string ciudad { get; set; }
        public string cargo { get; set; }
        public string autoriza { get; set; }
        public string autorizaCargo { get; set; }
        public string token { get; set; }
        public bool ShowHorasExtras { get; set; }
        public bool showBeneficios { get; set; }
        public double promedioBeneficios { get; set; }
        public double netoAPagar { get; set; }
        public short? mesesPromedio { get; internal set; }
        public bool? ShowotrosDevengos { get; set; }
        public string tipoContrato { get; set; }
        public bool showOtrosIngresos { get;  set; }
        public string otrosConceptos { get;  set; }
        public double promedioOtroIngresos { get; internal set; }
        public string textEmbajada { get; internal set; }
        public string textViajeLaboral { get; internal set; }
        public Dictionary<string, double> conceptosEspecificos { get; set; }
        public string firma { get; internal set; }
        public bool? Firmar_Certlab { get; internal set; }
        public string calidad { get; internal set; }
        public string calidad2 { get; internal set; }
        public double BeneficioNoSalarial { get; internal set; }
  }




  public class SRIRetefuenteVM
  {
    public string Empresa { get; set; }
    public EMPLEADOS[] Empleados { get; set; }
    public string ano { get; set; }
    public bool unificar { get; set; }
    public bool redondear { get; set; }
  }






}
