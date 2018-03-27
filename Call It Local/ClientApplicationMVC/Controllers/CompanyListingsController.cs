using ClientApplicationMVC.Models;

using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;

using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net.Http;

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
        public ActionResult DisplayCompany(string id)
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
            //GET ALL REVIEWS WITH POST
            ViewBag.CompanyName = id;
            ViewBag.AvgStars = 10;
            return View("DisplayReviews");
        }

        public ActionResult SubmitReview(string review, string companyName, string stars)
        {
            //SAVE REVIEW WITH POST
            //http://localhost:49834/
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
                var stringContent = new StringContent(reviewsObject.ToString());
                var response = companyReviewClient.PostAsync("http://localhost:49834/Home/SaveCompanyReview", stringContent);
                response.
                return RedirectToAction("DisplayReviews", "CompanyListsings", new { id = companyName });
            }
            else
                return View("Index", "Home");
        }
    }
    public class Review
    {
        public string companyName { get; set; }
        public string username { get; set; }
        public string review { get; set; }
        public int? stars { get; set; }
        public long? timestamp { get; set; }
    }
    public class Reviews
    {
        public Review review { get; set; }
    }
}