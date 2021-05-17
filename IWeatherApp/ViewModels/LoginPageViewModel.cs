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
    class LoginPageViewModel : MainViewModelBase
    {
        private LoginPageModel _model = null;

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
            _model = new LoginPageModel(Email, Password);

            // start login process
            await _model.SignInUserAsync();

            if (_model.IsSignedIn)
            {
                Frame navigationFrame = Window.Current.Content as Frame;
                bool success = navigationFrame.Navigate(typeof(WeatherDataPage));
            }
            else
            {
                ErrorMessage = _model.ShowError();
            }

            //OpenWeatherMapApiHelper apiHelper = new OpenWeatherMapApiHelper();
            //await apiHelper.GetCityWeatherData("Kraków");

            //Geolocalizator geolocalizator = new Geolocalizator();
            //await geolocalizator.GetLocationAsync();
        }
    }
}
