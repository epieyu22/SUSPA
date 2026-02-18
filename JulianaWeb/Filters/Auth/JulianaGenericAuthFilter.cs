using JulianaWeb.Models;
using JulianaWeb.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace JulianaWeb.Filters.Auth
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
  public class JulianaGenericAuthFilter : AuthorizationFilterAttribute
  {
    private readonly bool _isActive = true;

    public bool IsOpen { get; set; }

    public JulianaGenericAuthFilter(bool IsActive = true, bool IsOpen = false)
    {
      _isActive = IsActive;
      this.IsOpen = IsOpen;
    }

    public override void OnAuthorization(HttpActionContext filterContext)
    {
      if (!_isActive) return;

      var identity = FetchAuthHeader(filterContext);

      if (identity == null)
      {
        ChallengeAuthRequest(filterContext);
        return;
      }

      var genericIdentity = new GenericIdentity(identity.UserName);
      var genericPrincipal = new GenericPrincipal(genericIdentity, identity.Roles.Select(r => r.RoleId).ToArray());

      Thread.CurrentPrincipal = genericPrincipal;

      if (!OnAuthorizeUser(identity.UserName, identity.PasswordHash, filterContext))
      {
        ChallengeAuthRequest(filterContext);
        return;
      }

      base.OnAuthorization(filterContext);
    }

    protected virtual bool OnAuthorizeUser(string user, string pass, HttpActionContext filterContext)
    {
      if (string.IsNullOrEmpty(user))
        return false;
      return true;
    }

    protected virtual ApplicationUser FetchAuthHeader(HttpActionContext filterContext)
    {
      string authHeaderValue = null;
      bool isCookieAuth = false;
      bool isTokenAuth = false;

      var userCookie = filterContext.Request.Headers.GetCookies("authorizationData").FirstOrDefault()?["authorizationData"];
      var authHeader = filterContext.Request.Headers.Authorization;

      isCookieAuth = userCookie != null && !string.IsNullOrWhiteSpace(userCookie.Value);
      isTokenAuth = authHeader != null && authHeader.Scheme == "Bearer";

      if(isCookieAuth)
      {
        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(userCookie.Value);

        if(data == null || (data != null && (!data.ContainsKey("loggedIn") || !data.ContainsKey("UserName"))))
        {
          throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }

        using(var _repo = new AuthRepository())
        {
          return _repo.FindUser(data["UserName"].ToString());
        }
      }

      // Solo funciona con Cookies si no está abierto
      if (!this.IsOpen)
        return null;

      if (isTokenAuth && filterContext.ControllerContext.RequestContext.Principal.Identity != null)
      {
        IIdentity identity = filterContext.ControllerContext.RequestContext.Principal.Identity;
        return new ApplicationUser {
            UserName = identity.Name
        };
      }

      return null;
    }

    private static void ChallengeAuthRequest(HttpActionContext filterContext)
    {
      var dnsHost = filterContext.Request.RequestUri.DnsSafeHost;
      filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
    }

  }
}
