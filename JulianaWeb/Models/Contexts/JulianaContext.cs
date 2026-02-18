namespace JulianaWeb.Models
{
  using JulianaWeb.Models.Juliana.Generals;
  using System.Data.Entity;

  public partial class JulianaContext : DbContext
  {
    public JulianaContext(string context = "JulianaContext")
        : base("name=" + context)
    {
    }

    public virtual DbSet<USUARIOS_WEB> USUARIOS_WEB { get; set; }
    public virtual DbSet<PLANTILLAS_CERTIFICADOS> PLANTILLAS_CERTIFICADOS { get; set; }
    public virtual DbSet<REQUERIMIENTOS> REQUERIMIENTOS { get; set; }
    public virtual DbSet<NOVAUT> NOVAUT { get; set; }
    public virtual DbSet<NOVEDADES> NOVEDADES { get; set; }
    public virtual DbSet<VARIABLES> VARIABLES { get; set; }

    public virtual DbSet<VARIABLES_VACACIONES> VARIABLES_VACACIONES { get; set; }


    public virtual DbSet<AUDITORIA> AUDITORIA { get; set; }
    public virtual DbSet<RELACIONSOLVAC> RELACIONSOLVAC { get; set; }

    public virtual DbSet<OTRASNOV> OTRASNOV { get; set; }
    public virtual DbSet<IDIOMAS> IDIOMAS { get; set; }
    public virtual DbSet<AFP> AFP { get; set; }
    public virtual DbSet<APROBADORES> APROBADORES { get; set; }
    public virtual DbSet<APROBACIONES> APROBACIONES { get; set; }
    public virtual DbSet<ARP> ARP { get; set; }
    public virtual DbSet<BANCOS> BANCOS { get; set; }
    public virtual DbSet<CONCEPTOS> CONCEPTOS { get; set; }
    public virtual DbSet<CAJASCOMP> CAJASCOMP { get; set; }
    public virtual DbSet<CARGOS> CARGOS { get; set; }
    public virtual DbSet<CCOSTOS> CCOSTOS { get; set; }
    public virtual DbSet<EMPLEADOS> EMPLEADOS { get; set; }
    public virtual DbSet<EMPRESAS> EMPRESAS { get; set; }
    public virtual DbSet<EPS> EPS { get; set; }
    public virtual DbSet<HISTORICO> HISTORICO { get; set; }
    public virtual DbSet<HISTORICO_AUTOLIQUIDACIONES> HISTORICO_AUTOLIQUIDACIONES { get; set; }
    public virtual DbSet<PARAMETROS> PARAMETROS { get; set; }
    public virtual DbSet<RANGOSSOLPENSION> RANGOSSOLPENSION { get; set; }
    public virtual DbSet<WEB_USERS> WEB_USERS { get; set; }

    public virtual DbSet<COMPETENCIAS> COMPETENCIAS { get; set; }
    public virtual DbSet<DOCUMENTOS> DOCUMENTOS { get; set; }
    public virtual DbSet<HOJAVIDA> HOJAVIDA { get; set; }
    public virtual DbSet<INSTEDUCATIVAS> INSTEDUCATIVAS { get; set; }
    public virtual DbSet<CANDIDATOS> CANDIDATOS { get; set; }
    public virtual DbSet<TERCEROS> TERCEROS { get; set; }
    public virtual DbSet<COMPETENCIASHOJAVIDA> COMPETENCIASHOJAVIDA { get; set; }
    public virtual DbSet<APROBACION_PAGOS> APROBACION_PAGOS { get; set; }
    public virtual DbSet<EXPLABORAL> EXPLABORAL { get; set; }
    public virtual DbSet<FAMILIARES> FAMILIARES { get; set; }
    public virtual DbSet<FORMACADEMICA> FORMACADEMICA { get; set; }


    public virtual DbSet<DIAGNOSTICOS> DIAGNOSTICOS { get; set; }
    public virtual DbSet<CIUDADES> CIUDADES { get; set; }
    public virtual DbSet<DEPARTAMENTOS> DEPARTAMENTOS { get; set; }
    public virtual DbSet<DEPTOS> DEPTOS { get; set; }
    public virtual DbSet<SUCURSALES> SUCURSALES { get; set; }
    public virtual DbSet<ZONAS> ZONAS { get; set; }
    public virtual DbSet<CESANTIAS> CESANTIAS { get; set; }
    public virtual DbSet<PAISES> PAISES { get; set; }
    public virtual DbSet<SOLICITUD_VACACIONES> SOLICITUD_VACACIONES { get; set; }
    public virtual DbSet<VACACIONES> VACACIONES { get; set; }

    public virtual DbSet<SOLICITUDES> SOLICITUDES { get; set; }
    public virtual DbSet<PARAMETROS_CERTLAB> PARAMETROS_CERTLAB { get; set; }
    public virtual DbSet<PARAMETROS_WEB> PARAMETROS_WEB { get; set; }

    public virtual DbSet<SALARIOS> SALARIOS { get; set; }
    public virtual DbSet<ACCESODATOS> ACCESODATOS { get; set; }
    public virtual DbSet<USUARIOS> USUARIOS { get; set; }

    public virtual DbSet<MOTIVOS_RECHAZO> MOTIVOS_RECHAZO { get; set; }
    public virtual DbSet<REFERENCIAS_PERSONALES> REFERENCIAS_PERSONALES { get; set; }
    public virtual DbSet<IDIOMAS_HOJAVIDA> IDIOMAS_HOJAVIDA { get; set; }
    public virtual DbSet<PROFESIONES> PROFESIONES { get; set; }
    public virtual DbSet<ESPECIALIDADES> ESPECIALIDADES { get; set; }
    public virtual DbSet<STATUS_NOM_ELEC> STATUS_NOM_ELEC { get; set; }
    public virtual DbSet<AspNetUsers> ASPNETURSER { get; set; }


    /// <summary>
    /// Time Sheet
    /// </summary>
    public virtual DbSet<CLIENTES> CLIENTES { get; set; }
    public virtual DbSet<THS_AREA> THS_AREA { get; set; }
    public virtual DbSet<THS_AREA_CONCEPTOS> THS_AREA_CONCEPTOS { get; set; }
    public virtual DbSet<THS_HORAS> THS_HORAS { get; set; }

    public virtual DbSet<TURNOS> TURNOS { get; set; }
    public virtual DbSet<TIME_CONTROL> TIME_CONTROL { get; set; }


    public virtual DbSet<THS_PARAMETRO_GENERAL> PARAMETROS_GENERAL { get; set; }

    public virtual DbSet<MAPEO_NOM_ELEC> MAPEO_NOM_ELEC { get; set; }
    /// <summary>
    /// Bioseguridad
    /// </summary>
    public virtual DbSet<BIOSEGURIDAD_PREGUNTAS> BIOSEGURIDAD_PREGUNTAS { get; set; }

    public virtual DbSet<PARAMETROS_GENERALES> PARAMETROS_GENERALES { get; set; }

    public virtual DbSet<FERIADOS> FERIADOS { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {

      modelBuilder.Entity<RELACIONSOLVAC>()
        .HasKey(r => new { r.Cod_Solicitud, r.Cod_Vacaciones });

    }
  }
}
