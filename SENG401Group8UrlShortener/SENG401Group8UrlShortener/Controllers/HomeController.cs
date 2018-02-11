using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SENG401Group8UrlShortener.Models;
using LinkShortener.Models.Database;

namespace SENG401Group8UrlShortener.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult LinkShortener(string longURL)
        {
            var dbInstance = LinkDatabase.getInstance();
            dbInstance.createDB();
            
            string shortURL = longURL;
            ViewData["Title"] = "Link Shortener";
            //If string from post was null, then this field isn't displayed
            if (longURL != null)
            {
                //Randomly choose and int... This will actually be the PK value in the DB
                string shortUrl_iD = dbInstance.saveLongURL(longURL);
                string urlHost = HttpContext.Request.Url.Host;
                shortURL = urlHost + "/Home/Go/" + shortUrl_iD;
            }
            ViewData["shortURL"] = shortURL;

            return View();
        }

        public void Go(string id = null)
        {
            var dbInstance = LinkDatabase.getInstance();

            if (id != null)
            {
                string origUrl = dbInstance.getLongUrl(id);
                //Ideally, with the above ID we will squery the DB and get the respective url and redirect to that page
                Response.Redirect(origUrl);
            }
            else
            {
                //Otherwise, redirect back to our homepage
                Response.Redirect(HttpContext.Request.Url.Host);
            }
        }

    }
}