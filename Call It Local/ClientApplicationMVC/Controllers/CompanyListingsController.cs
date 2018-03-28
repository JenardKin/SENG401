using ClientApplicationMVC.Models;

using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;

using System;
using System.Web.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/CompanyListings/*
    /// </summary>
    public class CompanyListingsController : Controller
    {
        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Index()
        {
            if (Globals.isLoggedIn())
            {
                ViewBag.Companylist = null;
                return View("Index");
            }
            return RedirectToAction("Index", "Authentication");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/Search
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Search(string textCompanyName)
        {

            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if(connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            CompanySearchRequest request = new CompanySearchRequest(textCompanyName);

            CompanySearchResponse response = connection.searchCompanyByName(request);
            if (response.result == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.Companylist = response.list;

            return View("Index");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/DisplayCompany/*info*
        /// </summary>
        /// <param name="id">The name of the company whos info is to be displayed</param>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult DisplayCompany(string id, string responseStatus = null)
        {
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if ("".Equals(id))
            {
                return View("Index");
            }
 
            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.CompanyName = id;
            ViewBag.ResponseStatus = responseStatus;

            GetCompanyInfoRequest infoRequest = new GetCompanyInfoRequest(new CompanyInstance(id));
            GetCompanyInfoResponse infoResponse = connection.getCompanyInfo(infoRequest);
            ViewBag.CompanyInfo = infoResponse.companyInfo;

            return View("DisplayCompany");
        }

        public ActionResult DisplayReviews(string id)
        {
            if(Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if ("".Equals(id))
            {
                return View("Index");
            }
            var url = "http://localhost:49834/Home/GetCompanyReview/" + id;
            var companyReviewClient = new HttpClient();
            var content = companyReviewClient.GetStringAsync(url).Result;
            ResponseReview result = JsonConvert.DeserializeObject<ResponseReview>(content);
            int? totalStars = 0;
            int totalReviews = 0;
            foreach(var review in result.reviews)
            {
                totalStars += review.stars;
                totalReviews++;
            }
            //Format to 2 decimal places
            var avgStars = ((float)totalStars / totalReviews).ToString("0.00");
            ViewBag.AvgStars = avgStars;
            ViewBag.CompanyName = id;
            ViewBag.ReviewList = result.reviews;
            return View("DisplayReviews");
        }

        public ActionResult SubmitReview(string review, string companyName, string stars)
        {
            //SAVE REVIEW WITH POST
            //http://localhost:49834/
            //THIS IP NEEDS TO BE OF THE DEPLOYMENT
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if (review != "" && companyName != "" && stars != "")
            {
                string username = Globals.getUser();
                long timestamp = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                Review reviewObject = new Review()
                {
                    companyName = companyName,
                    username = username,
                    review = review,
                    stars = Int32.Parse(stars),
                    timestamp = timestamp
                };
                Reviews reviewsObject = new Reviews()
                {
                    review = reviewObject
                };
                var companyReviewClient = new HttpClient();
                var jsonObject = JsonConvert.SerializeObject(reviewsObject);
                var stringContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var res = companyReviewClient.PostAsync("http://localhost:49834/Home/SaveCompanyReview", stringContent).Result.Content.ReadAsStringAsync().Result;
                Response resObject = JsonConvert.DeserializeObject<Response>(res);
                if(resObject.response == "failure")
                {
                    return RedirectToAction("DisplayCompany", new { id = companyName, responseStatus = "Error occured when submitting review" });
                }
                else
                    return RedirectToAction("DisplayCompany", new { id = companyName, responseStatus = "Review successfully saved" });
            }
            else
                return View("Index", "Home");
        }
    }
}