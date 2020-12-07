using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;

public class Day7
{
    private IReadOnlyList<string> inputLines;
    public record BagColor(string value);
    public record BagGrouping(int count, BagColor color);
    public record BagContentsPattern(BagColor color, IReadOnlyCollection<BagGrouping> bagGroupings);

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test]
    public void Part1()
    {
        var arrow = new Regex("\\sbags\\scontain\\s");
        var nonEmptyBagTrailer = new Regex("(\\d+)\\s((\\w+\\s)+)bags?(\\.|,\\s)");
        var emptyBagTrailer = new Regex("no\\sother\\sbags\\.");
        IEnumerable<BagGrouping> CaptureBagGroupings(MatchCollection matches)
        {
            foreach (var match in matches.Cast<Match>())
            {
                yield return new BagGrouping(int.Parse(match.Groups[1].Value), new BagColor(match.Groups[2].Value));
            }
        }

        var parsedInputLines =
            (from line in inputLines
             let lineSides = line.Split(arrow)
             let emptyBagTrailerMatch = lineSides[1].Matches(emptyBagTrailer).SingleOrDefault()
             let nonEmptyBagTrailerMatches = lineSides[1].Matches(nonEmptyBagTrailer)
             select emptyBagTrailerMatch != null
                    ? new BagContentsPattern(new BagColor(lineSides[0]), ImmutableList<BagGrouping>.Empty)
                    : new BagContentsPattern(new BagColor(lineSides[0]), CaptureBagGroupings(nonEmptyBagTrailerMatches).ToImmutableList()))
            .ToImmutableList();
    }
}
