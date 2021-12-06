public static class ExpansionEnumerableExtensions
{
    public static IReadOnlyList<(T, T)> SelectUniquePairs<T>(this IReadOnlyList<T> self)
        where T : IEquatable<T> =>
        self.SelectUniquePairs((a, b) => (a, b));

    public static IReadOnlyList<TResult> SelectUniquePairs<TInput, TResult>(this IReadOnlyList<TInput> self, Func<TInput, TInput, TResult> resultSelector)
        where TInput : IEquatable<TInput>
    {
        var resultsBuilder = ImmutableList<TResult>.Empty.ToBuilder();
        var matchCache = ImmutableHashSet<TInput>.Empty;

        foreach (var a in self)
        {
            if (matchCache.Contains(a))
                continue;
            matchCache = matchCache.Add(a);

            foreach (var b in self)
            {
                if (matchCache.Contains(b))
                    continue;
                matchCache = matchCache.Add(b);

                resultsBuilder.Add(resultSelector(a, b));
            }
        }

        return resultsBuilder.ToImmutable();
    }
}
