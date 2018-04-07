using NServiceBus;
using System;

namespace Messages.ServiceBusRequest.Weather
{
    [Serializable]
    public class WeatherServiceRequest : ServiceBusRequest
    {
        public WeatherServiceRequest(WeatherRequest requestType)
            : base(Service.Weather)
        {
            this.requestType = requestType;
        }

        public WeatherRequest requestType;
    }
    public enum WeatherRequest { getWeather };
}
