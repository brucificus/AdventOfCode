
await NUnitApplication.CreateBuilder().Build().RunAsync();

public readonly record struct Part1InputParsed(ImmutableList<int> Values);
public readonly record struct Part1Answer(long Value);
public readonly record struct Part2InputParsed(ImmutableList<int> Values, int ForNumberOfDays);
public readonly record struct Part2Answer(long Value);

[TestFixture(Description = "Day06")]
public partial class Program : TestableSolverBase<Part1InputParsed, Part1Answer, Part2InputParsed, Part2Answer>
{
    protected override Part1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        new (lines.First().Split(',').Select(int.Parse).ToImmutableList());

    protected override Part1Answer Part1Solver(Part1InputParsed input) =>
        new (CountBreedingFish(input.Values, forNumberOfDays: 80));

    protected override Part1Answer Part1AnswerSample => new(5934);

    protected override Part1Answer Part1AnswerActual => new(359999);

    protected override Part2InputParsed ParseInputForPart2Sample(IReadOnlyList<string> lines) =>
        new(ParseInputForPart1(lines).Values, 256);

    protected override Part2InputParsed ParseInputForPart2Actual(IReadOnlyList<string> lines) =>
        new(ParseInputForPart1(lines).Values, 256);

    protected override Part2Answer Part2Solver(Part2InputParsed input) =>
        new (CountBreedingFish(input.Values, forNumberOfDays: input.ForNumberOfDays));

    protected override Part2Answer Part2AnswerSample => new(26984457539);

    protected override Part2Answer Part2AnswerActual => new(1631647919273);

    private static long CountBreedingFish(ImmutableList<int> startingFishTimerValues, int forNumberOfDays)
    {
        var rangeOfFishTimerValues = Enumerable.Range(0, 9).ToImmutableArray();

        var countOfFishWithTimerValue = rangeOfFishTimerValues.ToImmutableDictionary(v => v, _ => 0L);
        countOfFishWithTimerValue = startingFishTimerValues.Aggregate(countOfFishWithTimerValue, (p, c) => p.SetItem(c, p[c] + 1));

        foreach (var daysElapsed in Enumerable.Range(0, forNumberOfDays))
        {
            var builderForNewCountOfFishWithTimerValue = countOfFishWithTimerValue.Keys.Union(rangeOfFishTimerValues)
                .ToImmutableDictionary(v => v, _ => 0L).ToBuilder();
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

        return countOfFishWithTimerValue.Values.Sum();
    }
}

