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
        CurrentForecast _currentForecast;
        SevenDaysForecast _sevenDaysForecast;

        #region current weather data properties
        public string CityName { get; set; }
        public string Description { get; set; }
        public double MainTemp { get; set; }
        public double FeelTemp { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double Wind { get; set; }
        public int CityId { get; set; }
        public string WeatherIconName { get; set; }
        #endregion

        #region Seven days weather data properties
        public string[] SevenDaysForecastDates { get; set; }
        public string[] SevenDaysForecastWeatherIconNames { get; set; }
        public double[] SevenDaysForecastTemps { get; set; }
        #endregion

        public OpenWeatherMapApiHelper()
        {
            HttpClientFactory httpClientFactory = new HttpClientFactory();
            this._httpClient = httpClientFactory.GetClient();
        }

        public async Task GetCityWeatherData(string cityName)
        {
            await GetCurrentForecast(cityName);
            await Get7DaysForecast();

            AssignCurrentWeatherData();
            AssignSevenDaysWeatherData();
        }

        public async Task GetCityWeatherData(int cityId)
        {
            await GetCurrentForecast(cityId);
            await Get7DaysForecast();

            AssignCurrentWeatherData();
            AssignSevenDaysWeatherData();
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
                _currentForecast = JsonConvert.DeserializeObject<CurrentForecast>(_currentApiResponse);
            }
            catch (HttpRequestException e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }
        }

        private async Task GetCurrentForecast(int cityId)
        {
            // get response from API for current weather forecast
            string uri = "https://api.openweathermap.org/data/2.5/weather?id=" + cityId + "&units=metric&appid=" + Credentials.OpenWeatherMapApiKey;
            try
            {
                _currentApiResponse = await _httpClient.GetStringAsync(uri);
                _currentForecast = JsonConvert.DeserializeObject<CurrentForecast>(_currentApiResponse);
            }
            catch (HttpRequestException e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }
        }

        /// <summary>
        /// Gets api response for 7 days weather forecast and converts it into JSON object
        /// </summary>
        private async Task Get7DaysForecast()
        {
            string uri = "https://api.openweathermap.org/data/2.5/onecall?lat=" + _currentForecast.coord.lat + "&lon=" + _currentForecast.coord.lon + 
                "&exclude=current,minutely,hourly&units=metric&appid=" + Credentials.OpenWeatherMapApiKey;
            try
            {
                _7DaysApiResponse = await _httpClient.GetStringAsync(uri);
                _sevenDaysForecast = JsonConvert.DeserializeObject<SevenDaysForecast>(_7DaysApiResponse);
            }
            catch (HttpRequestException e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }
        }

        /// <summary>
        /// Assigning data about current weather forecast to properties accessible in other classes
        /// </summary>
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


        /// <summary>
        /// Assigning data about 7 days weather forecast to properties accessible in other classes
        /// </summary>
        private void AssignSevenDaysWeatherData()
        {
            // get info about the 7days weather
            SevenDaysForecastDates = new string[7];
            SevenDaysForecastTemps = new double[7];
            SevenDaysForecastWeatherIconNames = new string[7];

            for (int i=0; i<7; i++)
            {
                SevenDaysForecastDates[i] = ConvertTimestampToDate(_sevenDaysForecast.daily[i + 1].dt);
                SevenDaysForecastTemps[i] = _sevenDaysForecast.daily[i + 1].temp.day;
                SevenDaysForecastWeatherIconNames[i] = _sevenDaysForecast.daily[i + 1].weather[0].icon;
            }
        }

        private string ConvertTimestampToDate(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime.ToString();
        }
    }
}
