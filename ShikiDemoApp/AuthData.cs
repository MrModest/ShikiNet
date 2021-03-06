using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ShikiNet.Entities;

namespace ShikiDemoApp
{
    public class AuthData
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("redirect_uri")]
        public string RedirectUrl { get; set; } = "urn:ietf:wg:oauth:2.0:oob";

        [JsonProperty("oauth2_token")]
        public OAuth2Token OAuth2Token { get; set; }

        [JsonIgnore]
        public static string DefaultJson = "{'client_id':null,'client_secret':null,'redirect_uri':'urn:ietf:wg:oauth:2.0:oob','oauth2_token':null}";

        public static AuthData FromJsonPath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, DefaultJson);
            }
            var json = File.ReadAllText(filePath); //fixed!

            var settings = new JsonSerializerSettings //for (de-)serialization get-autoproperty
            {
                ContractResolver = new PrivateSetterContractResolver()
            };

            return JsonConvert.DeserializeObject<AuthData>(json, settings);
        }

        public void SaveToJson(string filePath)
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, json);
        }

        public static AuthData FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AuthData>(json);
        }

        public bool IsTokenExpired
        {
            get
            {
                if (OAuth2Token == null || String.IsNullOrWhiteSpace(OAuth2Token.AccessToken)) { return true; }

                var expiredDate = new DateTime(OAuth2Token.CreatedAt.Ticks).AddSeconds(OAuth2Token.ExpiresIn);
                if (expiredDate < DateTime.Now) { return true; }

                return false;
            }
        }
    }
}
