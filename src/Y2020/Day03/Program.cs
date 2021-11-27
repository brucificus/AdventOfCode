using AdventOfCode.Y2020.Shared.Mapping;
using TobogganPosition = AdventOfCode.Y2020.Shared.Mapping.ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette;

await NUnitApplication.CreateBuilder().Build().RunAsync();

[TestFixture]
public partial class Program
{
    const char tree = '#';
    private ISemisesquiBoundedInfiniteIntegralPlane<char> map = null!;
    private Slope part1Slope = null!;
    private TobogganPosition tobogganPosition = null!;

    [SetUp]
    public async Task SetUp()
    {
        var lines = await new InputFileFacadeFacade().ReadAllLinesAsync().WhereNot(string.IsNullOrWhiteSpace);
        map = TextualSemisesquiBoundedInfiniteIntegralPlane.FromLines(lines);
        part1Slope = new Slope(3, 1);
        tobogganPosition = map.Origin;
    }

    [Test(ExpectedResult = 216)]
    public int Part1()
    {
        return tobogganPosition.RepeatedlySlideToBottomOfMap<int>(part1Slope, 0, CountTrees);
    }

    [Test(ExpectedResult = "6708199680")]
    public string Part2()
    {
        var part2Slopes = new Slope[]
        {
            new Slope(1, 1),
            part1Slope,
            new Slope(5, 1),
            new Slope(7, 1),
            new Slope(1, 2)
        };

        var part2TreeCounts = part2Slopes.Select(s => tobogganPosition.RepeatedlySlideToBottomOfMap<int>(s, 0, CountTrees));

        return part2TreeCounts.Multiply().ToString();
    }

    private int CountTrees(int p, TobogganPosition c) => c.Peek() == tree ? p + 1 : p;
}
