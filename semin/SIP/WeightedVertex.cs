using System;

namespace SIP
{
	public class WeightedVertex : Vertex
	{
		
		public WeightedVertex(int id, String lbl, int w): base(id, lbl)
		{
			this.Weight = w;
		}

		public int Weight { get; private set; }

		public override string ToString ()
		{
			return string.Format ("{1}({0})", ID, Label);
		}
	}
}

