using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ShikiNet.Utils.JsonConverters;

namespace ShikiNet.Entities
{
    public class Anime : Item, ITitle
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(EnumJsonConverter))]
        public TitleStatus Status { get; }

        [JsonProperty("aired_on")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime? AiredOn { get; }

        [JsonProperty("released_on")]
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime? ReleasedOn { get; }

        //End-Common-Properies

        [JsonProperty("kind")]
        [JsonConverter(typeof(EnumJsonConverter))]
        public AnimeKind Kind { get; }

        [JsonProperty("episodes")]
        public int Episodes { get; }

        [JsonProperty("episodes_aired")]
        public int EpisodesAired { get; }
    }
}
