module AchromaticNum

open AchromaticNum.AchromaticPartition
open QuickGraph
open System.Collections.Generic

type Vertex = int
type Edge = SEdge<int>

let BruteForcePrecise (graph: UndirectedGraph<Vertex, Edge>) = 
    let Colors = new List<List<Vertex>>()
    Colors

let fullColouringApprox (graph: UndirectedGraph<Vertex, Edge>) = 
    // Supporting func
    let fulfilAchromatic (Colors: List<List<Vertex>>) = 
        let noColor = -1
        let findColor (vert: Vertex) = 
            let mutable toRet = noColor
            for color in Colors do
                if color.Contains vert then
                    toRet <- Colors.IndexOf color
                else
                    ()
            toRet
        
        let addToColorSet (vert: Vertex) = 
            let neighbours = graph.AdjacentEdges vert
            let selected = new List<int>()
            for item in neighbours do
                let target = item.Target
                let x = findColor target
                if x <> noColor then
                    if not <| selected.Contains x then
                        selected.Add x
            if selected.Count = Colors.Count then
                let n = new List<Vertex>()
                n.Add vert
                Colors.Add n
            else
                let mutable toAdd = -1
                for i in 1..Colors.Count do
                    if not <| selected.Contains i then
                        toAdd <- i
                Colors.[toAdd].Add vert
                        
        
        for vert in graph.Vertices do
            if findColor vert = noColor then
                

    let checkForFull (Colors: List<List<Vertex>>) = 
        true

    let algo = new AchromaticPartition<Vertex, Edge>()
    let res = algo.Execute graph

