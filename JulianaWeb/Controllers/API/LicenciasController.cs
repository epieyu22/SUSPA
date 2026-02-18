using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JulianaWeb.Models.Juliana.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Licencias")]
  public class LicenciasController : ApiController
  {
    [Route("{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult HistoricoEmpleado(string Empresa, short Cod_Empleado)
    {
      string FILTER_EMPLEADO = "1";
      string FILTER_CARGO = "2";
      string FILTER_DEPTO = "3";
      string FILTER_CCOSTO = "4";
      string FILTER_SUCURSAL = "5";
      string FILTER_ZONA = "6";
      string FILTER_GENERAL = "9";

      Dictionary<string, string> estadoDisplayName = new Dictionary<string, string>() {
      { "A",  "Aprobada" },
      { "A0",  "Aprobada Nivel 1" },
      { "A1",  "Aprobada Nivel 2" },
      { "A2",  "Aprobada Nivel 3" },
      { "A3",  "Aprobada Nivel 4" },
      { "A4",  "Aprobada Nivel 5" },
      { "A5",  "Aprobada Nivel 6" },
      { "A6",  "Aprobada Nivel 7" },
      { "A7",  "Aprobada Nivel 8" },
      { "A8",  "Aprobada Nivel 9" },
      { "A9",  "Aprobada Nivel 10" },
      { "AP", "Pagada" },
      { "P", "Pendiente Aprobación" },
      { "PR", "Pendiente Desaprobación" },
      { "R", "Rechazada" },
      { "D", "Eliminada" },
    };


      JulianaContext db = new JulianaContext(Empresa);
      var historico = from n in db.NOVAUT
                      join e in db.EMPLEADOS on n.Cod_Empleado equals e.Cod_Empleado
                      join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                      where
                          n.Cod_Empleado == Cod_Empleado
                          && c.Tipo_Concepto != "I"
                      orderby n.Desde descending
                      select new 
                      {
                        Concepto = c.Nom_Concepto,
                        Tipo_Concepto = c.Tipo_Concepto,
                        Dias = n.Dias,
                        Dias100 = n.Dias_Pag_100,
                        Desde = n.Desde,
                        Hasta = n.Hasta,
                        Usuario = n.Cod_Usuario,
                        Adjunto = n.Adjunto
                      };

      List<IncapacidadesViewModel> licencias = new List<IncapacidadesViewModel>();
      foreach (var i in historico)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta)
        };
        licencias.Add(d);
      }


      var solicitudes = (from s in db.SOLICITUDES
                         join c in db.CONCEPTOS on s.Cod_Concepto equals c.Cod_Concepto
                         join t in db.TERCEROS on s.Cod_Aprobador equals t.Cod_Tercero
                         where s.Cod_Empleado == Cod_Empleado
                               && s.Tipo_Solicitud == "L"
                               && s.Estado != "D"
                         select new SolicitudDTO
                         {
                           Cod_Solicitud = s.Cod_Solicitud,
                           Concepto = c.Nom_Concepto,
                           Aprobador = t.Tercero,
                           Cantidad = s.Cantidad,
                           Desde = (DateTime)s.Fec_Salida,
                           Hasta = (DateTime)s.Fec_Llegada,
                           Estado = s.Estado
                         }).ToList();

      bool contieneLuto = solicitudes.Any(s => s.Concepto.Contains("LUTO"));

      if (!contieneLuto)
      {
        var licenciasLutoRaw = (from n in db.NOVAUT
                             join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                             where n.Cod_Empleado == Cod_Empleado
                                   && c.Nom_Concepto.Contains("LUTO")
                             select new
                             {
                               Cod_Solicitud = (int)n.AutoNum,
                               Concepto = c.Nom_Concepto,
                               Aprobador = "Aprobador",
                               Cantidad = n.Dias,
                               Desde = n.Desde,
                               Hasta = n.Hasta,
                               Estado = "A"
                             }).ToList();

        var licenciasLuto = licenciasLutoRaw.Select(x => new SolicitudDTO
        {
          Cod_Solicitud = (int)x.Cod_Solicitud,
          Concepto = x.Concepto,
          Aprobador = x.Aprobador,
          Cantidad = x.Cantidad,
          Desde = DateTime.ParseExact(x.Desde, "yyyyMMdd", null),
          Hasta = DateTime.ParseExact(x.Hasta, "yyyyMMdd", null),
          Estado = x.Estado
        }).ToList();

        solicitudes.AddRange(licenciasLuto);
      }

      var empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);


      // Ausentimos especiales, BNP
      List<CONCEPTOS> conceptos;
      var dd = db.VARIABLES.SingleOrDefault(v => v.Uso == 104);
      Boolean UsaAusentimosEspeciales = false;
      var Cod_MiCumpleanos = 0;
      var Cod_MiTiempo = 0;
      var Cod_ServicioMilitar = 0;
      var Cod_DonaccionSangre = 0;
      var Cod_LicXMatrimonio = 0;
      var Cod_ExamenIngEduSup = 0;
      if (dd != null)
      {
        conceptos = db.CONCEPTOS
                      .Where(c => c.Tipo_Concepto == "H" || c.Cod_Concepto == 24 || c.Cod_Concepto == 25)
                      .OrderBy(c => c.Tipo_Concepto)
                      .ToList();
        UsaAusentimosEspeciales = true;

        var varMiTiempo = db.VARIABLES.SingleOrDefault(v => v.Uso == 105);
        if (varMiTiempo != null)
        {
          Cod_MiTiempo = varMiTiempo.Cod_Concepto;
        }

        var varMICumpleanos = db.VARIABLES.SingleOrDefault(v => v.Uso == 106);
        if (varMICumpleanos != null)
        {
          Cod_MiCumpleanos = varMICumpleanos.Cod_Concepto;
        }

        var varServicioMilitar = db.VARIABLES.SingleOrDefault(v => v.Uso == 107);
        if (varServicioMilitar != null)
        {
          Cod_ServicioMilitar = varServicioMilitar.Cod_Concepto;
        }

        var varDonacionSangre = db.VARIABLES.SingleOrDefault(v => v.Uso == 108);
        if (varDonacionSangre != null)
        {
          Cod_DonaccionSangre = varDonacionSangre.Cod_Concepto;
        }

        var varLicXMatrimonio = db.VARIABLES.SingleOrDefault(v => v.Uso == 109);
        if (varLicXMatrimonio != null)
        {
          Cod_LicXMatrimonio = varLicXMatrimonio.Cod_Concepto;
        }

        var varExamenIngEduSup = db.VARIABLES.SingleOrDefault(v => v.Uso == 110);
        if (varExamenIngEduSup != null)
        {
          Cod_ExamenIngEduSup = varExamenIngEduSup.Cod_Concepto;
        }
      }
      else
      {
        conceptos = db.CONCEPTOS.Where(c => c.Tipo_Concepto == "L" || c.Tipo_Concepto == "H").ToList();
      }
      //{ "Tipo": "INCAPACIDAD_GENERAL", "Validar_anexo": true, "Diagnostico": true, "Tercero_informar": 9 }

      List<PARAMETROS_GENERALES> param = db.PARAMETROS_GENERALES.Where(p => p.Cod_Parametro.StartsWith("CONCEPTOS_") && !string.IsNullOrEmpty(p.Valor) && p.Valor.Replace(" ", "").Contains("\"Invisible\":true")).ToList();

      conceptos = (from c in conceptos
                   join pg in param  on "CONCEPTOS_" + c.Cod_Concepto.ToString() equals pg.Cod_Parametro into CONCEP
                   from con in CONCEP.DefaultIfEmpty()
                   where con == null 
                   select c).ToList();

      List<DIAGNOSTICOS> diagnosticos = new List<DIAGNOSTICOS>();

      // Consultar varaible || ultimo dia mes
      int Dia_Cierre_Novedades = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
      var Dia_Cierre_Novedades_Var = db.VARIABLES.Where(v => v.Uso == 32).ToList();
      if (Dia_Cierre_Novedades_Var.Count() > 0)
      {
        Dia_Cierre_Novedades = Convert.ToInt32(Dia_Cierre_Novedades_Var.Last().Tamano);
      }
      DateTime Rango_Fec_Desde = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
      DateTime Rango_Fec_Hasta = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Math.Min(Dia_Cierre_Novedades, DateTime.Now.Day + 1));

      bool Periodo_Cerrado = db.VARIABLES_VACACIONES.FirstOrDefault(v => DateTime.Now.Day > v.Tamano) != null;

      var codsAprobadoresTerceros = from c in db.APROBADORES
                                    where
                                    (((c.Filtro == FILTER_EMPLEADO && c.Cod_Filtro == empleado.Cod_Empleado)
                                    || (c.Filtro == FILTER_CARGO && c.Cod_Filtro == empleado.Cod_Cargo)
                                    || (c.Filtro == FILTER_DEPTO && c.Cod_Filtro == empleado.Cod_Depto)
                                    || (c.Filtro == FILTER_CCOSTO && c.Cod_Filtro == empleado.Cod_Ccostos)
                                    || (c.Filtro == FILTER_SUCURSAL && c.Cod_Filtro == empleado.Cod_Sucursal)
                                    || (c.Filtro == FILTER_ZONA && c.Cod_Filtro == empleado.Cod_Zona))
                                    || (c.Filtro == FILTER_GENERAL))
                                    && c.Tipo_Aprobacion == "V"
                                    && c.Nivel == "0"
                                    && c.Estado != "R"
                                    //&& c.Cod_Filtro != Cod_Empleado 
                                    orderby c.Filtro
                                    select c.Cod_Empleado;

      var aprobadores = db.TERCEROS.Where(t => codsAprobadoresTerceros.Contains(t.Cod_Tercero));

      return Ok(new
      {
        licencias,
        solicitudes,
        conceptos,
        Dia_Cierre_Novedades,
        Rango_Fec_Desde,
        Rango_Fec_Hasta,
        Periodo_Cerrado,
        aprobadores,
        UsaAusentimosEspeciales,
        Cod_MiTiempo,
        Cod_MiCumpleanos,
        Cod_ServicioMilitar,
        Cod_DonaccionSangre,
        Cod_LicXMatrimonio,
        Cod_ExamenIngEduSup
      });
    }
    public class SolicitudDTO
    {
      public int Cod_Solicitud { get; set; }
      public string Concepto { get; set; }
      public string Aprobador { get; set; }
      public decimal Cantidad { get; set; }
      public DateTime Desde { get; set; }
      public DateTime Hasta { get; set; }
      public string Estado { get; set; }
    }
    public IHttpActionResult NuevaSolicitudLicencia()
    {
      return Ok();
    }
  }


}
