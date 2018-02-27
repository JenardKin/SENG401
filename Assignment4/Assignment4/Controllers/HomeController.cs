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
            //Basic view returned on default HttpGet request
            return View();
        }
        [HttpPost]
        //Method saves a json object to our DB
        //HttpPost with JSON object will be deserialized into a Reviews object
        public string SaveCompanyReview(Reviews reviews)
        {
            //Save the review field for easier access
            var r = reviews.review;
            //Create a response object and a serializer to serialize the response
            Response response = new Response();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //Check if any of the fields of the deserialized object is null.
            //Also check that the number of stars is in the range 1 to 5
            if (r == null || r.companyName == null || r.username == null || 
                r.review == null || r.stars == null || r.timestamp == null ||
                r.stars < 1 || r.stars > 5)
            {
                //If any of these fail, return a failure
                response.response = "failure";
            }
            else
            {
                //Get the db instance (and generate the DB if needed)
                var dbInstance = ReviewDatabase.getInstance();
                dbInstance.createDB();
                //Try to save the company review, if successful report so, if not report failure
                if(dbInstance.saveCompanyReview(r.companyName, r.username, r.review, r.stars, r.timestamp))
                    response.response = "success";
                else
                    response.response = "failure";
            }
            //Serialize the response and return it
            return ser.Serialize(response);
        }
        private string GetReviews(CompanyName name)
        {
            //Create a ResponseReview object and serializer
            ResponseReview response = new ResponseReview();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //Check that the companyName wasn't null
            if (name == null || name.companyName == null)
            {
                //If so, return a serialized response object
                return ser.Serialize(new Response() { response = "failure" });
            }
            else
            {
                //Get the db instance (and create the DB if needed)
                var dbInstance = ReviewDatabase.getInstance();
                dbInstance.createDB();
                List<Review> list = null;
                try
                {
                    //try to get the list of reviews
                    list = dbInstance.getCompanyReviews(name.companyName);
                }
                catch (ArgumentException)
                {
                    //If there was no entry in the DB with the company name, serialize a failure response
                    return ser.Serialize(new Response() { response = "failure" });
                }
                //The following converts a list to an array
                //Though it is not necessary, the serialization of an array felt like a better option for json objects
                //Generate the review list to the same size as the list of reviews
                response.reviews = new Review[list.Count];
                int i = 0;
                //Loop through all items in the list
                foreach (var review in list)
                {
                    //foreach item in the list, save it at the current index of the array, then increment the index
                    response.reviews[i] = review;
                    i++;
                }
                //Generate the response field to success
                response.response = "success";
            }
            //Serialize the ResponseReview object and return
            return ser.Serialize(response);
        }
        [HttpGet]
        //Method gets company reviews from our DB and returns their serialized form
        //HttpGet with string parameter will be deserialized into a CompanyName object
        public string GetCompanyReview(string companyName)
        {
            CompanyName name = new CompanyName()
            {
                companyName = companyName
            };
            return this.GetReviews(name);
        }
        [HttpPost]
        //Method gets company reviews from our DB and returns their serialized form
        //HttpPost with JSON object will be deserialized into a CompanyName object
        public string GetCompanyReview(CompanyName name)
        {
            return this.GetReviews(name);
        }
    }
}