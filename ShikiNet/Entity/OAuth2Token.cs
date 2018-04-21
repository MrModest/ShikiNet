using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ShikiNet.Util;

namespace ShikiNet.Entity
{
    public class OAuth2Token
    {
        [JsonProperty("access_token")]
        public string AccessToken  { get; }

        [JsonProperty("token_type")]
        public string TokenType    { get; }

        [JsonProperty("expires_in")]
        public int    ExpiresIn    { get; } //Seconds

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; }

        [JsonProperty("created_at"), JsonConverter(typeof(SecondEpochConverter))]
        public DateTime CreatedAt { get; }
    }
}
