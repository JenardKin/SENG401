using ClientApplicationMVC.Models;

using Messages.NServiceBus.Commands;
using Messages.DataTypes;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication.Requests;

using System.Web.Mvc;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/Authentication/*
    /// </summary>
    public class AuthenticationController : Controller
    {
        /// <summary>
        /// The default method for this controller
        /// </summary>
        /// <returns>The login page</returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Please enter your username and password.";
            return View("Index");
        }
		
		//This class is incomplete and should be completed by the students in milestone 2
		//Hint: You will need to make use of the ServiceBusConnection class. See EchoController.cs for an example.
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        //HTTP Get
        public ActionResult UserLogIn(string username, string password)
        {
            LogInRequest request = new LogInRequest(username, password);
            ServiceBusResponse response = ConnectionManager.sendLogIn(request);
            if (response.response.Equals(""))
            {
                if (response.result)
                    return RedirectToAction("Index", "Home", new { msg = "Hello " + username + " logged in sucessfully!" });
                else
                    ViewBag.LogInResponse = "Invalid username or password!";
            }
            else
            {
                ViewBag.LogInResponse = "Invalid log in request: " + response.response;
            }

            ViewBag.LogInResult = response.result;

            return View("Index");
        }

        public ActionResult UserCreateAccount(string username, string password, string address, string phonenumber, string email, string accountType)
        {
            var account = AccountType.notspecified;
            switch (accountType)
            {
                case "user":
                    account = AccountType.user;
                    break;
                case "business":
                    account = AccountType.business;
                    break;
            }
            CreateAccountRequest request = new CreateAccountRequest(new CreateAccount()
            {
                username = username,
                password = password,
                address = address,
                email = email,
                phonenumber = phonenumber,
                type = account
            });
            ServiceBusResponse response = ConnectionManager.sendNewAccountInfo(request);
            if (response.result)
            {
                return RedirectToAction("Index", "Home", new { msg = "Hello " + username + " account successfully created!" } );
            }
            ViewBag.CreateAccountResponse = response.response;
            return View("CreateAccount");
        }
    }
}