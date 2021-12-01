namespace AdventOfCode.Y2020.Shared;

public static class PredicationEnumerableExtensions
{
    public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
        Enumerable.Where(source, x => !predicate(x));

    public static IReadOnlyList<T> WhereNot<T>(this IReadOnlyList<T> source, Func<T, bool> predicate) =>
        Enumerable.Where(source, x => !predicate(x)).ToImmutableArray();

    public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, int, bool> predicate) =>
        Enumerable.Where(source, (x, y) => !predicate(x, y));

    public static IReadOnlyList<T> WhereNot<T>(this IReadOnlyList<T> source, Func<T, int, bool> predicate) =>
        Enumerable.Where(source, (x, y) => !predicate(x, y)).ToImmutableArray();
}
