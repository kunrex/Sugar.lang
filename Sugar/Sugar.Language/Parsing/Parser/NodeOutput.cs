using System;
using System.Collections;
using System.Collections.Generic;

using Sugar.Language.Parsing.Nodes;

namespace Sugar.Language.Parsing.Parser
{
    internal sealed class NodeOutput : IEnumerable<Node>
    {
        private readonly List<Node> stack;
        private readonly List<Node> output;

        public int OutputCount { get => output.Count; }

        public NodeOutput()
        {
            stack = new List<Node>();
            output = new List<Node>();
        }

        public Node this[int index] { get => output[index]; }

        public void PushStack(Node node) => stack.Add(node);

        public void ClearStack() => stack.Clear();
        public void ClearOutputStack()
        {
            foreach (var node in stack)
                output.Add(node);

            stack.Clear();
        }

        public void PushOutput(Node node) => output.Add(node);

        public void Clear()
        {
            stack.Clear();
            output.Clear();
        }

        public IEnumerator<Node> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
