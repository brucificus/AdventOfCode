public record BingoRunInputs(IReadOnlyList<BingoCellValue> Drawings, IReadOnlyList<BingoCard> Cards)
{
    public static BingoRunInputs ParseLines(ImmutableList<string> lines)
    {
        var lineGroupings = lines.GroupBySubdividingEmptyLines();

        var drawings = lineGroupings[0].Single().Split(',').Select(s => new BingoCellValue(int.Parse(s))).ToImmutableArray();

        var cards = ImmutableList<BingoCard>.Empty;
        foreach (var cardLinesGrouping in lineGroupings.Skip(1))
        {
            cardLinesGrouping.Length.Should().Be(BingoCard.RowCount);
            cards = cards.Add(BingoCard.ParseLines(cardLinesGrouping));
        }

        return new BingoRunInputs(drawings, cards);
    }
}
