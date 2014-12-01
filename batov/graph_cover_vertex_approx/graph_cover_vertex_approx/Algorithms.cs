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
using TVertex = System.String;

namespace graph_cover_vertex_approx
{
    class Algorithms
    {
        static public int minVertexCover(UndirectedGraph<string, Edge<string>> graph)
            {
               List<TVertex> alphabet = (List<TVertex>) graph.Vertices.ToList();
                int maxLength = graph.VertexCount;
                
                // This will hold all the strings which we create
                List<List<TVertex>> strings = new List<List<TVertex>>();

                // This will hold the string which we created the previous time through
                // the loop (they will all have length i in the loop)
                List<List<TVertex>> lastStrings = new List<List<TVertex>>();
                foreach (TVertex t in alphabet)
                {
                    // Populate it with the string of length 1 read directly from alphabet
                    lastStrings.Add(new List<TVertex> (new TVertex[] { t }));
                }

                // This holds the string we make by appending each element from the
                // alphabet to the strings in lastStrings
                List<List<TVertex>> newStrings;

                // Here we make string2 for each length 2 to maxLength
                for (int i = 0; i < maxLength; ++i)
                {
                    newStrings = new List<List<TVertex>>();
                    foreach (List<TVertex> s in lastStrings)
                    {
                        newStrings.AddRange(AppendElements(s, alphabet));
                    }
                    strings.AddRange(lastStrings);
                    foreach (List<TVertex> thecover in lastStrings)
                    {
                        if (Tools.isVertexCover(graph, thecover))
                            return i+1;
                    }
                    lastStrings = newStrings;
                }

                return 0;
            }

        static List<List<TVertex>> AppendElements(List<TVertex> list, List<TVertex> alphabet)
            {
                // Here we just append an each element in the alphabet to the given list,
                // creating a list of new string which are one element longer.
                List<List<TVertex>> newList = new List<List<TVertex>>();
                List<TVertex> temp = new List<TVertex>(list);
                foreach (TVertex t in alphabet)
                {
                    // Append the element
                    if (!temp.Contains(t))
                    {
                    temp.Add(t);

                    // Add our new string to the collection
                    newList.Add(new List<TVertex>(temp));

                    // Remove the element so we can make another string using
                    // the next element of the alphabet
                    temp.RemoveAt(temp.Count - 1);
                    }
                }
                return newList;
            }

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
                if (!coverSet.Contains(source))
                coverSet.Add(source);
                if (!coverSet.Contains(target))
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

              source = rEdge.Source;
              target = rEdge.Target;

              if (inputGraph.AdjacentDegree(rEdge.Source) > 1)
              {       
                  if (!coverSet.Contains(source))
                  coverSet.Add(source);
              }

              if (inputGraph.AdjacentDegree(rEdge.Target) > 1)
              {               
                  if (!coverSet.Contains(target))
                  coverSet.Add(target);
              }

              if ((inputGraph.AdjacentDegree(rEdge.Target) == 1) && (inputGraph.AdjacentDegree(rEdge.Source) == 1))
              {
                  if (!coverSet.Contains(source))
                  coverSet.Add(source);
                  IEnumerable<Edge<TVertex>> toDel = inputGraph.AdjacentEdges(source);
                  inputGraph.RemoveEdges(toDel.ToList());

              }
              else
              {
                  IEnumerable<Edge<string>> toDel = inputGraph.AdjacentEdges(target).Concat(inputGraph.AdjacentEdges(source));
                  inputGraph.RemoveEdges(toDel.ToList());
              }
          }
          return coverSet;
      }
    }
}
