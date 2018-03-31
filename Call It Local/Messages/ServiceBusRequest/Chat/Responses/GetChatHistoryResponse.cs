using Messages.NServiceBus.Commands;

using System;

namespace Messages.ServiceBusRequest.Chat.Responses
{
    [Serializable]
    public class GetChatHistoryResponse : ServiceBusResponse
    {
        public GetChatHistoryResponse(bool result, string response, GetChatHistory responseData)
            : base(result, response)
        {
            this.responseData = responseData;
        }

        /// <summary>
        /// The NServiceBus command that contains the chat history requested by the client
        /// </summary>
        public GetChatHistory responseData;
    }
}
