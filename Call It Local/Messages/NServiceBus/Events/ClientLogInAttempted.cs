using NServiceBus;

using System.Net;

namespace Messages.NServiceBus.Events
{
    /// <summary>
    /// This event contains information about a client that has attempted to connect to the bus
    /// It contains the username and password used, whether or not the pair was valid,
    /// and the Address the request came from.
    /// This event is published by the authentication service every time a client attempts to log in
    /// </summary>
    public class ClientLogInAttempted : IEvent { 
        /// <summary>
        /// Username used in login attempt
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Password used in login attempt
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// The result of the login attempt
        /// </summary>
        public bool clientAccepted { get; set; }

        /// <summary>
        /// This represents the source of the login attempt
        /// </summary>
        public SocketAddress requestSource { get; set; }
    }
}
