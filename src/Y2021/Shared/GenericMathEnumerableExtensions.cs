using System.Runtime.Versioning;

namespace AdventOfCode.Y2021.Shared;

public static class GenericMathEnumerableExtensions
{
    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    public static TSum Sum<TAddend, TSum>(this IEnumerable<TAddend> values)
        where TAddend : IAdditionOperators<TAddend, TAddend, TSum>
        where TSum : IAdditionOperators<TSum, TAddend, TSum>, IAdditiveIdentity<TSum, TSum> =>
        values.Aggregate(TSum.AdditiveIdentity, static (p, c) => p + c);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    public static TProduct Multiply<TFactor, TProduct>(this IEnumerable<TFactor> values)
        where TFactor : IMultiplyOperators<TFactor, TFactor, TProduct>
        where TProduct : IMultiplyOperators<TProduct, TFactor, TProduct>, IMultiplicativeIdentity<TProduct, TProduct> =>
        values.Aggregate(TProduct.MultiplicativeIdentity, static (p, c) => p * c);
}
