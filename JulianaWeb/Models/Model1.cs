namespace JulianaWeb.Models
{
  using System;
  using System.Data.Entity;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;

  public partial class Model1 : DbContext
  {
    public Model1()
        : base("name=Model1")
    {
    }

    public virtual DbSet<USUARIOS_WEB> USUARIOS_WEB { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<USUARIOS_WEB>()
          .Property(e => e.Usuario)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<USUARIOS_WEB>()
          .Property(e => e.Clave)
          .IsUnicode(false);
    }
  }
}
