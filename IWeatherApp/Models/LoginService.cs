using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWeatherApp
{
    class LoginService
    {
        private string email;
        private string password;
        private bool _isSignedIn = false;
        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set { _isSignedIn = value; }
        }

        public LoginService(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public async Task SignInUserAsync()
        {
            // method is of type "async" because of "await" implementation inside
            UserService userService = new UserService();
            await userService.SignInUser(email, password);

            // pass isSignedIn status from userService to this ViewModel
            IsSignedIn = userService.IsSignedIn;
        }

        public string ShowError()
        {
            return "Wrong email or password";
        }
    }
}
