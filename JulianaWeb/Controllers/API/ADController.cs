using JulianaWeb.Models;
using JulianaWeb.Repositories;
using System;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/AD")]
  public class ADController : ApiController
  {
    PrincipalContext ADContext = null;
    UserPrincipal ADUser = null;

    [Route("Login")]
    [HttpGet]
    public IHttpActionResult ADLogin()
    {
      string domain = ConfigurationManager.AppSettings["ADDomain"];
      string windowsUsername = System.Web.HttpContext.Current.User.Identity.Name;
      ADContext = new PrincipalContext(ContextType.Domain, domain);
      ADUser = UserPrincipal.FindByIdentity(ADContext, windowsUsername);

      string docIdentidad = String.Empty;
      string ADField = ConfigurationManager.AppSettings["ADIdField"];

      if (ADUser.GetUnderlyingObjectType() == typeof(DirectoryEntry))
      {
        // Transition to directory entry to get other properties
        using (var entry = (DirectoryEntry)ADUser.GetUnderlyingObject())
        {
          if (entry.Properties[ADField] != null)
          {
            docIdentidad = entry.Properties[ADField].Value.ToString();
          }
        }
      }

      if (docIdentidad != String.Empty)
      {
        AuthContext _db = new AuthContext();
        AuthManager manager = new AuthManager();
        var user = _db.Users.FirstOrDefault(u => u.UserName == docIdentidad);
        if (user != null)
        {
          return Ok(new { username = user.UserName });
        }
        else
        {
          JulianaContext defaultDb = new JulianaContext();
          var empresas = defaultDb.EMPRESAS
                                  .Where(e => e.Estado == "A")
                                  .Select(e => e.BaseDatos);
          foreach (var BaseDatos in empresas)
          {
            JulianaContext db = new JulianaContext(BaseDatos.Trim());
            EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(
                                    ep => ep.Cedula == docIdentidad
                                    && ep.Estado != "R");
            if (empleado != null)
            {
              ApplicationUser newUser = new ApplicationUser()
              {
                UserName = docIdentidad,
                DBName = BaseDatos.Trim(),
                Empleado = empleado.Empleado,
              };
              manager.CreateUser(newUser, docIdentidad);
              //newUser = _db.Users.First(u => u.UserName == docIdentidad);
              ApplicationGroup empleadoGroup = _db.Groups.First(g => g.Name == "Empleado");
              manager.AddUserToGroup(newUser.Id, empleadoGroup.Id);
              return Ok(new { username = newUser.UserName });
            }
            else
            {
              return Ok(new { error = "El documento de identificación no pertenece a ningún empleado activo dentro de la base de datos de nomina" });
            }
          }
          return Ok(new { error = String.Format("No se ha encontrado un Empleado activo con el documento de identidad {0}", docIdentidad) });
        }
      }
      else
      {
        return BadRequest("Su usuario del directorio activo no está configurado para ingresar al sistema comuniquese con el equipo de sistemas.");
      }
    }



    [Route("RegularLogin")]
    [HttpPost]
    public IHttpActionResult RegularLogin([FromBody] LoginViewModel data)
    {
      string Domain = ConfigurationManager.AppSettings["ADDomain"];
      string ADField = ConfigurationManager.AppSettings["ADIdField"];
      string UseADLogin = ConfigurationManager.AppSettings["UseADLogin"];


      using (PrincipalContext context = new PrincipalContext(ContextType.Domain, Domain))
      {
        // validate the credentials

        bool isValid = context.ValidateCredentials(data.username, data.password);
        if (!isValid)
        {
          return Ok(new { error = "Usuario o Contraseña Incorrectos" });
        }

        var user = UserPrincipal.FindByIdentity(context, data.username);
        string docIdentidad = user.EmployeeId.Trim();

        //DirectoryEntry entry = (DirectoryEntry) ADUser.GetUnderlyingObject();
        //docIdentidad = entry.Properties[ADField].Value.ToString();
        //return Ok(new { user.EmployeeId, docIdentidad});



        if (docIdentidad != String.Empty)
        {
          AuthContext _db = new AuthContext();
          AuthManager manager = new AuthManager();
          var jw_Usuario = _db.Users.FirstOrDefault(u => u.UserName == docIdentidad);
          if (jw_Usuario != null)
          {
            return Ok(new { username = jw_Usuario.UserName });
          }
          else
          {
            JulianaContext defaultDb = new JulianaContext();
            var empresas = defaultDb.EMPRESAS
                                    .Where(e => e.Estado == "A")
                                    .Select(e => e.BaseDatos);
            foreach (var BaseDatos in empresas)
            {
              EMPLEADOS empleado = null;
              JulianaContext db = new JulianaContext(BaseDatos.Trim());
              empleado = db.EMPLEADOS.FirstOrDefault(
                                      ep => ep.Cedula == docIdentidad
                                      && ep.Estado != "R");
              if (empleado != null)
              {
                ApplicationUser newUser = new ApplicationUser()
                {
                  UserName = docIdentidad,
                  DBName = BaseDatos.Trim(),
                  Empleado = empleado.Empleado,
                };
                var result = manager.CreateUser(newUser, docIdentidad);
                //newUser = _db.Users.First(u => u.UserName == docIdentidad);
                ApplicationGroup empleadoGroup = _db.Groups.FirstOrDefault(g => g.Name == "Empleado");
                manager.AddUserToGroup(newUser.Id, empleadoGroup.Id);
                return Ok(new { username = newUser.UserName });
              }
            }
            return Ok(new { error = String.Format("No se ha encontrado un Empleado activo con el documento de identidad {0}", docIdentidad) });

          }
        }
        else
        {
          return BadRequest("Su usuario del directorio activo no está configurado para ingresar al sistema comuniquese con el equipo de sistemas.");
        }

      }


      /*
      DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain);
      DirectorySearcher Dsearch = new DirectorySearcher(entry);
      String Name = "Test1";
      Dsearch.Filter = "(&(objectClass=user)(l=" + Name + "))";
      var hola = "";
      foreach (SearchResult sResultSet in Dsearch.FindAll())
      {
        hola = GetProperty(sResultSet, "givenName");
      }*/

      return Ok();


    }

    public static string GetProperty(SearchResult searchResult, string PropertyName)
    {
      if (searchResult.Properties.Contains(PropertyName))
      {
        return searchResult.Properties[PropertyName][0].ToString();
      }
      else
      {
        return string.Empty;
      }
    }

  }
}
