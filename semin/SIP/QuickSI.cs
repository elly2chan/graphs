using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;

namespace SIP
{
	using Graph = UndirectedGraph<Vertex, Edge>;
	using WeightedGraph = UndirectedGraph<WeightedVertex, WeightedEdge>;

	public class QuickSI
	{

		Graph g; 
		Vertex[] gvs;
		List<SEQ.Item> seq; 
		List<int>[] adj;
		int[] h; 
		bool[] f; 
		int alpha;
		int beta;

		public QuickSI (List<SEQ.Item> seq, Graph g)
		{
			this.g = g;
			this.gvs = g.Vertices.ToArray ();
			this.alpha = g.VertexCount;
			this.beta = seq.Count;
			this.h = new int[beta];
			this.f = new bool[alpha];
			this.seq = seq;
			this.adj = mkAdjacencyLists (alpha, g.Edges);
			this.Result = algorithm (0);
		}

		public bool Result { get; private set; }

		bool algorithm(int d)
		{
			if (d >= beta) {
				return true;
			}
			var t = seq [d];

			for (int vNum = 0; vNum < alpha; vNum++) {
				var v = gvs [vNum];
				if (f [v.ID] || !v.Label.Equals (t.Label)) continue;
				if (d != 0 && !gHasEdge (v.ID, h [t.Parent])) continue;
				if (degreeInG (v) < t.Degree) continue;
				if (!t.ExtraEdges.All (p => gHasEdge (v.ID, h [p]))) continue;
				h [t.Vertex] = v.ID;
				f [v.ID] = true;
				if (algorithm (d + 1)) return true;
				f [v.ID] = false;
			}
			return false;
		}

		bool gHasEdge(int vID, int pID)
		{
			return (adj [vID].Count < adj [pID].Count) ?
				adj [vID].Contains (pID) :
				adj [pID].Contains (vID);
		}

		int degreeInG(Vertex v) {
			return adj[v.ID].Count();
		}

		static List<int>[] mkAdjacencyLists(int vCount, IEnumerable<Edge> edges) {
			var table = new List <int>[vCount];
			for (int i = 0; i < vCount; i++) {
				table [i] = new List<int> ();
			}
			foreach (var e in edges) {
				table [e.Source.ID].Add (e.Target.ID);
				table [e.Target.ID].Add (e.Source.ID);
			}
			return table;
		}

	}
}

