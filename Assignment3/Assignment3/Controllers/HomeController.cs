using LinkShortener.Models;
using LinkShortener.Models.Database;
using System;
using System.Web.Mvc;

namespace Assignment3.Controllers
{
    public class HomeController : Controller
    {
        //Defualt action of home controller is linkshortener rather than index (changed in route config)
        //Action takes an optional parameter (longURL) - The url to be shortened
        public ActionResult LinkShortener(string longURL = null)
        {   
            //Save the short url as the long url (currently)
            string shortURL = longURL;
            //Add the current title to the view bag (for display in the tab on a web broswer)
            ViewBag.Title = "Link Shortener";
            //If string from post was null, then this if statement is never entered
            if (longURL != null)
            {
                //Get the DB instance (and if necessary the DB itself)
                var dbInstance = LinkDatabase.getInstance();
                dbInstance.createDB();
                //Save the longurl in the db and save the return as primary key
                string pk = dbInstance.saveLongURL(longURL);
                //Encode the PK
                string encoding = Shortener.GetShortEncoding(Int32.Parse(pk));
                //Get the host and port parts of our website url
                string urlHost = HttpContext.Request.Url.Host;
                int urlPort = HttpContext.Request.Url.Port;
                //If no port, ignore adding it to the shortened url
                if (urlPort == 80)
                {
                    //Short url looks like: <website name>:<port>/Home/Go/<encoded pk>
                    shortURL = urlHost + "/Home/Go/" + encoding;
                }
                else
                {
                    //Short url looks like: <website name>/Home/Go/<encoded pk>
                    shortURL = urlHost + ":" + urlPort.ToString() +  "/Home/Go/" + encoding;
                }
            }
            //Return the shortened (or null) short url to the view
            ViewBag.shortURL = shortURL;
            //Render the view
            return View();
        }
        //Action to redirect to 
        public ActionResult Go(string id = null)
        {
            //Get the db instance
            var dbInstance = LinkDatabase.getInstance();

            if (id != null)
            {
                int decoding = Shortener.GetLongDecoding(id);
                try
                {
                    string longUrl = dbInstance.getLongUrl(decoding.ToString());
                    //Ideally, with the above ID we will query the DB and get the respective url and redirect to that page
                    //Response.Redirect(longUrl);
                    return new RedirectResult(longUrl);
                }
                catch (ArgumentException)
                {
                    //If long url couldn't be found, redirect back to our homepage
                    return RedirectToAction("LinkShortener", "Home");
                }
            }
            else
            {
                //Otherwise, redirect back to our homepage
                return RedirectToAction("LinkShortener", "Home");
            }
        }
    }
}