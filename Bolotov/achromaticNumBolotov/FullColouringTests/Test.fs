namespace AchromaticNum.FullColouringTests

open NUnit.Framework
open FsUnit
open QuickGraph
open AchromaticNum.Algorithms
open System

type Vertex = int
type Edge = SEdge<Vertex>

module Helper = 
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

    let complete count = random count 101

[<TestFixture>]
type ``Main Tests``() = 
    [<Test>]
    member this.``Complete graphs`` () = 
        for i in 1..10 do
            Assert.Equals(i, (fullColouringApprox <| Helper.complete i).Count) |> ignore

    member this.``Empty graph`` () = 
        let g = new UndirectedGraph<Vertex, Edge>()
        g.AddVertex 1 |> ignore
        Assert.Equals(0, fullColouringApprox g)

[<TestFixture>]
type ``Random 100 graphs`` () = 
    let count = 100
    [<Test>]
    member this.``10% edges`` () = 
        let graph = Helper.random count 10
        Assert.Equals(1, checkForCompleteness graph (fullColouringApprox graph))

    [<Test>]
    member this.``20% edges`` () = 
        let graph = Helper.random count 20
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)

    [<Test>]
    member this.``30% edges`` () = 
        let graph = Helper.random count 30
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)

    [<Test>]
    member this.``40% edges`` () = 
        let graph = Helper.random count 40
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)
        
    [<Test>]
    member this.``50% edges`` () = 
        let graph = Helper.random count 50
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)


    [<Test>]
    member this.``70% edges`` () = 
        let graph = Helper.random count 70
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)

    [<Test>]
    member this.``90% edges`` () = 
        let graph = Helper.random count 90
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)
        
[<TestFixture>]
type ``Random 1000 graphs`` () = 
    let count = 1000
    [<Test>]
    member this.``10% edges`` () = 
        let graph = Helper.random count 10
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)
        
    [<Test>]
    member this.``20% edges`` () = 
        let graph = Helper.random count 20
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)

    [<Test>]
    member this.``30% edges`` () = 
        let graph = Helper.random count 30
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)
        
    [<Test>]
    member this.``40% edges`` () = 
        let graph = Helper.random count 40
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)

    [<Test>]
    member this.``50% edges`` () = 
        let graph = Helper.random count 50
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)

    [<Test>]
    member this.``70% edges`` () = 
        let graph = Helper.random count 70
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)
        
    [<Test>]
    member this.``90% edges`` () = 
        let graph = Helper.random count 90
        Assert.Equals(1, checkForCompleteness graph <| fullColouringApprox graph)
