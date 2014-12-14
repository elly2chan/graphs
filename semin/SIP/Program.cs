using System;
using QuickGraph;
using System.Diagnostics;
using System.Collections.Generic;

namespace SIP
{
	using Graph = UndirectedGraph<Vertex, Edge>;
	using WeightedGraph = UndirectedGraph<WeightedVertex, WeightedEdge>;

	class MainClass
	{

		class TestRun {

			public TestRun(int runCount, Graph q, Graph g) {
				run (runCount, q, g); 
			}

			public TestRun(Graph q, Graph g) : this (1, q, g) {}

			void run(int runCount, Graph q, Graph g) {
				long sumTimeQuick = 0;
				long sumTimeUllman = 0;
				var seq = SEQ.BuildSEQ (q, g);
				Match = true;

				for (int runNumber = 0; runNumber < runCount; runNumber++) {
					var watch = Stopwatch.StartNew();
					var resultQuick = new QuickSI (seq, g).Result;
					watch.Stop();
					var ms = watch.ElapsedMilliseconds;
					sumTimeQuick += ms;

					watch = Stopwatch.StartNew ();
					var resultUllman = new UllmanSIP (q, g).Result;
					watch.Stop ();
					ms = watch.ElapsedMilliseconds;
					sumTimeUllman += ms;

					if (resultQuick ^ resultUllman)
						Match = false;
				}

				var avgTimeQuick = sumTimeQuick / runCount;
				var avgTimeUllman = sumTimeUllman / runCount;
				TimeQuickSI = avgTimeQuick;
				TimeUllman = avgTimeUllman;
			}

			public bool Match { get; set; }
			public long TimeQuickSI { get; set; }
			public long TimeUllman { get; set; }

			public string ToString (int vCount)
			{
				return (!Match) ? 
					(vCount + " : mismatch") : 
					(vCount + " :\t\t" + TimeUllman + "\t\t" + TimeQuickSI); 
			}
		}

		static void FixedDataTest(int amountOfSteps,
		                          int dataVertexCount,
		                          int dataEdgePercentage,
		                          int startPatternSize,
		                          int patternEdgePercantage,
		                          int patternStepGrowth) {
			var patternVertexCount = startPatternSize;
			var dataGraph = RandomGraph.GenerateGraph (dataVertexCount, dataEdgePercentage);
			var pattern = RandomGraph.GenerateGraph (patternVertexCount, patternEdgePercantage);
			var extend = (patternVertexCount * patternStepGrowth) / 100;
			for (int step = 0; step < amountOfSteps; step++) {
				var test = new TestRun (5, pattern, dataGraph);
				print (test.ToString (patternVertexCount)); 
				patternVertexCount += extend;
				if (patternVertexCount > dataVertexCount) break;
				pattern = RandomGraph.ExtendGraph (pattern, extend, patternEdgePercantage);
				extend = (patternVertexCount * patternStepGrowth) / 100;
			}
		}

		static void FixedPatternTest(int amountOfSteps, 
		                             int patternVertexCount, 
		                             int patternEdgePercentage, 
		                             int dataGraphStepGrowth) {
			var vCountData = patternVertexCount;
			var q = RandomGraph.GenerateGraph (patternVertexCount, patternEdgePercentage);
			for (int step = 0; step < amountOfSteps; step++) {
				var extend = ((vCountData * dataGraphStepGrowth) / 100);
				var g = RandomGraph.ExtendGraph (q, extend, 50);
				var test = new TestRun (5, q, g);
				vCountData += extend;
				print (test.ToString (vCountData)); 
			}
		}

		public static void Main (string[] args)
		{
			print ("Hello, World!");
		}

		static void print(String s) {
			Console.WriteLine (s);
		}

		static void printGraph(Graph q) {
			print ("Vertex count = " + q.VertexCount);
			foreach (var v in q.Vertices) {
				print(v.ToString ()); 
			}
			print ("Edge count = " + q.EdgeCount);
			foreach (var e in q.Edges) {
				print(e.ToString ()); 
			}
		}

	}
}

