using System;
using System.Collections.Generic;
using System.Text;

namespace ShikiNet.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static string ToLowerString(this Enum @enum)
        {
            return @enum.ToString().ToLower();
        }
    }
}
