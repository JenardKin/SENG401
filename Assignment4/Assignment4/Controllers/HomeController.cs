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
            if (reviews.review == null || reviews.review.companyName == null || reviews.review.username == null || 
                reviews.review.review == null || reviews.review.stars == null || reviews.review.timestamp == null ||
                reviews.review.stars < 1 || reviews.review.stars > 5)
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