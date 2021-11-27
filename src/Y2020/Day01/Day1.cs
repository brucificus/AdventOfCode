using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;
using static System.IO.File;

[TestFixture]
public class Day1
{
    private IEnumerable<int> values;

    [SetUp]
    public async Task SetUp()
    {
        values = (await ReadAllLinesAsync("input.txt"))
                .Where(v => v is string { Length: > 0 })
                .Select(v => int.Parse(v));
    }

    [Test(ExpectedResult = 786811)]
    public int Part1()
    {
        var pairs = values.SelectUniquePairs();

        return pairs.Where(p => p.Item1 + p.Item2 == 2020).Select(p => p.Item1 * p.Item2).First();
    }

    [Test(ExpectedResult = 199068980)]
    public int Part2()
    {
        var triplets = values.SelectUniqueTriplets();

        return triplets.Where(p => p.Item1 + p.Item2 + p.Item3 == 2020).Select(p => p.Item1 * p.Item2 * p.Item3).First();
    }
}
