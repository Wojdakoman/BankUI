using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Projekt.API
{
    class IP
    {
        static public async Task<IPBody> GetData()
        {
            using (HttpResponseMessage response = await APIHelper.ApiClient.GetAsync(APIHelper.ApiClient.BaseAddress))
            {
                if (response.IsSuccessStatusCode)
                {
                    string dane = await response.Content.ReadAsStringAsync();
                    IPBody noweDane = JsonConvert.DeserializeObject<IPBody>(dane);
                    return noweDane;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
