using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xunit;

namespace ShikiNet.Tests
{
    public class UnitTest1
    {
        public static string SITE_DOMAIN { get; private set; }
        public static string API_DOMAIN { get; private set; }

        [Fact]
        public void HttpRequestMessageUriTest()
        {
            SITE_DOMAIN = "https://shikimori.org";
            API_DOMAIN = SITE_DOMAIN + "/api";

            var url = "/users/4114";
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var shortUrl = httpRequestMessage.RequestUri.OriginalString;
            Console.WriteLine(JsonConvert.SerializeObject(httpRequestMessage));
            Console.WriteLine("--------------------------");
            httpRequestMessage.RequestUri = new Uri(GetFullUrl(shortUrl));
            Console.WriteLine(JsonConvert.SerializeObject(httpRequestMessage));
            Console.ReadKey();
            Assert.Equal("https://shikimori.org/api/users/411412", httpRequestMessage.RequestUri.OriginalString);
        }

        private string GetFullUrl(string shortUrl)
        {
            return API_DOMAIN + shortUrl;
        }
    }
}
