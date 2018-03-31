using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Chat;
using Messages.ServiceBusRequest.Chat.Requests;
using Messages.ServiceBusRequest.Chat.Responses;
using NServiceBus;

namespace AuthenticationService.Communication
{
    /// <summary>
    /// This portion of the class contains methods specifically for accessing the chat service.
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Listens for the client to secifify which task is being requested from the chat service
        /// </summary>
        /// <param name="request">Includes which task is being requested and any additional information required for the task to be executed</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse chatRequest(ChatServiceRequest request)
        {
            switch (request.requestType)
            {
                case (ChatRequest.getChatContacts):
                    return getAllChatContacts((GetChatContactsRequest)request);
                case (ChatRequest.getChatHistory):
                    return getChatHistory((GetChatHistoryRequest)request);
                case (ChatRequest.sendMessage):
                    return sendChatMessage((SendMessageRequest)request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid Request. Request received was:" + request.requestType.ToString());
            }
        }

        /// <summary>
        /// Sends the data to the chat service, and returns the response.
        /// </summary>
        /// <param name="request">The data sent by the client</param>
        /// <returns>The response from the chat service</returns>
        private GetChatContactsResponse getAllChatContacts(GetChatContactsRequest request)
        {
            if (authenticated == false)
            {
                return new GetChatContactsResponse(false, "Error: You must be logged in to use the chat service functionality.", null);
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<GetChatContactsResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends the data to the chat service, and returns the response.
        /// </summary>
        /// <param name="request">The data sent by the client</param>
        /// <returns>The response from the chat service</returns>
        private GetChatHistoryResponse getChatHistory(GetChatHistoryRequest request)
        {
            if (authenticated == false)
            {
                return new GetChatHistoryResponse(false, "Error: You must be logged in to use the chat service functionality.", null);
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<GetChatHistoryResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends the data to the chat service, and returns the response.
        /// </summary>
        /// <param name="request">The data sent by the client</param>
        /// <returns>The response from the chat service</returns>
        private ServiceBusResponse sendChatMessage(SendMessageRequest request)
        {
            if (authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the chat service functionality.");
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Chat");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
