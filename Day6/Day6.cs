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
        var passengerGroups = ParsePassengerGroups();
        var passengerGroupSummaries =
                                (from passengerGroup in passengerGroups
                                 let summaryData = passengerGroup.passengers.Aggregate(ImmutableHashSet<char>.Empty, (p, c) => p.Union(c.answers))
                                 select new PassengerGroupSummary(summaryData))
                                .ToImmutableList();
        return passengerGroupSummaries.Sum(pgs => pgs.answers.Count);
    }

    [Test(ExpectedResult = 3316)]
    public int Part2()
    {
        var passengerGroups = ParsePassengerGroups();
        var passengerGroupSummaries =
                                (from passengerGroup in passengerGroups
                                 let firstPassenger = passengerGroup.passengers.First()
                                 let remainingPassengers = passengerGroup.passengers.Skip(1)
                                 let summaryData = remainingPassengers.Aggregate(firstPassenger.answers, (p, c) => p.Intersect(c.answers))
                                 select new PassengerGroupSummary(summaryData))
                                .ToImmutableList();
        return passengerGroupSummaries.Sum(pgs => pgs.answers.Count);
    }

    private ImmutableList<PassengerGroup> ParsePassengerGroups()
    {
        return (from block in input.SplitOnEmptyLines()
                let records = (from line in block.Trim().SplitOnNewlines()
                               select new Passenger(line.ToImmutableHashSet()))
                              .ToImmutableList()
                select new PassengerGroup(records))
                              .ToImmutableList();
    }
}
