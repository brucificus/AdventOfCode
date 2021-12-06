using System.Runtime.Versioning;

[RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
public partial record SparsePositiveIntegerBoundedInfiniteRasterPlane<TDimension>(RasterConjunction<TDimension> Conjunction)
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
    public TDimension Left = TDimension.Zero;
    public TDimension Bottom = TDimension.Zero;

    private TDimension MaxX => Conjunction.ContainedPathExtents.Maximums.X;
    private TDimension MaxY => Conjunction.ContainedPathExtents.Maximums.Y;

    public Planchette CreatePlanchette()
    {
        return new Planchette(Left, Bottom, this);
    }

    public static SparsePositiveIntegerBoundedInfiniteRasterPlane<TDimension> Parse(IReadOnlyList<string> lines)
    {
        var lineSegments = lines.Select(l => LineSegment2<TDimension>.Parse(l, default)).ToImmutableList();
        return new SparsePositiveIntegerBoundedInfiniteRasterPlane<TDimension>(new RasterConjunction<TDimension>(lineSegments));
    }
}
