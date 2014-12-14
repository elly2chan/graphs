using System;
using QuickGraph;

namespace SIP
{
	public class Edge : IEdge<Vertex>, IComparable
	{

		Vertex src;
		Vertex tgt;

		public Edge(Vertex src, Vertex tgt)
		{
			this.src = src;
			this.tgt = tgt;
		}	

		public Vertex Source { get { return src; } }
		public Vertex Target { get { return tgt; } }

		public int CompareTo(object obj)
		{
			if (obj == null) return 1;
			var other = obj as Edge;
			if (other != null) return this.Equals (other) ? 0 : 1;
			throw new ArgumentException ("Object is not an Edge");
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			var other = obj as Edge;
			if (other != null) return this.Source.Equals(other.Source) && this.Target.Equals(other.Target);
			throw new ArgumentException ("Object is not an Edge");
		}

		public override int GetHashCode()
		{
			return this.Source.GetHashCode () ^ this.Target.GetHashCode ();
		}

		public static bool UndirectedEquals(IEdge<Vertex> e1, IEdge<Vertex> e2)
		{
			return (e1.Source.Label.Equals (e2.Source.Label) && 
				e1.Target.Label.Equals (e2.Target.Label)) || 
				(e1.Target.Label.Equals (e2.Source.Label) && 
				e1.Source.Label.Equals (e2.Target.Label));
		}
		
		public override string ToString ()
		{
			return string.Format ("[{0} -> {1}]", Source, Target);
		}

	}
}

