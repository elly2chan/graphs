module AchromaticNum

open AchromaticNum.AchromaticPartition
open QuickGraph

type Vertex = int
type Edge = SEdge<int>

let fullColouringApprox (graph: UndirectedGraph<Vertex>) = 
    let algo = new AchromaticPartition<Vertex, Edge>()
    algo.Execute graph

