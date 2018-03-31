using Messages.DataTypes.Database.Chat;

using System;

namespace Messages.ServiceBusRequest.Chat.Requests
{
    [Serializable]
    public class SendMessageRequest : ChatServiceRequest
    {
        public SendMessageRequest(ChatMessage message)
            : base(ChatRequest.sendMessage)
        {
            this.message = message;
        }

        /// <summary>
        /// The chat message the user wants to send
        /// </summary>
        public ChatMessage message;
    }
}
