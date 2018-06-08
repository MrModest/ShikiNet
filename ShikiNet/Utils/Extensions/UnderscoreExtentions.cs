using System.Linq;

namespace ShikiNet.Utils.Extensions
{
    public static class UnderscoreExtentions
    {
        public static string ToUnderscoreCase(this string str) {
            return string.Concat(str.Select((x, i) => (i > 0 && char.IsUpper(x)) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}