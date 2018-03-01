using System;

namespace Messages.ServiceBusRequest.Authentication
{
    [Serializable]
    public class AuthenticationServiceRequest : ServiceBusRequest
    {
        public AuthenticationServiceRequest(AuthenticationRequest requestType)
            : base(Service.Authentication)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the Authentication Service
        /// </summary>
        public AuthenticationRequest requestType;
    }

    public enum AuthenticationRequest { LogIn, CreateAccount };
}
