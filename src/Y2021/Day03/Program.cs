using TPart1InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;
using TPart1Answer = Undefined;
using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;
using TPart2Answer = Undefined;


await NUnitApplication.CreateBuilder().Build().RunAsync();


[TestFixture(Description = "Day03")]
public partial class Program : TestableSolverBase<TPart1InputParsed, TPart1Answer, TPart2InputParsed, TPart2Answer>
{
    protected override TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(_ => default(Undefined));

    protected override TPart1Answer Part1AnswerSample => throw new NotImplementedException("Part 1 Sample Answer");
    protected override TPart1Answer Part1AnswerActual => throw new NotImplementedException("Part 1 Actual Answer");

    protected override TPart1Answer Part1Solver(TPart1InputParsed input)
    {
        throw new NotImplementedException("Part 1 Solver");
    }



    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) => default!;

    protected override TPart2Answer Part2AnswerSample => throw new NotImplementedException("Part 2 Sample Answer");
    protected override TPart2Answer Part2AnswerActual => throw new NotImplementedException("Part 2 Actual Answer");

    protected override TPart2Answer Part2Solver(TPart2InputParsed input)
    {
        throw new NotImplementedException("Part 2 Solver");
    }
}
