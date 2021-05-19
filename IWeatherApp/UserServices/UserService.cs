using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Newtonsoft.Json;
using Windows.UI.Popups;

namespace IWeatherApp
{
    public class UserService
    {
        #region Events
        // This event fires when a user signs in and redirects user to homepage if signing in has completed successfully
        #endregion

        #region Properties
        // This boolean is required to store knowledge about user sign in status
        private bool _isSignedIn = false;
        public bool IsSignedIn
        {
            get { return _isSignedIn; }
            set { _isSignedIn = value; }
        }

        private bool _isRegistrationSucceeded = false;
        public bool IsRegistrationSucceeded
        {
            get { return _isRegistrationSucceeded; }
            set { _isRegistrationSucceeded = value; }
        }

        private FirebaseAuthLink _userData;
        public FirebaseAuthLink UserData => IsSignedIn ? _userData : null;
        #endregion

        #region Constructor
        public UserService() { 
        }
        #endregion

        private static UserService _userService;
        public static UserService Singleton 
        {
           get
           {
                if (_userService is null)
                {
                    _userService = new UserService();
                }
                return _userService;
           }
        }

        public async Task SignInUser(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));

            try
            {
                // Store and sign in user with email and password
                _userData = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                // Refresh user token as it needs to be valid always
                await _userData.GetFreshAuthAsync();

                // Set signed in state to true
                IsSignedIn = true;
            }
            catch (Exception e)
            {
                // if the user is signed in, then it gets him signed out
                IsSignedIn = false;

                new MessageDialog(e.Message);
            }
        }

        public void SignOutUser()
        {
            _userData = null;
            IsSignedIn = false;
        }

        public async Task CreateAccount(string email, string password)
        {

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(Credentials.FirebaseApiKey));

            try
            {
                // Store and signUp the user with email and password
                FirebaseAuthLink userData = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

                // Refresh user token as it needs to be valid always
                await userData.GetFreshAuthAsync();

                IsRegistrationSucceeded = true;
                new MessageDialog("Account created successfully!");
            }
            catch (Exception e)
            {
                IsRegistrationSucceeded = false;
                new MessageDialog(e.Message);
            }
        }
    }
}
