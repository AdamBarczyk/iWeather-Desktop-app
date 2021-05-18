using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace IWeatherApp
{
    class WeatherForecastViewModel : MainViewModelBase
    {
        WeatherForecastService _weatherForecastService = null;

        #region Properties
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
            set { _weatherIconPath = value; OnPropertyChanged("WeatherIconPath"); }
        }
        #endregion

        public ICommand SearchButtonClicked
        {
            get { return new DelegateCommand(async () => await SearchCity()); }
        }

        public async Task SearchCity()
        {
            if (_weatherForecastService == null)
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

        public async Task OnNavigatedTo()
        {
            await LoadWeatherForCurrentLocation();
        }

        private void PushDataToTheView()
        {
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
        }
    }
}
