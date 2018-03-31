using NServiceBus;

using Messages.DataTypes.Database.Chat;

using System;

namespace Messages.NServiceBus.Commands
{
    /// <summary>
    /// This class represents a request for the chat history between two users
    /// </summary>
    [Serializable]
    public class GetChatHistory : IMessage
    {
        /// <summary>
        /// The chat history between these two users.
        /// </summary>
        public ChatHistory history { get; set; }
    }
}
