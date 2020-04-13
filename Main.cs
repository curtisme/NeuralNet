using System;
using System.IO;
using KurtsNeuralNetworkz;
using KurtsNeuralNetworkz.Parsing;

public class NeuralNetworkTest
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Please provide filename for initialisation.");
            return;
        }

        FeedForwardBoi network;
        string s;
        try
        {
            network = InitParser.Parse(new StreamReader(args[0]));
            Console.WriteLine(network);
            while ((s = Console.ReadLine()) != null)
            {
                try
                {
                    PrintDoubleArray(network.Evaluate(CommandParser.Parse(s)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static void PrintDoubleArray(double[] D)
    {
        foreach(double d in D)
            Console.Write($"{d} ");
        Console.WriteLine();
    }
}
