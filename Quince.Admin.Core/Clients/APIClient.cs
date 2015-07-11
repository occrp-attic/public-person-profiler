using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Clients
{
    public static class APIClient
    {

        public static T GetApiObject<T>(string url) where T:class
        {
            using (var client = new HttpClient())
            {
                //   client.BaseAddress = new Uri("http://localhost:9000/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    T apiResonse = response.Content.ReadAsAsync<T>().Result;
                    return apiResonse;
                }
            }
            return null;

        }
    }
}
