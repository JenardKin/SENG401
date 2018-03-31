using NServiceBus;

using System;

namespace Messages.ServiceBusRequest.Chat
{
    [Serializable]
    public class ChatServiceRequest : ServiceBusRequest, IMessage
    {
        public ChatServiceRequest(ChatRequest requestType)
            : base(Service.Chat)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the Chat Service
        /// </summary>
        public ChatRequest requestType;
    }

    public enum ChatRequest { sendMessage, getChatContacts, getChatHistory };
}
