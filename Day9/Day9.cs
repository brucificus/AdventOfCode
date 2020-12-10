using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;

public class Day9
{
    private IReadOnlyCollection<string> inputLines;

    public record InputValue(int index, BigInteger value);
    public record ContextualizedInputValue(InputValue item, IEnumerable<InputValue> preamble);

    const int part1ExpectedResult = 85848519;

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test(ExpectedResult = part1ExpectedResult)]
    public int Part1()
    {
        var inputValues = ParseInputValues();

        const int preambleSize = 25;

        IEnumerable<InputValue> PreambleFor(InputValue item) =>
            Enumerable.Range(item.index - preambleSize, preambleSize).Select(i => inputValues[i]);

        var contextualizedInputValues = inputValues.Skip(preambleSize).Select(i => new ContextualizedInputValue(i, PreambleFor(i)));

        IEnumerable<(InputValue l, InputValue r)> CandidateAntecedentsFor(ContextualizedInputValue item) =>
            item.preamble.SelectUniquePairs(i => i.value).Where(i => i.Item1.value + i.Item2.value == item.item.value);

        bool HasCandidateAntecedentsFor(ContextualizedInputValue item) =>
            CandidateAntecedentsFor(item).Any();

        return (int)contextualizedInputValues.First(i => !HasCandidateAntecedentsFor(i)).item.value;
    }

    [Test(ExpectedResult = "13414198")]
    public string Part2()
    {
        var inputValues = ParseInputValues();

        const int desiredSum = part1ExpectedResult;

        IEnumerable<InputValue> FindContiguousRangeWithDesiredSum()
        {
            IEnumerable<InputValue> RangeOfInputValues(int start, int stop) =>
                Enumerable.Range(start, (stop - start) + 1).Select(i => inputValues[i]);

            var rangeStart = 0;
            var rangeStop = 1;
            var range = RangeOfInputValues(rangeStart, rangeStop);
            var sum = range.Select(i => i.value).Sum();

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
                sum = range.Select(i => i.value).Sum();
            } while (sum != desiredSum);

            return range;
        }

        var resultRange = FindContiguousRangeWithDesiredSum();

        return (resultRange.Min(x => x.value) + resultRange.Max(x => x.value)).ToString();
    }

    private IReadOnlyList<InputValue> ParseInputValues()
    {
        return inputLines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select((s, i) => new InputValue(i, BigInteger.Parse(s)))
            .ToImmutableList();
    }
}
