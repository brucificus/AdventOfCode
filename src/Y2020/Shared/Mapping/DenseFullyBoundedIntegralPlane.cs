namespace AdventOfCode.Y2020.Shared.Mapping;

public class DenseFullyBoundedIntegralPlane<TCell> : IPopulatedFullyBoundedPlane<int, TCell>
    where TCell : IEquatable<TCell>
{
    private readonly ImmutableList<ImmutableList<TCell>> _rowMajorGrid;

    private DenseFullyBoundedIntegralPlane(ImmutableList<ImmutableList<TCell>> rowMajorGrid)
    {
        _rowMajorGrid = rowMajorGrid;
    }

    public Vector2<int> Size
    {
        get
        {
            return new Vector2<int>(_rowMajorGrid[0].Count, _rowMajorGrid.Count);
        }
    }

    public Vector2<int> Center
    {
        get
        {
            return new Vector2<int>(_rowMajorGrid[0].Count / 2, _rowMajorGrid.Count / 2);
        }
    }

    private int Left => 0;

    private int Right => _rowMajorGrid[0].Count - 1;

    private int Bottom => 0;

    private int Top => _rowMajorGrid.Count - 1;

    public static IPopulatedFullyBoundedPlane<int, TCell> FromLines(IEnumerable<string> lines, Func<char, TCell> parseCell)
    {
        lines = lines.Select(l => l.Trim()).Where(l => l.Length > 0);
        var cells = lines.Select(l => l.Select(parseCell).ToImmutableList()).ToImmutableList();
        return new DenseFullyBoundedIntegralPlane<TCell>(cells);
    }

    public static IPopulatedFullyBoundedPlane<int, TCell> FromTuples(IEnumerable<(Vector2<int> coordinate, TCell cell)> items)
    {
        var itemsMaterialized = items.ToImmutableList();
        var width = itemsMaterialized.Max(i => i.coordinate.x) + 1;
        var height = itemsMaterialized.Max(i => i.coordinate.y) + 1;
        TCell[,] rowMajorStorage = new TCell[height, width];
        itemsMaterialized.ForEach(i => rowMajorStorage[i.coordinate.y, i.coordinate.x] = i.cell);
        var resultRowMajorStorage = Enumerable.Range(0, height).Select(y => Enumerable.Range(0, width).Select(x => rowMajorStorage[y, x]).ToImmutableList()).ToImmutableList();
        return new DenseFullyBoundedIntegralPlane<TCell>(resultRowMajorStorage);
    }

    public IEnumerable<(Vector2<int> coordinate, TCell cell)> CastRay(Vector2<int> originCoordinate, Vector2<int> step)
    {
        Vector2<int> Add(Vector2<int> left, Vector2<int> right) => new Vector2<int>(left.x + right.x, left.y + right.y);

        for (
            var coordinate = Add(originCoordinate, step);
            coordinate.x >= Left && coordinate.x <= Right && coordinate.y >= Bottom && coordinate.y <= Top;
            coordinate = Add(coordinate, step))
        {
            yield return (coordinate, _rowMajorGrid[coordinate.y][coordinate.x]);
        }
    }

    public bool Equals(IPopulatedFullyBoundedPlane<int, TCell> other)
    {
        if (other is null)
        {
            return false;
        }
        else if (ReferenceEquals(this, other))
        {
            return true;
        }
        else
        {
            return Size == other.Size && this.ToImmutableHashSet().SetEquals(other.ToImmutableHashSet());
        }
    }

    public IEnumerator<(Vector2<int> coordinate, TCell cell)> GetEnumerator()
    {
        for (var y = Bottom; y <= Top; y++)
        {
            for (var x = Left; x <= Right; x++)
            {
                var coordinate = new Vector2<int>(x, y);
                var cell = _rowMajorGrid[y][x];
                yield return (coordinate, cell);
            }
        }
    }

    public IEnumerable<IPopulatedFullyBoundedPlane<int, TCell>.IPlanchette> Scan(Vector2<int> peepholeSize)
    {
        for (var y = Bottom; y <= Top; y++)
        {
            for (var x = Left; x <= Right; x++)
            {
                yield return new Planchette(this, new Vector2<int>(x, y), peepholeSize);
            }
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    private class Planchette : IPopulatedFullyBoundedPlane<int, TCell>.IPlanchette
    {
        private readonly DenseFullyBoundedIntegralPlane<TCell> _parent;

        public Planchette(DenseFullyBoundedIntegralPlane<TCell> parent, Vector2<int> center, Vector2<int> peepholeSize)
        {
            _parent = parent;
            Center = center;
            Size = peepholeSize;
        }

        public IPopulatedFullyBoundedPlane<int, TCell> Parent { get; init; }

        public Vector2<int> Center { get; init; }

        public Vector2<int> Size { get; init; }

        private int Left => Center.x - (Size.x / 2);

        private int Right => Center.x + (Size.x / 2);

        private int Bottom => Center.y - (Size.y / 2);

        private int Top => Center.y + (Size.y / 2);

        public bool Equals(IPopulatedFullyBoundedPlane<int, TCell> other)
        {
            if (other is null)
            {
                return false;
            }
            else if (ReferenceEquals(this, other))
            {
                return true;
            }
            else if (other is IPopulatedFullyBoundedPlane<int, TCell>.IPlanchette otherPlanchette)
            {
                return Parent.Equals(otherPlanchette.Parent)
                       && Size == otherPlanchette.Size
                       && Center == otherPlanchette.Center;
            }
            else
            {
                return false;
            }
        }

        public IEnumerator<(Vector2<int> coordinate, TCell cell)> GetEnumerator()
        {
            var yMin = Math.Max(Bottom, 0);
            var yMax = Math.Min(Top, _parent.Size.y - 1);
            var xMin = Math.Max(Left, 0);
            var xMax = Math.Min(Right, _parent.Size.x - 1);

            for (var y = yMin; y <= yMax; y++)
            {
                for (var x = xMin; x <= xMax; x++)
                {
                    yield return (new Vector2<int>(x, y), _parent._rowMajorGrid[y][x]);
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
