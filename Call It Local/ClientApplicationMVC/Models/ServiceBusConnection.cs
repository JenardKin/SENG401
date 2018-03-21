using Messages.NServiceBus.Commands;
using Messages.DataTypes;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication.Requests;
using Messages.ServiceBusRequest.Echo.Requests;

using System;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;

namespace ClientApplicationMVC.Models
{
    /// <summary>
    /// This class is responsible for sending and receiving messages between the service bus and the web server in a secure manner.
    /// </summary>
    public partial class ServiceBusConnection
    {
        public ServiceBusConnection(string username)
        {
            this.username = username;
        }

        #region ServiceBusMessages

        #region AuthenticationServiceMessages


        /// <summary>
        /// Sends the login information to the bus and attempts to log in
        /// </summary>
        /// <param name="request">The login information</param>
        /// <returns>The response from the bus</returns>
        public ServiceBusResponse sendLogIn(LogInRequest request)
        {
            send(request);
            return readUntilEOF();
        }

        /// <summary>
        /// Indicates to the service bus that this client wishes to create a new account.
        /// Sends the new account info to the service bus and awaits a response indicating success or failure.
        /// </summary>
        /// <param name="request">The CreateAccountRequest object containing the new accounts information</param>
        /// <returns>The response from the bus</returns>
        public ServiceBusResponse sendNewAccountInfo(CreateAccountRequest request)
        {
            send(request);
            return readUntilEOF();
        }


        #endregion AuthenticationServiceMessages

       
        #region EchoServiceMessages


        /// <summary>
        /// Sends the data to be echo'd to the service bus
        /// </summary>
        /// <param name="request">The data to be echo'd</param>
        /// <returns>The response from the servicebus</returns>
        public ServiceBusResponse echoAsIs(AsIsEchoRequest request)
        {
            send(request);
            return readUntilEOF();
        }

        /// <summary>
        /// Sends the data to be echo'd to the service bus
        /// </summary>
        /// <param name="request">The data to be echo'd</param>
        /// <returns>The response from the servicebus</returns>
        public ServiceBusResponse echoReverse(ReverseEchoRequest request)
        {
            send(request);
            return readUntilEOF();
        }

        #endregion EchoServiceMessages

        #region CompanyDirectoryServiceMessages


        /// <summary>
        /// Sends the data to be echo'd to the service bus
        /// </summary>
        /// <param name="request">The data to be echo'd</param>
        /// <returns>The response from the servicebus</returns>
        public ServiceBusResponse searchCompanyByName(CompanySearchRequest request)
        {
            //???
            send(request);
            return readUntilEOF();
            //return (CompanySearchResponse)readUntilEOF();
        }

        /// <summary>
        /// Sends the data to be echo'd to the service bus
        /// </summary>
        /// <param name="request">The data to be echo'd</param>
        /// <returns>The response from the servicebus</returns>
        public GetCompanyInfoResponse getCompanyInfo(GetCompanyInfoRequest request)
        {
            send(request);
            return (GetCompanyInfoResponse)readUntilEOF();
        }

        #endregion CompanyDirectoryServiceMessages

        #endregion ServiceBusMessages

        #region ConnectionFunctions

        /// <summary>
        /// Indicates if this object is still connected to the service bus
        /// </summary>
        /// <returns>True if connected, false if not</returns>
        public bool isConnected()
        {
            return connection.Connected;
        }

        /// <summary>
        /// Closes the connection with the service bus.
        /// </summary>
        public void close()
        {
            connectionStream.Close();
            connection.Close();
        }

        /// <summary>
        /// Sends the sppecified message through the socket
        /// Attaches the msgEndDelim to the end of the message to indicate the end of the string
        /// </summary>
        /// <param name="message">The message to be sent</param>
        private void send(ServiceBusRequest message)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            binForm.Serialize(memStream, message);
			
            byte[] msg = memStream.ToArray();

            int msgSize = msg.Length;

            while (!connection.Connected)
            {
                connect();
            }

            //First write the total size of the message 

            byte[] msgSizeBytes = BitConverter.GetBytes(msgSize);
            connectionStream.Write(msgSizeBytes);
            connectionStream.Flush();

            connectionStream.Write(msg);
            connectionStream.Flush();
        }

        /// <summary>
        /// Attempts to connect to the service Bus through the socket, 
        /// then attempts to open an SslStream using the socket,
        /// then attempts to validate the connection with the server
        /// </summary>
        private void connect()
        {
            connection.Connect(ServiceBusInfo.serverHostName, ServiceBusInfo.serverPort);

            connectionStream = new SslStream(
                new NetworkStream(connection),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null);

            connectionStream.AuthenticateAsClient("localhost");
        }

        /// <summary>
        /// Continuously reads one byte at a time from the client until the end of file string of characters defined in the Messages library is found
        /// </summary>
        /// <returns>The string representation of bytes read from the server socket</returns>
        private ServiceBusResponse readUntilEOF()
        {
            int sizeOfMsg = 0;
            int bytesRead = 0;
            byte[] msgSize = new byte[4];

            while (bytesRead < msgSize.Length)
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
            ServiceBusResponse request = null;
            bytesRead = 0;

            while (bytesRead < requestBytes.Length)
            {
                try
                {
                    bytesRead += connectionStream.Read(requestBytes, bytesRead, requestBytes.Length - bytesRead);
                }
                catch (SocketException)// This is thrown when the timeout occurs.
                {
                    Thread.Yield();// Yield this threads remaining timeslice to another process, this process does not appear to need it currently because the read timed out
                }
            }

            MemoryStream memStream = new MemoryStream(requestBytes);
            BinaryFormatter binForm = new BinaryFormatter();

            request = (ServiceBusResponse)binForm.Deserialize(memStream);

            return request;
        }

        /// <summary>
        /// Disconnects the socket from the server. The socket will be able to reconnect later.
        /// </summary>
        private void terminateConnection()
        {
            connection.Disconnect(true);
        }

        /// <summary>
        /// Ensures that the response is from a validated service bus.
        /// </summary>
        /// <returns>True if the server can be validated. False otherwise.</returns>
        private bool ValidateServerCertificate(
            object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            return false;
        }

        #endregion ConnectionFunctions
    }

    /// <summary>
    /// This portion of the class contains the member variables
    /// </summary>
    partial class ServiceBusConnection
    {
        /// <summary>
        /// This is the socket that connects the application to the database
        /// </summary>
        private Socket connection = new Socket(ServiceBusInfo.ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        /// <summary>
        /// The stream used to communicate through securely
        /// </summary>
        private SslStream connectionStream;

        /// <summary>
        /// Semaphore in charge of making sure only one thread accesses the socket at a time
        /// </summary>
        private Semaphore _lock = new Semaphore(0, 1);

        /// <summary>
        /// The name of the user associates with this connection object
        /// </summary>
        private string username;

        /// <summary>
        /// The number of milliseconds the ServiceBusConnection should wait for a response from the server before yielding its remaining timeslice
        /// </summary>
        private const int readTimeout_ms = 50;
    }
}