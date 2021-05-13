using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IpInfo;
using Windows.UI.Popups;

namespace IWeatherApp
{
    class Geolocalizator
    {
        private HttpClient _httpClient;
        IpInfoApi api;

        public Geolocalizator()
        {
            HttpClientFactory httpClientFactory = new HttpClientFactory();
            this._httpClient = httpClientFactory.GetClient();
            this.api = new IpInfoApi(Credentials.IpInfoApiToken, _httpClient);
        }

        public async Task<string> GetLocationAsync()
        {
            string response = await api.GetCurrentCityAsync();
            return response;
        }
    }
}
