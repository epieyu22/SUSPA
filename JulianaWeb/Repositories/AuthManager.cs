using JulianaWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;

namespace JulianaWeb.Repositories
{
  public class AuthManager
  {
    private const string SECRET_JWT_BACKDOOR = "SECRET_JWT_BACKDOOR";

    private readonly AuthContext _db = new AuthContext();

    private readonly RoleManager<ApplicationRole> _roleManager = new RoleManager<ApplicationRole>(
        new RoleStore<ApplicationRole>(new AuthContext()));

    private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
        new UserStore<ApplicationUser>(new AuthContext()));

    public bool RoleExists(string name)
    {
      return _roleManager.RoleExists(name);
    }

    public IList<string> GetRoles(string userId)
    {
      return _userManager.GetRoles(userId);
    }

    public IdentityResult CreateRole(string name, string description = "")
    {
      return _roleManager.Create(new ApplicationRole(name, description));
    }

    internal IList<string> GetUserPermissions(string userId)
    {
      ApplicationUser user = _db.Users.First(u => u.UserName == userId);
      IList<string> perimissions = new List<string>();
      foreach (var group in user.Groups)
      {
        foreach (var role in group.Group.Roles)
        {
          perimissions.Add(role.Role.Name);
        }
      }
      if (user.UserName != "Admin")
      {
        using (JulianaContext db = new JulianaContext(user.DBName))
        {
          string Cedula = "";
          var queryalias = db.USUARIOS_WEB.FirstOrDefault(u => u.Clave == user.UserName);
          if (queryalias != null)
          {
            Cedula = queryalias.Usuario.Trim();
          }
          else
          {
            Cedula = user.UserName;
          }
          EMPLEADOS empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cedula && e.Estado != "R");
          if (empleado != null)
          {
            if (empleado.Tipo_Salario == "2")
            {
              perimissions.Remove("consultas.cesantias");
            }
            string[] contratos = { "1", "2" , "8"};
            if (!contratos.Contains(empleado.Tipo_Contrato))
            {
              perimissions.Remove("consultas.cesantias");
              perimissions.Remove("consultas.vacaciones");
            }
          }

          //JulianaContext defaultdb = new JulianaContext();
          //USUARIOS Usuario = defaultdb.USUARIOS.FirstOrDefault(u => u.Cedula == empleado.Cedula);
          //var count = db.USUARIOS.Count();
          //if(Usuario == null && count > 0)
          //{
          //  perimissions.Remove("retefuente.config");
          //  perimissions.Remove("certlaboral.config");
          //  perimissions.Remove("compropago.config");
          //  perimissions.Remove("retefuente.admin");
          //  perimissions.Remove("certlaboral.admin");
          //  perimissions.Remove("compropago.admin");
          //}
        }
      }
      return perimissions;
    }

    public bool CheckPassword(ApplicationUser user, string oldPasswrod)
    {
      return _userManager.CheckPassword(user, oldPasswrod);
    }

    public void ChangePassword(string userId, string oldPasswrod, string newPassword)
    {
      _userManager.ChangePassword(userId, oldPasswrod, newPassword);
    }

    public void ResetPassword(string userId, string newPassword)
    {
      ApplicationUser user = _userManager.FindById(userId);
      user.PasswordHash = _userManager.PasswordHasher.HashPassword(newPassword);
      var result = _userManager.Update(user);
    }

    public IdentityResult CreateUser(ApplicationUser user, string password)
    {
      return _userManager.Create(user, password);
    }

    public IdentityResult AddUserToRole(string userId, string roleName)
    {
      return _userManager.AddToRole(userId, roleName);
    }


    public void ClearUserRoles(string userId)
    {
      ApplicationUser user = _userManager.FindById(userId);
      var currentRoles = new List<IdentityUserRole>();
      user.Roles.Clear();
      /*currentRoles.AddRange(user.Roles);
      foreach (IdentityUserRole role in currentRoles)
      {
          _userManager.RemoveFromRole(userId, role.Role.Name);
      }*/
    }

    public void RemoveFromRole(string userId, string roleName)
    {
      _userManager.RemoveFromRole(userId, roleName);
    }

    public void DeleteRole(string roleId)
    {
      IQueryable<ApplicationUser> roleUsers = _db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
      ApplicationRole role = _db.Roles.Find(roleId);

      foreach (ApplicationUser user in roleUsers)
      {
        RemoveFromRole(user.Id, role.Name);
      }
      _db.Roles.Remove(role);
      _db.SaveChanges();
    }

    internal void setPassword(string userId, string password)
    {
      _userManager.RemovePassword(userId);
      _userManager.AddPassword(userId, password);
    }

    public void CreateGroup(string groupName)
    {
      if (GroupNameExists(groupName))
      {
        throw new GroupExistsException(
            "A group by that name already exists in the database. Please choose another name.");
      }

      var newGroup = new ApplicationGroup(groupName);
      _db.Groups.Add(newGroup);
      _db.SaveChanges();
    }

    public bool GroupNameExists(string groupName)
    {
      return _db.Groups.Any(gr => gr.Name == groupName);
    }


    public void ClearUserGroups(string userId)
    {
      //ClearUserRoles(userId);
      ApplicationUser user = _db.Users.Find(userId);
      user.Roles.Clear();
      user.Groups.Clear();
      _db.SaveChanges();
    }

    public void AddUserToGroup(string userId, int groupId)
    {
      ApplicationGroup group = _db.Groups.First(g => g.Id == groupId);
      ApplicationUser user = _db.Users.First(u => u.Id == userId);

      var userGroup = new ApplicationUserGroup
      {
        Group = group,
        GroupId = group.Id,
        User = user,
        UserId = user.Id
      };

      /*foreach (ApplicationRoleGroup role in group.Roles)
      {
          _userManager.AddToRole(userId, role.Role.Name);
      }*/
      user.Groups.Add(userGroup);
      _db.SaveChanges();
    }


    public void AddUserToGroup2(ApplicationUser user, ApplicationGroup group)
    {

      var userGroup = new ApplicationUserGroup
      {
        Group = group,
        GroupId = group.Id,
        User = user,
        UserId = user.Id
      };

      /*foreach (ApplicationRoleGroup role in group.Roles)
      {
          _userManager.AddToRole(userId, role.Role.Name);
      }*/
      user.Groups.Add(userGroup);
      _db.SaveChanges();
    }



    public void ClearGroupRoles(int groupId)
    {
      ApplicationGroup group = _db.Groups.Find(groupId);
      IQueryable<ApplicationUser> groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));

      foreach (ApplicationRoleGroup role in group.Roles)
      {
        string currentRoleId = role.RoleId;
        foreach (ApplicationUser user in groupUsers)
        {
          // Is the user a member of any other groups with this role?
          int groupsWithRole = user.Groups.Count(g => g.Group.Roles.Any(r => r.RoleId == currentRoleId));

          // This will be 1 if the current group is the only one:
          if (groupsWithRole == 1)
          {
            RemoveFromRole(user.Id, role.Role.Name);
          }
        }
      }
      group.Roles.Clear();
      _db.SaveChanges();
    }

    public void AddRoleToGroup(int groupId, string roleName)
    {
      ApplicationGroup group = _db.Groups.Find(groupId);
      ApplicationRole role = _db.Roles.First(r => r.Name == roleName);

      var newgroupRole = new ApplicationRoleGroup
      {
        GroupId = group.Id,
        Group = group,
        RoleId = role.Id,
        Role = role
      };

      // make sure the groupRole is not already present
      if (!group.Roles.Contains(newgroupRole))
      {
        group.Roles.Add(newgroupRole);
        _db.SaveChanges();
      }

      // Add all of the users in this group to the new role:
      IQueryable<ApplicationUser> groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
      foreach (ApplicationUser user in groupUsers)
      {
        if (!(_userManager.IsInRole(user.Id, roleName)))
        {
          AddUserToRole(user.Id, role.Name);
        }
      }
    }

    public void DeleteGroup(int groupId)
    {
      ApplicationGroup group = _db.Groups.Find(groupId);

      // Clear the roles from the group:
      ClearGroupRoles(groupId);
      _db.Groups.Remove(group);
      _db.SaveChanges();
    }

    public static string GetBackDoorPass()
    {
        return System.Configuration.ConfigurationManager.AppSettings[SECRET_JWT_BACKDOOR] ?? null;
    }

    public static List<string> GetBusinessConnections()
    {
      List<string> connections = new List<string>();

      foreach (ConnectionStringSettings connection in System.Configuration.ConfigurationManager.ConnectionStrings)
      {
        if (!connection.Name.ToLower().Contains("nm") || connection.Name.ToLower().Contains("Nomina")) 
          continue;

        connections.Add(connection.Name);
      }

      return connections;
    }

    public static bool HasBusinessConnection(string dbname)
    {
      return GetBusinessConnections().Contains(dbname);
    }
  }

  [Serializable]
  public class GroupExistsException : Exception
  {
    public GroupExistsException()
    {
    }

    public GroupExistsException(string message) : base(message)
    {
    }

    public GroupExistsException(string message, Exception inner) : base(message, inner)
    {
    }

    protected GroupExistsException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
  }
}
