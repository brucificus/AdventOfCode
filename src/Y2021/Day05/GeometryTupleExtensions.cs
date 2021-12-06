public static class GeometryTupleExtensions
{
    /// <remarks>
    /// Taken (and refactored slightly) from: https://stackoverflow.com/a/1968345
    /// It is based on an algorithm in Andre LeMothe's "Tricks of the Windows Game Programming Gurus": https://www.amazon.com/dp/0672323699
    /// </remarks>
    public static bool DoIntersect<TDimension>(this (LineSegment2<TDimension> a, LineSegment2<TDimension> b) segments)
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
        var aPoint1 = segments.a.Point1;
        var aPoint2 = segments.a.Point2;
        var bPoint1 = segments.b.Point1;
        var bPoint2 = segments.b.Point2;

        var ad = aPoint2 - aPoint1;
        var bd = bPoint2 - bPoint1;

        var nd = aPoint1 - bPoint1;

        var s = (-ad.Y * nd.X + ad.X * nd.Y) / (-bd.X * ad.Y + ad.X * bd.Y);
        var t = ( bd.X * nd.Y - bd.Y * nd.X) / (-bd.X * ad.Y + ad.X * bd.Y);

        if (s >= TDimension.Zero && s <= TDimension.One && t >= TDimension.Zero && t <= TDimension.One)
        {
            return true;
        }

        return false;
    }
}
