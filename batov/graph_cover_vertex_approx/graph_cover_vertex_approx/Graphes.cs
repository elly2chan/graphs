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
using System.IO;

namespace graph_cover_vertex_approx
{
    class Graphes
    {
        public static UndirectedGraph<string, Edge<string>>  CreateRandomGraph(int count, int coeff)
        {
            Random rnd = new Random();
            UndirectedGraph<string, Edge<string>> mGraph = new UndirectedGraph<string, Edge<string>>(true);

            for (int i = 1; i <= count; i++)
            {
                mGraph.AddVertex(i.ToString());
            }

            for (int i = 0; i < count * coeff; i++)
            {
                int rand = rnd.Next(count)+1;
                int rand2 = rnd.Next(count)+1;
                var edge12 = new Edge<string>(rand.ToString(), rand2.ToString());
                if (!mGraph.Edges.Contains(edge12))
                    mGraph.AddEdge(edge12);
            }
            return mGraph;
        }
        
        public static UndirectedGraph<string, Edge<string>> CreateHugeGraph()
        {
            UndirectedGraph<string, Edge<string>> graph = new UndirectedGraph<string, Edge<string>>(true);
            // Add some vertices to the graph
            graph.AddVertex("1");
            graph.AddVertex("2");
            graph.AddVertex("3");
            graph.AddVertex("4");
            graph.AddVertex("5");
            graph.AddVertex("6");
            graph.AddVertex("7");
            graph.AddVertex("8");
            graph.AddVertex("9");
            graph.AddVertex("10");
            graph.AddVertex("11");
            graph.AddVertex("12");
            graph.AddVertex("13");
            graph.AddVertex("14");
            graph.AddVertex("15");
            graph.AddVertex("16");
            graph.AddVertex("17");
            graph.AddVertex("18");
            graph.AddVertex("19");

            // Create the edges
            Edge<string> e1_2 = new Edge<string>("1", "2");
            Edge<string> e1_4 = new Edge<string>("1", "4");
            Edge<string> e2_3 = new Edge<string>("2", "3");
            Edge<string> e2_5 = new Edge<string>("2", "5");
            Edge<string> e3_6 = new Edge<string>("3", "6");
            Edge<string> e3_11 = new Edge<string>("3", "11");
            Edge<string> e4_5 = new Edge<string>("4", "5");
            Edge<string> e4_8 = new Edge<string>("4", "8");
            Edge<string> e5_6 = new Edge<string>("5", "6");
            Edge<string> e5_9 = new Edge<string>("5", "9");
            Edge<string> e6_10 = new Edge<string>("6", "10");
            Edge<string> e6_11 = new Edge<string>("6", "11");
            Edge<string> e8_9 = new Edge<string>("8", "9");
            Edge<string> e9_10 = new Edge<string>("9", "10");
            Edge<string> e10_11 = new Edge<string>("10", "11");
            Edge<string> e1_10 = new Edge<string>("1", "10");
            Edge<string> e5_12 = new Edge<string>("5", "12");
            Edge<string> e5_13 = new Edge<string>("5", "13");
            Edge<string> e12_14 = new Edge<string>("12", "14");
            Edge<string> e12_15 = new Edge<string>("12", "15");
            Edge<string> e15_17 = new Edge<string>("15", "17");
            Edge<string> e17_16 = new Edge<string>("17", "16");
            Edge<string> e17_18 = new Edge<string>("17", "18");
            Edge<string> e17_19 = new Edge<string>("17", "19");
            Edge<string> e19_14 = new Edge<string>("19", "14");
            Edge<string> e19_1 = new Edge<string>("19", "1");
            Edge<string> e8_16 = new Edge<string>("8", "16");
            Edge<string> e3_17 = new Edge<string>("3", "17");



            // Add the edges
            graph.AddEdge(e1_2);
            graph.AddEdge(e1_4);
            graph.AddEdge(e2_3);
            graph.AddEdge(e2_5);
            graph.AddEdge(e3_6);
            graph.AddEdge(e3_11);
            graph.AddEdge(e4_5);
            graph.AddEdge(e4_8);
            graph.AddEdge(e5_6);
            graph.AddEdge(e5_9);
            graph.AddEdge(e6_10);
            graph.AddEdge(e6_11);
            graph.AddEdge(e8_9);
            graph.AddEdge(e9_10);
            graph.AddEdge(e10_11);
            graph.AddEdge(e1_10);
            graph.AddEdge(e5_12);
            graph.AddEdge(e5_13);
            graph.AddEdge(e12_14);
            graph.AddEdge(e12_15);
            graph.AddEdge(e15_17);
            graph.AddEdge(e17_16);
            graph.AddEdge(e17_18);
            graph.AddEdge(e17_19);
            graph.AddEdge(e19_14);
            graph.AddEdge(e19_1);
            graph.AddEdge(e8_16);
            graph.AddEdge(e3_17);
            return graph;
        }
        public static UndirectedGraph<string, Edge<string>> CreateGraph()
        {
            UndirectedGraph<string, Edge<string>> graph = new UndirectedGraph<string, Edge<string>>(true);
            // Add some vertices to the graph
            graph.AddVertex("1");
            graph.AddVertex("2");
            graph.AddVertex("3");
            graph.AddVertex("4");
            graph.AddVertex("5");
            graph.AddVertex("6");
            graph.AddVertex("7");
            graph.AddVertex("8");
            graph.AddVertex("9");
            graph.AddVertex("10");
            graph.AddVertex("11");

            // Create the edges
            Edge<string> e1_2 = new Edge<string>("1", "2");
            Edge<string> e1_4 = new Edge<string>("1", "4");
            Edge<string> e2_3 = new Edge<string>("2", "3");
            Edge<string> e2_5 = new Edge<string>("2", "5");
            Edge<string> e3_6 = new Edge<string>("3", "6");
            Edge<string> e3_11 = new Edge<string>("3", "11");
            Edge<string> e4_5 = new Edge<string>("4", "5");
            Edge<string> e4_8 = new Edge<string>("4", "8");
            Edge<string> e5_6 = new Edge<string>("5", "6");
            Edge<string> e5_9 = new Edge<string>("5", "9");
            Edge<string> e6_10 = new Edge<string>("6", "10");
            Edge<string> e6_11 = new Edge<string>("6", "11");
            Edge<string> e8_9 = new Edge<string>("8", "9");
            Edge<string> e9_10 = new Edge<string>("9", "10");
            Edge<string> e10_11 = new Edge<string>("10", "11");
            Edge<string> e1_10 = new Edge<string>("1", "10");

            // Add the edges
            graph.AddEdge(e1_2);
            graph.AddEdge(e1_4);
            graph.AddEdge(e2_3);
            graph.AddEdge(e2_5);
            graph.AddEdge(e3_6);
            graph.AddEdge(e3_11);
            graph.AddEdge(e4_5);
            graph.AddEdge(e4_8);
            graph.AddEdge(e5_6); 
            graph.AddEdge(e5_9);
            graph.AddEdge(e6_10);
            graph.AddEdge(e6_11);
            graph.AddEdge(e8_9);
            graph.AddEdge(e9_10);
            graph.AddEdge(e10_11);
            graph.AddEdge(e1_10);

            return graph;
        }

        public static UndirectedGraph<string, Edge<string>> CreateSimpleGraph()
        {
            UndirectedGraph<string, Edge<string>> graph = new UndirectedGraph<string, Edge<string>>(true);
            // Add some vertices to the graph
            graph.AddVertex("1");
            graph.AddVertex("2");
            graph.AddVertex("3");
            graph.AddVertex("4");
            graph.AddVertex("5");
            graph.AddVertex("6");

            // Create the edges
            Edge<string> e1_2 = new Edge<string>("1", "2");
            Edge<string> e1_4 = new Edge<string>("1", "4");
            Edge<string> e4_1 = new Edge<string>("4", "2");
            Edge<string> e2_3 = new Edge<string>("2", "3");
            Edge<string> e3_6 = new Edge<string>("3", "6");
            Edge<string> e6_5 = new Edge<string>("6", "5");
            Edge<string> e2_5 = new Edge<string>("2", "5");

            // Add the edges
            graph.AddEdge(e1_2);
            graph.AddEdge(e1_4);
            graph.AddEdge(e4_1);
            graph.AddEdge(e2_3);
            graph.AddEdge(e3_6);
            graph.AddEdge(e6_5);
            graph.AddEdge(e2_5);
            return graph;
        }
    }
}
