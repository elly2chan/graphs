using System;
using QuickGraph;
using System.Linq;
using System.Collections.Generic;

namespace SIP
{
	using Graph = UndirectedGraph<Vertex, Edge>;
	public class RandomGraph
	{
		RandomGraph (){}

		static Random rnd = new Random();
		static string alphabet = "ABCDEFGHIJKLMNOPRSTUVWXYZ";

		public static Graph OneVertexGraph() {
			var q = new Graph (false);
			q.AddVertex (new Vertex(0, rndLabel ()));
			return q;
		}

		public static Graph GenerateGraph(int vCount, int ePercent)
		{
			return ExtendGraph (OneVertexGraph (), vCount - 1, ePercent);
		}

		public static Graph ExtendGraph(Graph q, int vCount, int ePercent)
		{
			var oldVc = q.VertexCount;

			Vertex[] qvs = q.Vertices.ToArray();

			Vertex[] gvsArr = new Vertex[vCount];
			List<Vertex> vs = new List<Vertex> ();
			vs.AddRange (q.Vertices);
			List<Edge> es = new List<Edge> ();
			es.AddRange (q.Edges);

			for (int i = 0; i < vCount; i++) {
				var nv = new Vertex (i + oldVc, rndLabel ());
				gvsArr [i] = nv;
				vs.Add (nv);
				var connected = false;
				for (int j = 0; j < oldVc + i; j++) {
					if (rnd.Next (100) < ePercent) {
						es.Add (new Edge ((j < oldVc) ? qvs [j] : gvsArr[j - oldVc], nv));
						connected = true;
					}
				}
				if (!connected) {
					int vIndex = rnd.Next (oldVc + i);
					es.Add (new Edge((vIndex < oldVc) ? qvs[vIndex] : gvsArr[vIndex - oldVc], nv));
				}
			}

			var g = new Graph (false);
			g.AddVertexRange (shuffle(vs));
			g.AddEdgeRange (shuffle(es));
			return g;
		}

		static string rndLabel()
		{
			return alphabet[rnd.Next (alphabet.Length)].ToString();
		}

		static IEnumerable<T> shuffle<T>(IEnumerable<T> list) {
			var count = list.Count ();
			var arr = list.ToArray ();
			for (int i = 0; i < 10 * count; i++) {
				var i1 = rnd.Next (count);
				var i2 = rnd.Next (count);
				var e = arr [i1];
				arr [i1] = arr [i2];
				arr [i2] = e;
			}
			return arr;
		}
	}
}

