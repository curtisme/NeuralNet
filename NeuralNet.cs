using System;

namespace NeuralNet
{
	public class Function
	{
		private int n_in, n_out;
		private int hiddenPerLayer, hiddenLayers;

		public Function(int n_in, int n_out,
				int hiddenPerLayer, int hiddenLayers)
		{

		}

		private abstract class Node
		{
			public double Value {get; protected set;}
		}

		private class Edge
		{
			public int Weight {get;}
			public Node From{get;}

			public Edge(int w, Node f)
			{
				Weight = w;
				From = f;
			}
		}

		private abstract class NonInputNode: Node
		{
			protected Edge[] edges;
			public abstract void UpdateValue();
		}

		private class InputNode : Node {}

		private class HiddenNode: NonInputNode
		{
			public override void UpdateValue()
			{
			}
		}

		private class OutputNode: NonInputNode
		{
			public override void UpdateValue()
			{
			}
		}
	}
}
