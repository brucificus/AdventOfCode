using System.Runtime.CompilerServices;

namespace AdventOfCode.Y2021.Shared;

public readonly record struct Vector2<TDimension>
    :
        IAdditionOperators<Vector2<TDimension>, Vector2<TDimension>, Vector2<TDimension>>,
        IAdditiveIdentity<Vector2<TDimension>, Vector2<TDimension>>,
        IEqualityOperators<Vector2<TDimension>, Vector2<TDimension>>,
        IDivisionOperators<Vector2<TDimension>, Vector2<TDimension>, Vector2<TDimension>>,
        IModulusOperators<Vector2<TDimension>, Vector2<TDimension>, Vector2<TDimension>>,
        IMultiplicativeIdentity<Vector2<TDimension>, Vector2<TDimension>>,
        IMultiplyOperators<Vector2<TDimension>, Vector2<TDimension>, Vector2<TDimension>>,
        ISpanParseable<Vector2<TDimension>>,
        ISubtractionOperators<Vector2<TDimension>, Vector2<TDimension>, Vector2<TDimension>>,
        IUnaryNegationOperators<Vector2<TDimension>, Vector2<TDimension>>,
        IUnaryPlusOperators<Vector2<TDimension>, Vector2<TDimension>>
    where TDimension :
    unmanaged,
    IAdditionOperators<TDimension, TDimension, TDimension>,
    IAdditiveIdentity<TDimension, TDimension>,
    IComparisonOperators<TDimension, TDimension>,
    IComparable,
    IComparable<TDimension>,
    IEqualityOperators<TDimension, TDimension>,
    IEquatable<TDimension>,
    IDecrementOperators<TDimension>,
    IDivisionOperators<TDimension, TDimension, TDimension>,
    IIncrementOperators<TDimension>,
    IModulusOperators<TDimension, TDimension, TDimension>,
    IMultiplicativeIdentity<TDimension, TDimension>,
    IMultiplyOperators<TDimension, TDimension, TDimension>,
    ISpanFormattable,
    IFormattable,
    ISpanParseable<TDimension>,
    IParseable<TDimension>,
    ISubtractionOperators<TDimension, TDimension, TDimension>,
    IUnaryNegationOperators<TDimension, TDimension>,
    IUnaryPlusOperators<TDimension, TDimension>,
    ISignedNumber<TDimension>
{
    public TDimension X => IntrinsicImplementation[0];
    public TDimension Y => IntrinsicImplementation[1];

    private readonly System.Numerics.Vector<TDimension> IntrinsicImplementation;

    public Vector2(TDimension x, TDimension y)
    {
        if (typeof(TDimension) == typeof(byte))
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, byte>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, byte>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, byte>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, byte>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(sbyte) )
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, sbyte>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, sbyte>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, sbyte>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, sbyte>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(short) )
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, short>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, short>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, short>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, short>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(ushort))
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, ushort>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, ushort>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, ushort>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, ushort>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(int)   )
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, int>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, int>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, int>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, int>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(uint)  )
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, uint>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, uint>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, uint>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, uint>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(float) )
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, float>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, float>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, float>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, float>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(long)  )
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, long>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, long>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, long>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, long>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(ulong) )
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, ulong>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, ulong>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, ulong>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, ulong>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(double))
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, double>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, double>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, double>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, double>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(nint))
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, nint>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, nint>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, nint>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, nint>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else if (typeof(TDimension) == typeof(nuint))
        {
            var intrinsicImplementation = new System.Numerics.Vector<TDimension>();
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, nuint>(ref Unsafe.AsRef(intrinsicImplementation)), 0) = Unsafe.As<TDimension, nuint>(ref x);
            Unsafe.Add(ref Unsafe.As<Vector<TDimension>, nuint>(ref Unsafe.AsRef(intrinsicImplementation)), 1) = Unsafe.As<TDimension, nuint>(ref y);
            IntrinsicImplementation = intrinsicImplementation;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public Vector2(System.Numerics.Vector<TDimension> intrinsicImplementation)
    {
        IntrinsicImplementation = intrinsicImplementation;
    }

    public static Vector2<TDimension> AdditiveIdentity { get; } =
        new(TDimension.AdditiveIdentity, TDimension.AdditiveIdentity);
    public static Vector2<TDimension> MultiplicativeIdentity { get; } =
        new(TDimension.MultiplicativeIdentity, TDimension.MultiplicativeIdentity);

    public static Vector2<TDimension> operator +(Vector2<TDimension> left, Vector2<TDimension> right) =>
        new Vector2<TDimension>(left.IntrinsicImplementation + right.IntrinsicImplementation);

    public static Vector2<TDimension> operator /(Vector2<TDimension> left, Vector2<TDimension> right) =>
        new(left.IntrinsicImplementation / right.IntrinsicImplementation);

    public static Vector2<TDimension> operator %(Vector2<TDimension> left, Vector2<TDimension> right) =>
        new(left.X % right.X, left.Y % right.Y);

    public static Vector2<TDimension> operator *(Vector2<TDimension> left, Vector2<TDimension> right) =>
        new(left.IntrinsicImplementation * right.IntrinsicImplementation);

    public static Vector2<TDimension> operator -(Vector2<TDimension> left, Vector2<TDimension> right) =>
        new(left.IntrinsicImplementation - right.IntrinsicImplementation);

    public static Vector2<TDimension> operator -(Vector2<TDimension> value) =>
        new(-value.IntrinsicImplementation);

    public static Vector2<TDimension> operator +(Vector2<TDimension> value) =>
        new(+value.X, +value.Y);

    public static Vector2<TDimension> Parse(string s, IFormatProvider? provider) =>
        Parse(s.AsSpan(), provider);

    public static Vector2<TDimension> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        var indexOfSeparator = s.IndexOf(',');
        if (indexOfSeparator == -1)
        {
            throw new FormatException("Vector element separator not found");
        }

        var x = TDimension.Parse(s[..indexOfSeparator], provider);
        var y = TDimension.Parse(s[(indexOfSeparator + 1)..], provider);

        return new Vector2<TDimension>(x, y);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Vector2<TDimension> result) =>
        throw new NotImplementedException();

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector2<TDimension> result) =>
        throw new NotImplementedException();

    public static TDimension DistanceSquared(Vector2<TDimension> point1, Vector2<TDimension> point2)
    {
        var n = point2 - point1;

        return (n.X * n.X) + (n.Y * n.Y);
    }
}
