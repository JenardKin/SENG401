using System;

namespace Messages.ServiceBusRequest.CompanyDirectory
{
    [Serializable]
    public class CompanyDirectoryServiceRequest : ServiceBusRequest
    {
        public CompanyDirectoryServiceRequest(CompanyDirectoryRequest requestType)
            : base(Service.CompanyDirectory)
        {
            this.requestType = requestType;
        }

        /// <summary>
        /// Indicates the type of request the client is seeking from the Company Directory Service
        /// </summary>
        public CompanyDirectoryRequest requestType;
    }

    public enum CompanyDirectoryRequest { CompanySearch, GetCompanyInfo };
}
