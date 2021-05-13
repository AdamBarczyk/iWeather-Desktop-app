using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace IWeatherApp
{
    class WeatherDataPageViewModel : MainViewModelBase
    {
        WeatherDataPageModel _model = null;

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
            set { _cityName = value; OnPropertyChanged("CityName"); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }
        private string _mainTemp;
        public string MainTemp
        {
            get { return _mainTemp; }
            set { _mainTemp = value; OnPropertyChanged("MainTemp"); }
        }
        private string _feelTemp;
        public string FeelTemp
        {
            get { return _feelTemp; }
            set { _feelTemp = value; OnPropertyChanged("FeelTemp"); }
        }
        private string _minTemp;
        public string MinTemp
        {
            get { return _minTemp; }
            set { _minTemp = value; OnPropertyChanged("MinTemp"); }
        }
        private string _maxTemp;
        public string MaxTemp
        {
            get { return _maxTemp; }
            set { _maxTemp = value; OnPropertyChanged("MaxTemp"); }
        }
        private string _pressure;
        public string Pressure
        {
            get { return _pressure; }
            set { _pressure = value; OnPropertyChanged("Pressure"); }
        }
        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set { _humidity = value; OnPropertyChanged("Humidity"); }
        }
        private string _wind;
        public string Wind
        {
            get { return _wind; }
            set { _wind = value; OnPropertyChanged("Wind"); }
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
            get { return new DelegateCommand(SearchCity); }
        }

        public async void SearchCity()
        {
            if (_model == null)
            {
                _model = new WeatherDataPageModel();
            }

            // update the model
            await _model.LoadWeatherData(SearchString);

            PushDataToTheView();
        }

        // NIE DZIAŁA WCZYTYWANIE POGODY DLA AKTUALNEJ POZYCJI PO ZALOGOWANIU
        public async void LoadWeatherForCurrentLocation()
        {
            if(_model == null)
            {
                _model = new WeatherDataPageModel();
            }

            Geolocalizator _geolocalizator = new Geolocalizator();

            // get current city name
            string city = await _geolocalizator.GetLocationAsync(); 

            // update the model
            await _model.LoadWeatherData(city);

            PushDataToTheView();
        }

        private void PushDataToTheView()
        {
            CityName = _model.CityName;
            Description = _model.Description;
            MainTemp = _model.MainTemp.ToString() + Constants.CelsiusDegree;
            FeelTemp = _model.FeelTemp.ToString() + Constants.CelsiusDegree;
            MinTemp = _model.MinTemp.ToString() + Constants.CelsiusDegree;
            MaxTemp = _model.MaxTemp.ToString() + Constants.CelsiusDegree;
            Pressure = _model.Pressure.ToString() + Constants.PressureUnit;
            Humidity = _model.Humidity.ToString() + Constants.PercentSymbol;
            Wind = _model.Wind.ToString() + Constants.SpeedUnit;
            WeatherIconPath = Constants.WeatherIconPathPart1 + _model.WeatherIconName + Constants.WeatherIconPathPart2;
        }
    }
}
