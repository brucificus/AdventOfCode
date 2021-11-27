namespace AdventOfCode.Y2020.Shared;

public static class EnumerableExtensions
{
    public static IReadOnlyList<(T, T)> SelectUniquePairs<T>(this IReadOnlyList<T> self)
        where T : IComparable<T> =>
        self.SelectUniquePairs(i => i);

    public static IReadOnlyList<(T, T)> SelectUniquePairs<T, TKey>(this IReadOnlyList<T> self, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        return self
            .SelectMany(a => self.Where(b => keySelector(a).CompareTo(keySelector(b)) < 0).Select(b => (a, b)))
            .ToImmutableArray();
    }

    public static IReadOnlyList<(int, int, int)> SelectUniqueTriplets(this IReadOnlyList<int> self)
    {
        return self
            .SelectMany(
                a => self.Where(b => a != b && a <= b)
                    .Select(b => (a, b))
                    .SelectMany(
                        p => self.Where(c => p.a != c && p.b != c && p.b <= c)
                            .Select(c => (p.a, p.b, c))))
            .ToImmutableArray();
    }

    public static BigInteger? Multiply(this IEnumerable<int> self)
    {
        var (product, run) = self.Aggregate((product: BigInteger.One, run: false), (p, c) => (p.product * c, true));

        return run ? product : null;
    }

    public static BigInteger Sum(this IEnumerable<BigInteger> self)
    {
        return self.Aggregate(BigInteger.Zero, (p, c) => p + c);
    }

    public static BigInteger Sum<T>(this IEnumerable<T> self, Func<T, BigInteger> selector)
    {
        return self.Aggregate(BigInteger.Zero, (p, c) => p + selector(c));
    }
}
