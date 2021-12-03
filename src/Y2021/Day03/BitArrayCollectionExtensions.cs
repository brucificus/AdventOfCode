using System.Collections;

namespace AdventOfCode.Y2021.Day03;

public static class BitArrayCollectionExtensions
{
    public static bool? MostCommonBitAtIndex(IReadOnlyList<BitArray> input, Index index)
    {
        var (falseCount, trueCount) = input.Aggregate((falseCount: 0, trueCount: 0), (p, c) => c[index] switch
        {
            false => p with { falseCount = p.falseCount + 1 },
            true => p with { trueCount = p.trueCount + 1 }
        });

        if (trueCount > falseCount)
        {
            return true;
        }

        if (falseCount > trueCount)
        {
            return false;
        }

        return null;
    }

    public static bool? LeastCommonBitAtIndex(IReadOnlyList<BitArray> input, Index index)
    {
        var mostCommon = MostCommonBitAtIndex(input, index);

        if (mostCommon == null) return null;
        return !mostCommon;
    }
}
