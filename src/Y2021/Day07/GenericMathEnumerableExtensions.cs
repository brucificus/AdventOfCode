public static class GenericMathEnumerableExtensions
{
    public static TProduct Multiply<TValue, TProduct>(this IEnumerable<TValue> values)
        where TValue : IMultiplyOperators<TValue, TProduct, TProduct>, IMultiplicativeIdentity<TValue, TProduct>
    {
        return values.Aggregate(TValue.MultiplicativeIdentity, static (p, c) => c * p);
    }

    public static (TProduct product, TCount count) MultiplyAndCount<TValue, TProduct, TCount>(this IEnumerable<TValue> values)
        where TValue : IMultiplyOperators<TValue, TProduct, TProduct>
        where TProduct : IMultiplicativeIdentity<TProduct, TProduct>
        where TCount : IAdditiveIdentity<TCount, TCount>, IIncrementOperators<TCount>
    {
        return values.Aggregate((product: TProduct.MultiplicativeIdentity, count: TCount.AdditiveIdentity), static (p, c) =>
        {
            var count = p.count;
            count++;
            return p with { product = c * p.product, count = count };
        });
    }

    public static TSum Sum<TValue, TSum>(this IEnumerable<TValue> values)
        where TValue : IAdditionOperators<TValue, TSum, TSum>, IAdditiveIdentity<TValue, TSum>
    {
        return values.Aggregate(TValue.AdditiveIdentity, (p, c) => c + p);
    }

    public static (TSum sum, TCount count) SumAndCount<TValue, TSum, TCount>(this IEnumerable<TValue> values)
        where TValue : IAdditionOperators<TValue, TSum, TSum>
        where TSum : IAdditiveIdentity<TSum, TSum>
        where TCount : IAdditiveIdentity<TCount, TCount>, IIncrementOperators<TCount>
    {
        return values.Aggregate((sum: TSum.AdditiveIdentity, count: TCount.AdditiveIdentity), static (p, c) =>
        {
            var count = p.count;
            count++;
            return p with { sum = c + p.sum, count = count };
        });
    }
}
