namespace JulianaWeb.Models
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Data.Entity.Spatial;

  public partial class AspNetUsers
  {

    [Key]
    public string Id { get; set; }

    public string Email { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }


    public string SecurityStamp { get; set; }



    public string PhoneNumber { get; set; }


    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    [Required]

    public DateTime? LockoutEndDateUtc { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public string UserName { get; set; }

    public string Empleado { get; set; }


    public string DBName { get; set; }

    public string Discriminator { get; set; }

  }
}
