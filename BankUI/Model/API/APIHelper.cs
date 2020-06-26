using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Projekt.API
{
    class APIHelper
    {
        static public HttpClient ApiClient { get; set; }
        static private string token = "5ec6e483727974c26afb2106d7141748240978e337e4b5c43b0da3d8";
        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri($"https://api.ipdata.co?api-key={token}");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
