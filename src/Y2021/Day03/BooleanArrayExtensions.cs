using System.Collections;

public static class BooleanArrayExtensions
{
    public static BitArray ToBitArray(this IEnumerable<bool> bits) => new(bits.ToArray());
}
