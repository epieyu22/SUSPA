using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JulianaWeb.Repositories;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

namespace JulianaWeb.Business.Auth
{
    public class ConsolidatedUserService
  {
    private readonly JulianaContext db;
    private readonly AuthManager authManager;
    private readonly AuthRepository authRepository;
    private readonly AuthContext authDb;

    public ConsolidatedUserService(JulianaContext db, AuthManager authManager, AuthRepository authRepository)
    {
      this.db = db;
      this.authManager = authManager;
      this.authRepository = authRepository;
      this.authDb = new AuthContext();
    }

    public ApplicationUser ProvideUserFromEmail(string email)
    {

      var existingUser = authRepository.FindUserByEmail(email);

      if (existingUser != null)
      {
        return existingUser;
      }

      var employeeByDbName = FindEmployeeInContexts(email);

      if (employeeByDbName?.Employee == null)
      {
        throw new JulianaException("EmployeeNotFound", "This email is not associated in our system");
      }

      return ResolveUnmatchedUser(email, employeeByDbName);
    }

    private ApplicationUser ResolveUnmatchedUser(string email, EmployeeByDbName employeeByDbName)
    {
      var user = authRepository.FindUser(employeeByDbName.Employee.Cedula);

      if(user != null)
      {
        user.Email = email;
        authRepository.UpdateUser(user);

        return user;
      }

      return CreateUser(email, employeeByDbName);
    }

    private ApplicationUser CreateUser(string email, EmployeeByDbName employeeByDbName)
    {
      authManager.CreateUser(new ApplicationUser
      {
        Email = email,
        UserName = employeeByDbName.Employee.Cedula,
        DBName = employeeByDbName.BusinessDb,
        Empleado = employeeByDbName.Employee.Empleado,
        //Attributes = JsonConvert.SerializeObject(employeeByDbName.Employee)
      }, CryptoHelper.generateNewPass());

      var newUser = authDb.Users.First(u => u.UserName == employeeByDbName.Employee.Cedula);
      ApplicationGroup empleadoGroup = authDb.Groups.First(g => g.Name == "Empleado");

      authManager.AddUserToGroup(newUser.Id, empleadoGroup.Id);

      return newUser;
    }

    private EmployeeByDbName FindEmployeeInContexts(string email)
    {
      var bdName = string.Empty;
      EMPLEADOS employee = null;
      foreach (var business in db.EMPRESAS.ToList())
      {
        try
        {
          using (var context = new JulianaContext(business.BaseDatos))
          {
            employee = context.EMPLEADOS.FirstOrDefault(e => e.Dir_Elec.ToLower() == email.ToLower() && e.Estado != "R");

            if (employee != null)
            {
              bdName = business.BaseDatos;
              break;
            }
          }
        }
        catch (Exception e)
        {
          Debug.WriteLine($"exception: {e.Message}");
          continue;
        }
      }

      return new EmployeeByDbName { Employee = employee, BusinessDb = bdName };
    }
  }
}
