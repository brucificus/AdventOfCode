
await NUnitApplication.CreateBuilder().Build().RunAsync();

public readonly record struct Part1InputParsed(ImmutableList<int> Values);
public readonly record struct Part1Answer(int Value);
public readonly record struct Part2InputParsed;
public readonly record struct Part2Answer;

[TestFixture(Description = "Day06")]
public partial class Program : TestableSolverBase<Part1InputParsed, Part1Answer, Part2InputParsed, Part2Answer>
{
    protected override Part1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        new (lines.First().Split(',').Select(int.Parse).ToImmutableList());

    protected override Part1Answer Part1Solver(Part1InputParsed input)
    {
        var rangeOfFishTimerValues = Enumerable.Range(0, 9).ToImmutableArray();

        var countOfFishWithTimerValue = rangeOfFishTimerValues.ToImmutableDictionary(v => v, _ => 0);
        countOfFishWithTimerValue = input.Values.Aggregate(countOfFishWithTimerValue, (p, c) => p.SetItem(c, p[c] + 1));

        foreach (var daysElapsed in Enumerable.Range(0, 80))
        {
            var builderForNewCountOfFishWithTimerValue = countOfFishWithTimerValue.Keys.Union(rangeOfFishTimerValues).ToImmutableDictionary(v => v, _ => 0).ToBuilder();
            foreach (var fishTimerValue in countOfFishWithTimerValue.Keys)
            {
                if (fishTimerValue == 0)
                {
                    builderForNewCountOfFishWithTimerValue[6] += countOfFishWithTimerValue[fishTimerValue];
                    builderForNewCountOfFishWithTimerValue[8] += countOfFishWithTimerValue[fishTimerValue];
                }
                else
                {
                    var newFishTimerValue = fishTimerValue - 1;
                    builderForNewCountOfFishWithTimerValue[newFishTimerValue] += countOfFishWithTimerValue[fishTimerValue];
                }
            }

            countOfFishWithTimerValue = builderForNewCountOfFishWithTimerValue.ToImmutable();
        }

        return new Part1Answer(countOfFishWithTimerValue.Values.Sum());
    }

    protected override Part1Answer Part1AnswerSample => new(5934);

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

