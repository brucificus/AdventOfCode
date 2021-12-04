using System.Text.RegularExpressions;

namespace AdventOfCode.Y2021.Shared;

public static class EnumerableStringExtensions
{
    public static IReadOnlyList<IReadOnlyList<string>> GroupBySubdividingEmptyLines(this IReadOnlyList<string> lines)
    {
        return GroupBySubdividingEmptyLines(lines.ToImmutableList()).Select(x => (IReadOnlyList<string>)x).ToImmutableArray();
    }

    public static ImmutableArray<ImmutableArray<string>> GroupBySubdividingEmptyLines(this ImmutableList<string> lines)
    {
        var allGroupsBuilder = ImmutableArray<ImmutableArray<string>>.Empty.ToBuilder();
        var itemsThisGroup = ImmutableArray<string>.Empty.ToBuilder();

        while (lines.Count > 0)
        {
            if (!string.IsNullOrWhiteSpace(lines[0]))
            {
                itemsThisGroup.Add(lines[0]);
            }
            else
            {
                if (itemsThisGroup.Count > 0)
                {
                    allGroupsBuilder.Add(itemsThisGroup.ToImmutable());
                    itemsThisGroup.Clear();
                }
            }

            lines = lines.RemoveAt(0);
        }

        if (itemsThisGroup.Count > 0)
        {
            allGroupsBuilder.Add(itemsThisGroup.ToImmutable());
            itemsThisGroup.Clear();
        }

        return allGroupsBuilder.ToImmutable();
    }
}

public static class StringExtensions
{
    public static IReadOnlyList<string> SplitOnWhitespace(this string line) =>
        line.Split(new Regex("\\s+")).Where(x => !string.IsNullOrWhiteSpace(x)).ToImmutableArray();
}
