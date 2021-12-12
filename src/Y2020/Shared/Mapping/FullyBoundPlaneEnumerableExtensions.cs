namespace AdventOfCode.Y2020.Shared.Mapping;

public static class FullyBoundPlaneEnumerableExtensions
{
    public static IEnumerable<(Vector2<TDimension> coordinate, TCell cell)> Where<TPlane, TDimension, TCell>(this TPlane plane, Func<TCell, Vector2<TDimension>, bool> predicate)
        where TPlane : IFullyBoundedPlane<TDimension>, IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>
        where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    {
        return plane.Where(t => predicate(t.cell, t.coordinate));
    }

    public static IEnumerable<TResult> Select<TPlane, TDimension, TCell, TResult>(this TPlane plane, Func<TCell, Vector2<TDimension>, TResult> selector)
        where TPlane : IFullyBoundedPlane<TDimension>, IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>
        where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    {
        return plane.Select(t => selector(t.cell, t.coordinate));
    }

    public static IEnumerable<TResult> SelectMany<TPlane, TDimension, TCell, TResult>(this TPlane plane, Func<TCell, Vector2<TDimension>, IEnumerable<TResult>> selector)
        where TPlane : IFullyBoundedPlane<TDimension>, IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>
        where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    {

        return plane.SelectMany(t => selector(t.cell, t.coordinate));
    }

    public static IEnumerable<TResult> SelectMany<TPlane, TDimension, TCell, TCollection, TResult>(this TPlane plane, Func<TCell, Vector2<TDimension>, IEnumerable<TCollection>> collectionSelector, Func<(Vector2<TDimension> coordinate, TCell cell), TCollection, TResult> resultSelector)
        where TPlane : IFullyBoundedPlane<TDimension>, IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>
        where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    {
        return plane.SelectMany(t => collectionSelector(t.cell, t.coordinate), resultSelector);
    }

    public static IEnumerable<TResult> SelectMany<TPlane, TDimension, TCell, TCollection, TResult>(this TPlane plane, Func<TCell, Vector2<TDimension>, IEnumerable<TCollection>> collectionSelector, Func<TCell, Vector2<TDimension>, TCollection, TResult> resultSelector)
        where TPlane : IFullyBoundedPlane<TDimension>, IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>
        where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    {
        return plane.SelectMany(t => collectionSelector(t.cell, t.coordinate), (t, l) =>  resultSelector(t.cell, t.coordinate, l));
    }
}
