using Messages;
using Messages.DataTypes.Database.Weather;
using Messages.ServiceBusRequest.Weather.Responses;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.AccuweatherAPI
{
    public class AccuweatherAPICalls
    {
        private AccuweatherAPICalls() { }
        private static AccuweatherAPICalls instance = null;
        // Key needed for the accuweather api
        private const string apikey = "3fDiGNhFgJMJxocmDbSeVAmAuQvS8ACi";

        public static AccuweatherAPICalls getInstance()
        {
            if (instance == null)
            {
                instance = new AccuweatherAPICalls();
            }
            return instance;
        }

        public GetWeatherResponse getWeatherInfo(string searchTerm)
        {
            bool result = false;
            string response = "";
            WeatherInfo info = null;
            string locationKey = "";
            string locationName = "";
            string temperatureMetric = null;
            string realFeelTemperatureMetric = null;
            string weatherText = null;

            string searchUrl = @"http://dataservice.accuweather.com/locations/v1/cities/search?apikey=" + apikey + @"&q=" + searchTerm;
            var accuweatherClient = new HttpClient();
            var content = accuweatherClient.GetStringAsync(searchUrl).Result;
            //Using Json.Net
            //Parses the response to an array
            var jsonobjects = JArray.Parse(content);
            if(jsonobjects.Count > 0)
            {
                // If more than one object, only care about the first result (i.e. most likely the result we wanted)
                // Doing so this way does not always gaurentee we get the correct result
                // For example if the weather for "London" was requested but the business was actually London, ON and not London, UK
                // then the wrong city will be used
                JObject firstResult = (JObject)jsonobjects.First;
                // Key and LocalizedName come before Country, thus break once country is found
                foreach(KeyValuePair<String, JToken> pair in firstResult)
                {
                    if(pair.Key == "Key")
                    {
                        locationKey = (String)pair.Value;
                    }
                    else if(pair.Key == "LocalizedName")
                    {
                        locationName += (String)pair.Value;
                    }
                    else if(pair.Key == "Country")
                    {
                        locationName += (", " + (String)pair.Value["ID"]);
                        break;
                    }
                }
            }
            else
            {
                response = "No location found for the following search term: " + searchTerm;
                return new GetWeatherResponse(result, response, info);
            }

            string conditionsUrl = @"http://dataservice.accuweather.com/currentconditions/v1/" + locationKey + @"?apikey=" + apikey + @"&details=true";
            content = accuweatherClient.GetStringAsync(conditionsUrl).Result;
            jsonobjects = JArray.Parse(content);
            // Only one result will necessarily be returned this time, but it's still in an array
            JObject weatherInfo = (JObject)jsonobjects.First;
            foreach(KeyValuePair<String, JToken> pair in weatherInfo)
            {
                if(pair.Key == "WeatherText")
                {
                    weatherText = (String)pair.Value;
                }
                else if(pair.Key == "Temperature")
                {
                    JObject metricTemp = (JObject)pair.Value["Metric"];
                    foreach(KeyValuePair<String, JToken> tempPair in metricTemp)
                    {
                        if(tempPair.Key == "Value")
                        {
                            temperatureMetric = (String)tempPair.Value;
                        }
                    }
                }
                else if (pair.Key == "RealFeelTemperature")
                {
                    JObject metricTemp = (JObject)pair.Value["Metric"];
                    foreach (KeyValuePair<String, JToken> tempPair in metricTemp)
                    {
                        if (tempPair.Key == "Value")
                        {
                            realFeelTemperatureMetric = (String)tempPair.Value;
                        }
                    }
                    result = true;
                    info = new WeatherInfo()
                    {
                        locationName = locationName,
                        temperatureMetric = temperatureMetric,
                        realFeelTemperatureMetric = realFeelTemperatureMetric,
                        weatherText = weatherText
                    };
                    break;
                }
            }

            return new GetWeatherResponse(result, response, info);
        }
    }
}
