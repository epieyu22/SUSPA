using JulianaWeb.Business.ElectronicPayroll;
using JulianaWeb.Helpers;
using JulianaWeb.Interfaces.Business.WebServices.ElectronicPayroll;
using JulianaWeb.Models;
using JulianaWeb.Models.Juliana.Generals;
using JulianaWeb.Models.Juliana.NominaElectronica;
using JulianaWeb.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace JulianaWeb.Controllers
{


  [RoutePrefix("API/facElectronica")]
  public class facElectronicaController : ApiController
  {
    const string DataicoPrefixes = "DataicoElectronicPrefixes";

    private static readonly HttpClient HttpClient = new HttpClient();


    [Route("{Empresa}/Employees")]
    [HttpPut]
    public dynamic GetDataPayroll(string Empresa, FacturacionViewModel data)
    {
      using (JulianaContext db = new JulianaContext(Empresa))
      {

        string historicDate = data.ano + data.mes + "01";
        DateTime startDate = UtilHelper.getDate(historicDate);
        DateTime finalDate = startDate.AddMonths(1).AddDays(-1);


        var employees = db.Database.SqlQuery<EmpleadosListViewModel>($@"
SELECT emp.Cod_Empleado
		, emp.Cedula
		, his.MinFec
		, his.MaxFec
		, emp.Empleado
		, emp.Salario
		, his.Devengo
		, his.Deduccion
		, his.Devengo - his.Deduccion AS Total
FROM EMPLEADOS emp
INNER JOIN (
	SELECT h.Cod_Empleado
		, '{startDate.ToString("dd/MMM")}' AS MinFec
		, '{finalDate.ToString("dd/MMM")}' AS MaxFec
		, SUM(CASE WHEN (c.Devengo = 'S' OR c.Cod_Concepto = '100') AND c.Tipo_Concepto NOT IN ('0', '9') THEN h.Val_Novedad ELSE 0 END) Devengo
		, SUM(CASE WHEN c.Devengo = 'N' THEN h.Val_Novedad ELSE 0 END) Deduccion
	FROM HISTORICO h
	INNER JOIN CONCEPTOS c ON h.Cod_Concepto = c.Cod_Concepto
	WHERE h.Fec_Nomina LIKE '{startDate.ToString("yyyyMM")}%' AND h.Estado = 'P'
	GROUP BY h.Cod_Empleado
) his ON his.Cod_Empleado = emp.Cod_Empleado
ORDER BY emp.Empleado").ToList();

        var reponses = db.STATUS_NOM_ELEC.ToList();
        var devengoTotal = employees.Aggregate(0D, (sum, e) => sum + e.Devengo);
        var deduccionTotal = employees.Aggregate(0D, (sum, e) => sum + e.Deduccion);
        var total = employees.Aggregate(0D, (sum, e) => sum + e.Total);

        return new { employees, reponses, totales = new { devengoTotal, deduccionTotal, total } };
      }

    }

    [Route("{Empresa}/DataSend")]
    [HttpPut]
    public async Task<HttpResponseMessage> Generate_bill(string Empresa, FacturacionViewModel data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      
      #region Validation
      Dictionary<string, object> validations = new Dictionary<string, object>();

      var CONCEPTOS_REPETIDOS = (from con in db.CONCEPTOS
                                 group con by new { con.Cod_Concepto, con.Nom_Concepto }
                           into rep
                                 select new
                                 {
                                   Codigo = rep.Key.Cod_Concepto,
                                   Nombre = rep.Key.Nom_Concepto,
                                   Cantidad = rep.Count()
                                 })
                            .Where(x => x.Cantidad > 1)
                            .Select(x => new {
                              Codigo = x.Codigo,
                              Nombre = x.Nombre
                            })
                            .ToList();

      if (CONCEPTOS_REPETIDOS.Count > 0)
        validations.Add("CONCEPTOS_REPETIDOS", new
        {
          message = "Revisar los conceptos repetidos",
          list = CONCEPTOS_REPETIDOS
        });


      var COD_ALTERNO2 = (from con in db.CONCEPTOS
                          join map in db.MAPEO_NOM_ELEC on con.Cod_Concepto.ToString() equals map.Cod_Concepto
                          where map.Cod_Tipo == 1 && con.Tipo_Concepto == "I" && !new string[] { "COMUN", "PROFESIONAL", "LABORAL" }.Contains(map.Cod_Alterno2)
                          select new {
                            Codigo = con.Cod_Concepto,
                            Nombre = con.Nom_Concepto.Trim()
                          }).ToList();

      if (COD_ALTERNO2.Count > 0)
        validations.Add("COD_ALTERNO2", new
        {
          message = "Revisar Codigo Alterno de los siguientes Conceptos",
          list = COD_ALTERNO2
        });

      var TIPOS_DE_CONTRATO = (from emp in db.EMPLEADOS
                               where emp.Estado != "R" &&
                                !(from map in db.MAPEO_NOM_ELEC
                                 where map.Cod_Tipo == 5
                                 select map.Cod_Concepto).ToList().Contains(emp.Tipo_Contrato)
                               select new
                               {
                                 Codigo = emp.Cod_Empleado,
                                 Nombre = emp.Empleado
                               }).ToList();

      if (TIPOS_DE_CONTRATO.Count > 0)
        validations.Add("TIPOS_DE_CONTRATO", new
        {
          message = "Revisar Tipo de Contrato de los siguientes Empleados",
          list = TIPOS_DE_CONTRATO
        });

      var TIPOS_DE_DOCUMENTO = (from emp in db.EMPLEADOS
                               where emp.Estado != "R" &&
                                !(from map in db.MAPEO_NOM_ELEC
                                  where map.Cod_Tipo == 3
                                  select map.Cod_Concepto).ToList().Contains(emp.Tip_Documento)
                               select new
                               {
                                 Codigo = emp.Cod_Empleado,
                                 Nombre = emp.Empleado
                               }).ToList();

      if (TIPOS_DE_DOCUMENTO.Count > 0)
        validations.Add("TIPOS_DE_DOCUMENTO", new
        {
          message = "Revisar Tipos de Documento de los siguientes Empleados",
          list = TIPOS_DE_DOCUMENTO
        });

      var CORREOS = (from emp in db.EMPLEADOS
                                where emp.Estado != "R" &&
                                 (emp.Dir_Elec == null ||
                                 !emp.Dir_Elec.Contains("@") ||
                                 emp.Dir_Elec.Contains(",") ||
                                 //emp.Dir_Elec.ToLower().Contains("systemsltda.com") ||
                                 (!emp.Dir_Elec.ToLower().Contains(".co") &&
                                 !emp.Dir_Elec.ToLower().Contains(".es") &&
                                 !emp.Dir_Elec.ToLower().Contains(".net")))
                                select new
                                {
                                  Codigo = emp.Cod_Empleado,
                                  Nombre = emp.Empleado
                                }).ToList();

      if (CORREOS.Count > 0)
        validations.Add("CORREOS", new
        {
          message = "Revisar Correo Electrónico de los siguientes Empleados",
          list = CORREOS
        });

      var DIRECCION = (from emp in db.EMPLEADOS
                     where emp.Estado != "R" &&
                     (emp.Direccion == null ||
                     emp.Direccion.Trim() == "")
                     select new
                     {
                       Codigo = emp.Cod_Empleado,
                       Nombre = emp.Empleado
                     }).ToList();

      if (DIRECCION.Count > 0)
        validations.Add("DIRECCION", new
        {
          message = "Revisar Dirección de los siguientes Empleados",
          list = DIRECCION
        });

      var CIUDAD = (from emp in db.EMPLEADOS
                       where emp.Estado != "R" && emp.Cod_Ciudad == 0
                       select new
                       {
                         Codigo = emp.Cod_Empleado,
                         Nombre = emp.Empleado
                       }).ToList();

      if (CIUDAD.Count > 0)
        validations.Add("CIUDAD", new
        {
          message = "Revisar Código de Ciudad de los siguientes Empleados",
          list = CIUDAD
        });

      var MAPEO = (from his in db.HISTORICO
                   where his.Estado == "P" && his.Fec_Nomina.Substring(0, 6) == data.ano + data.mes &&
                   !(from map in db.MAPEO_NOM_ELEC
                     where map.Cod_Tipo == 1
                     select map.Cod_Concepto).ToList().Contains(his.Cod_Concepto.ToString())
                   select new
                   {
                     Codigo = his.Cod_Concepto,
                     Fecha = his.Fec_Nomina
                   })
                   .Distinct()
                   .AsEnumerable()
                   .Select(x => new
                   {
                     Codigo = x.Codigo,
                     Fecha = UtilHelper.getDate(x.Fecha).Date.ToString("dd/MM/yyyy")
                   }).ToList();

      if (MAPEO.Count > 0)
        validations.Add("MAPEO", new
        {
          message = "Los siguientes Conceptos NO existen en el mapeo",
          list = MAPEO
        });

      var SOLIDARIDAD_PENSIONAL = (from his in db.HISTORICO
                                   where his.Estado == "P" &&
                                   his.Fec_Nomina.Substring(0, 6) == data.ano + data.mes &&
                                   his.Cod_Concepto == 11 &&
                                   his.Porcentaje == 0.0
                                   select new
                                   {
                                     Codigo = his.Cod_Concepto,
                                     Nombre = his.Fec_Nomina
                                   }).Distinct().ToList();

      if (SOLIDARIDAD_PENSIONAL.Count > 0)
        validations.Add("SOLIDARIDAD_PENSIONAL", new
        {
          message = "Revisar el % de solidaridad pensional",
          list = SOLIDARIDAD_PENSIONAL
        });

      var NOVEDAD_PRESTACIONES = (from his in db.HISTORICO
                                   where his.Estado == "P" &&
                                   his.Fec_Nomina.Substring(0, 6) == data.ano + data.mes &&
                                   new short[] { 97, 21, 98, 20, 19 }.Contains(his.Cod_Concepto) &&
                                   his.Dias_Novedad <= 0.0
                                   select new
                                   {
                                     Codigo = his.Cod_Concepto,
                                     Nombre = his.Fec_Nomina
                                   }).Distinct().ToList();

      if (NOVEDAD_PRESTACIONES.Count > 0)
        validations.Add("NOVEDAD_PRESTACIONES", new
        {
          message = "Revisar el % de solidaridad pensional",
          list = NOVEDAD_PRESTACIONES
        });

      var MAPEO_REPETIDOS = (from map in db.MAPEO_NOM_ELEC
                             where map.Dian_xml != "NIE000"
                             group map by new { map.Cod_Concepto, map.Cod_Alterno }
                             into rep
                             select new
                             {
                               Codigo = rep.Key.Cod_Concepto,
                               Nombre = rep.Key.Cod_Alterno,
                               Cantidad = rep.Count()
                             })
                            .Where(x => x.Cantidad > 1)
                            .Select(x => new
                            {
                              Codigo = x.Codigo,
                              Nombre = x.Nombre
                            })
                            .ToList();

      if (MAPEO_REPETIDOS.Count > 0)
        validations.Add("MAPEO_REPETIDOS", new
        {
          message = "Revisar conceptos x mapeo repetidos (Cod Alterno)",
          list = MAPEO_REPETIDOS
        });

      var VALIDAR_MAPEO = (from map in db.MAPEO_NOM_ELEC
                           where map.Dian_xml != null && (map.Cod_Alterno == null ||
                           (map.Dian_xml == "NIE070" && map.Cod_Alterno != "BASICO") ||
                           (map.Dian_xml == "NIE056" && map.Cod_Alterno != "BASICO") ||
                           (map.Dian_xml == "NIE071" && map.Cod_Alterno != "AUXILIO_DE_TRANSPORTE") ||
                           (map.Dian_xml == "NIE078" && map.Cod_Alterno != "HORA_EXTRA_DIURNA") ||
                           (map.Dian_xml == "NIE177" && map.Cod_Alterno != "RETENCION_FUENTE") ||
                           (map.Dian_xml == "NIE163" && map.Cod_Alterno != "SALUD") ||
                           (map.Dian_xml == "NIE166" && map.Cod_Alterno != "FONDO_PENSION") ||
                           (map.Dian_xml == "NIE093" && map.Cod_Alterno != "HORA_EXTRA_DIURNA_DF") ||
                           (map.Dian_xml == "NIE112" && map.Cod_Alterno != "VACACION") ||
                           (map.Dian_xml == "NIE116" && map.Cod_Alterno != "VACACION_COMPENSADA") ||
                           (map.Dian_xml == "NIE168" && map.Cod_Alterno != "FONDO_SOLIDARIDAD_PENSIONAL") ||
                           (map.Dian_xml == "NIE139" && map.Cod_Alterno != "BONIFICACION") ||
                           (map.Dian_xml == "NIE155" && map.Cod_Alterno != "COMISION") ||
                           (map.Dian_xml == "NIE181" && map.Cod_Alterno != "EMBARGO_FISCAL") ||
                           (map.Dian_xml == "NIE194" && map.Cod_Alterno != "ANTICIPO") ||
                           (map.Dian_xml == "NIE195" && map.Cod_Alterno != "PAGO_TERCERO") ||
                           (map.Dian_xml == "NIE196" && map.Cod_Alterno != "ANTICIPO") ||
                           (map.Dian_xml == "NIE118" && map.Cod_Alterno != "PRIMA") ||
                           (map.Dian_xml == "NIE120" && map.Cod_Alterno != "CESANTIAS") ||
                           (map.Dian_xml == "NIE122" && map.Cod_Alterno != "CESANTIAS") ||
                           (map.Dian_xml == "NIE131" && map.Cod_Alterno != "LICENCIA_PATERNIDAD") ||
                           (map.Dian_xml == "NIE120" && map.Cod_Alterno != "CESANTIAS") ||
                           (map.Dian_xml == "NIE135" && map.Cod_Alterno != "LICENCIA_REMUNERADA") ||
                           (map.Dian_xml == "NIE138" && map.Cod_Alterno != "LICENCIA_NO_REMUNERADA") ||
                           (map.Dian_xml == "NIE127" && map.Cod_Alterno != "INCAPACIDAD") ||
                           (map.Dian_xml == "NIE172" && map.Cod_Alterno != "SINDICATO") ||
                           (map.Dian_xml == "NIE138" && map.Cod_Alterno != "LICENCIA_NO_REMUNERADA") ||
                           (map.Dian_xml == "NIE160" && map.Cod_Alterno != "INDEMNIZACION") ||
                           (map.Dian_xml == "NIE072" && map.Cod_Alterno != "VIATICO") ||
                           (map.Dian_xml == "NIE127" && map.Cod_Alterno != "INCAPACIDAD") ||
                           (map.Dian_xml == "NIE194" && map.Cod_Alterno != "ANTICIPO") ||
                           (map.Dian_xml == "NIE088" && map.Cod_Alterno != "HORA_RECARGO_NOCTURNO") ||
                           (map.Dian_xml == "NIE083" && map.Cod_Alterno != "HORA_EXTRA_NOCTURNA") ||
                           (map.Dian_xml == "NIE198" && map.Cod_Alterno != "PENSION_VOLUNTARIA") ||
                           (map.Dian_xml == "NIE118" && map.Cod_Alterno != "PRIMA") ||
                           (map.Dian_xml == "NIE103" && map.Cod_Alterno != "HORA_EXTRA_NOCTURNA_DF") ||
                           (map.Dian_xml == "NIE156" && map.Cod_Alterno != "DOTACION") ||
                           (map.Dian_xml == "NIE098" && map.Cod_Alterno != "HORA_RECARGO_DIURNA_DF") ||
                           (map.Dian_xml == "NIE108" && map.Cod_Alterno != "HORA_RECARGO_NOCTURNO_DF") ||
                           (map.Dian_xml == "NIE140" && map.Cod_Alterno != "BONIFICACION") ||
                           (map.Dian_xml == "NIE142" && map.Cod_Alterno != "AUXILIO") ||
                           (map.Dian_xml == "NIE157" && map.Cod_Alterno != "APOYO_PRACTICA") ||
                           (map.Dian_xml == "NIE152" && map.Cod_Alterno != "BONO_EPCTV") ||
                           (map.Dian_xml == "NIE176" && map.Cod_Alterno != "LIBRANZA") ||
                           (map.Dian_xml == "NIE159" && map.Cod_Alterno != "BONIFICACION_RETIRO") ||
                           (map.Dian_xml == "NIE185" && map.Cod_Alterno != "DEUDA") ||
                           (map.Dian_xml == "NIE197" && map.Cod_Alterno != "OTRA_DEDUCCION") ||
                           (map.Dian_xml == "NIE180" && map.Cod_Alterno != "COOPERATIVA") ||
                           (map.Dian_xml == "NIE179" && map.Cod_Alterno != "AFC") ||
                           (map.Dian_xml == "NIE154" && map.Cod_Alterno != "BONO_EPCTV_ALIMENTACION") ||
                           (map.Dian_xml == "NIE182" && map.Cod_Alterno != "PLANES_COMPLEMENTARIOS") ||
                           (map.Dian_xml == "NIE201" && map.Cod_Alterno != "REINTEGRO") ||
                           (map.Dian_xml == "NIE148" && map.Cod_Alterno != "OTRO_CONCEPTO") ||
                           (map.Dian_xml == "NIE073" && map.Cod_Alterno != "VIATICO") ||
                           (map.Dian_xml == "NIE119" && map.Cod_Alterno != "PRIMA") ||
                           (map.Dian_xml == "NIE184" && map.Cod_Alterno != "REINTEGRO") ||
                           (map.Dian_xml == "NIE183" && map.Cod_Alterno != "EDUCACION"))
                           select new
                           {
                             Codigo = map.Dian_xml,
                             Nombre = map.Cod_Alterno
                           }).ToList();

      if (VALIDAR_MAPEO.Count > 0)
        validations.Add("VALIDAR_MAPEO", new {
          message = "Revisar validación de mapeo",
          list = VALIDAR_MAPEO
        });

      if (validations.Count > 0)
        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new { validations });
      #endregion


      Entries payrollInfoEntrie = new Entries();

      DataicoPayrollService Service = new DataicoPayrollService(db);
      IElectronicPayrollSave<ElectronicPayrollInfo, Task<PayrollResponse>> payrollSaver = new DataicoPayrollService(db);
      IElectronicPayrollUpdate<ElectronicPayrollInfo, Task<PayrollResponse>> payrollUpdate = new DataicoPayrollService(db);

      

      List<ElectronicPayrollInfo> payrollInfo = new List<ElectronicPayrollInfo>();
      //List<PayrollResponse> payrollResponses = new List<PayrollResponse>();

      var IntegralSalary = false;

      CIUDADES address = new CIUDADES();

      Stopwatch sw = Stopwatch.StartNew();
      string historicDate = null;
      string historicStartDate = null;
      string historicFinalDate = null;
      string dia30 = "30";
      string dia01 = "01";
      string dia15 = "15";
      string dia28 = "28";
      string dia29 = "29";


      Dictionary<string, PARAMETROS_GENERALES> dataicoParameters = db.PARAMETROS_GENERALES
                                                                          .Where(p => p.Cod_Parametro.Contains("Dataico"))
                                                                          .ToDictionary(p => p.Cod_Parametro, p => p);

      TipoComprobante tipoComprobante = JsonConvert.DeserializeObject<TipoComprobante>(dataicoParameters[DataicoPrefixes].Valor);

      

      if (data.mes == "02")
      {
        historicDate = data.ano + data.mes + dia28;
        historicFinalDate = dia28 + '/' + data.mes + '/' + data.ano;
        historicStartDate = dia01 + '/' + data.mes + '/' + data.ano;
      }
      else
      {
        historicFinalDate = dia30 + '/' + data.mes + '/' + data.ano;
        historicStartDate = dia01 + '/' + data.mes + '/' + data.ano;
        historicDate = data.ano + data.mes + 30;
      }



      Dictionary<string, MAPEO_NOM_ELEC> conceptCodeMapping = GenerateMappingCodes("CONCEPTO", data.Empresa);
      Dictionary<string, MAPEO_NOM_ELEC> contractCodeMapping = GenerateMappingCodes("TIPO_CONTRATO", data.Empresa);
      Dictionary<string, MAPEO_NOM_ELEC> documentCodeMapping = GenerateMappingCodes("TIPO_DOCUMENTO", data.Empresa);
      Dictionary<string, MAPEO_NOM_ELEC> paymentMeansCodeMapping = GenerateMappingCodes("TIPO_PAGO", data.Empresa);
      Dictionary<string, MAPEO_NOM_ELEC> medicalTypeCodeMapping = GenerateMappingCodes("TIPO_INCAPACIDAD", data.Empresa);
      Dictionary<string, MAPEO_NOM_ELEC> workerTypeCodeMapping = GenerateMappingCodes("TIPO_TRABAJADOR", data.Empresa);
      string DianTestID = "DataicoElectronicPayrollDianTestId";
      DianTestID = dataicoParameters[DianTestID].Valor;
      string DianID = "DataicoElectronicPayrollDianId";
      DianID = dataicoParameters[DianID].Valor;

      Dictionary<short, CONCEPTOS> concepts = db.CONCEPTOS
                                            .Where(c => c.Tipo_Concepto != "6" || c.Cod_Concepto == 100 || c.Cod_Concepto == 97 || c.Cod_Concepto == 98)
                                            .ToDictionary(c => c.Cod_Concepto, c => c);

      List<short> conceptIds = concepts.Keys.ToList();
      Dictionary<string, HISTORICO> historicsMapping = GetHistoricMapping(db, historicDate, conceptIds);
      List<EMPLEADOS> employees = (from e in db.EMPLEADOS.ToList()
                                   join d in data.empleados on e.Cod_Empleado equals d.Cod_Empleado
                                   select e).ToList();

      Software softwareInfo = new Software
      {
        Pin = "4123412",
        DianTestId = DianTestID,
        DianId = DianID
      };

      DateTime today = DateTime.Now;
      var issueDate = today.ToString("dd/MM/yyyy");
      int maxConsecutivos = 0;
      if (data.Cod_Archivo == "2")
      {
        var lastModified = db.STATUS_NOM_ELEC
                              .Where(c => c.Prefix == tipoComprobante.NotaAjusteRemplazo.Prefijo)
                              .OrderByDescending(c => c.Consecutivo)
                              .FirstOrDefault();

        maxConsecutivos = (int)(lastModified?.Consecutivo);
      }
      else if (data.Cod_Archivo == "1")
      {
        if (db.STATUS_NOM_ELEC.Any())
        {
          maxConsecutivos = db.STATUS_NOM_ELEC.Max(p => p.Consecutivo);
        }
      }

      foreach (EMPLEADOS employee in employees)
      {
        short Cod_Empleado = employee.Cod_Empleado;

        STATUS_NOM_ELEC status = new STATUS_NOM_ELEC();
        
        if (data.Cod_Archivo == "2")
        {
          maxConsecutivos++;
          status = GuardarRespuesta(historicFinalDate, ref maxConsecutivos, Cod_Empleado, data.Cod_Archivo, data.Empresa, tipoComprobante.NotaAjusteRemplazo.Prefijo);
        }
        else if (data.Cod_Archivo == "1")
        {
          maxConsecutivos++;
          status = GuardarRespuesta(historicFinalDate, ref maxConsecutivos, Cod_Empleado, data.Cod_Archivo, data.Empresa, tipoComprobante.SoporteNomina.Prefijo);
        }


        string ParseConsecutivo = maxConsecutivos.ToString();
        ElectronicPayrollInfo payrollRecord = new ElectronicPayrollInfo
        {
          Env = "PRODUCCION",
          send_dian = true,
          Prefix = tipoComprobante.SoporteNomina.Prefijo,
          Number = Int32.Parse(ParseConsecutivo),
          Periodicity = "MENSUAL",
          Salary = employee.Salario,
          InitialSettlementDate = historicStartDate,
          FinalSettlementDate = historicFinalDate,
          IssueDate = issueDate,
          PaymentDate = historicFinalDate,
          Accruals = new List<Accrual>(),
          Deductions = new List<Deduction>()
        };
        switch (data.Cod_Archivo)
        {
          case "2":
            payrollRecord.Prefix = tipoComprobante.NotaAjusteRemplazo.Prefijo;
            var cuneReport = db.STATUS_NOM_ELEC.FirstOrDefault(c => c.Number == Cod_Empleado && c.Fec_Nomina == historicFinalDate && c.Prefix == tipoComprobante.SoporteNomina.Prefijo);
            
            if (cuneReport != null)
            {
              payrollRecord.ReplacementFor = new Replacement
              {
                Cune = cuneReport.Cune,
                Prefix = tipoComprobante.SoporteNomina.Prefijo,
                Number = cuneReport.Consecutivo,
                IssueDate = issueDate
              };
            }

            break;
          case "3":
            break;
          default:

            break;
        }
        payrollRecord.Notes = new List<Note>
          {
            new Note{ Text = "Documento generado desde Juliana Web, Systems Integrated Solutions"}
          };


        // Se llenan los conceptos de devengos 
        foreach (CONCEPTOS accrualConcept in concepts.Values.Where(c => c.Tipo_Concepto == "L" || c.Devengo == "S" || c.Cod_Concepto == 100 || c.Cod_Concepto == 97 || c.Cod_Concepto == 16 || c.Cod_Concepto == 48 || c.Cod_Concepto == 98 || c.Cod_Concepto == 98))
        {
          if (!historicsMapping.ContainsKey($"{accrualConcept.Cod_Concepto}-{employee.Cod_Empleado}"))
            continue;

          HISTORICO accrualHistoric = historicsMapping[$"{accrualConcept.Cod_Concepto}-{employee.Cod_Empleado}"];
          var DianCode = GetMappedDianConcept(accrualHistoric.Cod_Concepto.ToString(), conceptCodeMapping);
          var DianaAlternalCode = GetMappedAlternalDianConcept(accrualHistoric.Cod_Concepto.ToString(), conceptCodeMapping);
          var DianaAlternal2Code = GetMappedAlternalDian2Concept(accrualHistoric.Cod_Concepto.ToString(), conceptCodeMapping);

          var Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping);

          if (accrualHistoric.Dias_Novedad == 0)
          {
            if (DianCode == "NIE152" || DianCode == "NIE148" || DianCode == "NIE073" || DianCode == "NIE142" || DianCode == "NIE154" || DianCode == "NIE140")
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = null,
                AmountNs = accrualHistoric.Val_Novedad,
                Days = null,
                Description = accrualConcept.Nom_Concepto,
              });
            }
            else if (DianCode.Contains("NIE159"))
            {
              if (accrualHistoric.Val_Novedad == 0.0)
              {

              }
              else
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                });
            }
            else if (Code.Contains("ANTICIPO"))
            {
              if (accrualHistoric.Val_Novedad == 0.0)
              {

              }
              else
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = null
                });
              }

            }
            else if (Code.Contains("HORA_EXTRA_DIURNA")
              || Code.Contains("HORA_EXTRA_NOCTURNA")
              || Code.Contains("HORA_EXTRA_DIURNA_DF")
              || Code.Contains("HORA_RECARGO_NOCTURNO")
              || Code.Contains("HORA_EXTRA_DIURNA_DF")
              || Code.Contains("HORA_RECARGO_DIURNA_DF")
              || Code.Contains("HORA_EXTRA_NOCTURNA_DF")
              || Code.Contains("HORA_RECARGO_NOCTURNO_DF"))
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = accrualHistoric.Val_Novedad,
                AmountNs = null,
                Days = null,
                Hours = accrualHistoric.Horas_Novedad,
              });
            }

            else if (Code.Contains("LICENCIA_NO_REMUNERADA"))
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Days = accrualHistoric.Dias_Novedad,
              });
            }
            else if (Code.Contains("VACACION"))
            {
              if (accrualHistoric.Dias_Novedad == 0)
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = "VACACION",
                  Amount = accrualHistoric.Val_Novedad,
                  Days = accrualHistoric.Dias_Novedad,
                });
              }
              else if (accrualHistoric.Dias_Novedad > 31)
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = "VACACION",
                  Amount = accrualHistoric.Val_Novedad,
                  Days = 31.00,
                });
              }
              else
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = accrualHistoric.Dias_Novedad,
                });
            }
            else if (Code.Contains("  "))
            {
              if (accrualHistoric.Val_Novedad != 0)
              {
                int porcentage = (int)(accrualHistoric.Dias_Novedad / 360 * 12);

                if (DianaAlternalCode.Contains("INTERESES"))
                {
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    CesantiasInterest = accrualHistoric.Val_Novedad,
                    Percentage = porcentage
                  }); ;
                }
                else
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    AmountNs = accrualHistoric.Val_Novedad,
                    Percentage = porcentage,
                    CesantiasInterest = null
                  });
              }
            }
            else if (Code.Contains("INCAPACIDAD"))
            {
              switch (DianaAlternal2Code)
              {
                case "COMUN":
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    Amount = accrualHistoric.Val_Novedad,
                    Days = (int?)accrualHistoric.Dias_Novedad,
                    MedicalLeaveType = DianaAlternal2Code,
                  });
                  break;
                case "PROFESIONAL":
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    Amount = accrualHistoric.Val_Novedad,
                    Days = (int?)accrualHistoric.Dias_Novedad,
                    MedicalLeaveType = DianaAlternal2Code,
                  });
                  break;
                case "LABORAL":
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    Amount = accrualHistoric.Val_Novedad,
                    Days = (int?)accrualHistoric.Dias_Novedad,
                    MedicalLeaveType = DianaAlternal2Code,
                  });
                  break;
                default:
                  break;
              }
            }
            else if (Code.Contains("OTRO_CONCEPTO"))
            {
              if (DianCode == "NIE147")
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Description = accrualConcept.Nom_Concepto.Trim(),
                  Days = null
                });
              }
              else if (DianCode == "NIE148")
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  AmountNs = accrualHistoric.Val_Novedad,
                  Description = accrualConcept.Nom_Concepto,
                  Days = null
                });
              }
            }
            else if (Code.Contains("PAGO_TERCERO"))
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = accrualHistoric.Val_Novedad,

              });
            }
            else if (Code.Contains("BASICO"))
            {
              if (accrualHistoric.Val_Novedad == 0 || accrualHistoric.Val_Novedad == null)
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = Convert.ToInt32(accrualHistoric.Dias_Novedad),
                });
              }
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = accrualHistoric.Val_Novedad,
                Days = Convert.ToInt32(accrualHistoric.Dias_Novedad),
              });
            }
            else if (Code.Contains("BONO_EPCTV_ALIMENTACION"))
            {
              if (DianCode == "NIE153")
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = null
                });
              }
              else if (DianCode == "NIE154")
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  AmountNs = accrualHistoric.Val_Novedad,
                  Days = null
                });
              }
              else if (Code.Contains("ANTICIPO"))
              {
                if (accrualHistoric.Val_Novedad == 0.0)
                {

                }
                else
                {
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    Amount = accrualHistoric.Val_Novedad,
                    Days = null
                  });
                }

              }
              else if (DianCode == "NIE152" || DianCode == "NIE148" || DianCode == "NIE073" || DianCode == "NIE142" || DianCode == "NIE154" || DianCode == "NIE140")
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = null,
                  AmountNs = accrualHistoric.Val_Novedad,
                  Days = null,
                  Description = accrualConcept.Nom_Concepto,
                });
              }
            }
            else if (Code.Contains("CESANTIAS"))
            {
              if (accrualHistoric.Val_Novedad != 0)
              {
                double porcentage = (double)(accrualHistoric.Dias_Novedad / 360 * 12);
                porcentage = (double)Math.Round((double)porcentage, 2);
                if (DianaAlternalCode.Contains("INTERESES"))
                {
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    CesantiasInterest = accrualHistoric.Val_Novedad,
                    AmountNs = accrualHistoric.Val_Novedad,
                    Percentage = porcentage
                  }); ;
                }
                else
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    AmountNs = accrualHistoric.Val_Novedad,
                    Percentage = porcentage,
                    CesantiasInterest = null
                  });
              }
            }
            else
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = accrualHistoric.Val_Novedad,
                AmountNs = null,
                Days = null,

              });
          }
          else if (DianCode == "NIE152" || DianCode == "NIE148" || DianCode == "NIE073" || DianCode == "NIE142" || DianCode == "NIE154" || DianCode == "NIE140")
          {
            payrollRecord.Accruals.Add(new Accrual
            {
              Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = null,
              AmountNs = accrualHistoric.Val_Novedad,
              Days = null,
              Description = accrualConcept.Nom_Concepto,
            });
          }
          else if (DianCode.Contains("NIE159"))
          {
            if (accrualHistoric.Val_Novedad == 0.0)
            {

            }
            payrollRecord.Accruals.Add(new Accrual
            {
              Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = accrualHistoric.Val_Novedad,
            });
          }
          else if (Code.Contains("LICENCIA_NO_REMUNERADA"))
          {
            payrollRecord.Accruals.Add(new Accrual
            {
              Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Days = accrualHistoric.Dias_Novedad,
            });
          }
          else if (DianCode.Contains("NIE116"))
          {
            double Dias_Nov = accrualHistoric.Dias_Novedad;
            Dias_Nov = (double)Math.Round((double)Dias_Nov, 2);
            if (accrualHistoric.Dias_Novedad == 0)
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = "VACACION",
                Amount = accrualHistoric.Val_Novedad,
                AmountNs = null,
                Days = Dias_Nov,
              });
            }
            else if (accrualHistoric.Dias_Novedad > 31.00)
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = "VACACION_COMPENSADA",
                AmountNs = accrualHistoric.Val_Novedad,
                Amount = null,
                Days = 31,
              });
            }
            else
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = "VACACION_COMPENSADA",
                AmountNs = accrualHistoric.Val_Novedad,
                Amount = null,
                Days = Dias_Nov,
              });
            }

          }
          else if (Code.Contains("VACACION"))
          {
            double Dias_Nov = accrualHistoric.Dias_Novedad;
            Dias_Nov = (double)Math.Round((double)Dias_Nov, 2);
            if (accrualHistoric.Dias_Novedad == 0)
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = "VACACION",
                Amount = accrualHistoric.Val_Novedad,
                AmountNs = null,
                Days = Dias_Nov,
              });
            }
            if (accrualHistoric.Dias_Novedad > 31)
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = "VACACION",
                Amount = accrualHistoric.Val_Novedad,
                AmountNs = null,
                Days = 31.00,
              });
            }
            else
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = accrualHistoric.Val_Novedad,
                AmountNs = null,
                Days = Dias_Nov,
              });
          }
          else if (Code.Contains("HORA_EXTRA_DIURNA")
              || Code.Contains("HORA_EXTRA_NOCTURNA")
              || Code.Contains("HORA_EXTRA_DIURNA_DF")
              || Code.Contains("HORA_RECARGO_NOCTURNO")
              || Code.Contains("HORA_EXTRA_DIURNA_DF")
              || Code.Contains("HORA_RECARGO_DIURNA_DF")
              || Code.Contains("HORA_EXTRA_NOCTURNA_DF")
              || Code.Contains("HORA_RECARGO_NOCTURNO_DF"))
          {
            payrollRecord.Accruals.Add(new Accrual
            {
              Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = accrualHistoric.Val_Novedad,
              AmountNs = null,
              Days = null,
              Hours = accrualHistoric.Horas_Novedad,
            });
          }
          else if (Code.Contains("APOYO_PRACTICA"))
          {
            payrollRecord.Accruals.Add(new Accrual
            {
              Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = 0.0,
              AmountNs = accrualHistoric.Val_Novedad,
            });
          }
          else if (Code.Contains("ANTICIPO"))
          {
            if (DianCode == "NIE194")
            {
              if (accrualHistoric.Val_Novedad == 0.0)
              {

              }
              else
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = null
                });
              }
            }
          }
          else if (Code.Contains("AUXILIO_DE_TRANSPORTE"))
          {

            payrollRecord.Accruals.Add(new Accrual
            {
              Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = accrualHistoric.Val_Novedad,
              AmountNs = null,
              Days = null,
            });

          }
          else if (Code.Contains("OTRO_CONCEPTO"))
          {
            if (DianCode == "NIE147")
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = accrualHistoric.Val_Novedad,
                Description = accrualConcept.Nom_Concepto,
                Days = null
              });
            }
            else if (DianCode == "NIE148")
            {
              payrollRecord.Accruals.Add(new Accrual
              {
                Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                //AmountNs = accrualHistoric.Val_Novedad,
                Description = accrualConcept.Nom_Concepto,
                Days = null
              });
            }


          }
          else if (Code.Contains("CESANTIAS") && DianCode != "NIE122")
          {
            if (accrualHistoric.Val_Novedad != 0)
            {
              double porcentage = ((double)accrualHistoric.Dias_Novedad / 360.0) * 12.0;
              porcentage = Math.Round(porcentage, 2);
              if (DianaAlternalCode.Contains("INTERESES"))
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  CesantiasInterest = accrualHistoric.Val_Novedad,
                  //Amount = accrualHistoric.Val_Novedad,
                  Percentage = porcentage
                }); ;
              }
              else
              {
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  AmountNs = accrualHistoric.Val_Novedad,
                  Percentage = porcentage,
                  CesantiasInterest = null
                });
              }
            }
          }
          // Primero INTERESES DE CESANTÍAS
          else if (DianCode == "NIE122")
          {
            if (accrualHistoric.Val_Novedad != 0)
            {
              // Buscar el accrual de CESANTÍAS ya agregado
              var cesantiasAccrual = payrollRecord.Accruals
                  .FirstOrDefault(a => a.Code.Contains("CESANTIAS"));

              if (cesantiasAccrual != null)
              {
                cesantiasAccrual.CesantiasInterest = accrualHistoric.Val_Novedad;

                if (accrualHistoric.Dias_Novedad > 0)
                {
                  double porcentage = (accrualHistoric.Dias_Novedad / 360.0) * 12.0;
                  cesantiasAccrual.Percentage = Math.Round(porcentage, 2);
                }
              }
              else
              {
                double porcentage = 0.0;
                if (accrualHistoric.Dias_Novedad > 0)
                  porcentage = Math.Round((accrualHistoric.Dias_Novedad / 360.0) * 12.0, 2);

                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  AmountNs = null,
                  Percentage = porcentage, // <-- ya no 0 fijo
                  CesantiasInterest = accrualHistoric.Val_Novedad
                });
              }
            }
          }
          else if (Code.Contains("INCAPACIDAD"))
          {
            switch (DianaAlternal2Code)
            {
              case "COMUN":
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = (int?)accrualHistoric.Dias_Novedad,
                  MedicalLeaveType = DianaAlternal2Code,
                });
                break;
              case "PROFESIONAL":
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = (int?)accrualHistoric.Dias_Novedad,
                  MedicalLeaveType = DianaAlternal2Code,
                });
                break;
              case "LABORAL":
                payrollRecord.Accruals.Add(new Accrual
                {
                  Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                  Amount = accrualHistoric.Val_Novedad,
                  Days = (int?)accrualHistoric.Dias_Novedad,
                  MedicalLeaveType = DianaAlternal2Code,
                });
                break;
              default:
                try
                {
                  payrollRecord.Accruals.Add(new Accrual
                  {
                    Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                    Amount = accrualHistoric.Val_Novedad,
                    Days = (int?)accrualHistoric.Dias_Novedad,
                    MedicalLeaveType = DianaAlternalCode,
                  });
                }
                catch (Exception e)
                {
                  throw e;
                }
                break;
            }
          }
          else
          {

            payrollRecord.Accruals.Add(new Accrual
            {
              Code = GetMappedConcept(accrualConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = accrualHistoric.Val_Novedad,
              Days = Convert.ToInt32(accrualHistoric.Dias_Novedad),
              Description = accrualConcept.Nom_Concepto.Trim(),

            });
          }


        }

        // Se llenan las deduciones
        foreach (CONCEPTOS deductionConcept in concepts.Values.Where(c => c.Devengo == "N" && c.Tipo_Concepto != "L"))
        {
          if (!historicsMapping.ContainsKey($"{deductionConcept.Cod_Concepto}-{employee.Cod_Empleado}"))
            continue;

          HISTORICO deductionHistoric = historicsMapping[$"{deductionConcept.Cod_Concepto}-{employee.Cod_Empleado}"];
          Deduction deductionMaped = new Deduction();

          var Code = GetMappedConcept(deductionConcept.Cod_Concepto.ToString(), conceptCodeMapping);
          var DianCode = GetMappedDianConcept(deductionConcept.Cod_Concepto.ToString(), conceptCodeMapping);

          if (Code.Contains("SALUD") || Code.Contains("FONDO_PENSION") || Code.Contains("FONDO_SOLIDARIDAD_PENSIONAL") || Code.Contains("FONDO_SUBSITENCIA") || Code.Contains("SINDICATO"))
          {
            deductionMaped = new Deduction
            {
              Code = GetMappedConcept(deductionConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = deductionHistoric.Val_Novedad,
              Percentage = deductionHistoric.Porcentaje
            };
            payrollRecord.Deductions.Add(deductionMaped);
          }
          else if (Code.Contains("OTRA_DEDUCCION"))
          {
            if (deductionHistoric.Val_Novedad != 0)
            {
              deductionMaped = new Deduction
              {
                Code = GetMappedConcept(deductionConcept.Cod_Concepto.ToString(), conceptCodeMapping),
                Amount = deductionHistoric.Val_Novedad,
                Description = deductionConcept.Nom_Concepto.Trim()
              };
              payrollRecord.Deductions.Add(deductionMaped);
            }

          }

          else if (Code.Contains("LIBRANZA"))
          {

            deductionMaped = new Deduction
            {
              Code = GetMappedConcept(deductionConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = deductionHistoric.Val_Novedad,
              Description = deductionConcept.Nom_Concepto.Trim()
            };

            payrollRecord.Deductions.Add(deductionMaped);
          }
          else
          {
            deductionMaped = new Deduction
            {
              Code = GetMappedConcept(deductionConcept.Cod_Concepto.ToString(), conceptCodeMapping),
              Amount = deductionHistoric.Val_Novedad,
            };
            payrollRecord.Deductions.Add(deductionMaped);

          }


        }

        // Validacion de salario integral
        if (employee.Tipo_Salario == "2")
        {
          IntegralSalary = true;
        }
        address = (CIUDADES)db.CIUDADES.SingleOrDefault(c => c.Codigo == employee.Cod_Ciudad);
        string var = address.Codigo_Dane.Trim();
        int codDane = var.Length;
        string startDate = "";
        string fireDate = "";
        string subCode = "";
        string workerType = "";
        string codDaneFormat = var.Substring((codDane - 3), 3);
        CultureInfo provider = CultureInfo.CreateSpecificCulture("es-CO");
        DateTime dateResult = new DateTime();
        dateResult = DateTime.ParseExact(employee.Fec_Ingreso, "yyyyMMdd", provider);
        startDate = dateResult.ToString("dd/MM/yyyy");
        if (String.IsNullOrEmpty(employee.Fec_Retiro))
        {
          dateResult = DateTime.ParseExact(employee.Fec_Retiro, "yyyyMMdd", provider);
          fireDate = dateResult.ToString("dd/MM/yyyy");
        }
        if (String.IsNullOrEmpty(employee.Tipo_Pension))
        {
          subCode = "NO_APLICA";
        }
        else
        {
          subCode = "NO_APLICA";
        }
        // Se llena la informacion del empleado
        string VSNombre = "";
        string VSApellido = "";
        if (employee.SNombre.Trim()=="") { VSNombre = "NN"; }else { VSNombre = employee.SNombre.Trim(); }
        if (employee.SApellido.Trim() == "") { VSApellido = "NN"; } else { VSApellido = employee.SApellido.Trim(); }

        string CodEmpleado = employee.Cod_Empleado.ToString();
        if (employee.Estado.Trim() != "R")
        {
          payrollRecord.Employee = new Employee
          {
            Code = CodEmpleado,
            Email = employee.Dir_Elec.Trim(),
            PaymentMeans = paymentMeansCodeMapping.ContainsKey(employee.FormaPago) ? paymentMeansCodeMapping[employee.FormaPago].Cod_Alterno : string.Empty,
            WorkerType = workerTypeCodeMapping.ContainsKey(employee.Tipo_Contrato) ? workerTypeCodeMapping[employee.Tipo_Contrato].Cod_Alterno : string.Empty,
            SubCode = subCode,
            StartDate = startDate,
            //FireDate = fireDate,
            HighRisk = false,
            IntegralSalary = IntegralSalary,
            ContractType = contractCodeMapping.ContainsKey(employee.Tipo_Contrato) ? contractCodeMapping[employee.Tipo_Contrato].Cod_Alterno : string.Empty,
            IdentificationType = documentCodeMapping.ContainsKey(employee.Tip_Documento) ? documentCodeMapping[employee.Tip_Documento].Cod_Alterno : string.Empty,
            Identification = employee.Cedula.Trim(),
            FirstName = employee.PNombre.Trim(),
            OtherNames = VSNombre,
            LastName = employee.PApellido.Trim(),
            SecondLastName = VSApellido,
            Bank = employee.Banco.Trim(),
            //AccountTypeKw = "",
            AccountNumber = employee.Num_Cta.Trim(),
            Address = new Address
            {
              City = codDaneFormat,
              Line = employee.Direccion.Trim(),
              Department = address.Codigo_Departamento.ToString(),
            }
          };
        }else
        {
          payrollRecord.Employee = new Employee
          {
            Code = CodEmpleado,
            Email = employee.Dir_Elec.Trim(),
            PaymentMeans = paymentMeansCodeMapping.ContainsKey(employee.FormaPago) ? paymentMeansCodeMapping[employee.FormaPago].Cod_Alterno : string.Empty,
            WorkerType = workerTypeCodeMapping.ContainsKey(employee.Tipo_Contrato) ? workerTypeCodeMapping[employee.Tipo_Contrato].Cod_Alterno : string.Empty,
            SubCode = subCode,
            StartDate = startDate,
            FireDate = fireDate,
            HighRisk = false,
            IntegralSalary = IntegralSalary,
            ContractType = contractCodeMapping.ContainsKey(employee.Tipo_Contrato) ? contractCodeMapping[employee.Tipo_Contrato].Cod_Alterno : string.Empty,
            IdentificationType = documentCodeMapping.ContainsKey(employee.Tip_Documento) ? documentCodeMapping[employee.Tip_Documento].Cod_Alterno : string.Empty,
            Identification = employee.Cedula.Trim(),
            FirstName = employee.PNombre.Trim(),
            OtherNames = VSNombre,
            LastName = employee.PApellido.Trim(),
            SecondLastName = VSApellido,
            Bank = employee.Banco.Trim(),
            //AccountTypeKw = "",
            AccountNumber = employee.Num_Cta.Trim(),
            Address = new Address
            {
              City = codDaneFormat,
              Line = employee.Direccion.Trim(),
              Department = address.Codigo_Departamento.ToString(),
            }
          };
        }

        payrollRecord.Software = softwareInfo;
        payrollInfo.Add(payrollRecord);

        List<Accrual> finalAccruals = VerifyDuplicateAccruals(payrollRecord.Accruals);

        List<Deduction> finalDeducctions = VerifyDuplicateDeducctions(payrollRecord.Deductions);

        if (finalAccruals != null)
        {
          payrollRecord.Accruals.Clear();
        }
        foreach (var item in finalAccruals)
        {
          payrollRecord.Accruals.Add(item);
        }

        if (finalDeducctions != null)
        {
          payrollRecord.Deductions.Clear();
        }
        foreach (var item in finalDeducctions)
        {
          payrollRecord.Deductions.Add(item);
        }

        object result = new object();

        var serilaizeJson = JsonConvert.SerializeObject(payrollInfo, Formatting.None,
             new JsonSerializerSettings
             {
               NullValueHandling = NullValueHandling.Ignore,
               //DefaultValueHandling = DefaultValueHandling.Ignore,
             });
        serilaizeJson = "{  \"entries\":" + serilaizeJson + "}";
        result = JsonConvert.DeserializeObject<Object>(serilaizeJson);
        string json = JsonConvert.SerializeObject(result);
        string path = HostingEnvironment.MapPath(@"~/Static/archivo.json");
        System.IO.File.WriteAllText(path, json);
        string path2 = HostingEnvironment.MapPath(@"~/Static/Nomina_Electronica/")+ db.Database.Connection.Database + "_" + tipoComprobante.SoporteNomina.Prefijo + "_" + data.ano + data.mes + ".json";
        System.IO.File.WriteAllText(path2, json);
      }
      //JObject rsp = null;
      //var jsonFile = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Static/archivo.json"));
      //string response = "";
      //string BaseUrl = "DataicoElectronicPayrollEndpoint";
      //string AuthToken = "DataicoElectronicPayrollAuthToken";
      //HttpWebService http = new HttpWebService(dataicoParameters[BaseUrl].Valor);
      //http.Headers.Add("auth-token", dataicoParameters[AuthToken].Valor);
      //if (http.PostRequest("/payroll-entries-batch", jsonFile, out response))
      //{
      //  rsp = JObject.Parse(response);
      //  string job_id = rsp.SelectToken("job-id")?.ToString();
      //  if (!string.IsNullOrEmpty(job_id))
      //    return Request.CreateResponse(System.Net.HttpStatusCode.OK, job_id);
      //  if (rsp.SelectToken("errors") != null)
      //    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, rsp);
      //}
      return Request.CreateResponse(System.Net.HttpStatusCode.OK);
      //return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
    }

    private List<Accrual> VerifyDuplicateAccruals(List<Accrual> accruals)
    {
      List<Accrual> listaNueva = new List<Accrual>();
      List<Accrual> listaSunDuplicados = new List<Accrual>();
      Accrual mapStatus = new Accrual(); 
      bool existeBasico = false;

      for (int i = 0; i < accruals.Count; i++)
      {
        if (new string[] { "BONO_EPCTV_ALIMENTACION" }.Contains(accruals[i].Code) && (accruals[i].Amount ?? 0.0) == 0.0)
          continue;

        if (!listaNueva.Contains(accruals[i]))
        {
          listaNueva.Add(accruals[i]);
        }
      }
      var DatosAgrupadosYSumados = listaNueva.GroupBy(x => x.Code).Select(x => new
      {
        Code = x.Key,
        Amount = x.Sum(y => y.Amount),
        AmountNs = x.Sum(n => n.AmountNs),
        Description = x.Select(t => t.Description),
        Days = x.Sum(d => d.Days),
        Hours = x.Sum(h => h.Hours),
        Percentage = x.Select(s => s.Percentage),
        MedicalLeaveType = x.Select(s => s.MedicalLeaveType),
        CesatiasInterest = x.Sum(l => l.CesantiasInterest)

      }).ToList();

      foreach (var item in DatosAgrupadosYSumados)
      {
        if (item.Code.Contains("BASICO"))
        {
          existeBasico = true;
        }

      }
      if (existeBasico == false)
      {
        mapStatus = new Accrual()
        {
          Code = "BASICO",
          Amount = 0,
          Days = 0
        };
        listaSunDuplicados.Add(mapStatus);

      }
      foreach (var trabajados in DatosAgrupadosYSumados)
      {
        string formatDays = trabajados.Days.ToString();

        
          if (formatDays == "0")
          {

            if (trabajados.Code.Contains("OTRO_CONCEPTO"))
            {
              if (trabajados.Amount != 0 && trabajados.AmountNs != 0)
              {
                mapStatus = new Accrual()
                {
                  Code = trabajados.Code,
                  Amount = trabajados.Amount,
                  AmountNs = trabajados.AmountNs,
                  Description = trabajados.Description.First().Trim(),
                  Days = null
                };
              }
              else if (trabajados.Amount != 0)
              {
                mapStatus = new Accrual()
                {
                  Code = trabajados.Code,
                  Amount = trabajados.Amount,
                  AmountNs = null,
                  Description = trabajados.Description.First().Trim(),
                  Days = null
                };
              }
              else
              {
                mapStatus = new Accrual()
                {
                  Code = trabajados.Code,
                  Amount = null,
                  AmountNs = trabajados.AmountNs,
                  Description = trabajados.Description.First().Trim(),
                  Days = null
                };
              }
            }
            else if (trabajados.Code.Contains("BASICO"))
            {

              if (trabajados.Days > 30)
              {
                mapStatus = new Accrual
                {
                  Code = trabajados.Code,
                  Amount = trabajados.Amount,
                  Days = 30,
                };
              }
              else
                mapStatus = new Accrual
                {
                  Code = trabajados.Code,
                  Amount = trabajados.Amount,
                  Days = trabajados.Days,
                };


            }
            else if (trabajados.Code.Contains("PRIMA"))
            {

              mapStatus = new Accrual
              {
                Code = trabajados.Code,
                AmountNs = trabajados.AmountNs,
                Days = trabajados.Days,
              };


            }

          else if (trabajados.Code.Contains("AUXILIO_DE_TRANSPORTE"))
            {

              mapStatus = new Accrual
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                Days = null,
              };

            }
            else if (trabajados.Code.Contains("CESANTIAS"))
            {
              if (trabajados.CesatiasInterest != 0)
              {
                mapStatus = new Accrual()
                {
                  Code = trabajados.Code,
                  AmountNs = trabajados.AmountNs,
                  CesantiasInterest = trabajados.CesatiasInterest,
                  Percentage = trabajados.Percentage.First()
                };
              }
              else
              {
                mapStatus = new Accrual()
                {
                  Code = trabajados.Code,
                  AmountNs = trabajados.AmountNs,
                  CesantiasInterest = null,
                  Percentage = trabajados.Percentage.First()
                };
              }

            }
            else if (trabajados.Code.Contains("INCAPACIDAD"))
            {
              mapStatus = new Accrual()
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                Days = trabajados.Days,
                MedicalLeaveType = trabajados.MedicalLeaveType.First()
              };
            }
            else if (trabajados.Code.Contains("ANTICIPO"))
            {                            
                mapStatus = new Accrual()
                {
                  Code = trabajados.Code,
                  Amount = trabajados.Amount,
                };              
            }
            else if (trabajados.Code.Contains("HORA_EXTRA_DIURNA") || trabajados.Code.Contains("HORA_EXTRA_NOCTURNA") || trabajados.Code.Contains("HORA_RECARGO_NOCTURNO") || trabajados.Code.Contains("HORA_EXTRA_DIURNA_DF") || trabajados.Code.Contains("HORA_EXTRA_DIURNA_DF") || trabajados.Code.Contains("HORA_RECARGO_DIURNA_DF") || trabajados.Code.Contains("HORA_RECARGO_NOCTURNO_DF"))
            {
              mapStatus = new Accrual()
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                AmountNs = null,
                Days = null,
                Hours = trabajados.Hours

              };
            }
            else if (trabajados.Code.Contains("VACACION"))
            {
              mapStatus = new Accrual
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                AmountNs = null,
                Days = trabajados.Days,
              };
            }
          else if (trabajados.AmountNs != 0.0 && trabajados.Amount != 0.0)
            {
              mapStatus = new Accrual()
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                AmountNs = trabajados.AmountNs,
                Days = null,
                Hours = null

              };
            }
            else if (trabajados.AmountNs != 0.0)
            {
              mapStatus = new Accrual()
              {
                Code = trabajados.Code,
                Amount = null,
                AmountNs = trabajados.AmountNs,
                Days = null,
                Hours = null

              };
            }
            else
              mapStatus = new Accrual()
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                AmountNs = null,
              };
            listaSunDuplicados.Add(mapStatus);
          }
          else if (trabajados.Code.Contains("BASICO"))
          {
            if (trabajados.Days >= 30)
            {
              mapStatus = new Accrual
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                Days = 30,
              };
              listaSunDuplicados.Add(mapStatus);

            }
            else if (trabajados.Amount == 0)
            {
              mapStatus = new Accrual
              {
                Code = trabajados.Code,
                Amount = 0,
                Days = 30,
              };
              listaSunDuplicados.Add(mapStatus);

            }
            else
            {
              mapStatus = new Accrual
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                Days = trabajados.Days,
              };
              listaSunDuplicados.Add(mapStatus);
            }
          }
          else if (trabajados.Code.Contains("INCAPACIDAD"))
          {
            mapStatus = new Accrual()
            {
              Code = trabajados.Code,
              Days = trabajados.Days,
              Amount = trabajados.Amount,
              MedicalLeaveType = trabajados.MedicalLeaveType.First()
            };
            listaSunDuplicados.Add(mapStatus);

          }
          else if (trabajados.Code.Contains("LICENCIA_NO_REMUNERADA"))
          {
            mapStatus = new Accrual()
            {
              Code = trabajados.Code,
              Days = trabajados.Days,
            };
            listaSunDuplicados.Add(mapStatus);

          }
          else if (trabajados.Code.Contains("AFC"))
          {

            if (trabajados.Amount == null && trabajados.AmountNs == null)
            {
              mapStatus = new Accrual()
              {
                Code = null,
                Days = null,
                Amount = null,
              };
            }


          }
          else if (trabajados.Code.Contains("INDEMNIZACION"))
          {
            mapStatus = new Accrual()
            {
              Code = trabajados.Code,
              Amount = trabajados.Amount,
              Days = null,
            };
            listaSunDuplicados.Add(mapStatus);

          }
          else if (trabajados.Code.Contains("ANTICIPO"))
          {            
            mapStatus = new Accrual()
            {
              Code = trabajados.Code,
              Amount = trabajados.Amount,
              Days = null,
            };
            listaSunDuplicados.Add(mapStatus);            
          }
          else if (trabajados.Code.Contains("PAGO_TERCERO"))
          {
            mapStatus = new Accrual()
            {
              Code = trabajados.Code,
              Amount = trabajados.Amount,
              Days = null,
            };
            listaSunDuplicados.Add(mapStatus);

          }
          else if (trabajados.Code.Contains("VACACION_COMPENSADA"))
          {
            mapStatus = new Accrual
            {
              Code = trabajados.Code,
              AmountNs = trabajados.AmountNs,
              Amount = null,
              Days = trabajados.Days,
            };
            listaSunDuplicados.Add(mapStatus);
          }
          else if (trabajados.Code.Contains("VACACION"))
          {
            mapStatus = new Accrual
            {
              Code = trabajados.Code,
              Amount = trabajados.Amount,
              AmountNs = null,
              Days = trabajados.Days,
            };
            listaSunDuplicados.Add(mapStatus);
          }
        else if (trabajados.Code.Contains("OTRO_CONCEPTO"))
          {
            if (trabajados.AmountNs != 0.0)
            {
              mapStatus = new Accrual()
              {
                Code = trabajados.Code,
                Amount = null,
                AmountNs = trabajados.AmountNs,
                Description = trabajados.Description.First(),
                Days = null,
                Hours = null

              };
              listaSunDuplicados.Add(mapStatus);

            }

            else
              mapStatus = new Accrual()
              {
                Code = trabajados.Code,
                Amount = trabajados.Amount,
                AmountNs = null,
                Description = trabajados.Description.First(),
                Days = null
              };
            listaSunDuplicados.Add(mapStatus);

          }
          else if (trabajados.Code.Contains("PRIMA"))
          {
            //if (trabajados.Days > 30)
            //{
            //  mapStatus = new Accrual
            //  {
            //    Code = trabajados.Code,
            //    Amount = trabajados.Amount,
            //    Days = 30,
            //  };
            //  listaSunDuplicados.Add(mapStatus);

            //}
            //else
            mapStatus = new Accrual
            {
              Code = trabajados.Code,
              Amount = trabajados.Amount,
              Days = trabajados.Days,
            };
            listaSunDuplicados.Add(mapStatus);

          }
          else if (trabajados.Code.Contains("AFC"))
          {

            if (trabajados.Amount == null && trabajados.AmountNs == null)
            {
              mapStatus = new Accrual()
              {
                Code = null,
                Days = null,
                Amount = null,
              };
            }


          }
          else
          {
            mapStatus = new Accrual()
            {
              Code = trabajados.Code,
              Amount = trabajados.Amount,
              AmountNs = trabajados.AmountNs,
              Days = trabajados.Days,
              Hours = null
            };
            listaSunDuplicados.Add(mapStatus);
          }
        }
      
      return listaSunDuplicados;
    }

    private List<Deduction> VerifyDuplicateDeducctions(List<Deduction> deductions)
    {
      List<Deduction> listaNueva = new List<Deduction>();
      List<Deduction> listaSunDuplicados = new List<Deduction>();
      for (int i = 0; i < deductions.Count; i++)
      {
        if (new string[] { "PLANES_COMPLEMENTARIOS" }.Contains(deductions[i].Code) &&  (deductions[i].Amount ?? 0.0) == 0.0)
          continue;
        if (new string[] { "RETENCION_FUENTE" }.Contains(deductions[i].Code) && (deductions[i].Amount ?? 0.0) == 0.0)
          continue;
        if (new string[] { "PAGO_TERCERO" }.Contains(deductions[i].Code) && (deductions[i].Amount ?? 0.0) == 0.0)
          continue;
        if (!(listaNueva.Contains(deductions[i])))
        {
          listaNueva.Add(deductions[i]);
        }
      }
      var DatosAgrupadosYSumados = listaNueva.GroupBy(x => x.Code).Select(x => new
      {
        Code = x.Key,
        Amount = x.Sum(y => y.Amount),
        Percentage = x.Select(s => s.Percentage),
        Description = x.Select(y => y.Description)

      }).ToList();

      Deduction deductionMaped = new Deduction();

      foreach (var trabajados in DatosAgrupadosYSumados)
      {
        if (trabajados.Code.Contains("SALUD") || trabajados.Code.Contains("FONDO_PENSION") || trabajados.Code.Contains("FONDO_SUBSITENCIA") || trabajados.Code.Contains("SINDICATO"))
        {
          double percentage = (double)trabajados.Percentage.First();
          percentage = (double)Math.Round((double)percentage, 2);
          deductionMaped = new Deduction
          {
            Code = trabajados.Code,
            Amount = trabajados.Amount,
            Percentage = percentage
          };
          listaSunDuplicados.Add(deductionMaped);

        }
        else if (trabajados.Code.Contains("AFC"))
        {

          if (trabajados.Amount == 0)
          {
            deductionMaped = new Deduction()
            {
              Code = null,
              Amount = null
            };
          }
          else
          {
            deductionMaped = new Deduction()
            {
              Code = trabajados.Code,
              Amount = trabajados.Amount
            };
            listaSunDuplicados.Add(deductionMaped);

          }
        }
        else if (trabajados.Code.Contains("OTRA_DEDUCCION"))
        {

          deductionMaped = new Deduction
          {
            Code = trabajados.Code,
            Amount = trabajados.Amount,
            Description = trabajados.Description.First().Trim()
          };

          listaSunDuplicados.Add(deductionMaped);
        }
        else if (trabajados.Code.Contains("LIBRANZA"))
        {
          deductionMaped = new Deduction
          {
            Code = trabajados.Code,
            Amount = trabajados.Amount,
            Description = trabajados.Description.First().Trim()
          };

          listaSunDuplicados.Add(deductionMaped);
        }

        else if (trabajados.Code.Contains("FONDO_SOLIDARIDAD_PENSIONAL"))
        {
          if (trabajados.Amount == 0.0)
          {

          }
          else if (trabajados.Percentage.First() != 0)
          {
            var Porcentage = trabajados.Percentage.First() / 100;
            double CalculoAmount = (double)((trabajados.Amount / Porcentage) * 0.005);
            CalculoAmount = Math.Round(CalculoAmount);
            deductionMaped = new Deduction
            {
              Code = trabajados.Code,
              Amount = CalculoAmount,
              Percentage = 0.5
            };
            listaSunDuplicados.Add(deductionMaped);

            deductionMaped = new Deduction
            {
              Code = "FONDO_SUBSISTENCIA",
              Amount = trabajados.Amount - CalculoAmount,
              Percentage = trabajados.Percentage.First() - 0.5
            };
            listaSunDuplicados.Add(deductionMaped);
          }
          else
          {

          }
        }
        else
        {
          deductionMaped = new Deduction
          {
            Code = trabajados.Code,
            Amount = trabajados.Amount,
          };
          listaSunDuplicados.Add(deductionMaped);

        }

      }
      return listaSunDuplicados;
    }

    public STATUS_NOM_ELEC GuardarRespuesta(/*PayrollResponse response, */string historicDate, ref int consecutivo, short Cod_Empleado, string Cod_Archivo, string Empresa, string tipoPrefijo)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var registroAntiguo = db.STATUS_NOM_ELEC
                                .Where(c => c.Number == Cod_Empleado && c.Fec_Nomina == historicDate && c.Prefix == tipoPrefijo)
                                .OrderByDescending(c => c.id)
                                .FirstOrDefault();

      if (registroAntiguo != null)
      {
        consecutivo--;
        return registroAntiguo;
      }

      STATUS_NOM_ELEC mapStatus = new STATUS_NOM_ELEC()
      {
        Xml = null,
        Pdf = null,
        Cune = null,
        Number = Cod_Empleado,
        Qrcode = null,       
        DianStatus = "ENVIADO_POR_LOTE",
        EmailStatus = "ENVIADO_POR_LOTE",
        Fec_Nomina = historicDate,
        Consecutivo = consecutivo,
        Prefix =  tipoPrefijo
      };
      db.STATUS_NOM_ELEC.Add(mapStatus);
      db.SaveChanges();

      return mapStatus;
    }

    private Dictionary<string, MAPEO_NOM_ELEC> GenerateMappingCodes(string type, string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      return db.MAPEO_NOM_ELEC
                .Where(m => m.Tipo_Mapeo.Nombre == type)
                .ToDictionary(m => m.Cod_Concepto.ToString(), m => m);
    }

    public string GetMappedConcept(string code, Dictionary<string, MAPEO_NOM_ELEC> mapping)
    {
      if (!mapping.ContainsKey(code))
        return code;

      return mapping[code].Cod_Alterno.Trim();
    }
    public string GetMappedDianConcept(string code, Dictionary<string, MAPEO_NOM_ELEC> mapping)
    {
      if (!mapping.ContainsKey(code))
        return code;

      return mapping[code].Dian_xml.Trim();
    }

    public string GetMappedAlternalDianConcept(string code, Dictionary<string, MAPEO_NOM_ELEC> mapping)
    {
      if (!mapping.ContainsKey(code))
        return code;

      return mapping[code].Descripcion.Trim();
    }

    public string GetMappedAlternalDian2Concept(string code, Dictionary<string, MAPEO_NOM_ELEC> mapping)
    {
      if (!mapping.ContainsKey(code))
        return code;

      return mapping[code].Cod_Alterno2.Trim();
    }

    private Dictionary<string, HISTORICO> GetHistoricMapping(JulianaContext db, string historicStartDate, List<short> conceptIds)
    {
      string YearMonth = UtilHelper.getDate(historicStartDate).ToString("yyyyMM");

      Dictionary<string, HISTORICO> historicsMapping = new Dictionary<string, HISTORICO>();
      var rawResults = db.HISTORICO
           .Where(h => h.Fec_Nomina.StartsWith(YearMonth) &&
                       h.Estado == "P" &&
                       conceptIds.Contains(h.Cod_Concepto))
           .OrderBy(c => c.Cod_Concepto)
           .ToList();


      var query = rawResults.GroupBy(h => $"{h.Cod_Concepto}-{h.Cod_Empleado}");

      foreach (var group in query)
      {
        HISTORICO historic = group.FirstOrDefault();       
        historic.Val_Novedad = group.Sum(i => i.Val_Novedad);
        historic.Dias_Novedad = group.Sum(d => d.Dias_Novedad);
        historicsMapping.Add(group.Key, historic);
      }

      return historicsMapping;
    }
  }
}
