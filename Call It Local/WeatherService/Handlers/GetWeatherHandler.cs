using WeatherService.AccuweatherAPI;

using Messages.ServiceBusRequest.Weather.Requests;
using Messages.ServiceBusRequest.Weather.Responses;

using NServiceBus;
using NServiceBus.Logging;

using System;
using System.Threading.Tasks;

namespace WeatherService.Handlers
{
    public class GetWeatherHandler : IHandleMessages<GetWeatherRequest>
    {
        static ILog log = LogManager.GetLogger<GetWeatherHandler>();

        public Task Handle(GetWeatherRequest request, IMessageHandlerContext context)
        {
            GetWeatherResponse response = AccuweatherAPICalls.getInstance().getWeatherInfo(request.searchTerm);

            return context.Reply(response);
        }
    }
}
