using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BlockAppsSDK
{
    internal static class Utils
    {
        public static async Task<string> GET(string url)
        {
            //return await new HttpClient().GetAsync(url);
            var httpclient = new HttpClient();
            var response = await httpclient.GetAsync(url);
            httpclient.Dispose();
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
            var httpclient = new HttpClient();
            var response = await httpclient.PostAsync(url, new StringContent(jsonObject, Encoding.UTF8, "application/json"));
            httpclient.Dispose();

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
