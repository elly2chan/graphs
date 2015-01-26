open System.IO
open GraphLibrary
open QuickGraph
open NUnit.Framework
open FsUnit
open Graphviz4Net.Dot.AntlrParser
open Graphviz4Net.Dot

[<TestFixture>]
type TestClass() = 

    static member testRobson(v, e, num) =
        let result = Algorithm.Robson(v, e)
        Seq.length result = num

    static member testRobson(filename, num) = 
        let loadGraphFromDOT filePath = 
            let parser = AntlrParserAdapter<string>.GetParser()
            new StreamReader(File.OpenRead filePath)
            |> parser.Parse
        let loadDotToQG gFile =
            let g = loadGraphFromDOT gFile
            let qGraph = new UndirectedGraph<int, Edge<int>>()
            g.Edges 
            |> Seq.iter(
                fun e ->
                    let edg = e :?> DotEdge<string>          
                    qGraph.AddVerticesAndEdge(new Edge<int>(int edg.Source.Id,int edg.Destination.Id)) |> ignore)
            qGraph

        let qg = loadDotToQG filename
        let result = Algorithm.Robson(qg)
        Seq.length result = num 

    [<Test>] member test.
     ``Баг 1`` ()=
        TestClass.testRobson(@"../../dot-tests/bag-1.dot", 10) |> should be True

    [<Test>] member test.
     ``Баг 2`` ()=
        TestClass.testRobson(@"../../dot-tests/bag-2.dot", 13) |> should be True

    [<Test>] member test.
     ``2 вершины 1 ребро из файла`` ()=
        TestClass.testRobson(@"../../dot-tests/test-1.dot", 1) |> should be True
    
    [<Test>] member test.
     ``22 вершины`` ()=
        TestClass.testRobson(@"../../dot-tests/test-2.dot", 10) |> should be True

    [<Test>] member test.
     ``28 вершин`` ()=
        TestClass.testRobson(@"../../dot-tests/test-3.dot", 12) |> should be True

    [<Test>] member test.
     ``1 вершина`` ()=
        TestClass.testRobson([0],[],1) |> should be True
    
    [<Test>] member test.
     ``2 вершины, 1 ребро`` ()=
        TestClass.testRobson([0;1],[new Edge<int>(0,1)],1) |> should be True
    
    [<Test>] member test.
     ``3 вершины, 2 ребра`` ()=
        TestClass.testRobson([0;1;2],[new Edge<int>(0,1);new Edge<int>(2,1)],2) |> should be True

    [<Test>] member test.
     ``5 вершин, 4 ребра`` ()=
        TestClass.testRobson([0;1;2;3;4],[new Edge<int>(0,1);new Edge<int>(2,1);new Edge<int>(2,3);new Edge<int>(3,4)],3) |> should be True
    
    [<Test>] member test.
     ``Двудольный граф`` ()=
        TestClass.testRobson([0;1;2;3;4;5],[new Edge<int>(0,3);new Edge<int>(0,4);new Edge<int>(0,5);new Edge<int>(1,3);new Edge<int>(1,4);new Edge<int>(1,5);new Edge<int>(2,3);new Edge<int>(2,4);new Edge<int>(2,5)],3) |> should be True
    
     [<Test>] member test.
     ``Граф Питерсена`` ()=
        TestClass.testRobson([0;1;2;3;4;5;6;7;8;9],[new Edge<int>(0,1);new Edge<int>(0,5);new Edge<int>(0,4);new Edge<int>(1,2);new Edge<int>(1,6);new Edge<int>(2,7);new Edge<int>(2,3);new Edge<int>(3,4);new Edge<int>(3,8);new Edge<int>(5,7);new Edge<int>(5,8);new Edge<int>(6,8);new Edge<int>(6,9);new Edge<int>(7,9)], 4) |> should be True

    [<Test>] member test.
     ``7 вершин по кругу и одна в центре`` ()=
        TestClass.testRobson([0;1;2;3;4;5;6;7],[new Edge<int>(0,1);new Edge<int>(2,1);new Edge<int>(2,3);new Edge<int>(3,4);new Edge<int>(4,5);new Edge<int>(5,6);new Edge<int>(0,6);new Edge<int>(7,1);new Edge<int>(7,2);new Edge<int>(7,0);new Edge<int>(7,3);new Edge<int>(7,4);new Edge<int>(7,5);new Edge<int>(7,6)],3) |> should be True
    
    [<Test>] member test.
     ``Произвольный граф 12 вершин`` ()=
        TestClass.testRobson([0;1;2;3;4;5;6;7;8;9;10],[new Edge<int>(0,1);new Edge<int>(1,2);new Edge<int>(2,3);new Edge<int>(3,4);new Edge<int>(4,5);new Edge<int>(5,3);new Edge<int>(5,6);new Edge<int>(6,2);new Edge<int>(6,7);new Edge<int>(7,8);new Edge<int>(8,1);new Edge<int>(0,8);new Edge<int>(7,11);new Edge<int>(9,11);new Edge<int>(10,11);new Edge<int>(10,4);new Edge<int>(10,9);new Edge<int>(0,9)],5) |> should be True

     [<Test>] member test.
     ``Произвольный граф 16 вершин`` ()=
        TestClass.testRobson([0;1;2;3;4;5;6;7;8;9;10;11;12;13;14;15],[new Edge<int>(0,8);new Edge<int>(1,4);new Edge<int>(0,4);new Edge<int>(8,4);new Edge<int>(11,9);new Edge<int>(4,6);new Edge<int>(1,6);new Edge<int>(6,2);new Edge<int>(3,2);new Edge<int>(5,3);new Edge<int>(5,6);new Edge<int>(5,7);new Edge<int>(7,3);new Edge<int>(7,9);new Edge<int>(10,6);new Edge<int>(10,9);new Edge<int>(13,5);new Edge<int>(13,15);new Edge<int>(15,12);new Edge<int>(12,3);new Edge<int>(9,12);new Edge<int>(10,12);new Edge<int>(11,10);new Edge<int>(14,11);new Edge<int>(12,14)],7) |> should be True
