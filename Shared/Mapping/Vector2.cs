using System;

namespace Shared.Mapping
{
    public record Vector2<TDimension>(TDimension x, TDimension y) where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>;
}
