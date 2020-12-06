using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;

public class Day6
{
    private string input;

    public record Passenger(ImmutableHashSet<char> answers);
    public record PassengerGroup(ImmutableList<Passenger> passengers);
    public record PassengerGroupSummary(ImmutableHashSet<char> answers);

    [SetUp]
    public async Task Setup()
    {
        input = await System.IO.File.ReadAllTextAsync("input.txt");
    }

    [Test(ExpectedResult = 6726)]
    public int Part1()
    {
        var passengerGroups = (from block in input.SplitOnEmptyLines()
                               let records = (from line in block.SplitOnNewlines()
                                              select new Passenger(line.ToImmutableHashSet()))
                                             .ToImmutableList()
                               select new PassengerGroup(records))
                              .ToImmutableList();
        var passengerGroupSummaries =
                                (from passengerGroup in passengerGroups
                                 let summaryData = passengerGroup.passengers.Aggregate(ImmutableHashSet<char>.Empty, (p, c) => p.Union(c.answers))
                                 select new PassengerGroupSummary(summaryData))
                                .ToImmutableList();
        return passengerGroupSummaries.Sum(pgs => pgs.answers.Count);
    }

    [Test]
    public void Part2()
    {
    }
}
