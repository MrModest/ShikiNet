using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;

namespace ShikiNet.Utils.JsonConverters
{
    public class DateJsonConverter : IsoDateTimeConverter
    {
        public DateJsonConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
