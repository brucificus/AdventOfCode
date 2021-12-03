using System.Collections;
using AdventOfCode.Y2021.Day03;
using TPart1InputParsed = System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<char>>;
using TPart1Answer = System.Int32;
using TPart2Answer = System.Int32;


await NUnitApplication.CreateBuilder().Build().RunAsync();

[TestFixture(Description = "Day03")]
public partial class Program : TestableSolverBase<TPart1InputParsed, TPart1Answer, ImmutableList<BitArray>, TPart2Answer>
{
    protected override TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(l => l.ToCharArray().ToArray()).ToArray();

    protected override TPart1Answer Part1AnswerSample => 198;
    protected override TPart1Answer Part1AnswerActual => 3148794;

    protected override TPart1Answer Part1Solver(TPart1InputParsed input)
    {
        var transposedInput = input.Transpose();
        var columnarBitFrequencies = transposedInput.Select((bits, column) => (bits, column)).SelectMany(i => i.bits.GroupBy(c => c).Select(g => (i.column, bit: g.Key, count: g.Count()))).ToArray();
        var gammaRateBits = columnarBitFrequencies.GroupBy(cbf => cbf.column).Select(g => g.OrderByDescending(f => f.count).First().bit).ToArray();
        var epsilonRateBits = columnarBitFrequencies.GroupBy(cbf => cbf.column).Select(g => g.OrderBy(f => f.count).First().bit).ToArray();
        var gammaRate = gammaRateBits.ParseAsBitsOfInt32();
        var epsilonRate = epsilonRateBits.ParseAsBitsOfInt32();

        var powerConsumption = gammaRate * epsilonRate;
        return powerConsumption;
    }



    protected override ImmutableList<BitArray> ParseInputForPart2(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(l => l.ToCharArray().Select(c => c.ParseAsBit()).ToBitArray()).ToImmutableList();

    protected override TPart2Answer Part2AnswerSample => 230;
    protected override TPart2Answer Part2AnswerActual => 2795310;

    protected override TPart2Answer Part2Solver(ImmutableList<BitArray> input)
    {
        var columnsCount = input.Max(ba => ba.Length);

        int FindNumber(Func<IReadOnlyList<BitArray>, Index, bool?> columnarBitSelector, bool columnarBitSelectorFallbackValue)
        {
            var remainingNumbers = input;
            foreach (var column in Enumerable.Range(0, columnsCount))
            {
                var columnarBitRequirement = columnarBitSelector(remainingNumbers, column) ?? columnarBitSelectorFallbackValue;
                remainingNumbers = remainingNumbers.WhereNotUnlessLast(ba => ba[column] != columnarBitRequirement);
                if (remainingNumbers.Count == 1)
                    return remainingNumbers.Single().ToInt32();
            }

            if (remainingNumbers.Count == 1)
                return remainingNumbers.Single().ToInt32();

            throw new InvalidOperationException();
        }

        var oxygenGeneratorRating = FindNumber(BitArrayCollectionExtensions.MostCommonBitAtIndex , true);
        var co2ScrubberRating = FindNumber(BitArrayCollectionExtensions.LeastCommonBitAtIndex, false);
        var lifeSupportRating = oxygenGeneratorRating * co2ScrubberRating;
        return lifeSupportRating;
    }
}
