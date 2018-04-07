using Messages.DataTypes.Database.Weather;
using System;

namespace Messages.ServiceBusRequest.Weather.Responses
{
    [Serializable]
    public class GetWeatherResponse : ServiceBusResponse
    {
        // Response contains a weather info object (containing city, temp, real feel temp, and weather text)
        public GetWeatherResponse(bool result, string response, WeatherInfo info)
            : base(result, response)
        {
            this.info = info;
        }

        public WeatherInfo info;
    }
}
