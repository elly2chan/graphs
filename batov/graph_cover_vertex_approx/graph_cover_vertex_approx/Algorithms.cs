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
using TVertex = System.String;

namespace graph_cover_vertex_approx
{
    class Algorithms
    {
       static Random rnd = new Random();
        public static VertexList<TVertex> standartAlgo(UndirectedGraph<string, Edge<string>> inputGraph)
        {
            VertexList<TVertex> coverSet = new VertexList<TVertex>();
            while (!inputGraph.IsEdgesEmpty)
            {
                //Get random edge
                int rNum = rnd.Next(inputGraph.Edges.Count() - 1);
                Edge<string> rEdge = inputGraph.Edges.ElementAt(rNum);
                //nears
                TVertex source = rEdge.Source;
                TVertex target = rEdge.Target;
                coverSet.Add(source);
                coverSet.Add(target);
                //removes
                IEnumerable <Edge<TVertex>> toDel = inputGraph.AdjacentEdges(target).Concat(inputGraph.AdjacentEdges(source));
                inputGraph.RemoveEdges(toDel.ToList());
            }
            return coverSet;
        }

        public static VertexList<TVertex> lsoAlgo(UndirectedGraph<string, Edge<string>> inputGraph)
      {
          VertexList<TVertex> coverSet = new VertexList<TVertex>();
          TVertex source = "-1";
          TVertex target = "-1";

          while (!inputGraph.IsEdgesEmpty)
          {
              List<Edge<string>> toRemove = new List<Edge<string>>();
              //Get random edge
              int rNum = rnd.Next(inputGraph.Edges.Count() - 1);
              Edge<string> rEdge = inputGraph.Edges.ElementAt(rNum);
              //nears
            
              if (inputGraph.AdjacentDegree(rEdge.Source) > 1)
              {
                  source = rEdge.Source;
                  coverSet.Add(source);
              }

              if (inputGraph.AdjacentDegree(rEdge.Target) > 1)
              {
                  target = rEdge.Target;
                  coverSet.Add(target);
              }

              if ((inputGraph.AdjacentDegree(rEdge.Target) == 1) && (inputGraph.AdjacentDegree(rEdge.Source) == 1))
              {
                  source = rEdge.Source;
                  coverSet.Add(source);
                  IEnumerable<Edge<TVertex>> toDel = inputGraph.AdjacentEdges(source);
                  inputGraph.RemoveEdges(toDel.ToList());

              }
              else
              {
                  IEnumerable<Edge<TVertex>> toDel = inputGraph.AdjacentEdges(target).Concat(inputGraph.AdjacentEdges(source));
                  inputGraph.RemoveEdges(toDel.ToList());
              }
          }
          return coverSet;
      }
    }
}
