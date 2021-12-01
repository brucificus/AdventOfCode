// TODO: Step 00: Have the project created in advance, using this file template and two blank text files for input.

// TODO: Step 01: Set input/output types for Part 1.
using TPart1InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;
using TPart1Answer = Undefined;
// TODO: Step 07: Set input/output types for Part 2.
using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;
using TPart2Answer = Undefined;


await NUnitApplication.CreateBuilder().Build().RunAsync();


[TestFixture(Description = "Day02")]
public partial class Program : TestableSolverBase<TPart1InputParsed, TPart1Answer, TPart2InputParsed, TPart2Answer>
{
    // TODO: Step 02: Parse input for Part 1.
    protected override TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(_ => default(Undefined));

    // TODO: Step 03: Record the challenge-provided answer for the sample for Part 1.
    protected override TPart1Answer Part1AnswerSample => throw new NotImplementedException("Part 1 Sample Answer");
    // TODO: Step 06: Record challenge-confirmed discovered answer for Part 1. Commit.
    protected override TPart1Answer Part1AnswerActual => throw new NotImplementedException("Part 1 Actual Answer");

    // TODO: Step 04: Write solver for Part 1's sample. Commit.
    protected override TPart1Answer Part1Solver(TPart1InputParsed input)
    {
        throw new NotImplementedException("Part 1 Solver");
    }
    // TODO: Step 05: Refine solver for Part 1's actual input data.



    // TODO: Step 08: Parse input for Part 2.
    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) =>
        ParseInputForPart1(lines);

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
