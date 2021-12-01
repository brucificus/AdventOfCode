using TPart1InputParsed = System.Collections.Generic.IReadOnlyList<int>;
using TPart1Answer = System.Int32;
using TPart2InputParsed = System.Collections.Generic.IReadOnlyList<int>;
using TPart2Answer = System.Int32;


await NUnitApplication.CreateBuilder().Build().RunAsync();


[TestFixture(Description = "Day01")]
public partial class Program : TestableSolverBase<TPart1InputParsed, TPart1Answer, TPart2InputParsed, TPart2Answer>
{
    protected override TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        lines.WhereNot(string.IsNullOrEmpty).Select(int.Parse);

    protected override TPart1Answer Part1AnswerSample => 7;
    protected override TPart1Answer Part1AnswerActual => 1162;

    protected override TPart1Answer Part1Solver(IReadOnlyList<int> input)
    {
        var slidingWindow = input.BufferWithoutPartials(2, 1);
        var depthIncreases = slidingWindow.Select(b => b[1] > b[0]).Count(v => v);
        return depthIncreases;
    }



    protected override TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines) =>
        ParseInputForPart1(lines);

    protected override TPart2Answer Part2AnswerSample => 5;
    protected override TPart2Answer Part2AnswerActual => 1190;

    protected override TPart2Answer Part2Solver(IReadOnlyList<int> input)
    {
        var rollingSumsWindow = input.BufferWithoutPartials(3, 1).Select(b => b.Sum<int, int>());
        var sumsComparisonWindow = rollingSumsWindow.BufferWithoutPartials(2, 1);
        var depthIncreases = sumsComparisonWindow.Select(b => b[1] > b[0]).Count(v => v);
        return depthIncreases;
    }
}
