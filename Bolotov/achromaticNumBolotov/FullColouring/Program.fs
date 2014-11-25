open QuickGraph
open AchromaticNum.Algorithms

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

[<EntryPoint>]
let main argv = 
    let x = Helper.complete 7
    let y = fullColouringApprox x
    let z = y.Count
    printf "%A" x
    0 // возвращение целочисленного кода выхода
