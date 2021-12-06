public static class SiftingImmutableExtensions
{
    public static ValueTuple<ImmutableList<TResult1>, ImmutableList<TResult2>> GroupByOneOf<TInput, TResult1, TResult2>(
        this ImmutableList<TInput> inputs,
        Func<TInput, OneOf<TResult1, TResult2>> selector)
    {
        var results = (
            results1: ImmutableList<TResult1>.Empty,
            results2: ImmutableList<TResult2>.Empty
        );

        while (inputs.Count > 0)
        {
            var item = inputs[0];

            var projection = selector(item);

            results = projection.Match(
                g1 => results with { results1 = results.results1.Add(g1) },
                g2 => results with { results2 = results.results2.Add(g2) });

            inputs = inputs.RemoveAt(0);
        }

        return results;
    }

    public static ValueTuple<ImmutableList<TResult1>, ImmutableList<TResult2>, ImmutableList<TResult3>> GroupByOneOf<TInput, TResult1, TResult2, TResult3>(
        this ImmutableList<TInput> inputs,
        Func<TInput, OneOf<TResult1, TResult2, TResult3>> selector)
    {
        var results = (
            results1: ImmutableList<TResult1>.Empty,
            results2: ImmutableList<TResult2>.Empty,
            results3: ImmutableList<TResult3>.Empty
        );

        while (inputs.Count > 0)
        {
            var item = inputs[0];

            var projection = selector(item);

            results = projection.Match(
                g1 => results with { results1 = results.results1.Add(g1) },
                g2 => results with { results2 = results.results2.Add(g2) },
                g3 => results with { results3 = results.results3.Add(g3) }
            );

            inputs = inputs.RemoveAt(0);
        }

        return results;
    }
}
