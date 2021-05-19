using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI.Popups;

namespace IWeatherApp
{
    class OpenWeatherMapApiHelper
    {
        private HttpClient _httpClient;
        private string _currentApiResponse;
        private string _7DaysApiResponse;
        Root _currentForecast;

        #region weather data properties
        private string _cityName;
        public string CityName { get; set; }
        private string _description;
        public string Description { get; set; }
        private double _mainTemp;
        public double MainTemp { get; set; }
        private double _feelTemp;
        public double FeelTemp { get; set; }
        private double _minTemp;
        public double MinTemp { get; set; }
        private double _maxTemp;
        public double MaxTemp { get; set; }
        private double _pressure;
        public double Pressure { get; set; }
        private double _humidity;
        public double Humidity { get; set; }
        private double _wind;
        public double Wind { get; set; }
        private int _cityId;
        public int CityId { get; set; }
        private string _weatherIconName;
        public string WeatherIconName { get; set; }
        #endregion

        public OpenWeatherMapApiHelper()
        {
            HttpClientFactory httpClientFactory = new HttpClientFactory();
            this._httpClient = httpClientFactory.GetClient();
        }

        public async Task GetCityWeatherData(string cityName)
        {
            await GetCurrentForecast(cityName);

            AssignCurrentWeatherData();

            //await new MessageDialog(Description + "\n" + MainTemp + "\n" + FeelTemp + "\n" + MinTemp + "\n" + MaxTemp + "\n" + Pressure + "\n" + Humidity + "\n" + Wind + "\n" + CityId + "\n" + WeatherIcon).ShowAsync();
        }

        /// <summary>
        /// Gets api response for current weather forecast and converts it into JSON object
        /// </summary>
        private async Task GetCurrentForecast(string cityName)
        {
            // get response from API for current weather forecast
            string uri = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&units=metric&appid=" + Credentials.OpenWeatherMapApiKey;
            try
            {
                _currentApiResponse = await _httpClient.GetStringAsync(uri);
                _currentForecast = JsonConvert.DeserializeObject<Root>(_currentApiResponse);
            }
            catch (HttpRequestException e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }
        }

        /// <summary>
        /// Gets api response for 7 days weather forecast and converts it into JSON object
        /// </summary>
        private async Task Get7DaysForecast(int[] coords)
        {
            string uri = "https://api.openweathermap.org/data/2.5/onecall?lat=";
        }

        private void AssignCurrentWeatherData()
        {
            // get info about the current weather

            CityName = _currentForecast.name;
            Description = _currentForecast.weather[0].description;
            MainTemp = _currentForecast.main.temp;
            FeelTemp = _currentForecast.main.feels_like;
            MinTemp = _currentForecast.main.temp_min;
            MaxTemp = _currentForecast.main.temp_max;
            Pressure = _currentForecast.main.pressure;
            Humidity = _currentForecast.main.humidity;
            Wind = _currentForecast.wind.speed;
            CityId = _currentForecast.id;
            WeatherIconName = _currentForecast.weather[0].icon;
        }
    }
}
