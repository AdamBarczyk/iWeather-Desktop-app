using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IWeatherApp.Models;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IWeatherApp.ViewModels
{
    class RegistrationViewModel : MainViewModelBase
    {
        private RegistrationService _registrationService = null;

        #region Properties
        private string _email;
        private string _password1;
        private string _password2;
        private string _errorMessage;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        public string Password1
        {
            get { return _password1; }
            set { _password1 = value; OnPropertyChanged(); }
        }
        public string Password2
        {
            get { return _password2; }
            set { _password2 = value; OnPropertyChanged(); }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }
        public bool ArePasswordsMatch { get; set; }
        #endregion

        #region Navigation
        public ICommand CancelButtonClickedCommand { get; }

        public ICommand SignUpButtonClickedCommand { get; }

        public ICommand GoBackButtonClickedCommand { get; }

        private void GoToStartPage()
        {
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(StartPage));
        }
        #endregion

        public RegistrationViewModel()
        {
            CancelButtonClickedCommand = new DelegateCommand(GoToStartPage);
            GoBackButtonClickedCommand = new DelegateCommand(GoToStartPage);
            SignUpButtonClickedCommand = new DelegateCommand(SignUpUser);
        }

        private async void SignUpUser()
        {
            if (Email == null || Password1 == null || Password2 == null)
            {
                ErrorMessage = "None of the text fields can be empty";
                return;
            }

            //Check if password and the password confirmation are equal
            CheckPasswords();
            if(ArePasswordsMatch)
            {
                _registrationService = new RegistrationService(Email, Password1);

                // start registration process
                await _registrationService.SignUpUserAsync();

                if (_registrationService.IsRegistrationSucceeded)
                {
                    await new MessageDialog("Account has been created successfully. You can sign in!").ShowAsync();

                    Frame navigationFrame = Window.Current.Content as Frame;
                    navigationFrame.Navigate(typeof(LoginPage));
                }
                else
                {
                    ErrorMessage = "Couldn't create account, please try again later";
                }
            }
            else
            {
                ErrorMessage = "Passwords don't match";
            }
        }

        private void CheckPasswords()
        {
            if (Password1 == Password2)
            {
                ArePasswordsMatch = true;
            }
            else
            {
                ArePasswordsMatch = false;
            }
        }
    }
}
