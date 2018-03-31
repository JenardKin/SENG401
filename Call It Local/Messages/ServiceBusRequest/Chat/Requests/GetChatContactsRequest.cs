using Messages.NServiceBus.Commands;

using NServiceBus;

using System;

namespace Messages.ServiceBusRequest.Chat.Requests
{
    [Serializable]
    public class GetChatContactsRequest : ChatServiceRequest
    {
        public GetChatContactsRequest(GetChatContacts getCommand)
            : base(ChatRequest.getChatContacts)
        {
            this.getCommand = getCommand;
        }

        /// <summary>
        /// The NServiceBus command containg information needed to get the chat contacts for the requesting client
        /// </summary>
        public GetChatContacts getCommand;
    }
}
