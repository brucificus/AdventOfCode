using System.Text.RegularExpressions;
using FastGraph;
using FastGraph.Algorithms;

await NUnitApplication.CreateBuilder().Build().RunAsync();

public partial class Program
{
    private IReadOnlyList<string> inputLines = null!;
    public record BagColor(string Value);
    public record BagGrouping(int Count, BagColor Color);
    public record BagGraphEdge(BagColor Inward, BagColor Outward, int Count) : FastGraph.IEdge<BagColor>
    {
        BagColor IEdge<BagColor>.Source => this.Inward;

        BagColor IEdge<BagColor>.Target => this.Outward;
    }

    [SetUp]
    public async Task Setup()
    {
        inputLines = await new InputFileFacade().ReadAllLinesAsync();
    }

    [Test(ExpectedResult = 274)]
    public int Part1()
    {
        var (uniqueBagColors, bagEdges) = ParseBagRules();
        var bagGraph = BuildBagGraph(uniqueBagColors, bagEdges);

        IEnumerable<BagColor> FindAllBagColorsThatCanContainBagColor(BagColor desiredBagColor)
        {
            var results = ImmutableHashSet<BagColor>.Empty;
            foreach (var root in uniqueBagColors)
            {
                var searchFunction = bagGraph.TreeBreadthFirstSearch(root);
                searchFunction(desiredBagColor, out IEnumerable<BagGraphEdge> resultEdges);
                resultEdges ??= ImmutableList<BagGraphEdge>.Empty;
                var allEdgesExceptLast = resultEdges.Take(resultEdges.Count() - 1);
                var lastEdge = resultEdges.Skip(resultEdges.Count() - 1).SingleOrDefault();
                results = allEdgesExceptLast.Aggregate(results, (p, c) => p.Add(c.Inward).Add(c.Outward));
                if (lastEdge != null)
                {
                    results = results.Add(lastEdge.Inward);
                }
            }

            return results;
        }

        var allBagColorsThatCanContainBagColor = FindAllBagColorsThatCanContainBagColor(new BagColor("shiny gold"));
        return allBagColorsThatCanContainBagColor.Count();
    }

    [Test(ExpectedResult = 158730)]
    public int Part2()
    {
        var (uniqueBagColors, bagEdges) = ParseBagRules();
        var bagGraph = BuildBagGraph(uniqueBagColors, bagEdges);

        int CountBagsInBag(BagColor desiredBagColor)
        {
            var workingGraph = bagGraph.IsolateRoot(desiredBagColor);

            var bagCosts = new Dictionary<BagColor, Lazy<int>>();

            int CountBagsInBagInternal(BagColor desiredBagColor)
            {
                workingGraph.TryGetOutEdges(desiredBagColor, out var edges);
                edges ??= Enumerable.Empty<BagGraphEdge>();
                return 1 + edges.Aggregate(0, (p, c) => p + (c.Count * bagCosts[c.Outward].Value));
            }

            foreach (var bagColor in workingGraph.TopologicalSort().Reverse())
            {
                bagCosts.Add(bagColor, new Lazy<int>(() => CountBagsInBagInternal(bagColor), false));
            }

            return bagCosts[desiredBagColor].Value - 1;
        }

        return CountBagsInBag(new BagColor("shiny gold"));
    }

    private IMutableVertexAndEdgeListGraph<BagColor, BagGraphEdge> BuildBagGraph(IReadOnlySet<BagColor> uniqueBagColors, IReadOnlySet<BagGraphEdge> bagEdges)
    {
        var bagGraph = new AdjacencyGraph<BagColor, BagGraphEdge>();
        bagGraph.AddVertexRange(uniqueBagColors);
        bagGraph.AddVerticesAndEdgeRange(bagEdges);
        return bagGraph;
    }

    private (IReadOnlySet<BagColor> uniqueBagColors, IReadOnlySet<BagGraphEdge> bagEdges) ParseBagRules()
    {
        var arrow = new Regex("\\sbags\\scontain\\s");
        var nonEmptyBagTrailer = new Regex("(\\d+)\\s((\\w+\\s)+)bags?(\\.|,\\s)");
        var emptyBagTrailer = new Regex("no\\sother\\sbags\\.");
        IEnumerable<BagGrouping> CaptureBagGroupings(MatchCollection matches)
        {
            foreach (var match in matches.Cast<Match>())
            {
                yield return new BagGrouping(int.Parse(match.Groups[1].Value), new BagColor(match.Groups[2].Value.Trim()));
            }
        }

        var parsedInputLines =
            (from line in inputLines
                let lineSides = line.Split(arrow)
                let inwardBagColor = new BagColor(lineSides[0].Trim())
                let emptyBagTrailerMatch = lineSides[1].Matches(emptyBagTrailer).SingleOrDefault()
                let nonEmptyBagTrailerMatches = lineSides[1].Matches(nonEmptyBagTrailer)
                let outwardBagColorsAndQuantities = emptyBagTrailerMatch != null
                    ? ImmutableList<BagGrouping>.Empty
                    : CaptureBagGroupings(nonEmptyBagTrailerMatches).ToImmutableList()
                select (inwardBagColor, outwardBagColorsAndQuantities))
            .ToImmutableDictionary(i => i.inwardBagColor, i => i.outwardBagColorsAndQuantities);

        var uniqueBagColors = parsedInputLines.Aggregate(parsedInputLines.Keys.ToImmutableHashSet(), (p, c) => p.Union(c.Value.Select(i => i.Color)));
        var bagEdges = (from inward in uniqueBagColors
                where parsedInputLines.ContainsKey(inward)
                let outwardsAndQuantities = parsedInputLines[inward]
                let edges = outwardsAndQuantities.Select(o => new BagGraphEdge(inward, o.Color, o.Count)).ToImmutableList().AsEnumerable()
                from edge in edges
                select edge)
            .ToImmutableHashSet();
        return (uniqueBagColors, bagEdges);
    }
}
