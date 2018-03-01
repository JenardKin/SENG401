using EchoService.Database;

using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Echo.Requests;

using NServiceBus;
using NServiceBus.Logging;

using System;
using System.Threading.Tasks;

namespace EchoService.Handlers
{
    
    /// <summary>
    /// This is the handler class for the reverse echo. 
    /// This class is created and its methods called by the NServiceBus framework
    /// </summary>
    public class ReverseEchoHandler : IHandleMessages<ReverseEchoRequest>
    {
        /// <summary>
        /// This is a class provided by NServiceBus. Its main purpose is to be use log.Info() instead of Messages.Debug.consoleMsg().
        /// When log.Info() is called, it will write to the console as well as to a log file managed by NServiceBus
        /// </summary>
        /// It is important that all logger member variables be static, because NServiceBus tutorials warn that GetLogger<>()
        /// is an expensive call, and there is no need to instantiate a new logger every time a handler is created.
        static ILog log = LogManager.GetLogger<ReverseEchoRequest>();

        /// <summary>
        /// Saves the echo to the database, reverses the data, and returns it back to the calling endpoint.
        /// </summary>
        /// <param name="message">Information about the echo</param>
        /// <param name="context">Used to access information regarding the endpoints used for this handle</param>
        /// <returns>The response to be sent back to the calling process</returns>
        public Task Handle(ReverseEchoRequest message, IMessageHandlerContext context)
        {
            //Save the echo to the database
            EchoServiceDatabase.getInstance().saveReverseEcho(message);

            //Reverse the string
            char[] charArray = message.data.ToCharArray();
            Array.Reverse(charArray);
            
            //The context is used to give a reply back to the endpoint that sent the request
            return context.Reply(new ServiceBusResponse(true, new string(charArray)));
        }
    }
}
