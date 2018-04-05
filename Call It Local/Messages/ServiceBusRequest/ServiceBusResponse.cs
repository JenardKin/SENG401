using NServiceBus;

using System;

namespace Messages.ServiceBusRequest
{
    /// <summary>
    /// This class represents a response the service bus has made to a request
    /// If you need to include more information than a string and a boolean, create
    /// a new class and extend this class and put any additional members in the child class
    /// </summary>
    [Serializable]
    public class ServiceBusResponse : IMessage
    {
        public ServiceBusResponse(bool result, string response)
        {
            this.result = result;
            this.response = response;
        }

        /// <summary>
        /// The result indicates whether or not the service bus was able to fulfill
        /// the client request. True for success, false for failure
        /// </summary>
        public bool result = false;

        /// <summary>
        /// The response indicates some information about the request, usually failure data
        /// </summary>
        public string response = "";
    }
}
