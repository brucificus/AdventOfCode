await NUnitApplication.CreateBuilder().Build().RunAsync();

public readonly record struct Part1InputParsed;
public readonly record struct Part1Answer;
public readonly record struct Part2InputParsed;
public readonly record struct Part2Answer;

[TestFixture(Description = "Day07")]
public partial class Program : TestableSolverBase<Part1InputParsed, Part1Answer, Part2InputParsed, Part2Answer>
{
    protected override Part1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        throw new IgnoreException("NotImplemented: Part 1 Input Parser");

    protected override Part1Answer Part1Solver(Part1InputParsed input)
    {
        throw new IgnoreException("NotImplemented: Part 1 Solver");
    }

    protected override Part1Answer Part1AnswerSample => throw new IgnoreException("NotImplemented: Part 1 Sample Answer");

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
