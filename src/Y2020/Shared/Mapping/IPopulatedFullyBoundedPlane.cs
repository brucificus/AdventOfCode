namespace AdventOfCode.Y2020.Shared.Mapping;

public interface IPopulatedFullyBoundedPlane<TDimension, TCell, TPlanchette>
    :  // IEquatable<IPopulatedFullyBoundedPlane<TDimension, TCell, TPlanchette>>,
        IFullyBoundedPlane<TDimension>,
        IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>,
        IScannablePlane<TDimension, TPlanchette, TCell>
    where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
    where TCell : IEquatable<TCell>
    where TPlanchette : IFullyBoundedPlane<TDimension>, IEnumerable<(Vector2<TDimension> coordinate, TCell cell)>
{
}

