using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BlockAppsSDK
{
    public static class Utils
    {

        public static async Task<string> GET(string url)
        {
            //return await new HttpClient().GetAsync(url);
            var response = await new HttpClient().GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException();
            }
        }

        public static async Task<string> POST(string url, string jsonObject)
        {
            var response = await new HttpClient().PostAsync(url, new StringContent(jsonObject, Encoding.UTF8));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException();
            }
        } 
    }
}
