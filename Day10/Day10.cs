using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;

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

        var allJoltageAdapters = inputValues.Prepend(outlet).Append(target);
        var allJoltageAdaptersByOutputJoltage = allJoltageAdapters.ToImmutableSortedDictionary(i => i.outputJoltage, i => i);

        var joltageAdapterGraph = allJoltageAdapters
            .ToDelegateVertexAndEdgeListGraph(
                joltageAdapter =>
                    Enumerable
                        .Range(joltageAdapter.outputJoltage, 4)
                        .Where(oj => allJoltageAdaptersByOutputJoltage.ContainsKey(oj))
                        .Select(oj => allJoltageAdaptersByOutputJoltage[oj])
                        .Where(ja => joltageAdapter != ja)
                        .Select(ja => new JoltageAdapterGraphEdge() { Source = joltageAdapter, Target = ja })
                        .ToImmutableList());

        IEnumerable<int> ConsecutiveDifferences(IReadOnlyList<JoltageAdapter> items) =>
            items.Buffer(2, 1).Where(b => b.Count == 2).Select(b => b[1].outputJoltage - b[0].outputJoltage);

        var consecutiveDifferences = ConsecutiveDifferences(joltageAdapterGraph.TopologicalSort().ToImmutableList()).ToImmutableList();

        TestContext.Out.WriteLine(joltageAdapterGraph.ToGraphviz());
        return consecutiveDifferences.Count(d => d == 1) * consecutiveDifferences.Count(d => d == 3);
    }

    private IReadOnlyList<JoltageAdapter> ParseInputValues()
    {
        return inputLines
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select((s, i) => new JoltageAdapter(i, int.Parse(s)))
            .ToImmutableList();
    }
}
