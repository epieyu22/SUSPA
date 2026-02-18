using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JulianaWeb.Repositories;
using JW3.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using JulianaWeb.Business;
using JulianaWeb.Services.Logging;
using NLog;
using JulianaWeb.Interfaces.Aspects;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Account")]
  public class AccountController : ApiController
  {
    private AuthRepository authRepository = null;
    private JulianaContext _db = null;
    private AuthContext authDb;

    private AuthBOService _authBOService;
    private readonly ILoggerService<AccountController> _logger;

    public AccountController()
    {
      authRepository = new AuthRepository();
      _db = new JulianaContext();
      authDb = new AuthContext();
      _authBOService = new AuthBOService(_db, new AuthManager(), authRepository, LoggerServiceFactory.Get<AuthBOService>());
      _logger = LoggerServiceFactory.Get<AccountController>();
    }

    [Route("")]
    [HttpGet]
    public IHttpActionResult Get()
    {
      var users = from u in authDb.Users where u.UserName != "Admin" orderby u.DBName, u.Empleado select u;
      var empresas = users.Select(u => u.DBName).Distinct();
      var roles = authDb.Groups.ToList();
      var uss = from u in users
                select new
                {
                  Id = u.Id,
                  Empleado = u.Empleado,
                  DBName = u.DBName,
                  UserName = u.UserName,
                  Role = u.Groups.FirstOrDefault().Group.Name
                };
      return Ok(new { users = uss.ToList(), empresas = empresas.ToList(), roles = roles });
    }

    [Route("seedOne")]
    [HttpGet]
    public IHttpActionResult GenerateOneEmpresa()
    {
      JulianaContext db = new JulianaContext();
      string DBName = db.EMPRESAS.FirstOrDefault().BaseDatos;
      IList<EMPLEADOS> empleados = db.EMPLEADOS.Where(e => e.Estado != "R").ToList();
      foreach (var e in empleados)
      {
        db.WEB_USERS.Add(new WEB_USERS
        {
          Cedula = e.Cedula.Trim(),
          Estado = e.Estado.Trim(),
          DB_Name = DBName.Trim()
        });
        db.SaveChanges();
      }
      return Ok("Todo ususario agregados");
    }


    [Route("seedAll")]
    [HttpGet]
    public IHttpActionResult GenerateAllEmpresa()
    {
      JulianaContext db = new JulianaContext();
      var empresas = db.EMPRESAS.ToList();
      foreach (var empresa in empresas)
      {
        JulianaContext dbReal = new JulianaContext(empresa.BaseDatos.Trim());
        IList<EMPLEADOS> empleados = dbReal.EMPLEADOS.Where(e => e.Estado != "R").ToList();
        foreach (var e in empleados)
        {
          db.WEB_USERS.Add(new WEB_USERS
          {
            Cedula = e.Cedula.Trim(),
            Estado = e.Estado.Trim(),
            DB_Name = empresa.BaseDatos.Trim()
          });
          db.SaveChanges();
        }
      }

      return Ok("Todo ususario agregados");
    }


    [Route("Seed/{Empresa}")]
    [HttpGet]
    public IHttpActionResult generate(string Empresa)
    {

      AuthManager manager = new AuthManager();
      JulianaContext db = new JulianaContext(Empresa);
      List<EMPLEADOS> empleados = db.EMPLEADOS.Where(e => e.Estado != "R").ToList();
      foreach (EMPLEADOS empleado in empleados)
      {
        ApplicationUser newUser = new ApplicationUser
        {
          UserName = empleado.Cedula.Trim(),
          Empleado = empleado.Empleado,
          DBName = Empresa
        };
        string newPass = "123456";

        var result = manager.CreateUser(newUser, newPass);
        var errors = GetErrorResult(result);
        if (errors != null)
        {
          return errors;
        }
        newUser = authDb.Users.First(u => u.UserName == empleado.Cedula);
        ApplicationGroup empleadoGroup = authDb.Groups.First(g => g.Name == "Empleado");
        manager.AddUserToGroup(newUser.Id, empleadoGroup.Id);
      }
      return Json(new { ok = "Usuarios Generados" });
    }

    //[AllowAnonymous]
    //[Route("{Cedula}/Generate")]
    //[HttpGet]
    //public IHttpActionResult requestPassword(string Cedula)
    //{
    //    AuthManager manager = new AuthManager();
    //    var activeUser = authDb.Users.FirstOrDefault(au => au.UserName == Cedula);
    //    var user = _db.WEB_USERS.SingleOrDefault(u => u.Cedula == Cedula);
    //    var dbNames = _db.EMPRESAS.Select(e => e.BaseDatos);
    //    if (activeUser != null)
    //    {
    //        EMPLEADOS empleado;
    //        using (JulianaContext db = new JulianaContext(activeUser.DBName))
    //        {
    //            empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cedula && e.Estado != "R");
    //        }
    //        string newpass = user.generaateNewPass();
    //        manager.setPassword(activeUser.Id, newpass);
    //        MailHelper.sendRestorePasswordMail(empleado, newpass);
    //        return Json(new { success = "Su empleado ya posse un usuario en Juliana Web, Hemos generado una nueva contraseña y enviamos a su correo las credenciales para ingresar al sistema"});
    //    }
    //    if (user != null)
    //    {
    //        EMPLEADOS empleado;
    //        using (JulianaContext db = new JulianaContext(user.DB_Name))
    //        {
    //            empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cedula && e.Estado != "R");
    //        }
    //        ApplicationUser newUser = new ApplicationUser
    //        {
    //            UserName = Cedula,
    //            Empleado = empleado.Empleado,
    //            DBName = user.DB_Name
    //        };
    //        string newPass = user.generaateNewPass();              

    //        var result = manager.CreateUser(newUser, newPass);
    //        var errors = GetErrorResult(result);
    //        if (errors != null)
    //        {
    //            return errors;
    //        }

    //        newUser = authDb.Users.First(u => u.UserName == Cedula);
    //        ApplicationGroup empleadoGroup = authDb.Groups.First(g => g.Name == "Empleado");
    //        manager.AddUserToGroup(newUser.Id, empleadoGroup.Id);
    //        MailHelper.sendWellcomeMail(empleado, newPass);
    //        return Json(new { success = "Hemos enviado a su correo "+empleado.Dir_Elec.Trim()+"  las credenciales para ingresar a Juliana Web" });
    //    }
    //    return NotFound();
    //}

    //[Route("{Cedula}/SetMail")]
    //[HttpPost]
    //public IHttpActionResult SetMail(string Cedula, [FromBody] string Dir_Elec)
    //{
    //    WEB_USERS webUser = _db.WEB_USERS.FirstOrDefault(wu => wu.Cedula == Cedula);
    //    JulianaContext db = new JulianaContext(webUser.DB_Name);
    //    EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cedula &&  e.Estado != "R");
    //    if(empleado != null)
    //    {
    //        db.EMPLEADOS.Attach(empleado);
    //        empleado.Dir_Elec = Dir_Elec;
    //        db.SaveChanges();
    //        AuthManager manager = new AuthManager();
    //        ApplicationUser user = new ApplicationUser()
    //        {
    //            UserName = Cedula,
    //            DBName = webUser.DB_Name,
    //            Empleado = empleado.Empleado
    //        };
    //        string password = UtilHelper.generaateNewPass();
    //        manager.CreateUser(user, password);
    //        ApplicationGroup empleadoGroup = authDb.Groups.First(g => g.Name == "Empleado");
    //        manager.AddUserToGroup(user.Id, empleadoGroup.Id);
    //        //MailHelper.sendWellcomeMail(empleado, password);
    //        return Ok();
    //    }
    //    else
    //    {
    //        return BadRequest("El empleado e encuantra retirado");
    //    }
    //}


    [Route("NoMailValidation")]
    [HttpPost]
    public IHttpActionResult NoMailValidation([FromBody] NoEmailValidationViewmodel data)
    {
      WEB_USERS webUser = _db.WEB_USERS.FirstOrDefault(wu => wu.Cedula == data.Cedula);
      if (webUser != null)
      {
        JulianaContext db = new JulianaContext(webUser.DB_Name);
        EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == data.Cedula && e.Estado != "R");
        if (empleado != null)
        {
          string fecNacimiento = UtilHelper.getUnglyDate(data.Fec_Nacimiento);
          if (empleado.Cod_Lugar_Expedicion == data.Cod_Ciudad
              && empleado.Fec_Nacimiento == fecNacimiento)
          {
            return Ok();
          }
          else
          {
            return BadRequest("Datos incorrectos, la información ingresada no coincide con las información en nuestra base de datos");
          }
        }
        else
        {
          return BadRequest("El empleado se encuentra retirado");
        }
      }
      else
      {
        return BadRequest("Empleado no encontrado en nuestra base de datos");
      }
    }

    [Route("{UserId}/Roles/{RoleId}")]
    [HttpPost]
    public IHttpActionResult AddUserToRole(string UserId, int RoleId)
    {
      AuthManager manager = new AuthManager();
      manager.AddUserToGroup(UserId, RoleId);
      return Ok();
    }

    [Route("{Cedula}/ChangePassword")]
    [HttpPost]
    public IHttpActionResult ChangePasswrod(string Cedula, [FromBody] ChangePasswordViewModel data)
    {
      AuthManager manager = new AuthManager();
      ApplicationUser user = authDb.Users.SingleOrDefault(u => u.UserName == Cedula);
      if (manager.CheckPassword(user, data.oldPassword))
      {
        manager.ChangePassword(user.Id, data.oldPassword, data.newPassword);
        return Ok();
      }
      else
      {
        return Ok(new { error = "Contraseña actual incorrecta" });
      }
    }

    [Route("{Cedula}/ChangeFirstPassword")]
    [HttpPost]
    public IHttpActionResult ChangeFirstPassword(string Cedula, [FromBody] ChangePasswordViewModel data)
    {
      AuthManager manager = new AuthManager();
      ApplicationUser user = authDb.Users.SingleOrDefault(u => u.UserName == Cedula);
      user.EmailConfirmed = true;
      var query = String.Format("UPDATE AspNetUsers set EmailConfirmed = 1 where Id = '{0}'", user.Id);
      authDb.Database.ExecuteSqlCommand(query);
      authDb.SaveChanges();
      manager.ResetPassword(user.Id, data.newPassword);
      return Ok();
    }


    [Route("Users/Roles/{RoleId}")]
    [HttpPost]
    public IHttpActionResult AddUsersToRole(int RoleId, [FromBody] List<ApplicationUser> data)
    {
      AuthManager manager = new AuthManager();
      foreach (var user in data)
      {
        manager.AddUserToGroup(user.Id, RoleId);
      }
      return Ok();
    }




    [Route("{Cedula}/Permissions")]
    [HttpGet]
    public IHttpActionResult GetGroupPermission(string Cedula)
    {
      AuthManager manager = new AuthManager();
      var permissions = manager.GetUserPermissions(Cedula);
      return Json(permissions);
    }




    // Crear los metodos para remover usuario del rol y eliminar roles
    [Route("user/{UserId}/roles/")]
    [HttpGet]
    public IHttpActionResult GetRoles(string UserId)
    {
      var roles = authRepository.GetRoles(UserId);
      if (roles != null)
      {
        return Json(new { roles = roles });
      }
      return NotFound();
    }

    [Route("user/{UserId}/roles/add/{RoleName}")]
    [HttpPost]
    public IHttpActionResult AddRoleToUser(string UserId, string RoleName)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }
      authRepository.AddUserToRole(UserId, RoleName);
      return Ok();
    }


    [Route("Roles")]
    [HttpGet]
    public async Task<IHttpActionResult> GetRoles()
    {
      return Json(authDb.Groups.OrderBy(r => r.Name).ToList());
    }

    [Route("Roles")]
    [HttpPost]
    public IHttpActionResult CreateRole([FromBody] string RoleName)
    {
      var roleExist = authDb.Groups.SingleOrDefault(r => r.Name == RoleName);
      if (roleExist != null)
      {
        return Json(new { error = "Ya existe un rol con ese Nombre" });
      }
      ApplicationGroup group = new ApplicationGroup
      {
        Name = RoleName
      };
      authDb.Groups.Add(group);
      authDb.SaveChanges();
      return Ok();
    }


    [Route("Roles/Assign")]
    [HttpPost]
    public IHttpActionResult AssingRoles([FromBody] AssignRolesViewModel data)
    {
      AuthManager manager = new AuthManager();
      foreach (ApplicationUser user in data.users)
      {
        manager.ClearUserGroups(user.Id);
        manager.AddUserToGroup(user.Id, data.groupId);
      }
      return Ok();
    }


    [Route("Roles/{RoleId}")]
    [HttpPut]
    public IHttpActionResult UpdateRole(int RoleId, [FromBody]string RoleName)
    {
      var roleExist = authDb.Groups.SingleOrDefault(r => r.Name == RoleName);
      if (roleExist != null)
      {
        return Json(new { error = "Ya existe un rol con ese Nombre" });
      }
      ApplicationGroup group = authDb.Groups.SingleOrDefault(g => g.Id == RoleId);
      authDb.Groups.Attach(group);
      group.Name = RoleName;
      authDb.SaveChanges();
      return Ok();
    }

    [Route("Roles/{RoleId}")]
    [HttpDelete]
    public IHttpActionResult DeleteRole(int RoleId)
    {
      AuthManager manager = new AuthManager();
      var usersInrole = authDb.ApplicationUserGroup.Where(apg => apg.GroupId == RoleId);
      var empleadoRole = authDb.Groups.SingleOrDefault(g => g.Name == "Empleado");
      foreach (var user in usersInrole)
      {
        manager.ClearUserGroups(user.UserId);
        manager.AddUserToGroup(user.UserId, empleadoRole.Id);
      }
      manager.DeleteGroup(RoleId);
      return Ok();
    }

    [Route("Roles/{RoleId}/Permissions/")]
    [HttpPost]
    public IHttpActionResult AssignPermissions(int RoleId, [FromBody] List<ApplicationRole> data)
    {
      var g = authDb.Groups.FirstOrDefault(gr => gr.Id == RoleId);
      g.Roles.Clear();
      authDb.SaveChanges();
      foreach (ApplicationRole role in data)
      {
        ApplicationRoleGroup newRoleGroup = new ApplicationRoleGroup
        {
          RoleId = role.Id,
          GroupId = g.Id,
        };
        if (!g.Roles.Contains(newRoleGroup))
        {
          g.Roles.Add(newRoleGroup);
        }
      }
      authDb.SaveChanges();
      return Ok();
    }

    [Route("Roles/{RoleId}/Permissions/Remove")]
    [HttpPost]
    public IHttpActionResult RemoveAssignPermissions(int RoleId, [FromBody] List<ApplicationRoleGroup> data)
    {
      var group = authDb.Groups.FirstOrDefault(g => g.Id == RoleId);
      authDb.Groups.Attach(group);
      foreach (ApplicationRoleGroup role in data)
      {
        var roleGruop = group.Roles.FirstOrDefault(r => r.RoleId == role.Role.Id);
        group.Roles.Remove(roleGruop);
      }
      authDb.Entry(group).State = EntityState.Modified;
      authDb.SaveChanges();
      return Ok();
    }


    [Route("Roles/{RoleId}/Permissions/unassigned")]
    [HttpGet]
    public IHttpActionResult GetUnassignedPermisisons(int RoleId)
    {
      ApplicationGroup group = authDb.Groups.SingleOrDefault(g => g.Id == RoleId);
      var groupRoles = group.Roles.Select(g => g.RoleId);
      List<ApplicationRole> unassigned = authDb.Roles
          .Where(r => !groupRoles.Contains(r.Id))
          .OrderBy(r => r.Name)
          .ToList();
      return Ok(unassigned);

    }

    [Route("Permission")]
    [HttpGet]
    public IHttpActionResult Get_Permission()
    {
      return Ok(authDb.Roles.OrderBy(r => r.Description).ToList());
    }

    [Route("Permission")]
    [HttpPost]
    public IHttpActionResult CreatePermission([FromBody] string name)
    {
      AuthManager manager = new AuthManager();
      manager.CreateRole(name);
      return Ok();
    }

    [Route("login")]
    [HttpPost]
    public async Task<IHttpActionResult> login(LoginViewModel data)
    {
      try
      {
        _logger.LogDebug("login Started ...");
        var loginResponse = _authBOService.Authenticate(data);

        if (loginResponse.user == null)
        {
          return Json(new { error = "Usuario o contraseña incorrectos" });
        }

        _logger.LogDebug("login succeed!");

        return Json(loginResponse);
      }
      catch (JulianaException ex)
      {
        _logger.LogError($"Error when login user... {data.username}", ex);

        return Json(new { errorCode = ex.Code, error = ex.Message });
      }
    }

    [Route("Adminlogin")]
    [HttpPost]
    public async Task<IHttpActionResult> Adminlogin(LoginViewModel data)
    {
      ApplicationUser user = authDb.Users.SingleOrDefault(u => u.UserName == data.username);
      if (user != null)
      {
        JulianaContext db = new JulianaContext(user.DBName.Trim());
        string Cedula = "";
        var queryalias = db.USUARIOS_WEB.FirstOrDefault(u => u.Clave == data.username);
        //USUARIOS_WEB queryalias = null;
        if (queryalias != null)
        {
          Cedula = queryalias.Usuario.Trim();
        }
        else
        {
          Cedula = user.UserName;
        }
        EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cedula && e.Estado != "R");
        TERCEROS tercero = db.TERCEROS.FirstOrDefault(e => e.Documento == Cedula);
        //TERCEROS tercero = null;
        if (empleado == null && user.UserName != "Admin" && tercero == null)
        {
          return Json(new { error = "Su usuario ha sido retirado, Solo empleados activos puden ingresar a la plataforma." });
        }
        AuthManager manager = new AuthManager();
        var permissions = manager.GetUserPermissions(data.username);
        user.DBName = user.DBName.Trim();

        if (empleado != null && user.UserName != "Admin" && tercero != null)
        {
          var msg = String.Format("Admin ingresó a Juliana Web como Cod_Empleado {0} -  Fecha: {1} - Hora: ", empleado.Cod_Empleado, DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("hh:mm"));
          AuditoriaHelper.Log(db, "ACCESO_SISTEMA", "N", msg);
        }
        if (empleado == null && tercero != null)
        {
          empleado = new EMPLEADOS
          {
            Empleado = tercero.Tercero,
            Cedula = tercero.Documento,
            Dir_Elec = tercero.Dir_Elec
          };
        }


        return Json(new { user, empleado, permissions });
      }
      else
      {
        return Json(new { error = "Usuario o Contraseña incorrecto", data = data, user = user });
      }
    }

    /*Este es*/
    [Route("RequestPassword")]
    [HttpPost]
    public IHttpActionResult RequestPassword([FromBody] string Username)
    {
      JulianaContext defaultDb = new JulianaContext();
      AuthManager manager = new AuthManager();
      ApplicationUser activeUser = authDb.Users.FirstOrDefault(u => u.UserName == Username);
      if (activeUser != null)
      {
        JulianaContext ddb = new JulianaContext(activeUser.DBName.Trim());
        var queryalias = ddb.USUARIOS_WEB.FirstOrDefault(u => u.Clave == Username);
        string ccc = "";
        if (queryalias != null)
        {
          ccc = queryalias.Usuario.Trim();
        }
        else
        {
          ccc = Username;
        }
        EMPLEADOS empleado = ddb.EMPLEADOS.FirstOrDefault(ep => ep.Cedula == ccc && ep.Estado != "R");
        TERCEROS tercero = ddb.TERCEROS.FirstOrDefault(t => t.Documento == ccc);
        if (empleado == null && tercero != null)
        {
          empleado = new EMPLEADOS
          {
            Cod_Empleado = tercero.Cod_Tercero,
            Empleado = tercero.Tercero,
            Cedula = tercero.Documento,
            Dir_Elec = tercero.Dir_Elec
          };
        }
        string newpass = CryptoHelper.generateNewPass();
        manager.setPassword(activeUser.Id, newpass);
        var query = String.Format("UPDATE AspNetUsers set EmailConfirmed = 0 where Id = '{0}'", activeUser.Id);
        authDb.Database.ExecuteSqlCommand(query);
        authDb.SaveChanges();
        MailHelper.sendWellcomeMail(empleado, newpass, Username);
        return Ok(new { success = "Su empleado ya posse un usuario en Juliana Web, Hemos generado una nueva contraseña y enviamos a su correo " + empleado.Dir_Elec.Trim() + "  las nuevas credenciales para ingresar al sistema" });
      }


      var Cedula = "";
      var empresas = defaultDb.EMPRESAS.Where(e => e.Estado == "A").Select(e => e.BaseDatos);
      var encontrado = false;
      foreach (var BaseDatos in empresas)
      {
        JulianaContext db = new JulianaContext(BaseDatos.Trim());
        var queryalias = db.USUARIOS_WEB.FirstOrDefault(u => u.Clave == Username);
        if (queryalias != null)
        {
          Cedula = queryalias.Usuario.Trim();
        }
        else
        {
          Cedula = Username;
        }

        EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(ep => ep.Cedula == Cedula && ep.Estado != "R");
        TERCEROS tercero = db.TERCEROS.FirstOrDefault(t => t.Documento == Cedula);
        if (empleado != null || tercero != null)
        {

          //if((empleado != null) )
          //{
          //  return Ok(new { error = "Su empleado no posee una dirección de correo electrónico en nustra base de datos. Use el enlace de abajo para ingresar uno." });
          //}

          if (empleado == null && tercero != null)
          {
            empleado = new EMPLEADOS
            {
              Cod_Empleado = tercero.Cod_Tercero,
              Empleado = tercero.Tercero,
              Cedula = tercero.Documento,
              Dir_Elec = tercero.Dir_Elec
            };
          }




          ApplicationUser newUser = new ApplicationUser
          {
            Email = empleado.Dir_Elec,
            UserName = Username,
            Empleado = empleado.Empleado,
            DBName = BaseDatos
          };

          string newPass = CryptoHelper.generateNewPass();
          var result = manager.CreateUser(newUser, newPass);
          var errors = GetErrorResult(result);
          if (errors != null)
          {
            return errors;
          }
          newUser = authDb.Users.First(u => u.UserName == Username);
          ApplicationGroup empleadoGroup = authDb.Groups.First(g => g.Name == "Empleado");
          manager.AddUserToGroup(newUser.Id, empleadoGroup.Id);
          MailHelper.sendWellcomeMail(empleado, newPass, Username);
          return Json(new { success = "Hemos enviado a su correo " + empleado.Dir_Elec.Trim() + "  las credenciales para ingresar a Juliana Web" });
        }
      }
      return Ok(new { error = "El documento de identificación no pertenece a ningún empleado activo dentro de la base de datos de nomina" });
    }

    [Route("Create/Aspirante")]
    [HttpPost]
    public IHttpActionResult NewAspirante([FromBody] AspiranteViewModel data)
    {
      JulianaContext db = new JulianaContext(data.DBName);
      var existUser = db.HOJAVIDA.FirstOrDefault(hv => hv.Doc_Identidad == data.Doc_Identidad);
      if (existUser != null)
      {
        return BadRequest("Ya existe un usuario con ese número de documento");
      }
      else
      {
        short id;
        if (db.HOJAVIDA.Count() > 0)
        {
          id = Convert.ToInt16(db.HOJAVIDA.Max(p => p.Cod_HojaVida) + 1);
        }
        else
        {
          id = 1;
        }

        HOJAVIDA h = new HOJAVIDA
        {
          Dir_Electronica = data.Dir_Electronica,
          Tipo_Documento = data.Tipo_Documento,
          Doc_Identidad = data.Doc_Identidad,
          PNombre = data.PNombre,
          SNombre = data.SNombre,
          PApellido = data.PApellido,
          SApellido = data.SApellido,
          Aspirante = data.PNombre + " " + data.SNombre + " " + data.PApellido + " " + data.SApellido,
          Celular = "",
          Cod_Cargo_Aspira = 0,
          Cod_Ciudad = 0,
          Cod_Lugar_Expedicion = 0,
          Cod_Nacionalidad = 1,
          Cod_Pais = 0,
          Cod_Personalidad = 0,
          Cod_Profesion = 0,
          Direccion = "",
          Distrito = "",
          Anos_Experiencia = 0,
          Estado = "P",
          Estudiante_Practica = "N",
          Est_Civil = "0",
          Fec_Actualizacion = "",
          Fec_Creacion = UtilHelper.getUnglyDate(DateTime.Now),
          Fec_Nacimiento = "",
          Idioma1 = "",
          Idioma2 = "",
          Idioma_Nativo = "",
          Nivel_Idioma1 = "",
          Nivel_Idioma2 = "",
          Lib_Militar = "",
          Porc_Conocimiento_Idioma1 = 0,
          Porc_Conocimiento_Idioma2 = 0,
          PTraslado = "",
          PViaje = "",
          Sexo = "",
          Sketch = "",
          Tel1 = "",
          Tel2 = "",
          Unidad_Tiempo = "",
          Cod_HojaVida = id
        };
        db.HOJAVIDA.Add(h);
        db.SaveChanges();
        // Empleado to send validation mail
        EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cod_Empleado == 5);
        MailHelper.sendConfirmarAspiranteMail(data);
        MailHelper.sendNewAspiranteAproMail(empleado, h);
        return Ok();
      }

    }


    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        authRepository.Dispose();
        _db.Dispose();
      }

      base.Dispose(disposing);
    }

    private IHttpActionResult GetErrorResult(IdentityResult result)
    {
      if (result == null)
      {
        return InternalServerError();
      }

      if (!result.Succeeded)
      {
        if (result.Errors != null)
        {
          foreach (string error in result.Errors)
          {
            ModelState.AddModelError("", error);
          }
        }

        if (ModelState.IsValid)
        {
          // No ModelState errors are available to send, so just return an empty BadRequest.
          return BadRequest();
        }

        return BadRequest(ModelState);
      }

      return null;
    }
  }
}
