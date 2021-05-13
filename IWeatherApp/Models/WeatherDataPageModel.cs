using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace IWeatherApp
{
    class WeatherDataPageModel
    {
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

        public async Task LoadWeatherData(string city)
        {
            OpenWeatherMapApiHelper apiHelper = new OpenWeatherMapApiHelper();
            await apiHelper.GetCityWeatherData(city);

            CityName = apiHelper.CityName;
            Description = apiHelper.Description;
            MainTemp = Math.Round(apiHelper.MainTemp);
            FeelTemp = Math.Round(apiHelper.FeelTemp);
            MinTemp = Math.Round(apiHelper.MinTemp);
            MaxTemp = Math.Round(apiHelper.MaxTemp);
            Pressure = Math.Round(apiHelper.Pressure);
            Humidity = Math.Round(apiHelper.Humidity);
            Wind = Math.Round(apiHelper.Wind);
            CityId = apiHelper.CityId;
            WeatherIconName = apiHelper.WeatherIconName;
        }
    }
}
