using JulianaWeb.Business;
using JulianaWeb.Models.ViewModels;
using System.Web.Http;


namespace JulianaWeb.Controllers.API
{

    [RoutePrefix("API/Bioseguridad")]
    public class BioseguirdadController : ApiController
  {
        //Bussiness Object
        private readonly BioseguridadBO BioseguirdadBO = new BioseguridadBO();

        [Route("{Empresa}/{Cedula}/Data")]
        [HttpGet]
        public IHttpActionResult Get_Data_Bioseguridad(string Empresa, string Cedula)
        {
            return Ok(BioseguirdadBO.GetDataBioseguridad(Empresa, Cedula));
        }
        [Route("{Cedula}/AddRespuestas")]
        [HttpPost]
        public IHttpActionResult AgregarNuevasRespuestas(string Cedula, [FromBody] BioseguridadViewModel data)
        {
            return Ok(BioseguirdadBO.AddNuevaRespuesta(Cedula, data));
        }
    }
}
