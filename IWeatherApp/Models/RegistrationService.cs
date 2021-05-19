using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWeatherApp.Models
{
    class RegistrationService
    {
        private string _email;
        private string _password;
        private bool _isRegistrationSucceeded = false;
        public bool IsRegistrationSucceeded
        {
            get { return _isRegistrationSucceeded; }
            set { _isRegistrationSucceeded = value; }
        }

        public RegistrationService(string email, string password)
        {
            this._email = email;
            this._password = password;
        }

        public async Task SignUpUserAsync()
        {
            await UserService.Singleton.CreateAccount(_email, _password);

            // pass IsRegistrationSucceeded status from userService to this Model
            IsRegistrationSucceeded = UserService.Singleton.IsRegistrationSucceeded;
        }
    }
}
