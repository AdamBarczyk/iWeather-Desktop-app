using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace IWeatherApp
{
    class FavoritesViewModel : MainViewModelBase
    {
        WeatherForecastService _weatherForecastService = null;
        FirebaseHelper _firebaseHelper = null;

        // --------------------------------- Testowanie bindingu listView ---------------------------------
        private IList<CityForecastItem> _favoriteCitiesTmp;
        private ObservableCollection<CityForecastItem> _favoriteCities;
        public ObservableCollection<CityForecastItem> FavoriteCities 
        {
            get { return _favoriteCities; } 
            set { _favoriteCities = value; OnPropertyChanged(); }
        }

        //public IList<Item1> Fruits { get; } = new List<Item1> 
        //{
        //    new Item1 
        //    {
        //        Name = "krakow",
        //        Temp = 11
        //    },
        //    new Item1
        //    {
        //        Name = "krakow",
        //        Temp = 12
        //    },
        //    new Item1
        //    {
        //        Name = "krakow",
        //        Temp = 13
        //    },
        //    new Item1
        //    {
        //        Name = "krakow",
        //        Temp = 14
        //    },
        //    new Item1
        //    {
        //        Name = "krakow",
        //        Temp = 15
        //    },

        //};
        // ------------------------------------------------------------------------------------------------
        
        public async Task OnNavigatedTo()
        {
            // -------------------------------- TESTOWANIE FIREBASE HELPERA --------------------------------

            // initialize firebase helper
            _firebaseHelper = new FirebaseHelper();
            await _firebaseHelper.GetFavoritesCities();

            // initialize list for storing favorites cities, which will be shown on the page
            _favoriteCitiesTmp = new ObservableCollection<CityForecastItem>(); // tmp list
            FavoriteCities = new ObservableCollection<CityForecastItem>();

            await FillFavoriteCitiesList();

            //await helper.DeleteCityFromFavorites(helper.Cities[0].Object.id);
            //await helper.PutFavoriteCity("city1", 123456);
            //await helper.GetFavoritesCities();

            // ---------------------------------------------------------------------------------------------
        }

        private async Task FillFavoriteCitiesList()
        {
            foreach (var city in _firebaseHelper.Cities)
            {
                await new MessageDialog(city.Object.name + "||" +  city.Object.id.ToString()).ShowAsync();
                await SearchCityById(city.Object.id);

                // show cities on the page after sorting tmp list with all cities by length of the city names
                FavoriteCities = new ObservableCollection<CityForecastItem>(_favoriteCitiesTmp.OrderByDescending(e => e.CityName.Length));
            }
        }

        private async Task SearchCityById(int cityId)
        {
            if (_weatherForecastService is null)
            {
                _weatherForecastService = new WeatherForecastService();
            }

            // update the model
            await _weatherForecastService.LoadWeatherData(cityId);

            // create an item for storing data about weather for searched city
            CityForecastItem item = new CityForecastItem
            {
                CityName = _weatherForecastService.CityName,
                Description = _weatherForecastService.Description,
                MainTemp = _weatherForecastService.MainTemp.ToString() + Constants.CelsiusDegree,
                FeelTemp = _weatherForecastService.FeelTemp.ToString() + Constants.CelsiusDegree,
                MinTemp = _weatherForecastService.MinTemp.ToString() + Constants.CelsiusDegree,
                MaxTemp = _weatherForecastService.MaxTemp.ToString() + Constants.CelsiusDegree,
                Pressure = _weatherForecastService.Pressure.ToString() + Constants.PressureUnit,
                Humidity = _weatherForecastService.Humidity.ToString() + Constants.PercentSymbol,
                Wind = _weatherForecastService.Wind.ToString() + Constants.SpeedUnit,
                WeatherIconPath = Constants.WeatherIconPathPart1 + _weatherForecastService.WeatherIconName + Constants.WeatherIconPathPart2,
            };

            // put the created city item into the tmp list with all city items
            _favoriteCitiesTmp.Add(item);
            //FavoriteCities = new ObservableCollection<CityForecastItem>(FavoriteCities.OrderByDescending(e => e.CityName));
        }
    }

    class Item1
    {
        public string Name { get; set; }
        public int Temp { get; set; }
    }

    class CityForecastItem
    {
        public string CityName { get; set; }
        public string Description { get; set; }
        public string MainTemp { get; set; }
        public string FeelTemp { get; set; }
        public string MinTemp { get; set; }
        public string MaxTemp { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }
        public string Wind { get; set; }
        public string WeatherIconPath { get; set; }
    }
}
