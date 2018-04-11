using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ShikiNet.Util;

namespace ShikiNet.Entity
{
    public class OAuth2Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken  { get; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType    { get; }

        [JsonProperty(PropertyName = "expires_in")]
        public int    ExpiresIn    { get; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; }

        [JsonProperty(PropertyName = "created_at"), JsonConverter(typeof(SecondEpochConverter))]
        public DateTime CreatedAt { get; }
    }
}
