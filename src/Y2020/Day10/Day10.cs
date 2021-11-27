using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NUnit.Framework;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;
using Shared;

public class Day10
{
    private IReadOnlyCollection<string> inputLines;

    public record JoltageAdapter(int index, int outputJoltage);

    public record JoltageAdapterGraphEdge : IEdge<JoltageAdapter>
    {
        public JoltageAdapter Source { get; init; }

        public int JoltageDifference => Target.outputJoltage - Source.outputJoltage;

        public JoltageAdapter Target { get; init; }
    }

    [SetUp]
    public async Task Setup()
    {
        inputLines = await System.IO.File.ReadAllLinesAsync("input.txt");
    }

    [Test(ExpectedResult = 2277)]
    public int Part1()
    {
        var outlet = new JoltageAdapter(-1, 0);
        var inputValues = ParseInputValues();
        var target = new JoltageAdapter(inputValues.Max(i => i.index) + 1, inputValues.Max(i => i.outputJoltage) + 3);
        var joltageAdapterGraph = CreateJoltageAdapterGraph(outlet, inputValues, target);

        IEnumerable<int> ConsecutiveDifferences(IReadOnlyList<JoltageAdapter> items) =>
            items.Buffer(2, 1).Where(b => b.Count == 2).Select(b => b[1].outputJoltage - b[0].outputJoltage);

        var consecutiveDifferences = ConsecutiveDifferences(joltageAdapterGraph.TopologicalSort().ToImmutableList()).ToImmutableList();

        return consecutiveDifferences.Count(d => d == 1) * consecutiveDifferences.Count(d => d == 3);
    }

    [Test(ExpectedResult = "37024595836928")]
    public string Part2()
    {
        var outlet = new JoltageAdapter(-1, 0);
        var inputValues = ParseInputValues();
        var target = new JoltageAdapter(inputValues.Max(i => i.index) + 1, inputValues.Max(i => i.outputJoltage) + 3);
        var joltageAdapterGraph = CreateJoltageAdapterGraph(outlet, inputValues, target).ToBidirectionalGraph();

        IEnumerable<JoltageAdapterGraphEdge> getOutEdges(JoltageAdapter ja) =>
            joltageAdapterGraph.TryGetOutEdges(ja, out var result) ? result : Enumerable.Empty<JoltageAdapterGraphEdge>();
        IEnumerable<JoltageAdapterGraphEdge> getInEdges(JoltageAdapter ja) =>
            joltageAdapterGraph.TryGetInEdges(ja, out var result) ? result : Enumerable.Empty<JoltageAdapterGraphEdge>();

        var successorPaths = new Dictionary<JoltageAdapter, Lazy<BigInteger>>();
        joltageAdapterGraph
            .TopologicalSort()
            .Where(ja => ja != target)
            .Select(ja => (ja, value: new Lazy<BigInteger>(() => getOutEdges(ja).Sum(e => successorPaths[e.Target].Value), false)))
            .ToList()
            .ForEach(i => successorPaths.Add(i.ja, i.value));
        successorPaths.Add(target, new Lazy<BigInteger>(1));

        return successorPaths[outlet].Value.ToString();
    }

    private IVertexAndEdgeListGraph<JoltageAdapter, JoltageAdapterGraphEdge> CreateJoltageAdapterGraph(JoltageAdapter outlet, IReadOnlyList<JoltageAdapter> inputValues, JoltageAdapter target)
    {
        var allJoltageAdapters = inputValues.Prepend(outlet).Append(target);
        var allJoltageAdaptersByOutputJoltage = allJoltageAdapters.ToImmutableSortedDictionary(i => i.outputJoltage, i => i);

        var graph = new AdjacencyGraph<JoltageAdapter, JoltageAdapterGraphEdge>();
        graph.AddVertexRange(allJoltageAdapters);

        var edges = allJoltageAdapters
                    .SelectMany(
                        joltageAdapter =>
                            Enumerable
                                .Range(joltageAdapter.outputJoltage, 4)
                                .Where(oj => allJoltageAdaptersByOutputJoltage.ContainsKey(oj))
                                .Select(oj => allJoltageAdaptersByOutputJoltage[oj])
                                .Where(ja => joltageAdapter != ja)
                                .Select(ja => new JoltageAdapterGraphEdge() { Source = joltageAdapter, Target = ja })
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
