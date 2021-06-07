using System;
using System.Collections.Generic;
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
    class LoginViewModel : MainViewModelBase
    {
        private LoginService _loginService = null;

        #region Properties
        private string _email;
        private string _password;
        private string _errorMessage;

        public string Email
        {
            get { return _email; }
            set { _email = value.ToLower(); OnPropertyChanged(); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        #endregion

        #region Navigation
        public ICommand CancelButtonClicked { get; }

        private void CancelRegistration()
        {
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(RegistrationPage));
        }
        #endregion

        public LoginViewModel()
        {
            CancelButtonClicked = new DelegateCommand( CancelRegistration );
        }

        public ICommand LoginButtonClicked
        {
            get { return new DelegateCommand(SignInUser); }
        }

        private async void SignInUser()
        {
            _loginService = new LoginService(Email, Password);

            // start login process
            await _loginService.SignInUserAsync();

            if (_loginService.IsSignedIn)
            {
                Frame navigationFrame = Window.Current.Content as Frame;
                navigationFrame.Navigate(typeof(WeatherForecastPage));
            }
            else
            {
                ErrorMessage = _loginService.ShowError();
            }

            //OpenWeatherMapApiHelper apiHelper = new OpenWeatherMapApiHelper();
            //await apiHelper.GetCityWeatherData("Kraków");

            //Geolocalizator geolocalizator = new Geolocalizator();
            //await geolocalizator.GetLocationAsync();
        }
    }
}
