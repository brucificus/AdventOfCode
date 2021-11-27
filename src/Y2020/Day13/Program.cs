await NUnitApplication.CreateBuilder().Build().RunAsync();

public partial class Program
{
    public readonly record struct Any;
    private IReadOnlyList<string> inputLines = null!;

    [SetUp]
    public async Task Setup()
    {
        inputLines = await new InputFileFacadeFacade().ReadAllLinesAsync();
    }

    [Test(ExpectedResult = 171)]
    public int Part1()
    {
        var (earliestTimestamp, busIds) = ParseInputForPart1();

        var busArrivals = busIds
            .Select(b => (busId: b, delay: (((earliestTimestamp / b) + 1) * b) - earliestTimestamp))
            .ToImmutableList();

        var nextArrival = busArrivals.OrderBy(b => b.delay).First();

        return nextArrival.busId * nextArrival.delay;
    }

    [Test(ExpectedResult = "539746751134958")]
    public string Part2()
    {
        var busIdPatterns = ParseInputForPart2();

        var busIdOffsets =
            (from offset in Enumerable.Range(0, busIdPatterns.Count)
                let busIdPattern = busIdPatterns[offset]
                where busIdPattern.Match(_ => true, _ => false)
                let busId = (BigInteger)busIdPattern.Match(n => n, _ => throw new InvalidOperationException())
                select (offset, busId))
            .ToImmutableArray();

        BigInteger position = 0;
        BigInteger productOfFoundBusIds = busIdOffsets.First().busId;

        foreach (var (offset, busId) in busIdOffsets.Skip(1))
        {
            while ((position + offset) % busId != 0)
            {
                position += productOfFoundBusIds;
            }

            productOfFoundBusIds *= busId;
        }

        return position.ToString();
    }

    private (int earliestTimestamp, IReadOnlyList<int> busIds) ParseInputForPart1()
    {
        return (
            int.Parse(inputLines[0]),
            inputLines[1].Split(',').Where(i => i != "x").Select(int.Parse).ToImmutableList());
    }

    private IReadOnlyList<OneOf<int, Any>> ParseInputForPart2()
    {
        return inputLines[1]
            .Split(',')
            .Select(v => (OneOf<int, Any>)
#pragma warning disable SA1119 // Statement should not use unnecessary parenthesis
                (v switch {
                    "x" => new Any(),
                    string n => int.Parse(n),
                    _ => throw new System.InvalidOperationException()
                }))
#pragma warning restore SA1119 // Statement should not use unnecessary parenthesis
            .ToImmutableList();
    }
}
