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
    }
}
