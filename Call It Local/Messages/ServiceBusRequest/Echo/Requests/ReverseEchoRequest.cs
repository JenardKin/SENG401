using Messages.ServiceBusRequest.Echo;

using System;

namespace Messages.ServiceBusRequest.Echo.Requests
{
    [Serializable]
    public class ReverseEchoRequest : EchoServiceRequest
    {
        public ReverseEchoRequest(string data, string username)
            : base(EchoRequest.ReverseEcho)
        {
            this.data = data;
            this.username = username;
        }

        /// <summary>
        /// The username of the client who made the request
        /// </summary>
        public string username;

        /// <summary>
        /// The data the user requested to be reversed
        /// </summary>
        public string data;
    }
}
