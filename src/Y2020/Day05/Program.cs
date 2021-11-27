await NUnitApplication.CreateBuilder().Build().RunAsync();

[TestFixture]
public partial class Program
{
    private IReadOnlyList<string> input = null!;

    [SetUp]
    public async Task SetUp()
    {
        input = await new InputFileFacade().ReadAllLinesAsync().Where(l => l.Length == 10);
    }

    [Test(ExpectedResult = 908)]
    public int Part1()
    {
        return GetSeatCoordinatesWithIdsDescending().First().seatId;
    }

    [Test(ExpectedResult = 619)]
    public int Part2()
    {
        var seatsPresent = GetSeatCoordinatesWithIdsDescending().Select(s => s.seatId).ToHashSet();
        return seatsPresent.First(s => !seatsPresent.Contains(s - 1) && seatsPresent.Contains(s - 2)) - 1;
    }

    private IEnumerable<(string rowCoordinate, string columnCoordinate, int seatId)> GetSeatCoordinatesWithIdsDescending()
    {
        var seatCoordinates = input.Select(l => (rowCoordinate: l.Substring(0, 7), columnCoordinate: l.Substring(7, 3)));

        int CalculateRowValue(string rowCoordinate)
        {
            (int, int) Aggregate((int, int) previous, char current)
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
            return rowCoordinate.Aggregate(valueRange, Aggregate).Item1;
        }

        int CalculateColumnValue(string columnCoordinate)
        {
            (int, int) Aggregate((int, int) previous, char current)
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
            return columnCoordinate.Aggregate(valueRange, Aggregate).Item1;
        }

        return from s in seatCoordinates
            let rowNumber = CalculateRowValue(s.rowCoordinate)
            let columnNumber = CalculateColumnValue(s.columnCoordinate)
            let seatId = (rowNumber * 8) + columnNumber
            orderby seatId descending
            select (s.rowCoordinate, s.columnCoordinate, seatId);
    }
}
