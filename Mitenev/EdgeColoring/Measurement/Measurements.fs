open System
open System.Windows.Forms
open FSharp.Charting
open QuickGraph

open EdgeColoring
open GraphBuilder
 
let check (graph : UndirectedGraph<int, TaggedEdge<int,int>>) =
    let mutable error = 0
    for v in graph.Vertices do
        let mutable listExistsColor = []
        for e in graph.AdjacentEdges(v) do
            if List.exists (fun c -> c = e. Tag) listExistsColor
            then error <- 1000
            else listExistsColor <- e.Tag :: listExistsColor
    error

let rand count = 
    let mutable values = []
    let vertCount = 30
    for i in [1..count] do
        let rnd = new Random()
        let g = buildRandomGraph <| vertCount
        printfn "%s %d" "vert" <| g.VertexCount
        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        edgeColoring g |> ignore
        stopWatch.Stop()
        printfn "%s %s" "time" <| stopWatch.ElapsedMilliseconds.ToString()
        values <- stopWatch.ElapsedMilliseconds :: values
    let arrvalues = List.toArray values
    Chart.Line([ for x in 0 .. count-2 -> x, arrvalues.[x]])

let randSeq count = 
    let mutable values = []
    let start = 6
    for i in [start..count] do
        let rnd = new Random()
        let g = buildRandomGraph <| i
        printfn "%s %d" "vert" <| g.VertexCount
        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        edgeColoring g |> ignore
        stopWatch.Stop()
        printfn "%s %s" "time" <| stopWatch.ElapsedMilliseconds.ToString()
        values <- stopWatch.ElapsedTicks :: values
    let arrvalues = List.toArray values
    Array.Reverse arrvalues
    Chart.Combine [ Chart.Line([ for x in 0 .. count-start -> x, x*x])
                    Chart.Line([ for x in 0 .. count-start -> x, arrvalues.[x]])]

let isGoodGraph count = 
    let mutable values = []
    for i in [1..count] do
        let rnd = new Random()
        let g = buildRandomGraph <| rnd.Next(6,50)
        let isError = edgeColoring g |> check
        values <- isError :: values
    let arrvalues = List.toArray values
    Chart.Line([ for x in 0 .. count-1 -> x, arrvalues.[x]])

[<EntryPoint>]
[<STAThread>]
let main argv = 
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault false
    Application.Run (rand(100).ShowChart())
    //Application.Run (randSeq(100).ShowChart())
    //Application.Run (isGoodGraph(1000).ShowChart())
    0

