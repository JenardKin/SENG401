using System;
using System.Collections.Generic;
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

        [HttpPost]
        public string GetCompanyReview(CompanyName name)
        {
            
            ResponseReview response = new ResponseReview();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            if (name == null || name.companyName == null)
            {
                response.response = "failure";
                return ser.Serialize(response.response);
            }
            else
            {
                var dbInstance = ReviewDatabase.getInstance();
                dbInstance.createDB();
                List<Review> list = null;
                try
                {
                    list = dbInstance.getCompanyReviews(name.companyName);
                }
                catch (ArgumentException)
                {
                    Response r = new Response() { response = "failure" };
                    return ser.Serialize(r);
                }

                var listlength = list.Count;
                response.reviews = new Review[listlength];
                int i = 0;
                foreach(var review in list)
                {
                    response.reviews[i] = review;
                    i++;
                }
            }

            return ser.Serialize(response);
        }
    }
}