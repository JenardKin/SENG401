using Messages.NServiceBus.Commands;
using Messages.NServiceBus.Events;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Echo;
using Messages.ServiceBusRequest.Echo.Requests;

using NServiceBus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Communication
{
    /// <summary>
    /// This portion of the class contains methods specifically for accessing the echo service.
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Listens for the client to secifify which task is being requested from the echo service
        /// </summary>
        /// <param name="request">Includes which task is being requested and any additional information required for the task to be executed</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse echoRequest(EchoServiceRequest request)
        {
            switch (request.requestType)
            {
                case (EchoRequest.AsIsEcho):
                    return asIsEcho((AsIsEchoRequest)request);
                case (EchoRequest.ReverseEcho):
                    return reverseEcho((ReverseEchoRequest)request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid Request. Request received was:" + request.requestType.ToString());
            }
        }

        /// <summary>
        /// Publishes an EchoEvent.
        /// </summary>
        /// <param name="request">The data to be echo'd back to the client</param>
        /// <returns>The data sent by the client</returns>
        private ServiceBusResponse asIsEcho(AsIsEchoRequest request)
        {
            AsIsEchoEvent echoEvent = new AsIsEchoEvent
            {
                data = request.data,
                username = username
            };

            //This function publishes the EchoEvent class. All endpoint instances that subscribed to these events prior
            //to the event being published will have their respictive handler functions called with the EchoEvent object
            //as one of the parameters
            eventPublishingEndpoint.Publish(echoEvent);
            return new ServiceBusResponse(true, request.data);
        }

        /// <summary>
        /// Sends the data to the echo service, and returns the response.
        /// </summary>
        /// <param name="request">The data sent by the client</param>
        /// <returns>The response from the echo service</returns>
        private ServiceBusResponse reverseEcho(ReverseEchoRequest request)
        {
            if(authenticated == false)
            {
                return new ServiceBusResponse(false, "Error: You must be logged in to use the echo reverse functionality.");
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Echo");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<ServiceBusResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
