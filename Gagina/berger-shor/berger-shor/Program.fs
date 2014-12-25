open System
open System.Collections.Generic
open System.Linq
open QuickGraph
open System.Diagnostics

type Graph() =

    static member graphFromEdges(edges : (string * string) list) =
        let graph = new AdjacencyGraph<string, Edge<string>>()
        List.iter (fun (source, target) -> graph.AddVerticesAndEdge(new Edge<string>(source, target)) |> ignore) edges
        graph

    static member graphFromVertices(n : int) =
        let graph = new AdjacencyGraph<string, Edge<string>>()
        List.iter (fun x -> graph.AddVertex(x.ToString()) |> ignore) [1 .. n]
        graph

    static member randomGraph(n : int) =
        let rnd = new Random()
        let vertices = [ for i in 1 .. n do yield i.ToString() ]
            
        //let numberOfEdges = rnd.Next(1, n * (n - 1) + 1)
        let numberOfEdges = n * (n - 1) * 100/100  //% of filling

        let createEdges numberOfEdges numberOfVertices = 
            
            let rec createEdge' (list : (int * int) list) edge =
                if list.Contains(edge) then createEdge' list (rnd.Next(1, numberOfVertices + 1), rnd.Next(1, numberOfVertices + 1))
                                       else edge :: list
           
            let rec createEdges' (list : (int * int) list) n =
                if list.Length = n then list
                                   else createEdges' (createEdge' list (rnd.Next(1, numberOfVertices), rnd.Next(1, numberOfVertices))) n

            let rec deleteLoops (list : (int * int) list) =
                if List.exists (fun (x, y) -> x = y) list then deleteLoops (List.filter (fun (x, y) -> x <> y) list)
                                                          else list

            deleteLoops <| createEdges' [] numberOfEdges

        let edges = List.map (fun (x, y) -> (x.ToString(), y.ToString())) (createEdges numberOfEdges n)
        let graph = new AdjacencyGraph<string, Edge<string>>(false, n, numberOfEdges)
        List.iter (fun (source, target) -> graph.AddVerticesAndEdge(new Edge<string>(source, target)) |> ignore) edges
        graph
        
            


    static member private removeIncomingEdges(initial : AdjacencyGraph<string, Edge<string>>,
                                              destination : AdjacencyGraph<string, Edge<string>>,
                                              feedbackArcSet : AdjacencyGraph<string, Edge<string>>,
                                              vertex : string) =
        //Add outcoming edges to the resulting graph and remove them from initial
        for e in initial.Edges.Where(fun e -> e.Source = vertex) do
            destination.AddEdge(e) |> ignore
        initial.RemoveEdgeIf(fun e -> e.Source = vertex) |> ignore

        //Add incoming edges to feedback arc set and remove them from initial
        for e in initial.Edges.Where(fun e -> e.Target = vertex) do
            feedbackArcSet.AddEdge(e) |> ignore
        initial.RemoveEdgeIf(fun e -> e.Target = vertex) |> ignore
        ()

    static member private removeOutcomingEdges(initial : AdjacencyGraph<string, Edge<string>>,
                                               destination : AdjacencyGraph<string, Edge<string>>,
                                               feedbackArcSet : AdjacencyGraph<string, Edge<string>>,
                                               vertex : string) =
        //Add incoming edges to the resulting graph
        for e in initial.Edges.Where(fun e -> e.Target = vertex) do
            destination.AddEdge(e) |> ignore
        //Remove incoming edges from initial
        initial.RemoveEdgeIf(fun e -> e.Target = vertex) |> ignore

        //Add outcoming edges to feedback arc set
        for e in initial.Edges.Where(fun e -> e.Source = vertex) do
            feedbackArcSet.AddEdge(e) |> ignore
        //Remove outcoming edges from initial
        initial.RemoveEdgeIf(fun e -> e.Source = vertex) |> ignore
        ()

    static member private inDegree(graph : AdjacencyGraph<string, Edge<string>>, vertex : string) =
        let g = graph.Clone()
        g.RemoveEdgeIf(fun e -> e.Target = vertex)

    static member private outDegree(graph : AdjacencyGraph<string, Edge<string>>, vertex : string) =
        let g = graph.Clone()
        g.RemoveEdgeIf(fun e -> e.Source = vertex)

    static member algorithm(graph : AdjacencyGraph<string, Edge<string>>) =
        let initial = graph
        let destination = new AdjacencyGraph<string, Edge<string>>()
        let feedbackArcSet = new AdjacencyGraph<string, Edge<string>>()
        //Add initial vertices
        for i in graph.Vertices do
            destination.AddVertex(i) |> ignore
            feedbackArcSet.AddVertex(i) |> ignore
        //Berger-Shor algorithm
        for i in graph.Vertices do
            if Graph.inDegree(graph, i) > Graph.outDegree(graph, i) 
                then Graph.removeOutcomingEdges(initial, destination, feedbackArcSet, i)
                else Graph.removeIncomingEdges(initial, destination, feedbackArcSet, i)
        //destination, feedbackArcSet, initial.IsEdgesEmpty
        destination

[<EntryPoint>]
let main argv = 

    //let g = Graph.randomGraph(100)
    let g = Graph.randomGraph(0)
    let s = new Stopwatch()
    s.Start()
    let dest = Graph.algorithm(g)
    s.Stop()
    //printfn " %A" (int s.ElapsedMilliseconds) 
    printfn "%A" <| Seq.map (fun x -> x.ToString()) dest.Edges

    //Graph.buildGraph([("A", "B"); ("A", "C"); ("A", "D"); ("B", "D"); ("C", "E"); ("D", "E"); ("E", "D"); ("E", "A")])
   
    0 // return an integer exit code
