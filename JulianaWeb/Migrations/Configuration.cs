namespace JulianaWeb.Migrations
{
  using Repositories;
  using Models;
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;
  using System.Data.Entity.Validation;

  internal sealed class Configuration : DbMigrationsConfiguration<JulianaWeb.Models.AuthContext>
  {
    private readonly AuthContext _db = new AuthContext();
    private readonly AuthManager _authManager = new AuthManager();

    private readonly string[] _initialGroupNames = { "Administrador", "Empleado" };
    private readonly string[] _AdministradorRoleNames = {
            "settings", "settings.permissions", "consultas", "consultas.vacaciones", "consultas.cesantias",
            "consultas.seguridadSocial", "consultas.hojavida", "certificados", "certificados.compropago",
            "certificados.certlab", "certificados.retefuente", "certificados.retefuente.all","certificados.otroscert", "consultas.incapacidades","consultas.licencias",
            "consultas.histosalario","timesheet.horas","descargar.politicas", "consultas.cumpleaños","consultas.turnos",
            "consultas.histosalario","timesheet.horas","descargar.politicas", "consultas.cumpleaños", "nom_electronica",
            "dashboard.RrHh","dashboard.payroll","dashboard.financiero"

        };
    private readonly string[] _EmpleadoRoleNames = { "consultas",  "consultas.seguridadSocial",
            "certificados", "certificados.compropago", "certificados.certlab", "certificados.retefuente","certificados.otroscert","descargar.politicas", "consultas.cumpleaños" , "consultas.turnos",
            "dashboard.RrHh","dashboard.payroll","dashboard.financiero"};

    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(AuthContext context)
    {
      //AddGroups();
      AddRoles();
      //AddUsers();
      //AddRolesToGroups();
      //AddUsersToGroups();
    }

    public void AddGroups()
    {
      foreach (var groupName in _initialGroupNames)
      {

        try
        {
          _authManager.CreateGroup(groupName);
        }
        catch (GroupExistsException)
        {
          // intentionally catched for seeding
        }
      }
    }

    private void AddRoles()
    {
      // Some example initial roles. These COULD BE much more granular:
      _authManager.CreateRole("settings", "Acceso al modulo de Configuración");
      _authManager.CreateRole("nom_electronica", "Acceso al modulo de nomina electronica");
      _authManager.CreateRole("settings.permissions", "Agregar o eliminar permisos a los roles de usuarios");
      _authManager.CreateRole("consultas", "Acceso al modulo de consultas");
      _authManager.CreateRole("consultas.vacaciones", "Ver y solicitar Vacaciones");
      _authManager.CreateRole("consultas.cumpleaños", "Ver cumpleaños de los empleados");
      _authManager.CreateRole("consultas.cesantias", "Ver y solicitar Cesantias");
      _authManager.CreateRole("consultas.seguridadSocial", "Ver fondos de seguridad social del empleado");
      _authManager.CreateRole("consultas.hojavida", "Ver y Modificar Hoja de vida");
      _authManager.CreateRole("timesheet.horas", "Acceso a el reporte de horas time sheet");
      _authManager.CreateRole("certificados", "Acceso al modulo de certificados");
      _authManager.CreateRole("certificados.compropago", "Generar Comprobante de pago");
      _authManager.CreateRole("certificados.certlab", "Generar Certificado laboral");
      _authManager.CreateRole("certificados.retefuente", "Generar Certificado de ingresos y Retenciones");
      _authManager.CreateRole("certificados.retefuente.all", "Generar Certificado de ingresos y Retencione de todos los empleados de todas las empresas");
      _authManager.CreateRole("aprobar", "Aprobar solicitudes");
      _authManager.CreateRole("descargar.politicas", "Descargar las politicas de la empresa");
      _authManager.CreateRole("consultas.turnos", "Acceso al modulo de turnos");
      _authManager.CreateRole("consultas.turnos", "Modulo de turnos");
      _authManager.CreateRole("dashboard.RrHh", "Acceso al Dashboar de RRHH");
      _authManager.CreateRole("dashboard.payroll", "Acceso al Dashboar de Nomina");
      _authManager.CreateRole("dashboard.financiero", "Acceso al Dashboar Financiero");
    }

    private void AddRolesToGroups()
    {
      // Add the Super-Admin Roles to the Super-Admin Group:
      IDbSet<ApplicationGroup> allGroups = _db.Groups;
      ApplicationGroup Administrador = allGroups.First(g => g.Name == "Administrador");
      foreach (string name in _AdministradorRoleNames)
      {
        _authManager.AddRoleToGroup(Administrador.Id, name);
      }

      ApplicationGroup Empleado = allGroups.First(g => g.Name == "Empleado");
      foreach (string name in _EmpleadoRoleNames)
      {
        _authManager.AddRoleToGroup(Empleado.Id, name);
      }
    }

    private void AddUsers()
    {
      ApplicationUser newUser = new ApplicationUser
      {
        UserName = "Admin",
        Empleado = "Administrador",
        DBName = "DefaultConnection"
      };
      var userCreationResult = _authManager.CreateUser(newUser, "JulWeb2020*");
      if (!userCreationResult.Succeeded)
      {
        // warn the user that it's seeding went wrong
        throw new DbEntityValidationException("Could not create InitialUser because: " + String.Join(", ", userCreationResult.Errors));
      }
    }

    private void AddUsersToGroups()
    {
      Console.WriteLine(String.Join(", ", _db.Users.Select(u => u.Email)));
      ApplicationUser user = _db.Users.First(u => u.UserName == "Admin");
      IDbSet<ApplicationGroup> allGroups = _db.Groups;
      foreach (ApplicationGroup group in allGroups)
      {
        _authManager.AddUserToGroup(user.Id, group.Id);
      }
    }
  }
}
