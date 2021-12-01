namespace AdventOfCode.Y2020.Shared;

public static class ProjectionEnumerableExtensions
{
    public static IReadOnlyList<TResult> Select<TInput, TResult>(this IReadOnlyList<TInput> source, Func<TInput, TResult> projection)
    {
        return Enumerable.Select(source, projection).ToImmutableArray();
    }

    public static IReadOnlyList<TResult> Select<TInput, TResult>(this IReadOnlyList<TInput> source, Func<TInput, int, TResult> projection)
    {
        return Enumerable.Select(source, projection).ToImmutableArray();
    }

    public static IReadOnlyList<TResult> SelectMany<TInput, TResult>(this IReadOnlyList<TInput> source, Func<TInput, IEnumerable<TResult>> projection)
    {
        return Enumerable.SelectMany(source, projection).ToImmutableArray();
    }

    public static IReadOnlyList<TResult> SelectMany<TInput, TResult>(this IReadOnlyList<TInput> source, Func<TInput, int, IEnumerable<TResult>> projection)
    {
        return Enumerable.SelectMany(source, projection).ToImmutableArray();
    }

    public static IReadOnlyList<TResult> SelectMany<TInput, TResult>(this IReadOnlyList<TInput> source, Func<TInput, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        return Enumerable.SelectMany(source, projection, resultSelector).ToImmutableArray();
    }

    public static IReadOnlyList<TResult> SelectMany<TInput, TResult>(this IReadOnlyList<TInput> source, Func<TInput, int, IEnumerable<TResult>> projection, Func<TInput, TResult, TResult> resultSelector)
    {
        return Enumerable.SelectMany(source, projection, resultSelector).ToImmutableArray();
    }
}
