using System;
using QuickGraph;
using System.Linq;
using System.Collections.Generic;

namespace SIP
{
	using Graph = UndirectedGraph<Vertex, Edge>;
	using WeightedGraph = UndirectedGraph<WeightedVertex, WeightedEdge>;

	public class UllmanSIP
	{

		Graph q;
		Graph g; 
		Vertex[] qvs;
		Vertex[] gvs;
		int[] h; 
		bool[] f; 
		int alpha;
		int beta;

		public UllmanSIP (Graph q, Graph g)
		{
			this.q = q;
			this.g = g;
			this.qvs = q.Vertices.ToArray ();
			this.gvs = g.Vertices.ToArray ();
			this.alpha = g.VertexCount;
			this.beta = q.VertexCount;
			this.h = new int[beta];
			this.f = new bool[alpha];

			this.Result = algorithm (0);
		}

		public bool Result { get; private set; }

		bool algorithm(int d)
		{
			if (d >= beta) {
				return true;
			}

			for (int k = 0; k < alpha; k++) {
				if (!f [k] && qvs[d].Label.Equals (gvs[k].Label) && gHasEdges (d, k)) {
					f [k] = true;
					h [d] = k;
					if (algorithm (d + 1)) {
						return true;
					}
					f [k] = false;
				}
			}

			return false;
		}

		bool gHasEdges(int vq, int vg)
		{
			for (int iq = 0; iq < vq; iq++) {
				if (q.ContainsEdge (qvs[iq], qvs[vq]) || 
				    q.ContainsEdge (qvs[vq], qvs[iq])) {
					int ig = h [iq];
					if (!(g.ContainsEdge (gvs[ig], gvs[vg]) || 
					    g.ContainsEdge (gvs[vg], gvs[ig]))) {
						return false;
					}
				}
			}
			return true;
		}

	}
}

