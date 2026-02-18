using IdentityModel.Client;
using JulianaWeb.Interfaces.Aspects;
using JulianaWeb.Models;
using JulianaWeb.Services.Logging;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace JulianaWeb.Business
{
  public class SsoServiceBO
  {
    private readonly HttpClient client;
    private readonly string julianaApi;
    private readonly ILoggerService<SsoServiceBO> _logger;

    public SsoServiceBO()
    {
        this.client = new HttpClient();
        this.julianaApi = ConfigurationManager.AppSettings["JulianaApiUrl"];
        _logger = LoggerServiceFactory.Get<SsoServiceBO>();
    }

    public async Task<LoginUserResponse> GetUserLogin(string username)
    {


      // Get the current HTTP request context
      var context = HttpContext.Current;

      // Determine the base URL dynamically
      string baseUrl = context.Request.Url.GetLeftPart(UriPartial.Authority);

      using (HttpClient httpClient = new HttpClient())
      {
        // Set base address for HttpClient
        baseUrl = (!string.IsNullOrWhiteSpace(this.julianaApi) ? this.julianaApi : baseUrl);
        var endPoint = $"{baseUrl}/API/Account/login";

        try
        {
          _logger.LogDebug($"trying user sso local login to {endPoint}");

          // Send GET request to the API
          HttpResponseMessage response = await httpClient.PostAsJsonAsync(endPoint, new
          {
            username = username,
            InternalLoginCode = ConfigurationManager.AppSettings["auth0:InternalLoginCode"]
          });

          // Check if request was successful
          if (response.IsSuccessStatusCode)
          {
            // Read response content
            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("Response: " + responseBody);

            return JsonConvert.DeserializeObject<LoginUserResponse>(responseBody);
          }

          _logger.LogDebug($"request: {response.RequestMessage}");
          _logger.LogDebug($"response: {response}");

          return new LoginUserResponse { error = "Failed to make request. Status code: " + response.StatusCode };
        }
        catch (HttpRequestException e)
        {
          _logger.LogError("Error when getting user sso local login: ", e);
          Debug.WriteLine("Error: " + e.Message);
        }
      }

      return new LoginUserResponse { error = "Invalid User" };
    }
  }
}
