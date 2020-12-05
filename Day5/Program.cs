using System;
using System.Linq;
using static System.Console;

var input = (await System.IO.File.ReadAllLinesAsync("input.txt")).Where(l => l.Length == 10);

var seatCoordinates = input.Select(l => (rowCoordinate: l.Substring(0, 7), columnCoordinate: l.Substring(7, 3)));

int calculateRowValue(string rowCoordinate)
{
    (int, int) aggregate((int, int) previous, char current)
    {
        var halfSize = (previous.Item2 - previous.Item1) / 2;
        return current switch
        {
            'F' => (previous.Item1, previous.Item1 + halfSize),
            'B' => (previous.Item1 + halfSize + 1, previous.Item2),
            _ => throw new ArgumentException(nameof(rowCoordinate))
        };
    }

    var valueRange = ((int)0, (int)127);
    return rowCoordinate.Aggregate(valueRange, aggregate).Item1;
}

int calculateColumnValue(string columnCoordinate)
{
    (int, int) aggregate((int, int) previous, char current)
    {
        var halfSize = (previous.Item2 - previous.Item1) / 2;
        return current switch
        {
            'L' => (previous.Item1, previous.Item1 + halfSize),
            'R' => (previous.Item1 + halfSize + 1, previous.Item2),
            _ => throw new ArgumentException(nameof(columnCoordinate))
        };
    }

    var valueRange = ((int)0, (int)7);
    return columnCoordinate.Aggregate(valueRange, aggregate).Item1;
}

var seatCoordinatesWithIdDescending = from s in seatCoordinates
                                let rowNumber = calculateRowValue(s.rowCoordinate)
                                let columnNumber = calculateColumnValue(s.columnCoordinate)
                                let seatId = (rowNumber * 8) + columnNumber
                                orderby seatId descending
                                select (s.rowCoordinate, s.columnCoordinate, seatId);

var part1Answer = seatCoordinatesWithIdDescending.First().seatId;

await Out.WriteLineAsync($"Part 1 Answer: {part1Answer}");
