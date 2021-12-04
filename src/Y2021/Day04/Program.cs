using System.Runtime.InteropServices.ComTypes;
using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;
using TPart2Answer = Undefined;


await NUnitApplication.CreateBuilder().Build().RunAsync();


[TestFixture(Description = "Day04")]
public partial class Program : TestableSolverBase<BingoRunInputs, Part1Answer, TPart2InputParsed, TPart2Answer>
{
    protected override BingoRunInputs ParseInputForPart1(IReadOnlyList<string> lines) =>
        BingoRunInputs.ParseLines(lines.ToImmutableList());

    protected override Part1Answer Part1AnswerSample => new(4512);
    protected override Part1Answer Part1AnswerActual => throw new NotImplementedException("Part 1 Actual Answer");

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

    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) => default!;

    protected override TPart2Answer Part2AnswerSample => throw new NotImplementedException("Part 2 Sample Answer");
    protected override TPart2Answer Part2AnswerActual => throw new NotImplementedException("Part 2 Actual Answer");

    protected override TPart2Answer Part2Solver(TPart2InputParsed input)
    {
        throw new NotImplementedException("Part 2 Solver");
    }
}

public readonly record struct Part1Answer(int Value);
