public partial record SparsePositiveIntegerBoundedInfiniteRasterPlane<TDimension>
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
    public readonly record struct Planchette(TDimension Left, TDimension Bottom,
            SparsePositiveIntegerBoundedInfiniteRasterPlane<TDimension> Parent)
        : IPlanchette<(Vector2<TDimension> Point, RasterConjunction<TDimension> Cell),
                SparsePositiveIntegerBoundedInfiniteRasterPlane<TDimension>>,
            IImmutablySelfEnumerable<Planchette>
    {
        public static readonly Vector2<TDimension> Size = new(TDimension.One, TDimension.One);
        public static readonly Vector2<TDimension> Skip = new(TDimension.One, TDimension.One);
        public Extents<Vector2<TDimension>> Peephole => CalculatePeepholeExtents(Left, Bottom, Size);

        public (Vector2<TDimension> Point, RasterConjunction<TDimension> Cell) Peek()
        {
            var point = new Vector2<TDimension>(Left, Bottom);
            var conjunction = Parent.Conjunction.WhereIntersectsWith(point, TDimension.One);
            return (point, conjunction);
        }

        public OneOf<Planchette, Boundary> MoveNext()
        {
            var x = Left + Skip.X;
            var y = Bottom;

            if (x > Parent.MaxX)
            {
                y += Skip.Y;
                if (y > Parent.MaxY)
                {
                    return default(Boundary);
                }
            }

            return this with { Left = x, Bottom = y };
        }

        public Planchette Reset()
        {
            if (this.Left == Parent.Left && this.Bottom == Parent.Bottom)
            {
                return this;
            }

            return Parent.CreatePlanchette();
        }

        private static Extents<Vector2<TDimension>> CalculatePeepholeExtents(TDimension left, TDimension bottom,
            Vector2<TDimension> size)
        {
            var bottomLeft = new Vector2<TDimension>(left, bottom);
            var topRight = bottomLeft + size;
            return new Extents<Vector2<TDimension>>(bottomLeft, topRight);
        }
    }
}
