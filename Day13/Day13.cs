using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

public class Day13
{
    private IReadOnlyList<string> inputLines;

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test(ExpectedResult = 171)]
    public int Part1()
    {
        var (earliestTimestamp, busIds) = ParseInput();

        var busArrivals = busIds
                            .Select(b => (busId: b, delay: (((earliestTimestamp / b) + 1) * b) - earliestTimestamp))
                            .ToImmutableList();

        var nextArrival = busArrivals.OrderBy(b => b.delay).First();

        return nextArrival.busId * nextArrival.delay;
    }

    private (int earliestTimestamp, IReadOnlyList<int> busIds) ParseInput()
    {
        return (
            int.Parse(inputLines[0]),
            inputLines[1].Split(',').Where(i => i != "x").Select(int.Parse).ToImmutableList());
    }
}
