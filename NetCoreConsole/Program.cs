using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace NetCoreConsole
{
    class Program
    {
        public static string SITE_DOMAIN { get; } = "https://shikimori.org";
        public static string API_DOMAIN { get; } = SITE_DOMAIN + "/api";

        [JsonProperty(PropertyName = "client_id")]
        public static string ClientId { get; } = "wer";

        [JsonProperty(PropertyName = "client_secret")]
        public static string ClientSecret { get; } = "qwer";

        [JsonProperty(PropertyName = "redirect_uri")]
        public static string RedirectUrl { get; } = "smth";

        static void Main(string[] args)
        {
            //SITE_DOMAIN 
            //API_DOMAIN = ;

            //var url = "/users/4114";
            //url = "/animes?query=fullmetal%20alchemist";

            //HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            //Console.WriteLine(httpRequestMessage.RequestUri.ToString());

            //var shortUrl = httpRequestMessage.RequestUri.OriginalString;
            //httpRequestMessage.RequestUri = new Uri(GetFullUrl(shortUrl));

            //Console.WriteLine(httpRequestMessage.RequestUri.OriginalString);
            //Console.WriteLine(httpRequestMessage.RequestUri.ToString());

            JObject jObject = JObject.FromObject(new
            {
                grant_type = "authorization_code",
                ClientId,
                ClientSecret,
                code = 123,
                RedirectUrl
            });

            Console.ReadKey();
        }

        private static string GetFullUrl(string shortUrl)
        {
            return API_DOMAIN + shortUrl;
        }
    }
}
