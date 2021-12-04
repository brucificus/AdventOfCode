public readonly record struct BingoCellValue(int Value);

public readonly record struct BingoCellCoordinate(int Row, int Column);

public readonly record struct BingoCell(BingoCellCoordinate Coordinate, BingoCellValue Value)
{
    public BingoCell(int row, int column, int value)
        : this(new BingoCellCoordinate(row, column), new BingoCellValue(value))
    {
    }
}

public record BingoCard(ImmutableArray<BingoCell> Cells)
{
    public const int RowCount = 5;

    public const int ColumnCount = 5;

    public ImmutableDictionary<BingoCellValue, BingoCellCoordinate> CellsValueIndex { get; } =
        Cells.ToImmutableDictionary(c => c.Value, c => c.Coordinate);

    public static BingoCard ParseLines(IReadOnlyList<string> lines)
    {
        var cellTuples = lines.Select(l => l.SplitOnWhitespace().Select(int.Parse)).ToImmutableArray().Disassemble();

        var cells =
            cellTuples
                .Select<(int major, int minor, int value), BingoCell>(p => new(p.major, p.minor, p.value))
                .ToImmutableArray();

        return new BingoCard(cells);
    }

    public BingoCardRunState WithMark(BingoCellValue newMark) =>
        BingoCardRunState.StartingFor(this).WithMark(newMark);
}
