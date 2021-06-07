using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IWeatherApp.Views;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IWeatherApp
{
    class FavoritesViewModel : MainViewModelBase
    {
        WeatherForecastService _weatherForecastService = null;
        FirebaseHelper _firebaseHelper = null;

        #region Variables/properties for favorite cities lists
        private IList<CityForecastItem> _favoriteCitiesTmp;
        private ObservableCollection<CityForecastItem> _favoriteCities;
        public ObservableCollection<CityForecastItem> FavoriteCities 
        {
            get { return _favoriteCities; } 
            set { _favoriteCities = value; OnPropertyChanged(); }
        }
        #endregion

        #region Navigation
        public ICommand GoBackButtonClicked
        {
            get { return new DelegateCommand( () => NavigateToWeatherForecastPage() ); }
        }

        private void NavigateToWeatherForecastPage()
        {
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(WeatherForecastPage));
        }
        #endregion

        public ICommand FavoriteButtonClickedCommand
        {
            get { return new DelegateCommand(async () => await DeleteCityFromFavorites()); }
        }

        public async Task OnNavigatedTo()
        {
            // initialize firebase helper
            _firebaseHelper = new FirebaseHelper();
            await _firebaseHelper.GetFavoritesCities();

            // initialize list for storing favorites cities, which will be shown on the page
            _favoriteCitiesTmp = new ObservableCollection<CityForecastItem>(); // tmp list
            FavoriteCities = new ObservableCollection<CityForecastItem>();

            await FillFavoriteCitiesList();
        }

        private async Task FillFavoriteCitiesList()
        {
            foreach (var city in _firebaseHelper.Cities)
            {
                await new MessageDialog(city.Object.name + "||" +  city.Object.id.ToString()).ShowAsync();
                await SearchCityById(city.Object.id);

                // show cities on the page after sorting tmp list with all cities by length of the city names
                FavoriteCities = new ObservableCollection<CityForecastItem>(
                    _favoriteCitiesTmp.OrderByDescending(e => e.CityName.Length)
                    );
            }
        }

        /// <summary>
        /// Deletes city from favorites after the user clicked on favorite button under weather details of the city
        /// and refreshes the content on the page
        /// </summary>
        /// <returns></returns>
        private async Task DeleteCityFromFavorites()
        {
            await new MessageDialog("DUPA DUPA DUPA UDPA").ShowAsync();

            // get the current favorites list
            FirebaseHelper firebaseHelper = new FirebaseHelper();
            await firebaseHelper.GetFavoritesCities();

            // delete the city from favorites
            if (firebaseHelper.CityIsInFavorites(_weatherForecastService.CityId))
            {
                await firebaseHelper.DeleteCityFromFavorites(_weatherForecastService.CityId);
            }
            else
            {
                // if the city is not in favorites for some reason, then show notification to the user
                // to contact with the developer
                await new MessageDialog("There is not such city in the database. \n " +
                    "Let's try to contact with the autor of this application, please").ShowAsync();
            }

            // refresh the layout
            await FillFavoriteCitiesList();
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
        }
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
