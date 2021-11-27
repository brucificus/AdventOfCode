await NUnitApplication.CreateBuilder().Build().RunAsync();

public partial class Program
{
    private IReadOnlyList<string> blocks = null!;

    public record Passenger(ImmutableHashSet<char> Answers);
    public record PassengerGroup(ImmutableList<Passenger> Passengers);
    public record PassengerGroupSummary(ImmutableHashSet<char> Answers);

    [SetUp]
    public async Task Setup()
    {
        blocks = await new InputFileFacadeFacade().ReadAllTextAsync().SplitOnEmptyLines();
    }

    [Test(ExpectedResult = 6726)]
    public int Part1()
    {
        var passengerGroups = ParsePassengerGroups();
        var passengerGroupSummaries =
            (from passengerGroup in passengerGroups
                let summaryData = passengerGroup.Passengers.Aggregate(ImmutableHashSet<char>.Empty, (p, c) => p.Union(c.Answers))
                select new PassengerGroupSummary(summaryData))
            .ToImmutableList();
        return passengerGroupSummaries.Sum(pgs => pgs.Answers.Count);
    }

    [Test(ExpectedResult = 3316)]
    public int Part2()
    {
        var passengerGroups = ParsePassengerGroups();
        var passengerGroupSummaries =
            (from passengerGroup in passengerGroups
                let firstPassenger = passengerGroup.Passengers.First()
                let remainingPassengers = passengerGroup.Passengers.Skip(1)
                let summaryData = remainingPassengers.Aggregate(firstPassenger.Answers, (p, c) => p.Intersect(c.Answers))
                select new PassengerGroupSummary(summaryData))
            .ToImmutableList();
        return passengerGroupSummaries.Sum(pgs => pgs.Answers.Count);
    }

    private ImmutableList<PassengerGroup> ParsePassengerGroups()
    {
        return (from block in blocks
                let records = (from line in block.Trim().SplitOnNewlines()
                        select new Passenger(line.ToImmutableHashSet()))
                    .ToImmutableList()
                select new PassengerGroup(records))
            .ToImmutableList();
    }
}
