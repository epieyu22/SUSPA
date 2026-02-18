using JulianaWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JulianaWeb.Repositories
{
    public class AuthRepository : IDisposable
    {
        private AuthContext DbCtx;        

        private UserManager<ApplicationUser> userManager;

        public AuthRepository()
        {
            DbCtx = new AuthContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbCtx));
            var provider = new DpapiDataProtectionProvider("JulianaWeb");
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
        }
        
        

        public IList<string> GetRoles(string userId)
        {
            var rol = userManager.GetRoles(userId);
            return rol;
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var idResult = userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
        

        public IdentityResult CreateUser(ApplicationUser user, string password)
        {            
            var result =  userManager.Create(user, password);
            return result;
                
        }


        public IdentityResult RegisterUser(string username, string password, string DBName)
        {

           /* var role = DbCtx.ApplicationRoles.SingleOrDefault(r => r.Name == "Empleado");
            DbCtx.ApplicationRoles.Attach(role);*/
           /* var user = new ApplicationUser
            {
                UserName = username,
                DBName = DBName,
                UserRoles = new List<ApplicationRole>
                {
                    role
                }
            };*/
            var user = new ApplicationUser
            {
                UserName = username,
                DBName = DBName,
            };
            var result = userManager.Create(user, password);
            return result;
        }

        public ApplicationUser SetPassword(ApplicationUser user, string password)
        {
            user.PasswordHash = userManager.PasswordHasher.HashPassword(password);
            return user;
        }

        public ApplicationUser FindUser(string username, string password)
        {
           return userManager.Find(username, password);
            //return user;
        }

        public void UpdateUser(ApplicationUser user)
        {
          userManager.Update(user);
        }

        public ApplicationUser FindUser(string username)
        {
          return userManager.FindByName(username);
        }

        public ApplicationUser FindUserByEmail(string email)
        {
          return userManager.FindByEmail(email);
        }

    	public void Dispose()
        {
            DbCtx.Dispose();
            userManager.Dispose();

        }

        /*
        internal object GetNotAssignedPermissionsByRolname(string rolename)
        {
            var permissions = from p in DbCtx.ApplicationPermissions where p.Roles.Any(r => r.Name == rolename) select p;
            var noAssignedPermissions = from p in DbCtx.ApplicationPermissions where !permissions.Contains(p) select p;

            return noAssignedPermissions.ToList();
        }*/

        public string GenerateEmailConfirmationToken(string userId)
        {
            return userManager.GenerateEmailConfirmationToken(userId);
        }

        /*
        internal List<ApplicationPermission> GetPermissions()
        {
            return DbCtx.ApplicationPermissions.ToList();
        }*/
        
    }
}
