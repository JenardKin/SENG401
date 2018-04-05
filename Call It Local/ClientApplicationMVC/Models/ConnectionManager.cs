using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication.Requests;
using Messages.ServiceBusRequest.Echo.Requests;

using System.Collections.Generic;

namespace ClientApplicationMVC.Models
{
    /// <summary>
    /// This class is responsible for maintaining and controlling the ServiceBusConnections for each client with an open session
    /// </summary>
    public static partial class ConnectionManager
    {
        #region AuthenticationServiceMessages

        /// <summary>
        /// Sends the login information to the bus
        /// </summary>
        /// <param name="request">The request to be sent to the service bus containg the necessary information</param>
        /// <returns>The response from the bus</returns>
        public static ServiceBusResponse sendLogIn(LogInRequest request)
        {
            ServiceBusConnection newConnection = new ServiceBusConnection(request.username);
            
            ServiceBusResponse response = newConnection.sendLogIn(request);

            if (response.result == true)
            {
                addConnection(request.username, newConnection);
                Globals.setUser(request.username);
            }
            else
            {
                newConnection.close();
            }

            return response;
        }

        /// <summary>
        /// Indicates to the service bus that this client wishes to create a new account.
        /// Sends the new account info to the service bus and awaits a response indicating success or failure.
        /// </summary>
        /// <param name="msg">The CreateAccount object containing the new accounts information</param>
        /// <returns>The response from the bus</returns>
        public static ServiceBusResponse sendNewAccountInfo(CreateAccountRequest request)
        {
            ServiceBusConnection newConnection = new ServiceBusConnection(request.createCommand.username);
            ServiceBusResponse response = newConnection.sendNewAccountInfo(request);

            if (response.result == true)
            {
                addConnection(request.createCommand.username, newConnection);
                Globals.setUser(request.createCommand.username);
            }
            else
            {
                newConnection.close();
            }

            return response;
        }

        #endregion AuthenticationServiceMessages

        #region EchoServiceMessages

        /// <summary>
        /// Sends the data to be echo'd to the service bus
        /// </summary>
        /// <param name="request">The data to be echo'd</param>
        /// <returns>The response from the servicebus</returns>
        public static ServiceBusResponse echoAsIs(AsIsEchoRequest request)
        {
            ServiceBusConnection tempConnection = new ServiceBusConnection("");
            ServiceBusResponse response = tempConnection.echoAsIs(request);
            tempConnection.close();
            return response;
        }

        #endregion EchoServiceMessages
        
        /// <summary>
        /// Returns the ServiceBusConnection object associates with the given user.
        /// </summary>
        /// <param name="user">The name of the user to get the connection object for</param>
        /// <returns>The connection object if the user has been properly authenticated recently. null otherwise</returns>
        public static ServiceBusConnection getConnectionObject(string user)
        {
            ServiceBusConnection connection;
            if (connections.TryGetValue(user, out connection) == false)
            {
                return null;
            }

            if(connection.isConnected() == false)
            {
                connection.close();
                connections.Remove(user);
                return null;
            }

            return connection;
        }

        /// <summary>
        /// Adds the given connection to the list of connection with the given string as a key
        /// </summary>
        /// <param name="user">The identifier for the connection</param>
        /// <param name="connection">The ServiceBusConnection to add to the list</param>
        private static void addConnection(string user, ServiceBusConnection connection)
        {
            connections[user] = connection;
        }
    }

    /// <summary>
    /// This portion of the class contains member variables
    /// </summary>
    public static partial class ConnectionManager
    {
        /// <summary>
        /// Contains bus connection for all users who are logged in
        /// </summary>
        private static Dictionary<string, ServiceBusConnection> connections =
            new Dictionary<string, ServiceBusConnection>();

        
    }
}