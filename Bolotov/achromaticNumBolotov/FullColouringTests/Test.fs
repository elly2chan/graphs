namespace AchromaticNum.FullColouringTests

open NUnit.Framework
open FsUnit
open QuickGraph
open AchromaticNum.Algorithms

type Vertex = int
type Edge = SEdge<Vertex>

module Helper = 
    let complete (count: int) = 
        let graph = new UndirectedGraph<Vertex, Edge>(false)
        for i in 1..count do
            graph.AddVertex i |> ignore
            for item in graph.Vertices do
                if item = i then 
                    ()
                else
                    let x = new Edge(i, item)
                    graph.AddEdge x |> ignore
        graph

[<TestFixture>]
type ``Main Tests``() = 
    [<Test>]
    member this.``Complete graphs`` () = 
        Assert.Equals(7, (fullColouringApprox <| Helper.complete 7).Count)
