using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JulianaWeb.Models.Juliana.Generals;
using JulianaWeb.Models.ViewModels;
using JW3.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/Incapacidades")]
  public class IncapacidadesController : ApiController
  {

    string FILTER_EMPLEADO = "1";
    string FILTER_CARGO = "2";
    string FILTER_DEPTO = "3";
    string FILTER_CCOSTO = "4";
    string FILTER_SUCURSAL = "5";
    string FILTER_ZONA = "6";
    string FILTER_GENERAL = "9";

    [Route("{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult Get_Historico_Empleado(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var query = from n in db.NOVAUT
                  join e in db.EMPLEADOS on n.Cod_Empleado equals e.Cod_Empleado
                  join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                  where
                      n.Cod_Empleado == Cod_Empleado
                  orderby n.Desde descending
                  select new
                  {
                    Concepto = c.Nom_Concepto,
                    Tipo_Concepto = c.Tipo_Concepto,
                    Dias = n.Dias,
                    Dias100 = n.Dias_Pag_100,
                    Desde = n.Desde,
                    Hasta = n.Hasta,
                    Estado = n.Estado,
                    Usuario = n.Cod_Usuario,
                    Adjunto = n.Adjunto,
                    Cod_Novaut = n.AutoNum
                  };

      var historicoIncapacidades = query.Where(q => q.Estado == "P" && q.Tipo_Concepto == "I");
      var historicoLicencias = query.Where(q => q.Estado == "P" && (q.Tipo_Concepto != "I"));
      var licencias = query.Where(q => q.Usuario == 999 && (q.Tipo_Concepto != "I"));
      var incapacidades = query.Where(q => q.Usuario == 999 && q.Tipo_Concepto == "I");
      List<IncapacidadesViewModel> dataIncapacidades = new List<IncapacidadesViewModel>();
      foreach (var i in historicoIncapacidades)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias100.Value,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta)
        };
        dataIncapacidades.Add(d);
      }

      List<IncapacidadesViewModel> dataLicencias = new List<IncapacidadesViewModel>();
      foreach (var i in historicoLicencias)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias100.Value,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta)
        };
        dataLicencias.Add(d);
      }

      List<IncapacidadesViewModel> incData = new List<IncapacidadesViewModel>();
      foreach (var i in incapacidades)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta),
          Adjunto = i.Adjunto,
          Estado = i.Estado,
          Cod_Novaut = Convert.ToInt32(i.Cod_Novaut)
        };
        incData.Add(d);
      }

      List<IncapacidadesViewModel> licData = new List<IncapacidadesViewModel>();
      foreach (var i in licencias)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta),
          Adjunto = i.Adjunto
        };
        licData.Add(d);
      }

      return Ok(new { historicoIncapacidades = dataIncapacidades, historicoLicencias = dataLicencias, incapacidades = incData, licencias = licData });
    }

    [Route("Licencias/{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult Historico_Licencias(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);

      var query = from n in db.NOVAUT
                  join e in db.EMPLEADOS on n.Cod_Empleado equals e.Cod_Empleado
                  join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                  where
                      n.Cod_Empleado == Cod_Empleado
                  orderby n.Desde descending
                  select new
                  {
                    Concepto = c.Nom_Concepto,
                    Tipo_Concepto = c.Tipo_Concepto,
                    Dias = n.Dias,
                    Dias100 = n.Dias_Pag_100,
                    Desde = n.Desde,
                    Hasta = n.Hasta,
                    Estado = n.Estado,
                    Usuario = n.Cod_Usuario,
                    Adjunto = n.Adjunto
                  };

      var historicoIncapacidades = query.Where(q => q.Estado == "P" && q.Tipo_Concepto == "I");
      var historicoLicencias = query.Where(q => q.Estado == "P" && (q.Tipo_Concepto != "I"));
      var licencias = query.Where(q => (q.Tipo_Concepto != "I"));
      var incapacidades = query.Where(q => q.Tipo_Concepto == "I");
      List<IncapacidadesViewModel> dataIncapacidades = new List<IncapacidadesViewModel>();
      foreach (var i in historicoIncapacidades)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias100.Value,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta)
        };
        dataIncapacidades.Add(d);
      }

      List<IncapacidadesViewModel> dataLicencias = new List<IncapacidadesViewModel>();
      foreach (var i in historicoLicencias)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias100.Value,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta)
        };
        dataLicencias.Add(d);
      }

      List<IncapacidadesViewModel> incData = new List<IncapacidadesViewModel>();
      foreach (var i in incapacidades)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta),
          Adjunto = i.Adjunto
        };
        incData.Add(d);
      }

      List<IncapacidadesViewModel> licData = new List<IncapacidadesViewModel>();
      foreach (var i in licencias)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta),
          Adjunto = i.Adjunto
        };
        licData.Add(d);
      }

      return Ok(new { historicoIncapacidades = dataIncapacidades, historicoLicencias = dataLicencias, incapacidades = incData, licencias = licData });
    }

    [Route("Licencias/{Empresa}/{Cod_Empleado}")]
    [HttpGet]
    public IHttpActionResult Get_Historico_Empleado2(string Empresa, short Cod_Empleado)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var query = from n in db.NOVAUT
                  join e in db.EMPLEADOS on n.Cod_Empleado equals e.Cod_Empleado
                  join c in db.CONCEPTOS on n.Cod_Concepto equals c.Cod_Concepto
                  where
                      n.Cod_Empleado == Cod_Empleado &&
                      c.Tipo_Concepto == "L"
                  orderby n.Desde descending
                  select new
                  {
                    Concepto = c.Nom_Concepto,
                    Dias = n.Dias,
                    Dias100 = n.Dias_Pag_100,
                    Desde = n.Desde,
                    Hasta = n.Hasta,
                    Estado = n.Estado,
                    Usuario = n.Cod_Usuario,
                    Adjunto = n.Adjunto
                  };

      var historico = query.Where(q => q.Estado == "P");
      var incapacidades = query.Where(q => q.Usuario == 999);
      List<IncapacidadesViewModel> data = new List<IncapacidadesViewModel>();
      foreach (var i in historico)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias100.Value,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta)
        };
        data.Add(d);
      }

      List<IncapacidadesViewModel> incData = new List<IncapacidadesViewModel>();
      foreach (var i in incapacidades)
      {
        IncapacidadesViewModel d = new IncapacidadesViewModel
        {
          Concepto = i.Concepto.Trim(),
          Dias = i.Dias,
          Desde = UtilHelper.getDate(i.Desde),
          Hasta = UtilHelper.getDate(i.Hasta),
          Adjunto = i.Adjunto
        };
        incData.Add(d);
      }

      return Ok(new { historico = data, incapacidades = incData });
    }

    [Route("{Empresa}/{Cod_Empleado}")]
    [HttpPost]
    public IHttpActionResult New_Incapacidad(string Empresa, short Cod_Empleado, [FromBody] NewIncapacidadesViewModel data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      DateTime Hasta = data.Desde.AddDays(data.Dias);

      string Fec_Hasta = UtilHelper.getUnglyDate(Hasta);
      string Fec_Desde = UtilHelper.getUnglyDate(data.Desde);
            
      var Incapacidades_Fecha = from i in db.NOVAUT
                                where i.Cod_Empleado == Cod_Empleado
                                && i.Desde.CompareTo(Fec_Desde) >= 0
                                && i.Hasta.CompareTo(Fec_Hasta) <= 0
                                select i;

      if (Incapacidades_Fecha.Count() > 0)
      {
        if (Incapacidades_Fecha.Count() > 0)
        {
          return BadRequest("No se pudo completar la acción, ya se ha reportado una Novedad de Ausentismo en esa fecha.");
        }
      }

      var Vacaciones_Fecha = from i in db.VACACIONES
                             where i.Cod_Empleado == Cod_Empleado
                             && i.Desde.CompareTo(Fec_Desde) >= 0
                             && i.Hasta.CompareTo(Fec_Hasta) <= 0
                             select i;

      if (Vacaciones_Fecha.Count() > 0)
      {
        return BadRequest("No se pudo completar la acción, existe un periodo de vacaciones en esa fecha.");
      }


      var variables = db.VARIABLES.ToList();
      var solicitudes = db.SOLICITUDES.Where(s => s.Tipo_Solicitud == "L" || s.Tipo_Solicitud == "H");
      var empleado = db.EMPLEADOS.SingleOrDefault(e => e.Cod_Empleado == Cod_Empleado);
      if (variables.Count() > 0)
      {
        var ausentismosEspeciales = variables.SingleOrDefault(v => v.Uso == 104);
        if (ausentismosEspeciales != null)
        {
          var Cod_MiTiempo = 0;
          var varMiTiempo = db.VARIABLES.SingleOrDefault(v => v.Uso == 105);
          if (varMiTiempo != null)
          {
            Cod_MiTiempo = varMiTiempo.Cod_Concepto;
          }
          if (data.Cod_Concepto == Cod_MiTiempo)
          {
            Hasta = data.Desde.AddDays(1);
          }
          var sMiTiempo = solicitudes.FirstOrDefault(s => s.Cod_Concepto == Cod_MiTiempo);
          if (data.Cod_Concepto == Cod_MiTiempo && sMiTiempo != null && sMiTiempo.Fec_Salida.Value.Month == DateTime.Now.Month)
          {
            return BadRequest("No se pudo completar la acción, Ya ha realizado un solicitud de este tipo para este mes.");
          }

          var Cod_MiCumpleanos = 0;
          var varMICumpleanos = db.VARIABLES.SingleOrDefault(v => v.Uso == 106);
          if (varMICumpleanos != null)
          {
            Cod_MiCumpleanos = varMICumpleanos.Cod_Concepto;
          }
          if (data.Cod_Concepto == Cod_MiCumpleanos)
          {
            Hasta = data.Desde.AddDays(1);
          }
          var sMiCumpleanos = solicitudes.FirstOrDefault(s => s.Cod_Concepto == Cod_MiCumpleanos);
          var fecNacimiento = UtilHelper.getDate(empleado.Fec_Nacimiento);
          var cumpleanos = new DateTime(DateTime.Now.Year, fecNacimiento.Month, fecNacimiento.Day);
          if (data.Cod_Concepto == Cod_MiCumpleanos && (data.Desde < cumpleanos || data.Desde > cumpleanos.AddDays(30)))
          {
            return BadRequest("Esta solicitud solo puede hacerce dentro de los 30 días siguientes a la fecha de cumpleaños .");
          }
          if (data.Cod_Concepto == Cod_MiCumpleanos && sMiCumpleanos != null && sMiCumpleanos.Fec_Salida.Value.Year == DateTime.Now.Year)
          {
            return BadRequest("No se pudo completar la acción, Ya ha realizado un solicitud de ausencia por su cumplaños");
          }

          var Cod_ServicioMilitar = 0;
          var varServicioMilitar = db.VARIABLES.SingleOrDefault(v => v.Uso == 107);
          if (varServicioMilitar != null)
          {
            Cod_ServicioMilitar = varServicioMilitar.Cod_Concepto;
          }

          var sServicioMilitar = solicitudes.FirstOrDefault(s => s.Cod_Concepto == Cod_ServicioMilitar);
          if (data.Cod_Concepto == Cod_ServicioMilitar && sServicioMilitar != null && sServicioMilitar.Fec_Salida.Value.Year == DateTime.Now.Year)
          {
            return BadRequest("No se pudo completar la acción, Esta solicitud solo puede ser realizada una vez conforme documento");
          }

          var Cod_DonaccionSangre = 0;
          var varDonacionSangre = db.VARIABLES.SingleOrDefault(v => v.Uso == 108);
          if (varDonacionSangre != null)
          {
            Cod_DonaccionSangre = varDonacionSangre.Cod_Concepto;
          }

          var sDonacionSangre = solicitudes.FirstOrDefault(s => s.Cod_Concepto == Cod_DonaccionSangre);
          if (data.Cod_Concepto == Cod_ServicioMilitar && sDonacionSangre != null && sDonacionSangre.Fec_Salida.Value.Year == DateTime.Now.Year)
          {
            var diferenciaAnos = UtilHelper.GetYearsAhead(sDonacionSangre.Fec_Salida.Value);
            if (diferenciaAnos < 1)
            {
              return BadRequest("No se pudo completar la acción, Esta solicitud solo puede ser realizada una vez cada doce meses");
            }
          }
        }
      }

      var aprobador = db.TERCEROS.SingleOrDefault(e => e.Cod_Tercero == data.Cod_Aprobador);

      var concepto = db.CONCEPTOS.Where(c => c.Cod_Concepto == data.Cod_Concepto).ToList().First();
      Dictionary<string, object> dctPrms = null;
      SOLICITUDES solicitud = new SOLICITUDES();
      PARAMETROS_GENERALES prm = db.PARAMETROS_GENERALES.Where(x => x.Cod_Parametro == typeof(CONCEPTOS).Name + "_" + concepto.Cod_Concepto).FirstOrDefault();
      if (prm != null && !string.IsNullOrEmpty(prm.Valor)) dctPrms = JsonConvert.DeserializeObject<Dictionary<string, object>>(prm.Valor);
      if (new string[] {"L", "I" }.Contains(concepto.Tipo_Concepto))
      {
        if (dctPrms != null)
        {
          //data.Fec_Llegada = VacacionesHelper.Calcular_Fec_LLegada(Empleado, data.Cantidad, data.Fec_Salida.Value);

          DateTime desdeFec = UtilHelper.getDate(Fec_Desde).AddHours(-5);
          DateTime hastaFec = desdeFec.AddDays(data.Dias).AddMilliseconds(-3);
          if (dctPrms.ContainsKey("Tipo"))
          {
            //LICENCIA DE LUTO
            if (dctPrms["Tipo"].Equals("LICENCIA_LUTO"))
            {
              var solicitudes_Fecha = db.SOLICITUDES
                          .Where(
                              s => s.Cod_Empleado == Cod_Empleado
                              && (s.Estado != "AP" && s.Estado != "D" && s.Estado != "R")
                              && (
                                     (s.Fec_Salida <= data.Desde
                                   && s.Fec_Llegada >= hastaFec)
                                  //&& (s.Fec_Salida <= data.Desde
                                  //    && s.Fec_Llegada <= hastaFec)
                                 )
                            );
              if (solicitudes_Fecha.Count() > 0)
              {
                return BadRequest("No se pudo completar la acción, existe una solicitud en esa fecha.");
              }
              if (dctPrms.ContainsKey("Permitir_fecha_futura") && !Convert.ToBoolean(dctPrms["Permitir_fecha_futura"]) && DateTime.ParseExact(Fec_Desde, "yyyyMMdd", CultureInfo.InvariantCulture) > DateTime.Today)
              {
                return BadRequest($"Su solicitud de luto no puede comenzar en una fecha futura.");
              }
            }
            //&&
            //  db.SOLICITUDES.Where(x => x.Fec_Salida.HasValue && x.Fec_Llegada.HasValue &&
            //    x.Cod_Concepto == concepto.Cod_Concepto &&
            //    x.Cod_Empleado == empleado.Cod_Empleado &&
            //    x.Estado != "R" &&
            //    x.Fec_Salida.Value.Day >= desdeFec.Day && x.Fec_Salida.Value.Month == desdeFec.Month && x.Fec_Salida.Value.Year == desdeFec.Year &&
            //    x.Fec_Salida.Value.AddDays(data.Dias).Day <= hastaFec.Day && x.Fec_Salida.Value.AddDays(Int16.Parse(data.Dias)).Month == hastaFec.Month && x.Fec_Salida.Value.AddDays(Int16.Parse(data.Dias)).Year == hastaFec.Year
            //    ).Count() != 0)

            //{
            //    return BadRequest("No se pudo completar la acción, existe un periodo de Solicitudes en esa fecha.");
            //  }


            //LICENCIA REMUNERADA
            if (new string[] { "LICENCIA_REMUNERADA", "LICENCIA_LUTO" }.Contains(dctPrms["Tipo"]) && dctPrms.ContainsKey("Ignorar_Finde") && Convert.ToBoolean(dctPrms["Ignorar_Finde"]))
            {
              Fec_Hasta = UtilHelper.getUnglyDate(VacacionesHelper.Calcular_Fec_LLegada(empleado, data.Dias, desdeFec));
              Hasta = UtilHelper.getDate(Fec_Hasta);
            }


            if (dctPrms["Tipo"].Equals("CUMPLEANIOS"))
            {
              DateTime birth = UtilHelper.getDate(empleado.Fec_Nacimiento).AddHours(-5);
              DateTime cumpleFec = new DateTime(desdeFec.Year, birth.Month, birth.Day);
              if (cumpleFec > desdeFec) cumpleFec = cumpleFec.AddYears(-1);
              TimeSpan difFechas = desdeFec.Subtract(cumpleFec);
              int CanTotalDias = (int)difFechas.TotalDays;
              int diasEspeciales = VacacionesHelper.Dias_Especiales_EntreFechas(empleado, desdeFec, cumpleFec);
              if ((dctPrms.ContainsKey("Dias_habiles") && (Math.Abs(CanTotalDias) - diasEspeciales) > int.Parse(dctPrms["Dias_habiles"].ToString())) ||
              VacacionesHelper.FecEsFestivo(hastaFec.Date) || (empleado.Sabado != "1" && hastaFec.DayOfWeek == DayOfWeek.Saturday) ||
              hastaFec.DayOfWeek == DayOfWeek.Sunday)
              {
                return BadRequest($"Su solicitud no está dentro de los{(dctPrms.ContainsKey("Dias_habiles") ? $" {dctPrms["Dias_habiles"]} " : " ")}días hábiles.");
              }
            }

            if (dctPrms.ContainsKey("Maximo_dias") && (hastaFec.Date - desdeFec.Date).TotalDays >= int.Parse(dctPrms["Maximo_dias"].ToString()))
            {
              return BadRequest($"Solo puede tomarse {dctPrms["Maximo_dias"]} dia(s) para {concepto.Nom_Concepto.ToLower()}");
            }

            if (dctPrms.ContainsKey("Maximo_por_anio") && db.SOLICITUDES.Where(x => x.Fec_Salida.HasValue && x.Fec_Salida.Value.Year == desdeFec.Year && x.Cod_Concepto == concepto.Cod_Concepto && x.Cod_Empleado == empleado.Cod_Empleado && x.Estado != "R" && x.Estado != "D").Count() >= int.Parse(dctPrms["Maximo_por_anio"].ToString()))
            {
              return BadRequest($"Solo se puede hacer {dctPrms["Maximo_por_anio"]} solicitud(es) al año para {concepto.Nom_Concepto.ToLower()}");
            }
            if (dctPrms.ContainsKey("Maximo_por_semestre") &&
                db.SOLICITUDES.Where(x => x.Fec_Salida.HasValue &&
                  x.Fec_Salida.Value.Year == desdeFec.Year &&
                  x.Cod_Concepto == concepto.Cod_Concepto &&
                  x.Cod_Empleado == empleado.Cod_Empleado &&
                  x.Estado != "R" &&
                  x.Estado != "D" &&
                  ((desdeFec.Month >= 1 && desdeFec.Month <= 6 && x.Fec_Salida.Value.Month >= 1 && x.Fec_Salida.Value.Month <= 6) ||
                  (desdeFec.Month >= 7 && desdeFec.Month <= 12 && x.Fec_Salida.Value.Month >= 7 && x.Fec_Salida.Value.Month <= 12))
                  ).Count() >= int.Parse(dctPrms["Maximo_por_semestre"].ToString()))
            {
              return BadRequest($"Solo se puede hacer {dctPrms["Maximo_por_semestre"]} solicitud(es) por semestre para {concepto.Nom_Concepto.ToLower()}");
            }
          }
        }

        if (concepto.Tipo_Concepto == "L" && concepto.Nom_Concepto != "LICENCIA DE LUTO")
        {
          solicitud = new SOLICITUDES
          {
            Cod_Empleado = Cod_Empleado,
            Cod_Aprobador = data.Cod_Aprobador,
            Fec_Salida = UtilHelper.getDate(Fec_Desde),
            Fec_Llegada = UtilHelper.getDate(Fec_Hasta),
            Fec_Solicitud = DateTime.Now,
            Cantidad = data.Dias,
            Cod_Concepto = data.Cod_Concepto,
            Tipo_Solicitud = concepto.Tipo_Concepto,
            Descripcion = data.Descripcion,
            Estado = "P"
          };
          db.SOLICITUDES.Add(solicitud);
          db.SaveChanges();
          MailHelper.EnviarSolicitudLicencia(empleado, aprobador, solicitud, concepto.Nom_Concepto);
        }
      }

      string Estado = "";
      if (concepto.Tipo_Concepto == "I")
      {
        var FecNomina = db.PARAMETROS.ToList().LastOrDefault()?.Fec_Nomina;
        if (!string.IsNullOrEmpty(FecNomina))
        {
          var FecNominaDate = UtilHelper.getDate(FecNomina);
          var FecNominaDateReset = new DateTime(FecNominaDate.Year, FecNominaDate.Month, 1);
          if (DateTime.Compare(FecNominaDateReset, Hasta) > 0)
            Estado = "P";
        }
      }

      NOVAUT New_Novaut = new NOVAUT
      {
        Cod_Concepto = data.Cod_Concepto,
        Cod_Empleado = Cod_Empleado,
        Cod_Usuario = 999,
        Dias = data.Dias,
        Desde = UtilHelper.getUnglyDate(data.Desde),
        Hasta = UtilHelper.getUnglyDate(Hasta),
        Cod_Diag = data.Cod_Diagnostico,
        NumComprobante = "",
        Horas = 0,
        Prorroga = 0,
        DescuentaAuxilio = "",
        Cod_Imagen = 0,
        Fec_Aprobado= "",
        Fec_Desaprobado = "",
        Estado = Estado,
        Num_Autoriza="",
        Val_Novedad = 0,
        Dias_Pag_100 = 0,
        Val_Reembolso_Eps=0,
        IBC=0,
        Dias_Pag_100_Ant=0
      };
      var n = db.NOVAUT.Add(New_Novaut);
      if (concepto.Tipo_Concepto != "L" || concepto.Nom_Concepto == "LICENCIA DE LUTO")
      {
        db.SaveChanges();
      }
      if (concepto.Tipo_Concepto != "L" && dctPrms != null && dctPrms.ContainsKey("Tercero_informar"))
      {
        string tipoTercero = dctPrms["Tercero_informar"].ToString();
        List<TERCEROS> terceros = db.TERCEROS.Where(x => x.Tipo_Tercero == tipoTercero && !string.IsNullOrEmpty(x.Dir_Elec)).ToList();
        if (terceros.Count > 0)
          MailHelper.EnviarReporteIncapacidad(empleado, string.Join(";", terceros.Select(x => x.Dir_Elec.Trim())), New_Novaut, concepto.Nom_Concepto, data.Nom_Diagnostico);
      }
      var DirCopiaRRHH = db.VARIABLES.SingleOrDefault(v => v.Uso == 90);
      if (DirCopiaRRHH != null)
      {
        MailHelper.EnviarReporteIncapacidad(empleado, DirCopiaRRHH.Descripcion.Trim(), New_Novaut, concepto.Nom_Concepto);
      }
      if (concepto.Tipo_Concepto != "L" || concepto.Nom_Concepto == "LICENCIA DE LUTO")
      {
        List<short> codAprobadores = db.APROBADORES.Where(x => x.Cod_Filtro == Cod_Empleado && x.Estado == "A").Select(x => x.Cod_Empleado).ToList();
        List<TERCEROS> terceros = db.TERCEROS.Where(x => codAprobadores.Contains(x.Cod_Tercero) && !string.IsNullOrEmpty(x.Dir_Elec)).ToList();
        if (terceros.Count > 0)
          MailHelper.EnviarReporteIncapacidad(empleado, string.Join(";", terceros.Select(x => x.Dir_Elec.Trim())), New_Novaut, concepto.Nom_Concepto);
      }
      return Ok(new { Cod_Incapacidad = n.AutoNum, Desde = data.Desde, Hasta = Hasta, Cod_Solicitud = solicitud?.Cod_Solicitud });
    }

    [Route("Delete/{Empresa}/{Cod_Incapacidad}")]
    [HttpPost]
    public IHttpActionResult DeleteIncapacidad(string Empresa, int Cod_Incapacidad)
    {
      bool saved = true;
      string message = "";

      JulianaContext db = new JulianaContext(Empresa);
      NOVAUT inc = db.NOVAUT.FirstOrDefault(i => i.AutoNum == Cod_Incapacidad);
      if (inc == null)
      {
        saved = false;
        message = "Incapacidad NO encontrada.";
      }

      if (saved)
      {
        CONCEPTOS con = db.CONCEPTOS.FirstOrDefault(c => c.Cod_Concepto == inc.Cod_Concepto);
        if (con == null || (con.Tipo_Concepto != "I" && con.Nom_Concepto != "LICENCIA DE LUTO"))
        {
          saved = false;
          message = "El registro no es una Incapacidad.";
        }
      }

      if (saved)
      {
        if (!UtilHelper.EsIgualA(inc.Estado, "", " "))
        {
          saved = false;
          message = "La incapacidad no puede ser eliminada. Ya está en proceso.";
        }
      }

      try
      {
        if (saved)
        {
          db.NOVAUT.Remove(inc);
          db.SaveChanges();
          message = "El registro de Incapacidad fue eliminado con éxito.";
        }
      }
      catch (Exception)
      {
        saved = false;
        message = "Ocurrió un error al eliminar la incapacidad reportada. Intente más tarde.";
      }
      finally
      {
        db.Dispose();
      }

      return Ok(new {
        status = saved ? "OK" : "BAD",
        message
      });
    }
  

    [Route("Adjunto/{Empresa}/{Cod_Incapacidad}")]
    [HttpPost]
    public IHttpActionResult Upload_Adjunto(string Empresa, int Cod_Incapacidad)
    {
      string uploadpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Client/uploads/");
      string year = DateTime.Now.ToString("yyyy");
      string month = DateTime.Now.ToString("MM");
      string day = DateTime.Now.ToString("dd");
      string fileUploadPath = uploadpath + "/" + year + "/" + month + "/" + day + "/";
      string fileUrl = "Client/uploads/" + year + "/" + month + "/" + day + "/";
      if (System.Web.HttpContext.Current.Request.Files.Count > 0)
      {
        JulianaContext db = new JulianaContext(Empresa);
        NOVAUT inc = db.NOVAUT.FirstOrDefault(i => i.AutoNum == Cod_Incapacidad);
        new FileInfo(fileUploadPath).Directory.Create();
        var file = System.Web.HttpContext.Current.Request.Files[0];
        var CodEmpleado = inc.Cod_Empleado;
        string fechaActual = DateTime.Now.ToString("yyMMdd_HHmms");
        if (file != null)
        {
          var filename = Empresa + "_" + CodEmpleado + "_" + Cod_Incapacidad + "_" + fechaActual + ".pdf";
          file.SaveAs(fileUploadPath + filename);
          inc.Adjunto = fileUrl + filename;
          db.Entry(inc).State = EntityState.Modified;
          db.SaveChanges();
        }
      }
      return Ok();
    }

    [Route("Adjunto/LicenciaDeLuto/{Empresa}/{Cod_Solicitud}")]
    [HttpPost]
    public IHttpActionResult Upload_Adjunto_LicenciaDeLuto(string Empresa, int Cod_Solicitud)
    {
      string uploadpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Client/uploads/");
      string year = DateTime.Now.ToString("yyyy");
      string month = DateTime.Now.ToString("MM");
      string day = DateTime.Now.ToString("dd");
      string fileUploadPath = uploadpath + "/" + year + "/" + month + "/" + day + "/";
      string fileUrl = "Client/uploads/" + year + "/" + month + "/" + day + "/";
      if (System.Web.HttpContext.Current.Request.Files.Count > 0)
      {
        JulianaContext db = new JulianaContext(Empresa);
        SOLICITUDES sol = db.SOLICITUDES.FirstOrDefault(s => s.Cod_Solicitud == Cod_Solicitud); ;
        new FileInfo(fileUploadPath).Directory.Create();
        var file = System.Web.HttpContext.Current.Request.Files[0];
        var CodEmpleado = sol.Cod_Empleado;
        string fechaActual = DateTime.Now.ToString("yyMMdd_HHmms");
        if (file != null)
        {
          var filename = Empresa + "_" + CodEmpleado + "_" + Cod_Solicitud + "_" + fechaActual + ".pdf";
          file.SaveAs(fileUploadPath + filename);
          sol.Adjunto = fileUrl + filename;
          db.Entry(sol).State = EntityState.Modified;
          db.SaveChanges();
        }
      }
      return Ok();
    }

    [Route("PreloadData/Incapacidades/{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_Config(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      List<CONCEPTOS> conceptos = db.CONCEPTOS.Where(c => c.Tipo_Concepto == "I").ToList();
      List<PARAMETROS_GENERALES> parametros = db.PARAMETROS_GENERALES.Where(p => p.Cod_Parametro.Trim().Contains("CONCEPTOS_")).ToList();
      List<DIAGNOSTICOS> diagnosticos = db.DIAGNOSTICOS.OrderBy(d => d.Descripcion).ToList();
      //List<DIAGNOSTICOS> diagnosticos = new List<DIAGNOSTICOS>();



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

      return Ok(new
      {
        conceptos,
        diagnosticos,
        Dia_Cierre_Novedades,
        Rango_Fec_Desde,
        Rango_Fec_Hasta,
        Periodo_Cerrado,
        parametros
      });
    }

    [Route("PreloadData/Licencias/{Empresa}")]
    [HttpGet]
    public IHttpActionResult Get_Config2(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      List<CONCEPTOS> conceptos;
      List<PARAMETROS_GENERALES> parametros = db.PARAMETROS_GENERALES.Where(p => p.Cod_Parametro.Trim().Contains("CONCEPTOS_")).ToList();
      // Ausentimos especiales, BNP
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
      //List<DIAGNOSTICOS> diagnosticos = db.DIAGNOSTICOS.OrderBy(d => d.Descripcion).ToList();
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

      var aprobadores = db.TERCEROS.Where(t => t.Tipo_Tercero == "8").ToList();
      return Ok(new
      {
        conceptos,
        Dia_Cierre_Novedades,
        Rango_Fec_Desde,
        Rango_Fec_Hasta,
        aprobadores,
        UsaAusentimosEspeciales,
        Cod_MiTiempo,
        Cod_MiCumpleanos,
        Cod_ServicioMilitar,
        Cod_DonaccionSangre,
        Cod_LicXMatrimonio,
        Cod_ExamenIngEduSup,
        parametros
      });
    }


  }
}
