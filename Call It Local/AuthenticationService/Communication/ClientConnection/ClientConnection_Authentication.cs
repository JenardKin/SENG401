using AuthenticationService.Database;

using Messages;
using Messages.NServiceBus.Commands;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication;
using Messages.ServiceBusRequest.Authentication.Requests;

using NServiceBus;

using System;
using System.Collections.Generic;
using System.Net;

namespace AuthenticationService.Communication
{
    /// <summary>
    /// This portion of the class contains methods specifially for accessing the authentication service
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Listens for the client to specify which task it is requesting from the authentication service
        /// </summary>
        /// <param name="request">Contains Information regarding the task being requested</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse authenticationRequest(AuthenticationServiceRequest request)
        {
            switch (request.requestType)
            {
                case (AuthenticationRequest.LogIn):
                    return attemptToAuthenticateUser((LogInRequest)request);
                case (AuthenticationRequest.CreateAccount):
                    return attemptNewAccountCreation((CreateAccountRequest)request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid request. Request received was:" + request.requestType.ToString());
            }
        }

        /// <summary>
        /// Parses new account info and attempts to insert the new account into the authentication database
        /// It also publishes an "AccountCreated" event
        /// </summary>
        /// <returns>A response message</returns>
        private ServiceBusResponse attemptNewAccountCreation(CreateAccountRequest request)
        {
            CreateAccount command = request.createCommand;

            ServiceBusResponse dbResponse = AuthenticationDatabase.getInstance().insertNewUserAccount(command);

            if (dbResponse.result == true)
            {
                authenticated = true;
                username = command.username;
                password = command.password;
                initializeRequestingEndpoint();
                eventPublishingEndpoint.Publish(new AccountCreated(command));
            }
            return dbResponse;
        }
        
        /// <summary>
        /// Parses username and password from info and checks the database for validity of the information sent.
        /// If invalid, closes the connection after sending the response. 
        /// If successful will keep the connection open
        /// </summary>
        /// <returns>A response message indicating the result of the attempt</returns>
        private ServiceBusResponse attemptToAuthenticateUser(LogInRequest request)
        {
            this.username = request.username;
            this.password = request.password;

            if ("".Equals(username) || "".Equals(password))
            {
                terminateConnection();
                return new ServiceBusResponse(false, "Failure. Username or password not sent properly");
            }

            ServiceBusResponse dbResponse = AuthenticationDatabase.getInstance().isValidUserInfo(username, password);
            authenticated = dbResponse.result;

            reportLogInAttempt();
            return dbResponse;
        }

        /// <summary>
        /// Publishes a ClientLogInAttempted event through the publishing endpoint
        /// </summary>
        private void reportLogInAttempt()
        {
            ClientLogInAttempted attempt = new ClientLogInAttempted
            {
                username = this.username,
                password = this.password,
                clientAccepted = this.authenticated,
                requestSource = ((IPEndPoint)connection.RemoteEndPoint).Serialize()
            };

            //Publish the log in attempt event for any other EP that wishes to know about it.
            //If an endpoint wishes to be notified about this event, it should subscribe to the event in its configuration
            Debug.consoleMsg("Log in attempted with credentials:" + "\n" +
                "Username:" + username + "\n" +
                "Password:" + password + "\n");

            if (authenticated == true)
            {
                initializeRequestingEndpoint();
            }
            eventPublishingEndpoint.Publish(attempt);
        }

        /// <summary>
        /// Starts the endoint that will be linked to this specific client connection
        /// </summary>
        private void initializeRequestingEndpoint()
        {
            EndpointConfiguration config = getConfig(username);
            requestingEndpoint = Endpoint.Start(config).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables
    /// </summary>
    partial class ClientConnection
    {

        /// <summary>
        /// The username given by the client
        /// </summary>
        private string username = "";

        /// <summary>
        /// the password given by the client
        /// </summary>
        private string password = "";

        /// <summary>
        /// Indicates whether or not the username and password given by the client are valid
        /// </summary>
        private bool authenticated = false;
    }
}
