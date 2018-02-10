using System;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using seng401a3.Models;

namespace seng401a3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult LinkShortener(string longURL = null)
        {
            string shortURL = longURL;
            ViewData["Title"] = "Link Shortener";
            //If string from post was null, then this field isn't displayed
            if(longURL != null){
                //Randomly choose and int... This will actually be the PK value in the DB
                Random rnd = new Random();
                int value = rnd.Next(0, 100);
                //Algorithm for shortended link
                string encoding = Shortener.GetShortEncoding(value);
                int decoding = Shortener.GetLongDecoding(encoding);
                string urlHost = Request.Host.ToString();
                shortURL = urlHost + "/Home/Go/" + encoding;
            }
            ViewData["shortURL"] = shortURL;

            return View();
        }
        public void Go(string id = null)
        {
            if (id != null)
            {
                int decoding = Shortener.GetLongDecoding(id);
                //Ideally, with the above ID we will squery the DB and get the respective url and redirect to that page
                Response.Redirect("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
            }
            else
            {
                //Otherwise, redirect back to our homepage
                Response.Redirect(Request.Host.ToString());
            }
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
