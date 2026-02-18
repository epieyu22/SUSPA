using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace JulianaWeb.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }


        public ApplicationRole(string name, string description) : base(name)
        {
            this.Description = description;
        }
        public virtual string Description { get; set; }
    }

    public class ApplicationRoleGroup
    {
        public virtual string RoleId { get; set; }
        public virtual int GroupId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        [JsonIgnore]
        public virtual ApplicationGroup Group { get; set; }
    }

    public class ApplicationUser : IdentityUser
    { 
        public ApplicationUser()
            : base()
        {
            this.Groups = new HashSet<ApplicationUserGroup>();
        }

        
        public string Empleado { get; set; }

        [Required]
        public string DBName { get; set; }


    //[Column("attrs")]
    //public string Attributes { get; set; } = "{}";

    public virtual ICollection<ApplicationUserGroup> Groups { get; set; }

    }

    public class ApplicationUserGroup
    {
        [Required]
        public virtual string UserId { get; set; }
        [Required]
        public virtual int GroupId { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationGroup Group { get; set; }
    }

    public class ApplicationGroup
    {
        public ApplicationGroup()
        {
        }


        public ApplicationGroup(string name) : this()
        {
            Roles = new List<ApplicationRoleGroup>();
            Name = name;
        }


        [Key]
        [Required]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }


        public virtual ICollection<ApplicationRoleGroup> Roles { get; set; }
    }




}
