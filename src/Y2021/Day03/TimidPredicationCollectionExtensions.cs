namespace AdventOfCode.Y2021.Day03;

public static class TimidPredicationCollectionExtensions
{
    public static ImmutableList<T> WhereNotUnlessLast<T>(this ImmutableList<T> input, Func<T, bool> antiPredicate)
    {
        var result = input;

        var indexOfNext = result.FindIndex(i => antiPredicate(i));

        while (indexOfNext > -1)
        {
            if (result.Count == 1)
                return result;

            result = result.RemoveAt(indexOfNext);
            indexOfNext = result.FindIndex(i => antiPredicate(i));
        }

        return result;
    }
}
