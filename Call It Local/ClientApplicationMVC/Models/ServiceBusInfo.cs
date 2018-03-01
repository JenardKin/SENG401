using System.Net;

namespace ClientApplicationMVC.Models
{
    /// <summary>
    /// This class contains information needed to connect to the service bus
    /// </summary>
    public static class ServiceBusInfo
    {
        /// <summary>
        /// The port to use when initializing a socket connection with the service bus
        /// </summary>
        public const int serverPort = 11000;

        /// <summary>
        /// The hostname of the service bus
        /// </summary>
        public const string serverHostName = "127.0.0.1";

        /// <summary>
        /// The IPAddress object of the servicebus
        /// </summary>
        public static readonly IPAddress ipAddress = IPAddress.Parse(serverHostName);
    }
}