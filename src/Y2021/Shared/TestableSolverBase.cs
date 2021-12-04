using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode.Y2021.Shared;


public abstract class TestableSolverBase<TPart1InputParsed, TPart1Answer, TPart2InputParsed, TPart2Answer>
    where TPart1Answer : IEquatable<TPart1Answer>
    where TPart2Answer : IEquatable<TPart2Answer>
{
    private TPart1InputParsed? _part1InputSample;
    private TPart1InputParsed? _part1InputActual;
    private TPart2InputParsed? _part2InputSample;
    private TPart2InputParsed? _part2InputActual;

    protected abstract TPart1InputParsed ParseInputForPart1(IReadOnlyList<string> lines);

    protected abstract TPart2InputParsed ParseInputForPart2(IReadOnlyList<string> lines);

    protected abstract TPart1Answer Part1Solver(TPart1InputParsed input);

    protected abstract TPart2Answer Part2Solver(TPart2InputParsed input);

    protected abstract TPart1Answer Part1AnswerSample { get; }

    protected abstract TPart1Answer Part1AnswerActual { get; }

    protected abstract TPart2Answer Part2AnswerSample { get; }

    protected abstract TPart2Answer Part2AnswerActual { get; }

    [SetUp]
    public async Task SetUp()
    {
    }

    [Test]
    public async Task Part1Sample()
    {
        _part1InputSample = ParseInputForPart1(await new SampleInputFacade().ReadAllLinesAsync());
        var actualAnswer = Part1Solver(_part1InputSample!);
        actualAnswer.Should().Be(Part1AnswerSample);
    }

    [Test]
    public async Task Part1Actual()
    {
        _part1InputActual = ParseInputForPart1(await new InputFileFacade().ReadAllLinesAsync());
        var actualAnswer = Part1Solver(_part1InputActual!);
        actualAnswer.Should().Be(Part1AnswerActual);
    }

    [Test]
    public async Task Part2Sample()
    {
        _part2InputSample = ParseInputForPart2(await new SampleInputFacade().ReadAllLinesAsync());
        var actualAnswer = Part2Solver(_part2InputSample!);
        actualAnswer.Should().Be(Part2AnswerSample);
    }

    [Test]
    public async Task Part2Actual()
    {
        _part2InputActual = ParseInputForPart2(await new InputFileFacade().ReadAllLinesAsync());
        var actualAnswer = Part2Solver(_part2InputActual!);
        actualAnswer.Should().Be(Part2AnswerActual);
    }
}
