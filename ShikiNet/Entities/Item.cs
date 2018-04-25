using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ShikiNet.Entities
{
    public abstract class Item
    {
        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("russian")]
        public string Russian { get; }

        [JsonProperty("image")]
        public Image Image { get; }

        [JsonProperty("url")]
        public string Url { get; }
    }
}
