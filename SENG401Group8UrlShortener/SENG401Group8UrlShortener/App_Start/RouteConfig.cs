using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SENG401Group8UrlShortener
{
    public class RouteConfig
    {
        public static void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "LinkShortener", id = System.Web.Mvc.UrlParameter.Optional }
            );
        }
    }
}
