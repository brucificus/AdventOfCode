namespace AdventOfCode.Y2020.Shared.Mapping;

#pragma warning disable RCS1139 // Add summary element to documentation comment.
/// <remarks>
/// I really wanted to use <see cref="System.Numerics.Vector{int}"/>, but its constructor is hard to use correctly because it isn't really designed for direct use.
/// <see cref="System.Numerics.Vector2" /> would have also been cool to use, except it is floating point.
/// </remarks>
public record Vector2<TDimension>(TDimension x, TDimension y)
#pragma warning restore RCS1139 // Add summary element to documentation comment.
    where TDimension : struct, IComparable<TDimension>, IEquatable<TDimension>
{
    public static readonly Vector2<TDimension> One = GetOne();
    public static readonly Vector2<TDimension> Zero = GetZero();

    public Vector2<TDimension> Add((TDimension x, TDimension y) other)
    {
        // Tricking the compiler is tedious.
        if (typeof(TDimension) == typeof(int))
        {
            if (this.x is int x1 && this.y is int y1 && other.x is int x2 && other.y is int y2)
            {
                var xr = x1 + x2;
                var yr = y1 + y2;
                if (xr is TDimension xrc && yr is TDimension yrc)
                {
                    return new Vector2<TDimension>(xrc, yrc);
                }
                else
                {
                    throw new InvalidProgramException();
                }
            }
            else
            {
                throw new InvalidProgramException();
            }
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    public Vector2<TDimension> Multiply(TDimension magnitude)
    {
        // Tricking the compiler is tedious.
        if (typeof(TDimension) == typeof(int))
        {
            if (this.x is int x1 && this.y is int y1 && magnitude is int m)
            {
                var xr = x1 * m;
                var yr = y1 * m;
                if (xr is TDimension xrc && yr is TDimension yrc)
                {
                    return new Vector2<TDimension>(xrc, yrc);
                }
                else
                {
                    throw new InvalidProgramException();
                }
            }
            else
            {
                throw new InvalidProgramException();
            }
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    public static Vector2<TDimension> operator +(Vector2<TDimension> left, Vector2<TDimension> right)
    {
        return left.Add(right);
    }

    public static Vector2<TDimension> operator +(Vector2<TDimension> left, (TDimension x, TDimension y) right)
    {
        return left.Add(right);
    }

    public static Vector2<TDimension> operator *(Vector2<TDimension> left, TDimension right)
    {
        return left.Multiply(right);
    }

    public static implicit operator (TDimension x, TDimension y)(Vector2<TDimension> input)
    {
        return (input.x, input.y);
    }

    public static explicit operator Vector2<TDimension>((TDimension x, TDimension y) input)
    {
        return new Vector2<TDimension>(input.x, input.y);
    }

    private static Vector2<TDimension> GetOne()
    {
        // Tricking the compiler is tedious.
        if (typeof(TDimension) == typeof(int))
        {
            const int vr = 1;

            if (vr is TDimension vrc)
            {
                return new Vector2<TDimension>(vrc, vrc);
            }
            else
            {
                throw new InvalidProgramException();
            }
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    private static Vector2<TDimension> GetZero()
    {
        // Tricking the compiler is tedious.
        if (typeof(TDimension) == typeof(int))
        {
            const int vr = 0;

            if (vr is TDimension vrc)
            {
                return new Vector2<TDimension>(vrc, vrc);
            }
            else
            {
                throw new InvalidProgramException();
            }
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
