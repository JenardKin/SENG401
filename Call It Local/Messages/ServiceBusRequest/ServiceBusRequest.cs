using NServiceBus;

using System;
using System.Runtime.Serialization;

namespace Messages.ServiceBusRequest
{
    [Serializable]
    public abstract partial class ServiceBusRequest : IMessage
    {
        public ServiceBusRequest(Service serviceRequested)
        {
            this.serviceRequested = serviceRequested;
        }

        /// <summary>
        /// Indicates which service is being requested by the client
        /// </summary>
        public Service serviceRequested;
    }

    public enum Service { Authentication, Echo };
}
