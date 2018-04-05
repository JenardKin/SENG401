using AuthenticationService.Communication;

using NServiceBus;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace AuthenticationService
{
    /// <summary>
    /// This class is responsible for listening for new client connections, configuring and starting ClientConnection threads 
    /// </summary>
    public partial class Server
    {
        /// <summary>
        /// Server constructor
        /// </summary>
        /// <param name="eventPublisher">The endpoint to be used by client connections to publish events</param>
        public Server(IEndpointInstance eventPublisher)
        {
            this.eventPublisher = eventPublisher;
            certificate = new X509Certificate2(certificateLocation, "");
        }

        /// <summary>
        /// Listen for incoming connections for as long as they continue to arrive until an exception is thrown
        /// </summary>
        public void StartListening()
        {
            // Establish the local endpoint for the socket.   
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IPv6 socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    connectionAttemptRecieved.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Messages.Debug.consoleMsg("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptConnection),
                        listener);

                    // Wait until a connection is made before continuing.  
                    connectionAttemptRecieved.WaitOne();
                }

            }
            catch (Exception e)
            {
                Messages.Debug.consoleMsg(e.ToString());
            }

        }

        /// <summary>
        /// Accepts the new connection and creates a new running thread to handle communication with the new connection
        /// </summary>
        /// <param name="ar">Contains the state object given during the "BeginAccept" call.</param>
        public void AcceptConnection(IAsyncResult ar)
        {

            // Get the socket that handles the client request.  
            Socket serverSocket = (Socket)ar.AsyncState;
            Socket specificClientSocket = serverSocket.EndAccept(ar);

            // Signal the main thread to continue listening for more connection attempts.  
            connectionAttemptRecieved.Set();

            ClientConnection connection = new ClientConnection(specificClientSocket, eventPublisher, getCertificate());

            Thread newThread = new Thread(new ThreadStart(connection.listenToClient));
            newThread.Start();
        }
        
        /// <summary>
        /// Gets the certificate used to encrypt/decrypt messages and authenticate clients
        /// </summary>
        /// <returns></returns>
        private X509Certificate2 getCertificate()
        {
            return certificate;
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables
    /// </summary>
    public partial class Server
    {
        private static X509Certificate2 certificate;

        /// <summary>
        /// Semaphore, used to indicate when a client connection has been recieved
        /// </summary>
        private ManualResetEvent connectionAttemptRecieved = new ManualResetEvent(false);

        /// <summary>
        /// Endpoint used by all clients to publish events
        /// </summary>
        private IEndpointInstance eventPublisher;

        /// <summary>
        /// The location of the certificate on the machine.
        /// The server will not be able to run properly unless this variable is properly set.
        /// </summary>
        private const string certificateLocation = "C:\\Users\\nardjay1997\\Desktop\\SENG401TenYears.pfx";
        //private const string certificateLocation ="C:\\Users\\Nate\\Desktop\\ProjectSkeleton\\ProjectSkeleton\\Certificate\\SENG401TenYears.pfx";
        //private const string certificateLocation = "C:\\Users\\Nathan\\Desktop\\SENG401TenYears.pfx";
    }
}
