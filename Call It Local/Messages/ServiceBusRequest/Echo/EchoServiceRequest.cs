using System;

namespace Messages.ServiceBusRequest.Echo
{
    [Serializable]
    public class EchoServiceRequest : ServiceBusRequest
    {
        public EchoServiceRequest(EchoRequest requestType)
            : base(Service.Echo)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the echo service
        /// </summary>
        public EchoRequest requestType;
    }

    public enum EchoRequest { AsIsEcho, ReverseEcho };
}
