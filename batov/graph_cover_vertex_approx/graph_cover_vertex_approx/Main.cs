using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TVertex = System.String;
using QuickGraph.Collections;
using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;


namespace graph_cover_vertex_approx
{
    class main
    {
        static void Main(string[] args)
        {
            UndirectedGraph<string, Edge<string>> graph;
            VertexList<TVertex> result;
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();

             //graph = Graphes.CreateGraph();
            // graph = Graphes.CreateSimpleGraph();
            // graph = Graphes.CreateHugeGraph();
            // graph = Graphes.CreateRandomGraph(3,2);
            // graph = Tools.ReadInDotFile("dottest.dot");
             List<string> lines = new List<string>();
             for (int i = 1; i <= 100; i++)
             {
                 graph = Graphes.CreateRandomGraph(i, 2);

                 myStopwatch.Start();
                 result = Algorithms.standartAlgo(graph);
                 myStopwatch.Stop();
                 Console.WriteLine("standart: {0}",result.Count);
                 lines.Add(String.Format("{0} : {1}", i ,myStopwatch.Elapsed));
                 myStopwatch.Reset();
             }
             System.IO.File.WriteAllLines(@"C:\Users\1\Desktop\text.txt", lines);

                 /*
                  Console.WriteLine(String.Concat("Vertex count = ", graph.VertexCount));
                  myStopwatch.Start();
                 int ideal = Algorithms.minVertexCover(graph);
                 myStopwatch.Stop();
                 Console.WriteLine(" ---- Minimal Vertex Cover ---- ");
                 Console.Write("Size = ");
                 Console.WriteLine(ideal);
                 Console.Write("Time = ");
                 Console.WriteLine(myStopwatch.Elapsed);

                 myStopwatch.Reset();
                 myStopwatch.Start();
                 result = Algorithms.lsoAlgo(graph);
                 //result = Algorithms.standartAlgo(graph);  
                 myStopwatch.Stop();
               

                 // Check and print results
                 if (Tools.isVertexCover(graph, result)) 
                 {
                     Console.WriteLine(" ---- Approx Vertex Cover ---- ");
                     Console.WriteLine(string.Concat("Size = ",result.Count));
                     Console.WriteLine(string.Concat("Time = ", myStopwatch.Elapsed));
                     Console.WriteLine(" ---- Result ---- ");
                     Console.Write("result/MVC: ");
                     Console.WriteLine((float) result.Count/ideal);
                     //Tools.printCoverSet(result);
                 }
                else
                     Console.WriteLine("Is not cover set!");
                 */
                 Console.Read();
            
        } 
    }
}
