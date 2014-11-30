open QuickGraph
open AchromaticNum.Algorithms
open System

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

[<EntryPoint>]
let main argv = 
(*
    for i in 10..10..70 do
        let x = Helper.random 100 i
        let y = fullColouringApprox x
        let z = y.Count
        printf "%A\n" z
        *)
    let x = Helper.random 10 30
    let y = fullColouringApprox x
    let z = y.Count
    let b = checkForCompleteness x y
    printf "%A\n%A\n" z b
    0 // возвращение целочисленного кода выхода
