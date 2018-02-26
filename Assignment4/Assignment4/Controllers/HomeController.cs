using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Assignment4.Models;

namespace Assignment4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string SaveCompanyReview(Reviews reviews)
        {
            Response response = new Response();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            if (reviews.review == null)
            {
                response.response = "failure";
            }
            else
            {
                response.response = "success";
            }
            return ser.Serialize(response);
        }
        public Review[] GetCompanyReview()
        {
            return new Review[0];
        }
    }
}