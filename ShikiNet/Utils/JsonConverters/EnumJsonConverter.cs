using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ShikiNet.Utils.JsonConverters
{
    public class EnumJsonConverter : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((Enum)value).ToString().ToLower());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            var val = reader.Value.ToString().Replace("_", String.Empty);
            return Enum.Parse(objectType, val, true);
        }
    }
}
