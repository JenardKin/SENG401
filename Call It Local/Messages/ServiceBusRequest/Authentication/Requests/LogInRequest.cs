using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.ServiceBusRequest.Authentication.Requests
{
    [Serializable]
    public class LogInRequest : AuthenticationServiceRequest
    {
        public LogInRequest(string username, string password) : base(AuthenticationRequest.LogIn)
        {
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// The username being used to log in
        /// </summary>
        public string username = "";

        /// <summary>
        /// The password being used to log in
        /// </summary>
        public string password = "";
    }
}
