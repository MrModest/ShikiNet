using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ShikiNet.Entity;

namespace ShikiNet.Core
{
    public class Api
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static HttpClient client;
        private static JsonSerializerSettings jsonSerializerSettings;

        public static string SITE_DOMAIN { get; }
        public static string API_DOMAIN { get; }

        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }
        public static string RedirectUrl { get; set; }

        public static string AppName { get; set; }
        public static string DevName { get; set; }

        private static string AccessToken { get; set; }
        public static OAuth2Token OAuth2Token { get; private set; }
        public static bool IsAuthorized
        {
            get
            {
                return AccessToken != null;
            }
        }
        public static bool IsTokenExpired
        {
            get
            {
                if (OAuth2Token == null || String.IsNullOrWhiteSpace(OAuth2Token.AccessToken)) { return true; }

                var expiredDate = new DateTime(OAuth2Token.CreatedAt.Ticks).AddSeconds(OAuth2Token.ExpiresIn);
                if (expiredDate < DateTime.Now) { return true; }

                return false;
            }
        }

        static Api()
        {
            SITE_DOMAIN = "https://shikimori.org";
            API_DOMAIN = SITE_DOMAIN + "/api";

            AppName = "ShikiNet (from api)";
            DevName = "MrModest (from api)";

            jsonSerializerSettings = new JsonSerializerSettings //for (de-)serialization get-autoproperty
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
        }

        private static string GetFullUrl(string shortUrl, RequestVersion requestVersion = RequestVersion.API_V1)
        {
            switch (requestVersion)
            {
                case RequestVersion.API_V1:
                    return API_DOMAIN + shortUrl;
                case RequestVersion.API_V2:
                    return API_DOMAIN + "/v2" + shortUrl;
                case RequestVersion.SITE:
                    return SITE_DOMAIN + shortUrl;
                default:
                    return null;
            }
        }

        //ToDo: set laconic name for arg
        private static async Task<string> RequestAsync(HttpRequestMessage httpRequestMessage)
        {
            var shortUrl = httpRequestMessage.RequestUri.OriginalString;
            httpRequestMessage.RequestUri = new Uri(GetFullUrl(shortUrl)); //get FULL url

            httpRequestMessage.Headers.Add("User-Agent", AppName + "@" + DevName);
            if (IsAuthorized)
            {
                httpRequestMessage.Headers.Add("Authorization", "Bearer " + AccessToken);
            }

            HttpResponseMessage response;
            using(client = new HttpClient())
            {
                response = await client.SendAsync(httpRequestMessage);
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            logger.Warn($"Request | url: [{httpRequestMessage.RequestUri.OriginalString}] | code: [{response.StatusCode}] | message: [{response.ReasonPhrase}]");

            return null;
        }

        internal static async Task<T> GetAsync<T>(string url, RequestVersion requestVersion = RequestVersion.API_V1)
        {

            var response = await RequestAsync(new HttpRequestMessage(HttpMethod.Get, url));

            try
            {
                return JsonConvert.DeserializeObject<T>(response, jsonSerializerSettings);
            }
            catch (JsonSerializationException ex)
            {
                logger.Warn(ex, $"GetAsync<{typeof(T).FullName}> | url: [{url}] | exMessage: [{ex.Message}]");
            }

            return default(T);
        }

        internal static async Task<T> PostAsync<T>(string url, string args = null, RequestVersion requestVersion = RequestVersion.API_V1)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            if (args != null)
            {
                httpRequestMessage.Content = new StringContent(args, Encoding.UTF8, "application/json");
            }
            //httpRequestMessage.Headers.Add("Content-Type", "application/json"); (need?)

            var response = await RequestAsync(httpRequestMessage);

            try
            {
                return JsonConvert.DeserializeObject<T>(response, jsonSerializerSettings);
            }
            catch (JsonSerializationException ex)
            {
                logger.Warn(ex, $"PostAsync<{typeof(T).FullName}> | url: [{url}] | args: [{args}] | exMessage: [{ex.Message}]");
            }

            return default(T);
        }

        internal static async Task<T> PutAsync<T>(string url, string args = null, RequestVersion requestVersion = RequestVersion.API_V1)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            if (args != null)
            {
                httpRequestMessage.Content = new StringContent(args, Encoding.UTF8, "application/json");
            }
            //httpRequestMessage.Headers.Add("Content-Type", "application/json"); (need?)

            var response = await RequestAsync(httpRequestMessage);

            try
            {
                return JsonConvert.DeserializeObject<T>(response, jsonSerializerSettings);
            }
            catch (JsonSerializationException ex)
            {
                logger.Warn(ex, $"PutAsync<{typeof(T).FullName}> | url: [{url}] | args: [{args}] | exMessage: [{ex.Message}]");
            }

            return default(T);
        }

        internal static async Task<T> DeleteAsync<T>(string url, RequestVersion requestVersion = RequestVersion.API_V1)
        {
            var response = await RequestAsync(new HttpRequestMessage(HttpMethod.Delete, url));

            try
            {
                return JsonConvert.DeserializeObject<T>(response, jsonSerializerSettings);
            }
            catch (JsonSerializationException ex)
            {
                logger.Warn(ex, $"DeleteAsync<{typeof(T).FullName}> | url: [{url}] | exMessage: [{ex.Message}]");
            }

            return default(T);
        }

        public static async Task<OAuth2Token> RequestTokenAsync(string authorizationCode)
        {
            if (ClientId == null || ClientSecret == null || authorizationCode == null || RedirectUrl == null) { return null; }

            JObject jObject = JObject.FromObject(new
            {
                grant_type    = "authorization_code",
                client_id     = ClientId,
                client_secret = ClientSecret,
                code          = authorizationCode,
                redirect_uri  = RedirectUrl //need?
            });

            OAuth2Token = await PostAsync<OAuth2Token>("/oauth/token", jObject.ToString(), RequestVersion.SITE);
            AccessToken = OAuth2Token.AccessToken;

            return OAuth2Token;
        }

        public static async Task<OAuth2Token> RefreshTokenAsync(string refreshToken)
        {
            if(ClientId == null || ClientSecret == null || refreshToken == null) { return null; }

            JObject jObject = JObject.FromObject(new
            {
                grant_type = "authorization_code",
                client_id = ClientId,
                client_secret = ClientSecret,
                refresh_token = refreshToken
            });

            OAuth2Token = await PostAsync<OAuth2Token>("/oauth/token", jObject.ToString(), RequestVersion.SITE);
            AccessToken = OAuth2Token.AccessToken;

            return OAuth2Token;
        }
    }

    internal enum RequestVersion
    {
        API_V1,
        API_V2,
        SITE
    }
}
