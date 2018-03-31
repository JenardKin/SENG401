using Messages.NServiceBus.Commands;

using NServiceBus;

using System;

namespace Messages.ServiceBusRequest.Chat.Requests
{
    [Serializable]
    public class GetChatHistoryRequest : ChatServiceRequest, IMessage
    {
        public GetChatHistoryRequest(GetChatHistory getCommand)
            : base(ChatRequest.getChatHistory)
        {
            this.getCommand = getCommand;
        }

        /// <summary>
        /// The NServiceBus command containg the information needed to get the chat history requested by the client
        /// </summary>
        public GetChatHistory getCommand;
    }
}
