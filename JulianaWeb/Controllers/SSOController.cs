using JulianaWeb.Business;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI;

namespace JulianaWeb.Controllers
{
  public class SSOController : Controller
  {
    private string auth0Domain;
    private string auth0ClientId;
    private string auth0RedirectUri;
    private string auth0PostLogoutRedirectUri;
    private SsoServiceBO ssoService;
    private HttpClient httpClient;

    public SSOController()
    {
      this.httpClient = new HttpClient();
      this.auth0Domain = ConfigurationManager.AppSettings["auth0:Domain"];
      this.auth0ClientId = ConfigurationManager.AppSettings["auth0:ClientId"];
      this.auth0RedirectUri = ConfigurationManager.AppSettings["auth0:RedirectUri"];
      this.auth0PostLogoutRedirectUri = ConfigurationManager.AppSettings["auth0:PostLogoutRedirectUri"];
      this.ssoService = new SsoServiceBO();
    }

    public async Task<ActionResult> Login()
    {
      if (!HttpContext.User.Identity.IsAuthenticated)
      {
        HttpContext.GetOwinContext().Authentication.Challenge(OpenIdConnectAuthenticationDefaults.AuthenticationType);
        return new HttpUnauthorizedResult();
      }

      return await GetUserAndRedirect();
    }

    private async Task<ActionResult> GetUserAndRedirect()
    {
      var user = (System.Security.Claims.ClaimsIdentity)HttpContext.User.Identity;
      var julianaUserResponse = await ssoService.GetUserLogin(user.FindFirst(ConfigurationManager.AppSettings["auth0:UsernameClaim"])?.Value);

      if(!string.IsNullOrWhiteSpace(julianaUserResponse.error))
      {
        HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType, OpenIdConnectAuthenticationDefaults.AuthenticationType);
      }

      TempData["sso-auth-info"] = JsonConvert.SerializeObject(
        julianaUserResponse
      );

      return RedirectToAction("Index", "Home");
    }

    public void Logout()
    {
      if (HttpContext.User.Identity.IsAuthenticated)
      {
        HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType, OpenIdConnectAuthenticationDefaults.AuthenticationType);
      }
    }

    public ActionResult PostLogout()
    {
      return RedirectToAction("Index", "Home");
    }
  }
}
