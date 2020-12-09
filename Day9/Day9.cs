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

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test(ExpectedResult = "85848519")]
    public string Part1()
    {
        var inputValues = inputLines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select((s, i) => new InputValue(i, BigInteger.Parse(s)))
            .ToImmutableList();

        const int preambleSize = 25;

        IEnumerable<InputValue> PreambleFor(InputValue item) =>
            Enumerable.Range(item.index - preambleSize, preambleSize).Select(i => inputValues[i]);

        var contextualizedInputValues = inputValues.Skip(preambleSize).Select(i => new ContextualizedInputValue(i, PreambleFor(i)));

        IEnumerable<(InputValue l, InputValue r)> CandidateAntecedentsFor(ContextualizedInputValue item) =>
            item.preamble.SelectUniquePairs(i => i.value).Where(i => i.Item1.value + i.Item2.value == item.item.value);

        bool HasCandidateAntecedentsFor(ContextualizedInputValue item) =>
            CandidateAntecedentsFor(item).Any();

        return contextualizedInputValues.First(i => !HasCandidateAntecedentsFor(i)).item.value.ToString();
    }
}
