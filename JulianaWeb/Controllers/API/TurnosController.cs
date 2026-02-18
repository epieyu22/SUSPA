using JulianaWeb.Business;
using JulianaWeb.Models;
using JulianaWeb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace JulianaWeb.Controllers
{

  [RoutePrefix("API/Turnos")]
  public class TurnosController : ApiController
  {
    //Bussiness Object
    private readonly TurnosBO turnosBO = new TurnosBO();

    [Route("{Usuario}/Data")]
    public IHttpActionResult Get_Data_Turnos(string Usuario)
    {
      return Ok(turnosBO.Get_Data_Turnos(Usuario));
    }

    [Route("{Cod_Depto}/DataPorDepto")]
    public IHttpActionResult GetDataPorDepartamento(short Cod_Depto)
    {
      return Ok(turnosBO.GetDataEmpleadosPorDepto(Cod_Depto));
    }
    [Route("{Empresa}/Add_Turno")]
    [HttpPut]
    public IHttpActionResult Add_Data_Turnos(string Empresa, [FromBody] TurnosViewModel data)
    {
      return Ok(turnosBO.AddTurno(Empresa, data));
    }

    [HttpPut]
    [Route("time-controls")]
    public List<TIME_CONTROL> GetTimeControls(TimeControlFilterViewModel filters)
    {
      return this.turnosBO.GetTimeControls(filters);
    }

    [HttpPut]
    [Route("register-punch")]
    public void RegisterPunch(TimeControlFilterViewModel payload)
    {
      this.turnosBO.RegisterPunch(payload);

      Ok();
    }
  }
}
