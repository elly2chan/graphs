using System;
using System.Collections.Generic;
using System.Reflection; 
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using QuickGraph;
using GraphLibrary;

using Microsoft.Office.Interop.Excel;


namespace Timestamps
{
    public class RandomGraph
    {
        /// <summary> 
        /// Picks a vertex randomly in the vertex list 
        /// </summary> 
        /// <param name="g">vertex list</param> 
        /// <param name="rnd">random generator</param> 
        /// <returns>randomaly chosen vertex</returns> 
        public static int Vertex(UndirectedGraph<int, Edge<int>> g, Random rnd)
        {
            if (g == null)
                throw new ArgumentNullException("g");
            if (rnd == null)
                throw new ArgumentNullException("random generator");
            if (g.VertexCount == 0)
                throw new ArgumentException("g is empty");

            int i = rnd.Next(g.VertexCount);
            foreach (int v in g.Vertices)
            {
                if (i == 0)
                    return v;
                else
                    --i;
            }

            // failed 
            throw new Exception();
        }

        /// <summary> 
        /// Picks an edge randomly in the edge list 
        /// </summary> 
        /// <param name="g">edge list</param> 
        /// <param name="rnd">random generator</param> 
        /// <returns>randomaly chosen edge</returns> 
        public static Edge<int> Edge(UndirectedGraph<int, Edge<int>> g, Random rnd)
        {
            if (g == null)
                throw new ArgumentNullException("g");
            if (rnd == null)
                throw new ArgumentNullException("random generator");
            if (g.EdgeCount == 0)
                throw new ArgumentException("g is empty");

            int i = rnd.Next(g.EdgeCount);
            foreach (Edge<int> e in g.Edges)
            {
                if (i == 1)
                    return e;
                else
                    --i;
            }

            // failed 
            throw new Exception();
        }

        /// <summary> 
        /// Generates a random graph 
        /// </summary> 
        /// <param name="g">Graph to fill</param> 
        /// <param name="V">number of vertices</param> 
        /// <param name="E">number of edges</param> 
        /// <param name="rnd">random generator</param> 
        /// <param name="selfEdges">self edges allowed</param> 
        public static QuickGraph.UndirectedGraph<int, QuickGraph.Edge<int>> CreateGraph(
            QuickGraph.UndirectedGraph<int, QuickGraph.Edge<int>> g,
            int V,
            int E,
            System.Random rnd,
            bool selfEdges
            )
        {

            if (g == null)
                throw new ArgumentNullException("g");
            if (rnd == null)
                throw new ArgumentNullException("random generator");

            int a = 0;
            int b = 0;
            // When parallel edges are not allowed, we create a new graph which 
            // does not allow parallel edges, construct it and copy back. 
            // This is not efficient if 'g' already disallow parallel edges, 
            // but that's task for later. 
            if (!g.AllowParallelEdges)
            {
                throw new Exception("not implemented");
            }
            else
            {
                for (int i = 1; i <= V; i++)
                    g.AddVertex(i);

                for (int j = 0; j < E; j++)
                {
                    a = Vertex((UndirectedGraph<int, Edge<int>>)g, rnd);
                    do
                    {
                        b = Vertex((UndirectedGraph<int, Edge<int>>)g, rnd);
                    }
                    while (((!selfEdges) && a == b) || g.ContainsEdge(a,b));

                    g.AddEdge(new Edge<int>(a, b));
                }
            }

            return g;
        }

    }

    class Program
    {
        /*
        static void sets(IEnumerable<int> M)
        {
            int w = M.Count(); //--кол-во элементов множества
 
            int i, j, n;
 
            n = (int)Math.Pow((double)2, (double)w);
            
            for ( i = 0; i < n; i++ ) //--перебор битовых маск
            {
                for ( j = 0; j < w; j++ ) //--перебор битов в маске
                    if ( i & (1 << j) ) //--если j-й бит установлен
                    printf("%d ", M[j]); //--то выводим j-й элемент множества
                    printf("}\n");
            }
        }

        static void bruteforce(UndirectedGraph<int, Edge<int>> g)
        {

        }
        */

        static void Main(string[] args)
        {

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                Console.WriteLine("EXCEL could not be started. Check that your office installation and project references are correct.");
                return;
            }
            xlApp.Visible = true;

            Workbook wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = (Worksheet)wb.Worksheets[1];

            if (ws == null)
            {
                Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
            }
            
            int k = 1;
            Array.ForEach(Directory.GetFiles(@"../../bag-report/"), File.Delete);

            Stopwatch stopWatch = new Stopwatch();
            int vertices = 40;
            int j = 0;
            int[] vs = new int[vertices];
            int[] es = new int[vertices];
            double[] ds = new double[vertices];

            for (int i = 5; i < vertices; i++)
            {
                int e = (int)Math.Floor((1.0 + 0.11 * i) * i); //0.07
                var generator = new Random();
                var graph = new UndirectedGraph<int, Edge<int>>();
                UndirectedGraph<int, Edge<int>> randomGraph = RandomGraph.CreateGraph(graph, i, e, generator, false);
                try
                {
                    stopWatch.Start();
                    var result = Algorithm.Robson(randomGraph);
                    stopWatch.Stop();
                    double time = stopWatch.Elapsed.TotalMilliseconds;
                    Console.WriteLine("({0:d},{1:d}):{2:g};", i, e, time);
                    vs[j] = i;
                    es[j] = e;
                    ds[j] = time;
                    j++;
                    stopWatch.Reset();
                }
                catch
                {
                    string bagname = "../../bag-report/bag-" + k.ToString() + ".dot";
                    StreamWriter file = new StreamWriter(bagname, true);
                    foreach (var v in randomGraph.Vertices)
                    {
                        file.Write("{0:d};", v);
                    }
                    file.WriteLine("");
                    foreach (Edge<int> edge in randomGraph.Edges)
                    {
                        file.WriteLine("{0:d}->{1:d};", edge.Source, edge.Target);
                    }
                    file.Close();
                    k++;
                }
            }

            for (int i = 0; i < j; i++ )
            {
                ws.Cells[i+1, 1] = vs[i];
                ws.Cells[i+1, 2] = es[i];
                ws.Cells[i+1, 3] = ds[i];
            }
                
        }
    }
}
