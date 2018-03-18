using Messages.DataTypes.Database.CompanyDirectory;

using System;

namespace Messages.ServiceBusRequest.CompanyDirectory.Requests
{
    [Serializable]
    public class GetCompanyInfoRequest : CompanyDirectoryServiceRequest
    {
        public GetCompanyInfoRequest(CompanyInstance companyInfo)
            : base(CompanyDirectoryRequest.GetCompanyInfo)
        {
            this.companyInfo = companyInfo;
        }

        /// <summary>
        /// Contains information needed to locate additional information about the company the client is requesting
        /// </summary>
        public CompanyInstance companyInfo;
    }
}
