using System.Linq;

namespace ShikiNet.Utils.Extensions
{
    public static class UnderscoreExtensions
    {
        //https://gist.github.com/vkobel/d7302c0076c64c95ef4b
        public static string ToUnderscoreCase(this string str) {
            return string.Concat(str.Select((x, i) => (i > 0 && char.IsUpper(x)) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}