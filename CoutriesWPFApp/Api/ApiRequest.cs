using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoutriesWPFApp.Api
{
    public static class ApiRequest
    {
        public static List<CountriesApi> CallWebAPi(Uri url, out bool isSuccessStatusCode)
        {
            List<CountriesApi> result = new List<CountriesApi>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = url;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(url).Result;
                isSuccessStatusCode = response.IsSuccessStatusCode;
                if (isSuccessStatusCode)
                {
                    var dataobj = response.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize< List<CountriesApi>>(dataobj.Result);
                }

            }
            return result;
        }
    }
}
