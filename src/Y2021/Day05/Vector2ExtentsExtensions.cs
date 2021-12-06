public static class Vector2ExtentsExtensions
{
    public static bool ContainsOrOverlapsWith<TDimension>(this Extents<Vector2<TDimension>> extents, Vector2<TDimension> point)
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
        IBinaryInteger<TDimension>,
        ISignedNumber<TDimension>
    {
        return point.X >= extents.Minimums.X && point.X <= extents.Maximums.X && point.Y >= extents.Minimums.Y && point.Y <= extents.Maximums.Y;
    }

    public static (LineSegment2<TDimension> left, LineSegment2<TDimension> top, LineSegment2<TDimension> right, LineSegment2<TDimension> bottom) Frame<TDimension>(this Extents<Vector2<TDimension>> extents)
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
        IBinaryInteger<TDimension>,
        ISignedNumber<TDimension>
    {
        var topLeft = new Vector2<TDimension>(extents.Minimums.X, extents.Maximums.Y);
        var topRight = extents.Maximums;
        var bottomRight = new Vector2<TDimension>(extents.Maximums.X, extents.Minimums.Y);
        var bottomLeft = extents.Minimums;

        return
        (
            left: new LineSegment2<TDimension>(bottomLeft, topLeft),
            top: new LineSegment2<TDimension>(topLeft, topRight),
            right: new LineSegment2<TDimension>(topRight, bottomRight),
            bottom: new LineSegment2<TDimension>(bottomRight, bottomLeft)
        );
    }
}
