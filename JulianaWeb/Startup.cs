using IdentityModel.Client;
using JulianaWeb.Providers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Layouts;

[assembly: OwinStartup(typeof(JulianaWeb.Startup))]
namespace JulianaWeb
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // Configure NLog for logging to a file
      ConfigureLogging();
      ConfigureOAuth(app);
      BundleConfig.RegisterScriptBundles(BundleTable.Bundles);
      AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
      GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Re‌​ferenceLoopHandling = ReferenceLoopHandling.Ignore;
    }

    public void ConfigureOAuth(IAppBuilder app)
    {
      System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      // Configure Auth0 parameters
      // TODO: Refactor this code, too much happening in this method
      string auth0Domain = ConfigurationManager.AppSettings["auth0:Domain"];
      string auth0Authority = ConfigurationManager.AppSettings["auth0:Authority"];
      string auth0ClientId = ConfigurationManager.AppSettings["auth0:ClientId"];
      string auth0ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"];
      string auth0RedirectUri = ConfigurationManager.AppSettings["auth0:RedirectUri"];
      string auth0PostLogoutRedirectUri = ConfigurationManager.AppSettings["auth0:PostLogoutRedirectUri"];
      string auth0TokenEndpoint = ConfigurationManager.AppSettings["auth0:TokenEndpoint"];
      string auth0UserInfoEndpoint = ConfigurationManager.AppSettings["auth0:UserInfoEndpoint"];

      app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        CookieManager = new SystemWebCookieManager()
      });

      // Then, configure OpenID Connect
      app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
      {
        ClientId = auth0ClientId,
        ClientSecret = auth0ClientSecret,
        Authority = auth0Authority,
        RedirectUri = auth0RedirectUri,
        PostLogoutRedirectUri = auth0PostLogoutRedirectUri,
        ResponseType = OpenIdConnectResponseType.CodeIdToken,
        Scope = $"{OpenIdConnectScope.OpenIdProfile} {OpenIdConnectScope.Email}",
        TokenValidationParameters = new TokenValidationParameters
        {

          NameClaimType = "name"
        },
        //CookieManager = new SystemWebCookieManager(),
        Notifications = new OpenIdConnectAuthenticationNotifications
        {
          AuthorizationCodeReceived = async n =>
          {
            // Exchange code for access and ID tokens
            var tokenClient = new TokenClient(auth0TokenEndpoint, auth0ClientId, auth0ClientSecret);
            var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(n.Code, auth0RedirectUri);

            if (tokenResponse.IsError)
            {
              throw new Exception(tokenResponse.Error);
            }

            var userInfoClient = new UserInfoClient(auth0UserInfoEndpoint);
            var userInfoResponse = await userInfoClient.GetAsync(tokenResponse.AccessToken);
            var claims = new List<Claim>();
            claims.AddRange(userInfoResponse.Claims);
            claims.Add(new Claim("id_token", tokenResponse.IdentityToken));
            claims.Add(new Claim("access_token", tokenResponse.AccessToken));

            if (!string.IsNullOrEmpty(tokenResponse.RefreshToken))
            {
              claims.Add(new Claim("refresh_token", tokenResponse.RefreshToken));
            }

            n.AuthenticationTicket.Identity.AddClaims(claims);

            return;
          },

          RedirectToIdentityProvider = n =>
          {
            // If signing out, add the id_token_hint
            if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Logout)
            {
              var idTokenClaim = n.OwinContext.Authentication.User.FindFirst("id_token");

              if (idTokenClaim != null)
              {
                n.ProtocolMessage.IdTokenHint = idTokenClaim.Value;
              }

            }

            return Task.CompletedTask;
          }
        }
      });


      OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
      {
        AllowInsecureHttp = true,
        TokenEndpointPath = new PathString("/token"),
        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
        Provider = new SimpleAuthorizationServerProvider(),
      };

      // Token Generation
      //app.UseOAuthAuthorizationServer(OAuthServerOptions);
      //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions{});

    }

    private void ConfigureLogging()
    {
      var logLayout = new CsvLayout()
      {
        Delimiter = CsvColumnDelimiterMode.Pipe,
        WithHeader = true,
        Columns =
         {
            new CsvColumn("time", "${longdate}"),
            new CsvColumn("level", "${level:upperCase=true}"),
            new CsvColumn("message", "${message}"),
            new CsvColumn("exception", "${exception:format=ToString}"),
         }
      };

      // Set up NLog to save logs to a file in a "Logs" folder
      var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
      Directory.CreateDirectory(logDirectory); // Ensure the directory exists

      var logFilePath = Path.Combine(logDirectory, "log.txt");

      // Configure NLog programmatically
      var config = new LoggingConfiguration();

      var fileTarget = new FileTarget
      {
        Name = "file",
        FileName = logFilePath,
        CreateDirs = true,
        MaxArchiveFiles = 10,
        Layout = logLayout
      };

      var consoleTarget = new DebugSystemTarget
      {
        Name = "console",
        Layout = logLayout,
      };

      config.AddTarget(fileTarget);
      config.AddRuleForAllLevels(fileTarget);
      config.AddTarget(consoleTarget);
      config.AddRuleForAllLevels(consoleTarget);

      LogManager.Configuration = config;

      var logger = LogManager.GetLogger("logger");

      // Log some messages for testing
      logger.Info("Initial Test: Application started.");
      logger.Warn("Initial Test: Warning: Something might be wrong.");
      logger.Error("Initial Test: Error: Something went terribly wrong!");
    }

    protected void Application_PostAuthorizeRequest()
    {
      System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
    }

  }
}
