using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared.Mapping;

public class Day11
{
    private IPopulatedFullyBoundedPlane<int, char> _initialMap;
    const char emptySeat = 'L';
    const char occupiedSeat = '#';
    const char floor = '.';

    [SetUp]
    public async Task Setup()
    {
        _initialMap = await DenseFullyBoundedIntegralPlane<char>.FromFileAsync("input.txt", c => c);
    }

    [Test(ExpectedResult = 2113)]
    public int Part1()
    {
        var x = _initialMap.Count(i => i.cell == '.');

        return SeatingIterations().Last().Count(c => c.cell == occupiedSeat);
    }

    private IEnumerable<IPopulatedFullyBoundedPlane<int, char>> SeatingIterations()
    {
        int i = 0;
        var previousSeatingIteration = _initialMap;
        Print(i, previousSeatingIteration);

        yield return previousSeatingIteration;

        while (true)
        {
            i++;
            var newSeatingIteration = IterateSeatingRulesOnce(previousSeatingIteration);
            Print(i, newSeatingIteration);

            yield return newSeatingIteration;

            if (newSeatingIteration.Equals(previousSeatingIteration))
            {
                yield break;
            }

            previousSeatingIteration = newSeatingIteration;
        }
    }

    private static IPopulatedFullyBoundedPlane<int, char> IterateSeatingRulesOnce(IPopulatedFullyBoundedPlane<int, char> input)
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

    private static void Print(int i, IPopulatedFullyBoundedPlane<int, char> map)
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
