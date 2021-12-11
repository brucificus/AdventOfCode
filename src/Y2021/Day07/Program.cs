await NUnitApplication.CreateBuilder().Build().RunAsync();

public readonly record struct Part1InputParsed(ImmutableArray<int> Values);
public readonly record struct Part1Answer(int Value);
public readonly record struct Part2InputParsed;
public readonly record struct Part2Answer;

[TestFixture(Description = "Day07")]
public partial class Program : TestableSolverBase<Part1InputParsed, Part1Answer, Part2InputParsed, Part2Answer>
{
    protected override Part1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        new(lines.First().Split(',').Select(int.Parse).ToImmutableArray());

    protected override Part1Answer Part1Solver(Part1InputParsed input)
    {
        PositiveSlopeTrickFunction<int> CrabMovementFuelCostFunction(int startingPosition) =>
            new(-1, startingPosition, new[] { (startingPosition, (nuint)2) });

        PositiveSlopeTrickFunction<int> AllCrabsMovementFuelCostFunction = input.Values
            .Select(CrabMovementFuelCostFunction)
            .Aggregate(PositiveSlopeTrickFunction<int>.AdditiveIdentity, (p, c) => p + c);

        var intervalStart = AllCrabsMovementFuelCostFunction.SlopeChanges.Min(sc => sc.X) - 1;
        var slopes =
            AllCrabsMovementFuelCostFunction
                .SlopeChanges
                .Scan(
                    seed: (slope: (int)AllCrabsMovementFuelCostFunction.M0, x: intervalStart),
                    accumulator: (p, c) => (slope: p.slope + (int)c.SlopeChange, x: c.X));

        var firstSlopeSignChange =
                slopes
                .BufferWithoutPartials(2,1).Where(b => Math.Sign(b[1].slope) != Math.Sign(b[0].slope)).Select(b => b[1].x).First();

        CrabMovementFuelCostFunction(0).Calculate(0).Should().Be(0);
        CrabMovementFuelCostFunction(1).Calculate(1).Should().Be(0);
        CrabMovementFuelCostFunction(2).Calculate(2).Should().Be(0);

        CrabMovementFuelCostFunction(1).Calculate(2).Should().Be(1);
        CrabMovementFuelCostFunction(16).Calculate(2).Should().Be(14);
        CrabMovementFuelCostFunction(1).Calculate(2).Should().Be(1);
        CrabMovementFuelCostFunction(2).Calculate(2).Should().Be(0);
        CrabMovementFuelCostFunction(0).Calculate(2).Should().Be(2);
        CrabMovementFuelCostFunction(4).Calculate(2).Should().Be(2);
        CrabMovementFuelCostFunction(2).Calculate(2).Should().Be(0);
        CrabMovementFuelCostFunction(7).Calculate(2).Should().Be(5);
        CrabMovementFuelCostFunction(2).Calculate(2).Should().Be(0);
        CrabMovementFuelCostFunction(14).Calculate(2).Should().Be(12);

        var n = CrabMovementFuelCostFunction(16) +
                CrabMovementFuelCostFunction(1) +
                CrabMovementFuelCostFunction(2) +
                CrabMovementFuelCostFunction(0) +
                CrabMovementFuelCostFunction(4) +
                CrabMovementFuelCostFunction(2) +
                CrabMovementFuelCostFunction(7) +
                CrabMovementFuelCostFunction(1) +
                CrabMovementFuelCostFunction(2) +
                CrabMovementFuelCostFunction(14);

        n.Calculate(2).Should().Be(37);
        n.Calculate(1).Should().Be(41);
        n.Calculate(3).Should().Be(39);
        n.Calculate(10).Should().Be(71);

        return new Part1Answer(AllCrabsMovementFuelCostFunction.Calculate(firstSlopeSignChange));
    }

    protected override Part1Answer Part1AnswerSample => new(37);

    protected override Part1Answer Part1AnswerActual => throw new IgnoreException("NotImplemented: Part 1 Actual Answer");



    protected override Part2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) =>
        throw new IgnoreException("NotImplemented: Part 2 Input Parser");

    protected override Part2Answer Part2Solver(Part2InputParsed input)
    {
        throw new IgnoreException("NotImplemented: Part 2 Solver");
    }

    protected override Part2Answer Part2AnswerSample => throw new IgnoreException("NotImplemented: Part 2 Sample Answer");

    protected override Part2Answer Part2AnswerActual => throw new IgnoreException("NotImplemented: Part 2 Actual Answer");
}
