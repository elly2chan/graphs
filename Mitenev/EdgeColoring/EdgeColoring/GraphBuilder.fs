namespace EdgeColoring

open System
open QuickGraph
open System.Collections.Generic

type GraphBuilder ()=
    member this.buildBadGraph() =
        let verteices = [1..7]
        let g = new UndirectedGraph<int, TaggedEdge<int, int>>()
        List.iter (fun v -> g.AddVertex(v) |> ignore) verteices
        let e1 = new TaggedEdge<int,int>(2, 6, 0)
        let e2 = new TaggedEdge<int,int>(7, 4, 0)
        let e3 = new TaggedEdge<int,int>(2, 4, 0)
        let e4 = new TaggedEdge<int,int>(1, 2, 0)
        let e5 = new TaggedEdge<int,int>(5, 3, 0)
        let e6 = new TaggedEdge<int,int>(3, 6, 0)
        let e7 = new TaggedEdge<int,int>(6, 5, 0)
        let e8 = new TaggedEdge<int,int>(6, 7, 0)
        let e9 = new TaggedEdge<int,int>(2, 5, 0)
        let e10 = new TaggedEdge<int,int>(1, 3, 0)
        let e11 = new TaggedEdge<int,int>(6, 4, 0)
        let e12 = new TaggedEdge<int,int>(3, 4, 0)
        let e13 = new TaggedEdge<int,int>(1, 6, 0)
        let e14 = new TaggedEdge<int,int>(3, 2, 0)
        let e15 = new TaggedEdge<int,int>(4, 5, 0)
        let e16 = new TaggedEdge<int,int>(3, 7, 0)
        let e17 = new TaggedEdge<int,int>(2, 7, 0)
        let e18 = new TaggedEdge<int,int>(5, 1, 0)
        let e19 = new TaggedEdge<int,int>(1, 4, 0)
        let e20 = new TaggedEdge<int,int>(7, 1, 0)

        g.AddEdge(e20) |> ignore
        g.AddEdge(e19) |> ignore
        g.AddEdge(e18) |> ignore
        g.AddEdge(e17) |> ignore
        g.AddEdge(e16) |> ignore
        g.AddEdge(e15) |> ignore
        g.AddEdge(e14) |> ignore
        g.AddEdge(e13) |> ignore
        g.AddEdge(e12) |> ignore
        g.AddEdge(e11) |> ignore

        g.AddEdge(e10) |> ignore
        g.AddEdge(e9) |> ignore
        g.AddEdge(e8) |> ignore
        g.AddEdge(e7) |> ignore
        g.AddEdge(e6) |> ignore
        g.AddEdge(e5) |> ignore
        g.AddEdge(e4) |> ignore
        g.AddEdge(e3) |> ignore
        g.AddEdge(e2) |> ignore
        g.AddEdge(e1) |> ignore

        g


    member this.buildRandomGraph () =
        let equalEdges (e1 : TaggedEdge<int,int>) (e2 : TaggedEdge<int,int>) = 
            (e1.Source = e2.Source && e1.Target = e2.Target) ||
            (e1.Source = e2.Target && e1.Target = e2.Source)

        let max = 9
        let min = 6
        let rnd = new Random()
        let vertCount = rnd.Next(min, max)
        let verteices = [1..vertCount]
        let g = new UndirectedGraph<int, TaggedEdge<int, int>>()
        List.iter (fun v -> g.AddVertex(v) |> ignore) verteices
      
        let maxEdges = (vertCount * (vertCount - 1)) / 2
        let edgesCount = maxEdges - 3
        let mutable currentEdges = 0
        let mutable edges = []

        while (currentEdges < edgesCount) do
            let start = rnd.Next(1, vertCount+1)
            let target = rnd.Next(1, vertCount+1)
            let tag = 0
            match start, target with
            | n, m when n = m -> ()
            | _ -> 
                let edge = new TaggedEdge<int,int>(start, target, tag)
                let isExists =  List.exists (fun e -> equalEdges e edge) edges
                match isExists with
                | true -> ()
                | _    -> edges <- edge :: edges
                          currentEdges <- currentEdges + 1

        g.AddEdgeRange(edges) |> ignore
        
        g

