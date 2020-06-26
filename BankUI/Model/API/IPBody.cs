using Newtonsoft.Json;

namespace Projekt.API
{
    public class IPBody
    {
        [JsonProperty("ip")]
        public static string IP { get; set; } = "0.0.0.0";

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country_name")]
        public string Country_name { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
