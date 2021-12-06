using System.Runtime.Versioning;

[RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
public readonly record struct LineSegment2<TDimension>(Vector2<TDimension> Point1, Vector2<TDimension> Point2)
    :
        IEqualityOperators<LineSegment2<TDimension>, LineSegment2<TDimension>>,
        ISpanParseable<LineSegment2<TDimension>>
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
    public bool IsParallelToAxisX => Point1.X.Equals(Point2.X);
    public bool IsParallelToAxisY => Point1.Y.Equals(Point2.Y);
    public bool IsAxisParallel => IsParallelToAxisX || IsParallelToAxisY;

    public static LineSegment2<TDimension> Parse(string s, IFormatProvider? provider) =>
        Parse(s.AsSpan(), provider);

    public static LineSegment2<TDimension> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        const string separator = " -> ";
        var indexOfSeparator = s.IndexOf(separator, StringComparison.Ordinal);
        if (indexOfSeparator == -1)
        {
            throw new FormatException("LineSegment element separator not found");
        }

        var point1 = Vector2<TDimension>.Parse(s[..indexOfSeparator], provider);
        var point2 = Vector2<TDimension>.Parse(s[(indexOfSeparator + separator.Length)..], provider);

        return new LineSegment2<TDimension>(point1, point2);
    }

    /// <remarks>
    /// Taken (and refactored slightly) from: https://stackoverflow.com/a/1968345
    /// It is based on an algorithm in Andre LeMothe's "Tricks of the Windows Game Programming Gurus": https://www.amazon.com/dp/0672323699
    /// </remarks>
    public bool AnyIntersectsWith(LineSegment2<TDimension> other)
    {
        var aPoint1 = Point1;
        var aPoint2 = Point2;
        var bPoint1 = other.Point1;
        var bPoint2 = other.Point2;

        var ad = aPoint2 - aPoint1;
        var bd = bPoint2 - bPoint1;

        var nd = aPoint1 - bPoint1;

        var s = (-ad.Y * nd.X + ad.X * nd.Y) / (-bd.X * ad.Y + ad.X * bd.Y);
        var t = ( bd.X * nd.Y - bd.Y * nd.X) / (-bd.X * ad.Y + ad.X * bd.Y);

        return s >= TDimension.AdditiveIdentity && s <= TDimension.MultiplicativeIdentity && t >= TDimension.AdditiveIdentity && t <= TDimension.MultiplicativeIdentity;
    }

    public bool AnyIntersectsWith(Vector2<TDimension> point, TDimension? withinTolerance = default)
    {
        var withinToleranceSquared =
            withinTolerance.HasValue
            ? withinTolerance.Value * withinTolerance.Value
            : DefaultIntersectionComparisonToleranceSquared;

        var distance1Squared = Vector2<TDimension>.DistanceSquared(point, Point1);
        var distance2Squared = Vector2<TDimension>.DistanceSquared(Point2, point);

        return TDimension.Abs(distance2Squared - distance1Squared) < withinToleranceSquared;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out LineSegment2<TDimension> result) =>
        throw new NotImplementedException();

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out LineSegment2<TDimension> result) =>
        throw new NotImplementedException();

    private static readonly TDimension DefaultIntersectionComparisonToleranceSquared =  // nonsense that should always be 0 for integers.
        (TDimension.MultiplicativeIdentity * TDimension.MultiplicativeIdentity) /
        ((TDimension.MultiplicativeIdentity + TDimension.MultiplicativeIdentity) * (TDimension.MultiplicativeIdentity + TDimension.MultiplicativeIdentity));

}
