using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoutriesWPFApp.Api
{
    public static class ApiRequest
    {
        //Функция для получения результата API по url
        //На вход подаём ссылку на выходе получаем данные из файла JSON 
        public static T CallWebAPi<T>(Uri url, out bool isSuccessStatusCode)
        {
            T result = default(T);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = url;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //получаем ответ на запрос
                HttpResponseMessage response = client.GetAsync(url).Result;
                isSuccessStatusCode = response.IsSuccessStatusCode;
                if (isSuccessStatusCode)
                {
                    //читаем ответ
                    var dataobj = response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    //Десериализации с исключением возможных null в population и area
                    result = JsonSerializer.Deserialize<T>(
                        dataobj.Result
                        .Replace("\"area\":null", "\"area\":0.000")
                        .Replace("\"area\":null", "\"population\":0"));
                }
            }
            return result;
        }
    }
}
