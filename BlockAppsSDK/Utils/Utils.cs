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
            using (HttpClient httpclient = new HttpClient())
            {

                var response = await httpclient.GetAsync(url);
                httpclient.Dispose();
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return response.StatusCode.ToString();
                }
            }
        }

        public static async Task<string> POST(string url, string jsonObject)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                var response =
                    await httpclient.PostAsync(url, new StringContent(jsonObject, Encoding.UTF8, "application/json"));
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
}
