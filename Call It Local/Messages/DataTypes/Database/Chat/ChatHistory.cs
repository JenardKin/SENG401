using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.DataTypes.Database.Chat
{
    /// <summary>
    /// This class represents all messages sent between two users since the accounts creation
    /// </summary>
    [Serializable]
    public partial class ChatHistory
    {
        /// <summary>
        /// A list of all the messages sent between the two users.
        /// </summary>
        public List<ChatMessage> messages { get; set; } = new List<ChatMessage>(0);

        /// <summary>
        /// The username of the client 
        /// </summary>
        public string user1 { get; set; }

        /// <summary>
        /// The username of the company
        /// </summary>
        public string user2 { get; set; }
    }
}
