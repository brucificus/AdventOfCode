using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Shared
{
    public static class StringExtensions
    {
        public static IReadOnlyList<string> Split(this string self, Regex regex)
        {
            return regex.Split(self);
        }

        public static bool IsMatch(this string self, Regex regex)
        {
            return regex.IsMatch(self);
        }

        public static MatchCollection Matches(this string self, Regex regex)
        {
            return regex.Matches(self);
        }
    }
}
