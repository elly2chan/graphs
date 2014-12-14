module SupportFun
open QuickGraph

let equalEdges (e1 : TaggedEdge<int,int>) (e2 : TaggedEdge<int,int>) = 
        (e1.Source = e2.Source && e1.Target = e2.Target) ||
        (e1.Source = e2.Target && e1.Target = e2.Source)

let anotherVertexInEdge (edge : TaggedEdge<int,int>) vert = 
     if edge.Source = vert then edge.Target else edge.Source