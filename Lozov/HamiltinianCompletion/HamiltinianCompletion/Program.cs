using System;
using System.Collections.Generic;
using System.IO;
using QuickGraph;
using HamiltinianCompletion;


namespace Program
{
    class Program
    {

        static string help = "help:\r\nHamiltoniamCompletion.exe (BruteForceSearch | BranchAndBound | GreedyAlgorithm | Genetic) [INPUT path] [OUTPUT path]";

        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                int algNumber;

                switch (args[0])
                {
                    case "BruteForceSearch":
                        algNumber = 0;
                        break;
                    case "BranchAndBound":
                        algNumber = 1;
                        break;
                    case "GreedyAlgorithm":
                        algNumber = 2;
                        break;
                    case "Genetic":
                        algNumber = 3;
                        break;
                    default:
                        Console.WriteLine(help);
                        return;
                }

                try
                {
                    var g = Graph<string>.ReadInDotFile(args[1]);

                    var res = algNumber == 0 ? g.BruteForceSearchAlgorithm()
                            : algNumber == 1 ? g.BranchAndBoundAlgorithm()
                            : algNumber == 2 ? g.SquareGreedyAlgorithm()
                            : g.GeneticAlgorithm();

                    g.AddCicle(res.Cicle);
                    g.WriteToDotFile(args[2]);

                    var stream = new StreamWriter(string.Concat(args[2], ".log"), false);
                    stream.Write(res.ToString());
                    stream.Close();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File with graph not found");
                    return;
                }
                catch
                {
                    Console.WriteLine("Incorrect graph in file");
                    return;
                }
            }
            else Console.WriteLine(help);
        }
    }
}
