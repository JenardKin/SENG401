using System;
using System.Collections.Generic;
using ClientApplicationMVC.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ClientApplicationMVC
{
    public class MvcApplication : HttpApplication
    {
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();//Added by VS upon creation of project
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//Added by VS upon creation of project
            RouteConfig.RegisterRoutes(RouteTable.Routes);//Added by VS upon creation of project
            BundleConfig.RegisterBundles(BundleTable.Bundles);//Added by VS upon creation of project
        }

        public void Session_OnStart()
        {
            HttpContext.Current.Session.Add("user", "Log In");
            HttpContext.Current.Session.Timeout = Globals.patienceLevel_ms / (1000 * 60);//Convert from ms to minutes
        }
    }
}
