using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JulianaWeb.Helpers
{
    public class AllowCrossSiteJsonAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type");
            base.OnActionExecuting(filterContext);
        }
    }
}