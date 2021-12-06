public static class LineSegmentIntersectionEnumerableExtensions
{
    public static (ImmutableList<LineSegment2<TDimension>> overlappedIntersection, ImmutableQueue<LineSegment2<TDimension>> other)
        GroupByIntersectionWith<TDimension>(
            this ImmutableQueue<LineSegment2<TDimension>> lineSegments, LineSegment2<TDimension> intersectionComparand)
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
        var overlappedIntersectionItems = ImmutableList<LineSegment2<TDimension>>.Empty.ToBuilder();
        var otherItems = ImmutableQueue<LineSegment2<TDimension>>.Empty;

        while (!lineSegments.IsEmpty)
        {
            ref readonly var item = ref lineSegments.PeekRef();

            if (item.AnyIntersectsWith(intersectionComparand))
            {
                overlappedIntersectionItems.Add(item);
            }
            else
            {
                otherItems = otherItems.Enqueue(item);
            }
        }

        return (overlappedIntersectionItems.ToImmutable(), otherItems);
    }
}
