using AdventOfCode.Y2020.Shared.Mapping;
using Map = AdventOfCode.Y2020.Shared.Mapping.IPopulatedFullyBoundedPlane<int, char>;

namespace AdventOfCode.Y2020.Day11;

public class Day11
{
    private static readonly IEnumerable<Vector2<int>> VisibilityRuleDirections = Enumerable.Range(-1, 3).SelectMany(y => Enumerable.Range(-1, 3).Where(x => y != 0 || x != 0).Select(x => new Vector2<int>(x, y))).ToArray();

    private Map _initialMap = null!;
    const char emptySeat = 'L';
    const char occupiedSeat = '#';
    const char floor = '.';

    [SetUp]
    public async Task Setup()
    {
        static char ParseCell(char c) => c;
        _initialMap = DenseFullyBoundedIntegralPlane<char>.FromLines(await new InputFileFacadeFacade().ReadAllLinesAsync(), ParseCell);
    }

    [Test(ExpectedResult = 2113)]
    public int Part1()
    {
        return SeatingIterations(ProximitySeatingRule).Last().Count(c => c.cell == occupiedSeat);
    }

    [Test(ExpectedResult = 1865)]
    public int Part2()
    {
        return SeatingIterations(VisibilitySeatingRule).Last().Count(c => c.cell == occupiedSeat);
    }

    private IEnumerable<Map> SeatingIterations(Func<Map, Map> iterationRule)
    {
        int i = 0;
        var previousSeatingIteration = _initialMap;
        Print(i, previousSeatingIteration);

        yield return previousSeatingIteration;

        while (true)
        {
            i++;
            var newSeatingIteration = iterationRule(previousSeatingIteration);
            Print(i, newSeatingIteration);

            yield return newSeatingIteration;

            if (newSeatingIteration.Equals(previousSeatingIteration))
            {
                yield break;
            }

            previousSeatingIteration = newSeatingIteration;
        }
    }

    private static Map VisibilitySeatingRule(Map input)
    {
        var newMapCells =
            from center in input
            let seenSeats = from direction in VisibilityRuleDirections
                let ray = input.CastRay(center.coordinate, direction)
                let firstSeatInRay = ray.Cast<(Vector2<int> coordinate, char cell)?>().FirstOrDefault(c => c.Value.cell != floor)
                where firstSeatInRay.HasValue
                select firstSeatInRay.Value
            let currentSeatState = center.cell
            let seenSeatsOccupied = seenSeats.Count(c => c.cell == occupiedSeat)
            let newSeatState = currentSeatState switch
            {
                emptySeat => seenSeatsOccupied == 0 ? occupiedSeat : currentSeatState,
                occupiedSeat => seenSeatsOccupied >= 5 ? emptySeat : currentSeatState,
                floor => floor,
                _ => throw new InvalidOperationException()
            }
            select (center.coordinate, cell: newSeatState);
        return DenseFullyBoundedIntegralPlane<char>.FromTuples(newMapCells);
    }

    private static Map ProximitySeatingRule(Map input)
    {
        var newMapCells =
            from window in input.Scan(new Vector2<int>(3, 3))
            let center = window.Single(c => c.coordinate == window.Center)
            let currentSeatState = center.cell
            let surrounding = window.Where(c => c.coordinate != window.Center)
            let surroundingOccupied = surrounding.Count(c => c.cell == occupiedSeat)
            let newSeatState = currentSeatState switch
            {
                emptySeat => surroundingOccupied == 0 ? occupiedSeat : currentSeatState,
                occupiedSeat => surroundingOccupied >= 4 ? emptySeat : currentSeatState,
                floor => floor,
                _ => throw new InvalidOperationException()
            }
            select (center.coordinate, cell: newSeatState);
        return DenseFullyBoundedIntegralPlane<char>.FromTuples(newMapCells);
    }

    private static void Print(int i, Map map)
    {
        TestContext.Out.WriteLine(i);
        TestContext.Out.WriteLine(Enumerable.Repeat('=', map.Size.x).ToArray());

        var output = map.OrderBy(x => x.coordinate.y).Buffer(map.Size.x).Select(row => row.OrderBy(c => c.coordinate.x).Select(c => c.cell).ToArray()).ToArray();
        foreach (var row in output)
        {
            TestContext.Out.WriteLine(row);
        }

        if (i > 0)
        {
            TestContext.Out.WriteLine();
        }
    }
}
