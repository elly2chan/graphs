using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QuickGraph;
using QuickGraph.Data;
using QuickGraph.Algorithms;
using QuickGraph.Collections;
using QuickGraph.Predicates;
using QuickGraph.Serialization;
using QuickGraph.Contracts;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;
using TVertex = System.String;

namespace graph_cover_vertex_approx
{
    class Tools
    {
        public static void printCoverSet(VertexList<TVertex> set)
        {
            Console.Write("Cover vertex set = {");
            set.ForEach(delegate(TVertex vertex)
            {
                Console.Write(vertex + ",");
            });
            Console.WriteLine("}");
            Console.Write("Size = ");
            Console.WriteLine(set.Count);
        }
        static public bool isVertexCover(UndirectedGraph<string, Edge<string>> graph, VertexList<TVertex> cover)
        {
            foreach (Edge<string> e in graph.Edges)
            {
                foreach (string v in cover)
                {
                    if (!e.IsAdjacent(v))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
