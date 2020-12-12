using System;
using System.Collections.Generic;

namespace Shared.Mapping
{

    public interface IScannablePlane<TDimension, TPlanchette, TCell>
        where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
        where TPlanchette : IFullyBoundedPlane<TDimension>
        where TCell : IEquatable<TCell>
    {
        IEnumerable<TPlanchette> Scan(Vector2<TDimension> peepholeSize);
    }
}
