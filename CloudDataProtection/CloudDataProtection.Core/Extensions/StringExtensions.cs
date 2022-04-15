using System;

namespace CloudDataProtection.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Remove(this string str, string c)
        {
            return str.Replace(c, "");
        }

        public static bool ContainsIgnoreCase(this string str, string c)
        {
            return str.Contains(c, StringComparison.OrdinalIgnoreCase);
        }
    }
}