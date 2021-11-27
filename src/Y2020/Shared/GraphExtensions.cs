using QuickGraph;
using QuickGraph.Algorithms;

namespace AdventOfCode.Y2020.Shared;

public static class GraphExtensions
{
    public static IVertexAndEdgeListGraph<TVertex, TEdge> IsolateRoot<TVertex, TEdge>(this IVertexAndEdgeListGraph<TVertex, TEdge> graph, TVertex root)
        where TVertex : IEquatable<TVertex>
        where TEdge : IEdge<TVertex>
    {
        var workingGraph = new AdjacencyGraph<TVertex, TEdge>();
        workingGraph.AddVertexRange(graph.Vertices);
        workingGraph.AddVerticesAndEdgeRange(graph.Edges);

        while (workingGraph.Roots().Any(bc => !bc.Equals(root)))
        {
            var rootsToRemove = workingGraph.Roots().Where(bc => !bc.Equals(root)).ToImmutableList();
            rootsToRemove.ForEach(bc => workingGraph.RemoveVertex(bc));
        }

        return workingGraph;
    }

    public static IEnumerable<TResult> Execute<TInput, TResult>(this TryFunc<TInput, IEnumerable<TResult>> tryFunc, TInput input)
    {
        if (tryFunc(input, out var result))
        {
            return result;
        } else
        {
            return Enumerable.Empty<TResult>();
        }
    }
}
