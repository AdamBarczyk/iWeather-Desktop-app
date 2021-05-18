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
            set { _email = value; OnPropertyChanged("Email"); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged("ErrorMessage"); }
        }
        #endregion

        public ICommand LoginButtonClicked
        {
            get { return new DelegateCommand(SignInUser); }
        }

        public async void SignInUser()
        {
            _loginService = new LoginService(Email, Password);

            // start login process
            await _loginService.SignInUserAsync();

            if (_loginService.IsSignedIn)
            {
                Frame navigationFrame = Window.Current.Content as Frame;
                navigationFrame.Navigate(typeof(WeatherDataPage));
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
