module AchromaticNum.Algorithm.AchromaticPartition

open QuickGraph
open System.Collections.Generic
open System


(*
type Graph<''Vertex, ''Edge when ''Edge :> I'Edge<''Vertex> and ''Vertex : equality> () =
    inherit IUndirectedGraph<''Vertex, ''Edge>()
    
    type 

    let colors = new Dictionary<''Vertex, int>()
    let 

    member this.Colors = colors
*)        

type AchromaticPartition<'Vertex, 'Edge when 'Edge :> IEdge<'Vertex> and 'Vertex: equality>() = 
(*
    let Passive = new List<'Vertex>()
    let Active = new List<'Vertex>()
*)
    let Colors = new List<List<'Vertex>>()
(*    
    let ColorPassive = new List<'Vertex>()
    let ColorActive = new List<'Vertex>()

    let PassivePassive = new List<'Vertex>()
    let PassiveActive = new List<'Vertex>()
    let ActivePassive = new List<'Vertex>()
    let ActiveActive = new List<'Vertex>()

    let PassiveIgnored = new List<'Vertex>()

    let PassiveRecursive = new List<'Vertex>()
    let ActiveRecursive = new List<'Vertex>()

    let mutable a' = 0
    let mutable d' = 0.0
    let mutable e' = 0.0
    *)
    let firstIteration 
        (graph: IUndirectedGraph<'Vertex, 'Edge>)
        (passive: List<'Vertex>)
        (active: List<'Vertex>) = 
        let d' = log(float graph.VertexCount) / log(2.0)
        let a' = int d'
        let e' = 1.0 / (sqrt (log(float graph.VertexCount)))
        let color = 0

        Colors.Add <| new List<'Vertex>()
        for item in graph.Vertices do
            passive.Add item

        for item in graph.Vertices do
            if passive.Contains item then
                if graph.AdjacentDegree item > a' then
                    Colors.[color].Add item
                    passive.Remove item |> ignore
                    for edge in graph.AdjacentEdges item do
                        let tmp = if edge.Target = item then edge.Source else edge.Target
                        active.Add tmp
                        passive.Remove tmp |> ignore
        
    /// Iteration: graph, P, A, d', a'
    let rec iteration
        (graph: IUndirectedGraph<'Vertex, 'Edge>) 
        (passiveSet: List<'Vertex>)
        (activeSet: List<'Vertex>)
        (d': float)
        (a': int) = 
        // Constants
        let e' = 1.0 / (sqrt (log(float graph.VertexCount)))
        // Supportive funcs
        let coversSet vert (set: List<'Vertex>) =
            let x = new List<'Vertex>()
            let neighbours = (graph.AdjacentEdges vert)
            for edge in neighbours do
                let item = if edge.Target = vert then edge.Source else edge.Target
                if set.Contains item && not <| x.Contains item then
                    x.Add item
            x.Count

        /// Step 1: graph, P, A, Pp, Ap, Pi
        let stepOne (graph: IUndirectedGraph<'Vertex, 'Edge>)
            (passiveSet: List<'Vertex>)
            (active: List<'Vertex>)
            (passivePassive: List<'Vertex>)
            (activePassive: List<'Vertex>)
            (passiveIgnored: List<'Vertex>) =

            for index in (passiveSet.Count - 1) .. -1 .. 0 do
                let vert = passiveSet.[index]
                let tmp = coversSet vert active
                if tmp > int a' && tmp < active.Count then
                    let neighbours = graph.AdjacentEdges vert
                    for edge in neighbours do
                        let item = if edge.Target = vert then edge.Source else edge.Target
                        if active.Contains item then
                            activePassive.Add item
                            active.Remove item |> ignore
                        elif passiveSet.Contains item then
                            passivePassive.Add item
                            passiveSet.Remove vert |> ignore


            for index in (passiveSet.Count - 1) .. -1 .. 0 do
                let vert = passiveSet.[index]
                let tmp = coversSet vert active
                if tmp = active.Count then
                    passiveIgnored.Add vert
                    passiveSet.Remove vert |> ignore

        /// Step 2: graph, P, A, Aa, Pa, Ca
        let stepTwo
            (graph: IUndirectedGraph<'Vertex, 'Edge>)
            (passive: List<'Vertex>)
            (active: List<'Vertex>)
            (activeActive: List<'Vertex>)
            (passiveActive: List<'Vertex>)
            (colorActive: List<'Vertex>) =

            for index in (active.Count - 1) .. -1 .. 0 do
                let vert = active.[index]
                let tmp = coversSet vert active
                if tmp > int a' then
                    colorActive.Add vert
                    active.Remove vert |> ignore
                    let neighbours = graph.AdjacentEdges vert
                    for edge in neighbours do
                        let item = if edge.Target = vert then edge.Source else edge.Target
                        if active.Contains item then
                            activeActive.Add item
                            active.Remove item |> ignore
                        elif passive.Contains item then
                            passiveActive.Add item
                            passive.Remove item |> ignore

            if colorActive.Count = 0 then
                let vert = active.[0]
                colorActive.Add vert
                //  Duplicate code (copy-paste)
                let neighbours = graph.AdjacentEdges vert
                for edge in neighbours do
                    let item = if edge.Target = vert then edge.Source else edge.Target
                    if active.Contains item then
                        activeActive.Add item
                        active.Remove item |> ignore
                    elif passive.Contains item then
                        passiveActive.Add item
                        passive.Remove item |> ignore

        /// Step 3: graph, P, A, Cp, Ca, Ad, Pa, Pr
        let stepThree
            (graph: IUndirectedGraph<'Vertex, 'Edge>)
            (passive: List<'Vertex>)
            (active: List<'Vertex>)
            (colorPassive: List<'Vertex>)
            (colorActive: List<'Vertex>)
            (activeDiscard: List<'Vertex>)
            (passiveActive: List<'Vertex>)
            (passiveRecursive: List<'Vertex>) =

            //  PassiveRecursive is obsolete
            colorActive.AddRange colorPassive
            Colors.Add colorActive
            passiveRecursive.AddRange passive
            passive.AddRange passiveActive
            if active.Count <= int (floor (Math.Pow(float graph.VertexCount, 1.0 - e'))) then
                activeDiscard.AddRange active
                active.Clear()
            else 
                let tmp = int (floor ((float a') * Math.Pow(float graph.VertexCount, e')))

                for index in (active.Count - 1) .. -1 .. 0 do
                    let vert = active.[index]
                    if coversSet vert passive > tmp then
                        activeDiscard.Add vert
                        active.Remove vert |> ignore

        /// Step 4: graph, P, Ap, Aa, Pi, Ar
        let stepFour
            (graph: IUndirectedGraph<'Vertex, 'Edge>)
            (passive: List<'Vertex>)
            (activePassive: List<'Vertex>)
            (activeActive: List<'Vertex>)
            (passiveIgnored: List<'Vertex>)
            (activeRecursive: List<'Vertex>) = 
            if not(activePassive.Count = 0 && activeActive.Count = 0) then
                let a' = int (ceil (d' * Math.Pow((float graph.VertexCount), e')))
                let d' = 2.0 * d' * Math.Pow(float graph.VertexCount, 2.0 * e')
                passiveIgnored.AddRange passive
                activeRecursive.AddRange passive
                let p1 = passiveIgnored
                let p2 = activeRecursive
                activePassive.AddRange activeActive
                let ap2 = new List<'Vertex>()
                for item in activePassive do
                    ap2.Add item
                // TODO: Parallel
                let res1: List<List<'Vertex>> = iteration graph p1 activePassive d' a'
                let res2: List<List<'Vertex>> = iteration graph p2 ap2 d' a'
                if res1.Count > res2.Count then
                    Colors.AddRange res1
                else
                    Colors.AddRange res2

        do
            let passiveActive = new List<'Vertex>()
            let passivePassive = new List<'Vertex>()
            let activePassive = new List<'Vertex>()
            let activeActive = new List<'Vertex>()

            let passiveIgnored = new List<'Vertex>()

            let colorActive = new List<'Vertex>()
            let colorPassive = new List<'Vertex>()

            let activeDiscard = new List<'Vertex>()

            let passiveRecursive = new List<'Vertex>()

            stepOne graph passiveSet activeSet passivePassive activePassive passiveIgnored
            stepTwo graph passiveSet activeSet activeActive passiveActive colorActive
            stepThree graph passiveSet activeSet colorPassive colorActive activeDiscard passiveActive passiveRecursive
            stepFour graph passiveSet activePassive activeActive passiveIgnored activeSet
        Colors

    member this.Execute (graph: IUndirectedGraph<'Vertex, 'Edge>) = 
        let Passive = new List<'Vertex>()
        let Active = new List<'Vertex>()
        let d' = log(float graph.VertexCount) / log(2.0)
        let a' = int d'
        firstIteration graph Passive Active
        iteration graph Passive Active d' a'
