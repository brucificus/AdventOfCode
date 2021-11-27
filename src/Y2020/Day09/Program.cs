await NUnitApplication.CreateBuilder().Build().RunAsync();

public partial class Program
{
    private IReadOnlyCollection<string> inputLines = null!;

    public record InputValue(int Index, BigInteger Value);
    public record ContextualizedInputValue(InputValue Item, IReadOnlyList<InputValue> Preamble);

    const int part1ExpectedResult = 85848519;

    [SetUp]
    public async Task Setup()
    {
        inputLines = await new InputFileFacadeFacade().ReadAllLinesAsync();
    }

    [Test(ExpectedResult = part1ExpectedResult)]
    public int Part1()
    {
        var inputValues = ParseInputValues();

        const int preambleSize = 25;

        IReadOnlyList<InputValue> PreambleFor(InputValue item) =>
            Enumerable.Range(item.Index - preambleSize, preambleSize).Select(i => inputValues[i]).ToImmutableArray();

        var contextualizedInputValues = inputValues.Skip(preambleSize).Select(i => new ContextualizedInputValue(i, PreambleFor(i)));

        IEnumerable<(InputValue l, InputValue r)> CandidateAntecedentsFor(ContextualizedInputValue item) =>
            item.Preamble.SelectUniquePairs(i => i.Value).Where(i => i.Item1.Value + i.Item2.Value == item.Item.Value);

        bool HasCandidateAntecedentsFor(ContextualizedInputValue item) =>
            CandidateAntecedentsFor(item).Any();

        return (int)contextualizedInputValues.First(i => !HasCandidateAntecedentsFor(i)).Item.Value;
    }

    [Test(ExpectedResult = "13414198")]
    public string Part2()
    {
        var inputValues = ParseInputValues();

        const int desiredSum = part1ExpectedResult;

        IReadOnlyList<InputValue> FindContiguousRangeWithDesiredSum()
        {
            IReadOnlyList<InputValue> RangeOfInputValues(int start, int stop) =>
                Enumerable.Range(start, (stop - start) + 1).Select(i => inputValues[i]).ToImmutableArray();

            var rangeStart = 0;
            var rangeStop = 1;
            var range = RangeOfInputValues(rangeStart, rangeStop);
            var sum = range.Select(i => i.Value).Sum();

            do
            {
                // This only works because we know that an inherent property of the list
                //  is that all numbers _are positive_. (So summing more numbers always means a larger sum.)
                //
                // Also, it will explode if the desiredSum isn't found.

                if (sum > desiredSum)
                {
                    rangeStart++;
                }
                else if (sum < desiredSum)
                {
                    rangeStop++;
                }

                range = RangeOfInputValues(rangeStart, rangeStop);
                sum = range.Select(i => i.Value).Sum();
            } while (sum != desiredSum);

            return range;
        }

        var resultRange = FindContiguousRangeWithDesiredSum();

        return (resultRange.Min(x => x.Value) + resultRange.Max(x => x.Value)).ToString();
    }

    private IReadOnlyList<InputValue> ParseInputValues()
    {
        return inputLines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select((s, i) => new InputValue(i, BigInteger.Parse(s)))
            .ToImmutableList();
    }
}
