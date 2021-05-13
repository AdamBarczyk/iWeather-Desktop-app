using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IWeatherApp
{
    public class StartPageModel
    {
        private string city;

        public StartPageModel(string arg)
        {
            city = arg;
        }

        public string ChangeText()
        {
            return city;
        }
    }
}
