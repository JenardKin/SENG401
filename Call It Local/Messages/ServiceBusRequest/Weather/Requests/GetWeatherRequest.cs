using Messages.NServiceBus.Commands;
using System;

namespace Messages.ServiceBusRequest.Weather.Requests
{
    [Serializable]
    public class GetWeatherRequest : WeatherServiceRequest
    {
        public GetWeatherRequest(string searchTerm)
            : base(WeatherRequest.getWeather)
        {
            this.searchTerm = searchTerm;
        }

        public string searchTerm;
    }
}
