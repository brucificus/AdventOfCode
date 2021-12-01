namespace AdventOfCode.Y2020.Shared;

public static class ProjectionTaskOfEnumerableExtensions
{
    public static async Task<IEnumerable<TResult>> Select<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection);
    }

    public static async Task<IReadOnlyList<TResult>> Select<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection);
    }

    public static async Task<IEnumerable<TResult>> Select<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, int, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection);
    }

    public static async Task<IReadOnlyList<TResult>> Select<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, int, TResult> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.Select(projection);
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection);
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection);
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector);
    }

    public static async Task<IEnumerable<TResult>> SelectMany<TInput, TResult>(this Task<IEnumerable<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector);
    }

    public static async Task<IReadOnlyList<TResult>> SelectMany<TInput, TResult>(this Task<IReadOnlyList<TInput>> source, Func<TInput, int, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        var itemsAwaited = await source;

        return itemsAwaited.SelectMany(projection, resultSelector);
    }
}
