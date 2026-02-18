using JulianaWeb.Models;
using JulianaWeb.Models.ViewModels;
using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace JulianaWeb.Business
{
  public class BioseguridadBO : ApiController
  {    
    public dynamic GetDataBioseguridad(string Empresa, string Cedula)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var Respuestas = db.BIOSEGURIDAD_PREGUNTAS.ToList();
      return new { Respuestas};
    }
    
    public dynamic AddNuevaRespuesta(string Cedula, BioseguridadViewModel data)
    {
      JulianaContext db = new JulianaContext();
      DateTime FechaActual = DateTime.Now;
      string error = "Prueba";

      var empleado = db.EMPLEADOS.FirstOrDefault(i => i.Cedula == Cedula);
      var Respuestas = db.BIOSEGURIDAD_PREGUNTAS.Where(x => x.Cod_Empleado == Cedula).ToList();

      var nuevaRespuesta = new BIOSEGURIDAD_PREGUNTAS
      {
        Cod_Empleado = Cedula,
        pregunta_1 = data.respuesta1,
        pregunta_2 = data.respuesta2,
        pregunta_3 = data.respuesta3,
        pregunta_4 = data.respuesta4,
        pregunta_5 = data.respuesta5,
        pregunta_6 = data.respuesta6,
        pregunta_7 = data.respuesta7,
        pregunta_8 = data.respuesta8,
        pregunta_9 = data.respuesta9,
        pregunta_10 = data.respuesta10,
        fecha = FechaActual.Date.ToString("ddMMyyyy"),
      };

      int lastIndex = Respuestas.Count - 1;
      try
      {
        foreach (var rta in Respuestas)
        {
          var rtaLast = Respuestas[lastIndex];
          if (rtaLast.fecha != FechaActual.Date.ToString("ddMMyyyy"))
          {
            db.BIOSEGURIDAD_PREGUNTAS.Add(nuevaRespuesta);
            db.SaveChanges();
          }
          else {

            return BadRequest(error);

          }
        }
        if (lastIndex == -1)
        {
          db.BIOSEGURIDAD_PREGUNTAS.Add(nuevaRespuesta);
          db.SaveChanges();
        }
      }
      catch (Exception e)
      {

        throw e;
      }
     
      return new { nuevaRespuesta };
    }
  }
}
