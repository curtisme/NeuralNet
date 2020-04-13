using System;
using System.IO;
using System.Linq;
using KurtsNeuralNetworkz;

namespace KurtsNeuralNetworkz.Parsing
{
    public class InitParser
    {
        public static FeedForwardBoi Parse(TextReader tr)
        {
            string s;
            int numEgs, iterations;
            double lRate;
            int[] topology;
            double[][][] egs;
            char[] space = new char[] {' '};
            char[] comma = new char[] {','};
            FeedForwardBoi ffb;

            s = tr.ReadLine();
            try
            {
                topology = (from i in s.Split(space)
                        select int.Parse(i)).ToArray();
                ffb = new FeedForwardBoi(topology[0], topology[1],
                        topology[2], topology[3]);
            }
            catch (Exception)
            {
                throw new Exception("Unable to read topology.");
            }

            s = tr.ReadLine();
            if (!int.TryParse(s, out numEgs) ||
                    numEgs < 0)
                numEgs = 0;
            s = tr.ReadLine();
            if (!int.TryParse(s, out iterations) ||
                    iterations < 0)
                iterations = 10000;
            s = tr.ReadLine();
            if (!double.TryParse(s, out lRate) ||
                    lRate < 0)
                lRate = 0.1;

            egs = new double[numEgs][][];

            for (int i=0;i<numEgs;i++)
            {
                if ((s = tr.ReadLine()) == null)
                    throw new Exception($"Unable to read example {i}!"
                            + " Not enough lines.");
                try
                {
                    egs[i] = (from eg in s.Split(comma)
                            select
                            (from d in eg.Split(space)
                             select double.Parse(d)).ToArray()
                            ).ToArray();
                }
                catch (Exception)
                {
                    throw new Exception($"Unable to read example {i}!"
                            + $" Issue parsing: {s}.");
                }
            }
            ffb.TrainOnExamples(egs, iterations, lRate);
            return ffb;
        }
    }

    public class CommandParser
    {
        public static double[] Parse(string s)
        {
            double[] input;
            try
            {
                input = (from d in s.Split(new char[] {' '})
                         select double.Parse(d)).ToArray();
            }
            catch (Exception)
            {
                throw new Exception($"Unable to parse input: {s}.");
            }
            return input;
        }
    }
}
