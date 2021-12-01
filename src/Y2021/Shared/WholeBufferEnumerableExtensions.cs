namespace AdventOfCode.Y2021.Shared;

public static class WholeBufferEnumerableExtensions
{
    public static IEnumerable<IList<TItem>> BufferWithoutPartials<TItem>(this IEnumerable<TItem> values, int count)
    {
        var buffer = values.Buffer(count);
        buffer = buffer.Where(b => b.Count == count);

        return buffer;
    }

    public static IEnumerable<IList<TItem>> BufferWithoutPartials<TItem>(this IEnumerable<TItem> values, int count, int skip)
    {
        var buffer = values.Buffer(count, skip);
        buffer = buffer.Where(b => b.Count == count);

        return buffer;
    }
}
