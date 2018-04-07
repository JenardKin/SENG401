using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Weather;
using Messages.ServiceBusRequest.Weather.Requests;
using Messages.ServiceBusRequest.Weather.Responses;
using NServiceBus;

namespace AuthenticationService.Communication
{
    /// <summary>
    /// This portion of the class contains methods specifically for accessing the weather service.
    /// </summary>
    partial class ClientConnection
    {
        /// <summary>
        /// Listens for the client to secifify which task is being requested from the company service
        /// </summary>
        /// <param name="request">Includes which task is being requested and any additional information required for the task to be executed</param>
        /// <returns>A response message</returns>
        private ServiceBusResponse weatherRequest(WeatherServiceRequest request)
        {
            switch (request.requestType)
            {
                case (WeatherRequest.getWeather):
                    return getWeather((GetWeatherRequest)request);
                default:
                    return new ServiceBusResponse(false, "Error: Invalid Request. Request received was:" + request.requestType.ToString());
            }
        }

        /// <summary>
        /// Sends the data to the weather service, and returns the response.
        /// </summary>
        /// <param name="request">The data sent by the client</param>
        /// <returns>The response from the company directory service</returns>
        private GetWeatherResponse getWeather(GetWeatherRequest request)
        {
            if (authenticated == false)
            {
                return new GetWeatherResponse(false, "Error: You must be logged in to use the weather functionality.", null);
            }

            // This class indicates to the request function where 
            SendOptions sendOptions = new SendOptions();
            sendOptions.SetDestination("Weather");

            // The Request<> funtion itself is an asynchronous operation. However, since we do not want to continue execution until the Request
            // function runs to completion, we call the ConfigureAwait, GetAwaiter, and GetResult functions to ensure that this thread
            // will wait for the completion of Request before continueing. 
            return requestingEndpoint.Request<GetWeatherResponse>(request, sendOptions).
                ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}

