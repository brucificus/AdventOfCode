using TPart1InputParsed = System.Collections.Generic.IReadOnlyList<MotionCommand>;
using TPart1Answer = System.Int32;
// TODO: Step 07: Set input/output types for Part 2.
using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;
using TPart2Answer = Undefined;

await NUnitApplication.CreateBuilder().Build().RunAsync();

public enum MotionDirection
{
    ForwardHorizontalIncrease,
    DownDepthIncrease,
    UpDepthDecrease
}

[TestFixture(Description = "Day02")]
public partial class Program : TestableSolverBase<TPart1InputParsed, TPart1Answer, TPart2InputParsed, TPart2Answer>
{
    protected override TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(MotionCommand.Parse);

    protected override TPart1Answer Part1AnswerSample => 150;
    // TODO: Step 06: Record challenge-confirmed discovered answer for Part 1. Commit.
    protected override TPart1Answer Part1AnswerActual => throw new NotImplementedException("Part 1 Actual Answer");

    protected override TPart1Answer Part1Solver(TPart1InputParsed input)
    {
        var origin = (depth: 0, horizontal: 0);

        var finalPosition = input.Aggregate(
            origin,
            (p, c) => c.Direction switch
            {
                MotionDirection.ForwardHorizontalIncrease => p with { horizontal = p.horizontal + (int)c.Units },
                MotionDirection.UpDepthDecrease => p with { depth = p.depth - (int)c.Units },
                MotionDirection.DownDepthIncrease => p with { depth = p.depth + (int)c.Units },
            });

        var coordinatesProduct = finalPosition.horizontal * finalPosition.depth;
        return coordinatesProduct;
    }
    // TODO: Step 05: Refine solver for Part 1's actual input data.



    // TODO: Step 08: Parse input for Part 2.
    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) => default!;

    // TODO: Step 09: Record challenge-provided answer for the sample for Part 2.
    protected override TPart2Answer Part2AnswerSample => throw new NotImplementedException("Part 2 Sample Answer");
    // TODO: Step 12: Record challenge-confirmed discovered answer for Part 2. Commit.
    protected override TPart2Answer Part2AnswerActual => throw new NotImplementedException("Part 2 Actual Answer");

    // TODO: Step 10: Write solver for Part 2's sample. Commit.
    protected override TPart2Answer Part2Solver(TPart2InputParsed input)
    {
        throw new NotImplementedException("Part 2 Solver");
    }
    // TODO: Step 11: Refine solver for Part 2's actual input data.
}

public record MotionCommand(MotionDirection Direction, uint Units)
{
    public static MotionCommand Parse(string line)
    {
        var lineSplit = line.Split(' ');
        var motionDirection = lineSplit[0] switch
        {
            "forward" => MotionDirection.ForwardHorizontalIncrease,
            "down" => MotionDirection.DownDepthIncrease,
            "up" => MotionDirection.UpDepthDecrease,
            _ => throw new ArgumentException()
        };
        var motionUnits = uint.Parse(lineSplit[1]);
        return new MotionCommand(motionDirection, motionUnits);
    }
}
