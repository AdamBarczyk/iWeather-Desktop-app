using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IWeatherApp
{
    class StartPageViewModel : MainViewModelBase
    {
        private StartPageModel _model = null;
        private string _cityLabel = "Your City";
        private string _cityInput = "";

        public string CityLabel
        {
            get { return _cityLabel; }
            set { _cityLabel = value; OnPropertyChanged("CityLabel"); }
        }

        public string CityInput
        {
            get { return _cityInput; }
            set { _cityInput = value; OnPropertyChanged("CityInput"); }
        }

        public ICommand OkButtonClicked
        {
            get { return new DelegateCommand(FindResult); }
        }

        public void FindResult()
        {
            _model = new StartPageModel(CityInput);

            // change text in city label
            CityLabel = _model.ChangeText();
        }
    }
}
