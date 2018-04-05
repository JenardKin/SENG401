using Messages;
using Messages.DataTypes;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication;
using Messages.ServiceBusRequest.Echo;
using Messages.ServiceBusRequest.CompanyDirectory;
using Messages.ServiceBusRequest.Chat;

using NServiceBus;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace AuthenticationService.Communication
{

    /// <summary>
    /// This portion of the class contains the nonstatic function definitions
    /// 
    /// This class is used to communicate with clients that wish to use the services the bus has
    /// 
    /// This particular file contains members and methods relevant to all services.
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Constructor. Sets member variables and Sets up the secure connection stream from the socket
        /// </summary>
        /// <param name="connection">The socket which is connected to the client</param>
        /// <param name="eventPublishingEndpoint">This is needed because the other services which subscribe to authentication endpoint events need an existing endpoint to subscribe too and cannot subscribe to user specific endpoint instances</param>
        /// <param name="certificate">The certificate object used for secure communication</param>
        public ClientConnection(Socket connection, IEndpointInstance eventPublishingEndpoint, X509Certificate2 certificate)
        {
            this.connection = connection;
            this.eventPublishingEndpoint = eventPublishingEndpoint;
            this.certificate = certificate;
            this.connectionStream = new SslStream(new NetworkStream(connection, false));
            this.connectionStream.AuthenticateAsServer(certificate, //Indicate that this stream represents the server
                   false, SslProtocols.Tls, true);
        }

        /// <summary>
        /// listens for message requests/commands for as long as the connection remains open.
        /// </summary>
        public void listenToClient()
        {
            while (connection.Connected == true)
            {
                //Read the call to the API
                ServiceBusRequest request = readUntilEOF();

                ServiceBusResponse responseMessage = executeRequest(request);

                sendToClient(responseMessage);

                if (authenticated == false)
                {
                    terminateConnection();
                }
            }

            Debug.consoleMsg("Client connection closing...");

            return;
        }

        /// <summary>
        /// Executes the request received from the server
        /// </summary>
        /// <param name="requestParameters">Information about the request</param>
        /// <returns>A string representing the result of the request</returns>
        private ServiceBusResponse executeRequest(ServiceBusRequest request)
        {
            switch (request.serviceRequested)
            {
                case (Service.Authentication):
                    return authenticationRequest((AuthenticationServiceRequest)request);
                case (Service.Echo):
                    return echoRequest((EchoServiceRequest)request);
                case (Service.CompanyDirectory):
                    return companyDirectoryRequest((CompanyDirectoryServiceRequest)request);
                case (Service.Chat):
                    return chatRequest((ChatServiceRequest)request);

                default:
                    return new ServiceBusResponse(false, "Error: Invalid request. Did not specify a valid service type. Specified type was: " + request.serviceRequested.ToString());
            }
        }

        /// <summary>
        /// Continuously reads data from the client until the end of file string of characters is found
        /// The end of file string is defined in the Messages library shared by the web server and the bus.
        /// </summary>
        /// <returns>The string representation of bytes read from the client socket</returns>
        private ServiceBusRequest readUntilEOF()
        {
            int sizeOfMsg = 0;
            int bytesRead = 0;
            byte[] msgSize = new byte[4];

            while(bytesRead < msgSize.Length)
            {
                try
                {
                    bytesRead += connectionStream.Read(msgSize, bytesRead, msgSize.Length - bytesRead);
                }
                catch (SocketException)// This is thrown when the timeout occurs. The timeout is set in the constructor
                {
                    Thread.Yield();// Yield this threads remaining timeslice to another process, this process does not appear to need it currently because the read timed out
                }
            }

            //First we will receive the size of the message
            sizeOfMsg = BitConverter.ToInt32(msgSize, 0);

            byte[] requestBytes = new byte[sizeOfMsg];
            ServiceBusRequest request = null;
            bytesRead = 0;

            while (bytesRead < requestBytes.Length)
            {
                try
                {
                    bytesRead += connectionStream.Read(requestBytes, bytesRead, requestBytes.Length - bytesRead);
                }
                catch (SocketException)// This is thrown when the timeout occurs. The timeout is set in the constructor
                {
                    Thread.Yield();// Yield this threads remaining timeslice to another process, this process does not appear to need it currently because the read timed out
                }
            }

            MemoryStream memStream = new MemoryStream(requestBytes);
            BinaryFormatter binForm = new BinaryFormatter();

            request = (ServiceBusRequest)binForm.Deserialize(memStream);

            return request;
        }

        /// <summary>
        /// Sends the given message to the client along with the msgEndDelim on the end
        /// </summary>
        /// <param name="msg">The message to send to the client</param>
        private void sendToClient(ServiceBusResponse message)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            binForm.Serialize(memStream, message);
            byte[] msg = memStream.ToArray();

            int msgSize = msg.Length;

            //First write the total size of the message 
            connectionStream.Write(BitConverter.GetBytes(msgSize));
            connectionStream.Flush();

            //Then write the serialized object
            connectionStream.Write(msg);
            connectionStream.Flush();
        }

        /// <summary>
        /// Closes the connection with the client
        /// </summary>
        private void terminateConnection()
        {
            closeRequestEndpoint();
            connection.Disconnect(false);
        }

        /// <summary>
        /// Closes the requesting endpoint and sets its reference to null, if it is not null already
        /// </summary>
        private void closeRequestEndpoint()
        {
            if (requestingEndpoint != null)
            {
                requestingEndpoint.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
                requestingEndpoint = null;
            }
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// The endpoint, used to communicate with other services in the bus.
        /// All clients use the same endpoint
        /// </summary>
        private IEndpointInstance requestingEndpoint = null;

        /// <summary>
        /// This endpoint is used to raise events.
        /// This second endpoint is used by all connections, and exists because other services need to attach
        /// to a single endpoint instance upon creation, and cannot continuously reattach to every clientconnection
        /// endpoint as they are created.
        /// </summary>
        private IEndpointInstance eventPublishingEndpoint;

        /// <summary>
        /// The connection between the client and the server.
        /// </summary>
        private Socket connection;

        /// <summary>
        /// This is the stream used to read and write to the socket securely
        /// </summary>
        private SslStream connectionStream = null;

        /// <summary>
        /// This is the timeout used by the socket as it waits for a message from the client
        /// </summary>
        private const int timeout_ms = 50;

        /// <summary>
        /// This is used to authenticate the connection between the client(web server) and the bus.
        /// </summary>
        private X509Certificate2 certificate = null;
    }

    /// <summary>
    /// This portion of the class contains the static members and methods
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Returns an endpoint configuration object to be used to craete a new endpoint instance
        /// This function is required because each endpoint configuration can only be associated with a single endpoint instance, and so a new one must be created for each connection
        /// </summary>
        /// <param name="addressableName">The uniquely addressable ID of the endpoint</param>
        /// <returns>Endpoint Configuration Object with relevant settings for use with this server</returns>
        private static EndpointConfiguration getConfig(string addressableName)
        {
            //Create a new Endpoint configuration with the name "Authentication"
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Authentication");

            //These two lines prevemt the endpoint configuration from scanning the MySql dll. 
            //This is donw because it speeds up the startup time, and it prevents a rare but 
            //very confusing error sometimes caused by NServiceBus scanning the file. If you 
            //wish to know morw about this, google it, then ask your TA(since they will probably
            //just google it anyway)
            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("MySql.Data.dll");


            //Allows the endpoint to run installers upon startup. This includes things such as the creation of message queues.
            endpointConfiguration.EnableInstallers();
            //Instructs the queue to serialize messages with Json, should it need to serialize them
            endpointConfiguration.UseSerialization<JsonSerializer>();
            //Instructs the endpoint to use local RAM to store queues. TODO: Good during development, not during deployment (According to the NServiceBus tutorial)
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            //Instructs the endpoint to send messages it cannot process to a queue named "error"
            endpointConfiguration.SendFailedMessagesTo("error");
            //Allows the endpoint to make requests to other endpoints and await a response.
            endpointConfiguration.EnableCallbacks();

            //Instructs the endpoint to use Microsoft Message Queuing TOD): Consider using RabbitMQ instead, only because Arcurve reccomended it. 
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            //This variable is used to configure how messages are routed. Using this, you may set the default reciever of a particular command, and/or subscribe to any number of events
            var routing = transport.Routing();

            endpointConfiguration.MakeInstanceUniquelyAddressable(addressableName);
            
            return endpointConfiguration;
        }
    }
}
