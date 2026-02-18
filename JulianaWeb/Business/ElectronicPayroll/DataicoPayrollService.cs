using JulianaWeb.Interfaces.Business.WebServices.ElectronicPayroll;
using JulianaWeb.Models;
using JulianaWeb.Models.Juliana.NominaElectronica;
using JulianaWeb.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace JulianaWeb.Business.ElectronicPayroll
{
  public class DataicoPayrollService :
    IElectronicPayrollSave<ElectronicPayrollInfo, Task<PayrollResponse>>,
    IElectronicPayrollUpdate<ElectronicPayrollInfo, Task<PayrollResponse>>

  {
    private const string DataicoAuthToken = "DataicoElectronicPayrollAuthToken";
    private const string DataicoAccountKey = "DataicoElectronicPayrollAccountId";
    private const string DataicoTestAccountKey = "DataicoElectronicPayrollDianTestId";

    private const string DataicoEndPoint = "DataicoElectronicPayrollEndpoint";
    private const string AuthTokenHeader = "auth-token";
    private const string DataicoAccountHeader = "dataico_account_id";
    private readonly JulianaContext context;
    private readonly Dictionary<string, string> dataicoParams;

    public DataicoPayrollService(JulianaContext context) 
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      this.context = context;
      this.dataicoParams = context.PARAMETROS_GENERALES
                                .Where(p => p.Cod_Parametro.Contains("Dataico"))
                                .ToDictionary(p => p.Cod_Parametro, p => p.Valor);
    }

    public async Task<PayrollResponse> Save(ElectronicPayrollInfo payload)
    {
      if (string.IsNullOrWhiteSpace(dataicoParams[DataicoEndPoint]))
        throw new ArgumentException("No esta configurada la url de la api de Dataico");

      using (var client = HttpClientFactory.Create())
      {
        client.DefaultRequestHeaders.Add(AuthTokenHeader, dataicoParams[DataicoAuthToken]);
        client.DefaultRequestHeaders.Add(DataicoAccountHeader, dataicoParams[DataicoAccountKey]);
        client.DefaultRequestHeaders.Add(DataicoTestAccountKey, dataicoParams[DataicoTestAccountKey]);

        PayrollResponse payrollResponse = new PayrollResponse();
        Object result = new Object();


        var serilaizeJson = JsonConvert.SerializeObject(payload, Formatting.None,
            new JsonSerializerSettings
            {
              NullValueHandling = NullValueHandling.Ignore,
            });

        if (payload.Deductions.Count == 0)
        {
          JObject jo = JObject.Parse(serilaizeJson);
          jo.Property("deductions").Remove();
          var json = jo.ToString();
          result = JsonConvert.DeserializeObject<Object>(json);
        }
        else {
          result = JsonConvert.DeserializeObject<Object>(serilaizeJson);

        }


        var response = await client.PostAsJsonAsync($"{dataicoParams[DataicoEndPoint]}/payroll-entries", result);

        payrollResponse = JsonConvert.DeserializeObject<PayrollResponse>(response.Content.ReadAsStringAsync().Result);

        return payrollResponse;

      }
    }

    public async Task<PayrollResponse> Update(ElectronicPayrollInfo payload)
    {
      if (string.IsNullOrWhiteSpace(dataicoParams[DataicoEndPoint]))
        throw new ArgumentException("No esta configurada la url de la api de Dataico");

      using (var client = HttpClientFactory.Create())
      {
        client.DefaultRequestHeaders.Add(AuthTokenHeader, dataicoParams[DataicoAuthToken]);
        client.DefaultRequestHeaders.Add(DataicoAccountHeader, dataicoParams[DataicoAccountKey]);
        PayrollResponse payrollResponse = new PayrollResponse();
       
        var serilaizeJson = JsonConvert.SerializeObject(payload, Formatting.None,
            new JsonSerializerSettings
            {
              NullValueHandling = NullValueHandling.Ignore,

            });
        
        Object result = JsonConvert.DeserializeObject<Object>(serilaizeJson);
        var response = await client.PostAsJsonAsync($"{dataicoParams[DataicoEndPoint]}/payroll-replacements", result);

        payrollResponse = JsonConvert.DeserializeObject<PayrollResponse>(response.Content.ReadAsStringAsync().Result);

        return payrollResponse;
      }
    }


  }
}
