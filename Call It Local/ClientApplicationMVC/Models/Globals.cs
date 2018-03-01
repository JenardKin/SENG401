using System.Threading;
using System.Web;

namespace ClientApplicationMVC.Models
{
    /// <summary>
    /// This class contains functions and variables relevant to many different places in the web server.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// The amount of time the client should wait for a response from the server
        /// Value of 600000 is 10mins
        /// </summary>
        public const int patienceLevel_ms = 600000 * 3;

        /// <summary>
        /// Returns true if the client is currently logged in.
        /// </summary>
        /// <returns>True if logged in. False otherwise.</returns>
        public static bool isLoggedIn()
        {
            if("Log In".Equals(getUser()))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sets the name of the current user
        /// </summary>
        /// <param name="user">The user name</param>
        public static void setUser(string user)
        {
            //TODO: What if the user is already logged in on another device ?
            HttpContext.Current.Session["user"] = user;
            //HttpContext.Current.Session.
            //HttpContext.Current.User.Identity.
        }

        /// <summary>
        /// gets the name of the current user
        /// </summary>
        /// <returns>The name of the current user</returns>
        public static string getUser()
        {
            return (string)HttpContext.Current.Session["user"];
        }
    }
}