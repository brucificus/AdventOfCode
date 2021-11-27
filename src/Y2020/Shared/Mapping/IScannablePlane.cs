namespace AdventOfCode.Y2020.Shared.Mapping;

public interface IScannablePlane<TDimension, out TPlanchette, TCell>
    where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    where TPlanchette : IFullyBoundedPlane<TDimension>
    where TCell : IEquatable<TCell>
{
    IEnumerable<TPlanchette> Scan(Vector2<TDimension> peepholeSize);

    IEnumerable<(Vector2<int> coordinate, TCell cell)> CastRay(Vector2<TDimension> originCoordinate, Vector2<TDimension> step);
}
