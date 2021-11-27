using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Shared;

public static class StringExtensions
{
    private static readonly Regex EmptyLines = new ("^(?:(?:\r ?\n |\r))+", RegexOptions.Multiline | RegexOptions.Compiled);
    private static readonly Regex Newlines = new("\\r?\\n", RegexOptions.Multiline | RegexOptions.Compiled);

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

    public static IReadOnlyList<string> SplitOnEmptyLines(this string self)
    {
        return self.Split(EmptyLines);
    }

    public static IReadOnlyList<string> SplitOnNewlines(this string self)
    {
        return self.Split(Newlines);
    }
}
