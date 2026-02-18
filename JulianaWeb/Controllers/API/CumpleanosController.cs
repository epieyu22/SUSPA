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

  [RoutePrefix("API/Cumpleanos")]
  public class CumpleanosController : ApiController
  {
    //Bussiness Object
    private readonly CumpleanosBO cumpleanosBO = new CumpleanosBO();

    [Route("{Empresa}/Data")]
    public IHttpActionResult Get_Data_Cumpleanos(string Empresa)
    {
      return Ok(cumpleanosBO.GetDataCumpleanos(Empresa));
    }

  }
}
