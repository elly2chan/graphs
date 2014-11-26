module LoadFromDot

// You need to install QuickGraph from NuGet and add Antlr3.Runtime and Graphviz4Net from 3rdParty (http://graphviz4net.codeplex.com/releases/view/106189).
// Note: Unfortunetly parser is very simple, so semicolons in DOT file are required (not optional) and list of all used vertices required also. See input\test.dot

open QuickGraph
open Graphviz4Net.Dot.AntlrParser
open Graphviz4Net.Dot
open System.IO

let loadGraphFromDOT filePath = 
    let parser = AntlrParserAdapter<string>.GetParser()
    new StreamReader(File.OpenRead filePath)
    |> parser.Parse

let loadDotToQG gFile =
    let g = loadGraphFromDOT gFile
    let qGraph = new AdjacencyGraph<int, TaggedEdge<_,string>>()
    g.Edges 
    |> Seq.iter(
        fun e ->
            let edg = e :?> DotEdge<string>            
            qGraph.AddVerticesAndEdge(new TaggedEdge<_,_>(int edg.Source.Id,int edg.Destination.Id,edg.Label)) |> ignore)
    qGraph

loadDotToQG @"..\..\..\input\test.dot" |> fun g -> printfn "Loaded: V = %A  E = %A" g.VertexCount g.EdgeCount

