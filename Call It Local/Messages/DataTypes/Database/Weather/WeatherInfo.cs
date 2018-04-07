using System;

namespace Messages.DataTypes.Database.Weather
{
    //Class representing how to return weather info
    [Serializable]
    public partial class WeatherInfo
    {
        public string locationName { get; set; }
        public string temperatureMetric { get; set; }
        public string realFeelTemperatureMetric { get; set; }
        public string weatherText { get; set; }
    }
}
