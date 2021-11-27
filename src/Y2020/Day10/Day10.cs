using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;

namespace AdventOfCode.Y2020.Day10;

public class Day10
{
    private IReadOnlyCollection<string> inputLines = null!;

    public record JoltageAdapter(int Index, int OutputJoltage);

    public record JoltageAdapterGraphEdge(JoltageAdapter Source, JoltageAdapter Target) : IEdge<JoltageAdapter>
    {
        public int JoltageDifference => Target.OutputJoltage - Source.OutputJoltage;
    }

    [SetUp]
    public async Task Setup()
    {
        inputLines = await new InputFileFacadeFacade().ReadAllLinesAsync();
    }

    [Test(ExpectedResult = 2277)]
    public int Part1()
    {
        var outlet = new JoltageAdapter(-1, 0);
        var inputValues = ParseInputValues();
        var target = new JoltageAdapter(inputValues.Max(i => i.Index) + 1, inputValues.Max(i => i.OutputJoltage) + 3);
        var joltageAdapterGraph = CreateJoltageAdapterGraph(outlet, inputValues, target);

        static IEnumerable<int> ConsecutiveDifferences(IReadOnlyList<JoltageAdapter> items) =>
            items.Buffer(2, 1).Where(b => b.Count == 2).Select(b => b[1].OutputJoltage - b[0].OutputJoltage);

        var consecutiveDifferences = ConsecutiveDifferences(joltageAdapterGraph.TopologicalSort().ToImmutableList()).ToImmutableList();

        return consecutiveDifferences.Count(d => d == 1) * consecutiveDifferences.Count(d => d == 3);
    }

    [Test(ExpectedResult = "37024595836928")]
    public string Part2()
    {
        var outlet = new JoltageAdapter(-1, 0);
        var inputValues = ParseInputValues();
        var target = new JoltageAdapter(inputValues.Max(i => i.Index) + 1, inputValues.Max(i => i.OutputJoltage) + 3);
        var joltageAdapterGraph = CreateJoltageAdapterGraph(outlet, inputValues, target).ToBidirectionalGraph();

        IEnumerable<JoltageAdapterGraphEdge> GetOutEdges(JoltageAdapter ja) =>
            joltageAdapterGraph.TryGetOutEdges(ja, out var result) ? result : Enumerable.Empty<JoltageAdapterGraphEdge>();
        IEnumerable<JoltageAdapterGraphEdge> GetInEdges(JoltageAdapter ja) =>
            joltageAdapterGraph.TryGetInEdges(ja, out var result) ? result : Enumerable.Empty<JoltageAdapterGraphEdge>();

        var successorPaths = new Dictionary<JoltageAdapter, Lazy<BigInteger>>();
        joltageAdapterGraph
            .TopologicalSort()
            .Where(ja => ja != target)
            .Select(ja => (ja, value: new Lazy<BigInteger>(() => GetOutEdges(ja).Sum(e => successorPaths[e.Target].Value), false)))
            .ToList()
            .ForEach(i => successorPaths.Add(i.ja, i.value));
        successorPaths.Add(target, new Lazy<BigInteger>(1));

        return successorPaths[outlet].Value.ToString();
    }

    private IVertexAndEdgeListGraph<JoltageAdapter, JoltageAdapterGraphEdge> CreateJoltageAdapterGraph(JoltageAdapter outlet, IReadOnlyList<JoltageAdapter> inputValues, JoltageAdapter target)
    {
        var allJoltageAdapters = inputValues.Prepend(outlet).Append(target);
        var allJoltageAdaptersByOutputJoltage = allJoltageAdapters.ToImmutableSortedDictionary(i => i.OutputJoltage, i => i);

        var graph = new AdjacencyGraph<JoltageAdapter, JoltageAdapterGraphEdge>();
        graph.AddVertexRange(allJoltageAdapters);

        var edges = allJoltageAdapters
            .SelectMany(
                joltageAdapter =>
                    Enumerable
                        .Range(joltageAdapter.OutputJoltage, 4)
                        .Where(oj => allJoltageAdaptersByOutputJoltage.ContainsKey(oj))
                        .Select(oj => allJoltageAdaptersByOutputJoltage[oj])
                        .Where(ja => joltageAdapter != ja)
                        .Select(ja => new JoltageAdapterGraphEdge(joltageAdapter, ja))
                        .ToImmutableList())
            .ToImmutableList();
        graph.AddEdgeRange(edges);

        TestContext.Out.WriteLine(graph.ToGraphviz());
        return graph;
    }

    private IReadOnlyList<JoltageAdapter> ParseInputValues()
    {
        return inputLines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select((s, i) => new JoltageAdapter(i, int.Parse(s)))
            .ToImmutableList();
    }
}
