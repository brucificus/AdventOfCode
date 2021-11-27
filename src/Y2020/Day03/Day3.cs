using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;
using Shared.Mapping;
using TobogganPosition = Shared.Mapping.ISemisesquiBoundedInfiniteIntegralPlane<char>.IPlanchette;

[TestFixture]
public class Day3
{
    const char tree = '#';
    private ISemisesquiBoundedInfiniteIntegralPlane<char> map;
    private Slope part1Slope;
    private TobogganPosition tobogganPosition;

    [SetUp]
    public async Task SetUp()
    {
        map = await TextualSemisesquiBoundedInfiniteIntegralPlane.FromFileAsync("input.txt");
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
