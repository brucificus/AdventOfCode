namespace AdventOfCode.Y2020.Shared;

public static class TaskOfStringExtensions
{
    public static async Task<IReadOnlyList<string>> SplitOnEmptyLines(this Task<string> self)
    {
        return (await self).SplitOnEmptyLines();
    }

    public static async Task<IReadOnlyList<string>> SplitOnNewlines(this Task<string> self)
    {
        return (await self).SplitOnNewlines();
    }
}
