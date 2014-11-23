using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using System.Windows.Media;

namespace ViewGraph
{
    class GraphConverter
    {
        public BidirectionalGraph<object, IEdge<object>> Convert(UndirectedGraph<int, TaggedEdge<int, int>> graph) 
        {
            var edges = graph.Edges;
            var convertedGraph = new BidirectionalGraph<object, IEdge<object>>();
            foreach (var edge in edges)
            {
                var sourse = edge.Source;
                var target = edge.Target;
                var color = TagToColor(edge.Tag);

                convertedGraph.AddVerticesAndEdge(new MyEdge(sourse, target)
                {
                    EdgeColor = color
                });
            }
            return convertedGraph;
        }

        private Color TagToColor(int tag)
        {
            //int maxVal = 256;
            //byte r = (byte)((tag * 411) % maxVal);
            //byte g = (byte)((tag * 317) % maxVal);
            //byte b = (byte)((tag * 611) % maxVal);
            //return Color.FromRgb(r, g, b);
            switch (tag)
            {
                case 1:
                    return Colors.Blue;
                case 2:
                    return Colors.Red;
                case 3:
                    return Colors.Green;
                case 4:
                    return Colors.Yellow;
                case 5:
                    return Colors.Brown;
                case 6:
                    return Colors.Purple;
                case 7:
                    return Colors.Orange;
                case 8:
                    return Colors.Pink;
                case 9:
                    return Colors.DarkRed;
                case 10:
                    return Colors.Beige;
                case 11:
                    return Colors.Lime;
                case 12:
                    return Colors.Azure;
                default:
                    return Colors.Black;
            }
        }
    }


}
