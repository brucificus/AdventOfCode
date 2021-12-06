using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<Undefined>;

await NUnitApplication.CreateBuilder().Build().RunAsync();


[TestFixture(Description = "Day05")]
public partial class Program : TestableSolverBase<SparsePositiveIntegerBoundedInfiniteRasterPlane<int>, Part1Answer, TPart2InputParsed, Part2Answer>
{
    protected override SparsePositiveIntegerBoundedInfiniteRasterPlane<int> ParseInputForPart1(IReadOnlyList<string> lines) =>
        SparsePositiveIntegerBoundedInfiniteRasterPlane<int>.Parse(lines);

    protected override Part1Answer Part1AnswerSample => new(5);
    protected override Part1Answer Part1AnswerActual => throw new NotImplementedException("Part 1 Actual Answer");

    protected override Part1Answer Part1Solver(SparsePositiveIntegerBoundedInfiniteRasterPlane<int> input)
    {
        var intersections = input.CreatePlanchette().AsEnumerable().Select(p => p.Peek()).Select(p => (point: p.Point, count: p.Cell.Count()));
        var answerValue = intersections.Where(p => p.count > 2).Count();
        return new Part1Answer(answerValue);
    }

    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) => default!;

    protected override Part2Answer Part2AnswerSample => throw new NotImplementedException("Part 2 Sample Answer");
    protected override Part2Answer Part2AnswerActual => throw new NotImplementedException("Part 2 Actual Answer");

    protected override Part2Answer Part2Solver(TPart2InputParsed input)
    {
        throw new NotImplementedException("Part 2 Solver");
    }
}

public readonly record struct Part1Answer(int Value);

public readonly record struct Part2Answer();
