﻿module AchromaticNum.Algorithms

open AchromaticNum.Algorithm.AchromaticPartition
open QuickGraph
open System.Collections.Generic

type Vertex = int
type Edge = SEdge<int>

module Helpers = 
    let noColor = -1
    let findColor (colors: List<List<Vertex>>) (vert: Vertex) = 
        let mutable toRet = noColor
        for i in 0..(colors.Count - 1) do
            if colors.[i].Contains vert then
                toRet <- i
        toRet

let BruteForcePrecise (graph: IUndirectedGraph<Vertex, Edge>) = 
    let Colors = new List<List<Vertex>>()    
    Colors

let fullColouringApprox (graph: IUndirectedGraph<Vertex, Edge>) = 
    // Supporting func
    let fulfilAchromatic (colors: List<List<Vertex>>) =         
        let addToColorSet (vert: Vertex) = 
            let neighbours = graph.AdjacentEdges vert
            let selected = new List<int>()
            for item in neighbours do
                let target = if vert = item.Target then item.Source else item.Target
                let x = Helpers.findColor colors target
                if x <> Helpers.noColor then
                    if not <| selected.Contains x then
                        selected.Add x
            if selected.Count = colors.Count then
                let n = new List<Vertex>()
                n.Add vert
                colors.Add n
            else
                let mutable toAdd = -1
                for i in 0..(colors.Count - 1) do
                    if not <| selected.Contains i then
                        toAdd <- i
                colors.[toAdd].Add vert
               
        for vert in graph.Vertices do
            let x = Helpers.findColor colors vert
            if x = Helpers.noColor then
                addToColorSet vert

    let algo = new AchromaticPartition<Vertex, Edge>()
    let res = algo.Execute graph
    fulfilAchromatic res
    res

/// -1 - broken colouring, 0 - not complete, 1 - complete.
let checkForCompleteness (graph: IUndirectedGraph<Vertex, Edge>) (colors: List<List<Vertex>>) = 
    let intersection = Array2D.create colors.Count colors.Count false
    let mutable toRet = 0

    for color in colors do
        let index = colors.IndexOf color
        for vertex in color do
            let neighbours = graph.AdjacentEdges vertex
            for i in neighbours do
                let item = if vertex = i.Target then i.Source else i.Target
                let x = Helpers.findColor colors item
                if x <> -1 then
                    intersection.[index, x] <- true
                    intersection.[x, index] <- true
                    if x = index then
                        toRet <- -1
    if toRet = -1 then
        toRet
    else
        toRet <- 1
        for i in 0..(colors.Count - 1) do
            for j in 0..(colors.Count - 1) do
                if not intersection.[i,j] && i<>j then
                    toRet <- 0
                else ()
        toRet
