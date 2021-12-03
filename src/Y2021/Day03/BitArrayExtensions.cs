using System.Collections;

namespace AdventOfCode.Y2021.Day03;

public static class BitArrayExtensions
{
    public static int ParseAsBitsOfInt32(this char[] bits, Endianness endianness = Endianness.BigEndian)
    {
        int result = 0;
        foreach (var pow in Enumerable.Range(0, bits.Length))
        {
            var index = endianness switch
            {
                Endianness.BigEndian => ^(1 + pow),
                Endianness.LittleEndian => pow,
                _ => throw new ArgumentOutOfRangeException()
            };
            result += bits[index] switch
            {
                '0' => 0,
                '1' => (int)Math.Pow(2, pow),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        return result;
    }

    public static int ToInt32(this ReadOnlySpan<bool> bits, Endianness endianness = Endianness.BigEndian)
    {
        int result = 0;
        foreach (var pow in Enumerable.Range(0, bits.Length))
        {
            var index = endianness switch
            {
                Endianness.BigEndian => ^(1 + pow),
                Endianness.LittleEndian => pow,
                _ => throw new ArgumentOutOfRangeException()
            };
            result += bits[index] switch
            {
                false => 0,
                true => (int)Math.Pow(2, pow)
            };
        }
        return result;
    }

    public static int ToInt32(this BitArray bits, Endianness endianness = Endianness.BigEndian)
    {
        int result = 0;
        foreach (var pow in Enumerable.Range(0, bits.Length))
        {
            var index = endianness switch
            {
                Endianness.BigEndian => ^(1 + pow),
                Endianness.LittleEndian => pow,
                _ => throw new ArgumentOutOfRangeException()
            };
            result += bits[index] switch
            {
                false => 0,
                true => (int)Math.Pow(2, pow)
            };
        }
        return result;
    }


    public static bool ParseAsBit(this char bit)
    {
        return bit switch
        {
            '0' => false,
            '1' => true,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public enum Endianness
{
    BigEndian,
    LittleEndian
}
