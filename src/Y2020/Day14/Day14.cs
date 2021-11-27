using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

public class Day14
{
    private IReadOnlyCollection<string> inputLines;

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test]
    public void Part1()
    {
        Assert.Pass();
    }
}
