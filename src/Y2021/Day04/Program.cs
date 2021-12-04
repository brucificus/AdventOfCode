await NUnitApplication.CreateBuilder().Build().RunAsync();


[TestFixture(Description = "Day04")]
public partial class Program : TestableSolverBase<BingoRunInputs, Part1Answer, BingoRunInputs, Part2Answer>
{
    protected override BingoRunInputs ParseInputForPart1(IReadOnlyList<string> lines) =>
        BingoRunInputs.ParseLines(lines.ToImmutableList());

    protected override Part1Answer Part1AnswerSample => new(4512);
    protected override Part1Answer Part1AnswerActual => new(39984);

    protected override Part1Answer Part1Solver(BingoRunInputs input)
    {
        var (drawings, unmarkedCards) = input;

        var cards = unmarkedCards.Select(BingoCardRunState.StartingFor).ToImmutableArray();
        foreach (var drawing in drawings)
        {
            cards = cards.Select(c => c.WithMark(drawing)).ToImmutableArray();

            var possiblyWinningCards = cards.Where(c => c.Bingo).ToImmutableArray();
            if (possiblyWinningCards.Any())
            {
                var unmarkedValueSumsOfPossiblyWinningCards = possiblyWinningCards.Select(c => c.ValuesUnmarked.Select(v => v.Value).Sum()).ToImmutableArray();
                var scoresOfPossiblyWinningCards = unmarkedValueSumsOfPossiblyWinningCards.Select(s => s * drawing.Value).ToImmutableArray();
                var winningScore = scoresOfPossiblyWinningCards.Max();
                return new Part1Answer(winningScore);
            }
        }

        throw new InvalidOperationException("Unexpected fall-through");
    }

    protected override BingoRunInputs ParseInputForPart2(IReadOnlyList<string> lines) => ParseInputForPart1(lines);

    protected override Part2Answer Part2AnswerSample => new(1924);
    protected override Part2Answer Part2AnswerActual => new(8468);

    protected override Part2Answer Part2Solver(BingoRunInputs input)
    {
        var (drawings, unmarkedCards) = input;

        var cards = unmarkedCards.Select(BingoCardRunState.StartingFor).ToImmutableArray();
        var winningCards = ImmutableList<BingoCardRunState>.Empty;
        foreach (var drawing in drawings)
        {
            cards = cards.Select(c => c.WithMark(drawing)).ToImmutableArray();

            var possiblyWinningCards = cards.Where(c => c.Bingo).ToImmutableArray();
            winningCards = winningCards.AddRange(possiblyWinningCards);
            cards = cards.Except(possiblyWinningCards).ToImmutableArray();

            if (!cards.Any())
            {
                var mostLosingestCard = winningCards.Last();
                var unmarkedValueSumsMostLosingestCard = mostLosingestCard.ValuesUnmarked.Select(v => v.Value).Sum();
                var scoreOfMostLosingestCard = unmarkedValueSumsMostLosingestCard * drawing.Value;
                return new Part2Answer(scoreOfMostLosingestCard);
            }
        }

        throw new InvalidOperationException("Unexpected fall-through");
    }
}

public readonly record struct Part1Answer(int Value);
public readonly record struct Part2Answer(int Value);
