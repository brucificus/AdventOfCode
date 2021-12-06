public readonly record struct RasterConjunction<TDimension>(ImmutableList<LineSegment2<TDimension>> LineSegments)
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
    private Lazy<Extents<Vector2<TDimension>>> LazyContainedPathExtents { get; } = CreateLazyExtents(AllPathReferencePointsContainedIn(LineSegments));
    public Extents<Vector2<TDimension>> ContainedPathExtents => LazyContainedPathExtents.Value;
    private static Lazy<Extents<Vector2<TDimension>>> CreateLazyExtents(IReadOnlyList<Vector2<TDimension>> containedPathReferencePoints) =>
        new(containedPathReferencePoints.AggregateExtents());

    public int Count() => LineSegments.Count;

    public bool AnyIntersectsWith(Vector2<TDimension> point, TDimension? withinTolerance = null)
    {
        if (!ContainedPathExtents.ContainsOrOverlapsWith(point))
        {
            return false;
        }

        return LineSegments.Any(ls => ls.AnyIntersectsWith(point, withinTolerance));
    }

    public bool AnyIntersectsWith(LineSegment2<TDimension> lineSegment)
    {
        return LineSegments.Any(ls => ls.AnyIntersectsWith(lineSegment));
    }

    public RasterConjunction<TDimension> WhereIntersectsWith(LineSegment2<TDimension> lineSegment)
    {
        var lineSegments = LineSegments.Where(ls => ls.AnyIntersectsWith(lineSegment)).ToImmutableList();
        return new RasterConjunction<TDimension>(lineSegments);
    }

    public RasterConjunction<TDimension> WhereIntersectsWith(Vector2<TDimension> point, TDimension? withinTolerance = null)
    {
        var lineSegments = LineSegments.Where(ls => ls.AnyIntersectsWith(point, withinTolerance)).ToImmutableList();
        return new RasterConjunction<TDimension>(lineSegments);
    }

    private static ImmutableArray<Vector2<TDimension>> AllPathReferencePointsContainedIn(IReadOnlyList<LineSegment2<TDimension>> lineSegments) =>
        lineSegments
            .Select(ls => ls.Point1)
            .Union(lineSegments.Select(ls => ls.Point2))
            .ToImmutableArray();
}
