namespace AdventOfCode.Y2020.Shared.Mapping;

public interface IFullyBoundedPlane<TDimension>
    where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
{
    Vector2<TDimension> Size { get; }

    Vector2<TDimension> Center { get; }
}
