using System;
using QuickGraph;

namespace SIP
{
	public class WeightedEdge : Edge, IEdge<WeightedVertex>
	{
		public WeightedEdge(WeightedVertex src, WeightedVertex tgt, int w) : base(src, tgt)
		{
			this.Weight = w;
			this.Source = src;
			this.Target = tgt;
		}	
		
		public int Weight { get; private set; }
		new public WeightedVertex Source { get; private set; }
		new public WeightedVertex Target { get; private set; }

		public override string ToString ()
		{
			return string.Format ("[{0} -> {1}]", Source, Target);
		}
	}
}

