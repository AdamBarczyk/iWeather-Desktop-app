using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IWeatherApp
{
    class ResetPasswordViewModel : MainViewModelBase
    {
        #region Properties
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        #endregion

        #region Navigation
        public ICommand GoBackButtonClickedCommand { get; }

        private void GoBackToLoginPage()
        {
            Frame navigationFrame = Window.Current.Content as Frame;
            navigationFrame.Navigate(typeof(LoginPage));
        }
        #endregion

        public ICommand ResetPasswordButtonClickedCommand { get; }

        public ResetPasswordViewModel()
        {
            ResetPasswordButtonClickedCommand = new DelegateCommand(async () => await SendPasswordResetEmail());
            GoBackButtonClickedCommand = new DelegateCommand(GoBackToLoginPage);
        }

        private async Task SendPasswordResetEmail()
        {
            await UserService.Singleton.SendPasswordResetEmail(Email);
        }
    }
}
