namespace AdventOfCode.Y2020.Shared.Mapping;

public interface IPopulatedFullyBoundedPlane<TDimension, TCell>
    : IEquatable<IPopulatedFullyBoundedPlane<TDimension, TCell>>,
        IFullyBoundedPlane<TDimension>,
        IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>,
        IScannablePlane<TDimension, IPopulatedFullyBoundedPlane<TDimension, TCell>.IPlanchette, TCell>
    where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    where TCell : IEquatable<TCell>
{
    public interface IPlanchette
        : IFullyBoundedPlane<TDimension>, IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>
    {
        IPopulatedFullyBoundedPlane<TDimension, TCell> Parent { get; }
    }
}
