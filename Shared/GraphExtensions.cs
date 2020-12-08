using System;
using System.Collections.Immutable;
using System.Linq;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Shared
{
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
    }
}
