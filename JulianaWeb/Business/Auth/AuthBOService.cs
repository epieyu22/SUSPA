using JulianaWeb.Business.Auth;
using JulianaWeb.Helpers;
using JulianaWeb.Interfaces.Aspects;
using JulianaWeb.Models;
using JulianaWeb.Repositories;
using NLog;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace JulianaWeb.Business
{
  public class AuthBOService
  {
    private JulianaContext _db;
    private readonly AuthManager authManager;
    private AuthRepository _authRepository;
    private readonly ILoggerService<AuthBOService> logger;
    private AuthManager _authManager;
    private ConsolidatedUserService _consolidatedUserService;

    public AuthBOService(JulianaContext db, AuthManager authManager, AuthRepository authRepository, ILoggerService<AuthBOService> logger)
    {
      _db = db;
      _authManager = authManager;
      _authRepository = authRepository;
      this.logger = logger;
      _consolidatedUserService = new ConsolidatedUserService(db, authManager, authRepository);
    }

    public LoginUserResponse Authenticate(LoginViewModel data)
    {
      logger.LogDebug($"Trying to auth user {data.username}, internalLogin: {!string.IsNullOrWhiteSpace(data.InternalLoginCode)}");

      ApplicationUser user = GetUser(data);

      if (user == null)
      {
        logger.LogDebug($"Invalid user {data.username}");
        return new LoginUserResponse();
      }

      JulianaContext db = new JulianaContext(user.DBName.Trim());

      string cedula = GetCedula(db, data.username, user);
      var employeeByDbName = GetEmpleado(db, cedula);

      if (employeeByDbName.Employee != null)
      {
        UpdateDbNameIfChanged(user, employeeByDbName);
      }

      TERCEROS tercero = GetTercero(db, cedula);

      logger.LogDebug($"validating user access... {data.username}");
      ValidateUserAccess(db, employeeByDbName.Employee, user, tercero);

      var permissions = _authManager.GetUserPermissions(cedula);

      return new LoginUserResponse
      {
        user = user,
        empleado = employeeByDbName.Employee,
        permissions = permissions.ToList()
      };
    }

    private void UpdateDbNameIfChanged(ApplicationUser user, EmployeeByDbName employeeByDbName)
    {
      if (employeeByDbName.BusinessDb != user.DBName)
      {
        logger.LogDebug($"Update User DbName from ${user.DBName.Trim()} -> {employeeByDbName.BusinessDb.Trim()}");

        _db.ASPNETURSER.FirstOrDefault(u => u.Id == user.Id).DBName = employeeByDbName.BusinessDb;
        _db.SaveChanges();
      }
    }

    private ApplicationUser GetUser(LoginViewModel data)
    {
      Func<string, ApplicationUser> strategy;

      switch (ConfigurationManager.AppSettings["auth0:LoginStrategy"])
      {
        case "Email":

          logger.LogDebug($"using email strategy");
          strategy = _consolidatedUserService.ProvideUserFromEmail;
          break;

        default:

          logger.LogDebug($"using default (cedula) strategy");
          strategy = _authRepository.FindUser;
          break;
      }

      bool IsSSOLogin = !string.IsNullOrWhiteSpace(data.InternalLoginCode) && data.InternalLoginCode == ConfigurationManager.AppSettings["auth0:InternalLoginCode"];

      return IsSSOLogin
          ? strategy(data.username)
          : _authRepository.FindUser(data.username, data.password);
    }

    private string GetCedula(JulianaContext userDb, string username, ApplicationUser user)
    {
      var queryalias = userDb.USUARIOS_WEB.FirstOrDefault(u => u.Clave == username);
      return queryalias?.Usuario.Trim() ?? user.UserName;
    }

    private EmployeeByDbName GetEmpleado(JulianaContext userDb, string cedula)
    {
      var mainDbEmployee = userDb.EMPLEADOS.FirstOrDefault(e => e.Cedula == cedula);
      if (mainDbEmployee == null) return new EmployeeByDbName { Employee = null, BusinessDb = string.Empty };

      var businessList = _db.EMPRESAS.ToList();

      var businessDbName = businessList.FirstOrDefault(b => b.Codigo == mainDbEmployee.Cod_Empleador)?.BaseDatos ?? string.Empty;


      if (mainDbEmployee.Estado == "R")
      {
        logger.LogDebug($"--- Searching user in other dbs, because is 'R'");
        foreach (var business in businessList)
        {
          try
          {
            JulianaContext dbReal = new JulianaContext(business.BaseDatos.Trim());
            mainDbEmployee = dbReal.EMPLEADOS.FirstOrDefault(e => e.Cedula == cedula && e.Estado != "R");
          }
          catch (Exception e)
          {
            Debug.WriteLine($"exception: {e.Message}");
            continue;
          }

          if(mainDbEmployee != null)
          {
            businessDbName = business.BaseDatos;
            break;
          }
        }
      }

      return new EmployeeByDbName { Employee = mainDbEmployee, BusinessDb = businessDbName };
    }

    private TERCEROS GetTercero(JulianaContext userDb, string cedula)
    {
      return this._db.TERCEROS.FirstOrDefault(e => e.Documento == cedula);
    }

    private void ValidateUserAccess(JulianaContext userDb, EMPLEADOS empleado, ApplicationUser user, TERCEROS tercero)
    {
      if (empleado == null && user.UserName != "Admin" && tercero == null)
      {
        var msg = "Usuario Invalido en el sistema. Comuniquese con su administrador";

        AuditoriaHelper.Log(this._db, "ACCESO_SISTEMA", "N", msg);

        //TODO: Translate exception messages
        throw new JulianaException("UserNotFound", msg);
      }

    }
  }

}
