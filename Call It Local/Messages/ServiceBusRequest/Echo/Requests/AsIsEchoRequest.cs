using Messages.ServiceBusRequest.Echo;

using System;

namespace Messages.ServiceBusRequest.Echo.Requests
{
    [Serializable]
    public class AsIsEchoRequest : EchoServiceRequest
    {
        public AsIsEchoRequest(string data)
            : base(EchoRequest.AsIsEcho)
        {
            this.data = data;
        }
        
        /// <summary>
        /// The data the client sent to be echo'd
        /// </summary>
        public string data;
    }
}
