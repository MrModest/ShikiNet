using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ShikiNet.Entities.Enums;
using ShikiNet.Utils.JsonConverters;

namespace ShikiNet.Entities
{
    public class Manga : Item, ITitle
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
        public MangaKind Kind { get; set; }

        [JsonProperty("volumes")]
        public int Volumes { get; set; }

        [JsonProperty("chapters")]
        public int Chapters { get; set; }
    }
}
