using JulianaWeb.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models
{
  [Table("RELACIONSOLVAC")]
  public partial class RELACIONSOLVAC
  {
    public static string[] ESTADOS_BORRABLES = new string[] { " ", "" };

    [Required]
    public int Cod_Solicitud { get; set; }

   
    [Required]
    public int Cod_Vacaciones { get; set; }


    public static bool GuardarRelacionVacaciones(string empresa, int codSolicitud, List<int> codsVacaciones) {
      var db = new JulianaContext(empresa);
      try
      {
        foreach(int codVacaciones in codsVacaciones)
        {
          db.RELACIONSOLVAC.Add(new RELACIONSOLVAC
          {
            Cod_Solicitud = codSolicitud,
            Cod_Vacaciones = codVacaciones
          });
        }
        db.SaveChanges();
        return true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        db.Dispose();
      }
    }


    public static int BorrarRelacionVacaciones(string empresa, int Cod_Solicitud)
    {
      var db = new JulianaContext(empresa);
      

      IList<VACACIONES> registrosVacaciones = new List<VACACIONES>();
      try
      {
        bool esBorrable = true;

        /// borra en la tabla de vacaciones
        var codsVacaciones = db.RELACIONSOLVAC.Where(x => x.Cod_Solicitud == Cod_Solicitud).Select(x => x.Cod_Vacaciones).ToList();


        if(codsVacaciones.Count == 0)
        {
          registrosVacaciones = ObtenerVacacionesSolicitud(empresa, Cod_Solicitud, db);
        }
        else
        {
          registrosVacaciones = db.VACACIONES.Where(x => codsVacaciones.Contains(x.AutoNum)).ToList();
        }

        esBorrable = registrosVacaciones.Aggregate<VACACIONES, bool>(esBorrable, (resultado, vacacion) =>
            resultado && UtilHelper.EsIgualA(vacacion.Estado, ESTADOS_BORRABLES)
        );

        if (!esBorrable) {
          db.Dispose();
          return -1;
        }

        foreach(var resgistro in registrosVacaciones)
        {
          db.VACACIONES.Remove(resgistro);
        }

        /// Si tambien quieren que se borre el registro en la tabla de RELACIONSOLVAC descomentar las siguiente linieas
        var relaciones = db.RELACIONSOLVAC.Where(x => x.Cod_Solicitud == Cod_Solicitud);
        foreach (var r in relaciones)
        {
          db.RELACIONSOLVAC.Remove(r);
        }

        db.SaveChanges();
        return 0;
      }
      catch (Exception ex)
      {
        AuditoriaHelper.Log(db, "RELACIONSOLVAC", "BorrarRelacionSolVac", $"Ocurrio un error al desasociar la solicitud '{Cod_Solicitud}' : " + ex.Message);
        return 1;
      }
      finally
      {
        db.Dispose();
      }
    }

    public static int BorrarRelacionLicencias(string empresa, int Cod_Solicitud)
    {
      var db = new JulianaContext(empresa);

      try
      {
        bool esBorrable = true;
        RELACIONSOLVAC relNovaut = null;
        NOVAUT novaut = null;

        relNovaut = db.RELACIONSOLVAC.FirstOrDefault(x => x.Cod_Solicitud == Cod_Solicitud);

        if (relNovaut != null) novaut = db.NOVAUT.FirstOrDefault(x => x.AutoNum == relNovaut.Cod_Vacaciones);

        if (novaut != null) esBorrable = UtilHelper.EsIgualA(novaut.Estado, ESTADOS_BORRABLES);

        if (!esBorrable)
        {
          db.Dispose();
          return -1;
        }

        if (novaut != null) db.NOVAUT.Remove(novaut);
        if (relNovaut != null) db.RELACIONSOLVAC.Remove(relNovaut);

        if (novaut != null || relNovaut != null) db.SaveChanges();
        return 0;
      }
      catch (Exception ex)
      {
        AuditoriaHelper.Log(db, "RELACIONSOLVAC", "BorrarRelacionSolVac", $"Ocurrio un error al desasociar la solicitud '{Cod_Solicitud}' : " + ex.Message);
        return 1;
      }
      finally
      {
        db.Dispose();
      }
    }

    public static bool TieneVacacionesBorrables(string empresa, int Cod_Solicitud, JulianaContext db = null) {

      IList<VACACIONES> registrosVacaciones = ObtenerVacacionesSolicitud(empresa, Cod_Solicitud, db);

      //SOLICITUDES solicitud = db.SOLICITUDES.Where(s => s.Cod_Solicitud == Cod_Solicitud).SingleOrDefault();
      bool esBorrable = registrosVacaciones.Aggregate<VACACIONES, bool>(true, (resultado, vacacion) =>
            resultado && UtilHelper.EsIgualA(vacacion.Estado, ESTADOS_BORRABLES)
        );

      return esBorrable;
    }

    public static bool TieneLicenciaBorrable(string Empresa, int Cod_Solicitud)
    {
      JulianaContext db = new JulianaContext(Empresa);
      NOVAUT novTMP = (from nov in db.NOVAUT
                join sol in db.RELACIONSOLVAC on nov.AutoNum equals sol.Cod_Vacaciones
                where sol.Cod_Solicitud == Cod_Solicitud
                select nov).FirstOrDefault();
      return novTMP == null || UtilHelper.EsIgualA(novTMP.Estado, ESTADOS_BORRABLES);
    }

    public static bool TieneVacacionesBorrables(IList<VACACIONES> vacaciones)
    {

      if (vacaciones != null) {
        return vacaciones.Aggregate<VACACIONES, bool>(true, (resultado, vacacion) =>
            resultado && UtilHelper.EsIgualA(vacacion.Estado, ESTADOS_BORRABLES)
        );
      }

      return true;
    }
    public static IList<VACACIONES> ObtenerVacacionesSolicitud(string empresa, int codSolicitud, JulianaContext db = null) {

      IList<VACACIONES> vacacionesSolicitud = new List<VACACIONES>();
      db                                    = db ?? new JulianaContext(empresa);
      SOLICITUDES solicitud                 = db.SOLICITUDES.Where(s => s.Cod_Solicitud == codSolicitud).SingleOrDefault();

      if (solicitud == null)
        return vacacionesSolicitud;

      var datosSolicitud = new
      {
        Desde       = UtilHelper.ConcatenarFecha(solicitud.Fec_Salida),
        Hasta       = UtilHelper.ConcatenarFecha(solicitud.Fec_Llegada),
        CodEmpleado = solicitud.Cod_Empleado
      };

      return db.VACACIONES.Where(v =>
          string.Compare(v.Desde, datosSolicitud.Desde) >= 0 &&
          string.Compare(v.Hasta, datosSolicitud.Hasta) <= 0 &&
          v.Cod_Empleado == datosSolicitud.CodEmpleado
      ).ToList();

    }
  }
}
