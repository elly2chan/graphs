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
using Graphviz4Net.Dot.AntlrParser;
using Graphviz4Net.Dot;
using System.IO;

namespace graph_cover_vertex_approx
{
    class Tools
    {
        public static UndirectedGraph<string, Edge<string>> ReadInDotFile(string filePath)
        {
            var parser = AntlrParserAdapter<string>.GetParser();
            var stream = new StreamReader(filePath);
            var dotGraph = parser.Parse(stream);

            var vertices = new List<TVertex>();
            foreach (var vertex in dotGraph.Vertices) vertices.Add(vertex.Id);

            var edges = new List<Edge<string>>();
            foreach (var objEdge in dotGraph.Edges)
            {
                var edge = (DotEdge<string>)objEdge;
                edges.Add(new Edge<string>(edge.Source.Id, edge.Destination.Id));

            }
            stream.Close();
           
            UndirectedGraph<string, Edge<string>> graph = new UndirectedGraph<string, Edge<string>> ();
            graph.AddVertexRange(vertices);
            graph.AddEdgeRange(edges);
            return graph;
        }

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
        static public bool isVertexCover(UndirectedGraph<string, Edge<string>> graph, List<TVertex> cover)
        {
            foreach (Edge<string> e in graph.Edges)
            {
                bool isCovered = false;
                foreach (string v in cover)
                {
                    if (e.IsAdjacent(v))
                    {
                        isCovered = true;
                        break;
                    }
                }
                if (!isCovered) return false;
            }
            return true;
        }
    }
}
