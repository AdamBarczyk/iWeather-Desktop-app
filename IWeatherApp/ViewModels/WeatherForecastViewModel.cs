using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IWeatherApp
{
    class WeatherForecastViewModel : MainViewModelBase
    {
        WeatherForecastService _weatherForecastService = null;

        #region Properties for current weather
        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set { _searchString = value; }
        }
        private string _cityName;
        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; OnPropertyChanged(); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(); }
        }
        private string _mainTemp;
        public string MainTemp
        {
            get { return _mainTemp; }
            set { _mainTemp = value; OnPropertyChanged(); }
        }
        private string _feelTemp;
        public string FeelTemp
        {
            get { return _feelTemp; }
            set { _feelTemp = value; OnPropertyChanged(); }
        }
        private string _minTemp;
        public string MinTemp
        {
            get { return _minTemp; }
            set { _minTemp = value; OnPropertyChanged(); }
        }
        private string _maxTemp;
        public string MaxTemp
        {
            get { return _maxTemp; }
            set { _maxTemp = value; OnPropertyChanged(); }
        }
        private string _pressure;
        public string Pressure
        {
            get { return _pressure; }
            set { _pressure = value; OnPropertyChanged(); }
        }
        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set { _humidity = value; OnPropertyChanged(); }
        }
        private string _wind;
        public string Wind
        {
            get { return _wind; }
            set { _wind = value; OnPropertyChanged(); }
        }
        private string _weatherIconPath;
        public string WeatherIconPath
        {
            get { return _weatherIconPath; }
            set { _weatherIconPath = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties for 1st day after today
        private string _dateDay1;
        public string DateDay1
        {
            get { return _dateDay1; }
            set { _dateDay1 = value; OnPropertyChanged(); }
        }
        private string _weatherIconPathDay1;
        public string WeatherIconPathDay1
        {
            get { return _weatherIconPathDay1; }
            set { _weatherIconPathDay1 = value; OnPropertyChanged(); }
        }
        private string _tempDay1;
        public string TempDay1
        {
            get { return _tempDay1; }
            set { _tempDay1 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties for 2nd day after today
        private string _dateDay2;
        public string DateDay2
        {
            get { return _dateDay2; }
            set { _dateDay2 = value; OnPropertyChanged(); }
        }
        private string _weatherIconPathDay2;
        public string WeatherIconPathDay2
        {
            get { return _weatherIconPathDay2; }
            set { _weatherIconPathDay2 = value; OnPropertyChanged(); }
        }
        private string _tempDay2;
        public string TempDay2
        {
            get { return _tempDay2; }
            set { _tempDay2 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties for 3rd day after today
        private string _dateDay3;
        public string DateDay3
        {
            get { return _dateDay3; }
            set { _dateDay3 = value; OnPropertyChanged(); }
        }
        private string _weatherIconPathDay3;
        public string WeatherIconPathDay3
        {
            get { return _weatherIconPathDay3; }
            set { _weatherIconPathDay3 = value; OnPropertyChanged(); }
        }
        private string _tempDay3;
        public string TempDay3
        {
            get { return _tempDay3; }
            set { _tempDay3 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties for 4th day after today
        private string _dateDay4;
        public string DateDay4
        {
            get { return _dateDay4; }
            set { _dateDay4 = value; OnPropertyChanged(); }
        }
        private string _weatherIconPathDay4;
        public string WeatherIconPathDay4
        {
            get { return _weatherIconPathDay4; }
            set { _weatherIconPathDay4 = value; OnPropertyChanged(); }
        }
        private string _tempDay4;
        public string TempDay4
        {
            get { return _tempDay4; }
            set { _tempDay4 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties for 5th day after today
        private string _dateDay5;
        public string DateDay5
        {
            get { return _dateDay5; }
            set { _dateDay5 = value; OnPropertyChanged(); }
        }
        private string _weatherIconPathDay5;
        public string WeatherIconPathDay5
        {
            get { return _weatherIconPathDay5; }
            set { _weatherIconPathDay5 = value; OnPropertyChanged(); }
        }
        private string _tempDay5;
        public string TempDay5
        {
            get { return _tempDay5; }
            set { _tempDay5 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties for 6th day after today
        private string _dateDay6;
        public string DateDay6
        {
            get { return _dateDay6; }
            set { _dateDay6 = value; OnPropertyChanged(); }
        }
        private string _weatherIconPathDay6;
        public string WeatherIconPathDay6
        {
            get { return _weatherIconPathDay6; }
            set { _weatherIconPathDay6 = value; OnPropertyChanged(); }
        }
        private string _tempDay6;
        public string TempDay6
        {
            get { return _tempDay6; }
            set { _tempDay6 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Properties for 7th day after today
        private string _dateDay7;
        public string DateDay7
        {
            get { return _dateDay7; }
            set { _dateDay7 = value; OnPropertyChanged(); }
        }
        private string _weatherIconPathDay7;
        public string WeatherIconPathDay7
        {
            get { return _weatherIconPathDay7; }
            set { _weatherIconPathDay7 = value; OnPropertyChanged(); }
        }
        private string _tempDay7;
        public string TempDay7
        {
            get { return _tempDay7; }
            set { _tempDay7 = value; OnPropertyChanged(); }
        }
        #endregion

        #region Navigation
        public ICommand SignOutButtonClicked
        {
            get { return new DelegateCommand( () => SignOutUser() ); }
        }

        private void NavigateToStartPage()
        {
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(StartPage));
        }
        #endregion

        public ICommand SearchButtonClicked
        {
            get { return new DelegateCommand(async () => await SearchCity()); }
        }

        /// <summary>
        /// method called when this page is loading
        /// </summary>
        public async Task OnNavigatedTo()
        {
            await LoadWeatherForCurrentLocation();
        }

        private void SignOutUser()
        {
            UserService.Singleton.SignOutUser();
            NavigateToStartPage();
        }

        public async Task SearchCity()
        {
            if (_weatherForecastService is null)
            {
                _weatherForecastService = new WeatherForecastService();
            }

            // update the model
            await _weatherForecastService.LoadWeatherData(SearchString);

            PushDataToTheView();
        }

        private async Task LoadWeatherForCurrentLocation()
        {
            if(_weatherForecastService == null)
            {
                _weatherForecastService = new WeatherForecastService();
            }

            Geolocalizator _geolocalizator = new Geolocalizator();

            // get current city name
            string city = await _geolocalizator.GetLocationAsync(); 

            // update the model
            await _weatherForecastService.LoadWeatherData(city);

            PushDataToTheView();
        }

        private void PushDataToTheView()
        {
            // current weather forecast
            CityName = _weatherForecastService.CityName;
            Description = _weatherForecastService.Description;
            MainTemp = _weatherForecastService.MainTemp.ToString() + Constants.CelsiusDegree;
            FeelTemp = _weatherForecastService.FeelTemp.ToString() + Constants.CelsiusDegree;
            MinTemp = _weatherForecastService.MinTemp.ToString() + Constants.CelsiusDegree;
            MaxTemp = _weatherForecastService.MaxTemp.ToString() + Constants.CelsiusDegree;
            Pressure = _weatherForecastService.Pressure.ToString() + Constants.PressureUnit;
            Humidity = _weatherForecastService.Humidity.ToString() + Constants.PercentSymbol;
            Wind = _weatherForecastService.Wind.ToString() + Constants.SpeedUnit;
            WeatherIconPath = Constants.WeatherIconPathPart1 + _weatherForecastService.WeatherIconName + Constants.WeatherIconPathPart2;


            // 7 days weather forecast
            DateDay1 = _weatherForecastService.SevenDaysForecastDates[0].Substring(0, 10);
            DateDay2 = _weatherForecastService.SevenDaysForecastDates[1].Substring(0, 10);
            DateDay3 = _weatherForecastService.SevenDaysForecastDates[2].Substring(0, 10);
            DateDay4 = _weatherForecastService.SevenDaysForecastDates[3].Substring(0, 10);
            DateDay5 = _weatherForecastService.SevenDaysForecastDates[4].Substring(0, 10);
            DateDay6 = _weatherForecastService.SevenDaysForecastDates[5].Substring(0, 10);
            DateDay7 = _weatherForecastService.SevenDaysForecastDates[6].Substring(0, 10);

            TempDay1 = _weatherForecastService.SevenDaysForecastTemps[0].ToString() + Constants.CelsiusDegree;
            TempDay2 = _weatherForecastService.SevenDaysForecastTemps[1].ToString() + Constants.CelsiusDegree;
            TempDay3 = _weatherForecastService.SevenDaysForecastTemps[2].ToString() + Constants.CelsiusDegree;
            TempDay4 = _weatherForecastService.SevenDaysForecastTemps[3].ToString() + Constants.CelsiusDegree;
            TempDay5 = _weatherForecastService.SevenDaysForecastTemps[4].ToString() + Constants.CelsiusDegree;
            TempDay6 = _weatherForecastService.SevenDaysForecastTemps[5].ToString() + Constants.CelsiusDegree;
            TempDay7 = _weatherForecastService.SevenDaysForecastTemps[6].ToString() + Constants.CelsiusDegree;

            WeatherIconPathDay1 = Constants.WeatherIconPathPart1 + _weatherForecastService.SevenDaysForecastWeatherIconNames[0] + Constants.WeatherIconPathPart2;
            WeatherIconPathDay2 = Constants.WeatherIconPathPart1 + _weatherForecastService.SevenDaysForecastWeatherIconNames[1] + Constants.WeatherIconPathPart2;
            WeatherIconPathDay3 = Constants.WeatherIconPathPart1 + _weatherForecastService.SevenDaysForecastWeatherIconNames[2] + Constants.WeatherIconPathPart2;
            WeatherIconPathDay4 = Constants.WeatherIconPathPart1 + _weatherForecastService.SevenDaysForecastWeatherIconNames[3] + Constants.WeatherIconPathPart2;
            WeatherIconPathDay5 = Constants.WeatherIconPathPart1 + _weatherForecastService.SevenDaysForecastWeatherIconNames[4] + Constants.WeatherIconPathPart2;
            WeatherIconPathDay6 = Constants.WeatherIconPathPart1 + _weatherForecastService.SevenDaysForecastWeatherIconNames[5] + Constants.WeatherIconPathPart2;
            WeatherIconPathDay7 = Constants.WeatherIconPathPart1 + _weatherForecastService.SevenDaysForecastWeatherIconNames[6] + Constants.WeatherIconPathPart2;
        }
    }
}
