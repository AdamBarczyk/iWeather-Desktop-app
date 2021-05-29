using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWeatherApp
{
    class Constants
    {
        public static string WeatherIconPathPart1 = "https://openweathermap.org/img/wn/";
        public static string WeatherIconPathPart2 = "@2x.png";

        public static string CelsiusDegree = "°C";
        public static string PressureUnit = " hPa";
        public static string SpeedUnit = " km/h";
        public static string PercentSymbol = "%";

        public static string MaxNumberOfFavoritesCitiesExceededErrorMessage = "You can have up to 5 cities in favorites";
        public static string CityDoesntExistsInFavoritesErrorMessage = "There is not such city in favorites";

        public static int MaxNumberOfFavoritesCities = 5;
    }
}
