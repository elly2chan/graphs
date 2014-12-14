module DotParser

open QuickGraph
open QuickGraph.Graphviz
open Graphviz4Net.Dot.AntlrParser
open Graphviz4Net.Dot
open System.IO

type FileDotEngine() =
    interface IDotEngine with
        member this.Run(imageType : Dot.GraphvizImageType, dot : string, outputFileName : string) =
            let output = outputFileName + ".dot";
            File.WriteAllText(output, dot)
            output

let readGraph filePath =
    let loadGraphFromDOT path = 
        let parser = AntlrParserAdapter<string>.GetParser()
        new StreamReader(File.OpenRead filePath)  
        |>parser.Parse

    let loadDotToQG gFile =
        let g = loadGraphFromDOT gFile  
        let qGraph = new UndirectedGraph<int, TaggedEdge<_,int>>()
        g.Edges 
        |> Seq.iter(
            fun e ->
                let edg = e :?> DotEdge<string>            
                qGraph.AddVerticesAndEdge(new TaggedEdge<_,_>(int edg.Source.Id,int edg.Destination.Id,0)) |> ignore)
        qGraph       
    loadDotToQG filePath

let writeToFile path (graph : UndirectedGraph<int, TaggedEdge<int,int>>) =
    let graphviz = new GraphvizAlgorithm<int,TaggedEdge<int,int>>(graph)
    graphviz.FormatEdge.Add(fun e -> e.EdgeFormatter.Comment <- e.Edge.Tag.ToString())
    graphviz.Generate(new FileDotEngine(), "answer")
