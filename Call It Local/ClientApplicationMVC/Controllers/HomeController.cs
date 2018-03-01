using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This is the controller that is used by default when a client navigates to the web page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The default controller method.
        /// </summary>
        /// <returns>The home page</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/Home/Contact
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}