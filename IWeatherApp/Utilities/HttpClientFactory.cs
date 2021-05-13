using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IWeatherApp
{
    class HttpClientFactory
    {
        private static HttpClient _httpClient;

        public HttpClient GetClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            return _httpClient;
        }
    }
}
