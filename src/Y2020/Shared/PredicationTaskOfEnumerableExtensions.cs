namespace AdventOfCode.Y2020.Shared;

public static class PredicationTaskOfEnumerableExtensions
{
    public static async Task<IEnumerable<T>> Where<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return Enumerable.Where(itemsAwaited, predicate);
    }

    public static async Task<IReadOnlyList<T>> Where<T>(this Task<IReadOnlyList<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return Enumerable.Where(itemsAwaited, predicate).ToImmutableArray();
    }

    public static async Task<IEnumerable<T>> Where<T>(this Task<IEnumerable<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return Enumerable.Where(itemsAwaited, predicate);
    }

    public static async Task<IReadOnlyList<T>> Where<T>(this Task<IReadOnlyList<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return Enumerable.Where(itemsAwaited, predicate).ToImmutableArray();
    }
    public static async Task<IEnumerable<T>> WhereNot<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.WhereNot(predicate);
    }

    public static async Task<IReadOnlyList<T>> WhereNot<T>(this Task<IReadOnlyList<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.WhereNot(predicate);
    }

    public static async Task<IEnumerable<T>> WhereNot<T>(this Task<IEnumerable<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.WhereNot(predicate);
    }

    public static async Task<IReadOnlyList<T>> WhereNot<T>(this Task<IReadOnlyList<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.WhereNot(predicate);
    }
}
