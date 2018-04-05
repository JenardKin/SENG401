using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using NServiceBus;

namespace AuthenticationService.Communication
{
    /// <summary>
    /// This portion of the class contains methods specifically for accessing the companyDirectory service.
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Listens for the client to secifify which task is being requested from the company service
        /// </summary>
        /// <param name="request">Includes which task is being requested and any additional information required for the task to be executed</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse companyDirectoryRequest(CompanyDirectoryServiceRequest request)
        {
            switch (request.requestType)
            {
                case (CompanyDirectoryRequest.CompanySearch):
                    return searchCompanyByName((CompanySearchRequest)request);
                case (CompanyDirectoryRequest.GetCompanyInfo):
                    return getCompanyInfo((GetCompanyInfoRequest)request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid Request. Request received was:" + request.requestType.ToString());
            }
        }

        /// <summary>
        /// Sends the data to the company directory service, and returns the response.
        /// </summary>
        /// <param name="request">The data sent by the client</param>
        /// <returns>The response from the company directory service</returns>
        private CompanySearchResponse searchCompanyByName(CompanySearchRequest request)
        {
            if (authenticated == false)
            {
                return new CompanySearchResponse(false, "Error: You must be logged in to use the company directory functionality.", null);
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("CompanyDirectory");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<CompanySearchResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sends the data to the company directory service, and returns the response.
        /// </summary>
        /// <param name="request">The data sent by the client</param>
        /// <returns>The response from the company directory service</returns>
        private GetCompanyInfoResponse getCompanyInfo(GetCompanyInfoRequest request)
        {
            if (authenticated == false)
            {
                return new GetCompanyInfoResponse(false, "Error: You must be logged in to use the echo reverse functionality.", null);
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("CompanyDirectory");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<GetCompanyInfoResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
