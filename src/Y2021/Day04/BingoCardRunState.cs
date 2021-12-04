public readonly record struct BingoCardRunState(BingoCard BingoCard, ImmutableList<BingoCellValue> ValuesMarked)
{
    public ImmutableArray<int> RowLengths { get; private init; } =
        Enumerable.Repeat(BingoCard.ColumnCount, BingoCard.RowCount).ToImmutableArray();

    public ImmutableArray<int> ColumnHeights { get; private init; } =
        Enumerable.Repeat(BingoCard.RowCount, BingoCard.ColumnCount).ToImmutableArray();

    public ImmutableHashSet<BingoCellValue> ValuesUnmarked { get; private init; } =
        BingoCard.CellsValueIndex.Keys.Except(ValuesMarked).ToImmutableHashSet();

    public bool Bingo { get; private init; } = false;

    public static BingoCardRunState StartingFor(BingoCard bingoCard) =>
        new(bingoCard, ImmutableList<BingoCellValue>.Empty);

    public BingoCardRunState WithMark(BingoCellValue newMark)
    {
        var result = this;
        result = result with { ValuesMarked = ValuesMarked.Add(newMark) };

        if (BingoCard.CellsValueIndex.TryGetValue(newMark, out var newMarkCoordinate))
        {
            result = result with { RowLengths = result.RowLengths.SetItem(newMarkCoordinate.Row, result.RowLengths[newMarkCoordinate.Row] - 1) };
            result = result with { ColumnHeights = result.ColumnHeights.SetItem(newMarkCoordinate.Column, result.ColumnHeights[newMarkCoordinate.Column] - 1) };
            result = result with { ValuesUnmarked = result.ValuesUnmarked.Except(new [] { newMark }) };
            result = result with { Bingo = result.RowLengths.Any(r => r == 0) || result.ColumnHeights.Any(c => c == 0) };
        }

        return result;
    }
}
