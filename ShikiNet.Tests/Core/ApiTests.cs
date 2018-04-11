using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace ShikiNet.Tests.Core
{
    public class ApiTests
    {
        public static string SITE_DOMAIN { get; }
        public static string API_DOMAIN { get; }

        [Fact]
        public void HttpRequestMessageUriTest()
        {
            var url = new Uri("/users/4114", UriKind.Relative);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var shortUrl = httpRequestMessage.RequestUri.OriginalString;
            Console.WriteLine(JsonConvert.SerializeObject(httpRequestMessage));
            Console.WriteLine("--------------------------");
            httpRequestMessage.RequestUri = new Uri(GetFullUrl(shortUrl));
            Console.WriteLine(JsonConvert.SerializeObject(httpRequestMessage));
        }

        private string GetFullUrl(string shortUrl)
        {
            return API_DOMAIN + shortUrl;
        }
    }
}
