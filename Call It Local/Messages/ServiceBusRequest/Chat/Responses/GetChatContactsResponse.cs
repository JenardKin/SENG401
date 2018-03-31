using Messages.NServiceBus.Commands;

using System;

namespace Messages.ServiceBusRequest.Chat.Responses
{
    [Serializable]
    public class GetChatContactsResponse : ServiceBusResponse
    {
        public GetChatContactsResponse(bool result, string response, GetChatContacts responseData)
            : base(result, response)
        {
            this.responseData = responseData;
        }

        /// <summary>
        /// Contains a list of chat contacts requested by the client
        /// </summary>
        public GetChatContacts responseData;
    }
}
