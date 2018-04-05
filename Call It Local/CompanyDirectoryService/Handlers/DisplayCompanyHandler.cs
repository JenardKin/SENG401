using CompanyDirectoryService.Database;

using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.Echo.Requests;

using NServiceBus;
using NServiceBus.Logging;

using System;
using System.Threading.Tasks;

namespace CompanyDirectoryService.Handlers
{
    
    /// <summary>
    /// This is the handler class for the company display request
    /// This class is created and its methods called by the NServiceBus framework
    /// </summary>
    public class DisplayCompanyHandler : IHandleMessages<GetCompanyInfoRequest>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<DisplayCompanyHandler>();

        /// <summary>
        /// Searches the db, and returns it's result back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public Task Handle(GetCompanyInfoRequest request, IMessageHandlerContext context)
        {
            GetCompanyInfoResponse response = CompanyDirectoryServiceDatabase.getInstance().getCompanyInfo(request.companyInfo.companyName);

            //The context is used to give a reply back to the endpoint that sent the request
            return context.Reply(response);
        }
    }
}
