using System.Collections;

namespace AdventOfCode.Y2020.Shared.Mapping;

public record DenseFullyBoundedIntegralPlane<TCell>(ImmutableArray<ImmutableArray<TCell>> RowMajorCells)
    : IPopulatedFullyBoundedPlane<int, TCell, DenseFullyBoundedIntegralPlane<TCell>.RectangularPlanchette>
    where TCell : IEquatable<TCell>
{
    public Vector2<int> Size => new(RowMajorCells[0].Length, RowMajorCells.Length);

    private int XMin => 0;

    private int XMax => RowMajorCells[0].Length - 1;

    private int YMin => 0;

    private int YMax => RowMajorCells.Length - 1;

    public static DenseFullyBoundedIntegralPlane<TCell> FromLines(IEnumerable<string> lines, Func<char, TCell> parseCell)
    {
        lines = lines.Select(l => l.Trim()).Where(l => l.Length > 0);
        var cells = lines.Select(l => l.Select(parseCell).ToImmutableArray()).ToImmutableArray();
        return new DenseFullyBoundedIntegralPlane<TCell>(cells);
    }

    public static DenseFullyBoundedIntegralPlane<TCell> FromTuples(IEnumerable<(Vector2<int> coordinate, TCell cell)> items)
    {
        var itemsMaterialized = items.ToImmutableList();
        var width = itemsMaterialized.Max(i => i.coordinate.x) + 1;
        var height = itemsMaterialized.Max(i => i.coordinate.y) + 1;
        TCell[,] rowMajorStorage = new TCell[height, width];
        itemsMaterialized.ForEach(i => rowMajorStorage[i.coordinate.y, i.coordinate.x] = i.cell);
        var resultRowMajorStorage = Enumerable.Range(0, height).Select(y => Enumerable.Range(0, width).Select(x => rowMajorStorage[y, x]).ToImmutableArray()).ToImmutableArray();
        return new DenseFullyBoundedIntegralPlane<TCell>(resultRowMajorStorage);
    }

    public DenseFullyBoundedIntegralPlane<TCell> With(Func<(Vector2<int> coordinate, TCell cell), TCell> selector) =>
        FromTuples(this.Select(t => (t.coordinate, selector(t))));

    public DenseFullyBoundedIntegralPlane<TCell> With(Func<TCell, Vector2<int>, TCell> selector) =>
        FromTuples(this.Select(t => (t.coordinate, selector(t.cell, t.coordinate))));

    public IEnumerable<(Vector2<int> coordinate, TCell cell)> CastRay(Vector2<int> originCoordinate, Vector2<int> step) =>
        CastRay(originCoordinate, step, (i, c) => (c, i));

    public IEnumerable<T> CastRay<T>(Vector2<int> originCoordinate, Vector2<int> step, Func<TCell, Vector2<int>, T> selector)
    {
        static Vector2<int> Add(Vector2<int> left, Vector2<int> right) => new(left.x + right.x, left.y + right.y);

        for (
            var coordinate = Add(originCoordinate, step);
            coordinate.x >= XMin && coordinate.x <= XMax && coordinate.y >= YMin && coordinate.y <= YMax;
            coordinate = Add(coordinate, step))
        {
            yield return selector(RowMajorCells[coordinate.y][coordinate.x], coordinate);
        }
    }

    public CoordinatePredicatedRectangularIterationEnumerable SelectAdjacents(Vector2<int> originCoordinate)
    {
        var (xmin, ymin) = new ValueTuple<int, int>(originCoordinate.x - 1, originCoordinate.y - 1);
        var (xmax, ymax) = new ValueTuple<int, int>(originCoordinate.x + 1, originCoordinate.y + 1);

        var coordinateEnumerable = new RectangularIterationCoordinateEnumerable(this, xmin, xmax, ymin, ymax);

        return new CoordinatePredicatedRectangularIterationEnumerable(this, coordinateEnumerable, c => c != originCoordinate);
    }

    public IEnumerable<RectangularPlanchette> Scan(Vector2<int> peepholeSize)
    {
        var planchetteCenterEnumerable = new RectangularIterationCoordinateEnumerable(this, XMin, XMax, YMin, YMax);

        foreach (var (x, y) in planchetteCenterEnumerable)
        {
            yield return RectangularPlanchette.Create(this, new Vector2<int>(x, y), peepholeSize);
        }
    }

    public CoordinatePredicatedRectangularIterationEnumerable Where(Func<Vector2<int>, bool> predicate)
    {
        var coordinateEnumerable = new RectangularIterationCoordinateEnumerable(this, XMin, XMax, YMin, YMax);
        return new CoordinatePredicatedRectangularIterationEnumerable(this, coordinateEnumerable, predicate);
    }

    public CellPredicatedRectangularIterationEnumerable Where(Func<TCell, bool> predicate)
    {
        var coordinateEnumerable = new RectangularIterationCoordinateEnumerable(this, XMin, XMax, YMin, YMax);
        var innerEnumerable = new RectangularIterationEnumerable(this, coordinateEnumerable);
        return new CellPredicatedRectangularIterationEnumerable(predicate, innerEnumerable);
    }

    public RectangularIterationEnumerable.Enumerator GetEnumerator()
    {
        var coordinateEnumerable = new RectangularIterationCoordinateEnumerable(this, XMin, XMax, YMin, YMax);
        return new RectangularIterationEnumerable.Enumerator(this, coordinateEnumerable.GetEnumerator());
    }

    IEnumerator<(Vector2<int> coordinate, TCell cell)> IEnumerable<(Vector2<int> coordinate, TCell cell)>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public readonly record struct RectangularPlanchette(RectangularIterationEnumerable Content)
        : IEnumerable<(Vector2<int> coordinate, TCell cell)>, IFullyBoundedPlane<int>
    {
        public static RectangularPlanchette Create(DenseFullyBoundedIntegralPlane<TCell> parent, Vector2<int> center, Vector2<int> size)
        {
            var halfX = Math.DivRem(size.x, 2, out int halfXRem);
            var halfY = Math.DivRem(size.y, 2, out int halfYRem);

            var xmin = center.x - halfX;
            var ymin = center.y - halfY;
            var xmax = halfXRem != 0 ? center.x + halfX + 1 : center.x + halfX;
            var ymax = halfYRem != 0 ? center.y + halfY + 1 : center.y + halfY;


            var content = new RectangularIterationEnumerable(parent, new RectangularIterationCoordinateEnumerable(parent, xmin, xmax, ymin, ymax));
            return new RectangularPlanchette(content);
        }

        public int XMin => Content.CoordinateEnumerable.XMin;
        public int XMax => Content.CoordinateEnumerable.XMax;
        public int YMin => Content.CoordinateEnumerable.YMin;
        public int YMax => Content.CoordinateEnumerable.YMax;
        public Vector2<int> Size => new(XMax - XMin, YMax - YMin);

        public RectangularIterationEnumerable.Enumerator GetEnumerator() => Content.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<(Vector2<int> coordinate, TCell cell)> IEnumerable<(Vector2<int> coordinate, TCell cell)>.GetEnumerator() => GetEnumerator();
    }

    public readonly record struct FullyPredicatedRectangularIterationEnumerable(Func<(Vector2<int>, TCell), bool> Predicate, RectangularIterationEnumerable InnerEnumerable)
        : IEnumerable<(Vector2<int> coordinate, TCell cell)>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<(Vector2<int> coordinate, TCell cell)> IEnumerable<(Vector2<int> coordinate, TCell cell)>.GetEnumerator() => GetEnumerator();

        public Enumerator GetEnumerator() => new(Predicate, InnerEnumerable.GetEnumerator());

        public record struct Enumerator(Func<(Vector2<int>, TCell), bool> Predicate, RectangularIterationEnumerable.Enumerator InnerEnumerator)
            : IEnumerator<(Vector2<int> coordinate, TCell cell)>
        {
            object IEnumerator.Current => Current;

            public (Vector2<int> coordinate, TCell cell) Current => InnerEnumerator.Current;

            public bool MoveNext()
            {
                var movedNext = InnerEnumerator.MoveNext();

                while (movedNext && !Predicate(Current))
                {
                    movedNext = InnerEnumerator.MoveNext();
                }

                return movedNext;
            }

            public void Reset() => InnerEnumerator.Reset();

            public void Dispose()
            {
            }
        }
    }

    public readonly record struct CellPredicatedRectangularIterationEnumerable(Func<TCell, bool> Predicate, RectangularIterationEnumerable InnerEnumerable)
        : IEnumerable<(Vector2<int> coordinate, TCell cell)>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<(Vector2<int> coordinate, TCell cell)> IEnumerable<(Vector2<int> coordinate, TCell cell)>.GetEnumerator() => GetEnumerator();

        public Enumerator GetEnumerator() => new(Predicate, InnerEnumerable.GetEnumerator());

        public record struct Enumerator(Func<TCell, bool> Predicate, RectangularIterationEnumerable.Enumerator InnerEnumerator)
            : IEnumerator<(Vector2<int> coordinate, TCell cell)>
        {
            object IEnumerator.Current => Current;

            public (Vector2<int> coordinate, TCell cell) Current => InnerEnumerator.Current;

            public bool MoveNext()
            {
                var movedNext = InnerEnumerator.MoveNext();

                while (movedNext && !Predicate(Current.cell))
                {
                    movedNext = InnerEnumerator.MoveNext();
                }

                return movedNext;
            }

            public void Reset() => InnerEnumerator.Reset();

            public void Dispose()
            {
            }
        }
    }

    public record struct RectangularIterationEnumerable(DenseFullyBoundedIntegralPlane<TCell> Parent, RectangularIterationCoordinateEnumerable CoordinateEnumerable)
        : IEnumerable<(Vector2<int> coordinate, TCell cell)>
    {
        public DenseFullyBoundedIntegralPlane<TCell> Parent { get; } = Parent;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<(Vector2<int> coordinate, TCell cell)> IEnumerable<(Vector2<int> coordinate, TCell cell)>.GetEnumerator() => GetEnumerator();

        public Enumerator GetEnumerator()
        {
            return new Enumerator(Parent, CoordinateEnumerable.GetEnumerator());
        }

        public record struct Enumerator(DenseFullyBoundedIntegralPlane<TCell> Parent, RectangularIterationCoordinateEnumerable.Enumerator CoordinateEnumerator)
            : IEnumerator<(Vector2<int> coordinate, TCell cell)>
        {
            public (Vector2<int>, TCell) Current { get; private set; } = default;

            public bool MoveNext()
            {
                if (CoordinateEnumerator.MoveNext())
                {
                    var coordinate = CoordinateEnumerator.Current;
                    var cell = Parent.RowMajorCells[coordinate.y][coordinate.x];

                    Current = (coordinate, cell);
                    return true;
                }
                else
                {
                    Current = default;
                    return false;
                }
            }

            public void Reset()
            {
                CoordinateEnumerator.Reset();
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }

    public record struct CoordinatePredicatedRectangularIterationEnumerable(DenseFullyBoundedIntegralPlane<TCell> Parent, RectangularIterationCoordinateEnumerable CoordinateEnumerable, Func<Vector2<int>, bool> Predicate)
        : IEnumerable<(Vector2<int> coordinate, TCell cell)>
    {
        public DenseFullyBoundedIntegralPlane<TCell> Parent { get; } = Parent;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<(Vector2<int> coordinate, TCell cell)> IEnumerable<(Vector2<int> coordinate, TCell cell)>.GetEnumerator() => GetEnumerator();

        public Enumerator GetEnumerator()
        {
            return new Enumerator(Parent, CoordinateEnumerable.GetEnumerator(), Predicate);
        }

        public record struct Enumerator(DenseFullyBoundedIntegralPlane<TCell> Parent, RectangularIterationCoordinateEnumerable.Enumerator CoordinateEnumerator, Func<Vector2<int>, bool> Predicate)
            : IEnumerator<(Vector2<int> coordinate, TCell cell)>
        {
            public (Vector2<int>, TCell) Current { get; private set; } = default;

            public bool MoveNext()
            {
                var movedNext = CoordinateEnumerator.MoveNext();
                while (movedNext && !Predicate(CoordinateEnumerator.Current))
                {
                    movedNext = CoordinateEnumerator.MoveNext();
                }

                if (movedNext)
                {
                    Current = (CoordinateEnumerator.Current, Parent.RowMajorCells[CoordinateEnumerator.Current.y][CoordinateEnumerator.Current.x]);
                    return movedNext;
                }
                else
                {
                    Current = default;
                    return movedNext;
                }
            }

            public void Reset()
            {
                Current = default;
                CoordinateEnumerator.Reset();
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }

    public record struct RectangularIterationCoordinateEnumerable(DenseFullyBoundedIntegralPlane<TCell> Parent, int XMin, int XMax, int YMin, int YMax)
        : IEnumerable<Vector2<int>>
    {
        public DenseFullyBoundedIntegralPlane<TCell> Parent { get; } = Parent;
        public int XMin { get; } = Math.Clamp(XMin, Parent.XMin, Parent.XMax);
        public int XMax { get; } = Math.Clamp(XMax, Parent.XMin, Parent.XMax);
        public int YMin { get; } = Math.Clamp(YMin, Parent.YMax, Parent.YMin);
        public int YMax { get; } = Math.Clamp(YMax, Parent.YMax, Parent.YMin);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<Vector2<int>> IEnumerable<Vector2<int>>.GetEnumerator() => GetEnumerator();

        public Enumerator GetEnumerator()
        {
            return new Enumerator(XMin, XMax, YMin, YMax);
        }

        public record struct Enumerator(int XMin, int XMax, int YMin, int YMax)
            : IEnumerator<Vector2<int>>
        {
            public Vector2<int> Current { get; private set; } = Vector2<int>.Zero;

            public bool MoveNext()
            {
                Current = Current with { x = Current.x + 1 };

                if (Current.x > XMax)
                {
                    Current = Current with { x = XMin, y = Current.y + 1 };

                    if (Current.y > YMax)
                    {
                        Current = default;
                        return false;
                    }
                }

                return true;
            }

            public void Reset()
            {
                Current = new Vector2<int>(XMin - 1, YMin);
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}
