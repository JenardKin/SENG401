using LinkShortener.Models;
using LinkShortener.Models.Database;
using System;
using System.Web.Mvc;

namespace Assignment3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult LinkShortener(string longURL)
        {   
            var dbInstance = LinkDatabase.getInstance();
            dbInstance.createDB();

            string shortURL = longURL;
            ViewBag.Title = "Link Shortener";
            //If string from post was null, then this field isn't displayed
            if (longURL != null)
            {
                string shortUrl_iD = dbInstance.saveLongURL(longURL);
                string encoding = Shortener.GetShortEncoding(Int32.Parse(shortUrl_iD));
                string urlHost = HttpContext.Request.Url.Host;
                string urlPort = HttpContext.Request.Url.Port.ToString();
                if (urlPort != "")
                {
                    shortURL = urlHost + ":" + urlPort + "/Home/Go/" + encoding;
                }
                else
                {
                    shortURL = urlHost + "/Home/Go/" + encoding;
                }
            }
            ViewBag.shortURL = shortURL;

            return View();
        }
        public void Go(string id = null)
        {
            var dbInstance = LinkDatabase.getInstance();

            if (id != null)
            {
                int decoding = Shortener.GetLongDecoding(id);
                string longUrl = dbInstance.getLongUrl(decoding.ToString());
                //Ideally, with the above ID we will squery the DB and get the respective url and redirect to that page
                Response.Redirect(longUrl);
            }
            else
            {
                //Otherwise, redirect back to our homepage
                Response.Redirect(HttpContext.Request.Url.Host);
            }
        }
    }
}