using System;
using System.Collections.Generic;
using QuickGraph;
using System.Linq;

namespace SIP
{
	using Graph = UndirectedGraph<Vertex, Edge>;
	using WeightedGraph = UndirectedGraph<WeightedVertex, WeightedEdge>;
	using Label = String;
	using TVFT = Dictionary<String, int>;
	using TEFT = Dictionary<Tuple<String, String>, int>;

	public class SEQ
	{

		SEQ() {}

		public static List<Item> BuildSEQ(Graph q, Graph g)
		{
			// Base data structures
			HashSet<int> vt = new HashSet<int>(); // verticies in seq (or in minimun spanning tree)
			List<Item> seq = new List<Item>();

			// Initialize tabels
			TVFT vft = mkVFT (q, g); // vertex frequency table
			TEFT eft = mkEFT (q, g); // edge frequency table
			WeightedGraph qw = weightGraph (vft, eft, q);

			// First step: find first candidates 
 			var vCount = q.VertexCount;
			var p = lightestEdges (qw);
			var e = selectFirstEdge (p.ToList (), qw);

			// Add first edge
			int srcID = e.Source.ID;
			int tgtID = e.Target.ID;
			vt.Add (srcID); vt.Add (tgtID); 
			seq.Add (new Item(-1,  srcID, e.Source.Label, degree(q, srcID), new List<int>()));
			seq.Add (new Item(srcID, tgtID, e.Target.Label, degree(q, tgtID), new List<int>()));
			qw.RemoveEdge (e);

			// Loop while building whole SEQ
			while (vt.Count() < vCount) {
				p = front (qw, vt); // Select next suitable set of verticies
				e = selectSpanningEdge (p, qw, vt); // Next best edge

				// Add currently chosen vertex to SEQ
				var outerVert = outerVertex (vt, e);
				var innerVert = (e.Source != outerVert) ? e.Source : e.Target;
				vt.Add (outerVert.ID);
				qw.RemoveEdge (e);

				// Other edges not included to tree but fully contained by its verticies
				var absorbed = from ee in qw.Edges 
					where (ee.Target.ID == outerVert.ID || ee.Source.ID == outerVert.ID) && 
						  vt.Contains (ee.Source.ID) && vt.Contains (ee.Target.ID) 
					orderby ee.Weight 
					select ee;
				var extras = new List<int> ();

				foreach (var ee in absorbed) {
					extras.Add ((ee.Source.ID != outerVert.ID) ? ee.Source.ID : ee.Target.ID);
					qw.RemoveEdge (ee);
				}
				seq.Add(new Item(innerVert.ID, outerVert.ID, outerVert.Label, degree(q, outerVert.ID), extras));
			}

			return seq;
		}

		static Dictionary<Label, int> mkVFT(Graph q, Graph g)
		{
			var	ft = new Dictionary<Label, int> ();
			foreach (var v in q.Vertices) {
				if (!ft.ContainsKey (v.Label)) {
					ft [v.Label] = g.Vertices.Count (v2 => v2.Label.Equals (v.Label));
				}
			}
			return ft;
		}

		static Dictionary<Tuple<Label, Label>, int> mkEFT(Graph q, Graph g)
		{
			var ft = new Dictionary<Tuple<Label, Label>, int> ();
			foreach (var e in q.Edges) {
				var key = Tuple.Create (e.Source.Label, e.Target.Label);
				var key2 = Tuple.Create (e.Target.Label, e.Source.Label);
				if (!(ft.ContainsKey (key) || ft.ContainsKey (key2))) {
					ft [key] = g.Edges.Count (e2 => Edge.UndirectedEquals (e, e2));
					ft [key2] = ft [key];
				}
			}
			return ft;
		}

		static WeightedGraph weightGraph(TVFT vft, TEFT eft, Graph q)
		{
			var qq = new WeightedGraph (false);
			qq.AddVerticesAndEdgeRange (
				from e in q.Edges select weightEdge(vft, eft, e)
			);
			return qq;
		}

		static WeightedEdge weightEdge(TVFT vft, TEFT eft, IEdge<Vertex> e)
		{
			var src = new WeightedVertex (e.Source.ID, e.Source.Label, vft [e.Source.Label]);
			var tgt = new WeightedVertex (e.Target.ID, e.Target.Label, vft [e.Target.Label]);
			var w = eft [Tuple.Create (src.Label, tgt.Label)];
			return new WeightedEdge(src, tgt, w);
		}

		static IEnumerable<WeightedEdge> lightestEdges(WeightedGraph q)
		{
			var minWeight = q.Edges.Min(e => e.Weight);
			return q.Edges.Where (e => e.Weight == minWeight);
		}

		static WeightedEdge selectFirstEdge(IEnumerable<WeightedEdge> p, WeightedGraph q)
		{
			if (p.Count() > 1) {
				Func<WeightedVertex, int> degree = v => q.AdjacentEdges(v).Count();
				Func<WeightedEdge, int> edgeDegree = e => degree (e.Source) + degree (e.Target);
				var minEdgeDegree = q.Edges.Min (edgeDegree);
				p = p.Where (e => edgeDegree (e) == minEdgeDegree);
			}
			return p.First ();
		}

		static int indGCount(WeightedGraph q, HashSet<int> vs)
		{
			return q.Edges.Count ( 
			    e => vs.Contains (e.Source.ID) && vs.Contains (e.Target.ID)
			);
		}

		static WeightedVertex outerVertex(HashSet<int> vs, WeightedEdge e)
		{
			return (vs.Contains (e.Source.ID)) ? e.Target : e.Source;
		}

		static WeightedEdge selectSpanningEdge(IEnumerable<WeightedEdge> p, WeightedGraph q, HashSet<int> vs)
		{
			if (p.Count () > 1) {
				Func<WeightedEdge, int> ind = e => {
					var o = outerVertex (vs, e);
					vs.Add (o.ID);
					var indg = indGCount (q, vs);
					vs.Remove (o.ID);
					return indg;
				};
				var maxIndG = q.Edges.Max (ind);
				p = p.Where (e => ind (e) == maxIndG);
			}
			if (p.Count () > 1) {
				Func<WeightedEdge, int> outerDegree = e => q.AdjacentEdges(outerVertex (vs, e)).Count();
				var minDegree = q.Edges.Min (outerDegree);
				p = p.Where (e => outerDegree (e) == minDegree);
			}
			return p.First ();
		}

		static int degree(Graph q, int vID) {
			var vert = (from v in q.Vertices where v.ID == vID select v).First();
			return q.AdjacentEdges (vert).Count ();
		}

		static IEnumerable<WeightedEdge> front(WeightedGraph q, HashSet<int> vs)
		{
			List<WeightedEdge> es = new List<WeightedEdge> ();
			foreach (var e in q.Edges) {
				var src = e.Source;
				var tgt = e.Target;
				var srcin = vs.Contains (src.ID);
				var tgtin = vs.Contains (tgt.ID);
				if (srcin ^ tgtin) {
					es.Add (e);
				}
			}
			return es;
		}

		public class Item
		{
			public Item(int parent, 
			            int vertex, 
			            Label label, 
			            int degree, 
			            List<int> extraEdges)
			{
				this.Parent = parent;
				this.Vertex = vertex;
				this.Label = label;
				this.Degree = degree;
				this.ExtraEdges = extraEdges;
			}

			public int Parent { get; private set; }
			public int Vertex { get; private set; }
			public String Label { get; private set; }
			public int Degree { get; private set; }
			public List<int> ExtraEdges { get; private set; }

			public override string ToString ()
			{
				return string.Format ("[{0} -> {1}({2}); d({3})]", Parent, Vertex, Label, Degree);
			}
		}
	}
}

