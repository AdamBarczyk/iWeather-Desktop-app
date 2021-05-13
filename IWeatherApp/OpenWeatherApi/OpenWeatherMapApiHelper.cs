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
        private string _apiResponse;
        Root _apiData;

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
            // get response from API
            string uri = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&units=metric&appid=" + Credentials.OpenWeatherMapApiKey;
            try
            {
                _apiResponse = await _httpClient.GetStringAsync(uri);
            } 
            catch (HttpRequestException e)
            {
                await new MessageDialog(e.Message).ShowAsync();
            }


            DeserializeJSON();
            AssignWeatherData();

            //await new MessageDialog(Description + "\n" + MainTemp + "\n" + FeelTemp + "\n" + MinTemp + "\n" + MaxTemp + "\n" + Pressure + "\n" + Humidity + "\n" + Wind + "\n" + CityId + "\n" + WeatherIcon).ShowAsync();
        }

        private void DeserializeJSON()
        {
            _apiData = JsonConvert.DeserializeObject<Root>(_apiResponse);
        } 

        private void AssignWeatherData()
        {
            // get info about the current weather

            CityName = _apiData.name;
            Description = _apiData.weather[0].description;
            MainTemp = _apiData.main.temp;
            FeelTemp = _apiData.main.feels_like;
            MinTemp = _apiData.main.temp_min;
            MaxTemp = _apiData.main.temp_max;
            Pressure = _apiData.main.pressure;
            Humidity = _apiData.main.humidity;
            Wind = _apiData.wind.speed;
            CityId = _apiData.id;
            WeatherIconName = _apiData.weather[0].icon;
        }
    }
}
