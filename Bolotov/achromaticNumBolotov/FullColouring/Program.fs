open QuickGraph
open Graphviz4Net.Dot.AntlrParser
open Graphviz4Net.Dot
open AchromaticNum.Algorithms
open System
open System.IO
open System.Collections.Generic

module ReadDot =     
    let loadGraphFromDOT filePath = 
        let parser = AntlrParserAdapter<string>.GetParser()
        new StreamReader(File.OpenRead filePath)
        |> parser.Parse
        
    let loadDotToQG gFile =
        let g = loadGraphFromDOT gFile
        let qGraph = new UndirectedGraph<int, SEdge<int>>()
        g.Edges 
        |> Seq.iter(
            fun e ->
                let edg = e :?> DotEdge<string>            
                qGraph.AddVerticesAndEdge(new SEdge<_>(int edg.Source.Id,int edg.Destination.Id)) |> ignore)
        qGraph

module Helper = 
    let noColor = -1
    let findColor (colors: List<List<Vertex>>) (vert: Vertex) = 
        let mutable toRet = noColor
        for i in 0..(colors.Count - 1) do
            if colors.[i].Contains vert then
                toRet <- i
        toRet

    /// Rate is given in percents
    let random (count: int) rate =
        let rand = new Random() 
        let graph = new UndirectedGraph<Vertex, Edge>(false)
        for i in 1..count do
            graph.AddVertex i |> ignore
            for item in graph.Vertices do
                if item = i then 
                    ()
                elif rate > rand.Next(100) then
                    let x = new Edge(i, item)
                    graph.AddEdge x |> ignore
        graph

    let mapColor color = 
        match color with
        | 1 -> QuickGraph.Color

    let complete count = random count 101

[<EntryPoint>]
let main argv = 
    let mutable name = "test.dot"
    for i in 0..argv.Length do
        if argv.[i] = "-n" then
            name <- argv.[i + 1]
    let x = ReadDot.loadDotToQG name
    let res = fullColouringApprox x
    let gviz = new Graphviz.GraphvizAlgorithm<int, SEdge<int>>(x)
    gviz.FormatVertex.Add (fun x -> x.VertexFormatter.Group <- string <| Helper.findColor res x.Vertex)
    printf "%A" gviz.Output
    0 // возвращение целочисленного кода выхода
