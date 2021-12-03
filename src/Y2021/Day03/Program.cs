using TPart1InputParsed = System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<char>>;
using TPart1Answer = System.Int32;
using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;
using TPart2Answer = Undefined;


await NUnitApplication.CreateBuilder().Build().RunAsync();

[TestFixture(Description = "Day03")]
public partial class Program : TestableSolverBase<TPart1InputParsed, TPart1Answer, TPart2InputParsed, TPart2Answer>
{
    protected override TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(l => l.ToCharArray()).ToArray();

    protected override TPart1Answer Part1AnswerSample => 198;
    protected override TPart1Answer Part1AnswerActual => 3148794;

    protected override TPart1Answer Part1Solver(TPart1InputParsed input)
    {
        var transposedInput = Transpose(input);
        var columnarBitFrequencies = transposedInput.Select((bits, column) => (bits, column)).SelectMany(i => i.bits.GroupBy(c => c).Select(g => (i.column, bit: g.Key, count: g.Count()))).ToArray();
        var gammaRateBits = columnarBitFrequencies.GroupBy(cbf => cbf.column).Select(g => g.OrderByDescending(f => f.count).First().bit).ToArray();
        var epsilonRateBits = columnarBitFrequencies.GroupBy(cbf => cbf.column).Select(g => g.OrderBy(f => f.count).First().bit).ToArray();
        var gammaRate = ParseIntBits(gammaRateBits);
        var epsilonRate = ParseIntBits(epsilonRateBits);

        var powerConsumption = gammaRate * epsilonRate;
        return powerConsumption;
    }



    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) => default!;

    protected override TPart2Answer Part2AnswerSample => throw new NotImplementedException("Part 2 Sample Answer");
    protected override TPart2Answer Part2AnswerActual => throw new NotImplementedException("Part 2 Actual Answer");

    protected override TPart2Answer Part2Solver(TPart2InputParsed input)
    {
        throw new NotImplementedException("Part 2 Solver");
    }


    private static int ParseIntBits(ReadOnlySpan<char> bits)
    {
        int result = 0;
        foreach (var pow in Enumerable.Range(0, bits.Length))
        {
            result += (bits[^(1 + pow)] switch
            {
                '0' => 0,
                '1' => (int)Math.Pow(2, pow),
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        return result;
    }

    private static IReadOnlyList<IReadOnlyList<T>> Transpose<T>(IReadOnlyList<IReadOnlyList<T>> input)
    {
        var inputWidth = input.Max(i => i.Count);
        var inputHeight = input.Count;

        var resultWidth = inputHeight;
        var resultHeight = inputWidth;

        var resultRows = new List<IReadOnlyList<T>>();
        foreach (var resultRow in Enumerable.Range(0, resultHeight))
        {
            var row = new List<T>();
            foreach (var resultColumn in Enumerable.Range(0, resultWidth))
            {
                var inputRow = resultColumn;
                var inputColumn = resultRow;

                row.Add(input[inputRow][inputColumn]);
            }
            resultRows.Add(row.AsReadOnly());
        }

        return resultRows;
    }
}
