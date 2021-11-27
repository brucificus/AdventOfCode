namespace AdventOfCode.Y2020.Shared;

public static class TaskOfEnumerableExtensions
{
    public static async Task<IEnumerable<T>> Where<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where(predicate);
    }

    public static async Task<IReadOnlyList<T>> Where<T>(this Task<IReadOnlyList<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where(predicate).ToImmutableArray();
    }

    public static async Task<IEnumerable<T>> Where<T>(this Task<IEnumerable<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where(predicate);
    }

    public static async Task<IReadOnlyList<T>> Where<T>(this Task<IReadOnlyList<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where(predicate).ToImmutableArray();
    }
    public static async Task<IEnumerable<T>> WhereNot<T>(this Task<IEnumerable<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where(x => !predicate(x));
    }

    public static async Task<IReadOnlyList<T>> WhereNot<T>(this Task<IReadOnlyList<T>> source, Func<T, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where(x => !predicate(x)).ToImmutableArray();
    }

    public static async Task<IEnumerable<T>> WhereNot<T>(this Task<IEnumerable<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where((x, y) => !predicate(x, y));
    }

    public static async Task<IReadOnlyList<T>> WhereNot<T>(this Task<IReadOnlyList<T>> source, Func<T, int, bool> predicate)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Where((x, y) => !predicate(x, y)).ToImmutableArray();
    }

    public static async Task<IEnumerable<TResult>> Select<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection);
    }

    public static async Task<IReadOnlyList<TResult>> Select<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection).ToImmutableArray();
    }

    public static async Task<IEnumerable<TResult>> Select<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, int, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection);
    }

    public static async Task<IReadOnlyList<TResult>> Select<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, int, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection).ToImmutableArray();
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection).ToImmutableArray();
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection).ToImmutableArray();
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector).ToImmutableArray();
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector).ToImmutableArray();
    }
}
