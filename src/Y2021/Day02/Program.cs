using TPart1InputParsed = System.Collections.Generic.IReadOnlyList<MotionCommand>;
using TPart1Answer = System.Int32;
using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<AimCommand>;
using TPart2Answer = System.Int32;

await NUnitApplication.CreateBuilder().Build().RunAsync();

[TestFixture(Description = "Day02")]
public partial class Program : TestableSolverBase<TPart1InputParsed, TPart1Answer, TPart2InputParsed, TPart2Answer>
{
    protected override TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(MotionCommand.Parse);

    protected override TPart1Answer Part1AnswerSample => 150;
    protected override TPart1Answer Part1AnswerActual => 1936494;

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
                _ => throw new ArgumentOutOfRangeException()
            });

        var coordinatesProduct = finalPosition.horizontal * finalPosition.depth;
        return coordinatesProduct;
    }



    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(AimCommand.Parse);

    protected override TPart2Answer Part2AnswerSample => 900;
    protected override TPart2Answer Part2AnswerActual => 1997106066;

    protected override TPart2Answer Part2Solver(TPart2InputParsed input)
    {
        var origin = (depth: 0, horizontal: 0, aim: 0);

        var finalPosition = input.Aggregate(
            origin,
            (tuple, command) => command.Direction switch
            {
                AimDirection.ForwardIncreaseHorizontalAndSkewDepth => tuple with
                {
                    horizontal = tuple.horizontal + (int)command.Units, depth = tuple.depth + (tuple.aim * (int)command.Units)
                },
                AimDirection.UpDecreaseAim => tuple with { aim = tuple.aim - (int)command.Units },
                AimDirection.DownIncreaseAim => tuple with { aim = tuple.aim + (int)command.Units },
                _ => throw new ArgumentOutOfRangeException()
            });

        var coordinatesProduct = finalPosition.horizontal * finalPosition.depth;
        return coordinatesProduct;
    }
}

public enum MotionDirection
{
    ForwardHorizontalIncrease,
    DownDepthIncrease,
    UpDepthDecrease
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
            _ => throw new ArgumentOutOfRangeException()
        };
        var motionUnits = uint.Parse(lineSplit[1]);
        return new MotionCommand(motionDirection, motionUnits);
    }
}

public enum AimDirection
{
    ForwardIncreaseHorizontalAndSkewDepth,
    DownIncreaseAim,
    UpDecreaseAim
}

public record AimCommand(AimDirection Direction, uint Units)
{
    public static AimCommand Parse(string line)
    {
        var lineSplit = line.Split(' ');
        var motionDirection = lineSplit[0] switch
        {
            "forward" => AimDirection.ForwardIncreaseHorizontalAndSkewDepth,
            "down" => AimDirection.DownIncreaseAim,
            "up" => AimDirection.UpDecreaseAim,
            _ => throw new ArgumentOutOfRangeException()
        };
        var motionUnits = uint.Parse(lineSplit[1]);
        return new AimCommand(motionDirection, motionUnits);
    }
}
