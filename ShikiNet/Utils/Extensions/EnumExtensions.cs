using System;
using System.Collections.Generic;
using System.Text;

namespace ShikiNet.Utils.Extentions
{
    public static class EnumExtensions
    {
        public static string ToLowerString(this Enum @enum)
        {
            return @enum.ToString().ToLower();
        }
    }
}
