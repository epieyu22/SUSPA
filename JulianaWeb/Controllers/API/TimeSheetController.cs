using JulianaWeb.Business;
using JulianaWeb.Models;
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

  [RoutePrefix("API/TimeSheet")]
  public class TimeSheetController : ApiController
  {
    //Bussiness Object
    private readonly TimeSheetBO timeSheetBO = new TimeSheetBO();

    [Route("{Empresa}/Data")]
    public IHttpActionResult Get_Data_TimeSheet(string Empresa)
    {
      return Ok(timeSheetBO.GetDataTimeSheet(Empresa));
    }
    [Route("{Cedula}/AgregarRegHoras")]
    [HttpPut]
    public IHttpActionResult AgregarNuevasHoras(string Cedula, [FromBody] TimeSheetViewModel data)
    {
      return Ok(timeSheetBO.AddDataTimeSheet(Cedula, data));
    }
  }
}
