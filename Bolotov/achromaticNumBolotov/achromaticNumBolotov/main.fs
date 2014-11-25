open AchromaticNum.AchromaticPartition
open System.Collections.Generic
open QuickGraph

[<EntryPoint>]
let main argv = 
    let x = QuickGraph.UndirectedGraph(false)
    let e = new List<SEdge<int>>()
    e.Add(SEdge(0, 1))
    x.AddVertex 0 |> ignore
    x.AddVertex 1 |> ignore
    x.AddEdge(e.Item(0)) |> ignore

    let algo = new AchromaticNum.AchromaticPartition.AchromaticPartition()
    let res = algo.execute(x)
    res
    0
