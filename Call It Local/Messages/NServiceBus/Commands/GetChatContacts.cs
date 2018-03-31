using NServiceBus;

using System;
using System.Collections.Generic;

namespace Messages.NServiceBus.Commands
{
    /// <summary>
    /// Represents a request for a list of all users the given user has contacted
    /// </summary>
    [Serializable]
    public partial class GetChatContacts: ICommand
    {
        /// <summary>
        /// The name of the user to get the chat contacts for
        /// </summary>
        public string usersname { get; set; }

        /// <summary>
        /// A list representing the usernames of all users the above user has contacted
        /// </summary>
        public List<string> contactNames { get; set; } = null;
    }
}
