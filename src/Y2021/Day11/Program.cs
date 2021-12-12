using System.Runtime.CompilerServices;
using AdventOfCode.Y2020.Shared.Mapping;
using FastGraph;
using FastGraph.Algorithms;
using FastGraph.Algorithms.Search;
using OneOf.Types;
using Spectre.Console;

await NUnitApplication.CreateBuilder().Build().RunAsync();

public readonly record struct Part1InputParsed(DenseFullyBoundedIntegralPlane<Octopus> Grid);
public readonly record struct Part1Answer;
public readonly record struct Part2InputParsed;
public readonly record struct Part2Answer;

[TestFixture(Description = "Day11")]
public partial class Program : TestableSolverBase<Part1InputParsed, Part1Answer, Part2InputParsed, Part2Answer>
{
    protected override Part1InputParsed ParseInputForPart1(IReadOnlyList<string> lines) =>
        new(DenseFullyBoundedIntegralPlane<Octopus>.FromLines(lines.Where(l => l.Length > 0), Octopus.Parse));

    protected override Part1Answer Part1Solver(Part1InputParsed input)
    {
        var grid = input.Grid;


        static bool IsFlashingNow(Octopus o) => o.EnergyLevel > 9;

        static bool CouldLatentlyFlash(Octopus o) => o.EnergyLevel >= 9;

        static DenseFullyBoundedIntegralPlane<Octopus> Advance(DenseFullyBoundedIntegralPlane<Octopus> grid)
        {
            grid = grid.With((i, c) => i with { EnergyLevel = i.EnergyLevel + 1, JustFlashed = false });

            var octopusConnectionGraph =
                grid
                    .Where(IsFlashingNow)
                    .Select(SpecificOctopus.FromTuple)
                    .ToDelegateVertexAndEdgeListGraph(t => grid.SelectAdjacentDiagonals(t.Coordinate).Select(u => new DirectedOctopusConnection(t, SpecificOctopus.FromTuple(u))))
                    .ToBidirectionalGraph();

            throw new NotImplementedException();
        }

        grid = Enumerable.Range(0, 100).Aggregate(grid, (p, c) => Advance(p));


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

public readonly record struct SpecificOctopus(Vector2<int> Coordinate, Octopus Cell)
{
    public static SpecificOctopus FromTuple((Vector2<int> coordinate, Octopus cell) t)
    {
        return new SpecificOctopus(t.coordinate, t.cell);
    }
}

public readonly record struct DirectedOctopusConnection(SpecificOctopus Source, SpecificOctopus Target)
    : IEdge<SpecificOctopus>
{
}

public readonly record struct Octopus(int EnergyLevel, bool JustFlashed = false, int TotalFlashCount = 0)
{
    public static Octopus Parse(char c) => new(byte.Parse(c.ToString()));
}

public static class DenseFullyBoundedIntegralPlaneExtensions
{
    private static readonly Vector2<int> RelativeDiagonal1 = new(-1, -1);
    private static readonly Vector2<int> RelativeDiagonal2 = new(1, -1);
    private static readonly Vector2<int> RelativeDiagonal3 = new(1, 1);
    private static readonly Vector2<int> RelativeDiagonal4 = new(-1, 1);

    public static IEnumerable<(Vector2<int> coordinate, TCell cell)> SelectAdjacentDiagonals<TCell>(this DenseFullyBoundedIntegralPlane<TCell> grid, Vector2<int> originCoordinate) where TCell : IEquatable<TCell>
    {

        var diagonal1 = RelativeDiagonal1 + originCoordinate;
        var diagonal2 = RelativeDiagonal2 + originCoordinate;
        var diagonal3 = RelativeDiagonal3 + originCoordinate;
        var diagonal4 = RelativeDiagonal4 + originCoordinate;

        return grid.SelectAdjacents(originCoordinate).Where(t => t.coordinate == diagonal1 || t.coordinate == diagonal2 || t.coordinate == diagonal3 || t.coordinate == diagonal4);
    }
}

