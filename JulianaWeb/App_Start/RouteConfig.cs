using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JulianaWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Pdfs",
                url: "pdf/{action}/{id}",
                defaults: new { controller = "Pdf", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Excels",
                url: "Excel/{action}/{id}",
                defaults: new { controller = "Excel", action = "Index", id = UrlParameter.Optional }
            );


          routes.MapRoute(
                name: "Generar",
                url: "GenerarCertlaboral/{action}/{id}",
                defaults: new { controller = "GenerarCertlaboral", action = "Index", id = UrlParameter.Optional }
            );

      routes.MapRoute(
               name: "SSO",
               url: "SSO/{action}",
               defaults: new { controller = "SSO", action = "Index" }
           );

      routes.MapRoute(
                name: "SPA",
                url: "{*catchall}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
