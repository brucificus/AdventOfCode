public static class LentPrecisionGenericMathEnumerableExtensions
{
    public static TValue Average<TValue, TSum, TSumDivisable>(this IEnumerable<TValue> values, Func<TSum, TSumDivisable> expandPrecision, Func<TSumDivisable, TValue> contractPrecision)
        where TValue : IAdditionOperators<TValue, TSum, TSum>
        where TSum : IAdditiveIdentity<TSum, TSum>
        where TSumDivisable : IFloatingPoint<TSumDivisable>, IIncrementOperators<TSumDivisable>, IMultiplicativeIdentity<TSumDivisable, TSumDivisable>, IDivisionOperators<TSumDivisable, TSumDivisable, TSumDivisable>
    {
        var sumAndCount = values.SumAndCount<TValue, TSum, TSumDivisable>();

        return contractPrecision(expandPrecision(sumAndCount.sum) / sumAndCount.count);
    }

    public static TValue GeoMean<TValue, TProduct, TProductRootable>(this IEnumerable<TValue> values, Func<TProduct, TProductRootable> expandPrecision, Func<TProductRootable, TValue> contractPrecision)
        where TValue : IMultiplyOperators<TValue, TProduct, TProduct>
        where TProduct : IMultiplicativeIdentity<TProduct, TProduct>
        where TProductRootable : IFloatingPoint<TProductRootable>, IIncrementOperators<TProductRootable>, IMultiplicativeIdentity<TProductRootable, TProductRootable>, IDivisionOperators<TProductRootable, TProductRootable, TProductRootable>
    {
        var productAndCount = values.MultiplyAndCount<TValue, TProduct, TProductRootable>();

        var power = TProductRootable.MultiplicativeIdentity / productAndCount.count;
        return contractPrecision(TProductRootable.Pow(expandPrecision(productAndCount.product), power));
    }
}
