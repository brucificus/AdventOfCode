using OneOf.Types;

public static class Vector2EnumerableExtensions
{
    public static Extents<Vector2<TDimension>> AggregateExtents<TDimension>(this IReadOnlyList<Vector2<TDimension>> vectors)
        where TDimension :
        unmanaged,
        IAdditionOperators<TDimension, TDimension, TDimension>,
        IAdditiveIdentity<TDimension, TDimension>,
        IComparisonOperators<TDimension, TDimension>,
        IComparable,
        IComparable<TDimension>,
        IEqualityOperators<TDimension, TDimension>,
        IEquatable<TDimension>,
        IDecrementOperators<TDimension>,
        IDivisionOperators<TDimension, TDimension, TDimension>,
        IIncrementOperators<TDimension>,
        IModulusOperators<TDimension, TDimension, TDimension>,
        IMultiplicativeIdentity<TDimension, TDimension>,
        IMultiplyOperators<TDimension, TDimension, TDimension>,
        ISpanFormattable,
        IFormattable,
        ISpanParseable<TDimension>,
        IParseable<TDimension>,
        ISubtractionOperators<TDimension, TDimension, TDimension>,
        IUnaryNegationOperators<TDimension, TDimension>,
        IUnaryPlusOperators<TDimension, TDimension>,
        ISignedNumber<TDimension>
    {
        var minX = vectors.Min(v => v.X);
        var minY = vectors.Min(v => v.Y);
        var maxX = vectors.Max(v => v.X);
        var maxY = vectors.Max(v => v.Y);

        return new(new Vector2<TDimension>(minX, minY), new Vector2<TDimension>(maxX, maxY));
    }

    public static bool AnyFoundWithin<TDimension>(this IEnumerable<Vector2<TDimension>> vectors, Extents<Vector2<TDimension>> extents)
        where TDimension :
        unmanaged,
        IAdditionOperators<TDimension, TDimension, TDimension>,
        IAdditiveIdentity<TDimension, TDimension>,
        IComparisonOperators<TDimension, TDimension>,
        IComparable,
        IComparable<TDimension>,
        IEqualityOperators<TDimension, TDimension>,
        IEquatable<TDimension>,
        IDecrementOperators<TDimension>,
        IDivisionOperators<TDimension, TDimension, TDimension>,
        IIncrementOperators<TDimension>,
        IModulusOperators<TDimension, TDimension, TDimension>,
        IMultiplicativeIdentity<TDimension, TDimension>,
        IMultiplyOperators<TDimension, TDimension, TDimension>,
        ISpanFormattable,
        IFormattable,
        ISpanParseable<TDimension>,
        IParseable<TDimension>,
        ISubtractionOperators<TDimension, TDimension, TDimension>,
        IUnaryNegationOperators<TDimension, TDimension>,
        IUnaryPlusOperators<TDimension, TDimension>,
        ISignedNumber<TDimension>
    {
        return vectors.Any(v => v.X >= extents.Minimums.X && v.X <= extents.Maximums.X && v.Y >= extents.Minimums.Y && v.Y <= extents.Maximums.Y);
    }

    public static bool AllFoundWithin<TDimension>(this IEnumerable<Vector2<TDimension>> vectors, Extents<Vector2<TDimension>> extents)
        where TDimension :
        unmanaged,
        IAdditionOperators<TDimension, TDimension, TDimension>,
        IAdditiveIdentity<TDimension, TDimension>,
        IComparisonOperators<TDimension, TDimension>,
        IComparable,
        IComparable<TDimension>,
        IEqualityOperators<TDimension, TDimension>,
        IEquatable<TDimension>,
        IDecrementOperators<TDimension>,
        IDivisionOperators<TDimension, TDimension, TDimension>,
        IIncrementOperators<TDimension>,
        IModulusOperators<TDimension, TDimension, TDimension>,
        IMultiplicativeIdentity<TDimension, TDimension>,
        IMultiplyOperators<TDimension, TDimension, TDimension>,
        ISpanFormattable,
        IFormattable,
        ISpanParseable<TDimension>,
        IParseable<TDimension>,
        ISubtractionOperators<TDimension, TDimension, TDimension>,
        IUnaryNegationOperators<TDimension, TDimension>,
        IUnaryPlusOperators<TDimension, TDimension>,
        ISignedNumber<TDimension>
    {
        return vectors.All(v => v.X >= extents.Minimums.X && v.X <= extents.Maximums.X && v.Y >= extents.Minimums.Y && v.Y <= extents.Maximums.Y);
    }

    public static IEnumerable<Vector2<TDimension>> WhereFoundWithin<TDimension>(this IEnumerable<Vector2<TDimension>> vectors, Extents<Vector2<TDimension>> extents)
        where TDimension :
        unmanaged,
        IAdditionOperators<TDimension, TDimension, TDimension>,
        IAdditiveIdentity<TDimension, TDimension>,
        IComparisonOperators<TDimension, TDimension>,
        IComparable,
        IComparable<TDimension>,
        IEqualityOperators<TDimension, TDimension>,
        IEquatable<TDimension>,
        IDecrementOperators<TDimension>,
        IDivisionOperators<TDimension, TDimension, TDimension>,
        IIncrementOperators<TDimension>,
        IModulusOperators<TDimension, TDimension, TDimension>,
        IMultiplicativeIdentity<TDimension, TDimension>,
        IMultiplyOperators<TDimension, TDimension, TDimension>,
        ISpanFormattable,
        IFormattable,
        ISpanParseable<TDimension>,
        IParseable<TDimension>,
        ISubtractionOperators<TDimension, TDimension, TDimension>,
        IUnaryNegationOperators<TDimension, TDimension>,
        IUnaryPlusOperators<TDimension, TDimension>,
        ISignedNumber<TDimension>
    {
        return vectors.Where(v => v.X >= extents.Minimums.X && v.X <= extents.Maximums.X && v.Y >= extents.Minimums.Y && v.Y <= extents.Maximums.Y);
    }
}
