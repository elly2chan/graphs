namespace EdgeColoring

open System
open QuickGraph
open System.Collections.Generic

type EdgePainter ()=
    member this.edgeColoring (graph : UndirectedGraph<int, TaggedEdge<int, int>>) = 
        let maxDelta = Seq.map (fun v -> graph.AdjacentDegree(v)) graph.Vertices |> Seq.max
        let colors = [1..maxDelta + 1]

        let equalEdges (e1 : TaggedEdge<int,int>) (e2 : TaggedEdge<int,int>) = 
            (e1.Source = e2.Source && e1.Target = e2.Target) ||
            (e1.Source = e2.Target && e1.Target = e2.Source)

        let isColorMissing vertex color = 
            graph.AdjacentEdges(vertex) 
            |> Seq.exists (fun e -> e.Tag = color) 
            |> not

        let firstMissingColor v =
            let colors = List.filter (fun c -> isColorMissing v c) colors
            colors.Head

        let rec makeSequence sourse target fanSeq = 
            let canAddEdge c = 
                graph.AdjacentEdges(sourse) 
                |> Seq.tryFind (fun edge -> edge.Tag = c &&
                                            not <| List.exists (fun f -> equalEdges f edge) fanSeq)
            let targetColorMissing = firstMissingColor target
            match canAddEdge targetColorMissing with
            | Some edge -> 
                    let anowerVertex = if edge.Source = sourse then edge.Target else edge.Source
                    makeSequence sourse anowerVertex (edge :: fanSeq)
            | None   -> fanSeq

        let rec shift (listEdges : TaggedEdge<int, int> list) =
            match listEdges with
            | e1 :: e2 :: edges ->
                e1.Tag <- e2.Tag
                shift (e2 :: edges)
            | e1 :: edges -> e1.Tag <- 0
            | _ -> ()
        
        let printEdge (e : TaggedEdge<int,int>) = printfn "%d %d %d" e.Source e.Target e.Tag

        let printEdges edges = 
            List.iter (fun (e : TaggedEdge<int,int>) -> printEdge e) edges

        let rec funSeqBeforeEdge s e res =
             match s with
             | hd :: tl when not <| equalEdges hd e -> funSeqBeforeEdge tl e (hd :: res)
             | hd :: tl                             -> hd :: res
             | []                                   -> res

        let makeSubGraph color1 color2 =
            let subGraph = new UndirectedGraph<int, TaggedEdge<int, int>>();
            Seq.iter (fun v -> subGraph.AddVertex(v) |> ignore) graph.Vertices
            Seq.iter (fun (e : TaggedEdge<int, int>) -> if e.Tag = color1 || e.Tag = color2 
                                                        then subGraph.AddEdge(e) |> ignore) graph.Edges
            subGraph

        let rec makeConnectedComponent (subGraph : UndirectedGraph<int, TaggedEdge<int, int>>) vertex (comp : TaggedEdge<int, int> list) = 
            let listOfEdges = Seq.toList <| subGraph.AdjacentEdges(vertex) 
            let filteredlist = List.filter (fun e -> not <| List.exists (fun f -> equalEdges e f) comp) listOfEdges          
            match filteredlist with
            | e :: es -> 
                         let another = if e.Source = vertex then e.Target else e.Source
                         makeConnectedComponent subGraph another (e :: comp)
            | _      -> comp

        let inverseColorInComponent (comp : TaggedEdge<int, int> list) color1 color2 = 
            Seq.iter (fun (e : TaggedEdge<int, int>) -> if e.Tag = color1 then e.Tag <- color2 else e.Tag <- color1) comp
               
        let ColoringOneEdge (edge : TaggedEdge<int, int>) = 
            let sourse = edge.Source
            let sourseMissingColor =  firstMissingColor sourse
            let target = edge.Target
            let missingColorInBoth = 
                List.tryFind (fun c -> isColorMissing sourse c &&
                                       isColorMissing target c) colors
                                     
            match missingColorInBoth with
            | Some c  -> edge.Tag <- c 
                         printfn "%s" "missingColorInBoth"
            | None    -> 
                printfn "%s" "not missingColorInBoth"
                let fanSeq = makeSequence sourse target [edge]
                let lastEdge  =  fanSeq.Head
                let anowerVertex = if lastEdge.Source = sourse then lastEdge.Target else lastEdge.Source

                let lastColor = firstMissingColor anowerVertex
                let edgeWithLastColor = List.tryFind (fun (e : TaggedEdge<int, int>) -> e.Tag = lastColor) fanSeq
                let reverseFanSeq = List.rev fanSeq
                match edgeWithLastColor with
                | Some e -> 
                    let edgeWithLastColorTarget = if e.Source = sourse then e.Target else e.Source
                    let beforeEdge = funSeqBeforeEdge reverseFanSeq e []
                    shift <| List.rev beforeEdge

                    let subgraph = makeSubGraph sourseMissingColor lastColor
                    let componentVj = makeConnectedComponent subgraph anowerVertex []
                    let componentVk = makeConnectedComponent subgraph edgeWithLastColorTarget []

                    let isComponentVkContainsV0 = List.exists (fun (e : TaggedEdge<int, int>) -> e.Source = sourse || e.Target = sourse) componentVk
                    match isComponentVkContainsV0 with
                    | true -> printfn "%s" "case 2.1"
                              let fanVkVj = funSeqBeforeEdge  fanSeq e []
                              shift fanVkVj
                              inverseColorInComponent componentVj  sourseMissingColor lastColor
                              lastEdge.Tag <- sourseMissingColor                              
                    | _    -> printfn "%s" "case 2.2"
                              inverseColorInComponent componentVk  sourseMissingColor lastColor
                              e.Tag <- sourseMissingColor
                       
                | _      -> printfn "%s" "case 1"
                            shift reverseFanSeq
                            lastEdge.Tag <- lastColor

        List.iter ColoringOneEdge <| Seq.toList graph.Edges



