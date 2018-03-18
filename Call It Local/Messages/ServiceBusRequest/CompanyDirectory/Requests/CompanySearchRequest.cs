using NServiceBus;

using System;

namespace Messages.ServiceBusRequest.CompanyDirectory.Requests
{
    [Serializable]
    public class CompanySearchRequest : CompanyDirectoryServiceRequest
    {
        public CompanySearchRequest(string searchDeliminator)
            : base(CompanyDirectoryRequest.CompanySearch)
        {
            this.searchDeliminator = searchDeliminator;
        }

        /// <summary>
        /// Information used to search the database for companies
        /// </summary>
        public string searchDeliminator;
    }
}
