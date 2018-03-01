using ClientApplicationMVC.Models;

using Messages.ServiceBusRequest.Echo.Requests;
using Messages.ServiceBusRequest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientApplicationMVC.Controllers
{
    public class EchoController : Controller
    {
        // GET: Echo
        public ActionResult Index()
        {
            ViewBag.AsIsResponse = " ";
            ViewBag.ReverseResponse = " ";
            return View();
        }

        /// <summary>
        /// This function is called by the client on the users computer when they request an asisecho
        /// </summary>
        /// <param name="asIsText">The text to be sent to the service bus</param>
        /// <returns>An html page containing the response from the service bus</returns>
        public ActionResult AsIsEcho(string asIsText)
        {
            AsIsEchoRequest request = new AsIsEchoRequest(asIsText);
            ServiceBusResponse response;
            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if(connection == null)
            {
                response = ConnectionManager.echoAsIs(request);
            }
            else
            {
                response = connection.echoAsIs(request);
            }

            ViewBag.AsIsResponse = response.response;

            return View("Index");
        }

        /// <summary>
        /// This function is called by the client on the users computer when they request a reverse echo
        /// </summary>
        /// <param name="reverseText">The text to be sent to the service bus</param>
        /// <returns>An html page containing the response from the service bus</returns>
        public ActionResult ReverseEcho(string reverseText)
        {
            ReverseEchoRequest request = new ReverseEchoRequest(reverseText, Globals.getUser());
            ServiceBusResponse response;
            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                response = new ServiceBusResponse(false, "Error: You must be logged in to use the echo reverse functionality.");
            }
            else
            {
                response = connection.echoReverse(request);
            }

            ViewBag.ReverseResponse = response.response;

            return View("Index");
        }
    }
}