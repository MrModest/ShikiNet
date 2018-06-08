using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ShikiNet.Entities
{
    public class Image
    {
        [JsonProperty("original")]
        public string Original { get;}

        [JsonProperty("preview")]
        public string Preview { get; }

        [JsonProperty("x96")]
        public string X96 { get; }

        [JsonProperty("x48")]
        public string X48 { get; }
    }
}
