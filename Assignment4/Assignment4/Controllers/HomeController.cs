using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Assignment4.Models;
using Assignment4.Models.Database;

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
            var r = reviews.review;
            Response response = new Response();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            if (r == null || r.companyName == null || r.username == null || 
                r.review == null || r.stars == null || r.timestamp == null ||
                r.stars < 1 || r.stars > 5)
            {
                response.response = "failure";
            }
            else
            {
                var dbInstance = ReviewDatabase.getInstance();
                dbInstance.createDB();
                if(dbInstance.saveCompanyReview(r.companyName, r.username, r.review, r.stars, r.timestamp))
                    response.response = "success";
                else
                    response.response = "failure";
            }
            return ser.Serialize(response);
        }
        public Review[] GetCompanyReview()
        {
            return new Review[0];
        }
    }
}