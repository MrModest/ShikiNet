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
using ShikiNet.Static;
using ShikiNet.Util;

namespace ShikiNet.Core
{
    public static class Api
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

        public static OAuth2Token OAuth2Token { get; private set; }
        public static bool AutoRefreshToken { get; set; }
        public static bool IsAuthorized
        {
            get
            {
                return OAuth2Token?.AccessToken != null;
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
            SITE_DOMAIN = StaticValue.SITE_DOMAIN;
            API_DOMAIN = StaticValue.API_DOMAIN;

            AppName = StaticValue.DEFAULT_APP_NAME;
            DevName = StaticValue.DEFAULT_DEV_NAME;

            AutoRefreshToken = false;

            jsonSerializerSettings = new JsonSerializerSettings //for (de-)serialization get-autoproperty
            {
                ContractResolver = new PrivateSetterContractResolver()
            };
        }

        private static string GetFullUrl(string shortUrl, RequestVersion requestVersion)
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

        private static async Task<string> RequestAsync(HttpRequestMessage request, RequestVersion requestVersion)
        {
            var shortUrl = request.RequestUri.OriginalString; //get FULL url
            request.RequestUri = new Uri(GetFullUrl(shortUrl, requestVersion)); 

            logger.InfoStartRequest(request); //logging

            request.Headers.Add("User-Agent", AppName + " %40" + DevName); //%40 - @
            if (IsAuthorized)
            {
                if (AutoRefreshToken && IsTokenExpired)
                {
                    await RefreshTokenAsync(OAuth2Token.AccessToken);
                }
                request.Headers.Add("Authorization", "Bearer " + OAuth2Token.AccessToken);
            }

            HttpResponseMessage response;
            using(client = new HttpClient())
            {
                response = await client.SendAsync(request);
            }

            if (response.IsSuccessStatusCode)
            {
                logger.InfoDoneRequest(request); //logging
                return await response.Content.ReadAsStringAsync();
            }

            logger.WarnNotOkResponse(request, response); //logging

            return null;
        }

        private static T HandleResponse<T>(string response, string method = "<unknown>", string url = "<unknown>", string args = null)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(response, jsonSerializerSettings);
            }
            catch (JsonSerializationException ex)
            {
                logger.WarnDeserializationFail(ex, response, $"{method}<{typeof(T).FullName}>", url, args); //logging
            }

            return default(T);
        }

        internal static async Task<T> GetAsync<T>(string url, RequestVersion requestVersion = RequestVersion.API_V1)
        {

            var response = await RequestAsync(new HttpRequestMessage(HttpMethod.Get, url), requestVersion);

            return HandleResponse<T>(response, "GetAsync", url);
        }

        internal static async Task<T> PostAsync<T>(string url, string args = null, RequestVersion requestVersion = RequestVersion.API_V1)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            if (args != null)
            {
                httpRequestMessage.Content = new StringContent(args, Encoding.UTF8, "application/json");
            }
            //httpRequestMessage.Headers.Add("Content-Type", "application/json"); (need?)

            var response = await RequestAsync(httpRequestMessage, requestVersion);

            return HandleResponse<T>(response, "PostAsync", url, args);
        }

        internal static async Task<T> PutAsync<T>(string url, string args = null, RequestVersion requestVersion = RequestVersion.API_V1)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            if (args != null)
            {
                httpRequestMessage.Content = new StringContent(args, Encoding.UTF8, "application/json");
            }
            //httpRequestMessage.Headers.Add("Content-Type", "application/json"); (need?)

            var response = await RequestAsync(httpRequestMessage, requestVersion);

            return HandleResponse<T>(response, "PutAsync", url, args);
        }

        internal static async Task<T> DeleteAsync<T>(string url, RequestVersion requestVersion = RequestVersion.API_V1)
        {
            var response = await RequestAsync(new HttpRequestMessage(HttpMethod.Delete, url), requestVersion);

            return HandleResponse<T>(response, "DeleteAsync", url);
        }

        public static string GetAuthorizationCodeRequestUrl()
        {
            return $"{SITE_DOMAIN}/oauth/authorize?client_id={ClientId}&redirect_uri={RedirectUrl}&response_type=code";
        }

        public static async Task<OAuth2Token> RequestTokenAsync(string authorizationCode)
        {
            if (ClientId == null || ClientSecret == null || authorizationCode == null || RedirectUrl == null) { return null; }

            logger.InfoExecutionStart("Requesting token"); //logging

            JObject jObject = JObject.FromObject(new
            {
                grant_type    = "authorization_code",
                client_id     = ClientId,
                client_secret = ClientSecret,
                code          = authorizationCode,
                redirect_uri  = RedirectUrl //need?
            });

            OAuth2Token = await PostAsync<OAuth2Token>("/oauth/token", jObject.ToString(), RequestVersion.SITE);

            logger.InfoOrWarnExecutionStatus("Requesting token", IsAuthorized); //logging

            return OAuth2Token;
        }

        public static async Task<OAuth2Token> RefreshTokenAsync(string refreshToken)
        {
            if(ClientId == null || ClientSecret == null || refreshToken == null) { return null; }

            logger.InfoExecutionStart("Refreshing token"); //logging

            JObject jObject = JObject.FromObject(new
            {
                grant_type = "authorization_code",
                client_id = ClientId,
                client_secret = ClientSecret,
                refresh_token = refreshToken
            });

            OAuth2Token = await PostAsync<OAuth2Token>("/oauth/token", jObject.ToString(), RequestVersion.SITE);

            logger.InfoOrWarnExecutionStatus("Refreshing token", !IsTokenExpired); //logging

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
