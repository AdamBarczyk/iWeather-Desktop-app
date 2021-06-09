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
        public ICommand GoBackButtonClickedCommand { get; }

        private void NavigateToWeatherForecastPage()
        {
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(WeatherForecastPage));
        }
        #endregion

        public FavoritesViewModel()
        {
            GoBackButtonClickedCommand = new DelegateCommand( () => NavigateToWeatherForecastPage() );
        }

        public async Task OnNavigatedTo()
        {
            // initialize firebase helper
            _firebaseHelper = new FirebaseHelper();

            // initialize list for storing favorites cities, which will be shown on the page
            _favoriteCitiesTmp = new List<CityForecastItem>(); // tmp list
            FavoriteCities = new ObservableCollection<CityForecastItem>();

            await RefreshFavoriteCitiesList();
        }

        private async Task RefreshFavoriteCitiesList()
        {
            _favoriteCitiesTmp.Clear();
            foreach (var city in await _firebaseHelper.GetFavoritesCities())
            {
                var item = await SearchCityById(city.Object.id);

                // 
                item.SetFavoriteButtonClickedHandler(DeleteCityFromFavorites);

                // put the created city item into the tmp list with all city items
                _favoriteCitiesTmp.Add(item);
            }

            // show cities on the page after sorting tmp list with all cities by length of the city names
            FavoriteCities = new ObservableCollection<CityForecastItem>(
                _favoriteCitiesTmp.OrderByDescending(e => e.CityName.Length)
                );
        }

        /// <summary>
        /// Deletes city from favorites after the user clicked on favorite button under weather details of the city
        /// and refreshes the content on the page
        /// </summary>
        /// <returns></returns>
        private async Task DeleteCityFromFavorites(int cityId)
        {
            // get the current favorites list
            await _firebaseHelper.GetFavoritesCities();

            // delete the city from favorites
            if (await _firebaseHelper.CityIsInFavorites(cityId))
            {
                await _firebaseHelper.DeleteCityFromFavorites(cityId);
            }
            else
            {
                // if the city is not in favorites for some reason, then show notification to the user
                // to contact with the developer
                await new MessageDialog("There is not such city in the database. \n " +
                    "Let's try to contact with the autor of this application, please").ShowAsync();
            }

            // refresh the layout
            //await Task.Delay(TimeSpan.FromSeconds(5)); 
            await RefreshFavoriteCitiesList();
        }

        private async Task<CityForecastItem> SearchCityById(int cityId)
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
                CityId = _weatherForecastService.CityId,
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

            return item;
        }
    }

    class CityForecastItem
    {
        public int CityId { get; set; }
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

        public ICommand FavoriteButtonClickedCommand { get; private set; }

        public void SetFavoriteButtonClickedHandler(Func<int, Task> handler)
        {
            FavoriteButtonClickedCommand = new DelegateCommand(async () => await handler.Invoke(CityId));
        }
    }
}
