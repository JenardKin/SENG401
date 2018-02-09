using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using seng401a3.Models;

namespace seng401a3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult LinkShortener(string longURL)
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
                shortURL = "Encoding of PK: " + value.ToString() + " equals: " + encoding;
            }
            ViewData["shortURL"] = shortURL;

            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
