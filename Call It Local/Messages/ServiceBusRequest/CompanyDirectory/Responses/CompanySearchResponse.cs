using Messages.DataTypes.Database.CompanyDirectory;

using System;

namespace Messages.ServiceBusRequest.CompanyDirectory.Responses
{
    [Serializable]
    public class CompanySearchResponse : ServiceBusResponse
    {
        public CompanySearchResponse(bool result, string response, CompanyList list)
            : base(result, response)
        {
            this.list = list;
        }

        /// <summary>
        /// A list of companies matching the search criteria given by the client
        /// </summary>
        public CompanyList list;
    }
}
