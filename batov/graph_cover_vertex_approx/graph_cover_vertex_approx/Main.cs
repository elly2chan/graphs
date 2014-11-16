using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TVertex = System.String;
using QuickGraph.Collections;
using QuickGraph;

namespace graph_cover_vertex_approx
{
    class main
    {
        static void Main(string[] args)
        {
            UndirectedGraph<string, Edge<string>> graph;
            VertexList<TVertex> result;

            // graph = Graphes.CreateGraph();
            // graph = Graphes.CreateSimpleGraph();
            // graph = Graphes.CreateHugeGraph();
            graph = Graphes.CreateRandomGraph(100,2);

            result = Algorithms.lsoAlgo(graph);
            //result = Algorithms.standartAlgo(graph);     

            // Check and print results
            if (Tools.isVertexCover(graph, result))
                Tools.printCoverSet(result);
            else
                Console.WriteLine("Is not cover set!");

            Console.Read();
            
        } 
    }
}
