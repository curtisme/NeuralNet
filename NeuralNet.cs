using System;
using System.Text;

//TODO: Sanity checks on input sizes
namespace KurtsNeuralNetworkz
{
    public class FeedForwardBoi
    {
        private class Node
        {
            public double inVal, outVal, tempSum, error;
            public Edge[] edges;

            public Node(Node[] parents)
            {
                if (parents != null)
                {
                    edges = new Edge[parents.Length];
                    for (int i=0;i<edges.Length;i++)
                        edges[i] = new Edge(parents[i], 0);
                }
                else
                    edges = null;
                inVal = outVal = tempSum = error = 0;
            }
        }

        private class Edge
        {
            public Node node;
            public double weight;

            public Edge(Node n, double w)
            {
                node = n;
                weight = w;
            }
        }

        private int ins, hiddens, nodesPerHidden, outs;
        private ActivationFunction f, df;
        private Node[][] layers;

        public FeedForwardBoi() : this(0,0,0,0) {}

        public FeedForwardBoi(int ins, int hiddens,
                int nodesPerHidden, int outs)
        {
            this.ins = ins;
            this.hiddens = hiddens;
            this.nodesPerHidden = nodesPerHidden;
            this.outs = outs;
            this.f = x => 1/(1 + Math.Exp(-1*x));
            this.df = x => f(x)*(1 - f(x));
            layers = new Node[hiddens + 2][];
            for (int i=0;i<layers.Length;i++)
            {
                int len = i==0 ?
                    ins :
                    (i+1==layers.Length ? 
                     outs :
                     nodesPerHidden);
                layers[i] = new Node[len];
                for (int j=0;j<len;j++)
                    layers[i][j] = new Node(i>0?layers[i-1]:null);
            }
            Initialise();
        }

        public void Initialise()
        {
            Random rand = new Random();
            for (int i=1;i<layers.Length;i++)
            {
                foreach(Node node in layers[i])
                    for(int j=0;j<node.edges.Length;j++)
                        node.edges[j].weight = rand.NextDouble();
            }
        }

        public double[] Evaluate(double[] input)
        {
            double[] output = new double[outs];
            eval(input);
            for (int i=0;i<outs;i++)
                output[i] = layers[layers.Length-1][i].outVal;
            return output;
        }

        public void TrainOnExamples(double[][][] egs, int iterations,
                double lRate)
        {
            for (int its=0;its<iterations;its++)
            {
                foreach(double[][] eg in egs)
                {
                    eval(eg[0]);
                    Node[] layer = layers[layers.Length-1];
                    for (int i=0;i<layer.Length;i++)
                    {
                        layer[i].error = df(layer[i].inVal)*(eg[1][i] - layer[i].outVal);
                        for (int j=0;j<layer[i].edges.Length;j++)
                        {
                            layer[i].edges[j].node.tempSum +=
                                layer[i].edges[j].weight*layer[i].error;
                        }
                    }
                    for (int i=layers.Length-2;i>0;i--)
                    {
                        layer = layers[i];
                        for (int j=0;j<layer.Length;j++)
                        {
                            layer[j].error = df(layer[j].inVal)*layer[j].tempSum;
                            for (int k=0;k<layer[j].edges.Length;k++)
                            {
                                layer[j].edges[k].node.tempSum +=
                                    layer[j].edges[k].weight*layer[j].error;
                            }
                        }
                    }
                    for (int i=1;i<layers.Length;i++)
                    {
                        for (int j=0;j<layers[i].Length;j++)
                        {
                            for (int k=0;k<layers[i][j].edges.Length;k++)
                            {
                                layers[i][j].edges[k].weight +=
                                    lRate
                                    *layers[i][j].edges[k].node.outVal
                                    *layers[i][j].error;
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{ins} input nodes.\n");
            sb.Append($"{hiddens} hidden layers, {nodesPerHidden} nodes per layer.\n");
            for (int i=1;i<layers.Length-1;i++)
            {
                sb.Append($"Hidden layer {i}:\n");
                PrintNodeArray(layers[i], sb);
            }
            sb.Append($"{outs} output nodes.\n");
            PrintNodeArray(layers[layers.Length-1], sb);
            return sb.ToString();
        }

        private void eval(double[] input)
        {
            for (int i=0;i<layers[0].Length;i++)
                layers[0][i].outVal = input[i];
            for (int i=1;i<layers.Length;i++)
            {
                for(int j=0;j<layers[i].Length;j++)
                {
                    layers[i][j].inVal = 0;
                    layers[i][j].tempSum = 0;
                    foreach(Edge e in layers[i][j].edges)
                        layers[i][j].inVal += e.node.outVal*e.weight;
                    layers[i][j].outVal = f(layers[i][j].inVal);
                }
            }
        }

        private void PrintNodeArray(Node[] nodes, StringBuilder sb)
        {
            if (sb != null)
            {
                foreach(Node node in nodes)
                {
                    foreach(Edge edge in node.edges)
                        sb.Append($"{edge.weight} ");
                    sb.Append('\n');
                }
            }
        }
    }

    public delegate double ActivationFunction(double x);
}
