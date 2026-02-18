using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
  public class LoginViewModel
  {
    public string username { get; set; }
    public string password { get; set; }
    public bool rememberme { get; set; }
    public string InternalLoginCode { get; set; }
  }

  public class AssignRolesViewModel
  {
    public int groupId { get; set; }
    public List<ApplicationUser> users { get; set; }
  }

  public class ChangePasswordViewModel
  {
    public string oldPassword { get; set; }
    public string newPassword { get; set; }
  }

  public class NoEmailValidationViewmodel
  {
    public string Cedula { get; set; }
    public string Cod_Departamento { get; set; }
    public short Cod_Ciudad { get; set; }
    public DateTime Fec_Nacimiento { get; set; }
  }

  public class AspiranteViewModel
  {
    public string Tipo_Documento { get; set; }
    public string Doc_Identidad { get; set; }
    public string PNombre { get; set; }
    public string SNombre { get; set; }
    public string PApellido { get; set; }
    public string SApellido { get; set; }
    public string Dir_Electronica { get; set; }
    public string DBName { get; set; }
  }

  public class LoginUserResponse
  {
    public ApplicationUser user { get; set; }

    public EMPLEADOS empleado { get; set; }

    public List<string> permissions { get; set; } = new List<string>();

    public string error { get; set; }

    public string errorCode { get; set; }
  }

  public class EmployeeByDbName
  {
    public EMPLEADOS Employee { get; set; }

    public string BusinessDb { get; set; }
  }

}
